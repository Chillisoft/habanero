//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Drawing;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the DateTimePicker class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_DateTimePicker : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateDateTimePicker();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the DateTimePicker class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_DateTimePicker : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateDateTimePicker();
        }
    }

    /// <summary>
    /// This class tests the DateTimePicker control.
    ///  - The issue of the control being nullable or not is tested.
    /// </summary>
    public abstract class TestDateTimePicker
    {
        protected abstract void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value);
        protected abstract void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value);

        protected abstract IControlFactory GetControlFactory();

        protected abstract EventArgs GetKeyDownEventArgsForDeleteKey();
        protected abstract EventArgs GetKeyDownEventArgsForBackspaceKey();
        protected abstract EventArgs GetKeyDownEventArgsForOtherKey();

        protected IDateTimePicker CreateDateTimePicker()
        {
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            //IFormHabanero form = GetControlFactory().CreateForm();
            //form.Controls.Add(dateTimePicker);
            //form.Visible = true;
            return dateTimePicker;
        }

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

            protected override EventArgs GetKeyDownEventArgsForDeleteKey()
            {
                return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.Delete);
            }

            protected override EventArgs GetKeyDownEventArgsForBackspaceKey()
            {
                return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.Back);
            }

            protected override EventArgs GetKeyDownEventArgsForOtherKey()
            {
                return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.A);
            }

            [Test, Ignore("Only for visual testing")]
            public void TestShowWithEvents()
            {
                //---------------Set up test pack-------------------
                System.Windows.Forms.DateTimePicker dateTimePicker = new System.Windows.Forms.DateTimePicker();
                dateTimePicker.ShowCheckBox = true;
                System.Windows.Forms.Form form = new System.Windows.Forms.Form();
                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                form.Controls.Add(textBox);
                textBox.Dock = System.Windows.Forms.DockStyle.Fill;
                textBox.Multiline = true;
                form.Controls.Add(dateTimePicker);
                dateTimePicker.Dock = System.Windows.Forms.DockStyle.Top;
                dateTimePicker.ValueChanged += delegate
                {
                    textBox.Text += "EventFired";
                };
                System.Windows.Forms.Button button = new System.Windows.Forms.Button();
                form.Controls.Add(button);
                button.Dock = System.Windows.Forms.DockStyle.Bottom;
                button.Click += delegate {
                    dateTimePicker.Checked = !dateTimePicker.Checked;
                };
                //-------------Assert Preconditions -------------

                //---------------Execute Test ----------------------
                form.ShowDialog();
                //---------------Test Result -----------------------

            }

            [Test, Ignore("Only for visual testing")]
            public void TestShowDatePickerForm()
            {
                //---------------Set up test pack-------------------
                IFormHabanero formWin = new FormWin();
                IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
                dateTimePicker.Format = Habanero.UI.Base.DateTimePickerFormat.Custom;
                dateTimePicker.CustomFormat = @"Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa: dd MMM yyyy";
                dateTimePicker.NullDisplayValue = "Please Click";
                //dateTimePicker.ShowCheckBox = true;
                ITextBox textBox = GetControlFactory().CreateTextBox();
                IButton button = GetControlFactory().CreateButton("Check/Uncheck", delegate
                                                                                       {
                    //dateTimePicker.Checked = !dateTimePicker.Checked;
                    if (dateTimePicker.ValueOrNull.HasValue)
                    {
                        dateTimePicker.ValueOrNull = null;
                    }
                    else
                    {
                        dateTimePicker.ValueOrNull = dateTimePicker.Value;
                    }
                });
                IButton enableButton = GetControlFactory().CreateButton("Enable/Disable", delegate {
                    dateTimePicker.Enabled = !dateTimePicker.Enabled;
                });
                GridLayoutManager gridLayoutManager = new GridLayoutManager(formWin, GetControlFactory());
                gridLayoutManager.SetGridSize(5,1);
                gridLayoutManager.AddControl(dateTimePicker);
                gridLayoutManager.AddControl(button);
                gridLayoutManager.AddControl(textBox);
                gridLayoutManager.AddControl(enableButton);
                gridLayoutManager.AddControl(GetControlFactory().CreateButton("ChangeColor", delegate
                                                                                                 {
                    Random random = new Random();
                    dateTimePicker.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    dateTimePicker.BackColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                }));
                dateTimePicker.ValueChanged += delegate {
                    textBox.Text = dateTimePicker.ValueOrNull.HasValue ? dateTimePicker.Value.ToString() : "";
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

            protected override EventArgs GetKeyDownEventArgsForDeleteKey()
            {
                return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.Delete);
            }

            protected override EventArgs GetKeyDownEventArgsForBackspaceKey()
            {
                return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.Back);
            }

            protected override EventArgs GetKeyDownEventArgsForOtherKey()
            {
                return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.A);
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
            //Assert.IsFalse(dateTimePicker.ShowCheckBox, "Default should not show checkbox");
        }

        [Test]
        public void TestSetBaseValue_ChangesValueForIDateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
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
        [Test, Ignore("This is a known issue. There is no event that responds to changing the value of the Checkbox on the control")]
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

        [Test]
        public void TestSetup_NullDisplayControlIsCreated()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dateTimePicker.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), dateTimePicker.Controls[0]);
            //IPanel pnl = (IPanel)dateTimePicker.Controls[0];
            //Assert.AreEqual(1, pnl.Controls.Count);
            //Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[0]);
            //ILabel lbl = (ILabel)pnl.Controls[0];
            //Assert.AreEqual("", lbl.Text);
            //---------------Tear Down -------------------------          
        }

        private IControlHabanero GetNullDisplayControl(IDateTimePicker dateTimePicker)
        {
            if (dateTimePicker == null) return null;
            if (dateTimePicker.Controls.Count == 0) return null;
            return dateTimePicker.Controls[0];
        }

        [Test]
        public void TestVisualState_NullDisplayValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            string nullDisplayValue = TestUtil.GetRandomString();
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.AreEqual("", dateTimePicker.NullDisplayValue);
            Assert.AreEqual("", nullDisplayControl.Text);
            //---------------Execute Test ----------------------
            dateTimePicker.NullDisplayValue = nullDisplayValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(nullDisplayValue, dateTimePicker.NullDisplayValue);
            Assert.AreEqual(nullDisplayValue, nullDisplayControl.Text);
        }

        [Test]
        public void TestVisualState_WhenCreated()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsTrue(nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestVisualState_WhenNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsTrue(nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestVisualState_WhenNotNull_ThenNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            dateTimePicker.ValueOrNull = DateTime.Now;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsTrue(nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestVisualState_WhenNotNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = DateTime.Now;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsFalse(nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
        }

        [Test]
        public void TestVisualState_WhenNull_ThenNotNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            dateTimePicker.ValueOrNull = null;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = DateTime.Now;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsFalse(nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
        }

        [Test]
        public void TestVisualState_ResizesCorrectly()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            dateTimePicker.ValueOrNull = null;
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            const int widthDifference = 24;
            const int heightDifference = 7;
            Assert.AreEqual(dateTimePicker.Width - widthDifference, nullDisplayControl.Width);
            Assert.AreEqual(dateTimePicker.Height - heightDifference, nullDisplayControl.Height);
            //---------------Execute Test ----------------------
            dateTimePicker.Size = Size.Add(dateTimePicker.Size, new Size(10, 4));
            //---------------Test Result -----------------------
            Assert.AreEqual(dateTimePicker.Width - widthDifference, nullDisplayControl.Width);
            Assert.AreEqual(dateTimePicker.Height - heightDifference, nullDisplayControl.Height);
        }

        [Test, Ignore("This does not seem to be testable")]
        public void TestVisualState_WhenNull_Selected()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(dateTimePicker.BackColor, nullDisplayControl.BackColor);
            Assert.AreEqual(dateTimePicker.ForeColor, nullDisplayControl.ForeColor);
            //---------------Execute Test ----------------------
            dateTimePicker.Focus();
            //EventHelper.RaiseEvent(dateTimePicker, "GotFocus");
            //---------------Test Result -----------------------
            Assert.AreEqual(SystemColors.Highlight, nullDisplayControl.BackColor);
            Assert.AreEqual(SystemColors.HighlightText, nullDisplayControl.ForeColor);

        }

        #endregion // Visual State Tests

        #region User Interaction Tests

        [Test]
        public void TestClick_ChangesNullToValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            //-------------Assert Preconditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "Click");
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestClick_WhenNotNullStaysAtValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = DateTime.Now;
            //-------------Assert Preconditions -------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "Click");
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestClick_OnDisplayControlChangesNullToValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(nullDisplayControl, "Click");
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestKeyPress_ChangesNullToValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            //-------------Assert Preconditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "KeyDown", GetKeyDownEventArgsForOtherKey());
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestKeyPress_DeleteChangesValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = DateTime.Now;
            //-------------Assert Preconditions -------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "KeyDown", GetKeyDownEventArgsForDeleteKey());
            //---------------Test Result -----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestKeyPress_BackSpaceChangesValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = DateTime.Now;
            //-------------Assert Preconditions -------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "KeyDown", GetKeyDownEventArgsForBackspaceKey());
            //---------------Test Result -----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
        }

        #endregion User Interaction Tests

    }
}
