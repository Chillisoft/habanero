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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Writes a set of <see cref="IBusinessObject"/> to an XmlWriter.
    /// There is a default implementation: <see cref="BusinessObjectXmlWriter"/> 
    /// It's easiest to use this interface via <see cref="DataStoreInMemoryXmlWriter"/>. 
    /// However, if you need to change how objects are read/written from xml, 
    /// implement your own <see cref="IBusinessObjectXmlReader"/>
    /// and <see cref="IBusinessObjectXmlWriter"/> and use them with those classes.
    /// </summary>
    public interface IBusinessObjectXmlWriter
    {
        /// <summary>
        /// Writes the given business objects to the xml writer.Writes the start document
        /// element and end document element, as well as closes the XmlWriter. Make sure the
        /// <see cref="ConformanceLevel"/> of the <see cref="XmlWriter"/> is set to Auto.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="businessObjects">The business objects to write</param>
        void Write(XmlWriter writer, IEnumerable<IBusinessObject> businessObjects);

        /// <summary>
        /// Writes the given business objects to the xml writer. If includeStartDocument is true,
        /// this method writes the start document element and end document element, as well as 
        /// closes the XmlWriter. If it's set to false it doesn't include start/end document elements
        /// and leaves the writer open. Make sure the
        /// <see cref="ConformanceLevel"/> of the <see cref="XmlWriter"/> is set to Auto.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="businessObjects">The business objects to write</param>
        /// <param name="includeStartDocument">If true the StartDocument element and EndDocument elements will be included, and the writer will be closed </param>

        void Write(XmlWriter writer, IEnumerable<IBusinessObject> businessObjects, bool includeStartDocument);
    }

    /// <summary>
    /// Writes a set of <see cref="IBusinessObject"/> to an XmlWriter.
    /// This is the default implementation.
    /// It's easiest to use this class via <see cref="DataStoreInMemoryXmlWriter"/>. 
    /// However, if you need to change how objects are read/written from xml, 
    /// implement your own <see cref="IBusinessObjectXmlReader"/>
    /// and <see cref="IBusinessObjectXmlWriter"/> and use them with that class.
    /// </summary>
    public class BusinessObjectXmlWriter : IBusinessObjectXmlWriter
    {
        /// <summary>
        /// Writes the given business objects to the xml writer. Writes the start document
        /// element and end document element, as well as closes the XmlWriter. Make sure the
        /// <see cref="ConformanceLevel"/> of the <see cref="XmlWriter"/> is set to Auto.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="businessObjects">The business objects to write</param>
        public void Write(XmlWriter writer, IEnumerable<IBusinessObject> businessObjects)
        {
            Write(writer, businessObjects, true);
        }

        /// <summary>
        /// Writes the given business objects to the xml writer. If includeStartDocument is true,
        /// this method writes the start document element and end document element, as well as 
        /// closes the XmlWriter. If it's set to false it doesn't include start/end document elements
        /// and leaves the writer open. Make sure the
        /// <see cref="ConformanceLevel"/> of the <see cref="XmlWriter"/> is set to Auto.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="businessObjects">The business objects to write</param>
        /// <param name="includeStartDocument">If true the StartDocument element and EndDocument elements will be included, and the writer will be closed </param>
        public void Write(XmlWriter writer, IEnumerable<IBusinessObject> businessObjects,bool includeStartDocument)
        {
            if (includeStartDocument) writer.WriteStartDocument();
            writer.WriteStartElement("BusinessObjects");
            foreach (var o in businessObjects)
            {
                writer.WriteStartElement("bo");
                writer.WriteAttributeString("__tn", o.ClassDef.ClassName);
                writer.WriteAttributeString("__an", o.ClassDef.AssemblyName);
                foreach (var prop in o.Props)
                {
                    writer.WriteAttributeString(prop.PropertyName, prop.PropertyValueString);
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            if (includeStartDocument)
            {
                writer.WriteEndDocument();
                writer.Close();
            }
        }
    }
}