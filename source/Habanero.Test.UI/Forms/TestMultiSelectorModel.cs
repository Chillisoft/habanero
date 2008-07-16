//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Tests the standard MultiSelector control's model
    /// </summary>
    [TestFixture]
    public class TestMultiSelectorModel
    {
        private MultiSelector<TestT>.Model itsModel;
        private List<TestT> itsOptions;
        private List<TestT> itsSelections;

        [SetUp]
        public void SetupTest() {
            itsModel = new MultiSelector<TestT>.Model ();
            itsOptions = new List<TestT>();
            itsOptions.Clear();
            itsOptions.Add(new TestT());
            itsOptions.Add(new TestT());
            itsOptions.Add(new TestT());
            itsOptions.Add(new TestT());
            itsModel.Options = itsOptions;
            itsSelections = new List<TestT>();
            itsSelections.Add(itsOptions[1]);
            itsModel.Selections = itsSelections;
        }

        /// <summary>
        /// Tests setting the option list 
        ///  </summary>
        [Test]
        public void TestSetOptions() {
            Assert.AreEqual(4, itsModel.OptionsView.Count);
        }

        /// <summary>
        /// Tests that AddOption adds to the option collection.
        /// </summary>
        [Test]
        public void TestAddOption() {
            itsModel.AddOption(new TestT());
            Assert.AreEqual(5, itsModel.OptionsView.Count);
        }

        /// <summary>
        /// Tests that a shallow copy of the list is made when you set the Options list.
        /// </summary>
        [Test]
        public void TestOptionsCollectionIsIndependant()
        {
            itsOptions.Add(new TestT());
            Assert.AreEqual(4, itsModel.OptionsView.Count);
        }

        /// <summary>
        /// Tests setting the selected list
        /// </summary>
        [Test]
        public void TestSetSelectedList() {
            Assert.AreEqual(1, itsModel.SelectionsView.Count); 
        }

        /// <summary>
        /// Tests the available options list
        /// </summary>
        [Test]
        public void TestAvailableOptions() {
            Assert.AreEqual(3, itsModel.AvailableOptions.Count);
            Assert.AreSame(itsModel.AvailableOptions[0], itsOptions[0]);
            Assert.AreSame(itsModel.AvailableOptions[1], itsOptions[2]);
            Assert.AreSame(itsModel.AvailableOptions[2], itsOptions[3]);
        }
        
        /// <summary>
        /// If the selections list has items in it and you set the options list
        /// if should remove all selections that are not in the options list
        /// and not show options that are in the selection list.
        /// </summary>
        [Test]
        public void TestSetOptionsWhenSelectionsIsSet()
        {
            itsModel.Options = itsOptions;
            Assert.AreEqual(3, itsModel.AvailableOptions.Count );
            Assert.AreEqual(1, itsModel.SelectionsView.Count);

            List<TestT> opts = new List<TestT>();
            opts.Add(new TestT());
            itsModel.Options = opts;
            Assert.AreEqual(0, itsModel.SelectionsView.Count );
        }

        /// <summary>
        /// Tests that selecting an option updates the base lists
        /// </summary>
        [Test]
        public void TestSelectOption() {
            TestT newSelection = itsOptions[0];
            itsModel.Select(newSelection);
            Assert.IsTrue(itsModel.OptionsView.Contains(newSelection));
            Assert.IsTrue(itsModel.SelectionsView.Contains(newSelection));
            Assert.IsFalse(itsModel.AvailableOptions.Contains(newSelection));
        }

        /// <summary>
        /// Tests that deselecting an option updates the base lists
        /// </summary>
        [Test]
        public void TestDeselectOption() {
            TestT oldSelection = itsSelections[0];
            itsModel.Deselect(oldSelection);
            Assert.IsTrue(itsModel.OptionsView.Contains(oldSelection));
            Assert.IsFalse(itsModel.SelectionsView.Contains(oldSelection));
            Assert.IsTrue(itsModel.AvailableOptions.Contains(oldSelection));
        }

        /// <summary>
        /// Tests that selecting an item adds to the Added collection
        /// </summary>
        [Test]
        public void TestAdded() {
            Assert.AreEqual(0, itsModel.Added.Count);
            TestT newSelection = itsOptions[0];
            itsModel.Select(newSelection);
            Assert.AreEqual(1, itsModel.Added.Count);
            Assert.AreSame(newSelection, itsModel.Added[0]);
        }

        /// <summary>
        /// Tests that deselecting an item adds to the removed collection
        /// </summary>
        [Test]
        public void TestRemoved() {
            Assert.AreEqual(0, itsModel.Removed.Count);
            TestT oldSelection = itsSelections [0];
            itsModel.Deselect(oldSelection);
            Assert.AreEqual(1, itsModel.Removed.Count);
            Assert.AreSame(oldSelection, itsModel.Removed[0]); 
        }

        /// <summary>
        /// Tests adding and removing affects collections correctly
        /// </summary>
        [Test]
        public void TestAddingAndRemoving() {
            TestT selection = itsOptions[0];
            itsModel.Select(selection);
            itsModel.Deselect(selection);
            Assert.AreEqual(0, itsModel.Added.Count);
            Assert.AreEqual(0, itsModel.Removed.Count);
        }

        /// <summary>
        /// Test selecting an item not in the options
        /// </summary>
        [Test]
        public void TestSelectingNonOption() {
            int beforeOptions = itsOptions.Count;
            int beforeSelections = itsSelections.Count;
            itsModel.Select(new TestT());
            Assert.AreEqual(beforeOptions, itsOptions.Count);
            Assert.AreEqual(beforeSelections, itsSelections.Count);
        }

        /// <summary>
        /// Test deselecting an item not in the selections
        /// </summary>
        [Test]
        public void TestDeselecting()
        {
            int beforeOptions = itsOptions.Count;
            int beforeSelections = itsSelections.Count;
            itsModel.Deselect( new TestT());
            Assert.AreEqual(beforeOptions, itsOptions.Count);
            Assert.AreEqual(beforeSelections, itsSelections.Count);
        }
        
        /// <summary>
        /// Test select all
        /// </summary>
        [Test]
        public void TestSelectAll() {
            itsModel.SelectAll();
            Assert.AreEqual(itsModel.OptionsView.Count, itsModel.SelectionsView.Count);
            Assert.AreEqual(0, itsModel.AvailableOptions.Count );
        }

        /// <summary>
        /// Test deselect all
        /// </summary>
        [Test]
        public void TestDeselectAll() {
            itsModel.DeselectAll();
            Assert.AreEqual(0, itsModel.SelectionsView.Count);
            Assert.AreEqual(itsModel.OptionsView.Count, itsModel.AvailableOptions.Count);
        }

        /// <summary>
        /// Test selecting multiple items at once.
        /// </summary>
        [Test]
        public void TestMultiSelect() {
            itsModel.Select(itsOptions );
            Assert.AreEqual(0, itsModel.AvailableOptions.Count);
            Assert.AreEqual(itsModel.OptionsView.Count, itsModel.SelectionsView.Count);
        }

        /// <summary>
        /// Test deselecting multiple items at once
        /// </summary>
        [Test]
        public void TestMultiDeselect() {
            itsModel.Select(itsModel.OptionsView[3]);
            List<TestT> deselections = new List<TestT>();
            deselections.AddRange(itsSelections);
            deselections.Add(itsModel.OptionsView[3]);
            itsModel.Deselect(deselections);
            Assert.AreEqual(itsModel.OptionsView.Count, itsModel.AvailableOptions.Count);
            Assert.AreEqual(0, itsModel.SelectionsView.Count);
        }
        
        private class TestT {
            
        }
    }
}