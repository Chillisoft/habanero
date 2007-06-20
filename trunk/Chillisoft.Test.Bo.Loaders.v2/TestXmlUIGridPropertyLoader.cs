using System.Windows.Forms;
using Habanero.Bo.Loaders;
using Habanero.Generic;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIGridPropertyLoader
    {
        private XmlUIGridPropertyLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIGridPropertyLoader();
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            UIGridProperty uiProp =
                loader.LoadUIProperty(
                    @"<uiGridProperty heading=""testheading"" propertyName=""testpropname"" gridControlTypeName=""DataGridViewCheckBoxColumn"" width=""40"" />");
            Assert.AreEqual("testheading", uiProp.Heading);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual(40, uiProp.Width);
            Assert.AreSame(typeof (DataGridViewCheckBoxColumn), uiProp.GridControlType);
        }

        [Test]
        public void TestDefaults()
        {
            UIGridProperty uiProp =
                loader.LoadUIProperty(@"<uiGridProperty heading=""testheading"" propertyName=""testpropname"" />");
            Assert.AreSame(typeof (DataGridViewTextBoxColumn), uiProp.GridControlType);
            Assert.AreEqual(100, uiProp.Width);
        }
    }
}