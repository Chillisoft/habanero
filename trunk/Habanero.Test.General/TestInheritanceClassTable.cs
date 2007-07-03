using System.Data;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestInheritanceClassTable : TestInheritanceBase
    {
        public static void RunTest()
        {
            TestInheritanceClassTable test = new TestInheritanceClassTable();
            test.SetupTest();
            test.TestSuperClassKey();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTest();
        }

        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
        }

        protected override void SetStrID()
        {
            strID = (string) DatabaseUtil.PrepareValue(objCircle.GetPropertyValue("CircleID"));
        }

        [Test]
        public void TestCircleIsUsingClassTableInheritance()
        {
            Assert.AreEqual(ORMapping.ClassTableInheritance, Circle.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestCircleIsNotDirty()
        {
            Circle circle = Circle.GetNewObject();
            Assert.IsFalse(circle.IsDirty);
        }

        [Test]
        public void TestCircleHasCircleIDAsPrimaryKey()
        {
            try
            {
                Assert.IsTrue(objCircle.ID.Contains("CircleID"));
                Assert.AreEqual(1, objCircle.ID.Count,
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
            objCircle.GetPropertyValue("ShapeName");
            objCircle.GetPropertyValue("Radius");
            objCircle.GetPropertyValue("CircleID");
            objCircle.GetPropertyValue("ShapeID");
        }

        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(2, itsInsertSql.Count,
                            "There should be 2 insert sql statements when using class table inheritance");
            Assert.AreEqual("INSERT INTO Shape (ShapeID, ShapeName) VALUES (?Param0, ?Param1)",
                            itsInsertSql[0].Statement.ToString(),
                            "Class Table inheritance: First insert Sql statement is incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual("MyShape", ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in first insert statement using class table inheritance");
            Assert.AreEqual("INSERT INTO Circle (CircleID, Radius, ShapeID) VALUES (?Param0, ?Param1, ?Param2)",
                            itsInsertSql[1].Statement.ToString(),
                            "Class Table inheritance: Second Sql statement is incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsInsertSql[1].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsInsertSql[1].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value in second insert statement using class table inheritance.");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[1].Parameters[1]).Value,
                            "Parameter Radius has incorrect value in second insert statement using class table inheritance.");
        }

        [Test]
        public void TestSuperClassKey()
        {
            BOKey msuperKey = BOPrimaryKey.GetSuperClassKey(Circle.GetClassDef(), objCircle);
            Assert.IsTrue(msuperKey.Contains("ShapeID"), "Super class key should contain the ShapeID property");
            Assert.AreEqual(1, msuperKey.Count, "Super class key should only have one prop");
            Assert.AreEqual(msuperKey["ShapeID"].PropertyValue, objCircle.ID["CircleID"].PropertyValue,
                            "ShapeID and CircleID should be the same");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(2, itsUpdateSql.Count,
                            "There should be 2 update sql statements when using class table inheritance");
            Assert.AreEqual("UPDATE Shape SET ShapeID = ?Param0, ShapeName = ?Param1 WHERE ShapeID = ?Param2",
                            itsUpdateSql[0].Statement.ToString(),
                            "Class table inheritance: first update sql statement is incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual("MyShape", ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter ShapeName has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual(strID, ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter ShapeID in where clause has incorrect value in first update statement using class table inheritance");
            Assert.AreEqual("UPDATE Circle SET Radius = ?Param0 WHERE CircleID = ?Param1",
                            itsUpdateSql[1].Statement.ToString(),
                            "Class table inheritance: second update sql statement is incorrect.");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[1].Parameters[0]).Value,
                            "Parameter Radius has incorrect value in second update statement using class table inheritance");
            Assert.AreEqual(strID, ((IDbDataParameter) itsUpdateSql[1].Parameters[1]).Value,
                            "Parameter CircleID has incorrect value in second update statement using class table inheritance");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(2, itsDeleteSql.Count,
                            "There should be 2 delete sql statements when using class table inheritance.");
            Assert.AreEqual("DELETE FROM Circle WHERE CircleID = ?Param0", itsDeleteSql[0].Statement.ToString(),
                            "Class table inheritance: first delete sql statement is incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in first delete statement in where clause.");
            Assert.AreEqual("DELETE FROM Shape WHERE ShapeID = ?Param0", itsDeleteSql[1].Statement.ToString(),
                            "Class table inheritance: second delete sql statement is incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsDeleteSql[1].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value in second delete statement in where clause.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT Circle.CircleID, Circle.Radius, Shape.ShapeID, Shape.ShapeName FROM Circle, Shape WHERE Shape.ShapeID = Circle.ShapeID AND CircleID = ?Param0",
                selectSql.Statement.ToString(), "Select sql is incorrect for class table inheritance.");
            Assert.AreEqual(strID, ((IDbDataParameter) selectSql.Parameters[0]).Value,
                            "Parameter CircleID is incorrect in select where clause for class table inheritance.");
        }
    }
}
