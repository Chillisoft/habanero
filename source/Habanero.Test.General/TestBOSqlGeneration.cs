//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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