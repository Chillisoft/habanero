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
using Habanero.Base;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using NUnit.Framework;
using IExpression=Habanero.Base.IExpression;
using IParameterSqlInfo=Habanero.Base.IParameterSqlInfo;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestExpression.
    /// </summary>
    [TestFixture]
    public class TestExpression : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            this.SetupDBConnection();
        }

        private static SqlStatement CreateSqlStatement()
        {
            return new SqlStatement(DatabaseConnection.CurrentConnection);
        }

        private string GetParamName(int index)
        {
            IParameterNameGenerator parameterNameGenerator = DatabaseConnection.CurrentConnection.CreateParameterNameGenerator();
            for (int i = 0; i < index; i++)
            {
                parameterNameGenerator.GetNextParameterName();
            }
            return parameterNameGenerator.GetNextParameterName();
        }

        [Test]
        public void TestCreateSimpleExpression()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", ">=", "value1"), new SqlOperator("AND"),
                               new Parameter("Field2", ">=", "value2"));
            Assert.AreEqual(exp.ExpressionString(), "(Field1 >= 'value1' AND Field2 >= 'value2')");
        }

        [Test]
        public void TestCreateDatabaseExpressionNoTableName()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", ">=", "value1"), new SqlOperator("AND"),
                               new Parameter("Field2", ">=", "value2"));
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "[", "]");

            

            Assert.AreEqual("([Field1] >= " + GetParamName(0) + " AND [Field2] >= " + GetParamName(1) + ")", st.Statement.ToString());
            Assert.AreEqual("value1", st.Parameters[0].Value);
            Assert.AreEqual("value2", st.Parameters[1].Value);
        }

        [Test]
        public void TestCreateDatabaseExpressionWithTableName()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", "tbName", "DBField1", ">=", "value1"), new SqlOperator("OR"),
                               new Parameter("Field2", "tbName", "DBField2", ">=", "value2"));
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "[", "]");
            Assert.AreEqual("([tbName].[DBField1] >= " + GetParamName(0) + " OR [tbName].[DBField2] >= " + GetParamName(1) + ")",
                            st.Statement.ToString());
            Assert.AreEqual("value1", st.Parameters[0].Value);
            Assert.AreEqual("value2", st.Parameters[1].Value);
        }

        [Test]
        public void TestCreateDatabaseExpressionWithTableNameNoFieldSeparators()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", "tb", "DBField1", ">=", "value1"), new SqlOperator("OR"),
                               new Parameter("Field2", "tb", "DBField2", ">=", "value2"));
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("(tb.DBField1 >= " + GetParamName(0) + " OR tb.DBField2 >= " + GetParamName(1) + ")", st.Statement.ToString());
            Assert.AreEqual("value1", st.Parameters[0].Value);
            Assert.AreEqual("value2", st.Parameters[1].Value);
        }

        [Test]
        public void TestCreateDatabaseExpressionWithInvertedCommas()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", "tb", "DBField1", ">=", "value'1"), new SqlOperator("OR"),
                               new Parameter("Field2", "tb", "DBField2", ">=", "value2"));
            SqlStatement st1 = CreateSqlStatement();
            exp.SqlExpressionString(st1, "", "");
            Assert.AreEqual("(tb.DBField1 >= " + GetParamName(0) + " OR tb.DBField2 >= " + GetParamName(1) + ")", st1.Statement.ToString());
            Assert.AreEqual("value'1", st1.Parameters[0].Value);
            Assert.AreEqual("value2", st1.Parameters[1].Value);

            exp =
                new Expression(new Parameter("Field1", "tb", "DBField1", ">=", "value''1"), new SqlOperator("OR"),
                               new Parameter("Field2", "tb", "DBField2", ">=", "value2"));
            SqlStatement st2 = CreateSqlStatement();
            exp.SqlExpressionString(st2, "", "");
            Assert.AreEqual("(tb.DBField1 >= " + GetParamName(0) + " OR tb.DBField2 >= " + GetParamName(1) + ")", st2.Statement.ToString());
            Assert.AreEqual("value''1", st2.Parameters[0].Value);
            Assert.AreEqual("value2", st2.Parameters[1].Value);
        }

        [Test]
        public void TestCreateDatabaseExpressionWithInOperator()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", "tb", "DBField1", "IN", "('a', 'zzz')"), new SqlOperator("OR"),
                               new Parameter("Field2", "tb", "DBField2", "in", "('12 mar 2004', '27 mar 2004')"));
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("(tb.DBField1 IN ('a', 'zzz') OR tb.DBField2 IN ('12 mar 2004', '27 mar 2004'))",
                            st.Statement.ToString());
        }

        [Test]
        public void TestCreateDatabaseExpressionTree()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", "tb", "DBField1", "IN", "('a', 'zzz')"), new SqlOperator("OR"),
                               new Parameter("Field2", "tb", "DBField2", "in", "('12 mar 2004', '27 mar 2004')"));
            SqlStatement st1 = CreateSqlStatement();
            exp.SqlExpressionString(st1, "", "");
            Assert.AreEqual("(tb.DBField1 IN ('a', 'zzz') OR tb.DBField2 IN ('12 mar 2004', '27 mar 2004'))",
                            st1.Statement.ToString());

            exp = new Expression(exp, new SqlOperator("And"), new Parameter("Field3", "=", "a"));
            SqlStatement st2 = CreateSqlStatement();
            exp.SqlExpressionString(st2, "", "");
            Assert.AreEqual(
                "((tb.DBField1 IN ('a', 'zzz') OR tb.DBField2 IN ('12 mar 2004', '27 mar 2004')) AND Field3 = " + GetParamName(0) + ")",
                st2.Statement.ToString());
            Assert.AreEqual("a", st2.Parameters[0].Value);
        }

        [Test]
        public void TestCreateDatabaseIsNull()
        {
            IExpression exp =
                new Expression(new Parameter("Field1", "tb", "DBField1", "Is", "Null"), new SqlOperator("OR"),
                               new Parameter("Field2", "tb", "DBField2", "is", "Not null"));
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("(tb.DBField1 IS NULL OR tb.DBField2 IS NOT NULL)", st.Statement.ToString());
        }

        [Test]
        public void TestParameterParsing()
        {
            IExpression exp = new Parameter("Field1 = 'test'");
            Assert.AreEqual("Field1 = 'test'", exp.ExpressionString());
        }

        // peter - this functionality is unnecessary i think.
        //[Test]
        //public void TestParameterParsing2()
        //{
        //    IExpression exp = new Parameter("Field1 = 'test''");
        //    Assert.AreEqual("Field1 = 'test''", exp.ExpressionString());
        //    exp = new Parameter("Field1 = ''te'st'''");
        //    Assert.AreEqual("Field1 = ''te'st'''", exp.ExpressionString());
        //}

        //[Test]
        //public void TestParameterParsingSql()
        //{
        //    IExpression exp = new Parameter("Field1 = ''te'st'''");
        //    SqlStatement st = CreateSqlStatement();
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("Field1 = " + GetParamName(0) + "", st.Statement.ToString());
        //    Assert.AreEqual("'te'st''", ((IDbDataParameter) st.Parameters[0]).Value);
        //}

        [Test]
        public void TestExpressionParsingSql()
        {
            IExpression exp = Expression.CreateExpression("Field1 = 'test' and Field2 = 'test2' or Field2 = 'test2'");
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("((Field1 = " + GetParamName(0) + " AND Field2 = " + GetParamName(1) + ") OR Field2 = " + GetParamName(2) + ")", st.Statement.ToString());
            Assert.AreEqual("test", st.Parameters[0].Value);
            Assert.AreEqual("test2", st.Parameters[1].Value);
            Assert.AreEqual("test2", st.Parameters[2].Value);
        }

        [Test]
        public void TestExpressionParsingSqlSingleParameter()
        {
            IExpression exp = Expression.CreateExpression("Field1 = 'test'");
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("Field1 = " + GetParamName(0) + "", st.Statement.ToString());
            Assert.AreEqual("test", st.Parameters[0].Value);
        }

        [Test]
        public void TestParameterSqlInfo()
        {
            IParameterSqlInfo paramSql1 =
                new MockParameterSqlInfo("testfieldname", "paramName", Habanero.Base.ParameterType.String, "tbl");
            IExpression exp = Expression.CreateExpression("paramName = 'test'");
            exp.SetParameterSqlInfo(paramSql1);
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("tbl.testfieldname = " + GetParamName(0) + "", st.Statement.ToString());
            Assert.AreEqual("test", st.Parameters[0].Value);
        }

        [Test]
        public void TestParameterSqlInfoWithMoreThanOne()
        {
            IParameterSqlInfo paramSql1 =
                new MockParameterSqlInfo("testfieldname", "paramName", ParameterType.String, "tbl");
            IParameterSqlInfo paramSql2 =
                new MockParameterSqlInfo("testfieldname2", "paramName2", ParameterType.Date, "tbl2");
            IExpression exp = Expression.CreateExpression("paramName = 'test' and paramName2 = '10 Feb 2003'");
            exp.SetParameterSqlInfo(paramSql2);
            exp.SetParameterSqlInfo(paramSql1);
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("(tbl.testfieldname = " + GetParamName(0) + " AND tbl2.testfieldname2 = " + GetParamName(1) + ")", st.Statement.ToString());
            Assert.AreEqual("test", st.Parameters[0].Value);
            Assert.AreEqual(new DateTime(2003, 02, 10), st.Parameters[1].Value);
        }

        [Test]
        public void TestOrInBracketsWithAndOutsideBrackets()
        {
            IExpression exp = Expression.CreateExpression("param1 = 'test' AND (param2 = 3 or param2 = 4)");
            SqlStatement st = CreateSqlStatement();
            exp.SqlExpressionString(st, "", "");
            Assert.AreEqual("(param1 = " + GetParamName(0) + " AND (param2 = " + GetParamName(1) + " OR param2 = " + GetParamName(2) + "))", st.Statement.ToString());
        }

        [Test]
        public void TestAppendExpression()
        {
            IExpression exp = Expression.CreateExpression("param1 = 'test' AND param2 = 3");
            IExpression result = Expression.AppendExpression(exp, new SqlOperator("or"), "param2 = 4");

            Assert.AreEqual("((param1 = 'test' AND param2 = '3') OR param2 = '4')",
                            result.ExpressionString());
        }

        [Test]
        public void TestParameterConstructors()
        {
            Parameter param = new Parameter("prop", "field", "=", "value");
            Assert.AreEqual("prop = 'value'", param.ExpressionString());
            Assert.AreEqual("[field]", param.FieldFullName("[", "]"));

            param = new Parameter("prop", "table", "field", "=", "value", ParameterType.String);
            Assert.AreEqual("prop = 'value'", param.ExpressionString());
            Assert.AreEqual("[table].[field]", param.FieldFullName("[", "]"));
        }

        [Test]
        public void TestParameterTypes()
        {
            Parameter param = new Parameter("prop", "table", "field", "=", "value", ParameterType.String);
            Assert.AreEqual("value", param.GetParameterValueAsObject());

            param = new Parameter("prop", "table", "field", "=", "true", ParameterType.Bool);
            Assert.AreEqual(true, param.GetParameterValueAsObject());

            param = new Parameter("prop", "table", "field", "=", "2007/2/1", ParameterType.Date);
            Assert.AreEqual(new DateTime(2007, 2, 1), param.GetParameterValueAsObject());

            param = new Parameter("prop", "table", "field", "=", "2.1", ParameterType.Number);
            Assert.AreEqual(2.1, param.GetParameterValueAsObject());
        }

        [Test]
        public void Test_GetParameterValueAsObject_WhenDateType_TodayString_ShouldReturnToday()
        {
            //---------------Set up test pack-------------------
            Parameter param = new Parameter("prop", "table", "field", "=", "today", ParameterType.Date);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime parameterValueAsObject = (DateTime)param.GetParameterValueAsObject();
            //---------------Test Result -----------------------
            Assert.IsNotNull(parameterValueAsObject);
            Assert.AreEqual(DateTimeToday.Value, parameterValueAsObject);
        }
        [Test]
        public void Test_GetParameterValueAsObject_WhenDateType_YesterdayString_ShouldReturnYesterday()
        {
            //---------------Set up test pack-------------------
            Parameter param = new Parameter("prop", "table", "field", "=", "Yesterday", ParameterType.Date);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime parameterValueAsObject = (DateTime)param.GetParameterValueAsObject();
            //---------------Test Result -----------------------
            Assert.IsNotNull(parameterValueAsObject);
            Assert.AreEqual(DateTimeToday.Value.AddDays(-1), parameterValueAsObject);
        }
        [Test]
        public void Test_GetParameterValueAsObject_WhenDateType_TomorrowString_ShouldReturnYesterday()
        {
            //---------------Set up test pack-------------------
            Parameter param = new Parameter("prop", "table", "field", "=", "tomorrow", ParameterType.Date);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime parameterValueAsObject = (DateTime)param.GetParameterValueAsObject();
            //---------------Test Result -----------------------
            Assert.IsNotNull(parameterValueAsObject);
            Assert.AreEqual(DateTimeToday.Value.AddDays(1), parameterValueAsObject);
        }
        [Test]
        public void TestGetSqlStringWithNoParameters()
        {
            Parameter param = new Parameter("prop", "table", "field", "=", "value", ParameterType.String);
            Assert.AreEqual("", param.GetSqlStringWithNoParameters());
        }

        [Test]
        public void TestSqlExpressionString()
        {
            SqlStatement statement = CreateSqlStatement();
            SqlOperator op = new SqlOperator("OR");
            op.SqlExpressionString(statement, "", "");
            op.SetParameterSqlInfo(null);  //for test coverage :)
        }

        public static void RunTest()
        {
            TestExpression expTest = new TestExpression();
            expTest.TestExpressionParsingSql();
        }

        private class MockParameterSqlInfo : IParameterSqlInfo
        {
            private readonly String mFieldName;
            private readonly String mParameterName;
            private readonly ParameterType mParameterType;
            private readonly string mTableName;

            public MockParameterSqlInfo(string fieldName, string parameterName, ParameterType parameterType,
                                        string tableName)
            {
                this.mFieldName = fieldName;
                this.mParameterName = parameterName;
                this.mParameterType = parameterType;
                this.mTableName = tableName;
            }

            public ParameterType ParameterType
            {
                get { return this.mParameterType; }
            }

            public string FieldName
            {
                get { return this.mFieldName; }
            }

            public string ParameterName
            {
                get { return this.mParameterName; }
            }

            /// <summary>
            /// Table name to be added to parameter.
            /// </summary>
            public string TableName
            {
                get { return this.mTableName; }
            }
        }
    }
}