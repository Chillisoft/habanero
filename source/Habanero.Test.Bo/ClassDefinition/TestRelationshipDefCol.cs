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
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestRelationshipDefCol
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddDuplicationException()
        {
            SingleRelationshipDef relDef = new SingleRelationshipDef("rel", typeof(MyRelatedBo), new RelKeyDef(), true);
            RelationshipDefCol col = new RelationshipDefCol();
            col.Add(relDef);
            col.Add(relDef);
        }

        [Test]
        public void TestRemove()
        {
            SingleRelationshipDef relDef = new SingleRelationshipDef("rel", typeof(MyRelatedBo), new RelKeyDef(), true);
            RelationshipDefColInheritor col = new RelationshipDefColInheritor();
            
            col.CallRemove(relDef);
            col.Add(relDef);
            Assert.AreEqual(1, col.Count);
            col.CallRemove(relDef);
            Assert.AreEqual(0, col.Count);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestThisIndexerException()
        {
            RelationshipDefCol col = new RelationshipDefCol();
            RelationshipDef relDef = col["rel"];
        }

        // Grants access to protected methods
        private class RelationshipDefColInheritor : RelationshipDefCol
        {
            public void CallRemove(RelationshipDef relDef)
            {
                Remove(relDef);
            }
        }
    }
}