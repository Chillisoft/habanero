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
    public class TestPropRuleString
    {
        [Test]
        public void TestStringRule()
        {
            PropRuleString rule = new PropRuleString("Surname", "Test", 2, 50, null);

            string errorMessage = "";

            //Test less than max length
            Assert.IsFalse(rule.isPropValueValid("Propname", "", ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("Propname", "fdfsdafasdfsdf", ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            Assert.IsFalse(
                rule.isPropValueValid("Propname", "MySurnameIsTooLongByFarThisWill Cause and Error in Bus object", ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test lengths and not compulsory

            rule = new PropRuleString("Surname", "Test", 10, 20, null);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.isPropValueValid("Propname", "", ref errorMessage)); //test zero length strings
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";

            //Test that it ignores negative max length
            rule = new PropRuleString("Surname", "Test", -10, -1, null);
            Assert.IsTrue(rule.isPropValueValid("Propname", "", ref errorMessage)); //test zero length strings
            Assert.IsFalse(errorMessage.Length > 0);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("Propname", "ffff", ref errorMessage)); //test zero length strings
            Assert.IsFalse(errorMessage.Length > 0);
            errorMessage = "";

            Assert.IsFalse(rule.isPropValueValid("Propname", 11, ref errorMessage)); //test zero length strings
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";
        }

        [Test]
        public void TestStringRulePatternMatch()
        {
            //Pattern match no numeric characters allowed
            string errorMessage = "";
            PropRuleString rule = new PropRuleString("Surname", "Test", 10, 20, @"^[a-zA-Z\- ]*$");
            Assert.IsFalse(rule.isPropValueValid("Propname", "fdfasd 3dfasdf", ref errorMessage), "fdfasd 3dfasdf");
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";
            Assert.IsTrue(rule.isPropValueValid("Propname", "fdfasd-fdf asdf", ref errorMessage), "fdfasd fdfasdf");
            Assert.IsFalse(errorMessage.Length > 0);

            Assert.IsFalse(rule.isPropValueValid("Propname", "fdfasd", ref errorMessage), "fdfasd");
            Assert.IsTrue(errorMessage.Length > 0);
        }
    }
}
