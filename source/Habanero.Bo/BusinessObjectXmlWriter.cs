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
        /// Writes the given business objects to the xml writer.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="businessObjects">The business objects to write</param>
        void Write(XmlWriter writer, IEnumerable<IBusinessObject> businessObjects);
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
            writer.WriteStartDocument();
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
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}