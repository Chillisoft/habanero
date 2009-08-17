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
            Initialise();
            ClassDef.ClassDefs.Clear();
        }

        protected void Initialise() {
            _loader = new XmlSimpleLookupListLoader(new DtdLoader(), GetDefClassFactory() );
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
