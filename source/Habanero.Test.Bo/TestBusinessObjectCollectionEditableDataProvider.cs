using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using NUnit.Framework;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollectionEditableDataProvider.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollectionEditableDataProvider : TestBusinessObjectCollectionDataProvider
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
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
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
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            ((BOCollectionEditableDataSetProvider) itsProvider).Connection = itsConnection;
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestDeleteRowDeletesBOOnSave()
        {
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

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