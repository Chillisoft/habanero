using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Habanero.Ui.Misc
{
    /// <summary>
    /// Provides a control that displays a picture
    /// </summary>
    /// TODO ERIC - there's no functionality here to set the picture to
    /// display apart from logo.jpg.  Add get/set for picture and constructor
    /// to set picture path.
    public class PictureControl : UserControl
    {
        private bool _stretchToFit;
        private Image _picture;
        private PictureBox _pictureControl;
        private bool _isPictureLoaded;

        /// <summary>
        /// Constructor to initialise the control
        /// </summary>
        /// <param name="stretchToFit">Whether to stretch the image to fill
        /// the space in which the control is located, or rather to
        /// center the image</param>
        public PictureControl(bool stretchToFit)
        {
            try
            {
                _picture = Image.FromFile("logo.jpg");
                _isPictureLoaded = true;
            }
            catch (FileNotFoundException)
            {
                _isPictureLoaded = false;
            }
            _pictureControl = new PictureBox();

            _pictureControl.Image = _picture;
            this.StretchToFit = stretchToFit;

            this.Controls.Add(_pictureControl);
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
        public bool StretchToFit
        {
            get { return _stretchToFit; }
            set
            {
                _stretchToFit = value;
                if (_stretchToFit)
                {
                    _pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    _pictureControl.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                ResizePicBox();
            }
        }

        /// <summary>
        /// Resizes the picture box
        /// </summary>
        public void ResizePicBox()
        {
            if (_isPictureLoaded)
            {
                if (_stretchToFit)
                {
                    int width = _picture.Width;
                    int height = _picture.Height;
                    int controlWidth = this.Width;
                    int controlHeight = this.Height;
                    double wr = controlWidth > 0 ? width/controlWidth : 0;
                    double hr = controlHeight > 0 ? height/controlHeight : 0;
                    if (wr > 1.0 && wr > hr)
                    {
                        _pictureControl.Width = (int) (width*1/wr);
                        _pictureControl.Height = (int) (height*1/wr);
                    }
                    else if (hr > 1.0)
                    {
                        _pictureControl.Width = (int) (width*1/hr);
                        _pictureControl.Height = (int) (height*1/hr);
                    }
                    _pictureControl.Left = (int) (this.Width/2 - (_pictureControl.Width/2));
                    _pictureControl.Top = (int) (this.Height/2 - (_pictureControl.Height/2));
                }
                else
                {
                    _pictureControl.Left = 0;
                    _pictureControl.Top = 0;
                    _pictureControl.Height = this.Height;
                    _pictureControl.Width = this.Width;
                }
            }
        }
    }
}