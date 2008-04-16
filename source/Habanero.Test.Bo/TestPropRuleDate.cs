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

using System;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleDate
    {
        [Test]
        public void TestDateRule()
        {
            //	PropDef lPropDef = new PropDef("Surname", typeof(string),PropReadWriteRule.ReadWrite);
            PropRuleDate rule =
                new PropRuleDate("BirthDate", "Test", new DateTime(1900, 01, 01), new DateTime(2010, 12, 31));

            string errorMessage = "";

            //Test less than max length
            Assert.IsFalse(rule.isPropValueValid("Propname", new DateTime(1891, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("Propname", new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            Assert.IsFalse(rule.isPropValueValid("Propname", new DateTime(2091, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);


            rule = new PropRuleDate("BirthDate", "Test");
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";

            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("Propname", new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
        }
    }

}
