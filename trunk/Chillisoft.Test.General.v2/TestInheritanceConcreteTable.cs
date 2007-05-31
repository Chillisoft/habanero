using System.Data;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Db.v2;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
{
    [TestFixture]
    public class TestInheritanceConcreteTable : TestInheritanceBase
    {
        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
        }

        protected override void SetStrID()
        {
            strID = (string) DatabaseUtil.PrepareValue(objCircle.GetPropertyValue("CircleID"));
        }

        [Test]
        public void TestCircleIsUsingConcreteTableInheritance()
        {
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, Circle.GetClassDef().SuperClassDesc.ORMapping);
        }

        [Test]
        public void TestObjCircleHasCorrectProperties()
        {
            objCircle.GetPropertyValue("ShapeName");
            objCircle.GetPropertyValue("CircleID");
            objCircle.GetPropertyValue("Radius");
        }

        [Test]
        public void TestCircleInsertSQL()
        {
            Assert.AreEqual(1, itsInsertSql.Count,
                            "There should only be one insert statement for concrete table inheritance.");
            Assert.AreEqual("INSERT INTO tbCircle (Radius, CircleID, ShapeName) VALUES (?Param0, ?Param1, ?Param2)",
                            itsInsertSql[0].Statement.ToString(),
                            "Concrete Table Inheritance insert SQL seems to be incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter CircleID has incorrect value");
            Assert.AreEqual("MyShape", ((IDbDataParameter) itsInsertSql[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSQL()
        {
            Assert.AreEqual(1, itsUpdateSql.Count,
                            "There should only be one update statement for concrete table inheritance.");
            Assert.AreEqual("UPDATE tbCircle SET Radius = ?Param0, ShapeName = ?Param1 WHERE CircleID = ?Param2",
                            itsUpdateSql[0].Statement.ToString(),
                            "Concrete Table Inheritance update SQL seems to be incorrect.");
            Assert.AreEqual(10, ((IDbDataParameter)itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual("MyShape", ((IDbDataParameter)itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter ShapeName incorrect value");
            Assert.AreEqual(strID, ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter CircleID in where clause has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSQL()
        {
            Assert.AreEqual(1, itsDeleteSql.Count,
                            "There should only be one delete statement for concrete table inheritance.");
            Assert.AreEqual("DELETE FROM tbCircle WHERE CircleID = ?Param0", itsDeleteSql[0].Statement.ToString(),
                            "Concrete Table Inheritance delete SQL seems to be incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in Delete SQL statement for concrete table inheritance.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT tbCircle.Radius, tbCircle.CircleID, tbCircle.ShapeName FROM tbCircle WHERE CircleID = ?Param0",
                selectSql.Statement.ToString(), "Select sql is incorrect for concrete table inheritance.");
            Assert.AreEqual(strID, ((IDbDataParameter) selectSql.Parameters[0]).Value,
                            "Parameter CircleID is incorrect in select where clause for concrete table inheritance.");
        }
    }
}