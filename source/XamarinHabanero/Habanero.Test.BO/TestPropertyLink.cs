#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base.Exceptions;
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
            string testValue = TestUtil.GetRandomString();
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
            bo.TestProp = TestUtil.GetRandomString();
            string testValue = TestUtil.GetRandomString();
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
            string testValue = TestUtil.GetRandomString();
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
            string testValue = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2",
                                             delegate(string input) { return String.IsNullOrEmpty(input) ? "" : input.ToUpper(); });
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
            string testValue = TestUtil.GetRandomString();
            PropertyLink<string, string> link = new PropertyLink<string, string>(bo, "TestProp", "TestProp2",
                                                                                 delegate(string input) { return input; });
            bo.TestProp = testValue;
            //---------------Assert PreConditions---------------      
            Assert.AreEqual(testValue, bo.TestProp2);
            //---------------Execute Test ----------------------
            link.Disable();
            bo.TestProp = TestUtil.GetRandomString();
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp2);
        }

        [Test]
        public void TestEnable()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            string testValue = TestUtil.GetRandomString();
            string testValue2 = TestUtil.GetRandomString();
            PropertyLink<string, string> link = new PropertyLink<string, string>(bo, "TestProp", "TestProp2",
                                                                                 delegate(string input) { return input; });
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
            string testValue = TestUtil.GetRandomString();
            new PropertyLink<string, string>(bo, "TestProp", "TestProp2", delegate(string input) { return input; });
            bo.TestProp = testValue;
            bo.TestProp2 = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            bo.TestProp = bo.TestProp2;
            bo.TestProp = testValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(testValue, bo.TestProp2);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_ConstructWhenPropNotExists()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();          
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new PropertyLink<string, string>(bo, "NonExistProp", "TestProp2", delegate(string input) { return input; });
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("A BOProp with the name", ex.Message);
                StringAssert.Contains("does not exist in the prop collection", ex.Message);
            }

        }
        [Test]
        public void Test_ConstructWhenRelPropNotExists()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();          
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new PropertyLink<string, string>(bo, "TestProp", "NonExistProp2", delegate(string input) { return input; });
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("A BOProp with the name", ex.Message);
                StringAssert.Contains("does not exist in the prop collection", ex.Message);
            }

        }
    }
}