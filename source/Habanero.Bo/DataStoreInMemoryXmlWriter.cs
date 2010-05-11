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
using System.IO;
using System.Xml;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemoryXmlWriter
    {
        private readonly Stream _stream;
        private XmlWriterSettings _settings;

        public DataStoreInMemoryXmlWriter(Stream stream): this(stream, new XmlWriterSettings {ConformanceLevel = ConformanceLevel.Auto})
        {
        }

        public DataStoreInMemoryXmlWriter(Stream stream, XmlWriterSettings xmlWriterSettings)
        {
            _stream = stream;
            _settings = xmlWriterSettings;
        }

        public void Write(DataStoreInMemory dataStore)
        {
            XmlWriter writer = XmlWriter.Create(_stream, _settings);
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