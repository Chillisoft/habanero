using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollectionReadOnlyDataSetProvider.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollectionReadOnlyDataSetProvider : TestBusinessObjectCollectionDataProvider
    {
        protected override IDataSetProvider CreateDataSetProvider(BusinessObjectBaseCollection col)
        {
            return new BusinessObjectCollectionReadOnlyDataSetProvider(itsCollection);
        }

        [Test]
        public void TestUpdateBusinessObjectUpdatesRow()
        {
            itsBo1.SetPropertyValue("TestProp", "UpdatedValue");
            Assert.AreEqual("UpdatedValue", itsTable.Rows[0][1]);
        }

        [Test]
        public void TestAddBusinessObjectAddsRow()
        {
            BusinessObjectBase bo3 = itsClassDef.CreateNewBusinessObject(itsConnection);
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "bo3prop2");
            itsCollection.Add(bo3);
            Assert.AreEqual(3, itsTable.Rows.Count);
            Assert.AreEqual("bo3prop1", itsTable.Rows[2][1]);
        }

        [Test]
        public void TestAddBusinessObjectAndUpdateUpdatesNewRow()
        {
            BusinessObjectBase bo3 = itsClassDef.CreateNewBusinessObject(itsConnection);
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "bo3prop2");
            itsCollection.Add(bo3);
            bo3.SetPropertyValue("TestProp", "UpdatedValue");
            Assert.AreEqual("UpdatedValue", itsTable.Rows[2][1]);
        }

        [Test]
        public void TestRemoveBusinessObjectRemovesRow()
        {
            itsCollection.RemoveAt(itsBo1);
            Assert.AreEqual(1, itsTable.Rows.Count);
        }
    }
}