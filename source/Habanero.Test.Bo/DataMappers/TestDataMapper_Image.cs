using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataMapper_Image
    {
        [Test]
        public void TryParsePropValue_WorksForNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new BOPropImageDataMapper();
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
            var dataMapper = new BOPropImageDataMapper();
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
            var dataMapper = new BOPropImageDataMapper();
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
        public void TryParsePropValue_FailsForOtherTypes()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new BOPropImageDataMapper();
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
            var dataMapper = new BOPropImageDataMapper();
            var img = new System.Drawing.Bitmap(100, 100);
            //---------------Execute Test ----------------------
            string strValue = dataMapper.ConvertValueToString(img);
            //---------------Test Result -----------------------
            Assert.AreNotEqual("System.Drawing.Bitmap", strValue);
        }
     
    }
}