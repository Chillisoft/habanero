using System.Data;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.DB;
using Habanero.Base;
using Habanero.Util;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestInheritanceHeirarchyClassTable.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchyClassTable : TestInheritanceHeirarchyBase
    {
        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
            FilledCircle.GetClassDef().SuperClassDef =
                new SuperClassDef(Circle.GetClassDef(), ORMapping.ClassTableInheritance);
        }

        protected override void SetStrID()
        {
            itsFilledCircleId = (string) DatabaseUtil.PrepareValue(itsFilledCircle.GetPropertyValue("FilledCircleID"));
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
                Assert.IsTrue(itsFilledCircle.ID.Contains("FilledCircleID"));
                Assert.AreEqual(1, itsFilledCircle.ID.Count,
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
            itsFilledCircle.GetPropertyValue("ShapeName");
            itsFilledCircle.GetPropertyValue("Radius");
            itsFilledCircle.GetPropertyValue("CircleID");
            itsFilledCircle.GetPropertyValue("ShapeID");
            itsFilledCircle.GetPropertyValue("FilledCircleID");
            itsFilledCircle.GetPropertyValue("Colour");
        }


        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(3, itsInsertSql.Count,
                            "There should be 3 insert sql statements when using class table inheritance");
            Assert.AreEqual("INSERT INTO Shape (ShapeID, ShapeName) VALUES (?Param0, ?Param1)",
                            itsInsertSql[0].Statement.ToString(),
                            "Class Table inheritance: First insert Sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in first insert statement using class table inheritance");

            Assert.AreEqual("INSERT INTO Circle (CircleID, Radius, ShapeID) VALUES (?Param0, ?Param1, ?Param2)",
                            itsInsertSql[1].Statement.ToString(),
                            "Class Table inheritance: Second Sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[1].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[1].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in second insert statement using class table inheritance.");

            Assert.AreEqual(
                "INSERT INTO FilledCircle (CircleID, Colour, FilledCircleID) VALUES (?Param0, ?Param1, ?Param2)",
                itsInsertSql[2].Statement.ToString(), "Class Table inheritance: Third Sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[2].Parameters[2]).Value,
                            "Parameter FilledCircleID  has incorrect value in third insert statement using class table inheritance.");
            Assert.AreEqual(3, ((IDbDataParameter) itsInsertSql[2].Parameters[1]).Value,
                            "Parameter Colour has incorrect value in third insert statement using class table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[2].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in third insert statement using class table inheritance.");
        }

        [Test]
        public void TestSuperClassKey()
        {
            BOKey msuperKey = BOPrimaryKey.GetSuperClassKey(FilledCircle.GetClassDef(), itsFilledCircle);
            Assert.IsTrue(msuperKey.Contains("CircleID"), "Super class key should contain the CircleID property");
            Assert.AreEqual(1, msuperKey.Count, "Super class key should only have one prop");
            Assert.AreEqual(msuperKey["CircleID"].PropertyValue,
                            itsFilledCircle.ID["FilledCircleID"].PropertyValue,
                            "CircleID and FilledCircleID should be the same");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(3, itsUpdateSql.Count,
                            "There should be 3 update sql statements when using class table inheritance");

            Assert.AreEqual("UPDATE Circle SET CircleID = ?Param0, Radius = ?Param1 WHERE CircleID = ?Param2",
                            itsUpdateSql[0].Statement.ToString(),
                            "Class table inheritance: first update sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter CircleID in where clause has incorrect value in first update statement using class table inheritance");

            Assert.AreEqual("UPDATE Shape SET ShapeID = ?Param0, ShapeName = ?Param1 WHERE ShapeID = ?Param2",
                            itsUpdateSql[1].Statement.ToString(),
                            "Class table inheritance: second update sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsUpdateSql[1].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[1].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[1].Parameters[2]).Value,
                            "Parameter ShapeID in where clause has incorrect value in second update statement using class table inheritance");

            Assert.AreEqual("UPDATE FilledCircle SET Colour = ?Param0 WHERE FilledCircleID = ?Param1",
                            itsUpdateSql[2].Statement.ToString(),
                            "Class table inheritance: third update sql statement is incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) itsUpdateSql[2].Parameters[0]).Value,
                            "Parameter Colour has incorrect value in third update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[2].Parameters[1]).Value,
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
            connectionControl.ExpectAndReturn("ExecuteSql", 3, new object[] {null, null});

            FilledCircle myCircle = FilledCircle.GetNewObject();
            myCircle.SetDatabaseConnection(connection);
            myCircle.ApplyEdit();
            myCircle.SetPropertyValue("Colour", 4);

            SqlStatementCollection myUpdateSql = myCircle.GetUpdateSql();
            Assert.AreEqual(1, myUpdateSql.Count);
            connectionControl.Verify();
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(3, itsDeleteSql.Count,
                            "There should be 3 delete sql statements when using class table inheritance.");
            Assert.AreEqual("DELETE FROM FilledCircle WHERE FilledCircleID = ?Param0",
                            itsDeleteSql[0].Statement.ToString(),
                            "Class table inheritance: first delete sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first delete statement in where clause.");
            Assert.AreEqual("DELETE FROM Circle WHERE CircleID = ?Param0", itsDeleteSql[1].Statement.ToString(),
                            "Class table inheritance: second delete sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second delete statement in where clause.");
            Assert.AreEqual("DELETE FROM Shape WHERE ShapeID = ?Param0", itsDeleteSql[2].Statement.ToString(),
                            "Class table inheritance: third delete sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[2].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in third delete statement in where clause.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT Circle.CircleID, FilledCircle.Colour, FilledCircle.FilledCircleID, Circle.Radius, Shape.ShapeID, Shape.ShapeName FROM FilledCircle, Circle, Shape WHERE Circle.CircleID = FilledCircle.CircleID AND Shape.ShapeID = Circle.ShapeID AND FilledCircleID = ?Param0",
                itsSelectSql.Statement.ToString(), "Select sql is incorrect for class table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsSelectSql.Parameters[0]).Value,
                            "Parameter FilledCircleID is incorrect in select where clause for class table inheritance.");
        }


//		[Test]
//		public void TestLoadSql() {
//			Assert.AreEqual("SELECT * FROM FilledCircle, Circle, Shape WHERE Circle.CircleID = FilledCircle.CircleID AND Shape.ShapeID = Circle.ShapeID", FilledCircle.GetClassDef().SelectSql);
//		}
    }
}
