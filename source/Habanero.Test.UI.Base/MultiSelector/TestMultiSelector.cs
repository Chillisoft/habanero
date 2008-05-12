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
using System.Collections.ObjectModel;
using Habanero.UI;
using Habanero.UI.WebGUI;
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

            [Test]
            public void TestSelectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.Options = CreateListWithTwoOptions();
                //---------------Execute Test ----------------------

                _selector.AvailableOptionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestSelectButtonIsDisabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.Options = CreateListWithTwoOptions();
                _selector.AvailableOptionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.AvailableOptionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                 Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestDeselectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.Options = options;
                //---------------Execute Test ----------------------
                _selector.Selections = options;

                //---------------Test Result -----------------------
                Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestDeselectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.Options = options;
                _selector.Selections = options;
                //---------------Execute Test ----------------------

                _selector.SelectionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestDeselectButtonIsDisabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.Options = options;
                _selector.Selections = options;
                _selector.SelectionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.SelectionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }
        }

        [TestFixture]
        public class TestMultiSelectorGiz : TestMultiSelector
        {
            protected override IControlFactory GetControlFactory()
            {
                return new GizmoxControlFactory();
            }

            [Test]
            public void TestSelectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();

                //---------------Execute Test ----------------------
                _selector.Options = CreateListWithTwoOptions();

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestSelectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.Options = CreateListWithTwoOptions();
                //---------------Execute Test ----------------------

                _selector.AvailableOptionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestSelectButtonIsEnabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.Options = CreateListWithTwoOptions();
                _selector.AvailableOptionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.AvailableOptionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestClickSelectButtonWithNoItemSelected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                _selector.Options = CreateListWithTwoOptions();
                _selector.AvailableOptionsListBox.SelectedIndex = -1;
                //---------------Execute Test ----------------------

                _selector.GetButton(MultiSelectorButton.Select).PerformClick();
                //---------------Test Result -----------------------

                AssertNoneSelected(_selector);
                //---------------Tear Down -------------------------      
            }



            [Test]
            public void TestDeselectButtonStateAtSet()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.Options = options;
                //---------------Execute Test ----------------------
                _selector.Selections = options;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestDeselectButtonStateUponSelection()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.Options = options;
                _selector.Selections = options;
                //---------------Execute Test ----------------------

                _selector.SelectionsListBox.SelectedIndex = 0;

                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestDeselectButtonIsDisabledWhenItemIsDeselected()
            {
                //---------------Set up test pack-------------------
                IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
                List<TestT> options = CreateListWithTwoOptions();
                _selector.Options = options;
                _selector.Selections = options;
                _selector.SelectionsListBox.SelectedIndex = 0;
                //---------------Execute Test ----------------------
                _selector.SelectionsListBox.SelectedIndex = -1;
                //---------------Test Result -----------------------
                Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
                //---------------Tear Down -------------------------          
            }

            [Test, Ignore("Problem selecting multiple items from code in gizmox")]
            public override void TestSelectingMultipleItemsAtOnce()
            {
                
            }

                       [Test, Ignore("Problem selecting multiple items from code in gizmox")]
            public override void TestDeselectingMultipleItemsAtOnce()
            {
                
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
            AssertNoneSelected(_selector);

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
            AssertNoneSelected(_selector);
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
            AssertAllSelected(_selector);
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
            AssertNoneSelected(_selector);
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



        //currently this test is not working for gizmox.
        [Test]
        public virtual void TestSelectingMultipleItemsAtOnce()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();
            ReadOnlyCollection<TestT> options = _selector.Model.OptionsView;
            IListBox availableOptionsListbox = _selector.AvailableOptionsListBox;
            availableOptionsListbox.SelectedItems.Add(options[0]);
            availableOptionsListbox.SelectedItems.Add(options[1]);

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Select).PerformClick();

            //---------------Test Result -----------------------
            AssertAllSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectButtonUpdatesListboxes()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();
            _selector.Model.Select(_selector.Model.OptionsView[0]);
            _selector.SelectionsListBox.SelectedIndex = 0;

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Deselect).PerformClick();
            //---------------Test Result -----------------------

            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        //currently this test is not working for gizmox.
        [Test]
        public virtual void TestDeselectingMultipleItemsAtOnce()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> options = CreateListWithTwoOptions();
            _selector.Options = options;
            _selector.Model.SelectAll();
            _selector.SelectionsListBox.SelectedItems.Add(options[0]);
            _selector.SelectionsListBox.SelectedItems.Add(options[1]);
            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Deselect).PerformClick();

            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSelectAllButton()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();

            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.SelectAll).PerformClick();

            //---------------Test Result -----------------------
            AssertAllSelected(_selector);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeselectAllButton()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.Options = CreateListWithTwoOptions();
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
            Assert.AreEqual(0, _selector.SelectionsListBox.Items.Count);
        }

        private void AssertAllSelected(IMultiSelector<TestT> _selector)
        {
            Assert.AreEqual(0, _selector.AvailableOptionsListBox.Items.Count);
            Assert.AreEqual(2, _selector.SelectionsListBox.Items.Count);
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
