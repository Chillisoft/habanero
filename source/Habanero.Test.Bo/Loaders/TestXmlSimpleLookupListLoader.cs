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
   
        /// <summary>
    /// This test class tests the XmlBusinessObjectLookupListLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlSimpleLookupListLoader
    {
        private XmlSimpleLookupListLoader _loader;

        [SetUp]
        public void SetupTest()
        {
            _loader = new XmlSimpleLookupListLoader(new DtdLoader(), null );
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestSimpleLookupListOptions()
        {
            ILookupList lookupList =
                _loader.LoadLookupList(
                    @"<simpleLookupList options=""option1|option2|option3"" />");
            SimpleLookupList source = (SimpleLookupList)lookupList;
            Assert.AreEqual(3, source.GetLookupList().Count, "LookupList should have three keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListOptionsAndItems()
        {
            ILookupList lookupList =
                _loader.LoadLookupList(
                    @"
						<simpleLookupList options=""option1|option2|option3"" >
							<item display=""s1"" value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
							<item display=""s2"" value=""{B89CC2C9-4CBB-4519-862D-82AB64796A58}"" />
                        </simpleLookupList>
					");
            SimpleLookupList source = (SimpleLookupList)lookupList;
            Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSimpleLookupListNoItems()
        {
            ILookupList lookupList = _loader.LoadLookupList(@"
					<simpleLookupList></simpleLookupList>
				");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSimpleLookupListItemHasNoDisplay()
        {
            ILookupList lookupList = _loader.LoadLookupList(@"
					<simpleLookupList>
						<item value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
                    </simpleLookupList>
				");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSimpleLookupListItemHasNoValue()
        {
            ILookupList lookupList = _loader.LoadLookupList(@"
				    <simpleLookupList>
						<item display=""s1"" />
                    </simpleLookupList>
				");
        }

    }
}
