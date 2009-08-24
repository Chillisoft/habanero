using System;
using System.IO;
using System.Xml;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemoryXmlWriter
    {
        private readonly Stream _stream;

        public DataStoreInMemoryXmlWriter(Stream stream)
        {
            _stream = stream;
        }

        public void Write(DataStoreInMemory dataStore)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;

            XmlWriter writer = XmlWriter.Create(_stream, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("BusinessObjects");
            foreach (var o in dataStore.AllObjects)
            {
                writer.WriteStartElement("bo");
                writer.WriteAttributeString("__tn", o.Value.ClassDef.ClassName);
                writer.WriteAttributeString("__an", o.Value.ClassDef.AssemblyName);
                foreach (IBOProp prop in o.Value.Props)
                {
                    writer.WriteAttributeString(prop.PropertyName, Convert.ToString(prop.Value));
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

        }
    }
}