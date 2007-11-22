//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using NUnit.Framework;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBOCollectionEditableDataProvider.
    /// </summary>
    [TestFixture]
    public class TestBOCollectionEditableDataProvider : TestBOCollectionDataProvider
    {
        protected override IDataSetProvider CreateDataSetProvider(BusinessObjectCollection<BusinessObject> col)
        {
            return new BOCollectionEditableDataSetProvider(itsCollection);
        }
		
        [Test]
        public void TestUpdateRowUpdatesBusinessObject()
        {
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            Assert.AreEqual("bo1prop1updated", itsBo1.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestAcceptChangesSavesBusinessObjects()
        {
        	SetupSaveExpectation();
			itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestAddRowAddsBusinessObjectToCol()
        {
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});
            Assert.AreEqual(3, itsCollection.Count, "Adding a row to the table should add a bo to the collection");
        }

        [Test]
        public void TestDeleteRowMarksBOAsDeleted()
        {
            itsTable.Rows[0].Delete();
            Assert.AreEqual(2, itsCollection.Count, "Deleting a row shouldn't remove any Bo's from the collection.");
            int numDeleted = 0;
            foreach (BusinessObject businessObjectBase in itsCollection)
            {
                if (businessObjectBase.State.IsDeleted)
                {
                    numDeleted++;
                }
            }
            Assert.AreEqual(1, numDeleted, "BO should be marked as deleted.");
        }

        [Test]
        public void TestAcceptChangesSavesNewBusinessObjects()
        {
        	SetupSaveExpectation();
        	((BOCollectionEditableDataSetProvider) itsProvider).Connection = itsConnection;
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});
            itsTable.AcceptChanges();
        }


    	[Test]
        public void TestDeleteRowDeletesBOOnSave()
        {
			SetupSaveExpectation();

            itsTable.AcceptChanges();
            itsTable.Rows[0].Delete();
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestRevertChangesRevertsBoValues()
        {
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            itsTable.RejectChanges();
            Assert.AreEqual("bo1prop1", itsBo1.GetPropertyValue("TestProp"));
            itsTable.AcceptChanges();
            itsTable.Rows[0].Delete();
            Assert.IsTrue(itsBo1.State.IsDeleted);
            itsTable.RejectChanges();
            Assert.IsFalse(itsBo1.State.IsDeleted);
        }

        [Test]
        public void TestAddBOToCollectionAddsRow()
        {
            BusinessObject newBo = itsClassDef.CreateNewBusinessObject(itsConnection);
            itsCollection.Add(newBo);
            Assert.AreEqual(3, itsTable.Rows.Count);
        }

        [Test]
        public void TestAddBOToCollectionAddsCorrectValues()
        {
            BusinessObject newBo = itsClassDef.CreateNewBusinessObject(itsConnection);
            newBo.SetPropertyValue("TestProp", "TestVal");
            itsCollection.Add(newBo);
            Assert.AreEqual("TestVal", itsTable.Rows[2][1]);
        }
    }
}