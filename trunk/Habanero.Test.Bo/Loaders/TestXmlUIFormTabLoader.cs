using Habanero.Bo.Loaders;
using Habanero.Base;
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
						<tab name=""testname"">
							<columnLayout>
								<field label=""testlabel1"" property=""testpropname1"" />
								<field label=""testlabel2"" property=""testpropname2"" />
							</columnLayout>
							<columnLayout>
								<field label=""testlabel3"" property=""testpropname3"" />
							</columnLayout>
						</tab>");
            Assert.AreEqual("testname", col.Name);
            Assert.AreEqual(2, col.Count, "There should be one column.");
            Assert.AreEqual(2, col[0].Count, "There should be two props in column 1");
            Assert.AreEqual(1, col[1].Count, "There should be one prop in column 2");
            Assert.AreEqual("testlabel1", col[0][0].Label);
            Assert.AreEqual("testlabel3", col[1][0].Label);
        }


//        [Test]
//        public void TestLoadWithGrid()
//        {
//            UIFormTab col =
//                loader.LoadUIFormTab(
//                    @"
//						<tab name=""testname"">
//							<uiFormGrid relationshipName=""test"" correspondingRelationshipName=""testCor"" />
//						</tab>");
//            Assert.IsNotNull(col.UIFormGrid);
//            Assert.AreEqual("test", col.UIFormGrid.RelationshipName);
//            Assert.AreEqual("testCor", col.UIFormGrid.CorrespondingRelationshipName);
//        }

        //TODO: make sure that having both a grid and formproperties in the same tab causes errror.
    }
}