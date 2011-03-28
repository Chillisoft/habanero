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
using System.Text;
using System.Xml;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemoryXmlWriter
    {
        private readonly Stream _stream;
        private XmlWriterSettings _settings;

        public DataStoreInMemoryXmlWriter()
            : this(new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto })
        {
        }

        public DataStoreInMemoryXmlWriter(XmlWriterSettings xmlWriterSettings)
        {
            _settings = xmlWriterSettings;
        }

        public DataStoreInMemoryXmlWriter(Stream stream)
            : this(stream, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto })
        {
        }

        public DataStoreInMemoryXmlWriter(Stream stream, XmlWriterSettings xmlWriterSettings)
        {
            _stream = stream;
            _settings = xmlWriterSettings;
        }

        public void Write(DataStoreInMemory dataStore)
        {
            Write(dataStore.AllObjects);
        }

        public void WriteToString(DataStoreInMemory dataStore, StringBuilder s)
        {
            XmlWriter writer = XmlWriter.Create(s, _settings);
            WriteObjects(writer, dataStore.AllObjects);
        }

        public void Write(Dictionary<Guid, IBusinessObject> businessObjects)
        {
            if (_stream == null) throw new ArgumentException("'stream' cannot be null");
            XmlWriter writer = XmlWriter.Create(_stream, _settings);
            WriteObjects(writer, businessObjects);
        }

        private void WriteObjects(XmlWriter writer, Dictionary<Guid, IBusinessObject> businessObjects)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("BusinessObjects");
            foreach (var o in businessObjects)
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