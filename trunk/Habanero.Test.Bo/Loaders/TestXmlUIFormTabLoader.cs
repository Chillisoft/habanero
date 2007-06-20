using Habanero.Bo.Loaders;
using Habanero.Generic;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlFormTabLoader
    {
        private XmlUIFormTabLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormTabLoader();
        }

        [Test]
        public void TestLoadColumn()
        {
            UIFormTab col =
                loader.LoadUIFormTab(
                    @"
						<uiFormTab name=""testname"">
							<uiFormColumn>
								<uiFormProperty label=""testlabel1"" propertyName=""testpropname1"" />
								<uiFormProperty label=""testlabel2"" propertyName=""testpropname2"" />
							</uiFormColumn>
							<uiFormColumn>
								<uiFormProperty label=""testlabel3"" propertyName=""testpropname3"" />
							</uiFormColumn>
						</uiFormTab>");
            Assert.AreEqual("testname", col.Name);
            Assert.AreEqual(2, col.Count, "There should be one column.");
            Assert.AreEqual(2, col[0].Count, "There should be two props in column 1");
            Assert.AreEqual(1, col[1].Count, "There should be one prop in column 2");
            Assert.AreEqual("testlabel1", col[0][0].Label);
            Assert.AreEqual("testlabel3", col[1][0].Label);
        }


        [Test]
        public void TestLoadWithGrid()
        {
            UIFormTab col =
                loader.LoadUIFormTab(
                    @"
						<uiFormTab name=""testname"">
							<uiFormGrid relationshipName=""test"" correspondingRelationshipName=""testCor"" />
						</uiFormTab>");
            Assert.IsNotNull(col.UIFormGrid);
            Assert.AreEqual("test", col.UIFormGrid.RelationshipName);
            Assert.AreEqual("testCor", col.UIFormGrid.CorrespondingRelationshipName);
        }

        //TODO: make sure that having both a grid and formproperties in the same tab causes errror.
    }
}