// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System.Collections.Generic;
using Habanero.UI.Base;


using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Habanero.Test.UI.Base
{
    public abstract class TestInputFormComboBox
    {
        protected abstract IControlFactory GetControlFactory();



        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            List<object> choices = new List<object> {"testItem1", "testItem2", "testItem3"};
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
        public void Test_Layout()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            //---------------Execute Test ----------------------
            IPanel panel = inputFormComboBox.CreateControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            ILabel label = TestUtil.AssertIsInstanceOf<ILabel>(panel.Controls[0]);
            IComboBox comboBox = TestUtil.AssertIsInstanceOf<IComboBox>(panel.Controls[1]);
            Assert.AreSame(inputFormComboBox.ComboBox, comboBox);
            Assert.That(label.Top + label.Height, Is.LessThan(comboBox.Top));
            Assert.That(comboBox.Top + comboBox.Height, Is.LessThanOrEqualTo(panel.Height));
            Assert.IsFalse(label.Font.Bold);
            Assert.AreEqual(panel.Width - 10, comboBox.Width, "Combo width should be panel width - border widths(5 pixels X 2)");
            Assert.That(label.Width, Is.GreaterThan(label.PreferredWidth));
            Assert.AreEqual(panel.Size, panel.MinimumSize);
        }

        [Test]
        public void Test_Layout_WhenShortLabelAndCombo_ShouldMinimumWidthForWidth()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems("x");
            //---------------Execute Test ----------------------
            IPanel panel = inputFormComboBox.CreateControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(200, panel.Width);
        }

        [Test]
        public void Test_Layout_WhenLongLabel_ShouldUseLabelPreferredWidthForWidth()
        {
            //---------------Set up test pack-------------------
            const string message = "This is a very long message for testing the width of the form being determined by the message width.";
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems(message);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormComboBox.CreateControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            ILabel label = TestUtil.AssertIsInstanceOf<ILabel>(panel.Controls[0]);
            Assert.AreEqual(label.PreferredWidth + 20, panel.Width);
        }

        [Test]
        public void Test_Layout_WhenLongComboBoxItem_ShouldUseComboPreferredWidthForWidth()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            const string longComboBoxItem = "This is a very long item for the purposes of testing the combo width";
            inputFormComboBox.ComboBox.Items.Add(longComboBoxItem);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormComboBox.CreateControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            int longComboBoxItemPreferredWidth = GetControlFactory().CreateLabel(longComboBoxItem).PreferredWidth;
            Assert.AreEqual(longComboBoxItemPreferredWidth + 40, panel.Width);
        }

        [Test]
        public void Test_SelectedItem_GetAndSet()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            int indexToSelect = TestUtil.GetRandomInt(0, 2);
            string itemToSelect = inputFormComboBox.ComboBox.Items[indexToSelect].ToString();
            //---------------Assert pre conditions--------------
            Assert.IsNull(inputFormComboBox.SelectedItem);
            //---------------Execute Test ----------------------
            inputFormComboBox.SelectedItem = itemToSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(itemToSelect, inputFormComboBox.SelectedItem);
        }

        [Test]
        public void Test_SelectedItem_WhenSet_ShouldSetSelectedItemOnComboBox()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            int indexToSelect = TestUtil.GetRandomInt(0, 2);
            string itemToSelect = inputFormComboBox.ComboBox.Items[indexToSelect].ToString();
            //---------------Assert pre conditions--------------
            Assert.IsNull(inputFormComboBox.SelectedItem);
            Assert.IsNull(inputFormComboBox.ComboBox.SelectedItem);
            Assert.AreEqual(-1, inputFormComboBox.ComboBox.SelectedIndex);
            //---------------Execute Test ----------------------
            inputFormComboBox.SelectedItem = itemToSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(itemToSelect, inputFormComboBox.SelectedItem);
            Assert.AreEqual(itemToSelect, inputFormComboBox.ComboBox.SelectedItem);
            Assert.AreEqual(indexToSelect, inputFormComboBox.ComboBox.SelectedIndex);
        }

        [Test]
        public virtual void Test_CreateOKCancelForm_ShouldSetMinimumSize()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IFormHabanero formHabanero = inputFormComboBox.CreateOKCancelForm();
            //---------------Test Result -----------------------
            Assert.AreEqual(formHabanero.Size, formHabanero.MinimumSize);
        }

        [Test]
        public virtual void Test_CreateOKCancelForm_ShouldSetFormBorderStyle()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IFormHabanero formHabanero = inputFormComboBox.CreateOKCancelForm();
            //---------------Test Result -----------------------
            Assert.AreEqual(FormBorderStyle.FixedToolWindow, formHabanero.FormBorderStyle);
        }

        [Test]
        public virtual void Test_CreateOKCancelForm_ShouldSetTitleToSelect()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IFormHabanero formHabanero = inputFormComboBox.CreateOKCancelForm();
            //---------------Test Result -----------------------
            Assert.AreEqual("Select", formHabanero.Text);
        }

        [Test]
        [Ignore("This is for visual testing purposes")]
        public void Test_Visually()
        {
            //---------------Set up test pack-------------------
            InputFormComboBox inputFormComboBox = CreateInputFormComboBoxWithThreeItems();
            inputFormComboBox.ComboBox.Items.Add("This is a very long item for the purposes of testing the combo width");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            inputFormComboBox.ShowDialog();
            //---------------Test Result -----------------------
        }

        private InputFormComboBox CreateInputFormComboBoxWithThreeItems()
        {
            return CreateInputFormComboBoxWithThreeItems("testMessage");
        }

        private InputFormComboBox CreateInputFormComboBoxWithThreeItems(string message)
        {
            List<object> choices = new List<object> { "testItem1", "testItem2", "testItem3" };
            return new InputFormComboBox(GetControlFactory(), message, choices);
        }
    }
}