using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
   
    [TestFixture]
    public class TestXmlDatabaseLookupListLoader
    {
        private XmlDatabaseLookupListLoader _loader;

        [SetUp]
        public void SetupTest()
        {
            _loader = new XmlDatabaseLookupListLoader();
            ClassDef.ClassDefs.Clear();
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDatabaseLookupListWithInvalidTimeout()
        {
            _loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" timeout=""aaa"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDatabaseLookupListWithNegativeTimeout()
        {
            _loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" timeout=""-1"" />");
        }

        [Test]
        public void TestDatabaseLookupListWithClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();

            ILookupList def =
                _loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" class=""MyBO"" assembly=""Habanero.Test"" />");
            DatabaseLookupList source = (DatabaseLookupList)def;
            Assert.IsNotNull(source.ClassDef);
            Assert.AreEqual(classDef.ClassName, source.ClassDef.ClassName);
            Assert.AreEqual(10000, source.TimeOut);
        }

    }
}
