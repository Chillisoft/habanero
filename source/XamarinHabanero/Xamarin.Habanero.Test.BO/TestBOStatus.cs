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
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOStatus 
    {
        [SetUp]
        public void SetupTest()
        {
           
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
        
        }

 

        [Test]
        public void TestCreateBOStatus()
        {
            //---------------Set up test pack-------------------
            Car bo = new Car();
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            IBOStatus boStatus = new BOStatus(bo);
            //---------------Tear Down -------------------------
            Assert.AreSame(boStatus.BusinessObject, bo);
        }

        [Test]
        public void TestDataAccessorDBConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            try
            {
                new BOStatus(null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("bo", ex.ParamName);
            }
        }
        [Test]
        public void Test_SetBONull()
        {
            //---------------Set up test pack-------------------
            Car bo = new Car();
            BOStatus status = new BOStatus(bo);
            //---------------Execute Test ----------------------
            try
            {
                status.BusinessObject = null;
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("value", ex.ParamName);
            }
        }
    }
}