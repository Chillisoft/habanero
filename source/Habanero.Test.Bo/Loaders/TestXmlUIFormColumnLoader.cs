using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormColumnLoader
    {
        private XmlUIFormColumnLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormColumnLoader();
        }

        [Test]
        public void TestLoadColumn()
        {
            UIFormColumn col =
                loader.LoadUIFormColumn(
                    @"
							<columnLayout width=""123"">
								<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
								<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
							</columnLayout>");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(123, col.Width);
            Assert.AreEqual("testlabel1", col[0].Label);
            Assert.AreEqual("testlabel2", col[1].Label);
        }
    }
}