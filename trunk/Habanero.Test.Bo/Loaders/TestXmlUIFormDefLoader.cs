using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormDefLoader
    {
        private XmlUIFormDefLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormDefLoader();
        }

        [Test]
        public void TestLoadPropertyCollection()
        {
            UIFormDef col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<tab name=""testtab"">
							<columnLayout>
								<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
								<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
							</columnLayout>
						</tab>
					</form>");
            //Assert.AreEqual(typeof(MyBo), col.Class);
            //Assert.AreEqual("Habanero.Test.Bo.MyBo_testName", col.Name.ToString() );
            Assert.AreEqual(1, col.Count, "There should be 1 tab"); // 1 tab
            Assert.AreEqual(1, col[0].Count, "There should be one column in that tab");
            Assert.AreEqual(2, col[0][0].Count, "There should be two propertys in that column");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][0][1].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }
    }
}