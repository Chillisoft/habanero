using System;
using System.Drawing;
using System.IO;
using System.Resources;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    [TestFixture]
    public class TestSerialisationUtilities
    {
        private ResourceManager _resourceManager;

        [SetUp]
        public void SetUpResources()
        {
            _resourceManager = new ResourceManager("Habanero.Test.TestResources", typeof(TestJpgMetaData).Assembly);
        }

        [Test]
        public void TestTwoWayConversion()
        {
            Image image = (Image)_resourceManager.GetObject("TestJpeg");

            byte[] bytesOut = SerialisationUtilities.ObjectToByteArray(image);
            Object objectOut = SerialisationUtilities.ByteArrayToObject(bytesOut);

            Assert.AreEqual(image.GetType(), objectOut.GetType());
        }
    }
}
