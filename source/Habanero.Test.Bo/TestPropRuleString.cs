//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

            string errorMessage;
            
            //Test less than min length
            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", "a", ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            errorMessage = "";
            Assert.IsTrue(rule.IsPropValueValid("Propname", "fdfsdafasdfsdf", ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", 
                "MySurnameIsTooLongByFarThisWill Cause and Error in Bus object", ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            //Test lengths and not compulsory
            rule = new PropRuleString("Surname", "Test", 10, 20, null);
            errorMessage = "";
            Assert.IsTrue(rule.IsPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            //test zero length strings
            errorMessage = "";
            Assert.IsTrue(rule.IsPropValueValid("Propname", "", ref errorMessage)); 
            Assert.IsTrue(errorMessage.Length == 0);

            //Test that it ignores negative max length
            rule = new PropRuleString("Surname", "Test", -10, -1, null);
            errorMessage = "";
            Assert.IsTrue(rule.IsPropValueValid("Propname", "", ref errorMessage)); //test zero length strings
            Assert.IsFalse(errorMessage.Length > 0);

            errorMessage = "";
            Assert.IsTrue(rule.IsPropValueValid("Propname", "ffff", ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);

            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", 11, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", new DateTime(2005,06,05), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }

        [Test]
        public void TestStringRulePatternMatch()
        {
            //Pattern match no numeric characters allowed
            string errorMessage = "";
            PropRuleString rule = new PropRuleString("Surname", "Test", 10, 20, @"^[a-zA-Z\- ]*$");
            Assert.IsFalse(rule.IsPropValueValid("Propname", "fdfasd 3dfasdf", ref errorMessage), "fdfasd 3dfasdf");
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";
            Assert.IsTrue(rule.IsPropValueValid("Propname", "fdfasd-fdf asdf", ref errorMessage), "fdfasd fdfasdf");
            Assert.IsFalse(errorMessage.Length > 0);

            Assert.IsFalse(rule.IsPropValueValid("Propname", "fdfasd", ref errorMessage), "fdfasd");
            Assert.IsTrue(errorMessage.Length > 0);
        }
    }
}
