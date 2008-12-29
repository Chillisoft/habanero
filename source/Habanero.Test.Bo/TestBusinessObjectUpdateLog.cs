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
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// This test class tests the BusinessObjectUpdateLog.
    /// The expected behaviour of this class when the object is being persisted is as follows:
    ///  - It will get called for an updated object.
    ///  - It will not get called for an object that has not been changed.
    ///  - It will always get called for a newly created object.
    ///  - It will not need to be called if the object is deleted.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectUpdateLog
    {

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            MyBusinessObjectUpdateLogBo.LoadDefaultClassDef();

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
            //base.TearDownTest();
        }

        [Test]
        public void TestSetupBusinessObjectUpdateLog()
        {
            //-------------Setup Test Pack ------------------
            
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            MyBusinessObjectUpdateLogBo myBusinessObjectUpdateLogBo = new MyBusinessObjectUpdateLogBo();
            //-------------Test Result ----------------------
            IBusinessObjectUpdateLog businessObjectUpdateLog = myBusinessObjectUpdateLogBo.BusinessObjectUpdateLog;
            Assert.IsNotNull(businessObjectUpdateLog);
            Assert.IsInstanceOfType(typeof(BusinessObjectUpdateLogStub), businessObjectUpdateLog);
        }

        [Test]
        public void TestCalledByBusinessObjectInUpdateObjectBeforePersisting_New()
        {
            //-------------Setup Test Pack ------------------
            TransactionCommitterStub transactionCommitterStub = new TransactionCommitterStub();
            MyBusinessObjectUpdateLogBo myBusinessObjectUpdateLogBo = new MyBusinessObjectUpdateLogBo();
            BusinessObjectUpdateLogStub businessObjectUpdateLog = myBusinessObjectUpdateLogBo.BusinessObjectUpdateLog as BusinessObjectUpdateLogStub;
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);
            //-------------Test Pre-conditions --------------
            Assert.IsTrue(myBusinessObjectUpdateLogBo.Status.IsNew, "BusinessObject should be new");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsDeleted, "BusinessObject should not be deleted");
            Assert.IsNotNull(businessObjectUpdateLog);
            Assert.IsFalse(businessObjectUpdateLog.Called, "BusinessObject Update Log should not have been called upon persisting.");
            //-------------Execute test ---------------------
            myBusinessObjectUpdateLogBo.UpdateObjectBeforePersisting(transactionCommitterStub);

            //-------------Test Result ----------------------
            Assert.IsTrue(businessObjectUpdateLog.Called, "BusinessObject Update Log should have been called upon persisting.");
        }

        [Test]
        public void TestCalledByBusinessObjectInUpdateObjectBeforePersisting_ExistingUpdated()
        {
            //-------------Setup Test Pack ------------------
            TransactionCommitterStub transactionCommitterStub = new TransactionCommitterStub();
            MyBusinessObjectUpdateLogBo myBusinessObjectUpdateLogBo = new MyBusinessObjectUpdateLogBo();
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);
            transactionCommitterStub.CommitTransaction();
            myBusinessObjectUpdateLogBo.MyName = "NewName";
            BusinessObjectUpdateLogStub businessObjectUpdateLog = myBusinessObjectUpdateLogBo.BusinessObjectUpdateLog as BusinessObjectUpdateLogStub;
            transactionCommitterStub = new TransactionCommitterStub();
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);

            //-------------Test Pre-conditions --------------
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsNew, "BusinessObject should not be new");
            Assert.IsTrue(myBusinessObjectUpdateLogBo.Status.IsDirty, "BusinessObject should be dirty");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsDeleted, "BusinessObject should not be deleted");
            Assert.IsNotNull(businessObjectUpdateLog);
            businessObjectUpdateLog.Called = false;

            //-------------Execute test ---------------------
            myBusinessObjectUpdateLogBo.UpdateObjectBeforePersisting(transactionCommitterStub);

            //-------------Test Result ----------------------
            Assert.IsTrue(businessObjectUpdateLog.Called, "BusinessObject Update Log should have been called upon persisting.");
        }

        [Test]
        public void TestNotCalledByBusinessObjectInUpdateObjectBeforePersisting_ExistingNotUpdated()
        {
            //-------------Setup Test Pack ------------------
            TransactionCommitterStub transactionCommitterStub = new TransactionCommitterStub();
            MyBusinessObjectUpdateLogBo myBusinessObjectUpdateLogBo = new MyBusinessObjectUpdateLogBo();
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);
            transactionCommitterStub.CommitTransaction();
            BusinessObjectUpdateLogStub businessObjectUpdateLog = myBusinessObjectUpdateLogBo.BusinessObjectUpdateLog as BusinessObjectUpdateLogStub;
            transactionCommitterStub = new TransactionCommitterStub();
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);

            //-------------Test Pre-conditions --------------
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsNew, "BusinessObject should not be new");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsDirty, "BusinessObject should not be dirty");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsDeleted, "BusinessObject should not be deleted");
            Assert.IsNotNull(businessObjectUpdateLog);
            businessObjectUpdateLog.Called = false;

            //-------------Execute test ---------------------
            myBusinessObjectUpdateLogBo.UpdateObjectBeforePersisting(transactionCommitterStub);

            //-------------Test Result ----------------------
            Assert.IsFalse(businessObjectUpdateLog.Called, "BusinessObject Update Log should have been called upon persisting.");
        }

        [Test]
        public void TestNotCalledByBusinessObjectInUpdateObjectBeforePersisting_Deleted()
        {
            //-------------Setup Test Pack ------------------
            TransactionCommitterStub transactionCommitterStub = new TransactionCommitterStub();
            MyBusinessObjectUpdateLogBo myBusinessObjectUpdateLogBo = new MyBusinessObjectUpdateLogBo();
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);
            transactionCommitterStub.CommitTransaction();
            BusinessObjectUpdateLogStub businessObjectUpdateLog = myBusinessObjectUpdateLogBo.BusinessObjectUpdateLog as BusinessObjectUpdateLogStub;
            myBusinessObjectUpdateLogBo.Delete();
            transactionCommitterStub = new TransactionCommitterStub();
            transactionCommitterStub.AddBusinessObject(myBusinessObjectUpdateLogBo);

            //-------------Test Pre-conditions --------------
            Assert.IsFalse(myBusinessObjectUpdateLogBo.Status.IsNew, "BusinessObject should not be new");
            Assert.IsTrue(myBusinessObjectUpdateLogBo.Status.IsDeleted, "BusinessObject should be deleted");
            Assert.IsNotNull(businessObjectUpdateLog);
            businessObjectUpdateLog.Called = false;

            //-------------Execute test ---------------------
            myBusinessObjectUpdateLogBo.UpdateObjectBeforePersisting(transactionCommitterStub);

            //-------------Test Result ----------------------
            Assert.IsFalse(businessObjectUpdateLog.Called, "BusinessObject Update Log should have been called upon persisting.");
        }


      

    
    }
    internal class BusinessObjectUpdateLogStub : IBusinessObjectUpdateLog
    {
        private bool _called = false;

        public bool Called
        {
            get { return _called; }
            set { _called = value; }
        }

        public void Update()
        {
            _called = true;
        }
    }
    internal class MyBusinessObjectUpdateLogBo : BusinessObject
    {
        public MyBusinessObjectUpdateLogBo()
        {
            SetBusinessObjectUpdateLog(new BusinessObjectUpdateLogStub());
        }

        public IBusinessObjectUpdateLog BusinessObjectUpdateLog
        {
            get { return _businessObjectUpdateLog; }
        }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBusinessObjectUpdateLogBo"" assembly=""Habanero.Test.BO"" table=""My_Table"">
					<property  name=""MyID"" type=""Guid"" />
					<property  name=""MyName"" />                
					<primaryKey>
						<prop name=""MyID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public Guid MyID
        {
            get { return (Guid)GetPropertyValue("MyID"); }
            set { this.SetPropertyValue("MyID", value); }
        }

        public string MyName
        {
            get { return (string)GetPropertyValue("MyName"); }
            set { SetPropertyValue("MyName", value); }
        }
        public override string ToString()
        {
            return MyName;
        }
    }
}
