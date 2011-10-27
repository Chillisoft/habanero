#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoader_GetBusinessObjectCollectionDB
        : TestBusinessObjectLoader_GetBusinessObjectCollection
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
        public void TestAfterLoadCalled_GetCollection_Generic_Reloaded()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, cp.ContactPersonID.ToString("B"));
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.AreNotSame(cp, col[0]);
            Assert.IsTrue(col[0].AfterLoadCalled);
        }


        [Test]
        public void TestAfterLoadCalled_GetCollection_NonGeneric_Reloaded()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, cp.ContactPersonID.ToString("B"));
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)col[0];
            Assert.AreNotSame(cp, loadedCP);
            Assert.IsTrue(loadedCP.AfterLoadCalled);
            Assert.IsInstanceOf(typeof(BusinessObjectCollection<ContactPersonTestBO>), col);
        }

        [Test]
        public void TestAfterLoadCalled_GetCollection_Generic_NotReloaded()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            ContactPersonTestBO.CreateSavedContactPerson();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            //---------------Assert Precondition----------------
            Assert.IsFalse(cp.AfterLoadCalled);

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, cp.ContactPersonID.ToString("B"));
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            ContactPersonTestBO loadedBO = col[0];
            Assert.IsTrue(loadedBO.AfterLoadCalled);
            //This works because if the object is not dirty then it is refreshed from the database
        }

        [Test]
        public void TestAfterLoadCalled_GetCollection_NonGeneric_NotReloaded()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            //---------------Assert Precondition----------------
            Assert.IsFalse(cp.AfterLoadCalled);

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria
                ("ContactPersonID", Criteria.ComparisonOp.Equals, cp.ContactPersonID.ToString("B"));
            IBusinessObjectCollection col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)col[0];
            Assert.IsTrue(loadedCP.AfterLoadCalled);
            Assert.IsInstanceOf(typeof(BusinessObjectCollection<ContactPersonTestBO>), col);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Generic_LoadOfSubTypeDoesntLoadSuperTypedObjects()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Shape shape = Shape.CreateSavedShape();
            Criteria criteria = Criteria.FromPrimaryKey(shape.ID);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<CircleNoPrimaryKey> loadedCircles =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<CircleNoPrimaryKey>
                    (criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, loadedCircles.Count);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_LoadOfSubTypeDoesntLoadSuperTypedObjects()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Shape shape = Shape.CreateSavedShape();
            Criteria criteria = Criteria.FromPrimaryKey(shape.ID);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedCircles =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, loadedCircles.Count);
            Assert.IsInstanceOf(typeof(BusinessObjectCollection<CircleNoPrimaryKey>), loadedCircles);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Generic_LoadOfSubTypeDoesntLoadSuperTypedObjects_Fresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Shape shape = Shape.CreateSavedShape();
            Criteria criteria = Criteria.FromPrimaryKey(shape.ID);
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<CircleNoPrimaryKey> loadedCircles =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<CircleNoPrimaryKey>
                    (criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, loadedCircles.Count);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_LoadOfSubTypeDoesntLoadSuperTypedObjects_Fresh()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Shape shape = Shape.CreateSavedShape();
            Criteria criteria = Criteria.FromPrimaryKey(shape.ID);
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedCircles =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, loadedCircles.Count);
            Assert.IsInstanceOf(typeof(BusinessObjectCollection<CircleNoPrimaryKey>), loadedCircles);
        }

        #region Test that the load returns the correct sub type

        [Test]
        public void Test_GetBusinessObjectCollection_Generic_ReturnsSubType_Fresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            Criteria criteria = Criteria.FromPrimaryKey(circle.ID);
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Shape> loadedShapes =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Shape>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedShapes.Count);
            Shape loadedShape = loadedShapes[0];
            Assert.IsInstanceOf(typeof(CircleNoPrimaryKey), loadedShape);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_ReturnsSubType_Fresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            Criteria criteria = Criteria.FromPrimaryKey(circle.ID);
            BusinessObjectManager.Instance.ClearLoadedObjects();

            IClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedShapes =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedShapes.Count);
            Assert.IsInstanceOf(typeof(Shape), loadedShapes[0]);
            Shape loadedShape = (Shape)loadedShapes[0];
            Assert.IsInstanceOf(typeof(CircleNoPrimaryKey), loadedShape);
            Assert.IsInstanceOf(typeof(BusinessObjectCollection<Shape>), loadedShapes);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Generic_ReturnsSubType_NonFresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            Criteria criteria = Criteria.FromPrimaryKey(circle.ID);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<Shape> loadedShapes =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<Shape>(criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedShapes.Count);
            Shape loadedShape = loadedShapes[0];
            Assert.IsInstanceOf(typeof(CircleNoPrimaryKey), loadedShape);
            Assert.AreSame(circle, loadedShape);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_ReturnsSubType_NonFresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            Criteria criteria = Criteria.FromPrimaryKey(circle.ID);

            IClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedShapes =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedShapes.Count);
            Assert.IsInstanceOf(typeof(Shape), loadedShapes[0]);
            Shape loadedShape = (Shape)loadedShapes[0];
            Assert.IsInstanceOf(typeof(CircleNoPrimaryKey), loadedShape);
            Assert.IsInstanceOf(typeof(BusinessObjectCollection<Shape>), loadedShapes);
            Assert.AreSame(circle, loadedShape);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Generic_ReturnsSubType_TwoLevelsDeep_DiscriminatorShared_Fresh()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>
                (filledCircle.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_ReturnsSubType_TwoLevelsDeep_DiscriminatorShared_Fresh()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            IClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObject loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, filledCircle.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(Shape), loadedShape);
            Assert.IsInstanceOf(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_GetBusinessObjectCollection_Generic_ReturnsSubType_TwoLevelsDeep_Fresh()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>
                (filledCircle.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_ReturnsSubType_TwoLevelsDeep_Fresh()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            IClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObject loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                (classDef, filledCircle.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(Shape), loadedShape);
            Assert.IsInstanceOf(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }
/*        [Test]
        public void Test_LoadWithLimit_SearchCriteriaNullString_ShouldNotRaiseError_FixBug565()
        {
            TestUsingDatabase.SetupDBDataAccessor();
            ContactPersonTestBO.DeleteAllContactPeople();
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = 2;
            const int expectedCount = 2;

            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------

            int totalNoOfRecords;
            col.LoadWithLimit((string)null, "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, expectedCount, contactPersonTestBOs, col, totalNoOfRecords);
        }
        [Test]
        public void Test_LoadWithLimit_SortCriteriaEmptyString_ShouldNotRaiseError_FixBug566()
        {

            TestUsingDatabase.SetupDBDataAccessor();
            ContactPersonTestBO.DeleteAllContactPeople();
            const int totalRecords = 3;
            const int firstRecord = 0;
            const int limit = 2;
            const int expectedCount = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            Assert.AreEqual
                (totalRecords, totalNoOfRecords, "The returned total number of availabe records is incorrect");
            Assert.AreEqual
                (firstRecord, col.SelectQuery.FirstRecordToLoad,
                 "Collection query FirstRecordToLoad does not match expectation.");
            Assert.AreEqual
                (limit, col.SelectQuery.Limit, "Collection query limit does not match expectation.");
            Assert.AreEqual(expectedCount, col.Count, "Collection size does not match expectation.");
        }
        [Test]
        public void Test_LoadWithLimit_SortCriteriaNullString_ShouldNotRaiseError_FixBug566()
        {

            TestUsingDatabase.SetupDBDataAccessor();
            ContactPersonTestBO.DeleteAllContactPeople();
            Assert.IsInstanceOf<DataAccessorDB>(BORegistry.DataAccessor, "Should b using Database");
            const int totalRecords = 6;
            const int firstRecord = 2;
            const int limit = 2;
            const int expectedCount = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.IsInstanceOf<DataAccessorDB>(BORegistry.DataAccessor, "Should b using Database");
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit((string)null, (string)null, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            Assert.AreEqual
                (totalRecords, totalNoOfRecords, "The returned total number of availabe records is incorrect");
            Assert.AreEqual
                (firstRecord, col.SelectQuery.FirstRecordToLoad,
                 "Collection query FirstRecordToLoad does not match expectation.");
            Assert.AreEqual
                (limit, col.SelectQuery.Limit, "Collection query limit does not match expectation.");
            Assert.AreEqual(expectedCount, col.Count, "Collection size does not match expectation.");
        }

        [Test]
        public void Test_LoadWithLimit_WithSortCriteriaNullString_WhenCompositePK_ShouldNotRaiseError_FixBug567()
        {
            TestUsingDatabase.SetupDBDataAccessor();
            ContactPersonCompositeKey.DeleteAllContactPeople();
            Assert.IsInstanceOf<DataAccessorDB>(BORegistry.DataAccessor, "Should b using Database");
            const int totalRecords = 6;
            const int firstRecord = 2;
            const int limit = 2;
            const int expectedCount = 2;
            ContactPersonCompositeKey.LoadClassDefs();
            ContactPersonCompositeKey[] contactPersonTestBOs = CreateCompositeContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonCompositeKey>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.IsInstanceOf<DataAccessorDB>(BORegistry.DataAccessor, "Should b using Database");
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit((string)null, (string)null, firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            Assert.AreEqual
                (totalRecords, totalNoOfRecords, "The returned total number of availabe records is incorrect");
            Assert.AreEqual
                (firstRecord, col.SelectQuery.FirstRecordToLoad,
                 "Collection query FirstRecordToLoad does not match expectation.");
            Assert.AreEqual
                (limit, col.SelectQuery.Limit, "Collection query limit does not match expectation.");
            Assert.AreEqual(expectedCount, col.Count, "Collection size does not match expectation.");
        }
        /// <summary>
        /// Creates the specified number of saved Contact People with random Surnames and returns an array of the 
        /// created items sorted by their surname.
        /// </summary>
        /// <param name="noOfPeople">The number of saved contact people to create</param>
        /// <returns>Returns an array of the created items sorted by their surname.</returns>
        private static ContactPersonTestBO[] CreateSavedSortedContactPeople(int noOfPeople)
        {
            System.Collections.Generic.List<ContactPersonTestBO> createdBos = new List<ContactPersonTestBO>(noOfPeople);
            while (createdBos.Count < noOfPeople)
            {
                createdBos.Add(ContactPersonTestBO.CreateSavedContactPerson(TestUtil.GetRandomString()));
            }
            createdBos.Sort((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(x.Surname, y.Surname));
            return createdBos.ToArray();
        }
        private static ContactPersonCompositeKey[] CreateCompositeContactPeople(int totalRecords)
        {
            List<ContactPersonCompositeKey> createdBos = new List<ContactPersonCompositeKey>(totalRecords);
            while (createdBos.Count < totalRecords)
            {
                createdBos.Add(ContactPersonCompositeKey.CreateSavedContactPerson());
            }
            return createdBos.ToArray();
        }*/
        #endregion //Test that the load returns the correct sub type
    }
}