//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
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
