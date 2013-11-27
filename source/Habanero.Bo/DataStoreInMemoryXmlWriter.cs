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
    /// Writes the contents of a <see cref="DataStoreInMemory"/> to an xml stream
    /// </summary>
    public class DataStoreInMemoryXmlWriter
    {
        private readonly XmlWriterSettings _settings;

        /// <summary>
        /// Default constructor - ConformanceLevel = Auto.
        /// </summary>
        public DataStoreInMemoryXmlWriter()
            : this(new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto })
        {
        }

        /// <summary>
        /// Constructor - set your own <see cref="XmlWriterSettings"/>
        /// </summary>
        /// <param name="xmlWriterSettings"></param>
        public DataStoreInMemoryXmlWriter(XmlWriterSettings xmlWriterSettings)
        {
            _settings = xmlWriterSettings;
        }

        /// <summary>
        /// Writes the data store's objects to the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="dataStore"></param>
        public void Write(Stream stream, DataStoreInMemory dataStore)
        {
            Write(stream, dataStore.AllObjects);
        }

        /// <summary>
        /// Writes the datastore's objects to a StringBuilder
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dataStore"></param>
        public void Write(StringBuilder s, DataStoreInMemory dataStore)
        {
            var writer = XmlWriter.Create(s, _settings);
            Write(writer, dataStore.AllObjects);
        }

        /// <summary>
        /// Writes the dictionary of business objects (only the actual bos) to the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="businessObjects"></param>
        public void Write(Stream stream, IDictionary<Guid, IBusinessObject> businessObjects)
        {
            var writer = XmlWriter.Create(stream, _settings);
            Write(writer, businessObjects);
        }

        /// <summary>
        /// Writes the businessobjects to the writer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="businessObjects"></param>
        /// <param name="includeStartDocument">If true, starts a new xml doc and will close the writer 
        /// after completion. If false, will act as if the write already has a doc started 
        /// (ie, won't add the startdoc element or close the writer). Defaults to true</param>
        public void Write(XmlWriter writer, IDictionary<Guid, IBusinessObject> businessObjects, bool includeStartDocument = true)
        {
            var boWriter = new BusinessObjectXmlWriter();
            boWriter.Write(writer, businessObjects.Values, includeStartDocument);
        }


    }
}