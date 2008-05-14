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
using Habanero.UI.Forms;
using Habanero.UI.Grid;
using NMock;
using NMock.Constraints;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    [TestFixture]
    public class TestReadOnlyGridButtonControl : TestUsingDatabase
    {
        private IBusinessObject itsBo;
        private Mock itsGridMock;
        private IReadOnlyGrid itsGrid;
        private ReadOnlyGridButtonControl itsButtons;
        private Mock itsObjectEditorMock;
        private IBusinessObjectEditor itsEditor;
        private Mock itsObjectCreatorMock;
        private IBusinessObjectCreator itsCreator;
        private Mock itsExceptionNotifierMock;
        private IExceptionNotifier itsExceptionNotifier;
        private Guid itsContactPersonID;
        private Guid itsAddressID;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            itsBo = MyBO.LoadDefaultClassDef().CreateNewBusinessObject();
            SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            itsGridMock = new DynamicMock(typeof (IReadOnlyGrid));
            itsGrid = (IReadOnlyGrid)itsGridMock.MockInstance;
            itsButtons = new ReadOnlyGridButtonControl(itsGrid);
            itsObjectEditorMock = new DynamicMock(typeof (IBusinessObjectEditor));
            itsEditor = (IBusinessObjectEditor) itsObjectEditorMock.MockInstance;
            itsObjectCreatorMock = new DynamicMock(typeof (IBusinessObjectCreator));
            itsCreator = (IBusinessObjectCreator) itsObjectCreatorMock.MockInstance;
            itsExceptionNotifierMock = new DynamicMock(typeof(IExceptionNotifier));
            itsExceptionNotifier = (IExceptionNotifier)itsExceptionNotifierMock.MockInstance;

            GlobalRegistry.UIExceptionNotifier = itsExceptionNotifier;
        }

        [TearDown]
        public void CleanUp()
        {
            if (itsAddressID != Guid.Empty)
            {
                Address address = BOLoader.Instance.GetBusinessObjectByID<Address>(itsAddressID);
                if (address != null)
                {
                    address.Delete();
                    address.Save();
                }
            }

            if (itsContactPersonID != Guid.Empty)
            {
                ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
                if (contactPerson != null)
                {
                    contactPerson.Delete();
                    contactPerson.Save();
                }
            }
        }

        [Test]
        public void TestControlCreation()
        {
            Assert.AreEqual(3, itsButtons.Controls.Count);
            Assert.AreEqual("Delete", itsButtons.Controls[0].Name);
            Assert.AreEqual("Add", itsButtons.Controls[1].Name);
            Assert.AreEqual("Edit", itsButtons.Controls[2].Name);
            
        }

        [Test]
        public void TestEditButtonClickSuccessfulEdit()
        {
            itsGridMock.ExpectAndReturn("SelectedBusinessObject", itsBo, new object[] {});
            itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
            itsObjectEditorMock.ExpectAndReturn("EditObject", true, new object[] { itsBo, "default" });
            //itsGridMock.Expect("RefreshRow", new object[] { itsBo }) ;
            itsButtons.BusinessObjectEditor = itsEditor;

            itsButtons.ClickButton("Edit");
            itsObjectEditorMock.Verify();
            itsGridMock.Verify();
        }

        [Test]
        public void TestEditButtonClickUnsuccessfulEdit()
        {
            itsGridMock.ExpectAndReturn("SelectedBusinessObject", itsBo, new object[] {});
            itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
            itsObjectEditorMock.ExpectAndReturn("EditObject", false, new object[] { itsBo, "default" });
            //itsGridMock.ExpectNoCall("RefreshRow", new Type[] {typeof(object)});
            itsButtons.BusinessObjectEditor = itsEditor;

            itsButtons.ClickButton("Edit");
            itsObjectEditorMock.Verify();
            itsGridMock.Verify();
        }

        [Test]
        public void TestEditButtonClickNothingSelected()
        {
            itsGridMock.ExpectAndReturn("SelectedBusinessObject", null, new object[] {});
            itsObjectEditorMock.ExpectNoCall("EditObject", new Type[] {typeof (object), typeof(string)});
            //itsGridMock.ExpectNoCall("RefreshRow", new Type[] {typeof(object)});
            itsButtons.BusinessObjectEditor = itsEditor;

            itsButtons.ClickButton("Edit");
            itsObjectEditorMock.Verify();
            itsGridMock.Verify();
        }

        [Test, Ignore("not working, but working in debug")]
        public void TestAddButtonClickSuccessfulAdd()
        {
            itsGridMock.ExpectAndReturn("UIName", "default", new object[]{} );
            itsObjectCreatorMock.ExpectAndReturn("CreateBusinessObject", itsBo, new object[] {});
            
            itsGridMock.Expect("AddBusinessObject", new object[] {itsBo});
            itsButtons.BusinessObjectCreator = itsCreator;
            itsButtons.BusinessObjectEditor = itsEditor;

            itsButtons.ClickButton("Add");
            itsObjectCreatorMock.Verify();
            itsGridMock.Verify();
        }

        [Test, Ignore("not working, but working in debug")]
        public void TestAddButtonClickUnsuccessfulAdd()
        {
            itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
            itsObjectCreatorMock.ExpectAndReturn("CreateBusinessObject", null, new object[] {itsEditor, null, "default"});
            itsGridMock.ExpectNoCall("AddBusinessObject", new Type[] {typeof (object)});
            itsButtons.BusinessObjectCreator = itsCreator;
            itsButtons.BusinessObjectEditor = itsEditor;

            itsButtons.ClickButton("Add");
            itsObjectCreatorMock.Verify();
            itsGridMock.Verify();
        }

        [Test]
        public void TestDeletionProperties()
        {
            Assert.IsFalse(itsButtons.ShowDefaultDeleteButton);
            itsButtons.ShowDefaultDeleteButton = true;
            Assert.IsTrue(itsButtons.ShowDefaultDeleteButton);

            Assert.IsTrue(itsButtons.ConfirmDeletion);
            itsButtons.ConfirmDeletion = false;
            Assert.IsFalse(itsButtons.ConfirmDeletion);
        }

        // These two tests both write to the database.  If there is a way
        //   to mock these without writing then please change it, but I
        //   couldn't see how to mock a BO or a connection successfully
        [Test]
        public void TestDeleteButtonClickSuccessfulDelete()
        {
            ContactPerson bo = new ContactPerson();
            bo.Surname = "please delete me.";
            bo.Save();
            itsContactPersonID = bo.ContactPersonID.Value;

            BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
            boCol.Add(bo);
            
            itsGridMock.ExpectAndReturn("SelectedBusinessObjects", boCol);
            itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo);
            itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo);
            
            itsButtons.ShowDefaultDeleteButton = true;
            itsButtons.ConfirmDeletion = false;
            itsButtons.ClickButton("Delete");
            itsGridMock.Verify();

            ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
            Assert.IsNull(contactPerson);
        }

        [Test]
        public void TestDeleteButtonClickUnsuccessfulDelete()
        {
            ContactPerson person = new ContactPerson();
            person.Surname = "please delete me";
            person.Save();
            itsContactPersonID = person.ContactPersonID.Value;
            person.AddPreventDeleteRelationship();

            Address address = new Address();
            address.ContactPersonID = itsContactPersonID;
            address.Save();
            itsAddressID = address.AddressID;

            BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
            boCol.Add(person);

            itsGridMock.ExpectAndReturn("SelectedBusinessObjects", boCol);
            itsExceptionNotifierMock.Expect("Notify", new IsAnything(), new IsAnything(), new IsAnything());
            itsGridMock.ExpectNoCall("SelectedBusinessObject");
            
            itsButtons.ShowDefaultDeleteButton = true;
            itsButtons.ConfirmDeletion = false;
            itsButtons.ClickButton("Delete");
            itsGridMock.Verify();
            
            ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
            Assert.IsNotNull(contactPerson);
        }

        [Test]
        public void TestDeleteButtonClickNothingSelected()
        {
            itsGridMock.ExpectAndReturn("SelectedBusinessObjects", new BusinessObjectCollection<MyBO>());
            itsGridMock.ExpectNoCall("SelectedBusinessObject");

            itsButtons.ShowDefaultDeleteButton = true;
            itsButtons.ConfirmDeletion = false;
            itsButtons.ClickButton("Delete");
            itsGridMock.Verify();
        }
    }
}