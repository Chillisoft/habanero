using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.CriteriaManager
{
    [TestFixture]
    public class TestCriteriaValues
    {
        [Test]
        public void Test_ToString_WithIntegers()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { 100, 200, 300 };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            string valuesAsString = criteriaValues.ToString();

            //-------------Test Result ----------------------
            Assert.AreEqual("(100, 200, 300)", valuesAsString);
        }

        [Test]
        public void Test_ToString_WithStrings()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "100", "200", "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            string valuesAsString = criteriaValues.ToString();

            //-------------Test Result ----------------------
            Assert.AreEqual("(100, 200, 300)", valuesAsString);
        }

        [Test]
        public void Test_CompareTo_WhenInValues_ShouldReturnZero()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "100", "200", "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            int result = criteriaValues.CompareTo("100");

            //-------------Test Result ----------------------
            Assert.AreEqual(0, result);
        }
    
        [Test]
        public void Test_CompareTo_WhenNotInValues_ShouldReturnNotZero()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "100", "200", "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            int result = criteriaValues.CompareTo("10");

            //-------------Test Result ----------------------
            Assert.AreNotEqual(0, result);
        }
    
        [Test]
        public void Test_CompareTo_WhenNullInValues_ShouldReturnZero()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "100", null, "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            int result = criteriaValues.CompareTo(null);

            //-------------Test Result ----------------------
            Assert.AreEqual(0, result);
        }
    
        [Test]
        public void Test_CompareTo_WhenNullInValuesAndLookingForValue_ShouldReturnZero()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "100", null, "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            int result = criteriaValues.CompareTo("300");

            //-------------Test Result ----------------------
            Assert.AreEqual(0, result);
        }
    
        [Test]
        public void Test_CompareTo_WhenNullNotInValues_ShouldReturnNotZero()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "100", "200", "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //-------------Execute test ---------------------
            int result = criteriaValues.CompareTo(null);

            //-------------Test Result ----------------------
            Assert.AreNotEqual(0, result);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void Test_ConstructorRemovesQuotesAroundStrings()
        {
            //-------------Setup Test Pack ------------------
            object[] values = new object[] { "'100'", "'200'", "'300'" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            
            //-------------Execute test ---------------------
            int result = criteriaValues.CompareTo("100");
            //-------------Test Result ----------------------
            Assert.AreEqual(0, result);
        }
    

    }
}
