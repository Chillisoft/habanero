using System;
using System.Collections.Generic;
using System.Text;
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

        private BOObjectID CreateBOObjectID()
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
