using Habanero.Bo.Loaders;
using Habanero.Generic;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormPropertyLoader
    {
        private XmlUIFormPropertyLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormPropertyLoader();
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            UIFormProperty uiProp =
                loader.LoadUIProperty(
                    @"<uiFormProperty label=""testlabel"" propertyName=""testpropname"" controlTypeName=""Button"" mapperTypeName=""testmappertypename"" isReadOnly=""true"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("Button", uiProp.ControlType.Name);
            Assert.AreEqual("testmappertypename", uiProp.MapperTypeName);
            Assert.AreEqual(true, uiProp.IsReadOnly);
        }

        [Test]
        public void TestDefaults()
        {
            UIFormProperty uiProp =
                loader.LoadUIProperty(@"<uiFormProperty label=""testlabel"" propertyName=""testpropname"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("TextBox", uiProp.ControlType.Name);
            Assert.AreEqual("TextBoxMapper", uiProp.MapperTypeName);
            Assert.AreEqual(false, uiProp.IsReadOnly);
        }


        [Test]
        public void TestPropertyAttributes()
        {
            UIFormProperty uiProp =
                loader.LoadUIProperty(
                    @"<uiFormProperty label=""testlabel"" propertyName=""testpropname"" ><uiFormPropertyAtt name=""TestAtt"" value=""TestValue"" /><uiFormPropertyAtt name=""TestAtt2"" value=""TestValue2"" /></uiFormProperty>");
            Assert.AreEqual("TestValue", uiProp.GetAttributeValue("TestAtt"));
            Assert.AreEqual("TestValue2", uiProp.GetAttributeValue("TestAtt2"));
        }
    }
}