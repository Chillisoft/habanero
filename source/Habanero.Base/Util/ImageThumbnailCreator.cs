//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

namespace Habanero.Util
{
    /// <summary>
    /// Creates image thumbnails
    /// </summary>
    /// TODO: What is the intention with the height/width inversion for h>w ?
    public class ImageThumbnailCreator
    {
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