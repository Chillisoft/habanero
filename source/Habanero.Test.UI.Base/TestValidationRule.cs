using System;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestValidationRule
    {
        [Test]
        public void TestConstructedCorrectly()
        {
            //---------------Set up test pack-------------------
            ValidationRule testRule = new ValidationRule();

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(testRule.IsCaseSensitive);
            Assert.AreEqual(ValidationDataType.String,testRule.DataType);
            Assert.AreSame(String.Empty,testRule.InitialValue);
            Assert.IsTrue(testRule.IsValid);
            Assert.IsFalse(testRule.IsRequired);
        }

        [Test]
        public void TestDataTypes()
        {
            //---------------Set up test pack-------------------
            String myString = "Hello World";
            int myInt = 9;


            //---------------Execute Test ----------------------
            bool result1 = ValidationUtil.CompareTypes(myString, ValidationDataType.String);
            bool result2 = ValidationUtil.CompareTypes(Convert.ToString(myInt), ValidationDataType.String);
            bool falseResult = ValidationUtil.CompareTypes(myString, ValidationDataType.Integer);

            //---------------Test Result -----------------------
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(falseResult);
        }

        [Test]
        public void TestCompareStrings()
        {
            //---------------Set up test pack-------------------
            string firstString = "FirstString";
            string sameAsFirstString = "FirstString";
            string myString = "Hello World";
            ValidationRule testRule = new ValidationRule();
            testRule.Operator = ValidationCompareOperator.Equal;
            testRule.DataType = ValidationDataType.String;

            //---------Assert Preconditions----------------------
            Assert.IsTrue(String.Equals(firstString, sameAsFirstString));

            //---------------Execute Test ----------------------
            bool result = ValidationUtil.CompareValues(firstString, sameAsFirstString, testRule.Operator, testRule.DataType);
            bool falseResult = ValidationUtil.CompareValues(firstString, myString, testRule.Operator, testRule.DataType);

            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.IsFalse(falseResult);
        }

        [Test]
        public void TestCompareIntegers()
        {
            //---------------Set up test pack-------------------
            int firstInt = 10;
            int sameAsFirstInt = 10;
            int myInt = 80;
            ValidationRule testRule = new ValidationRule();
            testRule.Operator = ValidationCompareOperator.Equal;
            testRule.DataType = ValidationDataType.Integer;
            //-------------Test Preconditions------------------
            Assert.AreEqual(firstInt,sameAsFirstInt);
            //---------------Execute Test ----------------------
            bool result = ValidationUtil.CompareValues(Convert.ToString(firstInt), Convert.ToString(sameAsFirstInt), testRule.Operator, testRule.DataType);
            bool falseResult = ValidationUtil.CompareValues(Convert.ToString(firstInt), Convert.ToString(myInt), testRule.Operator, testRule.DataType);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.IsFalse(falseResult);
        }

        //[Test]
        //public void TestRequiredField()
        //{
        //    //---------------Set up test pack-------------------
        //    int myInt = 5;
        //    ValidationRule testRule = new ValidationRule();
        //    testRule.Operator = ValidationCompareOperator.Equal;
        //    testRule.DataType = ValidationDataType.Integer;
        //    //----------Test Precondition-------------------
        //    //---------------Execute Test ----------------------
        //    bool result = ValidationUtil.CompareValues(Convert.ToString(myInt), "", testRule.Operator, testRule.DataType);

        //    //---------------Test Result -----------------------
        //    Assert.IsFalse(result);
        //}

        [Test]
        public void TestIsRequiredValdiation()
        {
            //---------------Set up test pack-------------------
            string myString = "Hello";
            ValidationRule testRule = new ValidationRule();
            testRule.IsRequired = true;

            //---------------Execute Test ----------------------
            bool result = ValidationUtil.CompareValues(myString, "", testRule.Operator, testRule.DataType);

            //---------------Test Result -----------------------
            Assert.IsTrue(result);

        }
    }
}
