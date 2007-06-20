using System;
using System.Drawing;

namespace Habanero.Util
{
    /// <summary>
    /// Creates image thumbnails
    /// </summary>
    public class ImageThumbnailCreator
    {
        /// <summary>
        /// Constructor to initialise a new creator
        /// </summary>
        public ImageThumbnailCreator()
        {
        }

        /// <summary>
        /// Creates a thumbnail from the image provided, scaled down to the
        /// new height specified while keeping the aspect ratio
        /// </summary>
        /// <param name="fullImage">The image to replicate</param>
        /// <param name="newHeight">The new height to scale to</param>
        /// <returns>Returns a thumbnail Image</returns>
        public Image CreateThumbnail(Image fullImage, int newHeight)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            int newWidth;
            if (fullImage.Height > fullImage.Width)
            {
                newWidth = Convert.ToInt32(Math.Round((double) ((fullImage.Height/fullImage.Width)*newHeight)));
            }
            else
            {
                newWidth = Convert.ToInt32(Math.Round((double) ((fullImage.Width/fullImage.Height)*newHeight)));
            }

            return fullImage.GetThumbnailImage(newWidth, newHeight, myCallback, IntPtr.Zero);
        }

        /// <summary>
        /// Returns false
        /// </summary>
        /// <returns>Returns false</returns>
        private bool ThumbnailCallback()
        {
            return false;
        }
    }
}