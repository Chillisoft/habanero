using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPrimaryKey 
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_CreateBOPrimaryKey()
        {
            //---------------Set up test pack-------------------
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(pkDef);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boPrimaryKey);
        }
        [Test]
        public void Test_CreateBOPrimaryKey_IsObjectID()
        {
            //---------------Set up test pack-------------------
            PrimaryKeyDef pkDef = new PrimaryKeyDef 
                    {new PropDef("prop2", typeof(Guid), PropReadWriteRule.ReadWrite, null)};
            pkDef.IsGuidObjectID = true;
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(pkDef);
            //---------------Assert Precondition----------------
            Assert.IsTrue(pkDef.IsGuidObjectID);
            //---------------Execute Test ----------------------
            bool isObjectID = boPrimaryKey.IsGuidObjectID;
            //---------------Test Result -----------------------
            Assert.IsTrue(isObjectID);
        } 
        [Test]
        public void Test_CreateBOPrimaryKey_IsObjectID_False()
        {
            //---------------Set up test pack-------------------
            PrimaryKeyDef pkDef = new PrimaryKeyDef 
                    {new PropDef("prop2", typeof(Guid), PropReadWriteRule.ReadWrite, null)};
            pkDef.IsGuidObjectID = false;
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(pkDef);
            //---------------Assert Precondition----------------
            Assert.IsFalse(pkDef.IsGuidObjectID);
            //---------------Execute Test ----------------------
            bool isObjectID = boPrimaryKey.IsGuidObjectID;
            //---------------Test Result -----------------------
            Assert.IsFalse(isObjectID);
        }

        [Test]
        public void Test_GetAsValue_Guid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            Guid contactPersonID = Guid.NewGuid();
            contactPersonTestBO.ContactPersonID = contactPersonID;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object value = contactPersonTestBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            Assert.AreEqual(contactPersonID, value);
        }

        [Test]
        public void Test_GetAsValue_Int()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithIntID();
            const int expecteID = 4;
            TestAutoInc testBO = new TestAutoInc {TestAutoIncID = expecteID};
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object value = testBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            Assert.AreEqual(expecteID, value);
        }
        [Test]
        public void Test_GetAsValue_CompositeKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            Guid contactPersonID = Guid.NewGuid();
            string surname = BOTestUtils.RandomString;
            contactPersonTestBO.ContactPersonID = contactPersonID;
            contactPersonTestBO.Surname = surname;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object value = contactPersonTestBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            string valueString = (string) value;
            StringAssert.Contains("ContactPersonID=" + contactPersonID, valueString);
            StringAssert.Contains("Surname=" + surname, valueString);
        }

        [Test]
        public void Test_CreatePrimaryKey_TwoPropDefs()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef2);
            keyDef.Add(propDef1);

            BOPropCol boPropCol = new BOPropCol();
            boPropCol.Add(propDef1.CreateBOProp(false));
            boPropCol.Add(propDef2.CreateBOProp(false));

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, keyDef.Count);
            //---------------Execute Test ----------------------
            BOPrimaryKey boPrimaryKey = (BOPrimaryKey)keyDef.CreateBOKey(boPropCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(keyDef.Count, boPrimaryKey.Count);
            Assert.IsTrue(boPrimaryKey.IsCompositeKey);
        }
        [Test]
        public void Test_CreatePrimaryKey_OnePropDefs()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef1);

            BOPropCol boPropCol = new BOPropCol();
            boPropCol.Add(propDef1.CreateBOProp(false));

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, keyDef.Count);
            //---------------Execute Test ----------------------
            BOPrimaryKey boPrimaryKey = (BOPrimaryKey)keyDef.CreateBOKey(boPropCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(keyDef.Count, boPrimaryKey.Count);
            Assert.IsFalse(boPrimaryKey.IsCompositeKey);
        }

        [Test]
        public void Test_CreateWithValue_ClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            int value = TestUtil.GetRandomInt();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = value };
            object expectedID = bo.ID;
            
            //---------------Execute Test ----------------------
            BOPrimaryKey key = BOPrimaryKey.CreateWithValue(autoIncClassDef, value);
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedID, key);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_CreateWithValue_Type()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            int value = TestUtil.GetRandomInt();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = value };
            object expectedID = bo.ID;
            
            //---------------Execute Test ----------------------
            BOPrimaryKey key = BOPrimaryKey.CreateWithValue(typeof(BOWithIntID), value);
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedID, key);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_HashCode()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            int value = TestUtil.GetRandomInt();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = value };
            BOPrimaryKey key = BOPrimaryKey.CreateWithValue(typeof(BOWithIntID), value);
            //---------------Assert PreConditions---------------       
            Assert.AreEqual(bo.ID, key);
            //---------------Execute Test ----------------------
            object expectedHashCode = bo.ID.GetHashCode();
            object keyHashCode = key.GetHashCode();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedHashCode, keyHashCode);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_HashCode_CompositeKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            new Car();
            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            object originalHashCode = contactPerson.ID.GetHashCode();
            contactPerson.Save();

            //---------------Execute Test ----------------------
            object hashCodeAfterSaving = contactPerson.ID.GetHashCode();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalHashCode, hashCodeAfterSaving);

        }
    }
}