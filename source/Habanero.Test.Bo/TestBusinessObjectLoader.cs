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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public abstract class TestBusinessObjectLoader
    {
        protected abstract void SetupDataAccessor();

        protected abstract void DeleteEnginesAndCars();

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDataAccessor();
        }

        [TearDown]
        public virtual void TearDownTest()
        {
           
        }

        [TestFixture]
        public class TestBusinessObjectLoaderInMemory : TestBusinessObjectLoader
        {
            private DataStoreInMemory _dataStore;
            protected override void SetupDataAccessor()
            {
                _dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
            }

            protected override void DeleteEnginesAndCars()
            {
                // do nothing
            }

            [Test]
            public void TestRefreshLoadedCollection_DeletedItem()
            {
                //---------------Set up test pack-------------------
                DataStoreInMemory dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
                ContactPersonTestBO.LoadDefaultClassDef();
                DateTime now = DateTime.Now;
                ContactPersonTestBO cp1 = new ContactPersonTestBO();
                cp1.DateOfBirth = now;
                cp1.Surname = Guid.NewGuid().ToString("N");
                cp1.Save();
                ContactPersonTestBO cp2 = new ContactPersonTestBO();
                cp2.DateOfBirth = now;
                cp2.Surname = Guid.NewGuid().ToString("N");
                cp2.Save();
                Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
                BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

                dataStore.Remove(cp2);
                //---------------Execute Test ----------------------
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                Assert.Contains(cp1, col);
                //---------------Tear Down -------------------------
            }
        }

        [TestFixture]
        public class TestBusinessObjectLoaderDB : TestBusinessObjectLoader
        {
            //TODO: stop this using the BOLoader
            [SetUp]
            public override void SetupTest()
            {
                base.SetupTest();
                ContactPersonTestBO.DeleteAllContactPeople();
            }

            protected override void DeleteEnginesAndCars()
            {
                Engine.DeleteAllEngines();
                Car.DeleteAllCars();

            }

            public TestBusinessObjectLoaderDB()
            {
                new TestUsingDatabase().SetupDBConnection();
            }

            protected override void SetupDataAccessor()
            {
                BORegistry.DataAccessor = new DataAccessorDB();
            }

         
            [Test]
            public void TestGetBusinessObject_SelectQuery_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = new ContactPersonTestBO();
                cp.Surname = Guid.NewGuid().ToString("N");
                cp.FirstName = Guid.NewGuid().ToString("N");
                cp.Save();
                BusinessObject.AllLoadedBusinessObjects().Clear();
                GCCollectAll();

                SelectQuery query = new SelectQuery(new Criteria("Surname", Criteria.Op.Equals, cp.Surname));
                query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", ""));
                query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
                query.Source = cp.ClassDef.TableName;

                //---------------Execute Test ----------------------
                ContactPersonTestBO loadedCp =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCp, cp);
                Assert.AreEqual(cp.ContactPersonID, loadedCp.ContactPersonID);
                Assert.AreEqual(cp.Surname, loadedCp.Surname);
                Assert.IsTrue(String.IsNullOrEmpty(loadedCp.FirstName), "Firstname is not being loaded"); // not being loaded
                Assert.IsFalse(loadedCp.State.IsNew);
                Assert.IsFalse(loadedCp.State.IsDeleted);
                Assert.IsFalse(loadedCp.State.IsDirty);
                Assert.IsTrue(loadedCp.State.IsValid());
            }

            //Test After load not yet done.

            private static void GCCollectAll()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


            [Test]
            public void TestGetBusinessObject_ReturnsSubType_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
                BusinessObject.AllLoadedBusinessObjects().Clear();

                //---------------Execute Test ----------------------
                Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(circle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestGetBusinessObject_ReturnsSubType_TwoLevelsDeep_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                BusinessObject.AllLoadedBusinessObjects().Clear();

                //---------------Execute Test ----------------------
                Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(filledCircle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestGetBusinessObject_ReturnsSubType_TwoLevelsDeep_DiscriminatorShared_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                BusinessObject.AllLoadedBusinessObjects().Clear();

                //---------------Execute Test ----------------------
                Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(filledCircle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestLoad_SingleTableInheritance_Fresh()
            {
                //---------------Set up test pack-------------------
                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();

                //---------------Execute Test ----------------------
                BusinessObject.AllLoadedBusinessObjects().Clear();
                CircleNoPrimaryKey loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(circle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCircle, circle);
                Assert.AreEqual(circle.Radius, loadedCircle.Radius);
                Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
            }

            [Test]
            public void TestLoad_SingleTableInheritance_Hierarchy_Fresh()
            {
                //---------------Set up test pack-------------------
                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                
                //---------------Execute Test ----------------------
                BusinessObject.AllLoadedBusinessObjects().Clear();
                FilledCircleNoPrimaryKey loadedFilledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(filledCircle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedFilledCircle, filledCircle);
                Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
                Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
                Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
                //---------------Tear Down -------------------------
            }
            
            [Test]
            public void TestLoad_ClassTableInheritance_Fresh()
            {
                //---------------Set up test pack-------------------
                Circle.GetClassDef();
                Circle circle = Circle.CreateSavedCircle();

                //---------------Execute Test ----------------------
                BusinessObject.AllLoadedBusinessObjects().Clear();
                Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCircle, circle);
                Assert.AreEqual(circle.Radius, loadedCircle.Radius);
                Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
            }


            [Test]
            public void TestLoad_ClassTableInheritance_Hierarchy_Fresh()
            {
                //---------------Set up test pack-------------------
                FilledCircle.GetClassDefWithClassInheritanceHierarchy();
                FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

                //---------------Execute Test ----------------------
                BusinessObject.AllLoadedBusinessObjects().Clear();
                FilledCircle loadedFilledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedFilledCircle, filledCircle);
                Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
                Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
                Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
            }

            [Test]
            public void TestLoad_ConcreteTableInheritance_Fresh()
            {
                //---------------Set up test pack-------------------
                Circle.GetClassDef();
                Circle circle = Circle.CreateSavedCircle();

                //---------------Execute Test ----------------------
                BusinessObject.AllLoadedBusinessObjects().Clear();
                Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedCircle, circle);
                Assert.AreEqual(circle.Radius, loadedCircle.Radius);
                Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
            }
            
            [Test]
            public void TestLoad_ConcreteTableInheritance_Hierarchy_Fresh()
            {
                //---------------Set up test pack-------------------
                FilledCircle.GetClassDefWithConcreteInheritanceHierarchy();
                FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

                //---------------Execute Test ----------------------
                BusinessObject.AllLoadedBusinessObjects().Clear();
                FilledCircle loadedFilledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

                //---------------Test Result -----------------------
                Assert.AreNotSame(loadedFilledCircle, filledCircle);
                Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
                Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
                Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
            }
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists_NotLoadedViaKey()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("ContactPersonID", Criteria.Op.Equals, Guid.NewGuid().ToString("N"));
               
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
        }

        [Test]
        public void TestGetBusinessObject_SelectQuery()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            SelectQuery query = new SelectQuery(new Criteria("Surname", Criteria.Op.Equals, cp.Surname));
            query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", ""));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
            query.Source = cp.ClassDef.TableName;

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);
            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, cp);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObject_SelectQuery_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            SelectQuery query = new SelectQuery(new Criteria("Surname", Criteria.Op.Equals, cp.Surname));
            query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", ""));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
            query.Source = cp.ClassDef.TableName;

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCp =
                (ContactPersonTestBO) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, query);
            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, cp);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObject_PrimaryKey()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_PrimaryKey_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)
                 BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObjectCollection_SelectQuery()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now.AddDays(1); 
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now.AddMinutes(1);
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();

            SelectQuery query = new SelectQuery(new Criteria("DateOfBirth", Criteria.Op.GreaterThan, now));
            query.Fields.Add("DateOfBirth", new QueryField("DateOfBirth", "DateOfBirth", ""));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
            query.Source = cp1.ClassDef.TableName;
            query.OrderCriteria = new OrderCriteria().Add("DateOfBirth");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(query);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObjectCollection_SelectQuery_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now.AddDays(1);
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now.AddMinutes(1);
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();

            SelectQuery query = new SelectQuery(new Criteria("DateOfBirth", Criteria.Op.GreaterThan, now));
            query.Fields.Add("DateOfBirth", new QueryField("DateOfBirth", "DateOfBirth", ""));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
            query.Source = cp1.ClassDef.TableName;
            query.OrderCriteria = new OrderCriteria().Add("DateOfBirth");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, query);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_WithOrder()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = "zzzz";
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = "aaaa";
            cp2.Save();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria, orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = "zzzz";
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = "aaaa";
            cp2.Save();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria, orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestRefreshLoadedCollection()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            ContactPersonTestBO cp3 = new ContactPersonTestBO();
            cp3.DateOfBirth = now;
            cp3.Surname = Guid.NewGuid().ToString("N");
            cp3.Save();
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            ContactPersonTestBO cp3 = new ContactPersonTestBO();
            cp3.DateOfBirth = now;
            cp3.Surname = Guid.NewGuid().ToString("N");
            cp3.Save();
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetRelatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            Car car = new Car();
            car.CarRegNo = "5";
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            engine.EngineNo = "20";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car);
            committer.AddBusinessObject(engine);
            committer.CommitTransaction();
            //---------------Execute Test ----------------------
            Car loadedCar = BORegistry.DataAccessor.BusinessObjectLoader.
                GetRelatedBusinessObject<Car>((SingleRelationship) engine.Relationships["Car"]);

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetRelatedBusinessObject_Untyped()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            Car car = new Car();
            car.CarRegNo = "5";
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            engine.EngineNo = "20";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car);
            committer.AddBusinessObject(engine);
            committer.CommitTransaction();
            //---------------Execute Test ----------------------
            Car loadedCar = (Car) BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject((SingleRelationship) engine.Relationships["Car"]);

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address = new Address();
            address.ContactPersonID = cp.ContactPersonID;
            address.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
           
            RelatedBusinessObjectCollection<Address> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<Address>(cp.Relationships["Addresses"]);
            //---------------Test Result -----------------------
            Criteria relationshipCriteria = Criteria.FromRelationship(cp.Relationships["Addresses"]);
            Assert.AreEqual(relationshipCriteria, addresses.SelectQuery.Criteria);
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address1 = new Address();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            Address address2 = new Address();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------

            RelatedBusinessObjectCollection<Address> addresses =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<Address>(cp.Relationships["Addresses"]);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            DeleteEnginesAndCars();
            Car car1 = new Car();
            car1.CarRegNo = "5";
            Car car2 = new Car();
            car2.CarRegNo = "2";

            Engine car1engine1 = new Engine();
            car1engine1.CarID = car1.CarID;
            car1engine1.EngineNo = "20";

            Engine car1engine2 = new Engine();
            car1engine2.CarID = car1.CarID;
            car1engine2.EngineNo = "10";

            Engine car2engine1 = new Engine();
            car2engine1.CarID = car2.CarID;
            car2engine1.EngineNo = "50";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car1);
            committer.AddBusinessObject(car2);
            committer.AddBusinessObject(car1engine1);
            committer.AddBusinessObject(car1engine2);
            committer.AddBusinessObject(car2engine1);
            committer.CommitTransaction();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString("Car.CarRegNo, EngineNo");
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(null, orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
            //---------------Tear Down -------------------------     
        }


        [Test]
        public void TestLoadThroughRelationship_Multiple()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address = new Address();
            address.ContactPersonID = cp.ContactPersonID;
            address.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<Address> addresses = cp.Addresses;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, addresses.Count);
            Assert.Contains(address, addresses);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLoadThroughRelationship_Single()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            Car car = new Car();
            car.CarRegNo = "5";
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            engine.EngineNo = "20";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car);
            committer.AddBusinessObject(engine);
            committer.CommitTransaction();
            //---------------Execute Test ----------------------
            Car loadedCar = engine.GetCar();

            //---------------Test Result -----------------------
            Assert.AreSame(car, loadedCar);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLoadAll_Loader()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            BOLoader.Instance.ClearLoadedBusinessObjects();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            col.LoadAll_Loader();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp, col);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestLoadWithOrderBy()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "eeeee";
            cp1.Save();

            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = "ggggg";
            cp2.Save();

            ContactPersonTestBO cp3 = new ContactPersonTestBO();
            cp3.Surname = "bbbbb";
            cp3.Save();
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(null, new OrderCriteria().Add("Surname"));
            //---------------Test Result -----------------------

            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestLoad_SingleTableInheritance()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();

            //---------------Execute Test ----------------------
            CircleNoPrimaryKey loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(circle.ID);
            //---------------Test Result -----------------------

            Assert.AreSame(loadedCircle, circle);
        }

        [Test]
        public void TestLoad_SingleTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
            
            //---------------Execute Test ----------------------
            FilledCircleNoPrimaryKey loadedFilledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedFilledCircle, filledCircle);
        }

        [Test]
        public void TestLoad_ClassTableInheritance()
        {
            //---------------Set up test pack-------------------
            Circle.GetClassDef();
            Circle circle = Circle.CreateSavedCircle();
            
            //---------------Execute Test ----------------------
            Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCircle, circle);
        }

        [Test]
        public void TestLoad_ClassTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithClassInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();
            
            //---------------Execute Test ----------------------
            FilledCircle loadedFilledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedFilledCircle, filledCircle);
        }
        
        [Test]
        public void TestLoad_ConcreteTableInheritance()
        {
            //---------------Set up test pack-------------------
            Circle.GetClassDefWithConcreteTableInheritance();
            Circle circle = Circle.CreateSavedCircle();

            //---------------Execute Test ----------------------
            Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCircle, circle);
        }

        [Test]
        public void TestLoad_ConcreteTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithConcreteInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();
            //---------------Execute Test ----------------------
            FilledCircle loadedFilledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedFilledCircle, filledCircle);
        }


        [Test]
        [ExpectedException(typeof(BusObjDeleteConcurrencyControlException))]
        public void TestTryLoadDeletedObject_RaiseError()
        {

            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO personToDelete = new ContactPersonTestBO();
            personToDelete.FirstName = Guid.NewGuid().ToString("B");
            personToDelete.Surname = Guid.NewGuid().ToString("B");
            personToDelete.Save();

            personToDelete.Delete();
            personToDelete.Save();

            //Ensure that a fresh object is loaded from DB
            ContactPerson.ClearContactPersonCol();

            //--------Execute------------------------------------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(personToDelete.ID);
        }
        [Test]
        [ExpectedException(typeof(BusObjDeleteConcurrencyControlException))]
        public void TestTryLoadDeletedObject_Untyped_RaiseError()
        {

            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO personToDelete = new ContactPersonTestBO();
            personToDelete.FirstName = Guid.NewGuid().ToString("B");
            personToDelete.Surname = Guid.NewGuid().ToString("B");
            personToDelete.Save();

            personToDelete.Delete();
            personToDelete.Save();

            //Ensure that a fresh object is loaded from DB
            ContactPerson.ClearContactPersonCol();

            //--------Execute------------------------------------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, personToDelete.ID);
        }

        /// <summary>
        /// Tests to ensure that if the object has been edited in the object manager by 
        /// another user the one we get back is always the latest.
        /// </summary>
        [Test, Ignore("To fix this")]
        public void TestAlwaysGetTheFreshestObject()
        {
            //------------------------------Setup Test
            ContactPerson originalContactPerson = new ContactPerson();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();

            ContactPerson.ClearContactPersonCol();

            //load second object from DB to ensure that it is now in the object manager
            ContactPerson myContact2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(originalContactPerson.ID);

            //-----------------------------Execute Test-------------------------
            //Edit first object and save
            originalContactPerson.Surname = "SecondSurname";
            originalContactPerson.Save(); //

            ContactPerson myContact3 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(originalContactPerson.ID);

            //-----------------------------Assert Result-----------------------
            Assert.AreSame(myContact3, myContact2);
            //The two surnames should be equal since the myContact3 was refreshed
            // when it was loaded.
            Assert.AreEqual(originalContactPerson.Surname, myContact3.Surname);
            //Just to check the myContact2 should also match since it is physically the 
            // same object as myContact3
            Assert.AreEqual(originalContactPerson.Surname, myContact2.Surname);
        }
    }


}
