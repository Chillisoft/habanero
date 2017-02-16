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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.Graphics;

namespace Habanero.Util
{
    /// <summary>
    /// Provides serialisation utilities
    /// </summary>
    public static class SerialisationUtilities
    {
        /// <summary>
        /// Converts an Object to a byte array
        /// </summary>
        /// <param name="obj">The Object to convert</param>
        /// <returns>Returns a byte array</returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj is Bitmap)
            {
                var image = (Bitmap)obj;
                using (var stream = new MemoryStream())
                {
                    image.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    return stream.ToArray();
                }
            }
            return new byte[0];
        }

        /// <summary>
        /// Converts a byte array to an Object
        /// </summary>
        /// <param name="arrBytes">The byte array to convert</param>
        /// <returns>Returns an Object</returns>
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            Object obj = binForm.Deserialize(memStream);

            return obj;
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        /// <exception cref="IOException">Thrown if any of the underlying 
        /// IO calls fail</exception>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length*2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte) nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
    }
}