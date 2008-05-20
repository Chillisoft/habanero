using System;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    public abstract class TestNumericUpDownIntegerMapper
    {
        public abstract IControlFactory GetControlFactory();
        private const string INT_PROP_NAME = "SampleInt";

        [TestFixture]
        public class TestNumericUpDownIntegerMapperGiz : TestNumericUpDownIntegerMapper
        {
            public override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [TestFixture]
        public class TestNumericUpDownIntegerMapperWin : TestNumericUpDownIntegerMapper
        {
            public override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            //---------------Execute Test ----------------------
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false);

            //---------------Test Result -----------------------
            Assert.AreSame(numUpDown, mapper.Control);
            Assert.AreSame(INT_PROP_NAME, mapper.PropertyName);
            Assert.AreEqual(0, numUpDown.DecimalPlaces);
            Assert.AreEqual(int.MinValue, numUpDown.Minimum);
            Assert.AreEqual(int.MaxValue, numUpDown.Maximum);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetBusinessObject()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false);
            Sample s = new Sample();
            s.SampleInt = 100;
            //---------------Execute Test ----------------------
            mapper.BusinessObject = s;
            //---------------Test Result -----------------------
            Assert.AreEqual(100, numUpDown.Value, "Value is not set.");

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestApplyChangesToBO()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false);
            Sample s = new Sample();
            s.SampleInt = 100;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            numUpDown.Value = 200;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(200, s.SampleInt, "Value is not set.");

            //---------------Tear Down -------------------------
        }


    }

    public class NumericUpDownIntegerMapper : ControlMapper
    {
        private readonly INumericUpDown _numericUpDown;
        public NumericUpDownIntegerMapper(IControlChilli ctl, string propName, bool isReadOnly)
            : base(ctl, propName, isReadOnly)
        {
            _numericUpDown = (INumericUpDown) ctl;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
        }

        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(Convert.ToInt32(_numericUpDown.Value));
        }

        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected override void UpdateControlValueFromBo()
        {
            _numericUpDown.Value = Convert.ToDecimal(GetPropertyValue());
        }
    }
}
