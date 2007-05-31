using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a control that displays a picture
    /// </summary>
    /// TODO ERIC - there's no functionality here to set the picture to
    /// display apart from logo.jpg.  Add get/set for picture and constructor
    /// to set picture path.
    public class PictureControl : UserControl
    {
        private bool itsFitToScreen;
        private Image itsPicture;
        private PictureBox itsPictureControl;
        private bool itsIsPictureLoaded;

        /// <summary>
        /// Constructor to initialise the control
        /// </summary>
        /// <param name="fitToScreen">Whether to stretch the image to fill
        /// the space in which the control is located, or rather to
        /// center the image</param>
        /// TODO ERIC - rename fitToScreen to stretchToFit
        public PictureControl(bool fitToScreen)
        {
            try
            {
                itsPicture = Image.FromFile("logo.jpg");
                itsIsPictureLoaded = true;
            }
            catch (FileNotFoundException)
            {
                itsIsPictureLoaded = false;
            }
            itsPictureControl = new PictureBox();

            itsPictureControl.Image = itsPicture;
            this.FitToScreen = fitToScreen;

            this.Controls.Add(itsPictureControl);
            this.Resize += new EventHandler(ResizeHandler);
            ResizePicBox();
        }

        /// <summary>
        /// Handles the event of the control being resized
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ResizeHandler(object sender, EventArgs e)
        {
            this.ResizePicBox();
        }

        /// <summary>
        /// Gets and sets the attribute that determines whether to stretch
        /// the image to fill the space provided, or rather to centre it
        /// </summary>
        /// TODO ERIC - rename to StretchToFit
        public bool FitToScreen
        {
            get { return itsFitToScreen; }
            set
            {
                itsFitToScreen = value;
                if (itsFitToScreen)
                {
                    itsPictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    itsPictureControl.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                ResizePicBox();
            }
        }

        /// <summary>
        /// Resizes the picture box
        /// </summary>
        public void ResizePicBox()
        {
            if (itsIsPictureLoaded)
            {
                if (itsFitToScreen)
                {
                    int width = itsPicture.Width;
                    int height = itsPicture.Height;
                    int controlWidth = this.Width;
                    int controlHeight = this.Height;
                    double wr = controlWidth > 0 ? width/controlWidth : 0;
                    double hr = controlHeight > 0 ? height/controlHeight : 0;
                    if (wr > 1.0 && wr > hr)
                    {
                        itsPictureControl.Width = (int) (width*1/wr);
                        itsPictureControl.Height = (int) (height*1/wr);
                    }
                    else if (hr > 1.0)
                    {
                        itsPictureControl.Width = (int) (width*1/hr);
                        itsPictureControl.Height = (int) (height*1/hr);
                    }
                    itsPictureControl.Left = (int) (this.Width/2 - (itsPictureControl.Width/2));
                    itsPictureControl.Top = (int) (this.Height/2 - (itsPictureControl.Height/2));
                }
                else
                {
                    itsPictureControl.Left = 0;
                    itsPictureControl.Top = 0;
                    itsPictureControl.Height = this.Height;
                    itsPictureControl.Width = this.Width;
                }
            }
        }
    }
}