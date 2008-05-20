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
            const string INT_PROP_NAME = "SampleInt";
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false);

            //---------------Test Result -----------------------
            Assert.AreSame(numUpDown, mapper.Control);
            Assert.AreSame(INT_PROP_NAME, mapper.PropertyName);
            Assert.AreEqual(0, numUpDown.DecimalPlaces);
            Assert.AreEqual(int.MinValue, numUpDown.Minimum);
            Assert.AreEqual(int.MaxValue, numUpDown.Maximum);

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected override void UpdateControlValueFromBo()
        {
            throw new NotImplementedException();
        }
    }
}
