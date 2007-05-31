using System.Windows.Forms;
using Chillisoft.Test.General.v2;
using Chillisoft.UI.BOControls.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.BOControls.v2
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestNumericUpDownIntegerMapper : TestMapperBase
    {
        private NumericUpDown itsNumUpDown;
        private NumericUpDownIntegerMapper mapper;
        private Sample s;

        public TestNumericUpDownIntegerMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            itsNumUpDown = new NumericUpDown();
            mapper = new NumericUpDownIntegerMapper(itsNumUpDown, "SampleInt", false);
            s = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(itsNumUpDown, mapper.Control);
            Assert.AreSame("SampleInt", mapper.PropertyName);
            Assert.AreEqual(0, itsNumUpDown.DecimalPlaces);
            Assert.AreEqual(int.MinValue, itsNumUpDown.Minimum);
            Assert.AreEqual(int.MaxValue, itsNumUpDown.Maximum);
        }

        [Test]
        public void TestControlValue()
        {
            s.SampleInt = 10;
            mapper.BusinessObject = s;
            Assert.AreEqual(10, itsNumUpDown.Value, "Value is not set.");
            s.SampleInt = 20;
            Assert.AreEqual(20, itsNumUpDown.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingControlValueUpdatesBO()
        {
            s.SampleInt = 12;
            mapper.BusinessObject = s;
            itsNumUpDown.Value = 13;
            Assert.AreEqual(13, s.SampleInt, "BO property value isn't changed when numupdown value is changed.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("4");
            mapper = new NumericUpDownIntegerMapper(itsNumUpDown, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual(4, itsNumUpDown.Value);
        }
    }
}