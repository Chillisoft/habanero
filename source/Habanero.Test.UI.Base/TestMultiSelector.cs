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


using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Gizmox;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Tests the standard MultiSelector control's model
    /// </summary>
    public abstract class TestMultiSelector
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestMultiSelectorWin : TestMultiSelector
        {
            protected override IControlFactory GetControlFactory()
            {
                return new WinControlFactory();
            }
        }

        [TestFixture]
        public class TestMultiSelectorGiz : TestMultiSelector
        {
            protected override IControlFactory GetControlFactory()
            {
                return new GizmoxControlFactory();
            }
        }

        #region Test Options List

        [Test]
        public void TestCreateMultiSelector()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IMultiSelector<object> multiSelector = GetControlFactory().CreateMultiSelector<object>();
            //---------------Test Result -----------------------

            Assert.IsNotNull(multiSelector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSettingOptionsPopulatesOptionsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Options = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);

            //---------------Tear Down -------------------------          
        }

        private static List<TestT> CreateListWithTwoOptions()
        {
            List<TestT> options = new List<TestT>();
            options.Add(new TestT());
            options.Add(new TestT());
            return options;
        }

        [Test]
        public void TestSettingOptionsPopulatesModel()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Options = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.Model.OptionsView.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddingOptionAddsToListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.AddOption(new TestT());

            //---------------Test Result -----------------------
            Assert.AreEqual(3, _selector.AvailableOptionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRemoveOptionRemovesFromListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.RemoveOption(_selector.Model.OptionsView[0]);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, _selector.AvailableOptionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSettingOptionsViaModelUpdatesListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.Options = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSettingBlankOptionsClearsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Model.Options = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.Options = new List<TestT>();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        #endregion Test Options List

        #region Test selections List

        [Test]
        public void TestSetSelectionsPopulatesOptionsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();
            _selector.Options = twoOptions;

            //---------------Execute Test ----------------------
            _selector.Selections = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.SelectionsListBox.Items.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetSelectionsRemovesItemsFromOptionsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();
            _selector.Options = twoOptions;

            //---------------Execute Test ----------------------
            _selector.Selections = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetEmptySelectionsRepopulatesOptionsAndSelections()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();
            _selector.Options = twoOptions;
            _selector.Selections = twoOptions;

            //---------------Execute Test ----------------------
            _selector.Selections = new List<TestT>();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selector.SelectionsListBox.Items.Count);

            //---------------Tear Down -------------------------          
        }

        #endregion

        #region Test Selecting and Deselecting

        [Test]
        public void TestSelectingItemUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.Select(_selector.Model.OptionsView[0]);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(1, _selector.SelectionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectingItemUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();
            _selector.Model.Select(_selector.Model.OptionsView[0]);

            //---------------Execute Test ----------------------
            _selector.Model.Deselect(_selector.Model.OptionsView[0]);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selector.SelectionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSelectAllUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.SelectAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(2, _selector.SelectionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectAllUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();
            _selector.Model.SelectAll();

            //---------------Execute Test ----------------------
            _selector.Model.DeselectAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selector.SelectionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        #endregion

        #region Test Buttons

        [Test]
        public void TestSelectButton()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();
            _selector.AvailableOptionsListBox.SelectedIndex = 0;

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Select).PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(1, _selector.SelectionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSelectButtonStateAtSet()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();

            //---------------Execute Test ----------------------
            _selector.Options = CreateListWithTwoOptions();
            
            //---------------Test Result -----------------------
            Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            //---------------Tear Down -------------------------          
        }

        #endregion

        ///// <summary>
        ///// Tests that the Select button is enabled or disabled according to the list
        ///// </summary>
        //[Test]
        //public void TestSelectButtonEnabling() {
        //    Button selectButton = GetButton("SelectButton");
        //    TestButtonEnabling(selectButton, _availableOptionsListBox);
        //}

        ///// <summary>
        ///// Test selecting multiple items at once
        ///// </summary>
        //[Test]
        //public void TestSelectMulti()
        //{
        //   // _availableOptionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[0]);
        //   // _availableOptionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[1]);
        //    GetButton("SelectButton").PerformClick();
        //    Assert.AreEqual(0, _availableOptionsListBox.Items.Count);
        //    Assert.AreEqual(2, _selectionsListBox.Items.Count);

        //}

        ///// <summary>
        ///// Tests that the button is disabled when nothing is selected
        ///// </summary>
        //[Test]
        //public void TestSelectWhenNothingIsSelected() {
        //    _availableOptionsListBox.SelectedIndex = 0;
        //    _availableOptionsListBox.SelectedIndex = -1;
        //    Button b = GetButton("SelectButton");
        //    Assert.IsFalse(b.Enabled );
        //}

        ///// <summary>
        ///// Tests the Deselect button deselects the current item
        ///// </summary>
        //[Test]
        //public void TestDeselectButton() {
        //    Button deselectButton = GetButton("DeselectButton");
        //    Assert.IsFalse(deselectButton.Enabled );

        //   // _selector.ModelInstance.Select(_selector.ModelInstance.OptionsView[0]);
        //    _selectionsListBox.SelectedIndex = 0;
        //    Assert.IsTrue(deselectButton.Enabled);

        //    deselectButton.PerformClick();

        //    Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
        //    Assert.AreEqual(0, _selectionsListBox.Items.Count);
        //}

        ///// <summary>
        ///// Tests that the Deselect button is enabled or disabled according to the list
        ///// </summary>
        //[Test]
        //public void TestDeselectButtonEnabling()
        //{
        //    Button deselectButton = GetButton("DeselectButton");
        //    Assert.AreEqual(0, _selectionsListBox.Items.Count);
        //    List<TestT> selections = new List<TestT>();
        //  //  selections.Add(_selector.ModelInstance.OptionsView[0]);
        // //   _selector.Selections = selections;
        //    TestButtonEnabling(deselectButton, _selectionsListBox);
        //}

        ///// <summary>
        ///// Test deselecting multiple items at once
        ///// </summary>
        //[Test]
        //public void TestDeselectMulti()
        //{
        //    //_selector.ModelInstance.SelectAll();
        //  //  _selectionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[0]);
        // //   _selectionsListBox.SelectedItems.Add(_selector.ModelInstance.OptionsView[1]);
        //    GetButton("DeselectButton").PerformClick();
        //    Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
        //    Assert.AreEqual(0, _selectionsListBox.Items.Count);
        //}

        ///// <summary>
        ///// Tests that the selectall button selects all available items
        ///// </summary>
        //[Test]
        //public void TestSelectAllButton() {
        //    Button selectAllButton = GetButton("SelectAllButton");
        //    Assert.IsTrue(selectAllButton.Enabled);

        //    selectAllButton.PerformClick();
        //    Assert.AreEqual(0, _availableOptionsListBox.Items.Count);
        //    Assert.AreEqual(2, _selectionsListBox.Items.Count);
        //    Assert.IsFalse(selectAllButton.Enabled);
        //}

        ///// <summary>
        ///// Tests that the deselectall button deselects all selected items.
        ///// </summary>
        //[Test]
        //public void TestDeselectAllButton() {
        //    Button deselectAllButton = GetButton("DeselectAllButton");
        //    Assert.IsFalse(deselectAllButton.Enabled);

        //  //  _selector.ModelInstance.SelectAll();

        //    Assert.IsTrue(deselectAllButton.Enabled);
        //    deselectAllButton.PerformClick();
        //    Assert.AreEqual(2, _availableOptionsListBox.Items.Count);
        //    Assert.AreEqual(0, _selectionsListBox.Items.Count);
        //    Assert.IsFalse(deselectAllButton.Enabled);            
        //}

        //private void TestButtonEnabling(Button selectButton, ListBox correspondingListBox)
        //{
        //    string buttonCaption = selectButton.Name;
        //    Assert.AreEqual(-1, correspondingListBox.SelectedIndex, "No item in the selected options list should be selected after the options have been loaded.");
        //    Assert.IsFalse(selectButton.Enabled, buttonCaption + " should be disabled after the list has just been loaded.");
        //    correspondingListBox.SelectedIndex = 0;
        //    Assert.AreEqual(0, correspondingListBox.SelectedIndex);
        //    Assert.IsTrue(selectButton.Enabled, buttonCaption + " should be enabled after an item in the list is selected.");
        //    correspondingListBox.SelectedIndex = -1;
        //    Assert.AreEqual(-1, correspondingListBox.SelectedIndex);
        //    Assert.IsFalse(selectButton.Enabled, buttonCaption + " should be disabled after the selected item is cleared.");
        //}

        private Button GetButton(string name)
        {
            // FieldInfo itsButtonInfo = typeof(MultiSelector<TestT>).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            // return (Button) itsButtonInfo.GetValue(_selector);
            return new Button();
        }


        private class TestT
        {
        }
    }
}