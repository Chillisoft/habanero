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
					<uiFormDef width=""100"" height=""120"" heading=""testheading"">
						<uiFormTab name=""testtab"">
							<uiFormColumn>
								<uiFormProperty label=""testlabel1"" propertyName=""testpropname1"" controlTypeName=""Button"" mapperTypeName=""testmappertypename1"" />
								<uiFormProperty label=""testlabel2"" propertyName=""testpropname2"" controlTypeName=""Button"" mapperTypeName=""testmappertypename2"" />
							</uiFormColumn>
						</uiFormTab>
					</uiFormDef>");
            //Assert.AreEqual(typeof(MyBo), col.Class);
            //Assert.AreEqual("Habanero.Test.Bo.MyBo_testName", col.Name.ToString() );
            Assert.AreEqual(1, col.Count, "There should be 1 tab"); // 1 tab
            Assert.AreEqual(1, col[0].Count, "There should be one column in that tab");
            Assert.AreEqual(2, col[0][0].Count, "There should be two propertys in that column");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][0][1].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Heading);
        }
    }
}