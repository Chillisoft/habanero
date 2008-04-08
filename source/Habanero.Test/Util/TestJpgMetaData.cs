//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Resources;
using Habanero.Base.Exceptions;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test.Util
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
            //File.WriteAllText("tempfile.txt", "test");
            //Image image = (Image)_resourceManager.GetObject("TestJpeg2");
            //image.Save("test.jpg");
        }

        [TearDown]
        public void DeleteTempFile()
        {
            //_resourceManager.ReleaseAllResources();
            //File.Delete("tempfile.txt");
            //File.Delete("test.jpg");
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

        [Test]
        public void TestExifMetadataWithPhoto()
        {
            Image image = (Image)_resourceManager.GetObject("TestPhoto");
            JpgMetaData data = new JpgMetaData();
            JpgMetaData.Metadata metadata = data.GetExifMetadata(image);

            Assert.AreEqual("618z3 Digital Camera ", metadata.CameraModel.DisplayValue);
            Assert.AreEqual("110", metadata.CameraModel.Hex);
            Assert.AreEqual("36-31-38-7A-33-20-44-69-67-69-74-61-6C-20-43-61-6D-65-72-61-20-00", metadata.CameraModel.RawValueAsString);

            Assert.AreEqual("2007:07:14 12:36:58", metadata.DatePictureTaken.DisplayValue);
            Assert.AreEqual("9003", metadata.DatePictureTaken.Hex);
            Assert.AreEqual("32-30-30-37-3A-30-37-3A-31-34-20-31-32-3A-33-36-3A-35-38-00", metadata.DatePictureTaken.RawValueAsString);

            Assert.AreEqual("SOMERFIELD/ AVENTI", metadata.EquipmentMake.DisplayValue);
            Assert.AreEqual("10f", metadata.EquipmentMake.Hex);
            Assert.AreEqual("53-4F-4D-45-52-46-49-45-4C-44-2F-20-41-56-45-4E-54-49-00", metadata.EquipmentMake.RawValueAsString);

            Assert.AreEqual("1000/100000", metadata.ExposureTime.DisplayValue);
            Assert.AreEqual("829a", metadata.ExposureTime.Hex);
            Assert.AreEqual("E8-03-00-00-A0-86-01-00", metadata.ExposureTime.RawValueAsString);

            Assert.AreEqual("F/4", metadata.Fstop.DisplayValue);
            Assert.AreEqual("829d", metadata.Fstop.Hex);
            Assert.AreEqual("35-01-00-00-40-00-00-00", metadata.Fstop.RawValueAsString);

            Assert.AreEqual(null, metadata.ShutterSpeed.DisplayValue);
            Assert.AreEqual("9201", metadata.ShutterSpeed.Hex);
            Assert.AreEqual(null, metadata.ShutterSpeed.RawValueAsString);

            Assert.AreEqual("0 (Needs work to confirm accuracy)", metadata.ExposureCompensation.DisplayValue);
            Assert.AreEqual("9204", metadata.ExposureCompensation.Hex);
            Assert.AreEqual("00-00-00-00-0A-00-00-00", metadata.ExposureCompensation.RawValueAsString);

            Assert.AreEqual("Center Weighted Average", metadata.MeteringMode.DisplayValue);
            Assert.AreEqual("9207", metadata.MeteringMode.Hex);
            Assert.AreEqual("02-00", metadata.MeteringMode.RawValueAsString);

            Assert.AreEqual(null, metadata.Flash.DisplayValue);
            Assert.AreEqual("9209", metadata.Flash.Hex);
            Assert.AreEqual("1F-00", metadata.Flash.RawValueAsString);

            Assert.AreEqual("72", metadata.XResolution.DisplayValue);
            Assert.AreEqual(null, metadata.XResolution.Hex);
            Assert.AreEqual(null, metadata.XResolution.RawValueAsString);

            Assert.AreEqual("72", metadata.YResolution.DisplayValue);
            Assert.AreEqual(null, metadata.YResolution.Hex);
            Assert.AreEqual(null, metadata.YResolution.RawValueAsString);

            Assert.AreEqual("100", metadata.ImageWidth.DisplayValue);
            Assert.AreEqual(null, metadata.ImageWidth.Hex);
            Assert.AreEqual(null, metadata.ImageWidth.RawValueAsString);

            Assert.AreEqual("100", metadata.ImageHeight.DisplayValue);
            Assert.AreEqual(null, metadata.ImageHeight.Hex);
            Assert.AreEqual(null, metadata.ImageHeight.RawValueAsString);
        }

        // Tried to write an image to a temp file, then read it here
        // and delete it in the teardown, but getting an error on deleting
        // the file
//        [Test]
//        public void TestExifMetadataFromFilePath()
//        {
//            JpgMetaData data = new JpgMetaData();
//            JpgMetaData.Metadata metadata = data.GetExifMetadata("test.jpg");
//            Assert.AreEqual("10", metadata.ImageHeight.DisplayValue);
//        }

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

        // Attempted to test the null test (file exists but image conversion goes
        // to null), but was getting outofmemoryexception
//        [Test, ExpectedException(typeof(FileLoadException))]
//        public void TestInvalidImageFileException()
//        {
//            JpgMetaData data = new JpgMetaData();
//            JpgMetaData.Metadata metadata = data.GetExifMetadata("tempfile.txt");
//        }

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
