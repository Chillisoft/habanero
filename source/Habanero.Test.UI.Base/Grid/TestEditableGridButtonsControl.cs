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
using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Grid
{
    public abstract class TestEditableGridButtonsControl //: TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract void AddControlToForm(IControlChilli cntrl);
        protected abstract IControlFactory GetControlFactory();
        //protected abstract IGridBase CreateReadOnlyGridControl();

        //TODO: get Win versions working
        //[TestFixture]
        //public class TestEditableGridButtonsControlWin : TestEditableGridButtonsControl
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }

        //protected override void AddControlToForm(IControlChilli cntrl)
        //{
        //    //TODO fix this
        //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
        //    frm.Controls.Add((Gizmox.WebGUI.Forms.Control) cntrl);
        //}

        //}

        [TestFixture]
        public class TestEditableGridButtonsControlGiz : TestEditableGridButtonsControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            protected override void AddControlToForm(IControlChilli cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
            }
        }

        public void TestCreateEditableGridButtonsControl()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IEditableGridButtonsControl editableGridButtonsControl = GetControlFactory().CreateEditableGridButtonsControl();
            //---------------Test Result ----------------------
            Assert.IsNotNull(editableGridButtonsControl);
            Assert.IsInstanceOfType(typeof(IEditableGridButtonsControl),editableGridButtonsControl);
        }

        [Test]
        public void TestCreateEditableGridButtonsControl_TestButtonsAdded()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IEditableGridButtonsControl buttonsControl = GetControlFactory().CreateEditableGridButtonsControl();
            //---------------Test Result ----------------------
            //IReadOnlyGridButtonsControl readOnlyGridButtonsControl = (IReadOnlyGridButtonsControl)grid;
           // AddControlToForm(buttonsControl);
            //Cancel button should be first (This is right aligned so that means it will be furthest right if visible
            Assert.AreEqual(2, buttonsControl.Controls.Count);
            IButton btn = (IButton)buttonsControl.Controls[0];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Cancel", btn.Name);
            Assert.IsTrue(btn.Visible);           

            //Save button should be second (This is right aligned so that means it will be second from left
            btn = (IButton)buttonsControl.Controls[1];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Save", btn.Name);
            Assert.IsTrue(btn.Visible);
        }

        [Test]
        public void TestCreateEditableGridButtonsControl_GetControlUsingIndexer()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IEditableGridButtonsControl buttonsControl = GetControlFactory().CreateEditableGridButtonsControl();
            //---------------Test Result ----------------------

            Assert.AreEqual(2, buttonsControl.Controls.Count);

            //Cancel Visible and Text Correct
            IButton btn = buttonsControl["Cancel"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Cancel", btn.Name);
            Assert.IsTrue(btn.Visible);

            //Save Visible and Text Correct
            btn = buttonsControl["Save"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Save", btn.Text);
            Assert.IsTrue(btn.Visible);

           
        }

        [Test]
        public void TestCancelButtonClick()
        {
            //---------------Set up test pack-------------------
            IEditableGridButtonsControl buttonsControl = GetControlFactory().CreateEditableGridButtonsControl();
            //AddControlToForm(readOnlyGridButtonsControl);
            IButton btn = buttonsControl["Cancel"];
            bool cancelClicked = false;

            buttonsControl.CancelClicked += delegate { cancelClicked = true; };
            //---------------Execute Test ----------------------
            btn.PerformClick();

            //---------------Test Result ----------------------
            Assert.IsTrue(cancelClicked);
        }

        [Test]
        public void TestSaveButtonClick()
        {
            //---------------Set up test pack-------------------
            IEditableGridButtonsControl buttonsControl = GetControlFactory().CreateEditableGridButtonsControl();
            //AddControlToForm(readOnlyGridButtonsControl);
            IButton btn = buttonsControl["Save"];
            bool saveClicked = false;

            buttonsControl.SaveClicked += delegate { saveClicked = true; };
            //---------------Execute Test ----------------------
            btn.PerformClick();

            //---------------Test Result ----------------------
            Assert.IsTrue(saveClicked);
        }

    }
}
