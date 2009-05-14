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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
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
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
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
            Assert.IsInstanceOfType(typeof(BusinessObjectCollection<ContactPersonTestBO>), col);
        }

        [Test]
        public void TestAfterLoadCalled_GetCollection_Generic_NotReloaded()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            ContactPersonTestBO.CreateSavedContactPerson();
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
            Assert.AreSame(cp, loadedBO);
            Assert.IsTrue(loadedBO.AfterLoadCalled);
            //This works because if the object is not dirty then it is refreshed from the database
        }

        [Test]
        public void TestAfterLoadCalled_GetCollection_NonGeneric_NotReloaded()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

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
            Assert.AreSame(cp, loadedCP);
            Assert.IsTrue(loadedCP.AfterLoadCalled);
            Assert.IsInstanceOfType(typeof(BusinessObjectCollection<ContactPersonTestBO>), col);
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
            ClassDef classDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Shape shape = Shape.CreateSavedShape();
            Criteria criteria = Criteria.FromPrimaryKey(shape.ID);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedCircles =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, loadedCircles.Count);
            Assert.IsInstanceOfType(typeof(BusinessObjectCollection<CircleNoPrimaryKey>), loadedCircles);
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
            ClassDef classDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Shape shape = Shape.CreateSavedShape();
            Criteria criteria = Criteria.FromPrimaryKey(shape.ID);
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedCircles =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, loadedCircles.Count);
            Assert.IsInstanceOfType(typeof(BusinessObjectCollection<CircleNoPrimaryKey>), loadedCircles);
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
            Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_ReturnsSubType_Fresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            Criteria criteria = Criteria.FromPrimaryKey(circle.ID);
            BusinessObjectManager.Instance.ClearLoadedObjects();

            ClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedShapes =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedShapes.Count);
            Assert.IsInstanceOfType(typeof(Shape), loadedShapes[0]);
            Shape loadedShape = (Shape)loadedShapes[0];
            Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
            Assert.IsInstanceOfType(typeof(BusinessObjectCollection<Shape>), loadedShapes);
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
            Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
            Assert.AreSame(circle, loadedShape);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_NonGeneric_ReturnsSubType_NonFresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            Criteria criteria = Criteria.FromPrimaryKey(circle.ID);

            ClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObjectCollection loadedShapes =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, loadedShapes.Count);
            Assert.IsInstanceOfType(typeof(Shape), loadedShapes[0]);
            Shape loadedShape = (Shape)loadedShapes[0];
            Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
            Assert.IsInstanceOfType(typeof(BusinessObjectCollection<Shape>), loadedShapes);
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
            Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
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

            ClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObject loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, filledCircle.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(Shape), loadedShape);
            Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
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
            Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
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

            ClassDef classDef = ClassDef.Get<Shape>();
            //---------------Execute Test ----------------------
            IBusinessObject loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                (classDef, filledCircle.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(Shape), loadedShape);
            Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }

        #endregion //Test that the load returns the correct sub type
    }
}