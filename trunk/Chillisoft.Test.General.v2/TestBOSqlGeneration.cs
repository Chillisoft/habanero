using Chillisoft.Db.v2;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
{
    [TestFixture]
    public class TestBOSqlGeneration : TestUsingDatabase
    {
        private Shape shape;
        private SqlStatementCollection insertSql;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
            shape = Shape.GetNewObject();
            insertSql = shape.GetInsertSQL();
        }

        [Test]
        public void TestInsertSQL()
        {
            Assert.AreEqual(1, insertSql.Count, "There should only be one insert statement.");
            Assert.AreEqual("INSERT INTO Shape (ShapeID, ShapeName) VALUES (?Param0, ?Param1)",
                            insertSql[0].Statement.ToString(), "Insert SQL is being created incorrectly");
        }
    }
}