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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Reads business objects from a binary file that has been saved with a <see cref="DataStoreInMemoryBinaryWriter"/>
    /// </summary>
    public class DataStoreInMemoryBinaryReader
    {
        private readonly Stream _stream;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">The binary stream to read from</param>
        public DataStoreInMemoryBinaryReader(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Reads from the stream.
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<Guid, IBusinessObject> Read()
        {
            var formatter = new BinaryFormatter();
            var deserializedObject = formatter.Deserialize(_stream);
            if (deserializedObject is ConcurrentDictionary<Guid, IBusinessObject>)
            {
                return (ConcurrentDictionary<Guid, IBusinessObject>)deserializedObject;
            }
            var deserialisedEnumerable = (IEnumerable<KeyValuePair<Guid, IBusinessObject>>) deserializedObject;
            return new ConcurrentDictionary<Guid, IBusinessObject>(deserialisedEnumerable);
        }
    }
}