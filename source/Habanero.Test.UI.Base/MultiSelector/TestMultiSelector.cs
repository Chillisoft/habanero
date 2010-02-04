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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.UI.Base;
using Habanero.UI.VWG;
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
                return new ControlFactoryWin();
            }

            //There are lots of different tests in giz and win because we do not want the event handling
            //overhead of hitting the server all the time to enable and disable buttons.
            [Test]
            public void Test_Win_SelectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();

                //---------------Execute Test ----------------------
                _selector.AllOptions = CreateListWithTwoOptions();

                //---------------Test Result -----------------------
                Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_Win_SelectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.AllOptions = CreateListWithTwoOptions();
                //---------------Execute Test ----------------------

                _selector.AvailableOptionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_Win_SelectButtonIsDisabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.AllOptions = CreateListWithTwoOptions();
                _selector.AvailableOptionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.AvailableOptionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_Win_DeselectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.AllOptions = options;
                //---------------Execute Test ----------------------
                _selector.SelectedOptions = options;

                //---------------Test Result -----------------------
                Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_Win_DeselectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.AllOptions = options;
                _selector.SelectedOptions = options;
                //---------------Execute Test ----------------------

                _selector.SelectedOptionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_Win_DeselectButtonIsDisabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.AllOptions = options;
                _selector.SelectedOptions = options;
                _selector.SelectedOptionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.SelectedOptionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_DoubleClickingHandlersAssigned()
            {
                //---------------Set up test pack-------------------
                
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, TestUtil.CountEventSubscribers(_selector.AvailableOptionsListBox, "DoubleClick"));
                Assert.AreEqual(1, TestUtil.CountEventSubscribers(_selector.SelectedOptionsListBox, "DoubleClick"));

                Assert.IsTrue(TestUtil.EventHasSubscriber(_selector.AvailableOptionsListBox, "DoubleClick", "DoSelect"));
                Assert.IsTrue(TestUtil.EventHasSubscriber(_selector.SelectedOptionsListBox, "DoubleClick", "DoDeselect"));
            }

        }

        [TestFixture]
        public class TestMultiSelectorVWG : TestMultiSelector
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
            //There are lots of different tests in giz because we do not want the event handling
            //overhead of hitting the server all the time to enable and disable buttons.
            [Test]
            public void TestVWG_SelectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();

                //---------------Execute Test ----------------------
                _selector.AllOptions = CreateListWithTwoOptions();

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestVWG_SelectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.AllOptions = CreateListWithTwoOptions();
                //---------------Execute Test ----------------------

                _selector.AvailableOptionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            }

            [Test]
            public void TestVWG_SelectButtonIsEnabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.AllOptions = CreateListWithTwoOptions();
                _selector.AvailableOptionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.AvailableOptionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            }

            [Test]
            public void TestVWG_ClickSelectButtonWithNoItemSelected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.AllOptions = CreateListWithTwoOptions();
                _selector.AvailableOptionsListBox.SelectedIndex = -1;
                //---------------Execute Test ----------------------
                _selector.GetButton(MultiSelectorButton.Select).PerformClick();
                //---------------Test Result -----------------------
                AssertNoneSelected(_selector);
            }


            [Test]
            public void TestVWG_DeselectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.AllOptions = options;
                //---------------Execute Test ----------------------
                _selector.SelectedOptions = options;
                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
            }

            [Test]
            public void TestVWG_DeselectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.AllOptions = options;
                _selector.SelectedOptions = options;
                //---------------Execute Test ----------------------
                _selector.SelectedOptionsListBox.SelectedIndex = 0;
                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
            }

            [Test]
            public void TestVWG_DeselectButtonIsDisabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.AllOptions = options;
                _selector.SelectedOptions = options;
                _selector.SelectedOptionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.SelectedOptionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }
            //There is a bug in giz that does not allow you to programmattically select 
            //multiple items in a list
            [Test, Ignore("Problem selecting multiple items from code in gizmox")]
            public override void TestSelectingMultipleItemsAtOnce_Click()
            {
            }
            //There is a bug in giz that does not allow you to programmattically select 
            //multiple items in a list
            [Test, Ignore("Problem selecting multiple items from code in gizmox")]
            public override void TestDeselectingMultipleItemsAtOnce_Click()
            {
            }
        }

        #region Test AllOptions List

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
            //---------------Assert Preconditions -------------
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selector.SelectedOptionsListBox.Items.Count);
            //---------------Execute Test ----------------------
            _selector.AllOptions = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count); 
        }

        [Test]
        public void Test_SetAllOptions_ToNull_ShouldClearAllOptions()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();
            _selector.AllOptions = twoOptions;
            //---------------Assert Preconditions -------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count); 
            //---------------Execute Test ----------------------
            _selector.AllOptions = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selector.SelectedOptionsListBox.Items.Count);
        }

        [Test]
        public void TestSettingOptionsPopulatesModel()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.AllOptions = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.Model.OptionsView.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddingOptionAddsToListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();

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
            _selector.AllOptions = CreateListWithTwoOptions();

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
            _selector.Model.AllOptions = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSettingBlankOptionsClearsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Model.AllOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.AllOptions = new List<TestT>();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestOptionsMirrorsModelOptions()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Model.AllOptions = CreateListWithTwoOptions();
            Assert.AreEqual(_selector.AllOptions, _selector.Model.AllOptions);
            //---------------Execute Test ----------------------
            _selector.AllOptions = new List<TestT>();
            //---------------Test Result -----------------------
            Assert.AreEqual(_selector.AllOptions, _selector.Model.AllOptions);
            //---------------Tear Down ------------------------- 
        }

        #endregion Test AllOptions List

        #region Test selections List

        [Test]
        public void TestSetSelectionsPopulatesOptionsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();
            _selector.AllOptions = twoOptions;

            //---------------Execute Test ----------------------
            _selector.SelectedOptions = twoOptions;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _selector.SelectedOptionsListBox.Items.Count);
            Assert.AreEqual(_selector.SelectedOptionsListBox.Items.Count, _selector.SelectionsView.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetSelectionsRemovesItemsFromOptionsListbox()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> twoOptions = CreateListWithTwoOptions();
            _selector.AllOptions = twoOptions;

            //---------------Execute Test ----------------------
            _selector.SelectedOptions = twoOptions;

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
            _selector.AllOptions = twoOptions;
            _selector.SelectedOptions = twoOptions;

            //---------------Execute Test ----------------------
            _selector.SelectedOptions = new List<TestT>();
            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSelectionsMirrorsModelSelections()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Model.SelectedOptions = CreateListWithTwoOptions();
            Assert.AreEqual(_selector.SelectedOptions, _selector.Model.SelectedOptions);
            //---------------Execute Test ----------------------
            _selector.AllOptions = new List<TestT>();
            //---------------Test Result -----------------------
            Assert.AreEqual(_selector.SelectedOptions, _selector.Model.SelectedOptions);
            //---------------Tear Down ------------------------- 
        }

        #endregion

        #region Test Selecting and Deselecting

        [Test]
        public void TestSelectingItemUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.Select(_selector.Model.OptionsView[0]);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(1, _selector.SelectedOptionsListBox.Items.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectingItemUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.Model.Select(_selector.Model.OptionsView[0]);

            //---------------Execute Test ----------------------
            _selector.Model.Deselect(_selector.Model.OptionsView[0]);

            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSelectAllUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.Model.SelectAll();

            //---------------Test Result -----------------------
            AssertAllSelected(_selector);
            Assert.AreEqual(_selector.SelectedOptionsListBox.Items.Count, _selector.SelectionsView.Count);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestDeselectAllUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.Model.SelectAll();

            //---------------Execute Test ----------------------
            _selector.Model.DeselectAll();

            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        #endregion

        #region Test Buttons

        [Test]
        public void TestSelectButton_Click()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.AvailableOptionsListBox.SelectedIndex = 0;

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Select).PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(1, _selector.SelectedOptionsListBox.Items.Count);
            Assert.AreEqual(_selector.SelectedOptionsListBox.Items.Count, _selector.SelectionsView.Count);
            //---------------Tear Down -------------------------          
        }

        //currently this test is not working for gizmox.
        [Test]
        public virtual void TestSelectingIListBoxItemsCollectionEnumerator()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            ReadOnlyCollection<TestT> options = _selector.Model.OptionsView;
            IListBox availableOptionsListbox = _selector.AvailableOptionsListBox;
            availableOptionsListbox.SelectedItems.Add(options[0]);
            availableOptionsListbox.SelectedItems.Add(options[1]);

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Select).PerformClick();

            //---------------Test Result -----------------------
            foreach (object o in _selector.SelectedOptionsListBox.Items)
            {
                Assert.IsNotNull(o);
            }
            //---------------Tear Down -------------------------          
        }

        //currently this test is not working for gizmox.
        //There is a bug_ in giz that does not allow you to programmattically select 
        //multiple items in a list
        [Test]
        public virtual void TestSelectingMultipleItemsAtOnce_Click()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            ReadOnlyCollection<TestT> options = _selector.Model.OptionsView;
            IListBox availableOptionsListbox = _selector.AvailableOptionsListBox;
            availableOptionsListbox.SelectedItems.Add(options[0]);
            availableOptionsListbox.SelectedItems.Add(options[1]);

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Select).PerformClick();

            //---------------Test Result -----------------------
            AssertAllSelected(_selector);
            Assert.AreEqual(_selector.SelectedOptionsListBox.Items.Count, _selector.SelectionsView.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectButtonUpdatesListboxes_Click()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.Model.Select(_selector.Model.OptionsView[0]);
            _selector.SelectedOptionsListBox.SelectedIndex = 0;

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Deselect).PerformClick();
            //---------------Test Result -----------------------

            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        //currently this test is not working for gizmox.
        //There is a bug_ in giz that does not allow you to programmattically select 
        //multiple items in a list
        [Test]
        public virtual void TestDeselectingMultipleItemsAtOnce_Click()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> options = CreateListWithTwoOptions();
            _selector.AllOptions = options;
            _selector.Model.SelectAll();
            _selector.SelectedOptionsListBox.SelectedItems.Add(options[0]);
            _selector.SelectedOptionsListBox.SelectedItems.Add(options[1]);
            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Deselect).PerformClick();

            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSelectAllButton_Click()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.SelectAll).PerformClick();

            //---------------Test Result -----------------------
            AssertAllSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectAllButton_click()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.Model.SelectAll();

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.DeselectAll).PerformClick();
            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        #endregion

        #region Custom asserts

        private static void AssertNoneSelected(IMultiSelector<TestT> _selector)
        {
            Assert.AreEqual(2, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(0, _selector.SelectedOptionsListBox.Items.Count);
        }

        private static void AssertAllSelected(IMultiSelector<TestT> _selector)
        {
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(2, _selector.SelectedOptionsListBox.Items.Count);
        }

        #endregion

        #region helper methods

        private static List<TestT> CreateListWithTwoOptions()
        {
            List<TestT> options = new List<TestT>();
            options.Add(new TestT());
            options.Add(new TestT());
            return options;
        }

        #endregion

        private class TestT
        {
        }
    }
}