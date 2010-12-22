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
//TODO andrew 22 Dec 2010: CF : Serialization not available in CF
//using System.Runtime.Serialization.Formatters.Binary;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemoryBinaryWriter
    {
        private readonly Stream _stream;

        public DataStoreInMemoryBinaryWriter(Stream stream)
        {
            _stream = stream;
        }

        public void Write(DataStoreInMemory dataStore)
        {
            Write(dataStore.AllObjects);
        }

        public void Write(Dictionary<Guid, IBusinessObject> businessObjects)
        {
            throw new NotImplementedException("CF: Code commented out to get CF to compile");
            //BinaryFormatter formatter = new BinaryFormatter();
            //formatter.Serialize(_stream, businessObjects);
        }
    }
}