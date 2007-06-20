using Habanero.Bo.Loaders;
using Habanero.Generic;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
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
							<uiFormColumn width=""123"">
								<uiFormProperty label=""testlabel1"" propertyName=""testpropname1"" controlTypeName=""Button"" mapperTypeName=""testmappertypename1"" />
								<uiFormProperty label=""testlabel2"" propertyName=""testpropname2"" controlTypeName=""Button"" mapperTypeName=""testmappertypename2"" />
							</uiFormColumn>");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(123, col.Width);
            Assert.AreEqual("testlabel1", col[0].Label);
            Assert.AreEqual("testlabel2", col[1].Label);
        }
    }
}