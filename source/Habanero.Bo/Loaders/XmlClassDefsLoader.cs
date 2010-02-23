// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Facilitates the loading of class definitions from xml data, which define
    /// the terms and properties of an xml structure before it can be
    /// populated with data
    /// </summary>
    public class XmlClassDefsLoader : XmlLoader, IClassDefsLoader
    {
        private ClassDefCol _classDefList;
        private readonly string _xmlClassDefs;

        /// <summary>
        /// Constructor to create a new list of class definitions from the
        /// string provided, using the dtd path provided
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
        /// <param name="dtdLoader">The dtd loader</param>
        public XmlClassDefsLoader(string xmlClassDefs, DtdLoader dtdLoader) : this(xmlClassDefs, dtdLoader, new DefClassFactory())
        {
        }

        /// <summary>
        /// Constructor to create a new list of class definitions from the
        /// string provided, using the dtd path provided
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
        /// <param name="dtdLoader">The dtd loader</param>
        /// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlClassDefsLoader(string xmlClassDefs, DtdLoader dtdLoader, IDefClassFactory defClassFactory)
            : base(dtdLoader, defClassFactory)
        {
            _xmlClassDefs = xmlClassDefs;
        }

        /// <summary>
        /// Loads class definitions, converting them from a 
        /// string containing these definitions to an IList object.
        /// If the conversion fails, an error message will be sent to the 
        /// console.
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
        /// <returns>Returns an IList object containing the definitions</returns>
        public ClassDefCol LoadClassDefs(string xmlClassDefs)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlClassDefs);
            }
            catch (XmlException ex)
            {
                throw new XmlException
                    ("The class definitions XML file has no root "
                     + "element 'classes'.  The document needs a master 'classes' element "
                     + "and individual 'class' elements for each of the classes you are " + "defining.", ex);
            }
            var origionalExceptionNotifier = GlobalRegistry.UIExceptionNotifier;
            try
            {
                
                var recordingExceptionNotifier = new RecordingExceptionNotifier();
                GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;

                var loadClassDefs = LoadClassDefs(doc.DocumentElement);
                recordingExceptionNotifier.RethrowRecordedException();
                return loadClassDefs;
            }
            finally
            {
                GlobalRegistry.UIExceptionNotifier = origionalExceptionNotifier;
            }

        }

        /// <summary>
        /// As with LoadClassDefs(string), but uses the definition string 
        /// provided on instatiation of the object if you used the
        /// parameterised constructor.
        /// </summary>
        /// <returns>Returns a ClassDefCol containing the definitions</returns>
        public ClassDefCol LoadClassDefs()
        {
            return LoadClassDefs(_xmlClassDefs);
        }

        /// <summary>
        /// As with LoadClassDefs(string), but uses the root element as a
        /// starting reference point.
        /// </summary>
        /// <param name="allClassesElement">The root element</param>
        /// <returns>Returns an IList object containing the definitions</returns>
        public ClassDefCol LoadClassDefs(XmlElement allClassesElement)
            ///// <returns>Returns an IList object containing the definitions</returns>
            //public IList LoadClassDefs(XmlElement allClassesElement)
        {
            ClassDefCol classDefCol = (ClassDefCol) this.Load(allClassesElement);
            return classDefCol;
        }

        /// <summary>
        /// Returns the IList object that contains the class definitions
        /// </summary>
        /// <returns>Returns an IList object</returns>
        protected override object Create()
        {
            return _classDefList;
        }

        /// <summary>
        /// Loads the class definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _classDefList = _defClassFactory.CreateClassDefCol();
            //_classDefList = new ArrayList();
            _reader.Read();
            _reader.Read();
            do
            {
                try
                {
                    XmlClassLoader classLoader = new XmlClassLoader(DtdLoader, _defClassFactory);
                    var classDefCol = classLoader.LoadClass(_reader.ReadOuterXml());
                    if(classDefCol != null) _classDefList.Add(classDefCol);
                }
                catch (Exception ex)
                {
                    //This is a RecordingExceptionNotifiere so it will log this error and
                    // allow the continued processing of this XMLFile.
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
                }
            } while (_reader.Name == "class");
        }
    }
}