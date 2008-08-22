using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public abstract class TestBusinessObjectLoader_GetBusinessObjectCollection //:TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDataAccessor();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
        }

        [TearDown]
        public virtual void TearDownTest()
        {
        }

        #endregion

        protected abstract void SetupDataAccessor();

        protected abstract void DeleteEnginesAndCars();

        [TestFixture]
        public class TestBusinessObjectLoader_GetBusinessObjectCollectionInMemory :
            TestBusinessObjectLoader_GetBusinessObjectCollection
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
        }

        [TestFixture]
        public class TestBusinessObjectLoader_GetBusinessObjectCollectionDB :
            TestBusinessObjectLoader_GetBusinessObjectCollection
        {
            #region Setup/Teardown

            [SetUp]
            public override void SetupTest()
            {
                base.SetupTest();
                ContactPersonTestBO.DeleteAllContactPeople();
            }

            #endregion

            protected override void DeleteEnginesAndCars()
            {
                Engine.DeleteAllEngines();
                Car.DeleteAllCars();
            }

            public TestBusinessObjectLoader_GetBusinessObjectCollectionDB()
            {
                new TestUsingDatabase().SetupDBConnection();
            }

            protected override void SetupDataAccessor()
            {
                BORegistry.DataAccessor = new DataAccessorDB();
            }

            [Test]
            public void TestAfterLoadCalled_GetCollection()
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                BusinessObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals,
                                                 cp.ContactPersonID.ToString("B"));
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(
                        criteria);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                Assert.AreNotSame(cp, col[0]);
                Assert.IsTrue(col[0].AfterLoadCalled);
            }

            [Test]
            public void TestAfterLoadCalled_GetCollection_NotReloaded()
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                ContactPersonTestBO.CreateSavedContactPerson();
                //---------------Assert Precondition----------------
                Assert.IsFalse(cp.AfterLoadCalled);

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals,
                                                 cp.ContactPersonID.ToString("B"));
                BusinessObjectCollection<ContactPersonTestBO> col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(
                        criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                ContactPersonTestBO loadedBO = col[0];
                Assert.AreSame(cp, loadedBO);
                Assert.IsTrue(loadedBO.AfterLoadCalled);
                    //This works because if the object is not dirty then it is refreshed from the database
            }


            [Test]
            public void TestAfterLoadCalled_GetCollection_Untyped()
            {
                //---------------Set up test pack-------------------
                ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
                BusinessObjectManager.Instance.ClearLoadedObjects();
                TestUtil.WaitForGC();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals,
                                                 cp.ContactPersonID.ToString("B"));
                IBusinessObjectCollection col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                ContactPersonTestBO loadedCP = (ContactPersonTestBO) col[0];
                Assert.AreNotSame(cp, loadedCP);
                Assert.IsTrue(loadedCP.AfterLoadCalled);
            }

            [Test]
            public void TestAfterLoadCalled_GetCollection_Untyped_NotReloaded()
            {
                //---------------Set up test pack-------------------
                ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
                ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

                //---------------Assert Precondition----------------
                Assert.IsFalse(cp.AfterLoadCalled);

                //---------------Execute Test ----------------------
                Criteria criteria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals,
                                                 cp.ContactPersonID.ToString("B"));
                IBusinessObjectCollection col =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, col.Count);
                ContactPersonTestBO loadedCP = (ContactPersonTestBO) col[0];
                Assert.AreSame(cp, loadedCP);
                Assert.IsTrue(loadedCP.AfterLoadCalled);
            }

            [Test]
            public void TestGetBusinessObjectCollection_Typed_LoadOfSubTypeDoesntLoadSuperTypedObjects()
            {
                //---------------Set up test pack-------------------
                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                Shape shape = Shape.CreateSavedShape();
                Criteria criteria = Criteria.FromPrimaryKey(shape.ID);
                
                //---------------Execute Test ----------------------
                BusinessObjectCollection<CircleNoPrimaryKey> loadedCircles = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<CircleNoPrimaryKey>(criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(0, loadedCircles.Count);
            }

            [Test]
            public void TestGetBusinessObjectCollection_Typed_LoadOfSubTypeDoesntLoadSuperTypedObjects_Fresh()
            {
                //---------------Set up test pack-------------------
                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                Shape shape = Shape.CreateSavedShape();
                Criteria criteria = Criteria.FromPrimaryKey(shape.ID);
                BusinessObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                BusinessObjectCollection<CircleNoPrimaryKey> loadedCircles = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<CircleNoPrimaryKey>(criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(0, loadedCircles.Count);
            }


            #region Test that the load returns the correct sub type

            [Test]
            public void TestGetBusinessObjectCollection_Typed_ReturnsSubType_Fresh()
            {
                //---------------Set up test pack-------------------
                CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
                CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
                Criteria criteria = Criteria.FromPrimaryKey(circle.ID);
                BusinessObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                BusinessObjectCollection<Shape> loadedShapes = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Shape>(criteria);

                //---------------Test Result -----------------------
                Assert.AreEqual(1, loadedShapes.Count);
                Shape loadedShape = loadedShapes[0];
                Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
            }

            [Test, Ignore("TBI")]
            public void TestGetBusinessObjectCollection_ReturnsSubType_TwoLevelsDeep_DiscriminatorShared_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                BusinessObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                Shape loadedShape =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(filledCircle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            [Test, Ignore("TBI")]
            public void TestGetBusinessObjectCollection_ReturnsSubType_TwoLevelsDeep_Fresh()
            {
                //---------------Set up test pack-------------------
                SetupDataAccessor();

                FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators();
                FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
                BusinessObjectManager.Instance.ClearLoadedObjects();

                //---------------Execute Test ----------------------
                Shape loadedShape =
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(filledCircle.ID);
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
                //---------------Tear Down -------------------------          
            }

            #endregion //Test that the load returns the correct sub type

        }

        private static SelectQuery CreateManualSelectQueryOrderedByDateOfBirth(DateTime now, BusinessObject cp1)
        {
            SelectQuery query = new SelectQuery(new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now));
            query.Fields.Add("DateOfBirth", new QueryField("DateOfBirth", "DateOfBirth", null));
            query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            query.Source = new Source(cp1.ClassDef.TableName);
            query.OrderCriteria = new OrderCriteria().Add("DateOfBirth");
            return query;
        }

        private static ContactPersonTestBO CreateContactPersonInDB_With_SSSSS_InSurname()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N") + "SSSSS";
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }

        private static void CreateContactPersonInDB()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N");
            contactPersonTestBO.Save();
            return;
        }

        [Test]
        public void Test_CollectionLoad_CriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            Assert.AreEqual("ContactPersonTestBO.Surname ASC", col.SelectQuery.OrderCriteria.ToString());
            Assert.AreEqual(-1, col.SelectQuery.Limit);
        }


        [Test]
        public void Test_CollectionLoad_CriteriaStringSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "searchSurname");
            const string stringCriteria = "Surname = searchSurname";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(stringCriteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            Assert.AreEqual("ContactPersonTestBO.Surname ASC", col.SelectQuery.OrderCriteria.ToString());
            Assert.AreEqual(-1, col.SelectQuery.Limit);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string surname = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO.CreateSavedContactPerson();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "Surname = " + surname;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_CriteriaString_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            //            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_NullCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();
            const Criteria criteria = null;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_NullCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("", "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_StringCriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            col.Sort("Surname", true, true);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }


        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_StringCriteriaObject_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_CollectionLoad_GetBusinessObjectCollection_StringCriteriaObject_WithOrder_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void
            Test_CollectionLoad_LoadWithLimit_RefreshLoadedCollection_Untyped_GTCriteriaObject_OrderClause_DoesNotLoadNewObject
            ()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadWithLimit(criteria, "Surname", 2);
            ContactPersonTestBO cpnew = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cpnew, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
            Assert.IsFalse(col.Contains(cp2));
        }

        [Test]
        public void Test_CollectionLoad_LoadWithLimit_Untyped_GTCriteriaObject_OrderClause_DoesNotLoadNewObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));

            //---------------Assert Precondition ---------------


            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadWithLimit(criteria, "Surname", 2);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
        }

        [Test]
        public void Test_CollectionLoad_LoadWithOrderBy_ManualOrderbyFieldName()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("", "Surname");

            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            col.Refresh();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Typed_LTEQCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);

            string criteria = "IntegerProperty <= " + 3;
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 1);
            ContactPersonTestBO cpExclude = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.Contains(cpEqualNew, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }

        private static ContactPersonTestBO CreateSavedContactPerson(string surnameValue, int integerPropertyValue)
        {
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = surnameValue;
            cp.SetPropertyValue("IntegerProperty", integerPropertyValue);
            cp.Save();
            return cp;
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Typed_LTEQCriteriaString_IntegerProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            cp1.SetPropertyValue("IntegerProperty", 0);
            cp1.Save();
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            cp2.SetPropertyValue("IntegerProperty", 1);
            cp2.Save();
            ContactPersonTestBO cpExcluded = ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
            cpExcluded.SetPropertyValue("IntegerProperty", 3);
            cpExcluded.Save();
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(+1));
            cpEqual.SetPropertyValue("IntegerProperty", 2);
            cpEqual.Save();

            string criteria = "IntegerProperty <= " + cpEqual.GetPropertyValue("IntegerProperty");
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
            cp3.SetPropertyValue("IntegerProperty", 1);
            cp3.Save();
            ContactPersonTestBO cpExcludeNew = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
            cpExcludeNew.SetPropertyValue("IntegerProperty", 5);
            cpExcludeNew.Save();
            ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(cpEqual.DateOfBirth);
            cpEqualNew.SetPropertyValue("IntegerProperty", 2);
            cpEqualNew.Save();

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cpEqual, col);
            Assert.IsFalse(col.Contains(cpExcluded));

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.Contains(cpEqualNew, col);
            Assert.IsFalse(col.Contains(cpExcludeNew));
            Assert.IsFalse(col.Contains(cpExcluded));
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Typed_NotEQ_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);

            string criteria = "IntegerProperty <> " + 3;
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsFalse(col.Contains(cpEqual));

            //---------------Execute Test ----------------------
            ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cpNotEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpNotEqual, col);
            Assert.IsFalse(col.Contains(cpEqualNew));
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Typed_NotEQ_CriteriaString_Null()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(+1));

            const string criteria = "DateOfBirth <> " + null;
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
            ContactPersonTestBO cpNotEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
            ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(cpEqual.DateOfBirth);

            //---------------Assert Precondition ---------------
            Assert.AreEqual(4, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsTrue(col.Contains(cpEqual));

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(7, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpNotEqual, col);
            Assert.IsTrue(col.Contains(cpEqualNew));
            Assert.IsTrue(col.Contains(cpEqual));
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Typed_NotEQ_CriteriaString_Null_2()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(null,"sn", "fn");

            const string criteria = "DateOfBirth <> " + null;
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
            ContactPersonTestBO cpNotEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
            ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(null, "sn2", "fn2");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsFalse(col.Contains(cpEqual));

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpNotEqual, col);
            Assert.IsFalse(col.Contains(cpEqualNew));
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_ISNull_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(null, "sn", "fn");

            const string criteria = "DateOfBirth IS NULL";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
            ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
            ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(null, "sn2", "fn2");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.Contains(cpEqual));

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.IsTrue(col.Contains(cpEqualNew));
            Assert.IsTrue(col.Contains(cpEqual));
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_IsNotNull_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(null, "sn", "fn");

            const string criteria = "DateOfBirth IS NOT NULL";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
            ContactPersonTestBO cpNotEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
            ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(null, "sn2", "fn2");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsFalse(col.Contains(cpEqual));

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpNotEqual, col);
            Assert.IsFalse(col.Contains(cpEqualNew));
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);
            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Untyped_GTCriteriaObject_DoesNotLoadNewObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_Untyped_GTCriteriaObject_LoadsNewObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-2));
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void
            Test_CollectionLoad_RefreshLoadedCollection_Untyped_GTCriteriaObject_OrderClause_DoesNotLoadNewObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            ContactPersonTestBO cpnew = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);

            //---------------Execute Test ----------------------
            //            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(4, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cpnew, col[1]);
            Assert.AreSame(cp2, col[2]);
            Assert.AreSame(cpLast, col[3]);
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_UsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("Surname like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpNewLikeMatch, col);
        }

        [Test]
        public void Test_CollectionLoad_RefreshLoadedCollection_UsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("Surname not like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.IsFalse(col.Contains(cpNewLikeMatch));
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_WithLimit()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 2;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>(selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.IsFalse(col.Contains(cpLast));
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_WithLimit_Negative()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = -1;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>(selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.AreSame(cpLast, col[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_SelectQuery_WithLimit_Zero()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 0;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>(selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Typed_RefreshLoadedCollection_UsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>("Surname like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpNewLikeMatch, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Typed_RefreshLoadedCollection_UsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>("Surname not like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.IsFalse(col.Contains(cpNewLikeMatch));
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Untyped_RefreshLoadedCollection_UsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection(classDef, "Surname like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpNewLikeMatch, col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_UnTyped_RefreshLoadedCollection_UsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection(classDef, "Surname not like %SSS%", "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.IsFalse(col.Contains(cpNewLikeMatch));
        }

        [Test]
        public void Test_GetBusinessObjectCollection_WithLimit_EqualNumberOfObjects()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 3;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>(selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.AreSame(cpLast, col[2]);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_WithLimit_LessObjects()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");
            ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = OrderCriteria.FromString("Surname");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            selectQuery.OrderCriteria = orderCriteria;
            selectQuery.Limit = 10;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection<ContactPersonTestBO>(selectQuery);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreSame(cp1, col[0]);
            Assert.AreSame(cp2, col[1]);
            Assert.AreSame(cpLast, col[2]);
        }

        [Test]
        public void Test_GetBusinesssObjectCollection_Untyped_GtCriteriaString()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);

            string criteria = "IntegerProperty > " + 3;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void Test_LoadUsingLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            col.Load("Surname like %SSS%", "Surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
        }

        [Test]
        public void Test_LoadUsingNotLike()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            ContactPersonTestBO.LoadDefaultClassDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            col.Load("Surname Not like %SSS%", "Surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void TestCriteriaSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestCriteriaSetUponLoadingCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, DateTime.Now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestCriteriaSetUponLoadingCollection_Untyped_Date()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string stringCriteria = "DateOfBirth = " + now;

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, stringCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria.FieldValue, col.SelectQuery.Criteria.FieldValue);
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestCriteriaStringSetUponLoadingCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "searchSurname");
            const string stringCriteria = "Surname = searchSurname";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, stringCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestCriteriaStringSetUponLoadingCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "searchSurname");
            const string stringCriteria = "Surname = searchSurname";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, stringCriteria);

            //---------------Test Result -----------------------
            Assert.IsNotNull(col.SelectQuery.Criteria);
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_DateTimeToday()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1));
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(today, "aaa");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1));
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, new DateTimeToday());

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_WithOrder()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");


            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria,
                                                                                                              orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaObject_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria,
                                                                                         orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string surname = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO.CreateSavedContactPerson();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "Surname = " + surname;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            //            DateTime now = DateTime.Now;
            const string surname = "TestSurname";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            cp1.OrganisationID = organisation.OrganisationID;
            cp1.Save();
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(surname);
            ContactPersonTestBO.CreateSavedContactPerson();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string criteria = string.Format("Organisation.OrganisationID = '{0}'", organisation.OrganisationID.ToString("B"));

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            Engine.LoadClassDef_IncludingCarAndOwner();
            //            DateTime now = DateTime.Now;
            string surname;
            string regno;
            string engineNo;
            Engine engine = CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
            CreateEngineWithCarWithContact();
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            string criteria = string.Format("Car.Owner.Surname = '{0}'", surname);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(engine, col);
        }

        private Engine CreateEngineWithCarWithContact()
        {
            string surname;
            string regno;
            string engineNo;
            return CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
        }
        private Engine CreateEngineWithCarWithContact(out string surname, out string regno, out string engineNo)
        {
            regno = TestUtil.CreateRandomString();
            engineNo = TestUtil.CreateRandomString();
            surname = TestUtil.CreateRandomString();
            ContactPerson owner = ContactPerson.CreateSavedContactPerson(surname);
            Car car = Car.CreateSavedCar(regno, owner);
            return Engine.CreateSavedEngine(car, engineNo);
        }


        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_Date_Today()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(-1));
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(today, "aaa");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(today, "bbb");
            ContactPersonTestBO.CreateSavedContactPerson(today.AddDays(1));
            const string criteria = "DateOfBirth = 'Today'";

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
//            DateTime now = DateTime.Now;
            //ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            //ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            //Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string firstName = "fName";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("aaa", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("zzz", firstName);
            const string criteria = "FirstName = " + firstName;

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            col.Sort("Surname", true, true);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_WithOrder_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
//            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            //            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria,
                                                                                         "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_WithOrderString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
//            DateTime now = DateTime.Now;
//            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
//            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
//            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
//            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");
            const string firstName = "fName";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaa", firstName);
            const string criteria = "FirstName = " + firstName;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria,
                                                                                                              "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_CriteriaString_WithOrderString_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            const string firstName = "fName";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaa", firstName);
            const string criteria = "FirstName = " + firstName;

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria,
                                                                                         "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_GetsSameObjectAsGetBusinessObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria collectionCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            Criteria singleCritieria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals, cp1.ContactPersonID);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO loadedCp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(singleCritieria);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(
                    collectionCriteria, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_GetsSameObjectAsGetBusinessObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            Criteria collectionCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            Criteria singleCritieria = new Criteria("ContactPersonID", Criteria.ComparisonOp.Equals, cp1.ContactPersonID);
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(cp1.ClassDef, "Surname");

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO loadedCp =
                (ContactPersonTestBO)
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(cp1.ClassDef, singleCritieria);
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef,
                                                                                         collectionCriteria,
                                                                                         orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreSame(loadedCp, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_NullCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void TestGetBusinessObjectCollection_NullCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson();
            const Criteria criteria = null;

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void TestGetBusinessObjectCollection_SelectQuery()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now.AddMinutes(1));

            SelectQuery query = CreateManualSelectQueryOrderedByDateOfBirth(now, cp1);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(query);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_SelectQuery_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now.AddMinutes(1));
            SelectQuery query = CreateManualSelectQueryOrderedByDateOfBirth(now, cp1);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, query);


            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
                                                                           "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(null, orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            ContactPerson contactPerson1 = ContactPerson.CreateSavedContactPerson("zzzz");
            ContactPerson contactPerson2 = ContactPerson.CreateSavedContactPerson("aaaa");
            Car car1 = Car.CreateSavedCar("2", contactPerson1);
            Car car2 = Car.CreateSavedCar("5", contactPerson2);
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
                                                                           "Car.Owner.Surname, EngineNo");
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
        public void TestGetBusinessObjectCollection_SortOrder_ThroughRelationship_Untyped()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
                                                                           "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(car1engine1.ClassDef, null,
                                                                                         orderCriteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_SortOrderString_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
            //OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
            //                                                               "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(null,
                                                                                                 "Car.CarRegNo, EngineNo");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_SortOrderString_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            ContactPerson contactPerson1 = ContactPerson.CreateSavedContactPerson("zzzz");
            ContactPerson contactPerson2 = ContactPerson.CreateSavedContactPerson("aaaa");
            Car car1 = Car.CreateSavedCar("2", contactPerson1);
            Car car2 = Car.CreateSavedCar("5", contactPerson2);
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");

            //---------------Execute Test ----------------------
//            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
//                                                                           "Car.Owner.Surname, EngineNo");
            BusinessObjectCollection<Engine> engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Engine>(null,
                                                                                                 "Car.Owner.Surname, EngineNo");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestGetBusinessObjectCollection_SortOrderString_ThroughRelationship_Untyped()
        {
            //---------------Set up test pack-------------------
            DeleteEnginesAndCars();
            Car car1 = Car.CreateSavedCar("5");
            Car car2 = Car.CreateSavedCar("2");
            Engine car1engine1 = Engine.CreateSavedEngine(car1, "20");
            Engine car1engine2 = Engine.CreateSavedEngine(car1, "10");
            Engine car2engine1 = Engine.CreateSavedEngine(car2, "50");
//            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(car1engine1.ClassDef,
//                                                                           "Car.CarRegNo, EngineNo");

            //---------------Execute Test ----------------------
            IBusinessObjectCollection engines =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(car1engine1.ClassDef, null,
                                                                                         "Car.CarRegNo, EngineNo");

            //---------------Test Result -----------------------
            Assert.AreEqual(3, engines.Count);
            Assert.AreSame(car2engine1, engines[0]);
            Assert.AreSame(car1engine2, engines[1]);
            Assert.AreSame(car1engine1, engines[2]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_StringCriteriaObject_Untyped()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            const string criteria = "FirstName = '" + firstName + "'";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria);
            col.Sort("Surname", true, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);

            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }


        [Test]
        public void TestGetBusinessObjectCollection_StringCriteriaObject_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(cp1.ClassDef, criteria);
            col.Sort("Surname", true, true);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestGetBusinessObjectCollection_StringCriteriaObject_WithOrder_Untyped_DateOfBirth()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "zzzz");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaaa");
            string criteria = "DateOfBirth = '" + now + "'";

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection(cp1.ClassDef, criteria, "Surname");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0]);
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void TestLoadAll()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            cpCol.LoadAll();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
        }


        //        [Test]
//        public void Test_LoadUsingLike()
//        {
//            //---------------Set up test pack-------------------
//            ContactPerson.DeleteAllContactPeople();
//            ContactPersonTestBO.LoadDefaultClassDef();
//            //Create data in the database with the 5 contact people two with Search in surname
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
//
//            //---------------Assert Precondition----------------
//
//            //---------------Execute Test ----------------------
//            col.Load("Surname like %SSS%", "Surname");
//            //---------------Test Result -----------------------
//            Assert.AreEqual(2, col.Count);
//        }
//        [Test]
//        public void Test_LoadUsingNotLike()
//        {
//            //---------------Set up test pack-------------------
//            ContactPerson.DeleteAllContactPeople();
//            ContactPersonTestBO.LoadDefaultClassDef();
//            //Create data in the database with the 5 contact people two with Search in surname
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            CreateContactPersonInDB_With_SSSSS_InSurname();
//            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
//
//            //---------------Assert Precondition----------------
//
//            //---------------Execute Test ----------------------
//            col.Load("Surname Not like %SSS%", "Surname");
//            //---------------Test Result -----------------------
//            Assert.AreEqual(3, col.Count);
//        }


        [Test]
        public void TestLoadAll_Loader()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp, col);
        }

        [Test]
        public void TestLoadWithOrderBy()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname");
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(null,
                                                                                                              orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }

        [Test]
        public void TestLoadWithOrderBy_ManualOrderbyFieldName()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("eeeee");
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("ggggg");
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson("bbbbb");
            OrderCriteria orderCriteria = new OrderCriteria();
            //---------------Execute Test ----------------------
            orderCriteria.Add(new OrderCriteria.Field("Surname", "Surname_field", null,
                                                      OrderCriteria.SortDirection.Ascending));
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(null,
                                                                                                              orderCriteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp3, col[0]);
            Assert.AreSame(cp1, col[1]);
            Assert.AreSame(cp2, col[2]);
        }

        [Test]
        public void TestRefreshLoadedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void TestRefreshLoadedCollection_Typed_GTEQCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

            string criteria = "DateOfBirth >= " + cpEqual.DateOfBirth;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(4, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }

        [Test]
        public void TestRefreshLoadedCollection_Typed_LTEQCriteriaString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 1);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 1);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);

            string criteria = "IntegerProperty <= " + 2;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
            ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 1);
            ContactPersonTestBO cpExclude = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.Contains(cpEqualNew, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);
            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped_GTCriteriaObject_DoesNotLoadNewObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, cpEqual.DateOfBirth);
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped_GTCriteriaObject_LoadsNewObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-2));
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped_GTCriteriaString()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 1);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);

            string criteria = "IntegerProperty > " + 2;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            //---------------Assert Precondition ---------------
            Assert.AreEqual(2, col.Count);
            Assert.IsFalse(col.Contains(cpEqual));

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.IsFalse(col.Contains(cpEqual));
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped_GTEQCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

            Criteria criteria = new Criteria("DateofBirth", Criteria.ComparisonOp.GreaterThanEqual, cpEqual.DateOfBirth);
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(4, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped_GTEQCriteriaString()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

            string criteria = "DateOfBirth >= " + cpEqual.DateOfBirth;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
            ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(4, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }


        [Test]
        public void TestRefreshLoadedCollection_Untyped_LTEQCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
            ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
            ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(+1));

            Criteria criteria = new Criteria("DateofBirth", Criteria.ComparisonOp.LessThanEqual, cpEqual.DateOfBirth);
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
            ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(4, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }

        [Test]
        public void TestRefreshLoadedCollection_Untyped_LTEQCriteriaString()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
            ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);
            ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);

            string criteria = "IntegerProperty <= " + 3;
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.CreateRandomString(), 2);
            ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.CreateRandomString(), 3);
            ContactPersonTestBO cpExclude = CreateSavedContactPerson(TestUtil.CreateRandomString(), 4);

            //---------------Assert Precondition ---------------
            Assert.AreEqual(3, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cpEqual, col);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.Contains(cp3, col);
            Assert.Contains(cpEqual, col);
            Assert.Contains(cpEqualNew, col);
            Assert.IsFalse(col.Contains(cpExclude));
        }

        [Test]
        public void TestSetColSelectQuery_null()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                col.SelectQuery = null;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("A collections select query cannot be set to null", ex.Message);
                StringAssert.Contains("A collections select query cannot be set to null", ex.DeveloperMessage);
            }
        }
    }
}