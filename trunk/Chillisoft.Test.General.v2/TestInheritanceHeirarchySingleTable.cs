using System.Data;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Db.v2;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for TestInheritanceHeirarchySingleTable.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchySingleTable : TestInheritanceHeirarchyBase
    {
        public TestInheritanceHeirarchySingleTable()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            FilledCircle.GetClassDef().SuperClassDesc =
                new SuperClassDesc(Circle.GetClassDef(), ORMapping.SingleTableInheritance);
        }

        protected override void SetStrID()
        {
            itsFilledCircleId = (string) DatabaseUtil.PrepareValue(itsFilledCircle.GetPropertyValue("ShapeID"));
        }

        [Test]
        public void TestFilledCircleIsUsingSingleTableInheritance()
        {
            Assert.AreEqual(ORMapping.SingleTableInheritance, Circle.GetClassDef().SuperClassDesc.ORMapping);
            Assert.AreEqual(ORMapping.SingleTableInheritance, FilledCircle.GetClassDef().SuperClassDesc.ORMapping);
        }

        [Test]
        public void TestFilledCircleHasShapeIDAsPrimaryKey()
        {
            try
            {
                itsFilledCircle.PrimaryKey.Contains("ShapeID");
            }
            catch (CoreBizArgumentException)
            {
                Assert.Fail("An object using SingleTableInheritance should receive its superclass as its primary key.");
            }
        }

        [Test]
        public void TestFilledCircleHasCorrectPropertyNames()
        {
            itsFilledCircle.GetPropertyValue("ShapeName");
            itsFilledCircle.GetPropertyValue("Radius");
            itsFilledCircle.GetPropertyValue("ShapeID");
            itsFilledCircle.GetPropertyValue("Colour");
        }

        [Test, ExpectedException(typeof (CoreBizArgumentException))]
        public void TestFilledCircleDoesntHaveCircleID()
        {
            itsFilledCircle.GetPropertyValue("CircleID");
            itsFilledCircle.GetPropertyValue("FilledCircleID");
        }


        [Test]
        public void TestCircleSelectSql()
        {
            Assert.AreEqual("SELECT tbShape.Colour, tbShape.Radius, tbShape.ShapeID, tbShape.ShapeName FROM tbShape",
                            itsSelectSql.Statement.ToString().Substring(0, 86),
                            "select statement is incorrect for Single Table inheritance");
        }

        [Test]
        public void TestCircleInsertSQL()
        {
            Assert.AreEqual(1, itsInsertSql.Count,
                            "There should only be one insert Sql statement when using Single Table Inheritance.");
            Assert.AreEqual(
                "INSERT INTO tbShape (Colour, Radius, ShapeID, ShapeName) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                itsInsertSql[0].Statement.ToString(), "Concrete Table Inheritance insert SQL seems to be incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsInsertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(1, itsUpdateSql.Count,
                            "There should only be one update sql statement when using single table inheritance.");
            Assert.AreEqual(
                "UPDATE tbShape SET Colour = ?Param0, Radius = ?Param1, ShapeID = ?Param2, ShapeName = ?Param3 WHERE ShapeID = ?Param4",
                itsUpdateSql[0].Statement.ToString());
            Assert.AreEqual(3, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsUpdateSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[4]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(1, itsDeleteSql.Count,
                            "There should only be one delete sql statement when using single table inheritance.");
            Assert.AreEqual("DELETE FROM tbShape WHERE ShapeID = ?Param0", itsDeleteSql[0].Statement.ToString(),
                            "Delete SQL for single table inheritance is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value for delete sql when using Single Table inheritance.");
        }
    }
}
