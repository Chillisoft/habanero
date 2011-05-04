// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// This class reads an xml stream and loads the objects in it into a <see cref="DataStoreInMemory"/>
    /// The class assumes you are loading afresh.  Before doing the load, we recommend that you clear your
    /// <see cref="BusinessObjectManager"/> too so that you only have one instance of each object.
    /// </summary>
    public class DataStoreInMemoryXmlReader
    {
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
        public DataStoreInMemory Read(Stream stream)
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
        public DataStoreInMemory Read(string xml)
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
        public DataStoreInMemory Read(XmlReader xmlReader)
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
        public DataStoreInMemory Read(Stream stream, IBusinessObjectXmlReader boReader)
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
        public DataStoreInMemory Read(XmlReader xmlReader, IBusinessObjectXmlReader boReader)
        {
            BOSequenceNumber.LoadNumberGenClassDef();
            var objects = new Dictionary<Guid, IBusinessObject>();
            var bos = boReader.Read(xmlReader);
            bos.ForEach(
                bo =>
                    {
                        var objectToAdd = ConfigureObjectAfterLoad(bo);
                        objects.Add(objectToAdd.ID.GetAsGuid(), objectToAdd);
                    });
 
            ReadResult = boReader.PropertyReadExceptions.Count() == 0 ? 
                new Result(true) : 
                new Result(false, BuildExceptionMessage(boReader.PropertyReadExceptions));
            return new DataStoreInMemory {AllObjects = objects};
        }

        /// <summary>
        /// Sets up the object after loading. In this case:
        /// 1. Sets the status of the object to not new, not dirty, not editing and not deleted.
        /// 2. Calls the <see cref="BusinessObject.AfterLoad()"/> method on the object
        /// 3. Backs up the property values, setting the persisted values of the properties
        /// </summary>
        /// <param name="bo">The object to configure</param>
        /// <returns>The business object to add to the <see cref="DataStoreInMemory"/></returns>
        protected virtual IBusinessObject ConfigureObjectAfterLoad(IBusinessObject bo)
        {
            BusinessObjectLoaderBase.SetStatusAfterLoad(bo);
            BusinessObjectLoaderBase.CallAfterLoad(bo);
            bo.Props.BackupPropertyValues();
            return bo;
        }

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