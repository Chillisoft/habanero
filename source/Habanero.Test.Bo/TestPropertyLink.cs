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
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
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
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
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
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
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
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return String.IsNullOrEmpty(input) ? "" : input.ToUpper(); });
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp);
            Assert.AreEqual(testValue.ToUpper(), bo.TestProp2);
        }

        [Test]
        public void TestDisable()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.CreateRandomString();
            PropertyLink<string, string> link = new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
            bo.TestProp = testValue;
            //---------------Assert PreConditions---------------      
            Assert.AreEqual(testValue, bo.TestProp2);
            //---------------Execute Test ----------------------
            link.Disable();
            bo.TestProp = TestUtil.CreateRandomString();
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp2);     
        }

        [Test]
        public void TestEnable()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.CreateRandomString();
            string testValue2 = TestUtil.CreateRandomString();
            PropertyLink<string, string> link = new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
            link.Disable();
            bo.TestProp = testValue;
            //---------------Assert PreConditions---------------      
            Assert.AreNotEqual(testValue, bo.TestProp2);
            //---------------Execute Test ----------------------
            bo.TestProp2 = testValue;
            link.Enable();
            bo.TestProp = testValue2;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue2, bo.TestProp2);     
        }

        [Test]
        public void TestChangeFromValueAndBack()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.CreateRandomString();
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
            bo.TestProp = testValue;
            bo.TestProp2 = TestUtil.CreateRandomString(); ;
            //---------------Execute Test ----------------------
            bo.TestProp = bo.TestProp2;
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp2);     
            //---------------Tear Down -------------------------          
        }
 
    }
}
