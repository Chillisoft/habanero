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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using Habanero.Test.BO;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Tests heirarchical inheritance where FilledCircle inherits from Circle using
    /// ClassTableInheritance and Circle inherits from Shape using SingleTableInheritance.
    /// In other words, two database tables exist: one that stores Circle and Shape
    /// and the other other that stores FilledCircle.  The implication is that Circle
    /// has no CircleID, meaning FilledCircle has a foreign key to ShapeID.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchySingleClass : TestInheritanceHeirarchyBase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTestForFilledCircleInheritsCircleNoPK();
        }

        protected override void SetupInheritanceSpecifics()
        {
            CircleNoPrimaryKey.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            CircleNoPrimaryKey.GetClassDef().SuperClassDef.Discriminator = "ShapeType";
            FilledCircleInheritsCircleNoPK.GetClassDef().SuperClassDef =
                new SuperClassDef(CircleNoPrimaryKey.GetClassDef(), ORMapping.ClassTableInheritance);
        }

        protected override void SetStrID()
        {
            _filledCircleId = (string) DatabaseUtil.PrepareValue(_filledCircle.GetPropertyValue("FilledCircleID"));
        }

        [Test]
        public void TestFilledCircleIsUsingCorrectInheritance()
        {
            Assert.AreEqual(ORMapping.SingleTableInheritance, CircleNoPrimaryKey.GetClassDef().SuperClassDef.ORMapping);
            Assert.AreEqual(ORMapping.ClassTableInheritance, FilledCircleInheritsCircleNoPK.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestFilledCircleHasFilledCircleIDAsPrimaryKey()
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
            _filledCircle.GetPropertyValue("ShapeID");
            _filledCircle.GetPropertyValue("FilledCircleID");
            _filledCircle.GetPropertyValue("Colour");
        }


        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(2, _insertSql.Count,
                            "There should be 2 insert sql statements.");
            Assert.AreEqual("INSERT INTO `Shape` (`ShapeType`, `Radius`, `ShapeID`, `ShapeName`) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                            _insertSql[0].Statement.ToString(),
                            "First insert Sql statement is incorrect.");
            Assert.AreEqual("FilledCircleInheritsCircleNoPK", ((IDbDataParameter)_insertSql[0].Parameters[0]).Value,
                            "Discriminator ('ShapeType') has incorrect value.");
            Assert.AreEqual(10, ((IDbDataParameter)_insertSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter)_insertSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter)_insertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value.");

            Assert.AreEqual(
                "INSERT INTO `FilledCircle` (`Colour`, `FilledCircleID`, `ShapeID`) VALUES (?Param0, ?Param1, ?Param2)",
                _insertSql[1].Statement.ToString(), "Sql statement is incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) _insertSql[1].Parameters[0]).Value,
                            "Parameter Colour has incorrect value.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _insertSql[1].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter)_insertSql[1].Parameters[2]).Value,
                            "Parameter ShapeID  has incorrect value.");
        }

        [Test]
        public void TestSuperClassKey()
        {
            BOKey msuperKey = BOPrimaryKey.GetSuperClassKey(FilledCircleInheritsCircleNoPK.GetClassDef(), _filledCircle);
            Assert.IsFalse(msuperKey.Contains("CircleID"), "Super class key should not contain the CircleID property");
            Assert.IsTrue(msuperKey.Contains("ShapeID"), "Super class key should contain the ShapeID property");
            Assert.AreEqual(1, msuperKey.Count, "Super class key should only have one prop");
            Assert.AreEqual(_filledCircle.Props["ShapeID"].Value, //msuperKey["ShapeID"].Value,
                            _filledCircle.ID["FilledCircleID"].Value,
                            "ShapeID and FilledCircleID should be the same");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(2, _updateSql.Count,
                            "There should be 2 update sql statements.");

            Assert.AreEqual("UPDATE `Shape` SET `Radius` = ?Param0, `ShapeID` = ?Param1, `ShapeName` = ?Param2 WHERE `ShapeID` = ?Param3",
                            _updateSql[0].Statement.ToString(),
                            "Update sql statement is incorrect.");
            Assert.AreEqual(10, ((IDbDataParameter)_updateSql[0].Parameters[0]).Value,
                            "Parameter Radius has incorrect value.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter)_updateSql[0].Parameters[1]).Value,
                            "Parameter ShapeID has incorrect value.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) _updateSql[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[0].Parameters[3]).Value,
                            "Parameter ShapeID in where clause has incorrect value.");

            Assert.AreEqual("UPDATE `FilledCircle` SET `Colour` = ?Param0 WHERE `FilledCircleID` = ?Param1",
                            _updateSql[1].Statement.ToString(),
                            "Update sql statement is incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) _updateSql[1].Parameters[0]).Value,
                            "Parameter Colour has incorrect value.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _updateSql[1].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value.");
        }

        [Test]
        public void TestUpdateWhenOnlyOneLevelUpdates()
        {
            IMock connectionControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection connection = (IDatabaseConnection) connectionControl.MockInstance;
            //connectionControl.ExpectAndReturn("LoadDataReader", null, new object[] {null});
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//            connectionControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            //connectionControl.ExpectAndReturn("ExecuteSql", 3, new object[] { null, null });

            FilledCircleInheritsCircleNoPK myCircle = new FilledCircleInheritsCircleNoPK();
            myCircle.SetDatabaseConnection(connection);
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
            Assert.AreEqual(2, _deleteSql.Count,
                            "There should be 2 delete sql statements.");
            Assert.AreEqual("DELETE FROM `FilledCircle` WHERE `FilledCircleID` = ?Param0",
                            _deleteSql[0].Statement.ToString(),
                            "First delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _deleteSql[0].Parameters[0]).Value,
                            "Parameter FilledCircleID has incorrect value.");
            Assert.AreEqual("DELETE FROM `Shape` WHERE `ShapeID` = ?Param0", _deleteSql[1].Statement.ToString(),
                            "Second delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _deleteSql[1].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT `FilledCircle`.`Colour`, `FilledCircle`.`FilledCircleID`, `Shape`.`Radius`, `Shape`.`ShapeID`, `Shape`.`ShapeName` FROM `FilledCircle`, `Shape` WHERE `Shape`.`ShapeID` = `FilledCircle`.`ShapeID` AND `FilledCircleID` = ?Param0",
                _selectSql.Statement.ToString(), "Select sql is incorrect for class table inheritance.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) _selectSql.Parameters[0]).Value,
                            "Parameter FilledCircleID is incorrect in select where clause for class table inheritance.");
        }


        //[Test]
        //public void TestLoadSql() {
        //    Assert.AreEqual("SELECT * FROM FilledCircle, Circle, Shape WHERE Circle.CircleID = FilledCircle.CircleID AND Shape.ShapeID = Circle.ShapeID", FilledCircle.GetClassDef().SelectSql);
        //}

        //TODO: Would like to separate these tests out later, but needs a structure
        // change and I'm out of time right now.
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

            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles = new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            filledCircles.LoadAll();
            Assert.AreEqual(0, filledCircles.Count);

            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
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

            FilledCircleInheritsCircleNoPK filledCircle = new FilledCircleInheritsCircleNoPK();
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

            CircleNoPrimaryKey circle = BOLoader.Instance.GetBusinessObject<CircleNoPrimaryKey>(
                "ShapeName = 'Circle' OR ShapeName = 'CircleChanged'");
            if (circle != null)
            {
                circle.Delete();
                circle.Save();
            }

            FilledCircleInheritsCircleNoPK filledCircle = BOLoader.Instance.GetBusinessObject<FilledCircleInheritsCircleNoPK>(
                "ShapeName = 'FilledCircle' OR ShapeName = 'FilledCircleChanged'");
            if (filledCircle != null)
            {
                filledCircle.Delete();
                filledCircle.Save();
            }
        }
    }
}
