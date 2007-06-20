using System.Windows.Forms;
using Chillisoft.Test.General.v2;
using Chillisoft.Test.UI.BOControls.v2;
using Habanero.Ui.BoControls;
using NUnit.Framework;

namespace Chillisoft.Test.UI.BOControls.v2
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestTextBoxMapper : TestMapperBase
    {
        private TextBox tb;
        private TextBoxMapper mapper;
        private Shape sh;


        [SetUp]
        public void SetupTest()
        {
            tb = new TextBox();
            mapper = new TextBoxMapper(tb, "ShapeName", false);
            sh = new Shape();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(tb, mapper.Control);
            Assert.AreSame("ShapeName", mapper.PropertyName);
        }

        [Test]
        public void TestBusinessObject()
        {
            mapper.BusinessObject = sh;
            Assert.AreSame(sh, mapper.BusinessObject);
        }

        [Test]
        public void TestValueWhenSettingBO()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            Assert.AreEqual("TestShapeName", tb.Text, "TextBox value is not set when bo is set.");
        }

        [Test]
        public void TestValueWhenUpdatingBOValue()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            sh.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName2", tb.Text, "Text property of textbox is not working.");
        }


        [Test]
        public void TestSettingToAnotherBusinessObject()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            Shape sh2 = new Shape();
            sh2.ShapeName = "Different";
            mapper.BusinessObject = sh2;
            Assert.AreEqual("Different", tb.Text, "Setting to another bo doesn't work.");
            sh.ShapeName = "TestShapeName2";
            Assert.AreEqual("Different", tb.Text,
                            "Setting to another bo doesn't remove the property updating event handler of the first.");
        }

        [Test]
        public void TestSettingTextBoxValueUpdatesBO()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            tb.Text = "Changed";
            Assert.AreEqual("Changed", sh.ShapeName, "BO property value isn't changed when textbox text is changed.");
        }

        [Test]
        public void TestSettingTextBoxValueWhenBOIsNotSet()
        {
            sh.ShapeName = "TestShapeName";
            tb.Text = "Changed";
            Assert.AreEqual("TestShapeName", sh.ShapeName,
                            "BO property value shouldn't change since bo of mapper wasn't set.");
            Assert.AreEqual("Changed", tb.Text, "Textbox value shouldn't change.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("MyValue");
            mapper = new TextBoxMapper(tb, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual("MyValue", tb.Text);
        }
    }
}