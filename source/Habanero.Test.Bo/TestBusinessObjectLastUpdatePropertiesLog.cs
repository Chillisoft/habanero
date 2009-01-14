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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            GlobalRegistry.SecurityController = null;
            new Address();
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
            Assert.IsNull(userBoProp.Value);
            //-------------Execute test ---------------------
            DateTime beforeUpdate = DateTime.Now;
            log.Update();
            DateTime afterUpdate = DateTime.Now;
            //-------------Test Result ----------------------
            Assert.IsNull(userBoProp.Value);
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
            new Engine(); new Car();
            ContactPerson contactPerson = new ContactPerson();
            IBOProp dateBoProp = contactPerson.Props["DateLastUpdated"];
            IBOProp userBoProp = contactPerson.Props["UserLastUpdated"];
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
            
            new Engine(); new Car();
            ContactPerson contactPerson = new ContactPerson();
            IBOProp dateBoProp = contactPerson.Props["DateLastUpdated"];
            IBOProp userBoProp = contactPerson.Props["UserLastUpdated"];
            contactPerson.CancelEdits();
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
            new Engine(); new Car();
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
