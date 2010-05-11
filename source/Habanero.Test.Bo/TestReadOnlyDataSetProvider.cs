// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestReadOnlyDataSetProvider : TestDataSetProvider
    {


        protected override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            _dataSetProvider = new ReadOnlyDataSetProvider(col);
            _dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents = true;
            return _dataSetProvider;
        }

        [SetUp]
        public void DoSetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }
//
//        [Test]
//        public void TestUpdateBusinessObjectUpdatesRow()
//        {
//            SetupTestData();
//            itsBo1.SetPropertyValue("TestProp", "UpdatedValue");
//            Assert.AreEqual("UpdatedValue", itsTable.Rows[0][1]);
//        }

//        [Test]
//        public void TestAddBusinessObjectAddsRow()
//        {
//            SetupTestData();
//            //IBusinessObject bo3 = _classDef.CreateNewBusinessObject(itsConnection);
//            IBusinessObject bo3 = _classDef.CreateNewBusinessObject();
//            bo3.SetPropertyValue("TestProp", "bo3prop1");
//            bo3.SetPropertyValue("TestProp2", "s1");
//            _collection.Add(bo3);
//            Assert.AreEqual(3, itsTable.Rows.Count);
//            Assert.AreEqual("bo3prop1", itsTable.Rows[2][1]);
//        }

//        [Test]
//        public void TestAddBusinessObjectAndUpdateUpdatesNewRow()
//        {
//            SetupTestData();
//            //IBusinessObject bo3 = _classDef.CreateNewBusinessObject(itsConnection);
//            IBusinessObject bo3 = _classDef.CreateNewBusinessObject();
//            bo3.SetPropertyValue("TestProp", "bo3prop1");
//            bo3.SetPropertyValue("TestProp2", "s2");
//            _collection.Add(bo3);
//            bo3.SetPropertyValue("TestProp", "UpdatedValue");
//            Assert.AreEqual("UpdatedValue", itsTable.Rows[2][1]);
//        }
//
//        [Test]
//        public void TestRemoveBusinessObjectRemovesRow()
//        {
//            SetupTestData();
//            _collection.Remove(itsBo1);
//            Assert.AreEqual(1, itsTable.Rows.Count);
//        }
//
//        [Test]
//        public void TestOrderItemAddAndFindBO()
//        {
//            SetupTestData();
//            OrderItem car = OrderItem.AddOrder1Car();
//            OrderItem chair = OrderItem.AddOrder2Chair();
//            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
//            col.LoadAll();
//
//            ReadOnlyDataSetProvider provider = new ReadOnlyDataSetProvider(col);
//            UIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
//            Assert.AreEqual(2, provider.GetDataTable(uiGrid).Rows.Count);
//            Assert.AreEqual(0, provider.FindRow(car));
//            Assert.AreEqual(1, provider.FindRow(chair));
//            Assert.AreEqual(car, provider.Find(0));
//            Assert.AreEqual(chair, provider.Find(1));
//            Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=1;OrderItem.Product=car"));
//            Assert.AreEqual(chair, provider.Find("OrderItem.OrderNumber=2;OrderItem.Product=chair"));
//
//            OrderItem roof = OrderItem.AddOrder3Roof();
//            Assert.AreEqual(2, provider.GetDataTable(uiGrid).Rows.Count);
//            Assert.AreEqual(-1, provider.FindRow(roof));
//            bool causedException = false;
//            try
//            {
//                provider.Find(2);
//            }
//            catch (Exception e)
//            {
//                causedException = true;
//                Assert.AreEqual(typeof(IndexOutOfRangeException), e.GetType());
//            }
//            Assert.IsTrue(causedException);
//            Assert.IsNull(provider.Find("OrderItem.OrderNumber=3;OrderItem.Product=roof"));
//
//            col.Add(roof);
//            Assert.AreEqual(3, provider.GetDataTable(uiGrid).Rows.Count);
//            Assert.AreEqual(2, provider.FindRow(roof));
//            Assert.AreEqual(roof, provider.Find(2));
//            Assert.AreEqual(roof, provider.Find("OrderItem.OrderNumber=3;OrderItem.Product=roof"));
//        }
//
//        [Test]
//        public void TestOrderItemRemove()
//        {
//            SetupTestData();
//            OrderItem.ClearTable();
//            OrderItem.AddOrder1Car();
//            OrderItem chair = OrderItem.AddOrder2Chair();
//            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
//            col.LoadAll();
//
//            ReadOnlyDataSetProvider provider = new ReadOnlyDataSetProvider(col);
//            UIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
//            DataTable table = provider.GetDataTable(uiGrid);
//            Assert.AreEqual(2, table.Rows.Count);
//
//            col.Remove(chair);
//            Assert.AreEqual(1, table.Rows.Count);
//            Assert.AreEqual(-1, provider.FindRow(chair));
//            bool causedException = false;
//            try
//            {
//                provider.Find(1);
//            }
//            catch (Exception e)
//            {
//                causedException = true;
//                Assert.AreEqual(typeof(IndexOutOfRangeException), e.GetType());
//            }
//            Assert.IsTrue(causedException);
//            Assert.IsNull(provider.Find("OrderNumber=2 AND Product=chair"));
//        }
//
//        [Test]
//        public void TestOrderItemChangeItemAndFind()
//        {
//            SetupTestData();
//            BORegistry.BusinessObjectManager.ClearLoadedObjects();
//            OrderItem.ClearTable();
//            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
//            col.LoadAll();
//            Assert.AreEqual(0, col.Count);
//
//            OrderItem car = OrderItem.AddOrder1Car();
//            OrderItem.AddOrder2Chair();
//            col = new BusinessObjectCollection<OrderItem>();
//            col.LoadAll();
//            Assert.AreEqual(2, col.Count);
//            ReadOnlyDataSetProvider provider = new ReadOnlyDataSetProvider(col);
//            UIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
//            DataTable table = provider.GetDataTable(uiGrid);
//            Assert.AreEqual(2, table.Rows.Count);
//
//            car.OrderNumber = 11;
//            Assert.AreEqual(0, provider.FindRow(car));
//            Assert.AreEqual(car, provider.Find(0));
//            Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=11;OrderItem.Product=car")); 
//            
//            car.Save();
//            Assert.AreEqual(0, provider.FindRow(car));
//            Assert.AreEqual(car, provider.Find(0));
//            Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=11;OrderItem.Product=car"));
//            
//            car.OrderNumber = 12;
//            Assert.AreEqual(0, provider.FindRow(car));
//            Assert.AreEqual(car, provider.Find(0));
//            Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=12;OrderItem.Product=car"));
//            
//            car.OrderNumber = 13;
//            Assert.AreEqual(0, provider.FindRow(car));
//            Assert.AreEqual(car, provider.Find(0));
//            Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=13;OrderItem.Product=car"));
//        }
    }
}