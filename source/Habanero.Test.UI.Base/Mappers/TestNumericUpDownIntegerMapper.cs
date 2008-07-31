//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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

            [Test]
            public void Test_ValueChangedEvent_UpdatesBusinessObject()
            {
                //---------------Set up test pack-------------------
                INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
                NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
                Sample s = new Sample();
                s.SampleInt = 100;
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                int newValue = 555;
                numUpDown.Value = newValue;
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(NumericUpDownMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual(newValue, s.SampleInt);
                //---------------Tear down -------------------------
            }

            
            [Test]
            public void Test_BusinessObjectChanged_UpdatesControl()
            {
                //---------------Set up test pack-------------------
                INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
                NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
                Sample s = new Sample();
                s.SampleInt = 100;
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                int newValue = 555;
                s.SampleInt = newValue;
                //---------------Test Result -----------------------
                Assert.AreEqual(newValue, numUpDown.Value);
                //---------------Tear down -------------------------
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            //---------------Execute Test ----------------------
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());

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
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
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
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
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
}
