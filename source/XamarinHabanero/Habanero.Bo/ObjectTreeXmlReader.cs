#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// This class reads an xml stream and loads the objects in it into an IEnumerable[IBusinessObject].
    /// It will update existing objects. For example, if you are deserialising an object from an xml stream
    /// that exists in your data store, it will load the object from the data store and update its properties
    /// from the xml stream. It won't persist after this.  If the object does not exist in the data store then
    /// it will instantiate the object and set up its properties as a new object, ready to persist.
    /// </summary>
    public class ObjectTreeXmlReader
    {
        /// <summary>
        /// Contains info about the read result.
        /// </summary>
        public Result ReadResult { get; private set; }

        /// <summary>
        /// Reads from a stream that contains xml text. Creates the XmlReader that reads the stream and then uses
        /// a default <see cref="IBusinessObjectXmlReader"/> to create the objects from the xml.
        /// Any errors that occur when setting properties on objects will be added to the ReadResult property.
        /// Once this method returns, check the Successful flag of the ReadResult to see if there are errors, and
        /// check the Message flag to see what the errors were.
        /// </summary>
        /// <param name="stream">The stream to read from. This stream must contain xml text</param>
        /// <returns>A <see cref="DataStoreInMemory"/> containing all objects that were in the xml</returns>
        public IEnumerable<IBusinessObject> Read(Stream stream)
        {
            var xmlReader = XmlReader.Create(stream, GetSettings());
            return Read(xmlReader);
        }

        /// <summary>
        /// Reads from a string that contains xml text. Creates the XmlReader that reads the string and then uses
        /// a default <see cref="IBusinessObjectXmlReader"/> to create the objects from the xml.
        /// Any errors that occur when setting properties on objects will be added to the ReadResult property.
        /// Once this method returns, check the Successful flag of the ReadResult to see if there are errors, and
        /// check the Message flag to see what the errors were.
        /// </summary>
        /// <param name="xml">The xml to read</param>
        /// <returns>A <see cref="DataStoreInMemory"/> containing all objects that were in the xml</returns>
        public IEnumerable<IBusinessObject> Read(string xml)
        {
            var reader = XmlReader.Create(new StringReader(xml), GetSettings());
            return Read(reader);
        }

        /// <summary>
        /// Reads from an XmlReader. Uses a default <see cref="IBusinessObjectXmlReader"/> to 
        /// create the objects from the xml.
        /// Any errors that occur when setting properties on objects will be added to the ReadResult property.
        /// Once this method returns, check the Successful flag of the ReadResult to see if there are errors, and
        /// check the Message flag to see what the errors were.
        /// </summary>
        /// <param name="xmlReader">The reader to use</param>
        /// <returns>A <see cref="DataStoreInMemory"/> containing all objects that were in the xml</returns>
        public IEnumerable<IBusinessObject> Read(XmlReader xmlReader)
        {
            var boReader = new BusinessObjectXmlReader();
            return Read(xmlReader, boReader);
        }

        /// <summary>
        /// Reads from a stream. Uses the given <see cref="IBusinessObjectXmlReader"/> to 
        /// create the objects from the xml.
        /// Any errors that occur when setting properties on objects will be added to the ReadResult property.
        /// Once this method returns, check the Successful flag of the ReadResult to see if there are errors, and
        /// check the Message flag to see what the errors were.
        /// </summary>
        /// <param name="stream">The stream to read the xml from</param>
        /// <param name="boReader">The <see cref="IBusinessObjectXmlReader"/> to use. This object
        /// converts the xml data into business objects, so you can control how the objects are deserialised by
        /// creating your own.</param>
        /// <returns>A <see cref="DataStoreInMemory"/> containing all objects that were in the xml</returns>
        public IEnumerable<IBusinessObject> Read(Stream stream, IBusinessObjectXmlReader boReader)
        {
            var reader = XmlReader.Create(stream, GetSettings());
            return Read(reader, boReader);
        }

        /// <summary>
        /// Reads from a stream. Uses the given <see cref="IBusinessObjectXmlReader"/> to 
        /// create the objects from the xml.
        /// Any errors that occur when setting properties on objects will be added to the ReadResult property.
        /// Once this method returns, check the Successful flag of the ReadResult to see if there are errors, and
        /// check the Message flag to see what the errors were.
        /// </summary>
        /// <param name="xmlReader">The xml reader to use</param>
        /// <param name="boReader">The <see cref="IBusinessObjectXmlReader"/> to use. This object
        /// converts the xml data into business objects, so you can control how the objects are deserialised by
        /// creating your own.</param>
        /// <returns>A <see cref="DataStoreInMemory"/> containing all objects that were in the xml</returns>
        public IEnumerable<IBusinessObject> Read(XmlReader xmlReader, IBusinessObjectXmlReader boReader)
        {
            BOSequenceNumber.LoadNumberGenClassDef();
            var objects = new List<IBusinessObject>();
            var bos = boReader.Read(xmlReader);
            bos.ForEach(bo => objects.Add(ConfigureObjectAfterLoad(bo)));

            ReadResult = boReader.PropertyReadExceptions.Count() == 0 
                 ? new Result(true) 
                 : new Result(false, BuildExceptionMessage(boReader.PropertyReadExceptions));
            return objects;
        }

        /// <summary>
        /// Sets up the object after loading. In this case:
        /// 1. Tries to load the object from the data store. If it exists:
        /// --a. Updates the existing object's properties
        /// --b. Returns the existing object
        /// 2. Otherwise:
        /// --a. Calls the <see cref="BusinessObject.AfterLoad()"/> method on the object
        /// --b. Backs up the property values, setting the persisted values of the properties
        /// </summary>
        /// <param name="bo">The object to configure</param>
        /// <returns>The business object to add. In the case of an existing object the object returned 
        /// will be different from the one passed in</returns>
        protected virtual IBusinessObject ConfigureObjectAfterLoad(IBusinessObject bo)
        {
            BusinessObjectManager.Instance.Remove(bo);
            var existingBo = TryGetUpdatedExistingBo(bo);
            if (existingBo != null) return existingBo; 
            
            ConfigureNewBo(bo);
            return bo;
        }

        private IBusinessObject TryGetUpdatedExistingBo(IBusinessObject bo)
        {
            var existingBo = GetExistingBo(bo.ClassDef, bo);
            if (existingBo == null) return null;
            UpdateExistingBo(bo, existingBo);
            return existingBo;
        }

        private static void ConfigureNewBo(IBusinessObject bo)
        {
            BusinessObjectLoaderBase.CallAfterLoad(bo);
            try
            {
                BusinessObjectManager.Instance.Add(bo);
            }
            catch (HabaneroDeveloperException ex)
            {
                // object already exists - this is a possible circumstance so we can let it go
                if (!ex.DeveloperMessage.Contains("Two copies of the business object"))
                    throw;
            }
            bo.Props.BackupPropertyValues();
        }

        private void UpdateExistingBo(IBusinessObject bo, IBusinessObject existingBo)
        {
            foreach (var prop in existingBo.Props)
            {
                existingBo.SetPropertyValue(prop.PropertyName, bo.GetPropertyValue(prop.PropertyName));
            }
        }

        private IBusinessObject GetExistingBo(IClassDef classDef, IBusinessObject bo)
        {
            if (BORegistry.DataAccessor != null)
            {
                try
                {
                    return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, bo.ID);
                } catch (BusObjDeleteConcurrencyControlException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the default XmlReaderSettings used. Override this to change them.
        /// </summary>
        /// <returns></returns>
        protected virtual XmlReaderSettings GetSettings()
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            return settings;
        }

        private static string BuildExceptionMessage(IEnumerable<string> propertyReadExceptions)
        {
            const string crlf = @"\r\n";
            var exceptionMessage = new StringBuilder("");
            foreach (var propertyReadException in propertyReadExceptions)
            {
                exceptionMessage.Append(propertyReadException);
                exceptionMessage.Append(crlf);
            }
            return exceptionMessage.ToString();
        }
     
    }
}