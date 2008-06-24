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

        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            MyBusinessObjectUpdateLogBo.LoadDefaultClassDef();
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
            Assert.IsTrue(myBusinessObjectUpdateLogBo.State.IsNew, "BusinessObject should be new");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsDeleted, "BusinessObject should not be deleted");
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
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsNew, "BusinessObject should not be new");
            Assert.IsTrue(myBusinessObjectUpdateLogBo.State.IsDirty, "BusinessObject should be dirty");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsDeleted, "BusinessObject should not be deleted");
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
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsNew, "BusinessObject should not be new");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsDirty, "BusinessObject should not be dirty");
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsDeleted, "BusinessObject should not be deleted");
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
            Assert.IsFalse(myBusinessObjectUpdateLogBo.State.IsNew, "BusinessObject should not be new");
            Assert.IsTrue(myBusinessObjectUpdateLogBo.State.IsDeleted, "BusinessObject should be deleted");
            Assert.IsNotNull(businessObjectUpdateLog);
            businessObjectUpdateLog.Called = false;

            //-------------Execute test ---------------------
            myBusinessObjectUpdateLogBo.UpdateObjectBeforePersisting(transactionCommitterStub);

            //-------------Test Result ----------------------
            Assert.IsFalse(businessObjectUpdateLog.Called, "BusinessObject Update Log should have been called upon persisting.");
        }


        private class BusinessObjectUpdateLogStub: IBusinessObjectUpdateLog
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

        private class MyBusinessObjectUpdateLogBo : BusinessObject
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
}
