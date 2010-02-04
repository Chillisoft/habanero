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

using System;
using System.Collections.Generic;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Tests the standard MultiSelector control's model
    /// </summary>
    [TestFixture]
    public class TestMultiSelectorModel
    {
        private MultiSelectorModel<TestT> _model;
        private List<TestT> _options;
        private List<TestT> _selections;

        [SetUp]
        public void SetupTest() {
            _model = new MultiSelectorModel<TestT>();
            _options = new List<TestT>();
            _options.Clear();
            _options.Add(new TestT());
            _options.Add(new TestT());
            _options.Add(new TestT());
            _options.Add(new TestT());
            _model.AllOptions = _options;
            _selections = new List<TestT>();
            _selections.Add(_options[1]);
            _model.SelectedOptions = _selections;
        }

        /// <summary>
        /// Tests setting the option list 
        ///  </summary>
        [Test]
        public void TestSetOptions() {
            Assert.AreEqual(4, _model.OptionsView.Count);
        }


        [Test]
        public void Test_SelectionsItemsInOrderOfSelection()
        {
            //---------------Set up test pack-------------------
            _model.AllOptions.Clear();
            List<TestT> availableOptions = new List<TestT>();
            TestT item1 = new TestT();
            item1.Name = "Item1";
            TestT item2 = new TestT();
            item2.Name = "Item2";
            TestT item3 = new TestT();
            item3.Name = "Item3";
            availableOptions.Add(item1);
            availableOptions.Add(item2);
            availableOptions.Add(item3);
            _model.AllOptions = availableOptions;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3,_model.AvailableOptions.Count);
            Assert.AreEqual(0,_model.SelectedOptions.Count);
            Assert.AreEqual(item1,_model.AvailableOptions[0]);
            Assert.AreEqual(item2,_model.AvailableOptions[1]);
            Assert.AreEqual(item3,_model.AvailableOptions[2]);
            //---------------Execute Test ----------------------
            _model.Select(_model.AllOptions[2]);
            _model.Select(_model.AllOptions[0]);
            _model.Select(_model.AllOptions[1]);
            //---------------Test Result -----------------------
            Assert.AreEqual(item3,_selections[0]);
            Assert.AreEqual(item1,_selections[1]);
            Assert.AreEqual(item2,_selections[2]);
        }

        [Test]
        public void TestSetSelectionsNull_NoError()
        {
            //---------------Set up test pack-------------------
            MultiSelectorModel<TestT> model = new MultiSelectorModel<TestT>();
            //--------------Assert PreConditions----------------            
            
            //---------------Execute Test ----------------------
            model.SelectedOptions = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(0,model.SelectionsView.Count);
            //---------------Tear Down -------------------------          
        }
        /// <summary>
        /// Tests that AddOption adds to the option collection.
        /// </summary>
        [Test]
        public void TestAddOption() {
            _model.AddOption(new TestT());
            Assert.AreEqual(5, _model.OptionsView.Count);
        }

        /// <summary>
        /// Tests that a shallow copy of the list is made when you set the AllOptions list.
        /// </summary>
        [Test]
        public void TestOptionsCollectionIsIndependant()
        {
            _options.Add(new TestT());
            Assert.AreEqual(4, _model.OptionsView.Count);
        }

        /// <summary>
        /// Tests setting the selected list
        /// </summary>
        [Test]
        public void TestSetSelectedList() {
            Assert.AreEqual(1, _model.SelectionsView.Count); 
        }

        /// <summary>
        /// Tests the available options list
        /// </summary>
        [Test]
        public void TestAvailableOptions() {
            Assert.AreEqual(3, _model.AvailableOptions.Count);
            Assert.AreSame(_model.AvailableOptions[0], _options[0]);
            Assert.AreSame(_model.AvailableOptions[1], _options[2]);
            Assert.AreSame(_model.AvailableOptions[2], _options[3]);
        }
        
        /// <summary>
        /// If the selections list has items in it and you set the options list
        /// if should remove all selections that are not in the options list
        /// and not show options that are in the selection list.
        /// </summary>
        [Test]
        public void TestSetOptionsWhenSelectionsIsSet()
        {
            _model.AllOptions = _options;
            Assert.AreEqual(3, _model.AvailableOptions.Count );
            Assert.AreEqual(1, _model.SelectionsView.Count);

            List<TestT> opts = new List<TestT>();
            opts.Add(new TestT());
            _model.AllOptions = opts;
            Assert.AreEqual(0, _model.SelectionsView.Count );
        }

        /// <summary>
        /// Tests that selecting an option updates the base lists
        /// </summary>
        [Test]
        public void TestSelectOption() {
            TestT newSelection = _options[0];
            _model.Select(newSelection);
            Assert.IsTrue(_model.OptionsView.Contains(newSelection));
            Assert.IsTrue(_model.SelectionsView.Contains(newSelection));
            Assert.IsFalse(_model.AvailableOptions.Contains(newSelection));
        }

        /// <summary>
        /// Tests that deselecting an option updates the base lists
        /// </summary>
        [Test]
        public void TestDeselectOption() {
            TestT oldSelection = _selections[0];
            _model.Deselect(oldSelection);
            Assert.IsTrue(_model.OptionsView.Contains(oldSelection));
            Assert.IsFalse(_model.SelectionsView.Contains(oldSelection));
            Assert.IsTrue(_model.AvailableOptions.Contains(oldSelection));
        }

        /// <summary>
        /// Tests that selecting an item adds to the Added collection
        /// </summary>
        [Test]
        public void TestAdded() {
            Assert.AreEqual(0, _model.Added.Count);
            TestT newSelection = _options[0];
            _model.Select(newSelection);
            Assert.AreEqual(1, _model.Added.Count);
            Assert.AreSame(newSelection, _model.Added[0]);
        }

        /// <summary>
        /// Tests that deselecting an item adds to the removed collection
        /// </summary>
        [Test]
        public void TestRemoved() {
            Assert.AreEqual(0, _model.Removed.Count);
            TestT oldSelection = _selections [0];
            _model.Deselect(oldSelection);
            Assert.AreEqual(1, _model.Removed.Count);
            Assert.AreSame(oldSelection, _model.Removed[0]); 
        }

        /// <summary>
        /// Tests adding and removing affects collections correctly
        /// </summary>
        [Test]
        public void TestAddingAndRemoving() {
            TestT selection = _options[0];
            _model.Select(selection);
            _model.Deselect(selection);
            Assert.AreEqual(0, _model.Added.Count);
            Assert.AreEqual(0, _model.Removed.Count);
        }

        /// <summary>
        /// Test selecting an item not in the options
        /// </summary>
        [Test]
        public void TestSelectingNonOption() {
            int beforeOptions = _options.Count;
            int beforeSelections = _selections.Count;
            _model.Select(new TestT());
            Assert.AreEqual(beforeOptions, _options.Count);
            Assert.AreEqual(beforeSelections, _selections.Count);
        }

        /// <summary>
        /// Test deselecting an item not in the selections
        /// </summary>
        [Test]
        public void TestDeselecting()
        {
            int beforeOptions = _options.Count;
            int beforeSelections = _selections.Count;
            _model.Deselect( new TestT());
            Assert.AreEqual(beforeOptions, _options.Count);
            Assert.AreEqual(beforeSelections, _selections.Count);
        }
        
        /// <summary>
        /// Test select all
        /// </summary>
        [Test]
        public void TestSelectAll() {
            _model.SelectAll();
            Assert.AreEqual(_model.OptionsView.Count, _model.SelectionsView.Count);
            Assert.AreEqual(0, _model.AvailableOptions.Count );
        }

        /// <summary>
        /// Test deselect all
        /// </summary>
        [Test]
        public void TestDeselectAll() {
            _model.DeselectAll();
            Assert.AreEqual(0, _model.SelectionsView.Count);
            Assert.AreEqual(_model.OptionsView.Count, _model.AvailableOptions.Count);
        }

        /// <summary>
        /// Test selecting multiple items at once.
        /// </summary>
        [Test]
        public void TestMultiSelect() {
            _model.Select(_options );
            Assert.AreEqual(0, _model.AvailableOptions.Count);
            Assert.AreEqual(_model.OptionsView.Count, _model.SelectionsView.Count);
        }

        /// <summary>
        /// Test deselecting multiple items at once
        /// </summary>
        [Test]
        public void TestMultiDeselect() {
            _model.Select(_model.OptionsView[3]);
            List<TestT> deselections = new List<TestT>();
            deselections.AddRange(_selections);
            deselections.Add(_model.OptionsView[3]);
            _model.Deselect(deselections);
            Assert.AreEqual(_model.OptionsView.Count, _model.AvailableOptions.Count);
            Assert.AreEqual(0, _model.SelectionsView.Count);
        }
        
        private class TestT {

            public string Name
            {
                get;set;
            }
        }
    }
}