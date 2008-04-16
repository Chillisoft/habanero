using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestMultipleRelationship : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            base.SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestTypeOfMultipleCollection()
        {
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            new Address();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            
            Assert.AreSame(typeof(RelatedBusinessObjectCollection<Address>), cp.Addresses.GetType());
        }

        [Test]
        public void TestReloadingRelationship()
        {
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            new Address();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectCollection addresses = cp.Addresses;
            Assert.AreSame(addresses, cp.Addresses);
        }
    }
}
