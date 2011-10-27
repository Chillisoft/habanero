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
using System.Drawing.Imaging;
using System.IO;
using Habanero.Base;
using Habanero.Base.DataMappers;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
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
            var valueToParse = new System.Drawing.Bitmap(100, 100);
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
            var img = new System.Drawing.Bitmap(100, 100);
            var valueToParse = dataMapper.ConvertValueToString(img);
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsInstanceOf(typeof(System.Drawing.Bitmap), parsedValue);
            Assert.AreEqual(img.Width, ((System.Drawing.Bitmap) parsedValue).Width);
            Assert.AreEqual(img.Height, ((System.Drawing.Bitmap) parsedValue).Height);
        }

        [Test]
        public void TryParsePropValue_ConvertsByteArrayToImage()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ImageDataMapper();
            var img = new System.Drawing.Bitmap(100, 100);
            var valueToParse = SerialisationUtilities.ObjectToByteArray(img);
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsInstanceOf(typeof(System.Drawing.Bitmap), parsedValue);
            Assert.AreEqual(img.Width, ((System.Drawing.Bitmap)parsedValue).Width);
            Assert.AreEqual(img.Height, ((System.Drawing.Bitmap)parsedValue).Height);
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
            var img = new System.Drawing.Bitmap(100, 100);
            //---------------Execute Test ----------------------
            string strValue = dataMapper.ConvertValueToString(img);
            //---------------Test Result -----------------------
            Assert.AreNotEqual("System.Drawing.Bitmap", strValue);
        }
     
    }
}