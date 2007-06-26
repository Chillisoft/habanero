using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Habanero.Ui.Forms;
using NUnit.Framework;

namespace Habanero.Test.Ui.Application
{
    /// <summary>
    /// Tests the standard MultiSelector control's model
    /// </summary>
    [TestFixture]
    public class TestMultiSelector
    {
        private MultiSelector<TestT> _selector;
        private FieldInfo _availableOptionsListBoxInfo = typeof(MultiSelector<TestT>).GetField("AvailableOptionsListBox", BindingFlags.Instance | BindingFlags.NonPublic);
        private ListBox _availableOptionsListBox;
        private FieldInfo _selectionsListBoxInfo = typeof(MultiSelector<TestT>).GetField("SelectionsListBox", BindingFlags.Instance | BindingFlags.NonPublic);
        private ListBox _selectionsListBox;

        /// <summary>
        /// Setup - run before each test
        /// </summary>
        [SetUp]
        public void Setup() {
            _selector = new MultiSelector<TestT>();
            List<TestT> options;
            options = new List<TestT>();
            options.Add(new TestT());
            options.Add(new TestT());
            _selector.Options = options;
            _availableOptionsListBox = (ListBox)_availableOptionsListBoxInfo.GetValue(_selector);
            _selectionsListBox = (ListBox)_selectionsListBoxInfo.GetValue(_selector);
		}

		#region Test Options List

		/// <summary>
        /// Tests that setting the options collection populates the options listbox
        /// </summary>
        [Test]
        public void TestOptions() {
            Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests whether adding an option to the options list adds the option to the listbox.
        /// </summary>
        [Test]
        public void TestAddOption() {
            _selector.ModelInstance.AddOption(new TestT());
            Assert.AreEqual(3, _availableOptionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests that removing an option from the options list removes the option from the listbox
        /// </summary>
        [Test]
        public void TestRemoveOption()
        {
            _selector.ModelInstance.RemoveOption(_selector.ModelInstance.OptionsView[0]);
            Assert.AreEqual(1, _availableOptionsListBox.Items.Count);
		}

		/// <summary>
		/// Tests that setting the options collection in the model populates the options listbox
		/// </summary>
		[Test]
		public void TestSetOptionsWithModel()
		{
			Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
			MultiSelector<TestT>.Model model = _selector.ModelInstance;
			List<TestT> options = model.AvailableOptions;
			model.Options = new List<TestT>();
			Assert.AreEqual(0, _availableOptionsListBox.Items.Count, 
				"Options list box should be cleared when the model options collection is set to an empty collection.");
			model.Options = options;
			Assert.AreEqual(2, _availableOptionsListBox.Items.Count, 
				"Options list box should be loaded with the new options when the model options collection is set to a new collection.");
		}

    	#endregion Test Options List

		#region Test selections List
		/// <summary>
		/// Tests that setting the selections collection in the model populates the options listbox
		/// </summary>
		[Test]
		public void TestSetSelectionsWithModel()
		{
			Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
			Assert.AreEqual(0, _selectionsListBox.Items.Count);
			MultiSelector<TestT>.Model model = _selector.ModelInstance;
			model.Selections = model.AvailableOptions;
			Assert.AreEqual(0, _availableOptionsListBox.Items.Count,
				"Options list box should be cleared when the model selections collection is set to all the options.");
			Assert.AreEqual(2, _selectionsListBox.Items.Count,
				"Selections list box should be filled when the model selections collection is set to all the options.");
			model.Selections = new List<TestT>();
			Assert.AreEqual(0, _selectionsListBox.Items.Count,
				"Selections list box should be cleared when the model selections collection is set to an empty collection.");
			Assert.AreEqual(2, _availableOptionsListBox.Items.Count,
				"Options list box should be filled when the model selections collection is set to an empty collection.");
		}

		#endregion Test selections List

		#region Test Selecting and Deselecting

		/// <summary>
        /// Tests that selecting an item in the model updates the selector.
        /// </summary>
        [Test]
        public void TestSelect() {
            _selector.ModelInstance.Select(_selector.ModelInstance.OptionsView[0]);
            Assert.AreEqual(1, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(1, _selectionsListBox.Items.Count);
            Assert.AreSame(_selector.ModelInstance.OptionsView[0], _selectionsListBox.SelectedItem);
        }
		
        /// <summary>
        /// Tests that deselecting in the model updates the selector.
        /// </summary>
        [Test]
        public void TestDeselect() {
            _selector.ModelInstance.Select(_selector.ModelInstance.OptionsView[0]);
            _selector.ModelInstance.Deselect(_selector.ModelInstance.OptionsView[0]);
            Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selectionsListBox.Items.Count);
            Assert.AreSame(_selector.ModelInstance.OptionsView[0], _availableOptionsListBox.SelectedItem);
        }

        /// <summary>
        /// Tests that selectall in the model updates the selector.
        /// </summary>
        [Test]
        public void TestSelectAll() {
            _selector.ModelInstance.SelectAll();
            Assert.AreEqual(0, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(2, _selectionsListBox.Items.Count);
         }

         /// <summary>
         /// Tests that deselectall in the model updates the selector.
         /// </summary>
         [Test]
         public void TestDeselectAll()
         {
             _selector.ModelInstance.SelectAll();
             _selector.ModelInstance.DeselectAll();
             Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
             Assert.AreEqual(0, _selectionsListBox.Items.Count);
		 }

		 #endregion Test Selecting and Deselecting

		#region Test Buttons

		 /// <summary>
        /// Tests that the Select button selects the current item
        /// </summary>
        [Test]
        public void TestSelectButton() {
            _availableOptionsListBox.SelectedIndex = 0;
            GetButton("SelectButton").PerformClick();
            Assert.AreEqual(1, _availableOptionsListBox.Items.Count);
            Assert.AreSame(_selector.ModelInstance.OptionsView[0], _selectionsListBox.Items[0]);
            Assert.AreEqual(1, _selectionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests that the Select button is enabled or disabled according to the list
        /// </summary>
        [Test]
        public void TestSelectButtonEnabling() {
        	Button selectButton = GetButton("SelectButton");
        	TestButtonEnabling(selectButton, _availableOptionsListBox);
        }
		
    	/// <summary>
        /// Test selecting multiple items at once
        /// </summary>
        [Test]
        public void TestSelectMulti()
        {
            _availableOptionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[0]);
            _availableOptionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[1]);
            GetButton("SelectButton").PerformClick();
            Assert.AreEqual(0, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(2, _selectionsListBox.Items.Count);

        }
		
        /// <summary>
        /// Tests that the button is disabled when nothing is selected
        /// </summary>
        [Test]
        public void TestSelectWhenNothingIsSelected() {
            _availableOptionsListBox.SelectedIndex = 0;
            _availableOptionsListBox.SelectedIndex = -1;
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

            _selector.ModelInstance.Select(_selector.ModelInstance.OptionsView[0]);
            _selectionsListBox.SelectedIndex = 0;
            Assert.IsTrue(deselectButton.Enabled);

            deselectButton.PerformClick();

            Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selectionsListBox.Items.Count);
        }

		/// <summary>
		/// Tests that the Deselect button is enabled or disabled according to the list
		/// </summary>
		[Test]
		public void TestDeselectButtonEnabling()
		{
			Button deselectButton = GetButton("DeselectButton");
			Assert.AreEqual(0, _selectionsListBox.Items.Count);
			List<TestT> selections = new List<TestT>();
			selections.Add(_selector.ModelInstance.OptionsView[0]);
			_selector.Selections = selections;
			TestButtonEnabling(deselectButton, _selectionsListBox);
		}

        /// <summary>
        /// Test deselecting multiple items at once
        /// </summary>
        [Test]
        public void TestDeselectMulti()
        {
            _selector.ModelInstance.SelectAll();
            _selectionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[0]);
            _selectionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[1]);
            GetButton("DeselectButton").PerformClick();
            Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selectionsListBox.Items.Count);
        }

        /// <summary>
        /// Tests that the selectall button selects all available items
        /// </summary>
        [Test]
        public void TestSelectAllButton() {
            Button selectAllButton = GetButton("SelectAllButton");
            Assert.IsTrue(selectAllButton.Enabled);

            selectAllButton.PerformClick();
            Assert.AreEqual(0, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(2, _selectionsListBox.Items.Count);
            Assert.IsFalse(selectAllButton.Enabled);
        }

        /// <summary>
        /// Tests that the deselectall button deselects all selected items.
        /// </summary>
        [Test]
        public void TestDeselectAllButton() {
            Button deselectAllButton = GetButton("DeselectAllButton");
            Assert.IsFalse(deselectAllButton.Enabled);

            _selector.ModelInstance.SelectAll();

            Assert.IsTrue(deselectAllButton.Enabled);
            deselectAllButton.PerformClick();
            Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selectionsListBox.Items.Count);
            Assert.IsFalse(deselectAllButton.Enabled);            
        }
		
	   	private void TestButtonEnabling(Button selectButton, ListBox correspondingListBox)
    	{
    		string buttonCaption = selectButton.Name;
			Assert.AreEqual(-1, correspondingListBox.SelectedIndex, "No item in the selected options list should be selected after the options have been loaded.");
    		Assert.IsFalse(selectButton.Enabled, buttonCaption + " should be disabled after the list has just been loaded.");
			correspondingListBox.SelectedIndex = 0;
			Assert.AreEqual(0, correspondingListBox.SelectedIndex);
    		Assert.IsTrue(selectButton.Enabled, buttonCaption + " should be enabled after an item in the list is selected.");
			correspondingListBox.SelectedIndex = -1;
			Assert.AreEqual(-1, correspondingListBox.SelectedIndex);
    		Assert.IsFalse(selectButton.Enabled, buttonCaption + " should be disabled after the selected item is cleared.");
    	}

        private Button GetButton(string name) {
            FieldInfo itsButtonInfo = typeof(MultiSelector<TestT>).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return (Button) itsButtonInfo.GetValue(_selector);
		}

		#endregion Test Buttons

		private class TestT{}

    }
}
