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
using System;
using System.Globalization;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.DB.InheritanceSqlGeneration
{
    /// <summary>
    /// Tests the default type of class table inheritance, where the child contains a
    /// foreign key to the parent ID that is not the child's ID field
    /// </summary>
    [TestFixture]
    public class TestInheritanceClassTableNoID : TestInheritanceBase
    {
        public static void RunTest()
        {
            TestInheritanceClassTableNoID test = new TestInheritanceClassTableNoID();
            test.SetupTestWithoutPrimaryKey();
            test.TestSuperClassKey();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTestWithoutPrimaryKey();
        }

        protected override void SetupInheritanceSpecifics()
        {
            SuperClassDef superClassDef = new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
            CircleNoPrimaryKey.GetClassDef().SuperClassDef = superClassDef;
            superClassDef.ID = "";
        }

        protected override void SetStrID()
        {
            strID = ((Guid)objCircle.GetPropertyValue("ShapeID")).ToString("B").ToUpper(CultureInfo.InvariantCulture);
        }

        [Test]
        public void TestCircleIsUsingClassTableInheritance()
        {
            Assert.AreEqual(ORMapping.ClassTableInheritance, CircleNoPrimaryKey.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestCircleIsNotDirty()
        {
            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
            Assert.IsFalse(circle.Status.IsDirty);
        }

        [Test]
        public void TestCircleHasShapeIDAsPrimaryKey()
        {
            try
            {
                Assert.IsTrue(objCircle.ID.Contains("ShapeID"));
                Assert.AreEqual(1, objCircle.ID.Count, "Should return the parent's primary key.");
            }
            catch (HabaneroArgumentException)
            {
                Assert.Fail("An object using ClassTableInheritance should have the subclass' primary key.");
            }
        }

        [Test]
        public void TestCircleHasCorrectPropertyNames()
        {
            //Merely testing that errros are not thrown
            objCircle.GetPropertyValue("ShapeName");
            objCircle.GetPropertyValue("Radius");
            objCircle.GetPropertyValue("ShapeID");
        }

        [Test]
        public void TestCircleInsertSql()
        {
            var sqlStatements = itsInsertSql.ToList();
            Assert.AreEqual(2, sqlStatements.Count,
                            "There should be 2 insert sql statements when using class table inheritance");
            Assert.AreEqual("INSERT INTO `Shape_table` (`ShapeID_field`, `ShapeName`) VALUES (?Param0, ?Param1)",
                            sqlStatements[0].Statement.ToString(),
                            "Class Table inheritance: First insert Sql statement is incorrect.");
            Assert.AreEqual(strID, sqlStatements[0].Parameters[0].Value,
                            "Parameter ShapeID has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual("MyShape", sqlStatements[0].Parameters[1].Value,
                            "Parameter ShapeName has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual("INSERT INTO `circle_table` (`Radius`, `ShapeID_field`) VALUES (?Param0, ?Param1)",
                            sqlStatements[1].Statement.ToString(),
                            "Class Table inheritance: Second Sql statement is incorrect.");
            Assert.AreEqual(strID, sqlStatements[1].Parameters[1].Value,
                            "Parameter CircleID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(10, sqlStatements[1].Parameters[0].Value,
                            "Parameter Radius has incorrect value in second insert statement using class table inheritance.");
        }

        [Test]
        public void TestSuperClassKey()
        {
            IBOKey msuperKey = BOPrimaryKey.GetSuperClassKey((ClassDef) CircleNoPrimaryKey.GetClassDef(), objCircle);
            Assert.IsTrue(msuperKey.Contains("ShapeID"), "Super class key should contain the ShapeID property");
            Assert.AreEqual(1, msuperKey.Count, "Super class key should only have one prop");
            Assert.AreEqual(msuperKey["ShapeID"].Value, objCircle.ID["ShapeID"].Value,
                            "ShapeID in parent and child should be the same");
        }

        //Note_: doesn't update the ShapeID because it is an ObjectID and is part of the parent's PK
        [Test]
        public void TestCircleUpdateSql()
        {
            var sqlStatements = itsUpdateSql.ToList();
            Assert.AreEqual(2, sqlStatements.Count,
                            "There should be 2 update sql statements when using class table inheritance");
            Assert.AreEqual("UPDATE `Shape_table` SET `ShapeName` = ?Param0 WHERE `ShapeID_field` = ?Param1",
                            sqlStatements[0].Statement.ToString(),
                            "Class table inheritance: first update sql statement is incorrect.");
            //Assert.AreEqual(strID, ((IDbDataParameter) _updateSql[0].Parameters[0]).Value,
            //                "Parameter ShapeID has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual("MyShape", sqlStatements[0].Parameters[0].Value,
                            "Parameter ShapeName has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(strID, sqlStatements[0].Parameters[1].Value,
                            "Parameter ShapeID in where clause has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual("UPDATE `circle_table` SET `Radius` = ?Param0 WHERE `ShapeID_field` = ?Param1",
                            sqlStatements[1].Statement.ToString(),
                            "Class table inheritance: second update sql statement is incorrect.");
            Assert.AreEqual(10, sqlStatements[1].Parameters[0].Value,
                            "Parameter Radius has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(strID, sqlStatements[1].Parameters[1].Value,
                            "Parameter ShapeID has incorrect value in second update statement using class table inheritance");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            var sqlStatements = itsDeleteSql.ToList();
            Assert.AreEqual(2, sqlStatements.Count,
                            "There should be 2 delete sql statements when using class table inheritance.");
            Assert.AreEqual("DELETE FROM `circle_table` WHERE `ShapeID_field` = ?Param0", sqlStatements[0].Statement.ToString(),
                            "Class table inheritance: first delete sql statement is incorrect.");
            Assert.AreEqual(strID, sqlStatements[0].Parameters[0].Value,
                            "Parameter ShapeID has incorrect value in first delete statement in where clause.");
            Assert.AreEqual("DELETE FROM `Shape_table` WHERE `ShapeID_field` = ?Param0", sqlStatements[1].Statement.ToString(),
                            "Class table inheritance: second delete sql statement is incorrect.");
            Assert.AreEqual(strID, sqlStatements[1].Parameters[0].Value,
                            "Parameter ShapeID has incorrect value in second delete statement in where clause.");
        }
        [Test]
        public void TestDatabaseReadWrite()
        {
            FixtureEnvironment.ClearBusinessObjectManager();
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
            Assert.IsFalse(circles[0].Props.Contains("CircleID"));
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
            shape.MarkForDelete();
            shape.Save();
            circle.MarkForDelete();
            circle.Save();
            shapes.LoadAll();
            Assert.AreEqual(0, shapes.Count);
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);
        }

        // Provided in case the above test fails and the rows remain in the database
        [TestFixtureTearDown]
        public void TearDown()
        {
            Criteria criteria1 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "MyShape");
            Criteria criteria2 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "MyShapeChanged");
            Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            Shape shape = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Shape>(
                criteria);
            if (shape != null)
            {
                shape.MarkForDelete();
                shape.Save();
            }
            criteria1 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "Circle");
            criteria2 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "CircleChanged");
            criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            CircleNoPrimaryKey circle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<CircleNoPrimaryKey>(
                criteria);
            if (circle == null) return;
            circle.MarkForDelete();
            circle.Save();
        }
    }
}