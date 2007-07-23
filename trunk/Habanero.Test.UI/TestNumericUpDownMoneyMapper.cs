using System.Collections;
using System.Windows.Forms;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestNumericUpDownMoneyMapper : TestMapperBase
    {
        private NumericUpDown itsNumUpDown;
        private NumericUpDownMoneyMapper mapper;
        private Sample s;

        public TestNumericUpDownMoneyMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            itsNumUpDown = new NumericUpDown();
            mapper = new NumericUpDownMoneyMapper(itsNumUpDown, "SampleMoney", false);
            s = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(itsNumUpDown, mapper.Control);
            Assert.AreSame("SampleMoney", mapper.PropertyName);
            Assert.AreEqual(2, itsNumUpDown.DecimalPlaces);
            Assert.AreEqual(int.MinValue, itsNumUpDown.Minimum);
            Assert.AreEqual(int.MaxValue, itsNumUpDown.Maximum);
        }

        [Test]
        public void TestControlValue()
        {
            s.SampleMoney = (decimal) 10.32;
            mapper.BusinessObject = s;
            Assert.AreEqual(10.32, itsNumUpDown.Value, "Value is not set.");
            s.SampleMoney = (decimal) 20.56;
            Assert.AreEqual(20.56, itsNumUpDown.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingControlValueUpdatesBO()
        {
            s.SampleMoney = (decimal) 13.45;
            mapper.BusinessObject = s;
            itsNumUpDown.Value = (decimal) 15.67;
            Assert.AreEqual(15.67, s.SampleMoney, "BO property value isn't changed when numupdown value is changed.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("4.32");
            mapper = new NumericUpDownMoneyMapper(itsNumUpDown, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual(4.32, itsNumUpDown.Value);
        }

        [Test]
        public void TestSettingAttributes()
        {
            Hashtable attributes = new Hashtable();
            attributes.Add("decimalPlaces", "4");
            mapper.SetPropertyAttributes(attributes);
            Assert.AreEqual(4, itsNumUpDown.DecimalPlaces);
        }
    }
}