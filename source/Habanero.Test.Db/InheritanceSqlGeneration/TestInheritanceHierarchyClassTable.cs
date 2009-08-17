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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.DB.InheritanceSqlGeneration
{
    [TestFixture]
    public class TestInheritanceHierarchyClassTable : TestInheritanceHierarchyBase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTest();
        }

        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
            FilledCircle.GetClassDef().SuperClassDef =
                new SuperClassDef(Circle.GetClassDef(), ORMapping.ClassTableInheritance);
        }

        protected override void SetStrID()
        {
            _filledCircleId = (string) DatabaseUtil.PrepareValue(_filledCircle.GetPropertyValue("FilledCircleID"));
        }

        [Test]
        public void TestFilledCircleIsUsingClassTableInheritance()
        {
            Assert.AreEqual(ORMapping.ClassTableInheritance, Circle.GetClassDef().SuperClassDef.ORMapping);
            Assert.AreEqual(ORMapping.ClassTableInheritance, FilledCircle.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestCircleHasCircleIDAsPrimaryKey()
        {
            try
            {
                Assert.IsTrue(_filledCircle.ID.Contains("FilledCircleID"));
                Assert.AreEqual(1, _filledCircle.ID.Count,
                                "There should only be one item in the primary key (even when using class table inheritance).");
            }
            catch (HabaneroArgumentException)
            {
                Assert.Fail("An object using ClassTableInheritance should have the subclass' primary key.");
            }
        }

        [Test]
        public void TestCircleHasCorrectPropertyNames()
        {
            _filledCircle.GetPropertyValue("ShapeName");
            _filledCircle.GetPropertyValue("Radius");
            _filledCircle.GetPropertyValue("CircleID");
            _filledCircle.GetPropertyValue("ShapeID");
            _filledCircle.GetPropertyValue("FilledCircleID");
            _filledCircle.GetPropertyValue("Colour");
        }


        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(3, _insertSql.Count,
                            "There should be 3 insert sql statements when using class table inheritance");
            Assert.AreEqual("INSERT INTO `Shape_table` (`ShapeID_field`, `ShapeName`) VALUES (?Param0, ?Param1)",
                            _insertSql[0].Statement.ToString(),
                            "Class Table inheritance: First insert Sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", _insertSql[0].Parameters[1].Value,
                            "Parameter ShapeName has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, _insertSql[0].Parameters[0].Value,
                            "Parameter ShapeID has incorrect value in first insert statement using class table inheritance");

            Assert.AreEqual("INSERT INTO `circle_table` (`CircleID_field`, `Radius`, `ShapeID_field`) VALUES (?Param0, ?Param1, ?Param2)",
                            _insertSql[1].Statement.ToString(),
                            "Class Table inheritance: Second Sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, _insertSql[1].Parameters[0].Value,
                            "Parameter CircleID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(_filledCircleId, _insertSql[1].Parameters[2].Value,
                            "Parameter ShapeID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(10, _insertSql[1].Parameters[1].Value,
                            "Parameter Radius has incorrect value in second insert statement using class table inheritance.");

            Assert.AreEqual(
                "INSERT INTO `FilledCircle_table` (`CircleID_field`, `Colour`, `FilledCircleID_field`) VALUES (?Param0, ?Param1, ?Param2)",
                _insertSql[2].Statement.ToString(), "Class Table inheritance: Third Sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, _insertSql[2].Parameters[2].Value,
                            "Parameter FilledCircleID  has incorrect value in third insert statement using class table inheritance.");
            Assert.AreEqual(3, _insertSql[2].Parameters[1].Value,
                            "Parameter Colour has incorrect value in third insert statement using class table inheritance.");
            Assert.AreEqual(_filledCircleId, _insertSql[2].Parameters[0].Value,
                            "Parameter CircleID has incorrect value in third insert statement using class table inheritance.");
        }

        [Test]
        public void TestSuperClassKey()
        {
            IBOKey msuperKey = BOPrimaryKey.GetSuperClassKey((ClassDef) FilledCircle.GetClassDef(), _filledCircle);
            Assert.IsTrue(msuperKey.Contains("CircleID"), "Super class key should contain the CircleID property");
            Assert.AreEqual(1, msuperKey.Count, "Super class key should only have one prop");
            Assert.AreEqual(msuperKey["CircleID"].Value,
                            _filledCircle.ID["FilledCircleID"].Value,
                            "CircleID and FilledCircleID should be the same");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(3, _updateSql.Count,
                            "There should be 3 update sql statements when using class table inheritance");

            Assert.AreEqual("UPDATE `circle_table` SET `CircleID_field` = ?Param0, `Radius` = ?Param1 WHERE `CircleID_field` = ?Param2",
                            _updateSql[0].Statement.ToString(),
                            "Class table inheritance: first update sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, _updateSql[0].Parameters[0].Value,
                            "Parameter CircleID has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(10, _updateSql[0].Parameters[1].Value,
                            "Parameter Radius has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, _updateSql[0].Parameters[2].Value,
                            "Parameter CircleID in where clause has incorrect value in first update statement using class table inheritance");

            Assert.AreEqual("UPDATE `Shape_table` SET `ShapeID_field` = ?Param0, `ShapeName` = ?Param1 WHERE `ShapeID_field` = ?Param2",
                            _updateSql[1].Statement.ToString(),
                            "Class table inheritance: second update sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", _updateSql[1].Parameters[1].Value,
                            "Parameter ShapeName has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, _updateSql[1].Parameters[0].Value,
                            "Parameter ShapeID has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, _updateSql[1].Parameters[2].Value,
                            "Parameter ShapeID in where clause has incorrect value in second update statement using class table inheritance");

            Assert.AreEqual("UPDATE `FilledCircle_table` SET `Colour` = ?Param0 WHERE `FilledCircleID_field` = ?Param1",
                            _updateSql[2].Statement.ToString(),
                            "Class table inheritance: third update sql statement is incorrect.");
            Assert.AreEqual(3, _updateSql[2].Parameters[0].Value,
                            "Parameter Colour has incorrect value in third update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, _updateSql[2].Parameters[1].Value,
                            "Parameter FilledCircleID has incorrect value in third update statement using class table inheritance");
        }

        [Test]
        public void TestUpdateWhenOnlyOneLevelUpdates()
        {
            IMock connectionControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection connection = (IDatabaseConnection) connectionControl.MockInstance;
            //connectionControl.ExpectAndReturn("LoadDataReader", null, new object[] {null});
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("ExecuteSql", 3, new object[] { null, null });

            FilledCircle myCircle = new FilledCircle();
//            myCircle.SetDatabaseConnection(connection);
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(myCircle);
            committer.CommitTransaction();
            //myCircle.Save();
            myCircle.SetPropertyValue("Colour", 4);

            SqlStatementCollection myUpdateSql = new UpdateStatementGenerator(myCircle, DatabaseConnection.CurrentConnection).Generate();
            Assert.AreEqual(1, myUpdateSql.Count);
            connectionControl.Verify();
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(3, _deleteSql.Count,
                            "There should be 3 delete sql statements when using class table inheritance.");
            Assert.AreEqual("DELETE FROM `FilledCircle_table` WHERE `FilledCircleID_field` = ?Param0",
                            _deleteSql[0].Statement.ToString(),
                            "Class table inheritance: first delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, _deleteSql[0].Parameters[0].Value,
                            "Parameter CircleID has incorrect value in first delete statement in where clause.");
            Assert.AreEqual("DELETE FROM `circle_table` WHERE `CircleID_field` = ?Param0", _deleteSql[1].Statement.ToString(),
                            "Class table inheritance: second delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, _deleteSql[1].Parameters[0].Value,
                            "Parameter CircleID has incorrect value in second delete statement in where clause.");
            Assert.AreEqual("DELETE FROM `Shape_table` WHERE `ShapeID_field` = ?Param0", _deleteSql[2].Statement.ToString(),
                            "Class table inheritance: third delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, _deleteSql[2].Parameters[0].Value,
                            "Parameter ShapeID has incorrect value in third delete statement in where clause.");
        }

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

            BusinessObjectCollection<Circle> circles = new BusinessObjectCollection<Circle>();
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);

            BusinessObjectCollection<FilledCircle> filledCircles = new BusinessObjectCollection<FilledCircle>();
            filledCircles.LoadAll();
            Assert.AreEqual(0, filledCircles.Count);

            Circle circle = new Circle();
            circle.Radius = 5;
            circle.ShapeName = "Circle";
            circle.Save();

            shapes.LoadAll("ShapeName");
            Assert.AreEqual(2, shapes.Count);
            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("MyShape", shapes[1].ShapeName);

            circles.LoadAll("ShapeName");
            Assert.AreEqual(1, circles.Count);
            Assert.AreEqual(circles[0].ShapeID, shapes[0].ShapeID);
            Assert.AreEqual(5, circles[0].Radius);
            Assert.AreEqual("Circle", circles[0].ShapeName);

            FilledCircle filledCircle = new FilledCircle();
            filledCircle.Colour = 3;
            filledCircle.Radius = 7;
            filledCircle.ShapeName = "FilledCircle";
            filledCircle.Save();

            shapes.LoadAll("ShapeName");
            Assert.AreEqual(3, shapes.Count);
            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("FilledCircle", shapes[1].ShapeName);
            Assert.AreEqual("MyShape", shapes[2].ShapeName);

            circles.LoadAll("ShapeName");
            Assert.AreEqual(2, circles.Count);
            Assert.AreEqual(circles[0].ShapeID, shapes[0].ShapeID);
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
            circles.LoadAll();
            Assert.AreEqual(0, circles.Count);
            filledCircles.LoadAll();
            Assert.AreEqual(0, filledCircles.Count);
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
            Circle circle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Circle>(
                criteria);
            if (circle != null)
            {
                circle.MarkForDelete();
                circle.Save();
            }

            criteria1 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "FilledCircle");
            criteria2 = new Criteria("ShapeName", Criteria.ComparisonOp.Equals, "FilledCircleChanged");
            criteria = new Criteria(criteria1, Criteria.LogicalOp.Or, criteria2);
            FilledCircle filledCircle = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<FilledCircle>(
                criteria);
            if (filledCircle == null) return;
            filledCircle.MarkForDelete();
            filledCircle.Save();
        }
    }
}