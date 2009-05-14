using System;
using System.IO;
using CustomerExample.BO;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestBase
    {
        Random rnd = new Random();
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.LoadClassDefs(
                    new XmlClassDefsLoader(new StreamReader("Classdefs.xml").ReadToEnd(), new DtdLoader()));
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }
        [TearDown]
        public virtual void TearDownTest()
        {
            ClassDef.ClassDefs.Clear();
        }
        
        protected static IBusinessObjectLoader GetBusinessObjectLoader()
        {
            return BORegistry.DataAccessor.BusinessObjectLoader;
        }

        protected static Customer CreateSavedCustomer()
        {
            Customer customer = new Customer();
            customer.CustomerName = "Valid Name";
            customer.CustomerCode = "Code";
            customer.Save();
            return customer;
        }
        public string GetRandomString()
        {
            return rnd.Next().ToString();
        }
    }
}