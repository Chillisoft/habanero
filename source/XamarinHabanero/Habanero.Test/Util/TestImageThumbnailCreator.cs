#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion

using System.Drawing;
using System.Resources;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestImageThumbnailCreator
    {
        private ResourceManager _resourceManager;

        [SetUp]
        public void SetUpResources()
        {
            _resourceManager = new ResourceManager("Habanero.Test.TestResources", typeof(TestImageThumbnailCreator).Assembly);
        }

        [Test]
        public void TestThumbnailCreation()
        {
            Image oldImage = (Image)_resourceManager.GetObject("TestPhoto");
            Assert.IsNotNull(oldImage);
            Assert.AreEqual(100, oldImage.Height);
            Assert.AreEqual(100, oldImage.Width);

            ImageThumbnailCreator creator = new ImageThumbnailCreator();

            Image newSmallImage = creator.CreateThumbnail(oldImage, 50);
            Assert.AreEqual(50, newSmallImage.Height);
            Assert.AreEqual(50, newSmallImage.Width);

            Image newLargeImage = creator.CreateThumbnail(oldImage, 200);
            Assert.AreEqual(200, newLargeImage.Height);
            Assert.AreEqual(200, newLargeImage.Width);
        }

        [Test]
        public void TestForHeightLargerThanWidth()
        {
            Image oldImage = (Image)_resourceManager.GetObject("TestJpeg2");
            Assert.IsNotNull(oldImage);
            Assert.AreEqual(10, oldImage.Height);
            Assert.AreEqual(5, oldImage.Width);

            ImageThumbnailCreator creator = new ImageThumbnailCreator();

            Image newSmallImage = creator.CreateThumbnail(oldImage, 2);
            Assert.AreEqual(2, newSmallImage.Height);
            Assert.AreEqual(4, newSmallImage.Width);

            Image newLargeImage = creator.CreateThumbnail(oldImage, 20);
            Assert.AreEqual(20, newLargeImage.Height);
            Assert.AreEqual(40, newLargeImage.Width);
        }

        [Test]
        public void TestForWidthLargerThanHeight()
        {
            Image oldImage = (Image)_resourceManager.GetObject("TestJpeg3");
            Assert.IsNotNull(oldImage);
            Assert.AreEqual(5, oldImage.Height);
            Assert.AreEqual(10, oldImage.Width);

            ImageThumbnailCreator creator = new ImageThumbnailCreator();

            Image newSmallImage = creator.CreateThumbnail(oldImage, 2);
            Assert.AreEqual(2, newSmallImage.Height);
            Assert.AreEqual(4, newSmallImage.Width);

            Image newLargeImage = creator.CreateThumbnail(oldImage, 20);
            Assert.AreEqual(20, newLargeImage.Height);
            Assert.AreEqual(40, newLargeImage.Width);
        }
    }
}
