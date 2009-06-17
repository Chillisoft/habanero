using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoaderDBWithInheritance : TestBusinessObjectLoaderWithInheritance
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
        }

        #endregion

        public TestBusinessObjectLoaderDBWithInheritance()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [Test]
        public void TestGetBusinessObject_ReturnsSubType_Fresh()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();

            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Execute Test ----------------------
            Shape loadedShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(circle.ID);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(CircleNoPrimaryKey), loadedShape);
        }

        [Test]
        public void TestGetBusinessObject_ReturnsSubType_TwoLevelsDeep_DiscriminatorShared_Fresh()
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
            Assert.IsInstanceOf(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObject_ReturnsSubType_TwoLevelsDeep_Fresh()
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
            Assert.IsInstanceOf(typeof(FilledCircleNoPrimaryKey), loadedShape);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLoad_ClassTableInheritance_Fresh()
        {
            //---------------Set up test pack-------------------
            Circle.GetClassDef();
            Circle circle = Circle.CreateSavedCircle();

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            FilledCircle loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

            //---------------Test Result -----------------------
            Assert.AreNotSame(loadedCircle, circle);
            Assert.AreEqual(circle.Radius, loadedCircle.Radius);
            Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
            Assert.IsFalse(loadedCircle.Status.IsNew);
            Assert.IsFalse(loadedCircle.Status.IsDeleted);
            Assert.IsFalse(loadedCircle.Status.IsEditing);
            Assert.IsFalse(loadedCircle.Status.IsDirty);
            Assert.AreEqual("", loadedCircle.Status.IsValidMessage);
            Assert.IsTrue(loadedCircle.Status.IsValid());
        }

        [Test]
        public void TestLoad_ConcreteTableInheritance_Hierarchy_Fresh()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithConcreteInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            FilledCircle loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreNotSame(loadedFilledCircle, filledCircle);
            Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
            Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
            Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
        }

        [Test]
        public void TestLoad_SingleTableInheritance_Fresh()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();

            //---------------Execute Test ----------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            CircleNoPrimaryKey loadedCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(circle.ID);

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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            FilledCircleNoPrimaryKey loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(
                    filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreNotSame(loadedFilledCircle, filledCircle);
            Assert.AreEqual(filledCircle.Radius, loadedFilledCircle.Radius);
            Assert.AreEqual(filledCircle.ShapeName, loadedFilledCircle.ShapeName);
            Assert.AreEqual(filledCircle.Colour, loadedFilledCircle.Colour);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestLoad_ClassTableInheritance__GetBOAsParent_ThenGetBOAsShape_ThenGetAsCircle_ShouldLoadCircle()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithClassInheritanceHierarchy();
            Circle circle = Circle.CreateSavedCircle();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Assert Preconditions---------------
            //---------------Execute Test ----------------------
            Shape circleLoadedAsShape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>("ShapeID = " + circle.ShapeID);
            Circle loadedCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

            //---------------Test Result -----------------------
            Assert.IsNotNull(circleLoadedAsShape);
            Assert.AreEqual(circle.ShapeName, circleLoadedAsShape.ShapeName);

            Assert.IsNotNull(loadedCircle);
            Assert.AreNotSame(loadedCircle, circle);
            Assert.AreNotSame(loadedCircle, circleLoadedAsShape);
            Assert.AreEqual(circle.Radius, loadedCircle.Radius);
            Assert.AreEqual(circle.ShapeName, loadedCircle.ShapeName);
        }
    }
}