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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOObjectID
    {
        [Test]
        public void Test_AsString_CurrentValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOObjectID();
            Guid guid = Guid.NewGuid();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(guid.ToString(), keyAsString);
        }

        [Test]
        public void Test_AsString_CurrentValue_SetValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOObjectID();
            Guid originalGuid = Guid.NewGuid();
            Guid newGuid = Guid.NewGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(originalGuid);
            primaryKey[0].Value = newGuid;
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(newGuid.ToString(), keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOObjectID();
            Guid originalGuid = Guid.NewGuid();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(originalGuid);
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(originalGuid.ToString(), keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue_SetValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOObjectID();
            Guid originalGuid = Guid.NewGuid();
            Guid newGuid = Guid.NewGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(originalGuid);
            primaryKey[0].Value = newGuid;
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(originalGuid.ToString(), keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue_SetValueTwice()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOObjectID();
            Guid originalGuid = Guid.NewGuid();
            primaryKey.SetObjectGuidID(originalGuid);
            Guid newGuid1 = Guid.NewGuid();
            primaryKey[0].Value = newGuid1;
            Guid newGuid2 = Guid.NewGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------

            primaryKey[0].Value = newGuid2;
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(newGuid1.ToString(), keyAsString);
        }

        [Test]
        public void Test_EqualsOp()
        {
            //--------------- Set up test pack ------------------
            BOObjectID id1 = null;
            BOObjectID id2 = null;
            
            //--------------- Execute Test ----------------------
            bool result = id1 == id2;
            //--------------- Test Result -----------------------
            Assert.IsTrue(result);
        }
        
        [Test]
        public void Test_EqualsOp_RightNull()
        {
            //--------------- Set up test pack ------------------
            BOObjectID id1 = CreateBOObjectID();
            BOObjectID id2 = null;
            
            //--------------- Execute Test ----------------------
            bool result = id1 == id2;
            //--------------- Test Result -----------------------
            Assert.IsFalse(result);
        }
        
        [Test]
        public void Test_EqualsOp_LeftNull()
        {
            //--------------- Set up test pack ------------------
            BOObjectID id1 = null;
            BOObjectID id2 = CreateBOObjectID();
            
            //--------------- Execute Test ----------------------
            bool result = id1 == id2;
            //--------------- Test Result -----------------------
            Assert.IsFalse(result);
        }
        
        [Test]
        public void Test_EqualsOp_Equal()
        {
            //--------------- Set up test pack ------------------
            BOObjectID id1 = CreateBOObjectID();
            BOObjectID id2 = CreateBOObjectID();
            Guid guid = Guid.NewGuid();
            id1[0].Value = guid;
            id2[0].Value = guid;
            //--------------- Execute Test ----------------------
            bool result = id1 == id2;
            //--------------- Test Result -----------------------
            Assert.IsTrue(result);
        }
        
        [Test]
        public void Test_EqualsOp_NotEqual()
        {
            //--------------- Set up test pack ------------------
            BOObjectID id1 = CreateBOObjectID();
            BOObjectID id2 = CreateBOObjectID();
            id1[0].Value = Guid.NewGuid();
            id2[0].Value = Guid.NewGuid();
            //--------------- Execute Test ----------------------
            bool result = id1 == id2;
            //--------------- Test Result -----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_ObjectID_NotSet()
        {
            //--------------- Set up test pack ------------------
            //--------------- Test Preconditions ----------------
            //--------------- Execute Test ----------------------
            BOObjectID primaryKey = CreateBOObjectID();
            //--------------- Test Result -----------------------
            Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
        }

        [Test]
        public void Test_ObjectID_SetObjectGuidID()
        {
            //--------------- Set up test pack ------------------
            BOObjectID primaryKey = CreateBOObjectID();
            Guid id = Guid.NewGuid();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(id);
            //--------------- Test Result -----------------------
            Assert.AreEqual(id, primaryKey.ObjectID);
        }

        [Test]
        public void Test_ObjectID_SetObjectGuidID_Twice()
        {
            //--------------- Set up test pack ------------------
            BOObjectID primaryKey = CreateBOObjectID();
            Guid id = Guid.NewGuid();
            primaryKey.SetObjectGuidID(id);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(id, primaryKey.ObjectID);
            //--------------- Execute Test ----------------------
            try
            {
                primaryKey.SetObjectGuidID(Guid.NewGuid());
            //--------------- Test Result -----------------------
                Assert.Fail("InvalidObjectIdException expected");
            } catch(InvalidObjectIdException ex)
            {
                Assert.AreEqual("The ObjectGuidID has already been set for this object.", ex.Message);
                Assert.AreEqual(id, primaryKey.ObjectID);
            }
        }

        [Test]
        public void Test_SetObjectID_SetsThePreviousObjectID()
        {
            //--------------- Set up test pack ------------------
            BOObjectID primaryKey = CreateBOObjectID();
            Guid id = Guid.NewGuid();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
            Assert.AreEqual(Guid.Empty, primaryKey.PreviousObjectID);
            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(id);
            //--------------- Test Result -----------------------
            IBOProp iboProp = primaryKey[0];
            Assert.AreEqual(id, primaryKey.ObjectID);
            Assert.AreEqual(id, iboProp.Value);
            Assert.AreEqual(id, primaryKey.PreviousObjectID);
        }
        [Test]
        public void Test_ObjectID_ResetObjectIDProperty()
        {
            //--------------- Set up test pack ------------------
            BOObjectID primaryKey = CreateBOObjectID();
            Guid id = Guid.NewGuid();
            primaryKey.SetObjectGuidID(id);
            IBOProp iboProp = primaryKey[0];
            Guid newID = Guid.NewGuid();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(id, primaryKey.ObjectID);
            Assert.AreEqual(id, iboProp.Value);
            Assert.AreEqual(id, primaryKey.PreviousObjectID);
            //--------------- Execute Test ----------------------
            iboProp.Value = newID;
            //--------------- Test Result -----------------------
            Assert.AreEqual(id, primaryKey.PreviousObjectID);
            Assert.AreEqual(newID, iboProp.Value);
            Assert.AreEqual(newID, primaryKey.ObjectID);
        }
        [Test]
        public void Test_ObjectID_EqualsIdPropValue()
        {
            //--------------- Set up test pack ------------------
            BOObjectID primaryKey = CreateBOObjectID();
            Guid id = Guid.NewGuid();
            IBOProp keyProp = primaryKey[0];
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
            //--------------- Execute Test ----------------------
            keyProp.Value = id;
            //--------------- Test Result -----------------------
            Assert.AreEqual(id, keyProp.Value);
            Assert.AreEqual(id, primaryKey.ObjectID);
        }

        //[Test]
        //public void Test_ObjectID_EqualsIdPropValue()
        //{
        //    //--------------- Set up test pack ------------------
        //    BOObjectID primaryKey = CreateBOObjectID();
        //    Guid id = Guid.NewGuid();
        //    IBOProp keyProp = primaryKey[0];
        //    //--------------- Test Preconditions ----------------
        //    Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
        //    //--------------- Execute Test ----------------------
        //    keyProp.Value = id;
        //    //--------------- Test Result -----------------------
        //    Assert.AreEqual(id, keyProp.Value);
        //    Assert.AreEqual(id, primaryKey.ObjectID);
        //}


        [Test]
        public void Test_ContactPersonIDIsSetForNewBusinessObject()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            Guid origContactPersonID = originalContactPerson.ContactPersonID;
            Guid newContactPersonID = Guid.NewGuid();
            //--------------- Test Preconditions ----------------
            Assert.IsNotNull(origContactPersonID);
            Assert.AreEqual(origContactPersonID, originalContactPerson.ID.ObjectID);
            //--------------- Execute Test ----------------------
            originalContactPerson.ContactPersonID = newContactPersonID;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(originalContactPerson.ContactPersonID);
            Assert.AreEqual(newContactPersonID, originalContactPerson.ContactPersonID);
            Assert.AreEqual(newContactPersonID, originalContactPerson.ID.ObjectID);
        }

        private static BOObjectID CreateBOObjectID()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            BOPropCol propCol = new BOPropCol();

            propCol.Add(propDef1.CreateBOProp(false));
            PrimaryKeyDef keyDef = new PrimaryKeyDef();
            keyDef.IsGuidObjectID = true;
            keyDef.Add(propDef1);
            return (BOObjectID)keyDef.CreateBOKey(propCol);
        }

    }
}
