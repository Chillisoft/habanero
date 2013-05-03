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
using Habanero.BO;
using NUnit.Framework;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleShort
    {
        [Test]
        public void Test_WithNegativeNumber_ShouldReturnErrorMessage()
        {
            string errorMessage = "";
            var rule = new PropRuleShort("num", "TestMessage", 5, 10);

            Assert.IsFalse(rule.IsPropValueValid("Propname", -5, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
        [Test]
        public void Test_ValueLTMin_ShouldReturnErrorMessage()
        {
            var rule = new PropRuleShort("num", "TestMessage", 5, 10);
            var errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.IsPropValueValid("Propname", 1, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
        [Test]
        public void Test_WithValidValue_ShouldReturnEmptyErrorMessage()
        {
            var rule = new PropRuleShort("num", "TestMessage", 5, 10);
            var errorMessage = "";

            Assert.IsTrue(rule.IsPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
        }
        [Test]
        public void Test_WithValueGTMax_ShouldReturnErrorMessage()
        {
            var rule = new PropRuleShort("num", "TestMessage", 5, 10);
            var errorMessage = "";

            Assert.IsFalse(rule.IsPropValueValid("Propname", 12, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }

        [Test]
        public void Test_WithNullValue_ShouldReturnEmptyErrorMessage()
        {
            var rule = new PropRuleShort("num", "TestMessage", 5, 10);
            var errorMessage = "";

            Assert.IsTrue(rule.IsPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
        }

        [Test]
        public void Test_MaxAndMinValueAreSetOnIComparableInterface()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPropRuleComparable<short> rule = new PropRuleShort("num", "TestMessage", 5, 10);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, rule.MinValue);
            Assert.AreEqual(10, rule.MaxValue);
        }

        [Test]
        public void ConstructWithoutMinOrMax_ShouldUseShortMinAndMax()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPropRuleComparable<short> rule = new PropRuleShort("num", "TestMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual(short.MinValue, rule.MinValue);
            Assert.AreEqual(short.MaxValue, rule.MaxValue);
        }
    }
}