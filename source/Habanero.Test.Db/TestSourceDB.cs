// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSourceDB
    {

        [Test]
        public void TestCreateSourceDB()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");

            //-------------Execute test ---------------------
            SourceDB sourceDB = new SourceDB(source);
            //-------------Test Result ----------------------
            Assert.AreEqual(source.Name, sourceDB.Name);
            Assert.AreEqual(source.EntityName, sourceDB.EntityName);
            Assert.AreEqual(source.Joins.Count, sourceDB.Joins.Count);
        }

        [Test]
        public void TestCreateSourceDB_WithJoins()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            Source.Join join = new Source.Join(source, joinSource);
            source.Joins.Add(join);

            //-------------Execute test ---------------------
            SourceDB sourceDB = new SourceDB(source);
            //-------------Test Result ----------------------
            Assert.AreEqual(source.Name, sourceDB.Name);
            Assert.AreEqual(source.EntityName, sourceDB.EntityName);
            Assert.AreEqual(source.Joins, sourceDB.Joins);
        }

        [Test]
        public void TestCreateSQL_Simple()
        {
            //-------------Setup Test Pack ------------------
            string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            SourceDB sourceDB = new SourceDB(source);
            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Assert.AreEqual(tableName, sql);
        }

        [Test]
        public void TestCreateSQL_Simple_WithDelimiter()
        {
            //-------------Setup Test Pack ------------------
            const string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            SourceDB sourceDB = new SourceDB(source);
            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL(new SqlFormatter("[", "]", "TOP", ""));
            //-------------Test Result ----------------------
            Assert.AreEqual(string.Format("[{0}]", tableName), sql);
        }

        [Test]
        public void TestCreateSQL_WithJoin()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");

            Source.Join join = CreateAndAddJoin(source, joinSource);

            SourceDB sourceDB = new SourceDB(source);
  
            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            string expectedSql = string.Format("({0} JOIN {1} ON {0}.{2} = {1}.{3})", source.EntityName, joinSource.EntityName,
                                               joinField.FromField.FieldName, joinField.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        [Test]
        public void TestCreateSQL_InnerJoin()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            Source.Join join = CreateAndAddJoin(source, joinSource);
            join.JoinType = Source.JoinType.InnerJoin;
            SourceDB sourceDB = new SourceDB(source);
            //-------------Execute test ---------------------

            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            StringAssert.Contains("JOIN", sql);
            Assert.IsFalse(sql.Contains("LEFT JOIN"));

        }

        [Test]
        public void TestCreateSQL_LeftJoin()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");

            Source.Join join = CreateAndAddJoin(source, joinSource);
            join.JoinType = Source.JoinType.LeftJoin;
            SourceDB leftJoinSourceDB = new SourceDB(source);
            //-------------Execute test ---------------------

            string sql = leftJoinSourceDB.CreateSQL();
            //-------------Test Result ----------------------
            StringAssert.Contains("LEFT JOIN", sql);
        }

        [Test]
        public void TestCreateSQL_WithJoin_TwoFields()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");

            Source.Join join = CreateAndAddJoin(source, joinSource);
            QueryField fromField = new QueryField("FromField2", "FROM_FIELD2", source);
            QueryField toField = new QueryField("ToField2", "TO_FIELD2", joinSource);
            join.JoinFields.Add(new Source.Join.JoinField(fromField, toField));
            Source.Join.JoinField joinField1 = join.JoinFields[0];
            Source.Join.JoinField joinField2 = join.JoinFields[1];

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------

            string expectedSql = string.Format("({0} JOIN {1} ON {0}.{2} = {1}.{3} AND {0}.{4} = {1}.{5})", 
                                               source.EntityName, joinSource.EntityName, 
                                               joinField1.FromField.FieldName, joinField1.ToField.FieldName,
                                               joinField2.FromField.FieldName, joinField2.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }


        [Test]
        public void TestCreateSQL_WithJoin_WithDelimiter()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");

            Source.Join join = CreateAndAddJoin(source, joinSource);

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            SqlFormatter myFormatter = new SqlFormatter("[", "]", "TOP", "");
            string sql = sourceDB.CreateSQL(myFormatter);
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            string expectedSql = string.Format("([{0}] JOIN [{1}] ON [{0}].[{2}] = [{1}].[{3}])", source.EntityName, joinSource.EntityName,
                                               joinField.FromField.FieldName, joinField.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }
        


        [Test]
        public void TestCreateSQL_WithJoin_NoFields()
        {
            //-------------Setup Test Pack ------------------
            string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            string joinTableName = "MY_JOINED_TABLE";
            Source joinSource = new Source("JoinSource", joinTableName);
            Source.Join join = new Source.Join(source, joinSource);
            source.Joins.Add(join);
            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            Exception exception = null;
            try
            {
                sourceDB.CreateSQL();
            } catch( Exception ex)
            {
                exception = ex;
            }
            //-------------Test Result ----------------------
            Assert.IsNotNull(exception, "An error was expected when creating SQL with joins that have no fields");
            Assert.IsInstanceOf(typeof(HabaneroDeveloperException), exception);
            string expectedMessage = string.Format("SQL cannot be created for the source '{0}' because it has a join to '{1}' without join fields", 
                                                   sourceDB.Name, join.ToSource.Name);
            StringAssert.Contains(expectedMessage, exception.Message);
        }

        [Test]
        public void TestCreateSQL_WithJoin_TwoLevels()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            Source joinSource2 = new Source("JoinSource2", "MY_JOINED_TABLE_2");

            Source.Join join = CreateAndAddJoin(source, joinSource);
            Source.Join join2 = CreateAndAddJoin(joinSource, joinSource2);

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            Source.Join.JoinField joinField2 = join2.JoinFields[0];
            string expectedSql = string.Format("(({0} JOIN {1} ON {0}.{2} = {1}.{3}) JOIN {4} ON {1}.{5} = {4}.{6})",
                                               sourceDB.EntityName,
                                               joinSource.EntityName, joinField.FromField.FieldName, joinField.ToField.FieldName,
                                               joinSource2.EntityName, joinField2.FromField.FieldName, joinField2.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        [Test]
        public void TestCreateSQL_WithJoin_TwoBranches()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            Source joinSource2 = new Source("JoinSource2", "MY_JOINED_TABLE_2");

            CreateAndAddJoin(source, joinSource);
            CreateAndAddJoin(source, joinSource2);

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = source.Joins[0].JoinFields[0];
            Source.Join.JoinField joinField2 = source.Joins[1].JoinFields[0];
            string expectedSql = string.Format("(({0} JOIN {1} ON {0}.{2} = {1}.{3}) JOIN {4} ON {0}.{5} = {4}.{6})",
                                               sourceDB.EntityName,
                                               joinSource.EntityName, joinField.FromField.FieldName, joinField.ToField.FieldName,
                                               joinSource2.EntityName, joinField2.FromField.FieldName, joinField2.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        [Test]
        public void TestCreateSQL_WithJoin_SecondLevel_TwoBranches()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            Source branch1 = new Source("JoinBranch1", "MY_BRANCH_1");
            Source branch2 = new Source("JoinBranch2", "MY_BRANCH_2");

            Source.Join join = CreateAndAddJoin(source, joinSource);
            Source.Join branchJoin1 = CreateAndAddJoin(joinSource, branch1);
            Source.Join branchJoin2 = CreateAndAddJoin(joinSource, branch2);

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            Source.Join.JoinField joinFieldBranch1 = branchJoin1.JoinFields[0];
            Source.Join.JoinField joinFieldBranch2 = branchJoin2.JoinFields[0];
            string expectedSql = string.Format("((({0} JOIN {1} ON {0}.{4} = {1}.{5}) JOIN {2} ON {1}.{6} = {2}.{7}) JOIN {3} ON {1}.{8} = {3}.{9})",
                                               sourceDB.EntityName, joinSource.EntityName, branch1.EntityName, branch2.EntityName,
                                               joinField.FromField.FieldName, joinField.ToField.FieldName,
                                               joinFieldBranch1.FromField.FieldName, joinFieldBranch1.ToField.FieldName, 
                                               joinFieldBranch2.FromField.FieldName, joinFieldBranch2.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        [Test]
        public void TestCreateSQL_WithInheritanceJoin()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");

            Source.Join join = CreateAndAddInheritanceJoin(source, joinSource);

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            string expectedSql = string.Format("({0} JOIN {1} ON {0}.{2} = {1}.{3})", source.EntityName, joinSource.EntityName,
                                               joinField.FromField.FieldName, joinField.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        [Test]
        public void TestCreateSQL_WithInheritanceJoin_TwoLevels()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_SOURCE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            Source joinSource2 = new Source("JoinSource2", "MY_JOINED_TABLE_2");

            Source.Join join = CreateAndAddInheritanceJoin(source, joinSource);
            Source.Join join2 = CreateAndAddInheritanceJoin(joinSource, joinSource2);

            SourceDB sourceDB = new SourceDB(source);

            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL();
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            Source.Join.JoinField joinField2 = join2.JoinFields[0];
            string expectedSql = string.Format("(({0} JOIN {1} ON {0}.{2} = {1}.{3}) JOIN {4} ON {1}.{5} = {4}.{6})",
                                               sourceDB.EntityName,
                                               joinSource.EntityName, joinField.FromField.FieldName, joinField.ToField.FieldName,
                                               joinSource2.EntityName, joinField2.FromField.FieldName, joinField2.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        private static Source.Join CreateAndAddJoin(Source fromSource, Source toSource)
        {
            Source.Join join = CreateJoin(fromSource, toSource);
            fromSource.Joins.Add(join);
            return join;
        }

        private static Source.Join CreateAndAddInheritanceJoin(Source fromSource, Source toSource)
        {
            Source.Join join = CreateJoin(fromSource, toSource);
            fromSource.InheritanceJoins.Add(join);
            return join;
        }

        private static Source.Join CreateJoin(Source fromSource, Source toSource)
        {
            Source.Join join = new Source.Join(fromSource, toSource);
            QueryField fromField = new QueryField("FromField", "FROM_FIELD", fromSource);
            QueryField toField = new QueryField("ToField", "TO_FIELD", toSource);
            join.JoinFields.Add(new Source.Join.JoinField(fromField, toField));
            return join;
        }
    }
}