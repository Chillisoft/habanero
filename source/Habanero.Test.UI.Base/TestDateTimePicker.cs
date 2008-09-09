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

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This class tests the DateTimePicker control.
    ///  - The issue of the control being nullable or not is tested.
    /// </summary>
    public abstract class TestDateTimePicker
    {
        protected abstract void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value);
        protected abstract void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value);

        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestDateTimePickerWin : TestDateTimePicker
        {
            protected override void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value)
            {
                System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
                picker.Value = value;
            }

            protected override void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value)
            {
                System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
                picker.Checked = value;
            }

            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
            
            [Test, Ignore("Only for visual testing")]
            public void TestShowDatePickerForm()

            {
                //---------------Set up test pack-------------------
                IFormHabanero formWin = new FormWin();
                IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
                //dateTimePicker.ShowCheckBox = true;
                ITextBox textBox = GetControlFactory().CreateTextBox();
                IButton button = GetControlFactory().CreateButton("Check/Uncheck", delegate(object sender, EventArgs e)
                {
                    dateTimePicker.Checked = !dateTimePicker.Checked;
                });
                BorderLayoutManager borderLayoutManager = GetControlFactory().CreateBorderLayoutManager(formWin);
                borderLayoutManager.AddControl(dateTimePicker, BorderLayoutManager.Position.North);
                borderLayoutManager.AddControl(button, BorderLayoutManager.Position.Centre);
                borderLayoutManager.AddControl(textBox, BorderLayoutManager.Position.South);
                dateTimePicker.ValueChanged += delegate(object sender, EventArgs e)
                {
                    if (dateTimePicker.ValueOrNull.HasValue)
                    {
                        textBox.Text = dateTimePicker.Value.ToString();
                    }
                    else
                    {
                        textBox.Text = "";
                    }
                };
                //---------------Execute Test ----------------------
                formWin.ShowDialog();
                //---------------Test Result -----------------------

                //---------------Tear down -------------------------

            }
        }

        [TestFixture]
        public class TestDateTimePickerVWG : TestDateTimePicker
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value)
            {
                Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
                picker.Value = value;
            }

            protected override void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value)
            {
                Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
                picker.Checked = value;
            }

        }

        private IDateTimePicker CreateDateTimePicker()
        {
            return GetControlFactory().CreateDateTimePicker();
        }

        //TODO: Test that the null state is represented visually
        //TODO: Test if anything intelligent needs to be done with the commented out events in the DateTimePickerManager

        [Test]
        public void TestCreateDateTimePicker()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            //---------------Tear Down   -----------------------
        }

        [Test]
        public void TestDefaultValue()
        {
            //-------------Setup Test Pack ------------------
            //-------------Execute test ---------------------
            DateTime beforeCreate = DateTime.Now;
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime afterCreate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            DateTime dateTime = dateTimePicker.Value;
            Assert.LessOrEqual(beforeCreate, dateTime, "Default value should be Now");
            Assert.GreaterOrEqual(afterCreate, dateTime, "Default value should be Now");
            Assert.IsFalse(dateTimePicker.ShowCheckBox, "Default should not show checkbox");
        
        }

        [Test]
        public void TestSetBaseValue_ChangesValueForIDateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            dateTimePicker.Value = dateTime;
            DateTime dateTimeNew = dateTime.AddDays(1);
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerValue(dateTimePicker, dateTimeNew);
            //---------------Test Result -----------------------
            Assert.AreEqual(dateTimeNew, dateTimePicker.Value);
        }

        

        [Test]
        public void TestSetValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime testDate = new DateTime(2008, 01, 01, 01, 01, 01);
            //---------------Execute Test ----------------------
            dateTimePicker.Value = testDate;
            //---------------Test Result -----------------------
            Assert.AreEqual(testDate, dateTimePicker.Value);
            //---------------Tear Down -------------------------    
        }
        
        [Test]
        public void TestSetValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            dateTimePicker.Value = dateTime;
            //---------------Assert Precondition----------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;
            //---------------Test Result -----------------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.Value);
        }

        [Test]
        public void TestSetNullThenValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            DateTime expectedDateTime = dateTime.AddDays(1);
            dateTimePicker.Value = dateTime;
            dateTimePicker.ValueOrNull = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.Value);
            //---------------Execute Test ----------------------
            dateTimePicker.Value = expectedDateTime;
            //---------------Test Result -----------------------
            Assert.IsNotNull(dateTimePicker.ValueOrNull);
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(expectedDateTime, dateTimePicker.Value);
            Assert.AreEqual(expectedDateTime, dateTimePicker.ValueOrNull.Value);
        }

        [Test]
        public void TestSetNullThenValue_UsingValueOrNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            DateTime expectedDateTime = dateTime.AddDays(1);
            dateTimePicker.Value = dateTime;
            dateTimePicker.ValueOrNull = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.Value);
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = expectedDateTime;
            //---------------Test Result -----------------------
            Assert.IsNotNull(dateTimePicker.ValueOrNull);
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(expectedDateTime, dateTimePicker.Value);
            Assert.AreEqual(expectedDateTime, dateTimePicker.ValueOrNull.Value);
        }

        
        #region Checkbox Tests

        [Test]
        public void TestSetToNull_UnChecksCheckbox()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.Checked = true;
            //-------------Test Pre-conditions --------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.ValueOrNull = null;
            //-------------Test Result ----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
            Assert.AreEqual(dateTimeValue, dateTimePicker.Value);
        }

        [Test]
        public void TestSetToNotNull_ChecksCheckbox()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.ValueOrNull = null;
            dateTimePicker.Checked = false;
            //-------------Test Pre-conditions --------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.ValueOrNull = dateTimeValue;
            //-------------Test Result ----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            Assert.AreEqual(dateTimeValue, dateTimePicker.Value);
        }

        [Test]
        public void TestSetUnCheckedChangesValue_ToNull()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.Checked = true;
            //-------------Test Pre-conditions --------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.Checked = false;
            //-------------Test Result ----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
        }

        [Test]
        public void TestSetCheckedChangesValue_ToValue()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.ValueOrNull = null;
            dateTimePicker.Checked = false;
            //-------------Test Pre-conditions --------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.Checked = true;
            //-------------Test Result ----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            Assert.AreEqual(dateTimeValue, dateTimePicker.ValueOrNull.Value);
        }

        [Test]
        public void TestSetBaseChecked_ChangesValueForIDateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTime = DateTime.Now;
            dateTimePicker.Value = dateTime;
            dateTimePicker.Checked = false;
            //---------------Assert Pre-Conditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerCheckedValue(dateTimePicker, true);
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.ValueOrNull);
        }

        #endregion //Checkbox Tests

        #region Test Events

        [Test]
        public void TestSetValueFiresValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            dateTimePicker.Value = sampleDate;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        [Test]
        public void TestSetBaseValue_FiresValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerValue(dateTimePicker, sampleDate);
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        //TODO: Add To Known Issues: There is no event that responds to changing the value of the Checkbox on the control.
        [Test, Ignore("This is a known issue")]
        public void TestSetBaseUnchecked_FiresValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerCheckedValue(dateTimePicker, false);
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        [Test]
        public void TestSetUnchecked_FiresValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            dateTimePicker.Checked = false;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        [Test]
        public void TestSetValueFiresValueChangedEvent_SetToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate;
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value to null.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        #endregion // Test Events

        #region Visual State Tests

        //[Test]
        //public void TestSetup_NullDisplayControlIsCreated()
        //{
        //    //---------------Set up test pack-------------------
        //    IDateTimePicker dateTimePicker = CreateDateTimePicker();

        //    //---------------Execute Test ----------------------

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, dateTimePicker.Controls.Count);
        //    Assert.IsInstanceOfType(typeof(IPanel), dateTimePicker.Controls[0]);
        //    IPanel pnl = (IPanel)dateTimePicker.Controls[0];
        //    Assert.AreEqual(1, pnl.Controls.Count);
        //    Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[0]);
        //    ILabel lbl = (ILabel)pnl.Controls[0];
        //    Assert.AreEqual("", lbl.Text);
        //    //---------------Tear Down -------------------------          
        //}

        //[Test]
        //public void TestVisualState_WhenNull()
        //{
        //    //---------------Set up test pack-------------------
        //    IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
        //    dateTimePicker.ShowCheckBox = false;
        //    //---------------Execute Test ----------------------
        //    dateTimePicker.ValueOrNull = null;

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(" ", dateTimePicker.CustomFormat);
        //    Assert.AreEqual(DateTimePickerFormat.Custom, dateTimePicker.Format);
        //}

        //[Test]
        //public void TestVisualState_WhenNotNull()
        //{
        //    //---------------Set up test pack-------------------
        //    IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
        //    dateTimePicker.ShowCheckBox = false;
        //    //---------------Execute Test ----------------------
        //    dateTimePicker.ValueOrNull = DateTime.Now;

        //    //---------------Test Result -----------------------
        //    Assert.AreNotEqual(" ", dateTimePicker.CustomFormat);
        //}

        #endregion // Visual State Tests



    }
}
