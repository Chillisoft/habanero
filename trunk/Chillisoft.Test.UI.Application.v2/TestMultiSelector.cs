using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Chillisoft.UI.Application.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Application.v2
{
    /// <summary>
    /// Tests the standard MultiSelector control's model
    /// </summary>
    [TestFixture]
    public class TestMultiSelector
    {
        private MultiSelector<TestT> itsSelector;
        private FieldInfo itsAvailableOptionsListBoxInfo = typeof(MultiSelector<TestT>).GetField("AvailableOptionsListBox", BindingFlags.Instance | BindingFlags.NonPublic);
        private ListBox itsAvailableOptionsListBox;
        private FieldInfo itsSelectionsListBoxInfo = typeof(MultiSelector<TestT>).GetField("SelectionsListBox", BindingFlags.Instance | BindingFlags.NonPublic);
        private ListBox itsSelectionsListBox;

        /// <summary>
        /// Setup - run before each test
        /// </summary>
        [SetUp]
        public void Setup() {
            itsSelector = new MultiSelector<TestT>();
            List<TestT> options;
            options = new List<TestT>();
            options.Add(new TestT());
            options.Add(new TestT());
            itsSelector.Options = options;
            itsAvailableOptionsListBox = (ListBox)itsAvailableOptionsListBoxInfo.GetValue(itsSelector);
            itsSelectionsListBox = (ListBox)itsSelectionsListBoxInfo.GetValue(itsSelector);
        }

        /// <summary>
        /// Tests that setting the options collection populates the options listbox
        /// </summary>
        [Test]
        public void TestOptions() {
            Assert.AreEqual(2, itsAvailableOptionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests whether adding an option to the options list adds the option to the listbox.
        /// </summary>
        [Test]
        public void TestAddOption() {
            itsSelector.ModelInstance.AddOption(new TestT());
            Assert.AreEqual(3, itsAvailableOptionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests that removing an option from the options list removes the option from the listbox
        /// </summary>
        [Test]
        public void TestRemoveOption()
        {
            itsSelector.ModelInstance.RemoveOption(itsSelector.ModelInstance.OptionsView[0]);
            Assert.AreEqual(1, itsAvailableOptionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests that selecting an item in the model updates the selector.
        /// </summary>
        [Test]
        public void TestSelect() {
            itsSelector.ModelInstance.Select(itsSelector.ModelInstance.OptionsView[0]);
            Assert.AreEqual(1, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(1, itsSelectionsListBox.Items.Count);
            Assert.AreSame(itsSelector.ModelInstance.OptionsView[0], itsSelectionsListBox.SelectedItem);
        }




        /// <summary>
        /// Tests that deselecting in the model updates the selector.
        /// </summary>
        [Test]
        public void TestDeselect() {
            itsSelector.ModelInstance.Select(itsSelector.ModelInstance.OptionsView[0]);
            itsSelector.ModelInstance.Deselect(itsSelector.ModelInstance.OptionsView[0]);
            Assert.AreEqual(2, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, itsSelectionsListBox.Items.Count);
            Assert.AreSame(itsSelector.ModelInstance.OptionsView[0], itsAvailableOptionsListBox.SelectedItem);
        }

        /// <summary>
        /// Tests that selectall in the model updates the selector.
        /// </summary>
        [Test]
        public void TestSelectAll() {
            itsSelector.ModelInstance.SelectAll();
            Assert.AreEqual(0, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(2, itsSelectionsListBox.Items.Count);
         }

         /// <summary>
         /// Tests that deselectall in the model updates the selector.
         /// </summary>
         [Test]
         public void TestDeselectAll()
         {
             itsSelector.ModelInstance.SelectAll();
             itsSelector.ModelInstance.DeselectAll();
             Assert.AreEqual(2, itsAvailableOptionsListBox.Items.Count);
             Assert.AreEqual(0, itsSelectionsListBox.Items.Count);
         }

        /// <summary>
        /// Tests that the Select button selects the current item
        /// </summary>
        [Test]
        public void TestSelectButton() {
            itsAvailableOptionsListBox.SelectedIndex = 0;
            GetButton("SelectButton").PerformClick();
            Assert.AreEqual(1, itsAvailableOptionsListBox.Items.Count);
            Assert.AreSame(itsSelector.ModelInstance.OptionsView[0], itsSelectionsListBox.Items[0]);
            Assert.AreEqual(1, itsSelectionsListBox.Items.Count);
        }


        /// <summary>
        /// Test selecting multiple items at once
        /// </summary>
        [Test]
        public void TestSelectMulti()
        {
            itsAvailableOptionsListBox.SelectedItems.Add(itsSelector.ModelInstance.OptionsView[0]);
            itsAvailableOptionsListBox.SelectedItems.Add(itsSelector.ModelInstance.OptionsView[1]);
            GetButton("SelectButton").PerformClick();
            Assert.AreEqual(0, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(2, itsSelectionsListBox.Items.Count);

        }


        /// <summary>
        /// Tests that the button is disabled when nothing is selected
        /// </summary>
        [Test]
        public void TestSelectWhenNothingIsSelected() {
            itsAvailableOptionsListBox.SelectedIndex = 0;
            itsAvailableOptionsListBox.SelectedIndex = -1;
            Button b = GetButton("SelectButton");
            Assert.IsFalse(b.Enabled );
        }

        /// <summary>
        /// Tests the Deselect button deselects the current item
        /// </summary>
        [Test]
        public void TestDeselectButton() {
            Button deselectButton = GetButton("DeselectButton");
            Assert.IsFalse(deselectButton.Enabled );

            itsSelector.ModelInstance.Select(itsSelector.ModelInstance.OptionsView[0]);
            itsSelectionsListBox.SelectedIndex = 0;
            Assert.IsTrue(deselectButton.Enabled);

            deselectButton.PerformClick();

            Assert.AreEqual(2, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, itsSelectionsListBox.Items.Count);
        }


        /// <summary>
        /// Test deselecting multiple items at once
        /// </summary>
        [Test]
        public void TestDeselectMulti()
        {
            itsSelector.ModelInstance.SelectAll();
            itsSelectionsListBox.SelectedItems.Add(itsSelector.ModelInstance.OptionsView[0]);
            itsSelectionsListBox.SelectedItems.Add(itsSelector.ModelInstance.OptionsView[1]);
            GetButton("DeselectButton").PerformClick();
            Assert.AreEqual(2, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, itsSelectionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests that the selectall button selects all available items
        /// </summary>
        [Test]
        public void TestSelectAllButton() {
            Button selectAllButton = GetButton("SelectAllButton");
            Assert.IsTrue(selectAllButton.Enabled);

            selectAllButton.PerformClick();
            Assert.AreEqual(0, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(2, itsSelectionsListBox.Items.Count);
            Assert.IsFalse(selectAllButton.Enabled);
        }

        /// <summary>
        /// Tests that the deselectall button deselects all selected items.
        /// </summary>
        [Test]
        public void TestDeselectAllButton() {
            Button deselectAllButton = GetButton("DeselectAllButton");
            Assert.IsFalse(deselectAllButton.Enabled);

            itsSelector.ModelInstance.SelectAll();

            Assert.IsTrue(deselectAllButton.Enabled);
            deselectAllButton.PerformClick();
            Assert.AreEqual(2, itsAvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, itsSelectionsListBox.Items.Count);
            Assert.IsFalse(deselectAllButton.Enabled);            
        }

        private Button GetButton(string name) {
            FieldInfo itsButtonInfo = typeof(MultiSelector<TestT>).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return (Button) itsButtonInfo.GetValue(itsSelector);
        }

        private class TestT{}

    }
}
