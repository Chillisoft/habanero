//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
        public virtual void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected void Initialise()
        {
            _loader = new XmlSimpleLookupListLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory() { return new DefClassFactory(); }

        [Test]
        public void TestSimpleLookupListOptions()
        {
            ILookupList lookupList =
                _loader.LoadLookupList(
                    @"<simpleLookupList options=""option1|option2|option3"" />");
            ISimpleLookupList source = (ISimpleLookupList)lookupList;
            Assert.AreEqual(3, source.GetLookupList().Count, "LookupList should have three keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListItems()
        {
            ILookupList lookupList =
                _loader.LoadLookupList(
                    @"
						<simpleLookupList>
							<item display=""s1"" value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
							<item display=""s2"" value=""{B89CC2C9-4CBB-4519-862D-82AB64796A58}"" />
                        </simpleLookupList>
					");
            ISimpleLookupList source = (ISimpleLookupList)lookupList;
            Dictionary<string, string> dictionary = source.GetLookupList();
            Assert.AreEqual(2, dictionary.Count, "LookupList should have 2 keyvaluepairs");
            Assert.IsTrue(dictionary.ContainsKey("s1"), "should contain the display value 's1' as a key");
            Assert.AreEqual(new Guid("{C2887FB1-7F4F-4534-82AB-FED92F954783}").ToString("D"), dictionary["s1"]);
            Assert.IsTrue(dictionary.ContainsKey("s2"), "should contain the display value 's2' as a key");
            Assert.AreEqual(new Guid("{B89CC2C9-4CBB-4519-862D-82AB64796A58}").ToString("D"), dictionary["s2"]);
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
            ISimpleLookupList source = (ISimpleLookupList)lookupList;
            Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListNoItems()
        {
            try
            {
                ILookupList lookupList = _loader.LoadLookupList(@"
					<simpleLookupList></simpleLookupList>
				");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A 'simpleLookupList' element does not contain any 'item' elements or any items in the 'options' attribute.  It should contain one or more ", ex.Message);
            }
        }

        [Test]
        public void TestSimpleLookupListItemHasNoDisplay()
        {
            try
            {
                ILookupList lookupList = _loader.LoadLookupList(@"
					<simpleLookupList>
						<item value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
                    </simpleLookupList>
				");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An 'item' is missing a 'display' attribute that specifies the string to show to the user in a display", ex.Message);
            }
        }

        [Test]
        public void TestSimpleLookupListItemHasNoValue()
        {
            try
            {
                ILookupList lookupList = _loader.LoadLookupList(@"
				    <simpleLookupList>
						<item display=""s1"" />
                    </simpleLookupList>
				");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An 'item' is missing a 'value' attribute that specifies the value to store for the given property", ex.Message);
            }
        }

    }
}
