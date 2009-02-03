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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    public abstract class TestBusinessObjectLoaderWithInheritance
    {
        protected abstract void SetupDataAccessor();

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

        #region Nested type: TestBusinessObjectLoaderDB

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
                Assert.IsInstanceOfType(typeof(CircleNoPrimaryKey), loadedShape);
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
                Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
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
                Assert.IsInstanceOfType(typeof(FilledCircleNoPrimaryKey), loadedShape);
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
        }

        #endregion

        #region Nested type: TestBusinessObjectLoaderInMemoryWithInheritance

        [TestFixture]
        public class TestBusinessObjectLoaderInMemoryWithInheritance : TestBusinessObjectLoaderWithInheritance
        {
            private DataStoreInMemory _dataStore;

            protected override void SetupDataAccessor()
            {
                _dataStore = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
            }
        }

        #endregion


        //[Test]
        //public void TestLoad_ClassTableInheritance()
        //{
        //    //---------------Set up test pack-------------------
        //    Circle.GetClassDef();
        //    Circle circle = Circle.CreateSavedCircle();

        //    //---------------Execute Test ----------------------
        //    Circle loadedCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(circle.ID);

        //    //---------------Test Result -----------------------
        //    Assert.AreSame(loadedCircle, circle);
        //}

        [Test]
        public void TestLoad_ClassTableInheritance()
        {
            //---------------Set up test pack-------------------
            Entity.LoadDefaultClassDef();
            Part.LoadClassDef_WithClassTableInheritance();
            Part part = Part.CreateSavedPart();

            //---------------Execute Test ----------------------
            Part loadedPart = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Part>(part.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(part, loadedPart);
        }

        [Test]
        public void TestLoad_ClassTableInheritance_WithCriteriaOnBase()
        {
            //---------------Set up test pack-------------------
            Entity.LoadDefaultClassDef();
            Part.LoadClassDef_WithClassTableInheritance();
            string entityType = TestUtil.GetRandomString();
            Part part = Part.CreateSavedPart();
            part.EntityType = entityType;
            part.Save();
            Criteria criteria = new Criteria("EntityType", Criteria.ComparisonOp.Equals, entityType);

            //---------------Execute Test ----------------------
            Part loadedPart = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Part>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(part, loadedPart);
        }

        //[Test]
        //public void TestLoad_ClassTableInheritance_Hierarchy()
        //{
        //    //---------------Set up test pack-------------------
        //    FilledCircle.GetClassDefWithClassInheritanceHierarchy();
        //    FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

        //    //---------------Execute Test ----------------------
        //    FilledCircle loadedFilledCircle =
        //        BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

        //    //---------------Test Result -----------------------
        //    Assert.AreSame(loadedFilledCircle, filledCircle);
        //}

        [Test]
        public void TestLoad_ClassTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithClassInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();

            //---------------Execute Test ----------------------
            FilledCircle loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(filledCircle, loadedFilledCircle);
        }

        [Test]
        public void TestLoad_SingleTableInheritance()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            CircleNoPrimaryKey circle = CircleNoPrimaryKey.CreateSavedCircle();

            //---------------Execute Test ----------------------
            CircleNoPrimaryKey loadedCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(circle.ID);

            //---------------Test Result -----------------------

            Assert.AreSame(circle, loadedCircle);
        }

        [Test]
        public void TestLoad_SingleTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            FilledCircleNoPrimaryKey filledCircle = FilledCircleNoPrimaryKey.CreateSavedFilledCircle();

            //---------------Execute Test ----------------------
            FilledCircleNoPrimaryKey loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(filledCircle, loadedFilledCircle);
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
            Assert.AreSame(circle, loadedCircle);
        }

        [Test]
        public void TestLoad_ConcreteTableInheritance_Hierarchy()
        {
            //---------------Set up test pack-------------------
            FilledCircle.GetClassDefWithConcreteInheritanceHierarchy();
            FilledCircle filledCircle = FilledCircle.CreateSavedFilledCircle();
            //---------------Execute Test ----------------------
            FilledCircle loadedFilledCircle =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(filledCircle.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(filledCircle, loadedFilledCircle);
        }
    }
}