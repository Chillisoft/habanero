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

namespace Habanero.Test.General
{
    /// <summary>
    /// Tests heirarchical inheritance where FilledCircle inherits from Circle using
    /// ClassTableInheritance and Circle inherits from Shape using SingleTableInheritance.
    /// In other words, two database tables exist: one that stores Circle and Shape
    /// and the other other that stores FilledCircle.  The implication is that Circle
    /// has no CircleID, meaning FilledCircle has a foreign key to ShapeID.
    /// </summary>
    [TestFixture, Ignore("This is a mixed hierarchy - still to do")]
    public class TestInheritanceHierarchySingleClass : TestInheritanceHierarchyBase
    {
        #region Setup/Teardown

        [TearDown]
        public void TearDown()
        {
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            shapes.LoadAll();
            foreach (Shape shape in shapes)
            {
                shape.MarkForDelete();
            }
            shapes.SaveAll();
            //Shape shape = BOLoader.Instance.GetBusinessObject<Shape>(
            //    "ShapeName = 'MyShape' OR ShapeName = 'MyShapeChanged'");
            //if (shape != null)
            //{
            //    shape.MarkForDelete();
            //    shape.Save();
            //}

            //CircleNoPrimaryKey circle = BOLoader.Instance.GetBusinessObject<CircleNoPrimaryKey>(
            //    "ShapeName = 'Circle' OR ShapeName = 'CircleChanged'");
            //if (circle != null)
            //{
            //    circle.MarkForDelete();
            //    circle.Save();
            //}

            //FilledCircleInheritsCircleNoPK filledCircle = BOLoader.Instance.GetBusinessObject<FilledCircleInheritsCircleNoPK>(
            //    "ShapeName = 'FilledCircle' OR ShapeName = 'FilledCircleChanged'");
            //if (filledCircle != null)
            //{
            //    filledCircle.MarkForDelete();
            //    filledCircle.Save();
            //}
        }

        #endregion

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTestForFilledCircleInheritsCircleNoPK();
        }

        protected override void SetupInheritanceSpecifics()
        {
            CircleNoPrimaryKey.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            CircleNoPrimaryKey.GetClassDef().SuperClassDef.Discriminator = "ShapeType_field";
            FilledCircleInheritsCircleNoPK.GetClassDef().SuperClassDef =
                new SuperClassDef(CircleNoPrimaryKey.GetClassDef(), ORMapping.ClassTableInheritance);
        }

        protected override void SetStrID()
        {
            _filledCircleId = (string) DatabaseUtil.PrepareValue(_filledCircle.GetPropertyValue("FilledCircleID"));
        }


        //[Test]
        //public void TestLoadSql() {
        //    Assert.AreEqual("SELECT * FROM FilledCircle, Circle, Shape WHERE Circle.CircleID = FilledCircle.CircleID AND Shape.ShapeID = Circle.ShapeID", FilledCircle.GetClassDef().SelectSql);
        //}

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(2, _deleteSql.Count,
                            "There should be 2 delete sql statements.");
            Assert.AreEqual("DELETE FROM `FilledCircle_table` WHERE `FilledCircleID_field` = ?Param0",
                            _deleteSql[0].Statement.ToString(),
                            "First delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, (_deleteSql[0].Parameters[0]).Value,
                            "Parameter FilledCircleID has incorrect value.");
            Assert.AreEqual("DELETE FROM `Shape_table` WHERE `ShapeID_field` = ?Param0",
                            _deleteSql[1].Statement.ToString(),
                            "Second delete sql statement is incorrect.");
            Assert.AreEqual(_filledCircleId, (_deleteSql[1].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value.");
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
            Assert.AreEqual(
                "INSERT INTO `Shape_table` (`ShapeType_field`, `Radius`, `ShapeID_field`, `ShapeName`) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                _insertSql[0].Statement.ToString(),
                "First insert Sql statement is incorrect.");
            Assert.AreEqual("FilledCircleInheritsCircleNoPK", (_insertSql[0].Parameters[0]).Value,
                            "Discriminator ('ShapeType') has incorrect value.");
            Assert.AreEqual(10, (_insertSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value.");
            Assert.AreEqual(_filledCircleId, (_insertSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value.");
            Assert.AreEqual("MyFilledCircle", (_insertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value.");

            Assert.AreEqual(
                "INSERT INTO `FilledCircle_table` (`Colour`, `FilledCircleID_field`, `ShapeID_field`) VALUES (?Param0, ?Param1, ?Param2)",
                _insertSql[1].Statement.ToString(), "Sql statement is incorrect.");
            Assert.AreEqual(3, (_insertSql[1].Parameters[0]).Value,
                            "Parameter Colour has incorrect value.");
            Assert.AreEqual(_filledCircleId, (_insertSql[1].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value.");
            Assert.AreEqual(_filledCircleId, (_insertSql[1].Parameters[2]).Value,
                            "Parameter ShapeID  has incorrect value.");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(2, _updateSql.Count,
                            "There should be 2 update sql statements.");

            Assert.AreEqual(
                "UPDATE `Shape_table` SET `Radius` = ?Param0, `ShapeID_field` = ?Param1, `ShapeName` = ?Param2 WHERE `ShapeID_field` = ?Param3",
                _updateSql[0].Statement.ToString(),
                "Update sql statement is incorrect.");
            Assert.AreEqual(10, (_updateSql[0].Parameters[0]).Value,
                            "Parameter Radius has incorrect value.");
            Assert.AreEqual(_filledCircleId, (_updateSql[0].Parameters[1]).Value,
                            "Parameter ShapeID has incorrect value.");
            Assert.AreEqual("MyFilledCircle", (_updateSql[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value.");
            Assert.AreEqual(_filledCircleId, (_updateSql[0].Parameters[3]).Value,
                            "Parameter ShapeID in where clause has incorrect value.");

            Assert.AreEqual("UPDATE `FilledCircle_table` SET `Colour` = ?Param0 WHERE `FilledCircleID_field` = ?Param1",
                            _updateSql[1].Statement.ToString(),
                            "Update sql statement is incorrect.");
            Assert.AreEqual(3, (_updateSql[1].Parameters[0]).Value,
                            "Parameter Colour has incorrect value.");
            Assert.AreEqual(_filledCircleId, (_updateSql[1].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value.");
        }

        [Test]
        public void TestDeleteShapes()
        {
            //-------------Setup Test Pack ------------------
            Shape shape = CreateSavedShape();
            CircleNoPrimaryKey circle = CreateSavedCircle();
            FilledCircleInheritsCircleNoPK filledCircle = CreateSavedFilledCircle();
            //-------------Execute test ---------------------
            shape.MarkForDelete();
            shape.Save();
            circle.MarkForDelete();
            circle.Save();
            filledCircle.MarkForDelete();
            filledCircle.Save();
            //-------------Test Result ----------------------
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles =
                new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            shapes.LoadAll();
            circles.LoadAll();
            filledCircles.LoadAll();
            Assert.AreEqual(0, shapes.Count);
            Assert.AreEqual(0, circles.Count);
            Assert.AreEqual(0, filledCircles.Count);
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
        public void TestFilledCircleIsUsingCorrectInheritance()
        {
            Assert.AreEqual(ORMapping.SingleTableInheritance, CircleNoPrimaryKey.GetClassDef().SuperClassDef.ORMapping);
            Assert.AreEqual(ORMapping.ClassTableInheritance,
                            FilledCircleInheritsCircleNoPK.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestLoadCreatedShapes_ShapeAndCircleAndFilledCircle()
        {
            //-------------Setup Test Pack ------------------
            CreateSavedShape();
            CreateSavedCircle();
            CreateSavedFilledCircle();

            //-------------Execute test ---------------------
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            shapes.LoadAll("ShapeName");
            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            circles.LoadAll("ShapeName");
            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles =
                new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            filledCircles.LoadAll();

            //-------------Test Result ----------------------
            Assert.AreEqual(3, shapes.Count);

            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("FilledCircle", shapes[1].ShapeName);
            Assert.AreEqual("MyShape", shapes[2].ShapeName);

            Shape circleShape = shapes[0];
            Shape filledCircleShape = shapes[1];
            Shape myShape = shapes[2];

            Assert.AreEqual(2, circles.Count);
            Assert.AreEqual("FilledCircle", circles[1].ShapeName);
            CircleNoPrimaryKey filledCircleCircle = circles[1];
            Assert.AreEqual(filledCircleShape.ShapeID, filledCircleCircle.ShapeID);
            Assert.AreEqual(7, filledCircleCircle.Radius);

            Assert.AreEqual(1, filledCircles.Count);
            Assert.AreEqual(filledCircles[0].ShapeID, filledCircleShape.ShapeID);
            FilledCircleInheritsCircleNoPK filledCircle = filledCircles[0];
            Assert.AreEqual(7, filledCircle.Radius);
            Assert.AreEqual("FilledCircle", filledCircle.ShapeName);
            Assert.AreEqual(3, filledCircle.Colour);
        }

        [Test]
        public void TestLoadCreatedShapes_ShapeAndCircleOnly()
        {
            //-------------Setup Test Pack ------------------
            Shape shape = CreateSavedShape();
            CircleNoPrimaryKey circle = CreateSavedCircle();

            //-------------Execute test ---------------------
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            shapes.LoadAll("ShapeName");
            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            circles.LoadAll();
            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles =
                new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            filledCircles.LoadAll();

            //-------------Test Result ----------------------
            Assert.AreEqual(2, shapes.Count);
            Assert.AreEqual("Circle", shapes[0].ShapeName);
            Assert.AreEqual("MyShape", shapes[1].ShapeName);

            Assert.AreEqual(1, circles.Count);
            Assert.AreEqual(circles[0].ShapeID, shapes[0].ShapeID);
            Assert.AreEqual(5, circles[0].Radius);
            Assert.AreEqual("Circle", circles[0].ShapeName);

            Assert.AreEqual(0, filledCircles.Count);
        }

        [Test]
        public void TestLoadCreatedShapes_ShapeOnly()
        {
            //-------------Setup Test Pack ------------------
            Shape shape = CreateSavedShape();

            //-------------Execute test ---------------------
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            shapes.LoadAll();
            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            circles.LoadAll();
            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles =
                new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            filledCircles.LoadAll();

            //-------------Test Result ----------------------
            Assert.AreEqual(1, shapes.Count);
            Assert.AreEqual(0, circles.Count);
            Assert.AreEqual(0, filledCircles.Count);
        }

        [Test]
        public void TestLoadThenUpdateThenLoadAgain()
        {
            //-------------Setup Test Pack ------------------
            Shape shape = CreateSavedShape();
            CircleNoPrimaryKey circle = CreateSavedCircle();
            FilledCircleInheritsCircleNoPK filledCircle = CreateSavedFilledCircle();
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            //shapes.LoadAll();
            //Assert.AreEqual(3, shapes.Count);
            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            circles.LoadAll();
            Assert.AreEqual(2, circles.Count);
            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles =
                new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            //-------------Execute test ---------------------

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
            filledCircles.LoadAll();
            Assert.AreEqual(10, circles[0].Radius);
            Assert.AreEqual(12, circles[1].Radius);
            Assert.AreEqual("CircleChanged", circles[0].ShapeName);
            Assert.AreEqual("FilledCircleChanged", circles[1].ShapeName);
            Assert.AreEqual(4, filledCircles[0].Colour);
            Assert.AreEqual(12, filledCircles[0].Radius);
            Assert.AreEqual("FilledCircleChanged", filledCircles[0].ShapeName);
        }

        [Test]
        public void TestLoadUpdatedShapes()
        {
            //-------------Setup Test Pack ------------------
            Shape shape = CreateSavedShape();
            CircleNoPrimaryKey circle = CreateSavedCircle();
            FilledCircleInheritsCircleNoPK filledCircle = CreateSavedFilledCircle();
            shape.ShapeName = "MyShapeChanged";
            shape.Save();
            circle.ShapeName = "CircleChanged";
            circle.Radius = 10;
            circle.Save();
            filledCircle.ShapeName = "FilledCircleChanged";
            filledCircle.Radius = 12;
            filledCircle.Colour = 4;
            filledCircle.Save();

            //-------------Execute test ---------------------
            BusinessObjectCollection<Shape> shapes = new BusinessObjectCollection<Shape>();
            BusinessObjectCollection<CircleNoPrimaryKey> circles = new BusinessObjectCollection<CircleNoPrimaryKey>();
            BusinessObjectCollection<FilledCircleInheritsCircleNoPK> filledCircles =
                new BusinessObjectCollection<FilledCircleInheritsCircleNoPK>();
            shapes.LoadAll("ShapeName");
            circles.LoadAll("ShapeName");
            filledCircles.LoadAll("ShapeName");

            //-------------Test Result ----------------------
            Assert.AreEqual(3, shapes.Count);
            Assert.AreEqual("CircleChanged", shapes[0].ShapeName);
            Assert.AreEqual("FilledCircleChanged", shapes[1].ShapeName);
            Assert.AreEqual("MyShapeChanged", shapes[2].ShapeName);

            Assert.AreEqual(2, circles.Count);

            Assert.AreEqual(10, circles[0].Radius);
            Assert.AreEqual(12, circles[1].Radius);
            Assert.AreEqual("CircleChanged", circles[0].ShapeName);
            Assert.AreEqual("FilledCircleChanged", circles[1].ShapeName);

            Assert.AreEqual(1, filledCircles.Count);
            Assert.AreEqual(4, filledCircles[0].Colour);
            Assert.AreEqual(12, filledCircles[0].Radius);
            Assert.AreEqual("FilledCircleChanged", filledCircles[0].ShapeName);
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
        public void TestUpdateWhenOnlyOneLevelUpdates()
        {
            IMock connectionControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection connection = (IDatabaseConnection) connectionControl.MockInstance;
            FilledCircleInheritsCircleNoPK myCircle = new FilledCircleInheritsCircleNoPK();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(myCircle);
            committer.CommitTransaction();
            myCircle.SetPropertyValue("Colour", 4);

            SqlStatementCollection myUpdateSql =
                new UpdateStatementGenerator(myCircle, DatabaseConnection.CurrentConnection).Generate();
            Assert.AreEqual(1, myUpdateSql.Count);
            connectionControl.Verify();
        }
    }
}