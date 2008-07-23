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

using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// This is testing inheritance implemented using the single table patterns (See Martin Fowler. Patterns of Enterprise Application Architecture).
    /// The Base Class is Shape. CircleNoPrimaryKey Inherits From Shape. NB the Class must be loaded using the Class Def - GetClassDefWithSingleInheritance. 
    /// Filled Circle (FilledCircleNoPrimaryKey) Inherits From CircleNoPrimaryKey.
    ///     NB the Class must be loaded using the Class Def -  GetClassDefWithSingleInheritanceHierarchy.
    ///     
    /// The table used is shape_table. The Discriminator Fields are ShapeType_field and CircleType_field.
    /// Shape has two Fields ShapeName and ShapeId.
    /// 
    /// CircleNoPrimaryKey has the additional field - Radius.
    /// 
    /// FilledCircleNoPrimaryKey has add field - Colour.
    /// </summary>
    [TestFixture]
    public class TestInheritanceSingleTable : TestInheritanceBase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTestWithoutPrimaryKey();
        }

        protected override void SetupInheritanceSpecifics()
        {
            CircleNoPrimaryKey.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            CircleNoPrimaryKey.GetClassDef().SuperClassDef.Discriminator = "ShapeType_field";
        }

        protected override void SetStrID()
        {
            strID = (string) DatabaseUtil.PrepareValue(objCircle.GetPropertyValue("ShapeID"));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TransactionCommitterDB committer = new TransactionCommitterDB();
            BusinessObjectCollection<Shape> shapes = BOLoader.Instance.GetBusinessObjectCol<Shape>(
                "ShapeName = 'MyShape' OR ShapeName = 'MyShapeChanged'", null);
            foreach (Shape shape in shapes)
            {
                shape.Delete();
                committer.AddBusinessObject(shape);
            }

            BusinessObjectCollection<CircleNoPrimaryKey> circles = BOLoader.Instance.GetBusinessObjectCol
                <CircleNoPrimaryKey>(
                "ShapeName = 'Circle' OR ShapeName = 'CircleChanged'", null);
            foreach (CircleNoPrimaryKey circle in circles)

            {
                circle.Delete();
                committer.AddBusinessObject(circle);
            }
            committer.CommitTransaction();
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(1, itsDeleteSql.Count,
                            "There should only be one delete sql statement when using single table inheritance.");
            Assert.AreEqual("DELETE FROM `Shape_table` WHERE `ShapeID_field` = ?Param0",
                            itsDeleteSql[0].Statement.ToString(),
                            "Delete Sql for single table inheritance is incorrect.");
            Assert.AreEqual(strID, (itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value for delete sql when using Single Table inheritance.");
        }

        [Test, ExpectedException(typeof (InvalidPropertyNameException))]
        public void TestCircleDoesntHaveCircleID()
        {
            objCircle.GetPropertyValue("CircleID");
        }

        [Test]
        public void TestCircleHasCorrectPropertyNames()
        {
            objCircle.GetPropertyValue("ShapeName");
            objCircle.GetPropertyValue("Radius");
            objCircle.GetPropertyValue("ShapeID");
        }

        [Test]
        public void TestCircleHasShapeIDAsPrimaryKey()
        {
            Assert.AreEqual(1, objCircle.ID.Count);
            try
            {
                objCircle.ID.Contains("ShapeID");
            }
            catch (HabaneroArgumentException)
            {
                Assert.Fail("An object using SingleTableInheritance should receive its superclass as its primary key.");
            }
        }

        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(1, itsInsertSql.Count,
                            "There should only be one insert Sql statement when using Single Table Inheritance.");
            Assert.AreEqual(
                "INSERT INTO `Shape_table` (`ShapeType_field`, `Radius`, `ShapeID_field`, `ShapeName`) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                itsInsertSql[0].Statement.ToString(),
                "Concrete Table Inheritance insert Sql seems to be incorrect.");
            Assert.AreEqual(4, itsInsertSql[0].Parameters.Count, "There should be 4 parameters.");
            Assert.AreEqual("CircleNoPrimaryKey", (itsInsertSql[0].Parameters[0]).Value,
                            "Discriminator has incorrect value");
            Assert.AreEqual(10, (itsInsertSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(strID, (itsInsertSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
            Assert.AreEqual("MyShape", (itsInsertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
        }

        [Test]
        public void TestCircleIsUsingSingleTableInheritance()
        {
            Assert.AreEqual(ORMapping.SingleTableInheritance, CircleNoPrimaryKey.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(1, itsUpdateSql.Count,
                            "There should only be one update sql statement when using single table inheritance.");
            Assert.AreEqual(
                "UPDATE `Shape_table` SET `Radius` = ?Param0, `ShapeName` = ?Param1 WHERE `ShapeID_field` = ?Param2",
                itsUpdateSql[0].Statement.ToString());
            // Is Object ID so doesn't get changed
            //Assert.AreEqual(strID, ((IDbDataParameter) _updateSql[0].Parameters[1]).Value,
            //                "Parameter ShapeID has incorrect value");
            Assert.AreEqual("MyShape", (itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, (itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(strID, (itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        // TODO: Would like to separate these tests out later, but needs a structure
        //  change and I'm out of time right now.
        [Test]
        public void TestDatabaseReadWrite()
        {
            // Test inserting & selecting
            Shape shape = new Shape();
            shape.ShapeName = "MyShape";
            shape.Save();

            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            shapes.LoadAll();
            Assert.AreEqual(1, shapes.Count);

            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);

            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
            circle.Radius = 5;
            circle.ShapeName = "Circle";
            circle.Save();

            shapes.LoadAll("ShapeName");
            Assert.AreEqual(2, shapes.Count);
            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("MyShape", shapes[1].ShapeName);

            circles.LoadAll();
            Assert.AreEqual(1, circles.Count);
            Assert.AreEqual(circles[0].ShapeID, shapes[0].ShapeID);
            Assert.AreEqual(5, circles[0].Radius);
            Assert.AreEqual("Circle", circles[0].ShapeName);

            // Test updating
            shape.ShapeName = "MyShapeChanged";
            shape.Save();
            circle.ShapeName = "CircleChanged";
            circle.Radius = 10;
            circle.Save();

            shapes.LoadAll("ShapeName");
            Assert.AreEqual("CircleChanged", shapes[0].ShapeName);
            Assert.AreEqual("MyShapeChanged", shapes[1].ShapeName);
            circles.LoadAll();
            Assert.AreEqual(10, circles[0].Radius);
            Assert.AreEqual("CircleChanged", circles[0].ShapeName);

            // Test deleting
            shape.Delete();
            shape.Save();
            circle.Delete();
            circle.Save();
            shapes.LoadAll();
            Assert.AreEqual(0, shapes.Count);
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);
        }


        [Test]
        public void TestLoadingRelatedObjectWithSingleTableInheritance()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection.ExecuteRawSql(
                "delete from filledcircle_table; delete from circle_table; delete from shape_table");
            //MyBO has a relationship to Shape. Shape potentially has a circle for single table inheritance.
            MyBO.LoadClassDefWithShape_SingleTableInheritance_Relationship();

            MyBO bo = new MyBO();
            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
            circle.Radius = 5;
            circle.ShapeName = "MyShape";
            circle.Save();
            bo.SetPropertyValue("ShapeID", circle.ShapeID);
            bo.Save();

            BOLoader.Instance.ClearLoadedBusinessObjects();

            //---------------Execute Test ----------------------
            bo = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(bo.ID);
            Shape shape = bo.Shape;

            //---------------Test Result -----------------------
            Assert.AreSame(typeof (CircleNoPrimaryKey), shape.GetType());
            Assert.IsFalse(shape.State.IsNew);
            Assert.IsFalse(shape.State.IsDeleted);
            Assert.IsFalse(shape.State.IsEditing);
            Assert.IsFalse(shape.State.IsDirty);
            Assert.IsTrue(shape.State.IsValid());
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT `Shape_table`.`Radius`, `Shape_table`.`ShapeID_field`, `Shape_table`.`ShapeName` FROM `Shape_table` WHERE `ShapeType_field` = 'CircleNoPrimaryKey' AND `ShapeID_field` = ?Param0",
                selectSql.Statement.ToString(), "Select sql is incorrect for single table inheritance.");

            Assert.AreEqual(strID, (selectSql.Parameters[0]).Value,
                            "Parameter ShapeID is incorrect in select where clause for single table inheritance.");
        }

        [Test, ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void TestUniqueKeyValidationForSubTypesOfSingleTableInheritanceStructure()
        {
            //Should not be allowed to save circle with the same shape name as shape.
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection.ExecuteRawSql(
                "delete from filledcircle_table; delete from circle_table; delete from shape_table");
            Shape shape = new Shape();
            shape.ShapeName = "MyShape";
            shape.Save();
            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
            circle.Radius = 5;
            circle.ShapeName = "MyShape";

            //---------------Execute Test ----------------------
            circle.Save();
        }


        // Provided in case the above test fails and the rows remain in the database

    }
}