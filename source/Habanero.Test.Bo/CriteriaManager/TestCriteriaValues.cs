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
            Assert.AreEqual("('100', '200', '300')", valuesAsString);
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
