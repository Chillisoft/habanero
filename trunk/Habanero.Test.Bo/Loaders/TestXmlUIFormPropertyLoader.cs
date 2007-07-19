using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormPropertyLoader
    {
        private XmlUIFormFieldLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormFieldLoader();
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(
                    @"<field label=""testlabel"" property=""testpropname"" type=""Button"" mapperType=""testmappertypename"" mapperAssembly=""testmapperassembly"" editable=""false"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("Button", uiProp.ControlType.Name);
            Assert.AreEqual("testmappertypename", uiProp.MapperTypeName);
            Assert.AreEqual("testmapperassembly", uiProp.MapperAssembly);
            Assert.AreEqual(false, uiProp.Editable);
        }

        [Test]
        public void TestDefaults()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(@"<field label=""testlabel"" property=""testpropname"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("TextBox", uiProp.ControlType.Name);
            Assert.AreEqual("TextBoxMapper", uiProp.MapperTypeName);
            Assert.AreEqual(true, uiProp.Editable);
        }


        [Test]
        public void TestPropertyAttributes()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(
                    @"<field label=""testlabel"" property=""testpropname"" ><parameter name=""TestAtt"" value=""TestValue"" /><parameter name=""TestAtt2"" value=""TestValue2"" /></field>");
            Assert.AreEqual("TestValue", uiProp.GetParameterValue("TestAtt"));
            Assert.AreEqual("TestValue2", uiProp.GetParameterValue("TestAtt2"));
        }

    }
}