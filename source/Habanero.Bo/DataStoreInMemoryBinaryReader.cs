using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemoryBinaryReader
    {
        private readonly Stream _stream;

        public DataStoreInMemoryBinaryReader(Stream stream)
        {
            _stream = stream;
        }

        public Dictionary<Guid, IBusinessObject> Read()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Dictionary<Guid, IBusinessObject> objects = (Dictionary<Guid, IBusinessObject>)formatter.Deserialize(_stream);
            return objects;
        }
    }
}