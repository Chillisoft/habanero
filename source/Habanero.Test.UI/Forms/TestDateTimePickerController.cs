//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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


using System;
using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    [TestFixture]
    public class TestDateTimePickerController
    {
        private DateTimePicker _dateTimePicker;
        private DateTimePickerController _dateTimePickerController;
        private Control _nullDisplayControl;

        [SetUp]
        public void SetupTest()
        {
            _dateTimePicker = new DateTimePicker();
            _dateTimePickerController = new DateTimePickerController(_dateTimePicker);
            if (_dateTimePicker.Controls.Count > 0)
            {
                _nullDisplayControl = _dateTimePicker.Controls[0];
            }
            Assert.IsNotNull(_nullDisplayControl, "The Date Time Picker was not set up correctly.");
        }

        #region Set Value Tests

        [Test]
        public void TestSetPickerValue()
        {
            DateTime sampleDate = new DateTime(2001, 01, 01, 01, 01, 01);
            _dateTimePicker.Value = sampleDate;
            Assert.AreEqual(sampleDate, _dateTimePickerController.Value);
            Assert.IsFalse(_nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
        }

        [Test]
        public void TestSetControllerValue()
        {
            DateTime sampleDate = new DateTime(2002, 02, 02, 02, 02, 02);
            _dateTimePickerController.Value = sampleDate;
            Assert.AreEqual(sampleDate, _dateTimePicker.Value);
            Assert.IsFalse(_nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
        }

        [Test]
        public void TestSetNullValue()
        {
            _dateTimePickerController.Value = null;
            Assert.AreEqual(null, _dateTimePickerController.Value, "The value should be null after it is set to null");
            Assert.IsTrue(_nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestSetNullThenControllerValue()
        {
            TestSetNullValue();
            TestSetControllerValue();
        }
		
        [Test]
        public void TestSetNullThenPickerValue()
        {
            TestSetNullValue();
            TestSetPickerValue();
        }

        #endregion //Set Value Tests

        #region Checkbox Tests

        [Test]
        public void TestSetControllerValueWithCheckbox()
        {
            DateTime sampleDate = new DateTime(2002, 02, 02, 02, 02, 02);
            _dateTimePicker.ShowCheckBox = true;
            _dateTimePickerController.Value = sampleDate;
            Assert.AreEqual(sampleDate, _dateTimePicker.Value);
            Assert.IsTrue(_dateTimePicker.Checked, "Checkbox should be checked when there is a value.");
            Assert.IsFalse(_nullDisplayControl.Visible, "Null display value control should not be visible when the checkbox is visible.");
        }

        [Test]
        public void TestSetNullValueWithCheckbox()
        {
            _dateTimePicker.ShowCheckBox = true;
            _dateTimePickerController.Value = null;
            Assert.IsFalse(_dateTimePicker.Checked, "Checkbox should be unchecked when the value is null.");
            Assert.AreEqual(null, _dateTimePickerController.Value, "The value should be null after it is set to null");
            Assert.IsFalse(_nullDisplayControl.Visible, "Null display value control should not be visible when the checkbox is visible.");
        }

        [Test]
        public void TestSetNullThenControllerValueWithCheckbox()
        {
            TestSetNullValueWithCheckbox();
            TestSetControllerValueWithCheckbox();
        }

        [Test]
        public void TestSetCheckboxChecked()
        {
            DateTime sampleDate = new DateTime(2002, 02, 02, 02, 02, 02);
            _dateTimePickerController.Value = sampleDate;
            TestSetNullValueWithCheckbox();
            _dateTimePicker.Checked = true;
            Assert.AreEqual(sampleDate, _dateTimePickerController.Value,
                            "The value should be restored when the checkbox is checked again.");
        }

        [Test]
        public void TestSetCheckboxUnChecked()
        {
            TestSetControllerValueWithCheckbox();
            _dateTimePicker.Checked = false;
            Assert.AreEqual(null, _dateTimePickerController.Value,
                            "The value should be set to null when the checkbox is unchecked.");
        }

        #endregion //Checkbox Tests
		
        #region Event Tests

        [Test]
        public void TestSetControllerValueFiresEvent()
        {
            DateTime sampleDate = new DateTime(2003, 03, 03, 03, 03, 03);
            bool isFired = false;
            EventHandler handleValueChanged = delegate
                                                  {
                                                      isFired = true;
                                                  };
            _dateTimePickerController.ValueChanged += handleValueChanged;
            _dateTimePickerController.Value = sampleDate;
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            isFired = false;
            _dateTimePickerController.Value = null;
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value to null.");
            _dateTimePickerController.ValueChanged -= handleValueChanged;
        }

        [Test]
        public void TestSetPickerValueFiresEvent()
        {
            DateTime sampleDate = new DateTime(2004, 04, 04, 04, 04, 04);
            bool isFired = false;
            EventHandler handleValueChanged = delegate
                                                  {
                                                      isFired = true;
                                                  };
            _dateTimePickerController.ValueChanged += handleValueChanged;
            _dateTimePicker.Value = sampleDate;
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            _dateTimePickerController.ValueChanged -= handleValueChanged;
        }

        [Test, Ignore("The dateTimePicker does not throw any event when the checked property is changed programatically, so this test will never work")]
        public void TestCheckingCheckboxFiresEvent()
        {
            DateTime sampleDate = new DateTime(2002, 02, 02, 02, 02, 02);
            bool isFired = false;
            EventHandler handleValueChanged = delegate
                                                  {
                                                      isFired = true;
                                                  };
            _dateTimePickerController.Value = sampleDate;
            TestSetNullValueWithCheckbox();
            _dateTimePickerController.ValueChanged += handleValueChanged;
            _dateTimePicker.Checked = true;
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after checking the checkbox.");
            _dateTimePickerController.ValueChanged -= handleValueChanged;
        }

        [Test, Ignore("The dateTimePicker does not throw any event when the checked property is changed programatically, so this test will never work")]
        public void TestUnCheckingCheckboxFiresEvent()
        {
            DateTime sampleDate = new DateTime(2002, 02, 02, 02, 02, 02);
            bool isFired = false;
            EventHandler handleValueChanged = delegate
                                                  {
                                                      isFired = true;
                                                  };
            _dateTimePickerController.Value = null;
            TestSetControllerValueWithCheckbox();
            _dateTimePickerController.ValueChanged += handleValueChanged;
            _dateTimePicker.Checked = false;
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after unchecking the checkbox.");
            _dateTimePickerController.ValueChanged -= handleValueChanged;
        }
		
        #endregion //Event Tests

        [Test]
        public void TestSetNullDisplayValue()
        {
            string nullDisplayValue = "<TEST_NULL_DISPLAY_VALUE>";
            _dateTimePickerController.NullDisplayValue = nullDisplayValue;
            Assert.AreEqual(nullDisplayValue, _dateTimePickerController.NullDisplayValue);
            Assert.AreEqual(1, _nullDisplayControl.Controls.Count, "Null display panel should have a control.");
            Control displayText = _nullDisplayControl.Controls[0];
            Assert.AreEqual(nullDisplayValue, displayText.Text, "Null display text should equal whas has just been set.");
        }

		
    }
}