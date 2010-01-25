//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Equals, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_GreaterThan()
        {
            //---------------Set up test pack-------------------
            const string operatorString = ">";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.GreaterThan, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_GreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            const string operatorString = ">=";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.GreaterThanEqual, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_LessThanEqual()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "<=";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.LessThanEqual, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_NotEquals()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "<>"; 

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotEquals, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_Like()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "Like";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Like, comparisonOp);
        }
        [Test]
        public void TestCreateOperator_NotLike()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "Not Like";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotLike, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_Is()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "Is";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Is, comparisonOp);
        }
        [Test]
        public void TestCreateOperator_IS()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "IS";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Is, comparisonOp);
        }
        [Test]
        public void TestCreateOperator_is()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "is";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Is, comparisonOp);
        }

        [Test]
        public void TestCreateOperator_IsNot()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "Is Not";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.IsNot, comparisonOp);
        }
        [Test]
        public void TestCreateOperator_isnot()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "Is Not";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.IsNot, comparisonOp);
        }
        [Test]
        public void TestCreateOperator_ISNOT()
        {
            //---------------Set up test pack-------------------
            const string operatorString = "IS NOT";

            //---------------Execute Test ----------------------
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.IsNot, comparisonOp);
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
            Criteria.ComparisonOp comparisonOp = CriteriaParser.CreateComparisonOperator(operatorString);

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.LessThan, comparisonOp);
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
        public void Test_CreateCriteria_Invalid()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                CriteriaParser.CreateCriteria("surnameValue");
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The criteria string surnameValue is not a valid criteria string", ex.DeveloperMessage);
            }
            //---------------Test Result -----------------------
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
            Assert.AreEqual(Criteria.ComparisonOp.Equals, criteria.ComparisonOperator);
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
            Assert.AreEqual(Criteria.ComparisonOp.GreaterThan, criteria.ComparisonOperator);
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
            Assert.AreEqual(Criteria.ComparisonOp.LessThan, criteria.ComparisonOperator);
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
            Assert.AreEqual(Criteria.ComparisonOp.NotEquals, criteria.ComparisonOperator);
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
            Assert.AreEqual(Criteria.ComparisonOp.LessThanEqual,criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname <= '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithLike()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname Like " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Like, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname LIKE '" + surnameValue + "'", criteriaAsString);
        }
        [Test]
        public void Test_CreateCriteria_Simple_WithLike_UCase()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname LIKE " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Like, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname LIKE '" + surnameValue + "'", criteriaAsString);
        }
        [Test]
        public void Test_CreateCriteria_Simple_WithLike_LCase()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname like " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Like, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname LIKE '" + surnameValue + "'", criteriaAsString);
        }
        [Test]
        public void Test_CreateCriteria_Simple_WithNotLike()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname Not Like " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotLike, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname NOT LIKE '" + surnameValue + "'", criteriaAsString);
        }
        [Test]
        public void Test_CreateCriteria_Simple_WithNotLike_UCase()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname  NOT LIKE " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotLike, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname NOT LIKE '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithNotLike_LCase()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname   not like " + surnameValue + "");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotLike, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname NOT LIKE '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithIs()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname Is Null");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.Is, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname IS NULL", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithIsNot()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname Is Not Null");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.IsNot, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname IS NOT NULL", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithIn()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname in ('Bob')");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.In, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname IN ('Bob')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithNotIn()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname not in ('Bob')");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotIn, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname NOT IN ('Bob')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithIn_MultipleValues()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname in ('Bob', 'Name2', 'Name3')");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.In, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname in ('Bob', 'Name2', 'Name3')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithNotIn_MultipleValues()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname not in ('Bob', 'Name2', 'Name3')");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.NotIn, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname not in ('Bob', 'Name2', 'Name3')", criteriaAsString);
        }

        [Test]
        public void Test_CreateCriteria_Simple_WithIn_MultipleValues_CommaInsideValue()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname in ('Bob, Bob2', 'Name2', 'Name3')");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.In, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname in ('Bob, Bob2', 'Name2', 'Name3')", criteriaAsString);
        }


        [Test]
        public void Test_CreateCriteria_Simple_WithIn_MultipleValues_QuoteInsideValue()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = CriteriaParser.CreateCriteria("Surname in ('Bob, \' Bob2', 'Name2', 'Name3')");
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            Assert.AreEqual(Criteria.ComparisonOp.In, criteria.ComparisonOperator);
            StringAssert.AreEqualIgnoringCase("Surname in ('Bob, \' Bob2', 'Name2', 'Name3')", criteriaAsString);
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
                CriteriaParser.CreateCriteria("Surname INALID " + surnameValue + "");
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("is not a valid criteria string", ex.DeveloperMessage);
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
            Assert.AreEqual(Criteria.ComparisonOp.GreaterThanEqual, criteria.ComparisonOperator);
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