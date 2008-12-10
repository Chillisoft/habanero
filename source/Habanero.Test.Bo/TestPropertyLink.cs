using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropertyLink
    {

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            
        }

        [Test]
        public void TestLink()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            new PropertyLink(bo, "TestProp", "TestProp2", delegate(object value) { return value; });
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp2);
        }

        [Test]
        public void TestLink_Twice()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            new PropertyLink(bo, "TestProp", "TestProp2", delegate(object value) { return value; });
            bo.TestProp = TestUtil.CreateRandomString();;
            string testValue = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp2);
        }

        [Test]
        public void TestLink_DoesntChangeDestinationIfAlreadySet()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.CreateRandomString();
            const string prop2Value = "my set value";
            bo.TestProp2 = prop2Value;
            //---------------Execute Test ----------------------
            new PropertyLink(bo, "TestProp", "TestProp2", delegate(object value) { return value; });
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(prop2Value, bo.TestProp2);
        }


        [Test]
        public void TestLink_TransformToUpper()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            new PropertyLink(bo, "TestProp", "TestProp2", delegate(object value) { return value == null ? null : ((String)value).ToUpper(); });
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp);
            Assert.AreEqual(testValue.ToUpper(), bo.TestProp2);
        }

 
    }
}
