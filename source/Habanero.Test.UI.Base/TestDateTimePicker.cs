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
using Habanero.UI.WebGUI;
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
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestDateTimePickerWin : TestDateTimePicker
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestDateTimePickerGiz : TestDateTimePicker
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
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
        public void TestSetToNull_UsingCheckbox()
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
            Assert.AreEqual(dateTimeValue, dateTimePicker.Value);
        }

        [Test]
        public void TestSetToNotNull_UsingCheckbox()
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

        #endregion //Checkbox Tests

        //[Test, Ignore("Currently Working On this(Bringing tests over from TestDateTimePickerController)")]
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
    }
}
