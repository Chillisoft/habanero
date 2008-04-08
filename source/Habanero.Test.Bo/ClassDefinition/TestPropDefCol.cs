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
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestPropDefCol
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddDuplicationException()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefCol col = new PropDefCol();
            col.Add(propDef);
            col.Add(propDef);
        }

        [Test]
        public void TestRemove()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefColInheritor col = new PropDefColInheritor();

            col.CallRemove(propDef);
            col.Add(propDef);
            Assert.AreEqual(1, col.Count);
            col.CallRemove(propDef);
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDefColInheritor col = new PropDefColInheritor();

            Assert.IsFalse(col.GetContains(propDef));
            col.Add(propDef);
            Assert.IsTrue(col.GetContains(propDef));
        }

        // Grants access to protected methods
        private class PropDefColInheritor : PropDefCol
        {
            public void CallRemove(PropDef propDef)
            {
                Remove(propDef);
            }

            public bool GetContains(PropDef propDef)
            {
                return Contains(propDef);
            }
        }
    }
}