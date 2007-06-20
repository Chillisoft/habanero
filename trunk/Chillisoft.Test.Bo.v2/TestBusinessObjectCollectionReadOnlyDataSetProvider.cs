using Habanero.Bo;
using Habanero.Generic;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollectionReadOnlyDataSetProvider.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollectionReadOnlyDataSetProvider : TestBusinessObjectCollectionDataProvider
    {
        protected override IDataSetProvider CreateDataSetProvider(BusinessObjectCollection col)
        {
            return new BOCollectionReadOnlyDataSetProvider(itsCollection);
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
            BusinessObject bo3 = itsClassDef.CreateNewBusinessObject(itsConnection);
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "bo3prop2");
            itsCollection.Add(bo3);
            Assert.AreEqual(3, itsTable.Rows.Count);
            Assert.AreEqual("bo3prop1", itsTable.Rows[2][1]);
        }

        [Test]
        public void TestAddBusinessObjectAndUpdateUpdatesNewRow()
        {
            BusinessObject bo3 = itsClassDef.CreateNewBusinessObject(itsConnection);
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