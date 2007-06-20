using Habanero.Bo.Loaders;
using Habanero.Generic;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIColllectionsLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIDefLoader
    {
        private XmlUIDefLoader itsLoader;

        [SetUp]
        public void Setup()
        {
            itsLoader = new XmlUIDefLoader();
        }

        [Test]
        public void TestLoadWithJustForm()
        {
            UIDef def =
                itsLoader.LoadUIDef(
                    @"
				<uiDef name=""defTestName1"">
					<uiFormDef>
						<uiFormTab name=""testtab"">
							<uiFormColumn>
								<uiFormProperty label=""testlabel1"" propertyName=""testpropname1"" controlTypeName=""Button"" mapperTypeName=""testmappertypename1"" />
								<uiFormProperty label=""testlabel2"" propertyName=""testpropname2"" controlTypeName=""Button"" mapperTypeName=""testmappertypename2"" />
							</uiFormColumn>
						</uiFormTab>
					</uiFormDef>
				</uiDef> 
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
				<uiDef name=""defTestName1"">
					<uiGridDef>
						<uiGridProperty heading=""testheading1"" propertyName=""testpropname1""  />
						<uiGridProperty heading=""testheading2"" propertyName=""testpropname2""  />
						<uiGridProperty heading=""testheading3"" propertyName=""testpropname3""  />
					</uiGridDef>
				</uiDef> 
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
				<uiDef name=""defTestName1"">
					<uiGridDef>
						<uiGridProperty heading=""testheading1"" propertyName=""testpropname1""  />
						<uiGridProperty heading=""testheading2"" propertyName=""testpropname2""  />
						<uiGridProperty heading=""testheading3"" propertyName=""testpropname3""  />
					</uiGridDef>
					<uiFormDef>
						<uiFormTab name=""testtab"">
							<uiFormColumn>
								<uiFormProperty label=""testlabel1"" propertyName=""testpropname1"" controlTypeName=""Button"" mapperTypeName=""testmappertypename1"" />
								<uiFormProperty label=""testlabel2"" propertyName=""testpropname2"" controlTypeName=""Button"" mapperTypeName=""testmappertypename2"" />
							</uiFormColumn>
						</uiFormTab>
					</uiFormDef>
				</uiDef> 
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
				<uiDef name=""defTestName1"">
					<uiGridDef>
						<uiGridProperty heading=""testheading1"" propertyName=""testpropname1""  />
						<uiGridProperty heading=""testheading2"" propertyName=""testpropname2""  />
						<uiGridProperty heading=""testheading3"" propertyName=""testpropname3""  />
					</uiGridDef>
				</uiDef> ");
            Assert.AreEqual("defTestName1", def.Name);
        }
    }
}