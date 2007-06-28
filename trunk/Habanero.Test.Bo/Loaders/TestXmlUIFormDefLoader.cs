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

        [Test]
        public void TestFormWithFields()
        {
            UIFormDef col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
						<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
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

        [Test]
        public void TestFormWithColumns()
        {
            UIFormDef col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<columnLayout>
						    <field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
                        </columnLayout>
                        <columnLayout>
    						<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
                        </columnLayout>
                    </form>");
            //Assert.AreEqual(typeof(MyBo), col.Class);
            //Assert.AreEqual("Habanero.Test.Bo.MyBo_testName", col.Name.ToString() );
            Assert.AreEqual(1, col.Count, "There should be 1 tab"); // 1 tab
            Assert.AreEqual(2, col[0].Count, "There should be one column in that tab");
            Assert.AreEqual(1, col[0][0].Count, "There should be two propertys in column 1");
            Assert.AreEqual(1, col[0][1].Count, "There should be two propertys in column 1");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][1][0].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }

        [Test]
        public void TestFormWithTabsAndNoColumns()
        {
            UIFormDef col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<tab name=""testtab"">
							<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
							<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
						</tab>
						<tab name=""testtab2"">
							<field label=""testlabel3"" property=""testpropname3"" type=""Button"" mapperType=""testmappertypename3"" />
						</tab>
                    </form>");
            Assert.AreEqual(2, col.Count, "There should be 2 tab"); 
            Assert.AreEqual(1, col[0].Count, "There should be one column in tab 1");
            Assert.AreEqual(1, col[0].Count, "There should be one column in tab 2");
            Assert.AreEqual(2, col[0][0].Count, "There should be two propertys in column 1 (tab 1)");
            Assert.AreEqual(1, col[1][0].Count, "There should be two propertys in column 1 (tab 2)");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][0][1].Label);
            Assert.AreEqual("testlabel3", col[1][0][0].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }

    }
}