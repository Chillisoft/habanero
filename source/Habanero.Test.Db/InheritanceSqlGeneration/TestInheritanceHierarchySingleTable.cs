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
using System;
using System.Globalization;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB.InheritanceSqlGeneration
{
    /// <summary>
    /// This class tests an inheritance Hierarchy of three classes, with the
    /// children both using single table inheritance
    /// </summary>
    [TestFixture]
    public class TestInheritanceHierarchySingleTable : TestInheritanceHierarchyBase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupFixture()
        {
            SetupTestForFilledCircleNoPK();
        }

        [TearDown]
        public void TearDown()
        {
            Criteria criteria1 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "MyShape");
            Shape shape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(
                criteria1);
            if (shape != null)
            {
                shape.MarkForDelete();
                shape.Save();
            }
            criteria1 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "Circle");
            Criteria criteria2 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "CircleChanged");
            Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            CircleNoPrimaryKey circle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(
                criteria);
            if (circle != null)
            {
                circle.MarkForDelete();
                circle.Save();
            }
            criteria1 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "FilledCircle");
            criteria2 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "FilledCircleChanged");
            criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            FilledCircleNoPrimaryKey filledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircleNoPrimaryKey>(
                criteria);
            if (filledCircle == null) return;
            filledCircle.MarkForDelete();
            filledCircle.Save();
        }

        #endregion

        private IClassDef _classDefCircleNoPrimaryKey;
        private IClassDef _classDefShape;
        private IClassDef _classDefFilledCircleNoPrimaryKey;

        protected override void SetupInheritanceSpecifics()
        {
            ClassDef.ClassDefs.Clear();
            FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            //_classDefShape = Shape.GetClassDef();
            //_classDefCircleNoPrimaryKey = CircleNoPrimaryKey.GetClassDef();
            //_classDefCircleNoPrimaryKey.SuperClassDef = new SuperClassDef(_classDefShape,
            //                                                              ORMapping.SingleTableInheritance);
            //_classDefCircleNoPrimaryKey.SuperClassDef.Discriminator = "ShapeType_field";
            //_classDefFilledCircleNoPrimaryKey = FilledCircleNoPrimaryKey.GetClassDef();
            //_classDefFilledCircleNoPrimaryKey.SuperClassDef = new SuperClassDef(_classDefCircleNoPrimaryKey,
            //                                                                    ORMapping.SingleTableInheritance);
            //_classDefFilledCircleNoPrimaryKey.SuperClassDef.Discriminator = "ShapeType_field";
        }

        protected override void SetStrID()
        {
            _filledCircleId = ((Guid)_filledCircle.GetPropertyValue("ShapeID")).ToString("B").ToUpper(CultureInfo.InvariantCulture);
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            var sqlStatements = _deleteSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one delete sql statement when using single table inheritance.");
            Assert.AreEqual("DELETE FROM `Shape_table` WHERE `ShapeID_field` = ?Param0",
                            sqlStatements[0].Statement.ToString(),
                            "Delete Sql for single table inheritance is incorrect.");
            Assert.AreEqual(_filledCircleId, (sqlStatements[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value for delete sql when using Single Table inheritance.");
        }

        [Test]
        public void TestCircleInsertSql()
        {
            var sqlStatements = _insertSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one insert Sql statement when using Single Table Inheritance.");
            Assert.AreEqual(
                "INSERT INTO `Shape_table` (`Colour`, `Radius`, `ShapeID_field`, `ShapeName`, `ShapeType_field`) VALUES (?Param0, ?Param1, ?Param2, ?Param3, ?Param4)",
                sqlStatements[0].Statement.ToString(), "Single Table Inheritance insert Sql seems to be incorrect.");
            Assert.AreEqual(3, sqlStatements[0].Parameters[0].Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual(10, (sqlStatements[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(_filledCircleId, (sqlStatements[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
            Assert.AreEqual("MyFilledCircle", (sqlStatements[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual("FilledCircleNoPrimaryKey", sqlStatements[0].Parameters[4].Value,
                "Discriminator has incorrect value");

        }

        [Test]
        public void TestCircleUpdateSql()
        {
            var sqlStatements = _updateSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one update sql statement when using single table inheritance.");
            Assert.AreEqual(
                "UPDATE `Shape_table` SET `Colour` = ?Param0, `Radius` = ?Param1, `ShapeName` = ?Param2, `ShapeType_field` = ?Param3 WHERE `ShapeID_field` = ?Param4",
                sqlStatements[0].Statement.ToString());
            Assert.AreEqual(3, (sqlStatements[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual(10, (sqlStatements[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual("MyFilledCircle", (sqlStatements[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value");
            //Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[0].Parameters[2]).Value,
            //                "Parameter ShapeID has incorrect value");
            Assert.AreEqual("FilledCircleNoPrimaryKey", sqlStatements[0].Parameters[3].Value,
                            "Discriminator has incorrect value");
            Assert.AreEqual(_filledCircleId, (sqlStatements[0].Parameters[4]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        //Would like to separate these tests out later, but needs a structure
        //  change and I'm out of time right now.
        [Test]
        public void TestDatabaseReadWrite()
        {
            // Test inserting & selecting
            Shape shape = new Shape {ShapeName = "MyShape"};
            shape.Save();

            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            shapes.LoadAll();
            Assert.AreEqual(1, shapes.Count);

            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);

            BusinessObjectCollection<FilledCircleNoPrimaryKey> filledCircles =
                new BusinessObjectCollection<FilledCircleNoPrimaryKey>();
            filledCircles.LoadAll();
            Assert.AreEqual(0, filledCircles.Count);

            CircleNoPrimaryKey circle = new CircleNoPrimaryKey {Radius = 5, ShapeName = "Circle"};
            circle.Save();

            BusinessObjectManager.Instance.ClearLoadedObjects();

            shapes.LoadAll("ShapeName");
            Assert.AreEqual(2, shapes.Count);
            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("MyShape", shapes[1].ShapeName);

            BusinessObjectManager.Instance.ClearLoadedObjects();
            circles.LoadAll();
            Assert.AreEqual(1, circles.Count);
            Assert.AreEqual(circles[0].ShapeID, shapes[0].ShapeID);
            Assert.AreEqual(5, circles[0].Radius);
            Assert.AreEqual("Circle", circles[0].ShapeName);

            FilledCircleNoPrimaryKey filledCircle = new FilledCircleNoPrimaryKey
                                                        {
                                                            Colour = 3,
                                                            Radius = 7,
                                                            ShapeName = "FilledCircle"
                                                        };
            filledCircle.Save();

            BusinessObjectManager.Instance.ClearLoadedObjects();
            shapes.LoadAll("ShapeName");
            Assert.AreEqual(3, shapes.Count);
            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("FilledCircle", shapes[1].ShapeName);
            Assert.AreEqual("MyShape", shapes[2].ShapeName);
            Assert.That(shapes[0], Is.InstanceOf(typeof(CircleNoPrimaryKey)));
            Assert.That(shapes[1], Is.InstanceOf(typeof(FilledCircleNoPrimaryKey)));
            Assert.That(shapes[2], Is.InstanceOf(typeof(Shape)));

            circles.LoadAll("ShapeName");
            Assert.AreEqual(2, circles.Count);
            Assert.AreEqual(circles[1].ShapeID, shapes[1].ShapeID);
            Assert.AreEqual(7, circles[1].Radius);
            Assert.AreEqual("FilledCircle", circles[1].ShapeName);

            filledCircles.LoadAll();
            Assert.AreEqual(1, filledCircles.Count);
            Assert.AreEqual(filledCircles[0].ShapeID, shapes[1].ShapeID);
            Assert.AreEqual(7, filledCircles[0].Radius);
            Assert.AreEqual("FilledCircle", filledCircles[0].ShapeName);
            Assert.AreEqual(3, filledCircles[0].Colour);

            // Test updating
            shape.ShapeName = "MyShapeChanged";
            shape.Save();
            circle.ShapeName = "CircleChanged";
            circle.Radius = 10;
            circle.Save();
            filledCircle.ShapeName = "FilledCircleChanged";
            filledCircle.Radius = 12;
            filledCircle.Colour = 4;
            filledCircle.Save();

            shapes.LoadAll("ShapeName");
            Assert.AreEqual("CircleChanged", shapes[0].ShapeName);
            Assert.AreEqual("FilledCircleChanged", shapes[1].ShapeName);
            Assert.AreEqual("MyShapeChanged", shapes[2].ShapeName);
            circles.LoadAll("ShapeName");
            Assert.AreEqual(10, circles[0].Radius);
            Assert.AreEqual(12, circles[1].Radius);
            Assert.AreEqual("CircleChanged", circles[0].ShapeName);
            Assert.AreEqual("FilledCircleChanged", circles[1].ShapeName);
            filledCircles.LoadAll();
            Assert.AreEqual(4, filledCircles[0].Colour);
            Assert.AreEqual(12, filledCircles[0].Radius);
            Assert.AreEqual("FilledCircleChanged", filledCircles[0].ShapeName);

            // Test deleting
            shape.MarkForDelete();
            shape.Save();
            circle.MarkForDelete();
            circle.Save();
            filledCircle.MarkForDelete();
            filledCircle.Save();

            shapes.LoadAll();
            Assert.AreEqual(0, shapes.Count);
            Assert.AreEqual(0, shapes.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, shapes.CreatedBusinessObjects.Count);
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);
            filledCircles.LoadAll();
            Assert.AreEqual(0, filledCircles.Count);
        }

        [Test]
        public void TestFilledCircleDoesntHaveCircleID()
        {
            try
            {
                _filledCircle.GetPropertyValue("CircleID");
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("The given property name 'CircleID' does not exist in the collection of properties for the class 'FilledCircleNoPrimaryKey'", ex.Message);
            }
        }

        [Test]
        public void TestFilledCircleHasCorrectPropertyNames()
        {
            _filledCircle.GetPropertyValue("ShapeName");
            _filledCircle.GetPropertyValue("Radius");
            _filledCircle.GetPropertyValue("ShapeID");
            _filledCircle.GetPropertyValue("Colour");
        }

        [Test]
        public void TestFilledCircleHasShapeIDAsPrimaryKey()
        {
            try
            {
                _filledCircle.ID.Contains("ShapeID");
            }
            catch (HabaneroArgumentException)
            {
                Assert.Fail("An object using SingleTableInheritance should receive its superclass as its primary key.");
            }
        }

        [Test]
        public void TestFilledCircleIsUsingSingleTableInheritance()
        {
            Assert.AreEqual(ORMapping.SingleTableInheritance, CircleNoPrimaryKey.GetClassDef().SuperClassDef.ORMapping);
            Assert.AreEqual(ORMapping.SingleTableInheritance,
                            FilledCircleNoPrimaryKey.GetClassDef().SuperClassDef.ORMapping);
        }
        // Provided in case the above test fails and the rows remain in the database

        //[Test]
        //public void Test_Load()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef classDef = ContactPerson.CreateClassDefWithShapeRelationship();
        //    ContactPerson contactPerson = new ContactPerson();
        //    CircleNoPrimaryKey circleNoPrimaryKey = new CircleNoPrimaryKey();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    SingleRelationship<ContactPerson>  relationship = (SingleRelationship<ContactPerson>) circleNoPrimaryKey.Relationships["ContactPerson"];
        //    relationship.SetRelatedObject(contactPerson);
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(contactPerson);
        //}

        
    }
}