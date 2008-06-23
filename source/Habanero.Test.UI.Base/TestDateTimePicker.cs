using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This class tests the DateTimePicker control.
    ///  - The issue of the control being nullable or not is tested.
    /// </summary>
    [TestFixture]
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

        [Test]
        public void TestSetValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.Value = DateTime.Now;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dateTimePicker.ValueOrNull);
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;
            //---------------Test Result -----------------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
        }

        [Test, Ignore("Currently Working On this(Bringing tests over from TestDateTimePickerController)")]
        public void TestSetup_NullDisplayControlIsCreated()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();

            //---------------Execute Test ----------------------
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dateTimePicker.Controls.Count);
            Assert.IsInstanceOfType(typeof(IPanel), dateTimePicker.Controls[0]);
            IPanel pnl = (IPanel)dateTimePicker.Controls[0];
            Assert.AreEqual(1, pnl.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[0]);
            ILabel lbl = (ILabel)pnl.Controls[0];
            Assert.AreEqual("", lbl.Text);
            //---------------Tear Down -------------------------          
        }
    }
}
