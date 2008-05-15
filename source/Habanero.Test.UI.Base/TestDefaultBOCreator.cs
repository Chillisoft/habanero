using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestDefaultBOCreator : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            base.SetupDBConnection();
        }
        [Test]
        public void TestCreateBusinessObjectFromCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            new ContactPerson();
            BusinessObjectCollection<ContactPerson> col = new BusinessObjectCollection<ContactPerson>();

            IBusinessObjectCreator boCreator = new DefaultBOCreator(col);

            //---------------Execute Test ----------------------
            ContactPerson cp = (ContactPerson) boCreator.CreateBusinessObject();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(col.Contains(cp));
            //---------------Tear Down -------------------------
        }
    }
}
