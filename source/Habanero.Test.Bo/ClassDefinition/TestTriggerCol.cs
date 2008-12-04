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

using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestTriggerCol
    {
        [Test]
        public void TestAddAndRemove()
        {
            TriggerCol col = new TriggerCol();
            Assert.AreEqual(0, col.Count);

            Trigger trigger1 = new Trigger("prop1", null, null, "action", "value");
            Trigger trigger2 = new Trigger("prop1", null, null, "action", "value");
            col.Add(trigger1);
            col.Add(trigger2);
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(trigger1, col[0]);
            Assert.AreEqual(trigger2, col[1]);

            col.Remove(trigger1);
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(trigger2, col[0]);

            col.Remove(trigger2);
            Assert.AreEqual(0, col.Count);
        }
    }
}