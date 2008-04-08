//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using System.Data;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Util;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestInheritanceHeirarchyClassTable : TestInheritanceHeirarchyBase
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
            Assert.AreEqual("INSERT INTO `Shape` (`ShapeID`, `ShapeName`) VALUES (?Param0, ?Param1)",
                            _insertSql[0].Statement.ToString(),
                            "Class Table inheritance: First insert Sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) _insertSql[0].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _insertSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in first insert statement using class table inheritance");

            Assert.AreEqual("INSERT INTO `Circle` (`CircleID`, `Radius`, `ShapeID`) VALUES (?Param0, ?Param1, ?Param2)",
                            _insertSql[1].Statement.ToString(),
                            "Class Table inheritance: Second Sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _insertSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _insertSql[1].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(10, ((IDbDataParameter) _insertSql[1].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in second insert statement using class table inheritance.");

            Assert.AreEqual(
                "INSERT INTO `FilledCircle` (`CircleID`, `Colour`, `FilledCircleID`) VALUES (?Param0, ?Param1, ?Param2)",
                _insertSql[2].Statement.ToString(), "Class Table inheritance: Third Sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _insertSql[2].Parameters[2]).Value,
                            "Parameter FilledCircleID  has incorrect value in third insert statement using class table inheritance.");
            Assert.AreEqual(3, ((IDbDataParameter) _insertSql[2].Parameters[1]).Value,
                            "Parameter Colour has incorrect value in third insert statement using class table inheritance.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _insertSql[2].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in third insert statement using class table inheritance.");
        }

        [Test]
        public void TestSuperClassKey()
        {
            BOKey msuperKey = BOPrimaryKey.GetSuperClassKey(FilledCircle.GetClassDef(), _filledCircle);
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

            Assert.AreEqual("UPDATE `Circle` SET `CircleID` = ?Param0, `Radius` = ?Param1 WHERE `CircleID` = ?Param2",
                            _updateSql[0].Statement.ToString(),
                            "Class table inheritance: first update sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(10, ((IDbDataParameter) _updateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[0].Parameters[2]).Value,
                            "Parameter CircleID in where clause has incorrect value in first update statement using class table inheritance");

            Assert.AreEqual("UPDATE `Shape` SET `ShapeID` = ?Param0, `ShapeName` = ?Param1 WHERE `ShapeID` = ?Param2",
                            _updateSql[1].Statement.ToString(),
                            "Class table inheritance: second update sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) _updateSql[1].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[1].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[1].Parameters[2]).Value,
                            "Parameter ShapeID in where clause has incorrect value in second update statement using class table inheritance");

            Assert.AreEqual("UPDATE `FilledCircle` SET `Colour` = ?Param0 WHERE `FilledCircleID` = ?Param1",
                            _updateSql[2].Statement.ToString(),
                            "Class table inheritance: third update sql statement is incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) _updateSql[2].Parameters[0]).Value,
                            "Parameter Colour has incorrect value in third update statement using class table inheritance");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[2].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value in third update statement using class table inheritance");
        }

        [Test]
        public void TestUpdateWhenOnlyOneLevelUpdates()
        {
            IMock connectionControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection connection = (IDatabaseConnection) connectionControl.MockInstance;
            connectionControl.ExpectAndReturn("LoadDataReader", null, new object[] {null});
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
			connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
			connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
			connectionControl.ExpectAndReturn("ExecuteSql", 3, new object[] { null, null });

            FilledCircle myCircle = new FilledCircle();
            myCircle.SetDatabaseConnection(connection);
            myCircle.Save();
            myCircle.SetPropertyValue("Colour", 4);

            SqlStatementCollection myUpdateSql = myCircle.GetUpdateSql();
            Assert.AreEqual(1, myUpdateSql.Count);
            connectionControl.Verify();
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(3, _deleteSql.Count,
                            "There should be 3 delete sql statements when using class table inheritance.");
            Assert.AreEqual("DELETE FROM `FilledCircle` WHERE `FilledCircleID` = ?Param0",
                            _deleteSql[0].Statement.ToString(),
                            "Class table inheritance: first delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _deleteSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first delete statement in where clause.");
            Assert.AreEqual("DELETE FROM `Circle` WHERE `CircleID` = ?Param0", _deleteSql[1].Statement.ToString(),
                            "Class table inheritance: second delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _deleteSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second delete statement in where clause.");
            Assert.AreEqual("DELETE FROM `Shape` WHERE `ShapeID` = ?Param0", _deleteSql[2].Statement.ToString(),
                            "Class table inheritance: third delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _deleteSql[2].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in third delete statement in where clause.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT `Circle`.`CircleID`, `FilledCircle`.`Colour`, `FilledCircle`.`FilledCircleID`, `Circle`.`Radius`, `Shape`.`ShapeID`, `Shape`.`ShapeName` FROM `FilledCircle`, `Circle`, `Shape` WHERE `Circle`.`CircleID` = `FilledCircle`.`CircleID` AND `Shape`.`ShapeID` = `Circle`.`ShapeID` AND `FilledCircleID` = ?Param0",
                _selectSql.Statement.ToString(), "Select sql is incorrect for class table inheritance.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _selectSql.Parameters[0]).Value,
                            "Parameter FilledCircleID is incorrect in select where clause for class table inheritance.");
        }


//		[Test]
//		public void TestLoadSql() {
//			Assert.AreEqual("SELECT * FROM FilledCircle, Circle, Shape WHERE Circle.CircleID = FilledCircle.CircleID AND Shape.ShapeID = Circle.ShapeID", FilledCircle.GetClassDef().SelectSql);
//		}

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

            shapes.LoadAll();
            Assert.AreEqual(2, shapes.Count);
            Assert.AreEqual("MyShape", shapes[0].ShapeName);
            Assert.AreEqual("Circle", shapes[1].ShapeName);

            circles.LoadAll();
            Assert.AreEqual(1, circles.Count);
            Assert.AreEqual(circles[0].ShapeID, shapes[1].ShapeID);
            Assert.AreEqual(5, circles[0].Radius);
            Assert.AreEqual("Circle", circles[0].ShapeName);

            FilledCircle filledCircle = new FilledCircle();
            filledCircle.Colour = 3;
            filledCircle.Radius = 7;
            filledCircle.ShapeName = "FilledCircle";
            filledCircle.Save();

            shapes.LoadAll();
            Assert.AreEqual(3, shapes.Count);
            Assert.AreEqual("MyShape", shapes[0].ShapeName);
            Assert.AreEqual("Circle", shapes[1].ShapeName);
            Assert.AreEqual("FilledCircle", shapes[2].ShapeName);

            circles.LoadAll();
            Assert.AreEqual(2, circles.Count);
            Assert.AreEqual(circles[1].ShapeID, shapes[2].ShapeID);
            Assert.AreEqual(7, circles[1].Radius);
            Assert.AreEqual("FilledCircle", circles[1].ShapeName);

            filledCircles.LoadAll();
            Assert.AreEqual(1, filledCircles.Count);
            Assert.AreEqual(filledCircles[0].ShapeID, shapes[2].ShapeID);
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

            shapes.LoadAll();
            Assert.AreEqual("MyShapeChanged", shapes[0].ShapeName);
            Assert.AreEqual("CircleChanged", shapes[1].ShapeName);
            Assert.AreEqual("FilledCircleChanged", shapes[2].ShapeName);
            circles.LoadAll();
            Assert.AreEqual(10, circles[0].Radius);
            Assert.AreEqual(12, circles[1].Radius);
            Assert.AreEqual("CircleChanged", circles[0].ShapeName);
            Assert.AreEqual("FilledCircleChanged", circles[1].ShapeName);
            filledCircles.LoadAll();
            Assert.AreEqual(4, filledCircles[0].Colour);
            Assert.AreEqual(12, filledCircles[0].Radius);
            Assert.AreEqual("FilledCircleChanged", filledCircles[0].ShapeName);

            // Test deleting
            shape.Delete();
            shape.Save();
            circle.Delete();
            circle.Save();
            filledCircle.Delete();
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
            Shape shape = BOLoader.Instance.GetBusinessObject<Shape>(
                "ShapeName = 'MyShape' OR ShapeName = 'MyShapeChanged'");
            if (shape != null)
            {
                shape.Delete();
                shape.Save();
            }

            Circle circle = BOLoader.Instance.GetBusinessObject<Circle>(
                "ShapeName = 'Circle' OR ShapeName = 'CircleChanged'");
            if (circle != null)
            {
                circle.Delete();
                circle.Save();
            }

            FilledCircle filledCircle = BOLoader.Instance.GetBusinessObject<FilledCircle>(
                "ShapeName = 'FilledCircle' OR ShapeName = 'FilledCircleChanged'");
            if (filledCircle != null)
            {
                filledCircle.Delete();
                filledCircle.Save();
            }
        }
    }
}
