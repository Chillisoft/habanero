using System;
using System.Drawing;
using System.IO;
using System.Resources;
using Habanero.Base.Exceptions;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    /// <summary>
    /// TODO:
    /// - test file loading from path
    /// </summary>
    [TestFixture]
    public class TestJpgMetaData
    {
        private ResourceManager _resourceManager;

        [SetUp]
        public void SetUpResources()
        {
            _resourceManager = new ResourceManager("Habanero.Test.TestResources", typeof(TestJpgMetaData).Assembly);
        }

        [Test]
        public void TestExifMetadata()
        {
            Image image = (Image)_resourceManager.GetObject("TestJpeg");
            JpgMetaData data = new JpgMetaData();
            JpgMetaData.Metadata metadata = data.GetExifMetadata(image);

            Assert.AreEqual(null, metadata.CameraModel.DisplayValue);
            Assert.AreEqual("110", metadata.CameraModel.Hex);
            Assert.AreEqual(null, metadata.CameraModel.RawValueAsString);

            Assert.AreEqual(null, metadata.DatePictureTaken.DisplayValue);
            Assert.AreEqual("9003", metadata.DatePictureTaken.Hex);
            Assert.AreEqual(null, metadata.DatePictureTaken.RawValueAsString);

            Assert.AreEqual(null, metadata.EquipmentMake.DisplayValue);
            Assert.AreEqual("10f", metadata.EquipmentMake.Hex);
            Assert.AreEqual(null, metadata.EquipmentMake.RawValueAsString);

            Assert.AreEqual(null, metadata.ExposureTime.DisplayValue);
            Assert.AreEqual("829a", metadata.ExposureTime.Hex);
            Assert.AreEqual(null, metadata.ExposureTime.RawValueAsString);

            Assert.AreEqual(null, metadata.Fstop.DisplayValue);
            Assert.AreEqual("829d", metadata.Fstop.Hex);
            Assert.AreEqual(null, metadata.Fstop.RawValueAsString);

            Assert.AreEqual(null, metadata.ShutterSpeed.DisplayValue);
            Assert.AreEqual("9201", metadata.ShutterSpeed.Hex);
            Assert.AreEqual(null, metadata.ShutterSpeed.RawValueAsString);

            Assert.AreEqual(null, metadata.ExposureCompensation.DisplayValue);
            Assert.AreEqual("9204", metadata.ExposureCompensation.Hex);
            Assert.AreEqual(null, metadata.ExposureCompensation.RawValueAsString);

            Assert.AreEqual(null, metadata.MeteringMode.DisplayValue);
            Assert.AreEqual("9207", metadata.MeteringMode.Hex);
            Assert.AreEqual(null, metadata.MeteringMode.RawValueAsString);

            Assert.AreEqual(null, metadata.Flash.DisplayValue);
            Assert.AreEqual("9209", metadata.Flash.Hex);
            Assert.AreEqual(null, metadata.Flash.RawValueAsString);

            Assert.AreEqual("96", metadata.XResolution.DisplayValue);
            Assert.AreEqual(null, metadata.XResolution.Hex);
            Assert.AreEqual(null, metadata.XResolution.RawValueAsString);

            Assert.AreEqual("96", metadata.YResolution.DisplayValue);
            Assert.AreEqual(null, metadata.YResolution.Hex);
            Assert.AreEqual(null, metadata.YResolution.RawValueAsString);

            Assert.AreEqual("10", metadata.ImageWidth.DisplayValue);
            Assert.AreEqual(null, metadata.ImageWidth.Hex);
            Assert.AreEqual(null, metadata.ImageWidth.RawValueAsString);

            Assert.AreEqual("10", metadata.ImageHeight.DisplayValue);
            Assert.AreEqual(null, metadata.ImageHeight.Hex);
            Assert.AreEqual(null, metadata.ImageHeight.RawValueAsString);
        }

        [Test, ExpectedException(typeof(HabaneroArgumentException))]
        public void TestInvalidImageException()
        {
            Image image = (Image)_resourceManager.GetObject("TestJpegInvalid");
            JpgMetaData data = new JpgMetaData();
            JpgMetaData.Metadata metadata = data.GetExifMetadata(image);
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void TestInvalidImagePathException()
        {
            JpgMetaData data = new JpgMetaData();
            JpgMetaData.Metadata metadata = data.GetExifMetadata("falsepath");
        }

        [Test]
        public void TestLookupExifValue()
        {
            JpgMetaData data = new JpgMetaData();

            Assert.AreEqual("Unknown", data.LookupExifValue("MeteringMode", "0"));
            Assert.AreEqual("Average", data.LookupExifValue("MeteringMode", "1"));
            Assert.AreEqual("Center Weighted Average", data.LookupExifValue("MeteringMode", "2"));
            Assert.AreEqual("Spot", data.LookupExifValue("MeteringMode", "3"));
            Assert.AreEqual("Multi-spot", data.LookupExifValue("MeteringMode", "4"));
            Assert.AreEqual("Multi-segment", data.LookupExifValue("MeteringMode", "5"));
            Assert.AreEqual("Partial", data.LookupExifValue("MeteringMode", "6"));
            Assert.AreEqual("Other", data.LookupExifValue("MeteringMode", "255"));

            Assert.AreEqual("No Units", data.LookupExifValue("ResolutionUnit", "1"));
            Assert.AreEqual("Inch", data.LookupExifValue("ResolutionUnit", "2"));
            Assert.AreEqual("Centimeter", data.LookupExifValue("ResolutionUnit", "3"));

            Assert.AreEqual("Flash did not fire", data.LookupExifValue("Flash", "0"));
            Assert.AreEqual("Flash fired", data.LookupExifValue("Flash", "1"));
            Assert.AreEqual("Flash fired but strobe return light not detected", data.LookupExifValue("Flash", "5"));
            Assert.AreEqual("Flash fired and strobe return light detected", data.LookupExifValue("Flash", "7"));
        }
    }
}
