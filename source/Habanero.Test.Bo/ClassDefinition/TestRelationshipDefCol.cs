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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestRelationshipDefCol
    {
        [Test]
        public void TestAddDuplicationException()
        {
            //---------------Set up test pack-------------------
            SingleRelationshipDef relDef = new SingleRelationshipDef("rel", typeof(MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent);
            RelationshipDefCol col = new RelationshipDefCol();
            col.Add(relDef);
            //---------------Execute Test ----------------------
            try
            {
                col.Add(relDef);
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("A relationship definition with the name 'rel' already exists", ex.Message);
            }
        }

        [Test]
        public void TestRemove()
        {
            SingleRelationshipDef relDef = new SingleRelationshipDef("rel", typeof(MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent);
            RelationshipDefColInheritor col = new RelationshipDefColInheritor();
            
            col.CallRemove(relDef);
            col.Add(relDef);
            Assert.AreEqual(1, col.Count);
            col.CallRemove(relDef);
            Assert.AreEqual(0, col.Count);
        }
#pragma warning disable 168
        [Test]
        public void TestThisIndexerException()
        {
            //---------------Set up test pack-------------------
            RelationshipDefCol col = new RelationshipDefCol();
            //---------------Execute Test ----------------------
            try
            {
                IRelationshipDef relDef = col["rel"];
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The relationship name 'rel' does not exist in the collection of relationship definitions", ex.Message);
            }
        }
#pragma warning restore 168
        [Test]
        public void Test_SetClassDef_ShouldSet()
        {
            //---------------Set up test pack-------------------
            var classDef = MockRepository.GenerateStub<IClassDef>();
            IRelationshipDefCol col = new RelationshipDefCol();
            //---------------Assert Precondition----------------
            Assert.IsNull(col.ClassDef);
            //---------------Execute Test ----------------------
            col.ClassDef = classDef;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, col.ClassDef);
        }
/*        [Test]
        public void Test_Add_ShouldSetRelDefsClassDef()
        {
            //---------------Set up test pack-------------------
            RelationshipDef relDef = new RelationshipDef();
            var col = new RelationshipDefCol();
            var expectedClassDef = MockRepository.GenerateStub<IClassDef>();
            col.ClassDef = expectedClassDef;
            //---------------Assert Preconditions---------------
            Assert.IsNull(relDef.ClassDef);
            //---------------Execute Test ----------------------
            col.Add(relDef);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, relDef.ClassDef);
        }*/

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