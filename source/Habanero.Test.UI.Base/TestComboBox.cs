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
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;
using AutoCompleteSource=Habanero.UI.Base.AutoCompleteSource;

namespace Habanero.Test.UI.Base
{

    /// <summary>
    /// This test class tests the ComboBox class.
    /// </summary>
    public abstract class TestComboBox
    {

        protected IComboBox CreateComboBox()
        {
            return GetControlFactory().CreateComboBox();
        }

        protected abstract IControlFactory GetControlFactory();

        protected abstract string GetUnderlyingAutoCompleteSourceToString(IComboBox controlHabanero);

        protected void AssertAutoCompleteSourcesSame(IComboBox comboBox)
        {
            AutoCompleteSource AutoCompleteSource = comboBox.AutoCompleteSource;
            string AutoCompleteSourceToString = GetUnderlyingAutoCompleteSourceToString(comboBox);
            Assert.AreEqual(AutoCompleteSource.ToString(), AutoCompleteSourceToString);
        }
       


        [Test]
        public virtual void TestConversion_AutoCompleteSource_None()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.None;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.None, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_AllSystemSources()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.AllSystemSources;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.AllSystemSources, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_AllUrl()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.AllUrl;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.AllUrl, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_CustomSource()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.CustomSource, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_FileSystem()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.FileSystem;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.FileSystem, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_FileSystemDirectories()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.FileSystemDirectories, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_HistoryList()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.HistoryList;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.HistoryList, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_ListItems()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.ListItems;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.ListItems, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public virtual void TestConversion_AutoCompleteSource_RecentlyUsedList()
        {
            //---------------Set up test pack-------------------
            IComboBox control = CreateComboBox();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.AutoCompleteSource = AutoCompleteSource.RecentlyUsedList;
            //---------------Test Result -----------------------
            Assert.AreEqual(AutoCompleteSource.RecentlyUsedList, control.AutoCompleteSource);
            AssertAutoCompleteSourcesSame(control);
        }

        [Test]
        public void Test_AddingItems()
        {
            //---------------Set up test pack-------------------
            IComboBox comboBox = CreateComboBox();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, comboBox.Items.Count);
            //---------------Execute Test ----------------------
            comboBox.Items.Add("");
            comboBox.Items.Add("Bob");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, comboBox.Items.Count);
        }

      
    }

}
