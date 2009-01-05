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

using System.Data;
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestEditableDataProvider.
    /// </summary>
    [TestFixture]
    public class TestEditableDataProvider : TestDataSetProvider
    {
        protected override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            return new EditableDataSetProvider(col);
        }

        [Test]
        public void TestUpdateRowUpdatesBusinessObject()
        {
            SetupTestData();
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            Assert.AreEqual("bo1prop1updated", itsBo1.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestAcceptChangesSavesBusinessObjects()
        {
            SetupTestData();
            SetupSaveExpectation();
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestAddRowCreatesBusinessObjectThroughCollection()
        {
            SetupTestData();
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            bo.SetPropertyValue("TestProp", "bo1prop1");
            bo.SetPropertyValue("TestProp2", "s1");
            bo.Save();
            boCollection.Add(bo);

            MyBO bo2 = new MyBO();
            bo2.SetPropertyValue("TestProp", "bo2prop1");
            bo2.SetPropertyValue("TestProp2", "s2");
            bo2.Save();
            boCollection.Add(bo2);

            _dataSetProvider = new EditableDataSetProvider(boCollection);
            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, boCollection.Count);
            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

            //---------------Execute Test ----------------------
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});

            //---------------Test Result -----------------------
            Assert.AreEqual
                (1, boCollection.CreatedBusinessObjects.Count,
                 "Adding a row to the table should use the collection to create the object");
            //Assert.AreEqual(2, boCollection.Count, "Adding a row to the table should not add a bo to the main collection");
            Assert.AreEqual(3, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
            //Note: This behaviour has changed and we need to asses the impact of this change.
        }

        [Test]
        public void TestDeleteRowMarksBOAsDeleted()
        {
            SetupTestData();
            itsTable.Rows[0].Delete();
            //TODO: Discuss with peter
            //I have changed this what are the consequences
            // : Assert.AreEqual(2, _collection.Count, "Deleting a row shouldn't remove any Bo's from the collection.");
            Assert.AreEqual(1, _collection.Count, "Deleting a row shouldn't remove any Bo's from the collection.");
            int numDeleted = 0;
//
            //TODO: Discuss with peter
            //foreach (BusinessObject businessObjectBase in _collection)
//            {
//                if (businessObjectBase.Status.IsDeleted)
//                {
//                    numDeleted++;
//                }
//            }

            numDeleted = _collection.MarkedForDeleteBusinessObjects.Count;
            Assert.AreEqual(1, numDeleted, "BO should be marked as deleted.");
        }

        [Test, Ignore("Changes have been made recently (Brett?) that are now breaking editable grids.")]
        public void TestRejectChangesRemovesNewRow()
        {
            //---------------Set up test pack-------------------
            SetupTestData();
            int originalCount = _collection.Count;
            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
            //---------------Assert Precondition----------------
            //Assert.AreEqual(originalCount + 1, _collection.Count);

            //---------------Execute Test ----------------------
            itsTable.RejectChanges();
            //---------------Test Result -----------------------

        }

        [Test]
        public void TestAcceptChangesSavesNewBusinessObjects()
        {
            SetupTestData();
            SetupSaveExpectation();
            ((EditableDataSetProvider) _dataSetProvider).Connection = itsConnection;
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});
            itsTable.AcceptChanges();
        }


        [Test]
        public void TestDeleteRowDeletesBOOnSave()
        {
            SetupTestData();
            SetupSaveExpectation();

            itsTable.AcceptChanges();
            itsTable.Rows[0].Delete();
            itsTable.AcceptChanges();
        }

        [Test,
         Ignore(
             "Brett - to consult with peter by fundamentally changing the way BO's respond to edits we are fundamentally altering the way these data providers work"
             )]
        public void TestRevertChangesRevertsBoValues()
        {
            SetupTestData();
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            itsTable.RejectChanges();
            Assert.AreEqual("bo1prop1", itsBo1.GetPropertyValue("TestProp"));
            itsTable.AcceptChanges();
            itsTable.Rows[0].Delete();
            Assert.IsTrue(itsBo1.Status.IsDeleted);
            itsTable.RejectChanges();
            Assert.IsFalse(itsBo1.Status.IsDeleted);
        }

        [Test]
        public void TestAddBOToCollectionAddsRow()
        {
            SetupTestData();
            //IBusinessObject newBo = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject newBo = _classDef.CreateNewBusinessObject();
            _collection.Add(newBo);
            Assert.AreEqual(3, itsTable.Rows.Count);
        }

        [Test]
        public void TestAddBOToCollectionAddsCorrectValues()
        {
            SetupTestData();
            //IBusinessObject newBo = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject newBo = _classDef.CreateNewBusinessObject();
            newBo.SetPropertyValue("TestProp", "TestVal");
            _collection.Add(newBo);
            Assert.AreEqual("TestVal", itsTable.Rows[2][1]);
        }

        [Test, ExpectedException(typeof (DuplicateNameException))]
        public void TestDuplicateColumnNames()
        {
            SetupTestData();
            BOMapper mapper = new BOMapper(_collection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef("duplicateColumns").GetUIGridProperties());
        }

        [Test]
        public void TestFind()
        {
            SetupTestData();
            IBusinessObject bo = ((EditableDataSetProvider) _dataSetProvider).Find(0);
            Assert.AreEqual(_collection[0], bo);

            MyBO unlistedBO = new MyBO();
            Assert.AreEqual(-1, ((EditableDataSetProvider) _dataSetProvider).FindRow(unlistedBO));
        }

        [Test]
        public void TestGetConnection()
        {
            SetupTestData();
            Assert.IsNull(((EditableDataSetProvider) _dataSetProvider).Connection);
        }
    }
}