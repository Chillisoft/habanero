using System;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestCriteriaParser //:TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        #endregion

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test]
        public void TestCreateOperator()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "=";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.Equals, op);
        }

        [Test]
        public void TestCreateOperator_GreaterThan()
        {
            //---------------Set up test pack-------------------
            const string operatorString = ">";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.GreaterThan, op);
        }

        [Test]
        public void TestCreateOperator_LessThan()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "<";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.LessThan, op);
        }

        [Test]
        public void TestCreateLogicalOperator()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "and";
            //---------------Execute Test ----------------------
            Criteria.LogicalOp op = CriteriaParser.CreateLogicalOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.LogicalOp.And, op);
        }

        [Test]
        public void TestCreateLogicalOperator_VariedCase()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "AnD";
            //---------------Execute Test ----------------------
            Criteria.LogicalOp op = CriteriaParser.CreateLogicalOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.LogicalOp.And, op);
        }

        [Test]
        public void TestCreateLogicalOperator_UnTrimmed()
        {
            //---------------Set up test pack-------------------
            const string operatorString = " and ";
            //---------------Execute Test ----------------------
            Criteria.LogicalOp op = CriteriaParser.CreateLogicalOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.LogicalOp.And, op);
        }

        [Test]
        public void TestCreateLogicalOperator_Or()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "or";
            //---------------Execute Test ----------------------
            Criteria.LogicalOp op = CriteriaParser.CreateLogicalOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.LogicalOp.Or, op);
        }

        [Test]
        public void TestCreateLogicalOperator_Or_VariedCase()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "oR";
            //---------------Execute Test ----------------------
            Criteria.LogicalOp op = CriteriaParser.CreateLogicalOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.LogicalOp.Or, op);
        }

        [Test]
        public void TestCreateLogicalOperator_Or_UnTrimmed()
        {
            //---------------Set up test pack-------------------
            const string operatorString = " or ";
            //---------------Execute Test ----------------------
            Criteria.LogicalOp op = CriteriaParser.CreateLogicalOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.LogicalOp.Or, op);
        }

        [Test]
        public void Test_CreateCriteria_Simple()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname = surnameValue");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname = 'surnameValue'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_PropNameDiff()
        {
            //---------------Set up test pack-------------------
            const string propName = "OtherPropName";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria(propName + " = surnameValue");
            string criteriaAsString = criteria.ToString();

            StringAssert.AreEqualIgnoringCase(propName + " = 'surnameValue'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_ValueDiff()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname = " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname = '" + surnameValue + "'", criteriaAsString);
        }
        [Test]
        public void Test_CreateCriteria_Simple_WithGreaterThan()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname > " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname > '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_TwoParts()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("FirstName = firstNameValue and Surname = surnameValue");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("(FirstName = 'firstNameValue') AND (Surname = 'surnameValue')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_TwoParts_WithOr()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("FirstName = firstNameValue or Surname = surnameValue");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("(FirstName = 'firstNameValue') OR (Surname = 'surnameValue')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_ThreeParts()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("FirstName = firstNameValue and MiddleName = middleNameValue and Surname = surnameValue");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("(FirstName = 'firstNameValue') AND ((MiddleName = 'middleNameValue') AND (Surname = 'surnameValue'))", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_ThreeParts_WithBrackets()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("(FirstName = firstNameValue and MiddleName = middleNameValue) and Surname = surnameValue");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("((FirstName = 'firstNameValue') AND (MiddleName = 'middleNameValue')) AND (Surname = 'surnameValue')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_FourParts()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("FirstName = firstNameValue and MiddleName = middleNameValue " +
                "and Surname = surnameValue and Nickname = nicknameValue");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("(FirstName = 'firstNameValue') AND ((MiddleName = 'middleNameValue') " +
                "AND ((Surname = 'surnameValue') AND (Nickname = 'nicknameValue')))", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_FourParts_WithBrackets()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("(FirstName = firstNameValue and MiddleName = middleNameValue) " +
                "or (Surname = surnameValue and Nickname = nicknameValue)");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("((FirstName = 'firstNameValue') AND (MiddleName = 'middleNameValue')) " +
                "OR ((Surname = 'surnameValue') AND (Nickname = 'nicknameValue'))", criteriaAsString);
        }

        //        [Test]
//        public void TestCreateSimpleExpression()
//        {
//            Criteria exp =
//                new Criteria(new Criteria("Field1", ">=", "value1"), Criteria.LogicalOp.And,
//                             new Criteria("Field2", ">=", "value2"));
//                Assert.AreEqual(exp.ExpressionString(), "(Field1 >= 'value1' AND Field2 >= 'value2')");
//        }

        //[Test]
        //public void TestCreateDatabaseExpressionNoTableName()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", ">=", "value1"), new SqlOperator("AND"),
        //                       new Parameter("Field2", ">=", "value2"));
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "[", "]");
        //    Assert.AreEqual("([Field1] >= ?Param0 AND [Field2] >= ?Param1)", st.Statement.ToString());
        //    Assert.AreEqual("value1", ((IDbDataParameter)st.Parameters[0]).Value);
        //    Assert.AreEqual("value2", ((IDbDataParameter)st.Parameters[1]).Value);
        //}

        //[Test]
        //public void TestCreateDatabaseExpressionWithTableName()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", "tbName", "DBField1", ">=", "value1"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tbName", "DBField2", ">=", "value2"));
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "[", "]");
        //    Assert.AreEqual("([tbName].[DBField1] >= ?Param0 OR [tbName].[DBField2] >= ?Param1)",
        //                    st.Statement.ToString());
        //    Assert.AreEqual("value1", ((IDbDataParameter)st.Parameters[0]).Value);
        //    Assert.AreEqual("value2", ((IDbDataParameter)st.Parameters[1]).Value);
        //}

        //[Test]
        //public void TestCreateDatabaseExpressionWithTableNameNoFieldSeparators()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", "tb", "DBField1", ">=", "value1"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tb", "DBField2", ">=", "value2"));
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("(tb.DBField1 >= ?Param0 OR tb.DBField2 >= ?Param1)", st.Statement.ToString());
        //    Assert.AreEqual("value1", ((IDbDataParameter)st.Parameters[0]).Value);
        //    Assert.AreEqual("value2", ((IDbDataParameter)st.Parameters[1]).Value);
        //}

        //[Test]
        //public void TestCreateDatabaseExpressionWithInvertedCommas()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", "tb", "DBField1", ">=", "value'1"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tb", "DBField2", ">=", "value2"));
        //    SqlStatement st1 = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st1, "", "");
        //    Assert.AreEqual("(tb.DBField1 >= ?Param0 OR tb.DBField2 >= ?Param1)", st1.Statement.ToString());
        //    Assert.AreEqual("value'1", ((IDbDataParameter)st1.Parameters[0]).Value);
        //    Assert.AreEqual("value2", ((IDbDataParameter)st1.Parameters[1]).Value);

        //    exp =
        //        new Expression(new Parameter("Field1", "tb", "DBField1", ">=", "value''1"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tb", "DBField2", ">=", "value2"));
        //    SqlStatement st2 = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st2, "", "");
        //    Assert.AreEqual("(tb.DBField1 >= ?Param0 OR tb.DBField2 >= ?Param1)", st2.Statement.ToString());
        //    Assert.AreEqual("value''1", ((IDbDataParameter)st2.Parameters[0]).Value);
        //    Assert.AreEqual("value2", ((IDbDataParameter)st2.Parameters[1]).Value);
        //}

        //[Test]
        //public void TestCreateDatabaseExpressionWithInOperator()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", "tb", "DBField1", "IN", "('a', 'zzz')"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tb", "DBField2", "in", "('12 mar 2004', '27 mar 2004')"));
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("(tb.DBField1 IN ('a', 'zzz') OR tb.DBField2 IN ('12 mar 2004', '27 mar 2004'))",
        //                    st.Statement.ToString());
        //}

        //[Test]
        //public void TestCreateDatabaseExpressionTree()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", "tb", "DBField1", "IN", "('a', 'zzz')"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tb", "DBField2", "in", "('12 mar 2004', '27 mar 2004')"));
        //    SqlStatement st1 = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st1, "", "");
        //    Assert.AreEqual("(tb.DBField1 IN ('a', 'zzz') OR tb.DBField2 IN ('12 mar 2004', '27 mar 2004'))",
        //                    st1.Statement.ToString());

        //    exp = new Expression(exp, new SqlOperator("And"), new Parameter("Field3", "=", "a"));
        //    SqlStatement st2 = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st2, "", "");
        //    Assert.AreEqual(
        //        "((tb.DBField1 IN ('a', 'zzz') OR tb.DBField2 IN ('12 mar 2004', '27 mar 2004')) AND Field3 = ?Param0)",
        //        st2.Statement.ToString());
        //    Assert.AreEqual("a", ((IDbDataParameter)st2.Parameters[0]).Value);
        //}

        //[Test]
        //public void TestCreateDatabaseIsNull()
        //{
        //    IExpression exp =
        //        new Expression(new Parameter("Field1", "tb", "DBField1", "Is", "Null"), new SqlOperator("OR"),
        //                       new Parameter("Field2", "tb", "DBField2", "is", "Not null"));
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("(tb.DBField1 IS NULL OR tb.DBField2 IS NOT NULL)", st.Statement.ToString());
        //}

        //[Test]
        //public void TestParameterParsing()
        //{
        //    IExpression exp = new Parameter("Field1 = 'test'");
        //    Assert.AreEqual("Field1 = 'test'", exp.ExpressionString());
        //}

        //// peter - this functionality is unnecessary i think.
        ////[Test]
        ////public void TestParameterParsing2()
        ////{
        ////    IExpression exp = new Parameter("Field1 = 'test''");
        ////    Assert.AreEqual("Field1 = 'test''", exp.ExpressionString());
        ////    exp = new Parameter("Field1 = ''te'st'''");
        ////    Assert.AreEqual("Field1 = ''te'st'''", exp.ExpressionString());
        ////}

        ////[Test]
        ////public void TestParameterParsingSql()
        ////{
        ////    IExpression exp = new Parameter("Field1 = ''te'st'''");
        ////    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        ////    exp.SqlExpressionString(st, "", "");
        ////    Assert.AreEqual("Field1 = ?Param0", st.Statement.ToString());
        ////    Assert.AreEqual("'te'st''", ((IDbDataParameter) st.Parameters[0]).Value);
        ////}

        //[Test]
        //public void TestExpressionParsingSql()
        //{
        //    IExpression exp = Expression.CreateExpression("Field1 = 'test' and Field2 = 'test2' or Field2 = 'test2'");
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("((Field1 = ?Param0 AND Field2 = ?Param1) OR Field2 = ?Param2)", st.Statement.ToString());
        //    Assert.AreEqual("test", ((IDbDataParameter)st.Parameters[0]).Value);
        //    Assert.AreEqual("test2", ((IDbDataParameter)st.Parameters[1]).Value);
        //    Assert.AreEqual("test2", ((IDbDataParameter)st.Parameters[2]).Value);
        //}

        //[Test]
        //public void TestExpressionParsingSqlSingleParameter()
        //{
        //    IExpression exp = Expression.CreateExpression("Field1 = 'test'");
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("Field1 = ?Param0", st.Statement.ToString());
        //    Assert.AreEqual("test", ((IDbDataParameter)st.Parameters[0]).Value);
        //}

        //[Test]
        //public void TestParameterSqlInfo()
        //{
        //    IParameterSqlInfo paramSql1 =
        //        new MockParameterSqlInfo("testfieldname", "paramName", ParameterType.String, "tbl");
        //    IExpression exp = Expression.CreateExpression("paramName = 'test'");
        //    exp.SetParameterSqlInfo(paramSql1);
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("tbl.testfieldname = ?Param0", st.Statement.ToString());
        //    Assert.AreEqual("test", ((IDbDataParameter)st.Parameters[0]).Value);
        //}

        //[Test]
        //public void TestParameterSqlInfoWithMoreThanOne()
        //{
        //    IParameterSqlInfo paramSql1 =
        //        new MockParameterSqlInfo("testfieldname", "paramName", ParameterType.String, "tbl");
        //    IParameterSqlInfo paramSql2 =
        //        new MockParameterSqlInfo("testfieldname2", "paramName2", ParameterType.Date, "tbl2");
        //    IExpression exp = Expression.CreateExpression("paramName = 'test' and paramName2 = '10 Feb 2003'");
        //    exp.SetParameterSqlInfo(paramSql2);
        //    exp.SetParameterSqlInfo(paramSql1);
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("(tbl.testfieldname = ?Param0 AND tbl2.testfieldname2 = ?Param1)", st.Statement.ToString());
        //    Assert.AreEqual("test", ((IDbDataParameter)st.Parameters[0]).Value);
        //    Assert.AreEqual(new DateTime(2003, 02, 10), ((IDbDataParameter)st.Parameters[1]).Value);
        //}

        //[Test]
        //public void TestOrInBracketsWithAndOutsideBrackets()
        //{
        //    IExpression exp = Expression.CreateExpression("param1 = 'test' AND (param2 = 3 or param2 = 4)");
        //    SqlStatement st = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    exp.SqlExpressionString(st, "", "");
        //    Assert.AreEqual("(param1 = ?Param0 AND (param2 = ?Param1 OR param2 = ?Param2))", st.Statement.ToString());
        //}

        //[Test]
        //public void TestAppendExpression()
        //{
        //    IExpression exp = Expression.CreateExpression("param1 = 'test' AND param2 = 3");
        //    IExpression result = Expression.AppendExpression(exp, new SqlOperator("or"), "param2 = 4");

        //    Assert.AreEqual("((param1 = 'test' AND param2 = '3') OR param2 = '4')",
        //                    result.ExpressionString());
        //}

        //[Test]
        //public void TestParameterConstructors()
        //{
        //    Parameter param = new Parameter("prop", "field", "=", "value");
        //    Assert.AreEqual("prop = 'value'", param.ExpressionString());
        //    Assert.AreEqual("[field]", param.FieldFullName("[", "]"));

        //    param = new Parameter("prop", "table", "field", "=", "value", ParameterType.String);
        //    Assert.AreEqual("prop = 'value'", param.ExpressionString());
        //    Assert.AreEqual("[table].[field]", param.FieldFullName("[", "]"));
        //}

        //[Test]
        //public void TestParameterTypes()
        //{
        //    Parameter param = new Parameter("prop", "table", "field", "=", "value", ParameterType.String);
        //    Assert.AreEqual("value", param.GetParameterValueAsObject());

        //    param = new Parameter("prop", "table", "field", "=", "true", ParameterType.Bool);
        //    Assert.AreEqual(true, param.GetParameterValueAsObject());

        //    param = new Parameter("prop", "table", "field", "=", "2007/2/1", ParameterType.Date);
        //    Assert.AreEqual(new DateTime(2007, 2, 1), param.GetParameterValueAsObject());

        //    param = new Parameter("prop", "table", "field", "=", "2.1", ParameterType.Number);
        //    Assert.AreEqual(2.1, param.GetParameterValueAsObject());
        //}

        //[Test]
        //public void TestGetSqlStringWithNoParameters()
        //{
        //    Parameter param = new Parameter("prop", "table", "field", "=", "value", ParameterType.String);
        //    Assert.AreEqual("", param.GetSqlStringWithNoParameters());
        //}

        //[Test]
        //public void TestSqlExpressionString()
        //{
        //    SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
        //    SqlOperator op = new SqlOperator("OR");
        //    op.SqlExpressionString(statement, "", "");
        //    op.SetParameterSqlInfo(null);  //for test coverage :)
        //}

        //public static void RunTest()
        //{
        //    TestExpression expTest = new TestExpression();
        //    expTest.TestExpressionParsingSql();
        //}

        //private class MockParameterSqlInfo : IParameterSqlInfo
        //{
        //    private String mFieldName;
        //    private String mParameterName;
        //    private ParameterType mParameterType;
        //    private string mTableName;

        //    public MockParameterSqlInfo(string fieldName, string parameterName, ParameterType parameterType,
        //                                string tableName)
        //    {
        //        this.mFieldName = fieldName;
        //        this.mParameterName = parameterName;
        //        this.mParameterType = parameterType;
        //        this.mTableName = tableName;
        //    }

        //    public ParameterType ParameterType
        //    {
        //        get { return this.mParameterType; }
        //    }

        //    public string FieldName
        //    {
        //        get { return this.mFieldName; }
        //    }

        //    public string ParameterName
        //    {
        //        get { return this.mParameterName; }
        //    }

        //    /// <summary>
        //    /// Table name to be added to parameter.
        //    /// </summary>
        //    public string TableName
        //    {
        //        get { return this.mTableName; }
        //    }
        //}
    }
}