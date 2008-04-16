//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleDecimal
    {
        [Test]
        public void TestDecimalRule()
        {
            PropRuleDecimal rule = new PropRuleDecimal("num", "TestMessage", 5.32m, 10.11111m);

            string errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.isPropValueValid("Propname", 5.31116m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max
            Assert.IsFalse(rule.isPropValueValid("Propname", 10.1111112m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            rule = new PropRuleDecimal("num", "TestMessage", 5.32m, 10.11111m);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.isPropValueValid("Propname", -53444.33222m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
    }
}
