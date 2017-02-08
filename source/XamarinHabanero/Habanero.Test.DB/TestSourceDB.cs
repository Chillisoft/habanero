#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections.Generic;
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source));
            //-------------Test Result ----------------------
            Assert.AreEqual(tableName + " a1", sql);
        }

        [Test]
        public void TestCreateSQL_Simple_WithDelimiter()
        {
            //-------------Setup Test Pack ------------------
            const string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            SourceDB sourceDB = new SourceDB(source);
            //-------------Execute test ---------------------
            string sql = sourceDB.CreateSQL(new SqlFormatter("[", "]", "TOP", ""), CreateAliases(source));
            //-------------Test Result ----------------------
            Assert.AreEqual(string.Format("[{0}] a1", tableName), sql);
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            string expectedSql = string.Format("({0} a1 JOIN {1} a2 ON a1.{2} = a2.{3})", source.EntityName, joinSource.EntityName,
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

            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource));
            //-------------Test Result ----------------------
            StringAssert.Contains("JOIN", sql);
            Assert.IsFalse(sql.Contains("LEFT JOIN"));

        }

        private IDictionary<string, string> CreateAliases(params Source[] sources)
        {
            Dictionary<string, string> aliases = new Dictionary<string, string>();
            for (int i = 0; i < sources.Length; i++)
            {
                aliases.Add(sources[i].ToString(), "a" + (i+1));
            }
            return aliases;
        }

        private SqlFormatter GetSqlFormatter()
        {
            return new SqlFormatter("", "", "", "");
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

            string sql = leftJoinSourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource));
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource));
            //-------------Test Result ----------------------

            string expectedSql = string.Format("({0} a1 JOIN {1} a2 ON a1.{2} = a2.{3} AND a1.{4} = a2.{5})", 
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
            string sql = sourceDB.CreateSQL(myFormatter, CreateAliases(source, joinSource));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            string expectedSql = string.Format("([{0}] a1 JOIN [{1}] a2 ON a1.[{2}] = a2.[{3}])", source.EntityName, joinSource.EntityName,
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
                sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource));
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource, joinSource2));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            Source.Join.JoinField joinField2 = join2.JoinFields[0];
            string expectedSql = string.Format("(({0} a1 JOIN {1} a2 ON a1.{2} = a2.{3}) JOIN {4} a3 ON a2.{5} = a3.{6})",
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource, joinSource2));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = source.Joins[0].JoinFields[0];
            Source.Join.JoinField joinField2 = source.Joins[1].JoinFields[0];
            string expectedSql = string.Format("(({0} a1 JOIN {1} a2 ON a1.{2} = a2.{3}) JOIN {4} a3 ON a1.{5} = a3.{6})",
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource, branch1, branch2));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            Source.Join.JoinField joinFieldBranch1 = branchJoin1.JoinFields[0];
            Source.Join.JoinField joinFieldBranch2 = branchJoin2.JoinFields[0];
            string expectedSql = string.Format("((({0} a1 JOIN {1} a2 ON a1.{4} = a2.{5}) JOIN {2} a3 ON a2.{6} = a3.{7}) JOIN {3} a4 ON a2.{8} = a4.{9})",
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            string expectedSql = string.Format("({0} a1 JOIN {1} a2 ON a1.{2} = a2.{3})", source.EntityName, joinSource.EntityName,
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
            string sql = sourceDB.CreateSQL(GetSqlFormatter(), CreateAliases(source, joinSource, joinSource2));
            //-------------Test Result ----------------------
            Source.Join.JoinField joinField = join.JoinFields[0];
            Source.Join.JoinField joinField2 = join2.JoinFields[0];
            string expectedSql = string.Format("(({0} a1 JOIN {1} a2 ON a1.{2} = a2.{3}) JOIN {4} a3 ON a2.{5} = a3.{6})",
                                               sourceDB.EntityName,
                                               joinSource.EntityName, joinField.FromField.FieldName, joinField.ToField.FieldName,
                                               joinSource2.EntityName, joinField2.FromField.FieldName, joinField2.ToField.FieldName);
            Assert.AreEqual(expectedSql, sql);
        }

        [Test]
        public void Test_CreateSQL_ShouldUseAliasesInJoins()
        {
            //---------------Set up test pack-------------------
            var mysource = new Source("mysource");
            QueryField fieldOnMySource = new QueryField("testfield", "testfield", mysource);
            Source joinedTableSource = new Source("myjoinedtosource");
            QueryField fieldOnJoinedTableSource = new QueryField("testfield", "testfield", joinedTableSource);
            mysource.Joins.Add(new Source.Join(mysource, joinedTableSource));
            mysource.Joins[0].JoinFields.Add(new Source.Join.JoinField(fieldOnMySource, fieldOnJoinedTableSource));
            SourceDB sourceDB = new SourceDB(mysource);
            SqlFormatter sqlFormatter = new SqlFormatter("[", "]", "", "LIMIT");
            IDictionary<string, string> aliases = new Dictionary<string, string>() { { mysource.ToString(), "a1" }, { joinedTableSource.ToString(), "a2"} };
            //---------------Execute Test ----------------------
            string sql = sourceDB.CreateSQL(sqlFormatter, aliases);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(
                "([mysource] a1 JOIN [myjoinedtosource] a2 on a1.[testfield] = a2.[testfield])", sql); 
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