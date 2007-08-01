using System.Drawing;
using System.Resources;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    [TestFixture]
    public class TestImageThumbnailCreator
    {
        private ResourceManager _resourceManager;

        [SetUp]
        public void SetUpResources()
        {
            _resourceManager = new ResourceManager("Habanero.Test.TestResources", typeof(TestJpgMetaData).Assembly);
        }

        [Test]
        public void TestThumbnailCreation()
        {
            Image oldImage = (Image)_resourceManager.GetObject("TestPhoto");
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
    }
}
