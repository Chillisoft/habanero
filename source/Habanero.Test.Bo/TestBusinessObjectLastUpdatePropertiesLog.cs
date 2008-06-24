using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// This test class tests that the BusinessObjectLastUpdatePropertiesLog updates
    /// the relevant properties with the necessary values whenever the business object is updated.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectLastUpdatePropertiesLog
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            GlobalRegistry.SecurityController = null;
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
        public void TestUpdatesProperties_NoGlobalSecurityController()
        {
            //-------------Setup Test Pack ------------------
            BOProp dateBoProp = new BOProp(new PropDef("DateLastUpdated", typeof(DateTime), PropReadWriteRule.ReadWrite, null));
            BOProp userBoProp = new BOProp(new PropDef("UserLastUpdated", typeof(string), PropReadWriteRule.ReadWrite, null));
            BusinessObjectLastUpdatePropertiesLog log = new BusinessObjectLastUpdatePropertiesLog(userBoProp, dateBoProp);
            //-------------Test Pre-conditions --------------
            Assert.IsNull(GlobalRegistry.SecurityController);
            //-------------Execute test ---------------------
            DateTime beforeUpdate = DateTime.Now;
            log.Update();
            DateTime afterUpdate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsNotNull(userBoProp.Value);
            Assert.AreEqual("", userBoProp.Value);
            Assert.IsNotNull(dateBoProp.Value);
            Assert.IsTrue(beforeUpdate <= (DateTime)dateBoProp.Value);
            Assert.IsTrue(afterUpdate >= (DateTime)dateBoProp.Value);
            
        }

        [Test]
        public void TestUpdatesProperties_UsingSecurityController()
        {
            //-------------Setup Test Pack ------------------
            BOProp dateBoProp = new BOProp(new PropDef("DateLastUpdated", typeof(DateTime), PropReadWriteRule.ReadWrite, null));
            BOProp userBoProp = new BOProp(new PropDef("UserLastUpdated", typeof(string), PropReadWriteRule.ReadWrite, null));
            ISecurityController securityController = new MySecurityController();
            BusinessObjectLastUpdatePropertiesLog log = new BusinessObjectLastUpdatePropertiesLog(userBoProp, dateBoProp, securityController);
            //-------------Test Pre-conditions --------------
            Assert.IsNull(GlobalRegistry.SecurityController);
            //-------------Execute test ---------------------
            DateTime beforeUpdate = DateTime.Now;
            log.Update();
            DateTime afterUpdate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsNotNull(userBoProp.Value);
            Assert.AreEqual("MyUserName", userBoProp.Value);
            Assert.IsNotNull(dateBoProp.Value);
            Assert.IsTrue(beforeUpdate <= (DateTime)dateBoProp.Value);
            Assert.IsTrue(afterUpdate >= (DateTime)dateBoProp.Value);

        }

        [Test]
        public void TestUpdatesProperties_UsingGlobalSecurityController()
        {
            //-------------Setup Test Pack ------------------
            BOProp dateBoProp = new BOProp(new PropDef("DateLastUpdated", typeof(DateTime), PropReadWriteRule.ReadWrite, null));
            BOProp userBoProp = new BOProp(new PropDef("UserLastUpdated", typeof(string), PropReadWriteRule.ReadWrite, null));
            ISecurityController securityController = new MySecurityController();
            GlobalRegistry.SecurityController = securityController;
            BusinessObjectLastUpdatePropertiesLog log = new BusinessObjectLastUpdatePropertiesLog(userBoProp, dateBoProp);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            DateTime beforeUpdate = DateTime.Now;
            log.Update();
            DateTime afterUpdate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsNotNull(userBoProp.Value);
            Assert.AreEqual("MyUserName", userBoProp.Value);
            Assert.IsNotNull(dateBoProp.Value);
            Assert.IsTrue(beforeUpdate <= (DateTime)dateBoProp.Value);
            Assert.IsTrue(afterUpdate >= (DateTime)dateBoProp.Value);
        }


        [Test]
        public void TestUpdatesProperties_GivenBo_UserNameAndDate()
        {
            //-------------Setup Test Pack ------------------
            ContactPerson contactPerson = new ContactPerson();
            BOProp dateBoProp = contactPerson.Props["DateLastUpdated"];
            BOProp userBoProp = contactPerson.Props["UserLastUpdated"];
            contactPerson.Restore();
            ISecurityController securityController = new MySecurityController();
            GlobalRegistry.SecurityController = securityController;
            BusinessObjectLastUpdatePropertiesLog log = new BusinessObjectLastUpdatePropertiesLog(contactPerson);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------       
            DateTime beforeUpdate = DateTime.Now;
            log.Update();
            DateTime afterUpdate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsNotNull(userBoProp.Value);
            Assert.AreEqual("MyUserName", userBoProp.Value);
            Assert.IsNotNull(dateBoProp.Value);
            Assert.IsTrue(beforeUpdate <= (DateTime)dateBoProp.Value);
            Assert.IsTrue(afterUpdate >= (DateTime)dateBoProp.Value);
        }

        [Test]
        public void TestUpdatesProperties_GivenBo_UserNameAndDate_AndSecurityController()
        {
            //-------------Setup Test Pack ------------------
            ContactPerson contactPerson = new ContactPerson();
            BOProp dateBoProp = contactPerson.Props["DateLastUpdated"];
            BOProp userBoProp = contactPerson.Props["UserLastUpdated"];
            contactPerson.Restore();
            ISecurityController securityController = new MySecurityController();
            BusinessObjectLastUpdatePropertiesLog log = new BusinessObjectLastUpdatePropertiesLog(contactPerson, securityController);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------       
            DateTime beforeUpdate = DateTime.Now;
            log.Update();
            DateTime afterUpdate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsNotNull(userBoProp.Value);
            Assert.AreEqual("MyUserName", userBoProp.Value);
            Assert.IsNotNull(dateBoProp.Value);
            Assert.IsTrue(beforeUpdate <= (DateTime)dateBoProp.Value);
            Assert.IsTrue(afterUpdate >= (DateTime)dateBoProp.Value);
        }

        [Test]
        public void TestUpdatesProperties_GivenBo_WithoutUserNameAndDate()
        {
            //-------------Setup Test Pack ------------------
            ContactPerson contactPerson = new ContactPerson();
            contactPerson.Props.Remove("DateLastUpdated");
            contactPerson.Props.Remove("UserLastUpdated");
            contactPerson.Restore();
            ISecurityController securityController = new MySecurityController();
            GlobalRegistry.SecurityController = securityController;
            BusinessObjectLastUpdatePropertiesLog log = new BusinessObjectLastUpdatePropertiesLog(contactPerson);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------       
            log.Update();
            //-------------Test Result ----------------------
            //Should give no errors.
        }

        public class MySecurityController : ISecurityController
        {
            #region ISecurityController Members

            ///<summary>
            /// Returns the current user's name.
            ///</summary>
            public string CurrentUserName
            {
                get { return "MyUserName"; }
            }

            #endregion
        }
    }
}
