using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormGridLoader
    {
        private XmlUIFormGridLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormGridLoader();
        }

		// TODO: This test uses Habanero.UI.Pro, please investigate if this is necessary
		// and then fix this so that it can be uncommented ( - Mark (2007/07/26) )
		//[Test]
		//public void TestSimpleGrid()
		//{
		//    UIFormGrid formGrid =
		//        loader.LoadUIFormGrid(
		//            @"<formGrid relationship=""testrelationshipname"" reverseRelationship=""testcorrespondingrelationshipname"" />");
		//    Assert.AreEqual("testrelationshipname", formGrid.RelationshipName);
		//    Assert.AreEqual("testcorrespondingrelationshipname", formGrid.CorrespondingRelationshipName);
		//    Assert.AreEqual(typeof (EditableGrid), formGrid.GridType);
		//}
    }
}