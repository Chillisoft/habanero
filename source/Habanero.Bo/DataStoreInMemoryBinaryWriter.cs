using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(_stream, dataStore.AllObjects);
        }
    }
}