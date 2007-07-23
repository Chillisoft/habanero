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

        [Test]
        public void TestSimpleGrid()
        {
            UIFormGrid formGrid =
                loader.LoadUIFormGrid(
                    @"<formGrid relationship=""testrelationshipname"" reverseRelationship=""testcorrespondingrelationshipname"" />");
            Assert.AreEqual("testrelationshipname", formGrid.RelationshipName);
            Assert.AreEqual("testcorrespondingrelationshipname", formGrid.CorrespondingRelationshipName);
            Assert.AreEqual(typeof (EditableGrid), formGrid.GridType);
        }
    }
}