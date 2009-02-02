using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// This test class tests the XmlBusinessObjectLookupListLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlBusinessObjectLookupListLoader
    {
        private XmlBusinessObjectLookupListLoader itsLoader;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlBusinessObjectLookupListLoader();
            ClassDef.ClassDefs.Clear();
        }

        //TODO - Mark 02 Feb 2009 : Add DTD validation tests, possibly?

        [Test]
        public void TestBusinessObjectLookupList()
        {
            //---------------Set up test pack-------------------
            const string xml = @"<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" />";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ILookupList lookupList = itsLoader.LoadLookupList(xml);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BusinessObjectLookupList), lookupList);
            BusinessObjectLookupList source = (BusinessObjectLookupList)lookupList;
            //Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
            Assert.AreEqual(null, source.Criteria);

        }

        [Test]
        public void TestBusinessObjectLookupListWithCriteria()
        {
            //---------------Set up test pack-------------------
            const string xml = @"<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" criteria=""TestProp=Test"" />";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ILookupList lookupList = itsLoader.LoadLookupList(xml);
            //---------------Test Result -----------------------
            BusinessObjectLookupList source = (BusinessObjectLookupList)lookupList;
            Assert.AreEqual("TestProp = 'Test'", source.Criteria.ToString());
        }

        [Test]
        public void TestBusinessObjectLookupListWithSort()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            const string xml = @"<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" sort=""TestProp asc"" />";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ILookupList lookupList = itsLoader.LoadLookupList(xml);
            //---------------Test Result -----------------------
            BusinessObjectLookupList source = (BusinessObjectLookupList)lookupList;
            Assert.AreEqual("MyBO.TestProp ASC", source.OrderCriteria.ToString());
        }
    }
}