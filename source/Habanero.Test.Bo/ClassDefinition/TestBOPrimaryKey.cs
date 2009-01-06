using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.BO
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
            string surname = TestUtils.RandomString;
            contactPersonTestBO.ContactPersonID = contactPersonID;
            contactPersonTestBO.Surname = surname;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object value = contactPersonTestBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            List<string> list = (List<string>) value;
            Assert.Contains("ContactPersonID=" + contactPersonID, list);
            Assert.Contains("Surname=" + surname, list);
        }
    }
}