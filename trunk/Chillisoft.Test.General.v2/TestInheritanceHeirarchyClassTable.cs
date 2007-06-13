using System.Data;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using NMock;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for TestInheritanceHeirarchyClassTable.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchyClassTable : TestInheritanceHeirarchyBase
    {
        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
            FilledCircle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Circle.GetClassDef(), ORMapping.ClassTableInheritance);
        }

        protected override void SetStrID()
        {
            itsFilledCircleId = (string) DatabaseUtil.PrepareValue(itsFilledCircle.GetPropertyValue("FilledCircleID"));
        }

        [Test]
        public void TestFilledCircleIsUsingClassTableInheritance()
        {
            Assert.AreEqual(ORMapping.ClassTableInheritance, Circle.GetClassDef().SuperClassDesc.ORMapping);
            Assert.AreEqual(ORMapping.ClassTableInheritance, FilledCircle.GetClassDef().SuperClassDesc.ORMapping);
        }

        [Test]
        public void TestCircleHasCircleIDAsPrimaryKey()
        {
            try
            {
                Assert.IsTrue(itsFilledCircle.PrimaryKey.Contains("FilledCircleID"));
                Assert.AreEqual(1, itsFilledCircle.PrimaryKey.Count,
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
        public void TestCircleInsertSQL()
        {
            Assert.AreEqual(3, itsInsertSql.Count,
                            "There should be 3 insert sql statements when using class table inheritance");
            Assert.AreEqual("INSERT INTO tbShape (ShapeID, ShapeName) VALUES (?Param0, ?Param1)",
                            itsInsertSql[0].Statement.ToString(),
                            "Class Table inheritance: First insert Sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in first insert statement using class table inheritance");

            Assert.AreEqual("INSERT INTO tbCircle (CircleID, Radius, ShapeID) VALUES (?Param0, ?Param1, ?Param2)",
                            itsInsertSql[1].Statement.ToString(),
                            "Class Table inheritance: Second Sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[1].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[1].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in second insert statement using class table inheritance.");

            Assert.AreEqual(
                "INSERT INTO tbFilledCircle (CircleID, Colour, FilledCircleID) VALUES (?Param0, ?Param1, ?Param2)",
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
                            itsFilledCircle.PrimaryKey["FilledCircleID"].PropertyValue,
                            "CircleID and FilledCircleID should be the same");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(3, itsUpdateSql.Count,
                            "There should be 3 update sql statements when using class table inheritance");

            Assert.AreEqual("UPDATE tbCircle SET CircleID = ?Param0, Radius = ?Param1 WHERE CircleID = ?Param2",
                            itsUpdateSql[0].Statement.ToString(),
                            "Class table inheritance: first update sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter CircleID in where clause has incorrect value in first update statement using class table inheritance");

            Assert.AreEqual("UPDATE tbShape SET ShapeID = ?Param0, ShapeName = ?Param1 WHERE ShapeID = ?Param2",
                            itsUpdateSql[1].Statement.ToString(),
                            "Class table inheritance: second update sql statement is incorrect.");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsUpdateSql[1].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[1].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[1].Parameters[2]).Value,
                            "Parameter ShapeID in where clause has incorrect value in second update statement using class table inheritance");

            Assert.AreEqual("UPDATE tbFilledCircle SET Colour = ?Param0 WHERE FilledCircleID = ?Param1",
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

            SqlStatementCollection myUpdateSql = myCircle.GetUpdateSQL();
            Assert.AreEqual(1, myUpdateSql.Count);
            connectionControl.Verify();
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(3, itsDeleteSql.Count,
                            "There should be 3 delete sql statements when using class table inheritance.");
            Assert.AreEqual("DELETE FROM tbFilledCircle WHERE FilledCircleID = ?Param0",
                            itsDeleteSql[0].Statement.ToString(),
                            "Class table inheritance: first delete sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first delete statement in where clause.");
            Assert.AreEqual("DELETE FROM tbCircle WHERE CircleID = ?Param0", itsDeleteSql[1].Statement.ToString(),
                            "Class table inheritance: second delete sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second delete statement in where clause.");
            Assert.AreEqual("DELETE FROM tbShape WHERE ShapeID = ?Param0", itsDeleteSql[2].Statement.ToString(),
                            "Class table inheritance: third delete sql statement is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[2].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in third delete statement in where clause.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT tbCircle.CircleID, tbFilledCircle.Colour, tbFilledCircle.FilledCircleID, tbCircle.Radius, tbShape.ShapeID, tbShape.ShapeName FROM tbFilledCircle, tbCircle, tbShape WHERE tbCircle.CircleID = tbFilledCircle.CircleID AND tbShape.ShapeID = tbCircle.ShapeID AND FilledCircleID = ?Param0",
                itsSelectSql.Statement.ToString(), "Select sql is incorrect for class table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsSelectSql.Parameters[0]).Value,
                            "Parameter FilledCircleID is incorrect in select where clause for class table inheritance.");
        }


//		[Test]
//		public void TestLoadSql() {
//			Assert.AreEqual("SELECT * FROM tbFilledCircle, tbCircle, tbShape WHERE tbCircle.CircleID = tbFilledCircle.CircleID AND tbShape.ShapeID = tbCircle.ShapeID", FilledCircle.GetClassDef().SelectSql);
//		}
    }
}
