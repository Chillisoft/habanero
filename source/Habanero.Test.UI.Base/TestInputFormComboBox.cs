using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;
using ScrollBars = Habanero.UI.Base.ScrollBars;

namespace Habanero.Test.UI.Base
{
    public abstract class TestInputFormComboBox
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestInputFormComboBoxGiz : TestInputFormComboBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }


        [TestFixture]
        public class TestInputFormComboBoxWin : TestInputFormComboBox
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
            List<object> choices = new List<object>();
            choices.Add("testItem1");
            choices.Add("testItem2");
            choices.Add("testItem3");

            //---------------Execute Test ----------------------
            InputFormComboBox inputFormComboBox = new InputFormComboBox(GetControlFactory(), message, choices);

            //---------------Test Result -----------------------
            Assert.AreEqual(message, inputFormComboBox.Message);
            Assert.AreEqual(choices.Count,inputFormComboBox.ComboBox.Items.Count);
            Assert.AreEqual(choices[0], inputFormComboBox.ComboBox.Items[0]);
            Assert.AreEqual(choices[1], inputFormComboBox.ComboBox.Items[1]);
            Assert.AreEqual(choices[2], inputFormComboBox.ComboBox.Items[2]);
        }

        [Test]
        public void TestLayout()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            List<object> choices = new List<object>();
            choices.Add("testItem1");
            choices.Add("testItem2");
            choices.Add("testItem3");

            InputFormComboBox inputFormComboBox = new InputFormComboBox(GetControlFactory(), message, choices);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormComboBox.createControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(IComboBox), panel.Controls[1]);
            Assert.Greater(panel.Controls[0].Top, panel.Top);
            Assert.IsFalse(panel.Controls[0].Font.Bold);
            Assert.AreEqual(panel.Width, panel.Controls[1].Width + 30);
            int width = GetControlFactory().CreateLabel(message, true).PreferredWidth + 20;
            Assert.AreEqual(panel.Width, width);
        }

        [Test]
        public void TestSelectedItem()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            List<object> choices = new List<object>();
            choices.Add("testItem1");
            object testitem2 = "testItem2";
            choices.Add(testitem2);
            choices.Add("testItem3");
            InputFormComboBox inputFormComboBox = new InputFormComboBox(GetControlFactory(), message, choices);
            //---------------Assert pre conditions--------------
            Assert.AreEqual(null, inputFormComboBox.SelectedItem);
            //---------------Execute Test ----------------------
            inputFormComboBox.SelectedItem = testitem2;
            //---------------Test Result -----------------------
            Assert.AreSame(testitem2, inputFormComboBox.SelectedItem);
            //---------------Tear Down -------------------------
        }
    }
}