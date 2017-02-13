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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Graphics;
using Android.Media;
using Habanero.Base.DataMappers;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestImageDataMapper
    {
        [Test]
        public void TryParsePropValue_WorksForNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_WorksForImage()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            var valueToParse = LoadImageForTest("TestJpeg.jpg");
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreSame(valueToParse, parsedValue);
        }

        [Test]
        public void TryParsePropValue_ConvertsStringToImage()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            var img = LoadImageForTest("TestJpeg.jpg");
            var valueToParse = dataMapper.ConvertValueToString(img);
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsInstanceOf(typeof(Bitmap), parsedValue);
            Assert.AreEqual(img.Width, ((Bitmap) parsedValue).Width);
            Assert.AreEqual(img.Height, ((Bitmap) parsedValue).Height);
        }

        [Test]
        public void TryParsePropValue_ConvertsByteArrayToImage()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            var img = LoadImageForTest("TestJpeg.jpg");
            var valueToParse = SerialisationUtilities.ObjectToByteArray(img);
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsInstanceOf(typeof(Bitmap), parsedValue);
            Assert.AreEqual(img.Width, ((Bitmap) parsedValue).Width);
            Assert.AreEqual(img.Height, ((Bitmap) parsedValue).Height);
        }

        [Test]
        public void TryParsePropValue_FailsForOtherTypes()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parsedSucceed = dataMapper.TryParsePropValue(3, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(parsedSucceed);
        }

        [Test]
        public void ConvertValueToString_FromBitmap()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            var img = LoadImageForTest("TestJpeg.jpg");
            //---------------Execute Test ----------------------
            string strValue = dataMapper.ConvertValueToString(img);
            //---------------Test Result -----------------------
            Assert.AreNotEqual("System.Drawing.Bitmap", strValue);
        }

        private static Image LoadImageForTest(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            var path = asm.GetManifestResourceNames().FirstOrDefault(x => x.Contains(name));

            using (var stream = asm.GetManifestResourceStream(path))
            {
                var bytes = new List<byte>();
                byte toRead = 0;
                while ((toRead = (byte) stream.ReadByte()) != -1)
                {
                    bytes.Add(toRead);
                }
                var result = (Image) Image.FromArray(bytes.ToArray());
                return result;
            }
        }
    }
}