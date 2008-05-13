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


using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
//using Habanero.UI.Forms;
//using Habanero.UI.Grid;
//using NMock;
//using NMock.Constraints;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public abstract class TestReadOnlyGridButtonControl //: TestUsingDatabase
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
        protected abstract void AddControlToForm(IChilliControl cntrl);
        protected abstract IControlFactory GetControlFactory();
        //protected abstract IGridBase CreateReadOnlyGridWithButtons();

        //[TestFixture]
        //public class TestReadOnlyGridButtonControlWin : TestReadOnlyGridButtonControl
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new WinControlFactory();
        //    }
        
            //protected override void AddControlToForm(IChilliControl cntrl)
            //{
            //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //    frm.Controls.Add((Gizmox.WebGUI.Forms.Control) cntrl);
            //}

        //}
        [TestFixture]
        public class TestReadOnlyGridButtonControlGiz : TestReadOnlyGridButtonControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new GizmoxControlFactory();
            }

            protected override void AddControlToForm(IChilliControl cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
            }
        }

        public void TestCreateReadOnlyGridButtonControl()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IChilliControl grid = GetControlFactory().CreateReadOnlyGridButtonsControl();
            //---------------Test Result ----------------------
            Assert.IsNotNull(grid);
            Assert.IsTrue(grid is IReadOnlyGridButtonsControl);
        }

        [Test, Ignore("Peter please review, we cannot get this to work. Test in ButtonControl")]
        public void TestCreateReadOnlyGridButtonControl_TestButtonsAdded()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IChilliControl grid = GetControlFactory().CreateReadOnlyGridButtonsControl();
            //---------------Test Result ----------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = (IReadOnlyGridButtonsControl)grid;
            AddControlToForm(readOnlyGridButtonsControl);
            //Delete button should be first (This is right aligned so that means it will be furthest right if visible
            Assert.AreEqual(3, readOnlyGridButtonsControl.Controls.Count);
            IButton btn = (IButton) readOnlyGridButtonsControl.Controls[0];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Delete", btn.Name);

            //Edit button should be second (This is right aligned so that means it will be second from left
            btn = (IButton)readOnlyGridButtonsControl.Controls[1];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Edit", btn.Name);

            //Add button should be last (This is right aligned so that means it will be the first button from the left 
            //(last from the right)
            btn = (IButton)readOnlyGridButtonsControl.Controls[2];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Add", btn.Name);
        }

        [Test]
        public void TestCreateReadOnlyGridButtonControl_GetControlUsingIndexer()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IChilliControl grid = GetControlFactory().CreateReadOnlyGridButtonsControl();
            //---------------Test Result ----------------------

            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = (IReadOnlyGridButtonsControl)grid;
            AddControlToForm(readOnlyGridButtonsControl);
            Assert.AreEqual(3, readOnlyGridButtonsControl.Controls.Count);
            IButton btn = readOnlyGridButtonsControl["Delete"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Delete", btn.Name);

            btn = readOnlyGridButtonsControl["Edit"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Edit", btn.Name);

            btn = readOnlyGridButtonsControl["Add"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Add", btn.Name);
        }
        [Test, Ignore("To be implemented")]
        public void TestDeleteButtonClick()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = GetControlFactory().CreateReadOnlyGridButtonsControl();
            AddControlToForm(readOnlyGridButtonsControl);  
            IButton btn = readOnlyGridButtonsControl["Delete"];         
            //---------------Execute Test ----------------------
            btn.PerformClick();

            //---------------Test Result ----------------------
            Assert.IsTrue(false);
        }
        //[Test]
        //public void TestEditButtonClickSuccessfulEdit()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo, new object[] {});
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
        //    //itsObjectEditorMock.ExpectAndReturn("EditObject", true, new object[] { bo, "default" });
        //    ////itsGridMock.Expect("RefreshRow", new object[] { bo }) ;
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Edit");
        //    //itsObjectEditorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestEditButtonClickUnsuccessfulEdit()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo, new object[] {});
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
        //    //itsObjectEditorMock.ExpectAndReturn("EditObject", false, new object[] { bo, "default" });
        //    ////itsGridMock.ExpectNoCall("RefreshRow", new Type[] {typeof(object)});
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Edit");
        //    //itsObjectEditorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestEditButtonClickNothingSelected()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", null, new object[] {});
        //    //itsObjectEditorMock.ExpectNoCall("EditObject", new Type[] {typeof (object), typeof(string)});
        //    ////itsGridMock.ExpectNoCall("RefreshRow", new Type[] {typeof(object)});
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Edit");
        //    //itsObjectEditorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestAddButtonClickSuccessfulAdd()
        //{
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[]{} );
        //    //itsObjectCreatorMock.ExpectAndReturn("CreateObject", bo, new object[] {itsEditor, null, "default"});
        //    //itsGridMock.Expect("AddBusinessObject", new object[] {bo});
        //    //itsButtons.ObjectCreator = itsCreator;
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Add");
        //    //itsObjectCreatorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestAddButtonClickUnsuccessfulAdd()
        //{
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
        //    //itsObjectCreatorMock.ExpectAndReturn("CreateObject", null, new object[] {itsEditor, null, "default"});
        //    //itsGridMock.ExpectNoCall("AddBusinessObject", new Type[] {typeof (object)});
        //    //itsButtons.ObjectCreator = itsCreator;
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Add");
        //    //itsObjectCreatorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestDeletionProperties()
        //{
        //    //Assert.IsFalse(itsButtons.ShowDefaultDeleteButton);
        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //Assert.IsTrue(itsButtons.ShowDefaultDeleteButton);

        //    //Assert.IsTrue(itsButtons.ConfirmDeletion);
        //    //itsButtons.ConfirmDeletion = false;
        //    //Assert.IsFalse(itsButtons.ConfirmDeletion);
        //}

        //// These two tests both write to the database.  If there is a way
        ////   to mock these without writing then please change it, but I
        ////   couldn't see how to mock a BO or a connection successfully
        //[Test]
        //public void TestDeleteButtonClickSuccessfulDelete()
        //{
        //    //ContactPerson bo = new ContactPerson();
        //    //bo.Surname = "please delete me.";
        //    //bo.Save();
        //    //itsContactPersonID = bo.ContactPersonID.Value;

        //    //BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
        //    //boCol.Add(bo);
            
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObjects", boCol);
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo);
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo);
            
        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //itsButtons.ConfirmDeletion = false;
        //    //itsButtons.ClickButton("Delete");
        //    //itsGridMock.Verify();

        //    //ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
        //    //Assert.IsNull(contactPerson);
        //}

        //[Test]
        //public void TestDeleteButtonClickUnsuccessfulDelete()
        //{
        //    //ContactPerson person = new ContactPerson();
        //    //person.Surname = "please delete me";
        //    //person.Save();
        //    //itsContactPersonID = person.ContactPersonID.Value;
        //    //person.AddPreventDeleteRelationship();

        //    //Address address = new Address();
        //    //address.ContactPersonID = itsContactPersonID;
        //    //address.Save();
        //    //itsAddressID = address.AddressID;

        //    //BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
        //    //boCol.Add(person);

        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObjects", boCol);
        //    //itsExceptionNotifierMock.Expect("Notify", new IsAnything(), new IsAnything(), new IsAnything());
        //    //itsGridMock.ExpectNoCall("SelectedBusinessObject");
            
        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //itsButtons.ConfirmDeletion = false;
        //    //itsButtons.ClickButton("Delete");
        //    //itsGridMock.Verify();
            
        //    //ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
        //    //Assert.IsNotNull(contactPerson);
        //}

        //[Test]
        //public void TestDeleteButtonClickNothingSelected()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObjects", new BusinessObjectCollection<MyBO>());
        //    //itsGridMock.ExpectNoCall("SelectedBusinessObject");

        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //itsButtons.ConfirmDeletion = false;
        //    //itsButtons.ClickButton("Delete");
        //    //itsGridMock.Verify();
        //}
    }

}