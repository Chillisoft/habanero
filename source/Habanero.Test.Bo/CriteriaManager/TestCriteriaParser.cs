using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
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
            Criteria.Op op = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.Equals, op);
        }

        [Test]
        public void TestCreateOperator_GreaterThan()
        {
            //---------------Set up test pack-------------------
            const string operatorString = ">";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.GreaterThan, op);
        }

        [Test]
        public void TestCreateOperator_GreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            const string operatorString = ">=";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.GreaterThanEqual, op);
        }

        [Test]
        public void TestCreateOperator_LessThanEqual()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "<=";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.LessThanEqual, op);
        }

        [Test]
        public void TestCreateOperator_NotEquals()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "<>"; 

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.NotEquals, op);
        }

        [Test]
        public void Test_InvalidOperatorString()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "INVALID";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                CriteriaParser.CreateComparisonOperator(operatorString);
                Assert.Fail("Should fail and throw an error.");
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("Invalid operator ",ex.DeveloperMessage);
                StringAssert.Contains(operatorString,ex.DeveloperMessage);
            }
            //---------------Test Result -----------------------

        }

        [Test]
        public void TestCreateOperator_LessThan()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "<";

            //---------------Execute Test ----------------------
            Criteria.Op op = CriteriaParser.CreateComparisonOperator(operatorString);

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
            Assert.AreEqual(Criteria.Op.Equals, criteria.ComparisonOperator);
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
            Assert.AreEqual(Criteria.Op.GreaterThan, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname > '" + surnameValue + "'", criteriaAsString);
        }
        [Test]
        public void Test_CreateCriteria_Simple_WithLessThan()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname < " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.LessThan, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname < '" + surnameValue + "'", criteriaAsString);
        }
            
        [Test]
        public void Test_CreateCriteria_Simple_WithNotEquals()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname <> " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.NotEquals, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname <> '" + surnameValue + "'", criteriaAsString);
        }


        [Test]
        public void Test_CreateCriteria_Simple_WithLessThanEqual()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname <= " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.LessThanEqual,criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname <= '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithInvalid()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                Criteria criteria = CriteriaParser.CreateCriteria("Surname INALID " + surnameValue + "");
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("Invalid operator ", ex.DeveloperMessage);
            }
        }


        [Test]
        public void Test_CreateCriteria_Simple_WithGreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname >= " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.Op.GreaterThanEqual, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname >= '" + surnameValue + "'", criteriaAsString);
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


        [Test]
        public void Test_CreateCriteria_FourParts_WithBrackets_MultipleOperators()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("(FirstName < firstNameValue and MiddleName >= middleNameValue) " +
                "or (Surname > surnameValue and Nickname <> nicknameValue)");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("((FirstName < 'firstNameValue') AND (MiddleName >= 'middleNameValue')) " +
                "OR ((Surname > 'surnameValue') AND (Nickname <> 'nicknameValue'))", criteriaAsString);
        }

    }
}