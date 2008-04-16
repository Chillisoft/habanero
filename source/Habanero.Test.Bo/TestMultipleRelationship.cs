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

        [Test]
        public void TestTypeOfMultipleCollection()
        {
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            new Address();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            
            Assert.AreSame(typeof(RelatedBusinessObjectCollection<Address>), cp.Addresses.GetType());


            

        }
    }
}
