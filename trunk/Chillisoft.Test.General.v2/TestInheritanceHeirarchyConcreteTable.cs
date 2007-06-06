using System.Data;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Db.v2;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for TestInheritanceHeirarchyConcreteTable.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchyConcreteTable : TestInheritanceHeirarchyBase
    {
        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
            FilledCircle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
        }

        protected override void SetStrID()
        {
            itsFilledCircleId = (string) DatabaseUtil.PrepareValue(itsFilledCircle.GetPropertyValue("FilledCircleID"));
        }

        [Test]
        public void TestCircleIsUsingConcreteTableInheritance()
        {
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, Circle.GetClassDef().SuperClassDesc.ORMapping);
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, FilledCircle.GetClassDef().SuperClassDesc.ORMapping);
        }

        [Test]
        public void TestObjCircleHasCorrectProperties()
        {
            itsFilledCircle.GetPropertyValue("ShapeName");
            itsFilledCircle.GetPropertyValue("FilledCircleID");
            itsFilledCircle.GetPropertyValue("Radius");
            itsFilledCircle.GetPropertyValue("Colour");
        }


        [Test]
        public void TestCircleSelectSql()
        {
            Assert.AreEqual(
                "SELECT tbFilledCircle.Colour, tbFilledCircle.FilledCircleID, tbFilledCircle.Radius, tbFilledCircle.ShapeName FROM tbFilledCircle WHERE FilledCircleID = ?Param0",
                itsSelectSql.Statement.ToString(), "select statement is incorrect for Concrete Table inheritance");
        }

        [Test]
        public void TestCircleInsertSQL()
        {
            Assert.AreEqual(1, itsInsertSql.Count,
                            "There should only be one insert statement for concrete table inheritance.");
            Assert.AreEqual(
                "INSERT INTO tbFilledCircle (Colour, FilledCircleID, Radius, ShapeName) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                itsInsertSql[0].Statement.ToString(), "Concrete Table Inheritance insert SQL seems to be incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value");
            Assert.AreEqual(3, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsInsertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[0].Parameters[2]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSQL()
        {
            Assert.AreEqual(1, itsUpdateSql.Count,
                            "There should only be one update statement for concrete table inheritance.");
            Assert.AreEqual(
                "UPDATE tbFilledCircle SET Colour = ?Param0, Radius = ?Param1, ShapeName = ?Param2 WHERE FilledCircleID = ?Param3",
                itsUpdateSql[0].Statement.ToString(), "Concrete Table Inheritance update SQL seems to be incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[3]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSQL()
        {
            Assert.AreEqual(1, itsDeleteSql.Count,
                            "There should only be one delete statement for concrete table inheritance.");
            Assert.AreEqual("DELETE FROM tbFilledCircle WHERE FilledCircleID = ?Param0",
                            itsDeleteSql[0].Statement.ToString(),
                            "Concrete Table Inheritance delete SQL seems to be incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter FilledCircleID has incorrect value in Delete SQL statement for concrete table inheritance.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT tbFilledCircle.Colour, tbFilledCircle.FilledCircleID, tbFilledCircle.Radius, tbFilledCircle.ShapeName FROM tbFilledCircle WHERE FilledCircleID = ?Param0",
                itsSelectSql.Statement.ToString(), "Select sql is incorrect for concrete table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsSelectSql.Parameters[0]).Value,
                            "Parameter CircleID is incorrect in select where clause for concrete table inheritance.");
        }
    }
}
