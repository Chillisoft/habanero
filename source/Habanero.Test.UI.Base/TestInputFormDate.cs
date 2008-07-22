using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestInputFormDate
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestInputFormDateGiz : TestInputFormDate
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }


        [TestFixture]
        public class TestInputFormDateWin : TestInputFormDate
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [Test]
        public void TestSimpleConstructor()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            //---------------Execute Test ----------------------
            InputFormDate inputFormDate  = new InputFormDate(GetControlFactory(), message);

            //---------------Test Result -----------------------
            Assert.AreEqual(message, inputFormDate.Message);
            Assert.AreEqual(DateTime.Now.Date, inputFormDate.DateTimePicker.Value.Date);

        }

        [Test]
        public void TestLayout()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";


            InputFormDate inputFormDate = new InputFormDate(GetControlFactory(), message);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormDate.createControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(IDateTimePicker), panel.Controls[1]);
            Assert.Greater(panel.Controls[0].Top, panel.Top);
            Assert.IsFalse(panel.Controls[0].Font.Bold);
            int width = GetControlFactory().CreateLabel(message, true).PreferredWidth + 20;
            Assert.AreEqual(panel.Width, width);
        }

        [Test]
        public void TestSelectedValue()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";

            InputFormDate inputFormDate = new InputFormDate(GetControlFactory(), message);
            //---------------Assert pre conditions--------------
            Assert.AreEqual(DateTime.Now.Date, inputFormDate.DateTimePicker.Value.Date);
            //---------------Execute Test ----------------------
            DateTime value = DateTime.Now.Date.AddDays(-5);
            inputFormDate.DateTimePicker.Value = value;
            //---------------Test Result -----------------------
            Assert.AreEqual(value, inputFormDate.Value );
            //---------------Tear Down -------------------------
        }
    }

}