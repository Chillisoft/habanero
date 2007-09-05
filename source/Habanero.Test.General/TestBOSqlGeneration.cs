using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
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
            shape = new Shape();
            insertSql = shape.GetInsertSql();
        }

        [Test]
        public void TestInsertSql()
        {
            Assert.AreEqual(1, insertSql.Count, "There should only be one insert statement.");
            Assert.AreEqual("INSERT INTO Shape (ShapeID, ShapeName) VALUES (?Param0, ?Param1)",
                            insertSql[0].Statement.ToString(), "Insert Sql is being created incorrectly");
        }
    }
}