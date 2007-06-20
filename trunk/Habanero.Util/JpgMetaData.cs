using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Habanero.Util
{
    /// <summary>
    /// Provides facilities of obtaining metadata from a jpeg image
    /// </summary>
    public class JpgMetaData
    {
        /// <summary>
        /// Constructor to initialise a new data object
        /// </summary>
        public JpgMetaData()
        {
        }

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
            public MetadataDetail
                EquipmentMake;

            public MetadataDetail CameraModel;

            public MetadataDetail
                ExposureTime;

            public MetadataDetail Fstop;

            public MetadataDetail
                DatePictureTaken;

            public MetadataDetail
                ShutterSpeed;

            public MetadataDetail
                ExposureCompensation;

            public MetadataDetail
                MeteringMode;

            public MetadataDetail Flash;
            public MetadataDetail XResolution;
            public MetadataDetail YResolution;
            public MetadataDetail ImageWidth;
            public MetadataDetail ImageHeight;
        }

        /// <summary>
        /// Returns the exif value for the specified arguments
        /// </summary>
        /// <param name="Description">The exif description</param>
        /// <param name="Value">The exif value type</param>
        /// <returns>Returns a string</returns>
        public string LookupExifValue(string
                                          Description, string Value)
        {
            string DescriptionValue = null;

            if (Description == "MeteringMode")
            {
                switch (Value)
                {
                    case "0":

                        DescriptionValue = "Unknown";
                        break;
                    case "1":

                        DescriptionValue = "Average";
                        break;
                    case "2":

                        DescriptionValue = "Center Weighted Average";
                        break;
                    case "3":

                        DescriptionValue = "Spot";
                        break;
                    case "4":

                        DescriptionValue = "Multi-spot";
                        break;
                    case "5":

                        DescriptionValue = "Multi-segment";
                        break;
                    case "6":

                        DescriptionValue = "Partial";
                        break;
                    case "255":

                        DescriptionValue = "Other";
                        break;
                }
            }

            if (Description
                == "ResolutionUnit")
            {
                switch (Value)
                {
                    case "1":

                        DescriptionValue = "No Units";
                        break;
                    case "2":

                        DescriptionValue = "Inch";
                        break;
                    case "3":

                        DescriptionValue = "Centimeter";
                        break;
                }
            }

            if (Description == "Flash")
            {
                switch (Value)
                {
                    case "0":

                        DescriptionValue = "Flash did not fire";
                        break;
                    case "1":

                        DescriptionValue = "Flash fired";
                        break;
                    case "5":

                        DescriptionValue = "Flash fired but strobe return light not detected";
                        break;
                    case "7":

                        DescriptionValue = "Flash fired and strobe return light detected";
                        break;
                }
            }
            return DescriptionValue;
        }

        /// <summary>
        /// Returns a Metadata object with exif metadata for the given photo
        /// </summary>
        /// <param name="PhotoName">The photo</param>
        /// <returns>Returns a Metadata object</returns>
        public Metadata GetExifMetadata(string
                                            PhotoName)
        {
            // Create an instance of the image to gather metadata from 
            Image MyImage = Image.FromFile
                (PhotoName);

            // Create an integer array to hold the property id list,
            // and populate it with the list from my image.
            /* Note: this only generates a
										 list of integers, one for for each PropertyID.
													  * We will populate the
										 PropertyItem values later. */
            int[] MyPropertyIdList =
                MyImage.PropertyIdList;

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
            PropertyItem[] MyPropertyItemList
                = new PropertyItem[MyPropertyIdList.Length];

            // Create an instance of Metadata and populate Hex codes (values populated later)
            Metadata MyMetadata = new Metadata
                ();
            MyMetadata.EquipmentMake.Hex
                = "10f";
            MyMetadata.CameraModel.Hex
                = "110";
            MyMetadata.DatePictureTaken.Hex
                = "9003";
            MyMetadata.ExposureTime.Hex
                = "829a";
            MyMetadata.Fstop.Hex = "829d";
            MyMetadata.ShutterSpeed.Hex
                = "9201";

            MyMetadata.ExposureCompensation.Hex = "9204";
            MyMetadata.MeteringMode.Hex
                = "9207";
            MyMetadata.Flash.Hex = "9209";

            // Declare an ASCIIEncoding to use for returning string values from bytes
            ASCIIEncoding Value =
                new ASCIIEncoding();

            // Populate MyPropertyItemList.
            // For each propertyID...
            int index = 0;
            foreach (int MyPropertyId in
                MyPropertyIdList)
            {
                // ... try to call GetPropertyItem (it crashes if PropertyItem has length 0, so use Try/Catch)
                try
                {
                    // Assign the image's PropertyItem to the PropertyItem array
                    MyPropertyItemList
                        [index] = MyImage.GetPropertyItem(MyPropertyId);

                    // Troublshooting
                    /*
                
																					textBox1.AppendText("\r\n\t" +
																				BitConverter.ToString(MyImage.GetPropertyItem
																				(MyPropertyId).Value));
                
																					textBox1.AppendText("\r\n\thex location: " +
																				MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x"));
																									*/

                    // Assign each element of MyMetadata
                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "10f") // EquipmentMake
                    {
                        MyMetadata.EquipmentMake.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        MyMetadata.EquipmentMake.DisplayValue =
                            Value.GetString(MyPropertyItemList[index].Value);
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "110") // CameraModel
                    {
                        MyMetadata.CameraModel.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        MyMetadata.CameraModel.DisplayValue =
                            Value.GetString(MyPropertyItemList[index].Value);
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "9003") // DatePictureTaken
                    {
                        MyMetadata.DatePictureTaken.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        MyMetadata.DatePictureTaken.DisplayValue =
                            Value.GetString(MyPropertyItemList[index].Value);
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "9207") // MeteringMode
                    {
                        MyMetadata.MeteringMode.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        MyMetadata.MeteringMode.DisplayValue =
                            LookupExifValue("MeteringMode", BitConverter.ToInt16
                                                                (MyImage.GetPropertyItem(MyPropertyId).Value, 0).
                                                                ToString
                                                                ());
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "9209") // Flash
                    {
                        MyMetadata.Flash.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        MyMetadata.Flash.DisplayValue = LookupExifValue
                            ("Flash", BitConverter.ToInt16(MyImage.GetPropertyItem
                                                               (MyPropertyId).Value, 0).ToString());
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "829a") // ExposureTime
                    {
                        MyMetadata.ExposureTime.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        string
                            StringValue = "";
                        for (int
                                 Offset = 0;
                             Offset < MyImage.GetPropertyItem
                                          (MyPropertyId).Len;
                             Offset = Offset + 4)
                        {
                            StringValue += BitConverter.ToInt32
                                               (MyImage.GetPropertyItem
                                                    (MyPropertyId).Value, Offset).ToString() + "/";
                        }

                        MyMetadata.ExposureTime.DisplayValue =
                            StringValue.Substring(0, StringValue.Length - 1);
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "829d") // F-stop
                    {
                        MyMetadata.Fstop.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        int int1;
                        int int2;
                        int1 =
                            BitConverter.ToInt32(MyImage.GetPropertyItem
                                                     (MyPropertyId).Value, 0);
                        int2 =
                            BitConverter.ToInt32(MyImage.GetPropertyItem
                                                     (MyPropertyId).Value, 4);

                        MyMetadata.Fstop.DisplayValue = "F/" +
                                                        (int1/int2);
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "9201") // ShutterSpeed
                    {
                        MyMetadata.ShutterSpeed.RawValueAsString =
                            BitConverter.ToString(MyImage.GetPropertyItem
                                                      (MyPropertyId).Value);

                        string
                            StringValue = BitConverter.ToString
                                (MyImage.GetPropertyItem(MyPropertyId).Value).Substring
                                (0, 2);

                        MyMetadata.ShutterSpeed.DisplayValue = "1/" +
                                                               StringValue;
                    }

                    if
                        (MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x")
                         == "9204") // ExposureCompensation
                    {
                        MyMetadata.ExposureCompensation.RawValueAsString
                            = BitConverter.ToString(MyImage.GetPropertyItem
                                                        (MyPropertyId).Value);

                        string
                            StringValue = BitConverter.ToString
                                (MyImage.GetPropertyItem(MyPropertyId).Value).Substring
                                (0, 1);

                        MyMetadata.ExposureCompensation.DisplayValue =
                            StringValue + " (Needs work to confirm accuracy)";
                    }
                }
                catch (Exception exc)
                {
                    // if it is the expected error, do nothing
                    if (exc.GetType
                            ().ToString() != "System.ArgumentNullException")
                    {
                    }
                }
                finally
                {
                    index++;
                }
            }


            MyMetadata.XResolution.DisplayValue =
                MyImage.HorizontalResolution.ToString();

            MyMetadata.YResolution.DisplayValue =
                MyImage.VerticalResolution.ToString();

            MyMetadata.ImageHeight.DisplayValue =
                MyImage.Height.ToString();

            MyMetadata.ImageWidth.DisplayValue =
                MyImage.Width.ToString();

            return MyMetadata;
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