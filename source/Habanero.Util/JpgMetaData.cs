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
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    /// <summary>
    /// Provides facilities of obtaining metadata from a jpeg image
    /// </summary>
    public class JpgMetaData
    {
        /// <summary>
        /// A struct to hold image metadata detail
        /// </summary>
        public struct MetadataDetail
        {
            public string Hex;
            public string RawValueAsString;
            public string DisplayValue;
        }

        /// <summary>
        /// A struct to hold image metadata
        /// </summary>
        public struct Metadata
        {
            public MetadataDetail EquipmentMake;
            public MetadataDetail CameraModel;
            public MetadataDetail ExposureTime;
            public MetadataDetail Fstop;
            public MetadataDetail DatePictureTaken;
            public MetadataDetail ShutterSpeed;
            public MetadataDetail ExposureCompensation;
            public MetadataDetail MeteringMode;
            public MetadataDetail Flash;
            public MetadataDetail XResolution;
            public MetadataDetail YResolution;
            public MetadataDetail ImageWidth;
            public MetadataDetail ImageHeight;
        }

        /// <summary>
        /// Returns the exif value for the specified arguments
        /// </summary>
        /// <param name="description">The exif description</param>
        /// <param name="value">The exif value type</param>
        /// <returns>Returns a string</returns>
        public string LookupExifValue(string description, string value)
        {
            string descriptionValue = null;

            if (description == "MeteringMode")
            {
                switch (value)
                {
                    case "0":
                        descriptionValue = "Unknown";
                        break;
                    case "1":
                        descriptionValue = "Average";
                        break;
                    case "2":
                        descriptionValue = "Center Weighted Average";
                        break;
                    case "3":
                        descriptionValue = "Spot";
                        break;
                    case "4":
                        descriptionValue = "Multi-spot";
                        break;
                    case "5":
                        descriptionValue = "Multi-segment";
                        break;
                    case "6":
                        descriptionValue = "Partial";
                        break;
                    case "255":
                        descriptionValue = "Other";
                        break;
                }
            }

            if (description == "ResolutionUnit")
            {
                switch (value)
                {
                    case "1":
                        descriptionValue = "No Units";
                        break;
                    case "2":
                        descriptionValue = "Inch";
                        break;
                    case "3":
                        descriptionValue = "Centimeter";
                        break;
                }
            }

            if (description == "Flash")
            {
                switch (value)
                {
                    case "0":
                        descriptionValue = "Flash did not fire";
                        break;
                    case "1":
                        descriptionValue = "Flash fired";
                        break;
                    case "5":
                        descriptionValue = "Flash fired but strobe return light not detected";
                        break;
                    case "7":
                        descriptionValue = "Flash fired and strobe return light detected";
                        break;
                }
            }
            return descriptionValue;
        }

        /// <summary>
        /// Returns a Metadata object with exif metadata for the image
        /// in the given path
        /// </summary>
        /// <param name="imagePath">The image path</param>
        /// <returns>Returns a Metadata object</returns>
        public Metadata GetExifMetadata(string imagePath)
        {
            Image image = Image.FromFile(imagePath);
            if (image == null)
            {
                throw new FileLoadException("Image file not valid.");
            }
            return GetExifMetadata(image);
        }

        /// <summary>
        /// Returns a Metadata object with exif metadata for the given image
        /// </summary>
        /// <param name="image">The photo</param>
        /// <returns>Returns a Metadata object</returns>
        public Metadata GetExifMetadata(Image image)
        {
            if (image == null)
            {
                throw new HabaneroArgumentException("image");
            }

            // Create an integer array to hold the property id list,
            // and populate it with the list from my image.
            /* Note: this only generates a
										 list of integers, one for for each PropertyID.
													  * We will populate the
										 PropertyItem values later. */
            int[] propertyIdList = image.PropertyIdList;

            // Create an array of PropertyItems, but don't populate it yet.
            /* Note: there is a bug in .class="highlight1">net
						  framework v1.0 SP2 and also in 1.1 beta:
									   * If any particular PropertyItem
						  has a length of 0, you will get an unhandled error
									   * when you populate the array
						  directly from the image.
									   * So, rather than create an
						  array of PropertyItems and then populate it directly
									   * from the image, we will create
						  an empty one of the appropriate length, and then
									   * test each of the PropertyItems
						  ourselves, one at a time, and not add any that
									   * would cause an error. */
            PropertyItem[] propertyItemList = new PropertyItem[propertyIdList.Length];

            // Create an instance of Metadata and populate Hex codes (values populated later)
            Metadata metadata = new Metadata();
            metadata.EquipmentMake.Hex = "10f";
            metadata.CameraModel.Hex = "110";
            metadata.DatePictureTaken.Hex = "9003";
            metadata.ExposureTime.Hex = "829a";
            metadata.Fstop.Hex = "829d";
            metadata.ShutterSpeed.Hex = "9201";
            metadata.ExposureCompensation.Hex = "9204";
            metadata.MeteringMode.Hex = "9207";
            metadata.Flash.Hex = "9209";

            // Declare an ASCIIEncoding to use for returning string values from bytes
            ASCIIEncoding Value = new ASCIIEncoding();

            // Populate MyPropertyItemList.
            // For each propertyID...
            int index = 0;
            foreach (int propertyId in propertyIdList)
            {
                // ... try to call GetPropertyItem (it crashes if PropertyItem has length 0, so use Try/Catch)
                try
                {
                    // Assign the image's PropertyItem to the PropertyItem array
                    propertyItemList[index] = image.GetPropertyItem(propertyId);

                    // Troublshooting
                    /*
                
																					textBox1.AppendText("\r\n\t" +
																				BitConverter.ToString(image.GetPropertyItem
																				(MyPropertyId).Value));
                
																					textBox1.AppendText("\r\n\thex location: " +
																				image.GetPropertyItem(MyPropertyId).Id.ToString("x"));
																									*/

                    // Assign each element of MyMetadata
                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "10f") // EquipmentMake
                    {
                        metadata.EquipmentMake.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
                        metadata.EquipmentMake.DisplayValue =
                            Value.GetString(propertyItemList[index].Value);
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "110") // CameraModel
                    {
                        metadata.CameraModel.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
                        metadata.CameraModel.DisplayValue =
                            Value.GetString(propertyItemList[index].Value);
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9003") // DatePictureTaken
                    {
                        metadata.DatePictureTaken.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
                        metadata.DatePictureTaken.DisplayValue =
                            Value.GetString(propertyItemList[index].Value);
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9207") // MeteringMode
                    {
                        metadata.MeteringMode.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
                        metadata.MeteringMode.DisplayValue =
                            LookupExifValue("MeteringMode", BitConverter.ToInt16
                                                                (image.GetPropertyItem(propertyId).Value, 0).ToString());
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9209") // Flash
                    {
                        metadata.Flash.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
                        metadata.Flash.DisplayValue = LookupExifValue
                            ("Flash", BitConverter.ToInt16(image.GetPropertyItem
                                                               (propertyId).Value, 0).ToString());
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "829a") // ExposureTime
                    {
                        metadata.ExposureTime.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);

                        string StringValue = "";
                        for (int Offset = 0; Offset < image.GetPropertyItem(propertyId).Len; Offset = Offset + 4)
                        {
                            StringValue += BitConverter.ToInt32
                                               (image.GetPropertyItem
                                                    (propertyId).Value, Offset).ToString() + "/";
                        }

                        metadata.ExposureTime.DisplayValue =
                            StringValue.Substring(0, StringValue.Length - 1);
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "829d") // F-stop
                    {
                        metadata.Fstop.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);

                        int int1;
                        int int2;
                        int1 = BitConverter.ToInt32(image.GetPropertyItem(propertyId).Value, 0);
                        int2 = BitConverter.ToInt32(image.GetPropertyItem(propertyId).Value, 4);

                        metadata.Fstop.DisplayValue = "F/" + (int1/int2);
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9201") // ShutterSpeed
                    {
                        metadata.ShutterSpeed.RawValueAsString =
                            BitConverter.ToString(image.GetPropertyItem(propertyId).Value);

                        string StringValue = BitConverter.ToString
                                (image.GetPropertyItem(propertyId).Value).Substring(0, 2);

                        metadata.ShutterSpeed.DisplayValue = "1/" + StringValue;
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9204") // ExposureCompensation
                    {
                        metadata.ExposureCompensation.RawValueAsString
                            = BitConverter.ToString(image.GetPropertyItem(propertyId).Value);

                        string StringValue = BitConverter.ToString
                                (image.GetPropertyItem(propertyId).Value).Substring(0, 1);

                        metadata.ExposureCompensation.DisplayValue =
                            StringValue + " (Needs work to confirm accuracy)";
                    }
                }
                catch (Exception exc)
                {
                    // if it is the expected error, do nothing
                    if (exc.GetType().ToString() != "System.ArgumentNullException")
                    {
                    }
                }
                finally
                {
                    index++;
                }
            }


            metadata.XResolution.DisplayValue =
                image.HorizontalResolution.ToString();

            metadata.YResolution.DisplayValue =
                image.VerticalResolution.ToString();

            metadata.ImageHeight.DisplayValue =
                image.Height.ToString();

            metadata.ImageWidth.DisplayValue =
                image.Width.ToString();

            return metadata;
        }
    }
}

  //private void button1_Click(object sender,
  //    System.EventArgs e)
  //{
  //    string MyPicture = "D:\\My
  //        Documents\\My Pictures\\DCP02560.class="highlight2">JPG";

  //     // create an instance of the class
  //     Utilities.ExifMetadata
  //         MyExifMetadata = new Utilities.ExifMetadata();

  //     // create an instance of the
  //     Metadata struct
  //                  Utilities.ExifMetadata.Metadata
  //                                MyMetadata;

  //                  // Populate the instance of the
  //                  struct by calling GetExifMetadata
  //                                MyMetadata =
  //                      MyExifMetadata.GetExifMetadata(MyPicture);

  //                      textBox1.Clear();
  //                      textBox1.AppendText("Equipment
  //                      Make: " + MyMetadata.EquipmentMake.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Camera
  //                      Model: " + MyMetadata.CameraModel.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Date Picture
  //                      Taken: " + MyMetadata.DatePictureTaken.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Exposure
  //                      Compensation: " +
  //                      MyMetadata.ExposureCompensation.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Exposure
  //                      Time: " + MyMetadata.ExposureTime.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Flash: " +
  //                      MyMetadata.Flash.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Fstop: " +
  //                      MyMetadata.Fstop.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Metering
  //                      Mode: " + MyMetadata.MeteringMode.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Shutter
  //                      Speed: " + MyMetadata.ShutterSpeed.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("X
  //                      Resolution: " + MyMetadata.XResolution.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Y
  //                      Resolution: " + MyMetadata.YResolution.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Image
  //                      Width: " + MyMetadata.ImageWidth.DisplayValue);
  //                      textBox1.AppendText("\r\n");
  //                      textBox1.AppendText("Image
  //                      Height: " + MyMetadata.ImageHeight.DisplayValue);
  //		}