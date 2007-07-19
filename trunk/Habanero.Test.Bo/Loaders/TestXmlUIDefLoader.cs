using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIColllectionsLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIDefLoader
    {
        private XmlUILoader itsLoader;

        [SetUp]
        public void Setup()
        {
            itsLoader = new XmlUILoader();
        }

        [Test]
        public void TestLoadWithJustForm()
        {
            UIDef def =
                itsLoader.LoadUIDef(
                    @"
				<ui name=""defTestName1"">
					<form>
						<tab name=""testtab"">
							<columnLayout>
								<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
								<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
							</columnLayout>
						</tab>
					</form>
				</ui> 
							");
            Assert.IsNotNull(def.UIFormDef);
            Assert.AreEqual(1, def.UIFormDef.Count);
        }

        [Test]
        public void TestLoadWithJustGrid()
        {
            UIDef def =
                itsLoader.LoadUIDef(
                    @"
				<ui name=""defTestName1"">
					<grid>
						<column heading=""testheading1"" property=""testpropname1""  />
						<column heading=""testheading2"" property=""testpropname2""  />
						<column heading=""testheading3"" property=""testpropname3""  />
					</grid>
				</ui> 
							");
            Assert.IsNotNull(def.UIGridDef);
            Assert.AreEqual(3, def.UIGridDef.Count);
        }

        [Test]
        public void TestLoadWithBothGridAndForm()
        {
            UIDef def =
                itsLoader.LoadUIDef(
                    @"
				<ui name=""defTestName1"">
					<grid>
						<column heading=""testheading1"" property=""testpropname1""  />
						<column heading=""testheading2"" property=""testpropname2""  />
						<column heading=""testheading3"" property=""testpropname3""  />
					</grid>
					<form>
						<tab name=""testtab"">
							<columnLayout>
								<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
								<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
							</columnLayout>
						</tab>
					</form>
				</ui> 
							");
            Assert.IsNotNull(def.UIFormDef);
            Assert.AreEqual(1, def.UIFormDef.Count);
            Assert.IsNotNull(def.UIGridDef);
            Assert.AreEqual(3, def.UIGridDef.Count);
        }

        [Test]
        public void TestName()
        {
            UIDef def =
                itsLoader.LoadUIDef(
                    @"
				<ui name=""defTestName1"">
					<grid>
						<column heading=""testheading1"" property=""testpropname1""  />
						<column heading=""testheading2"" property=""testpropname2""  />
						<column heading=""testheading3"" property=""testpropname3""  />
					</grid>
				</ui> ");
            Assert.AreEqual("defTestName1", def.Name);
        }
    }
}