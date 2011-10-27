// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    [TestFixture]
    public class TestXmlFilterLoader
    {
        [SetUp]
        public virtual void SetupTest()
        {
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }
        [Test]
        public void TestConstruction()
        {
            //---------------Set up test pack-------------------
       
            //---------------Execute Test ----------------------
            XmlFilterLoader loader = new XmlFilterLoader(new DtdLoader(), GetDefClassFactory());
            //---------------Test Result -----------------------
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterDef()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            string propName = TestUtil.GetRandomString();
            string label = TestUtil.GetRandomString();
            string filterDefXml = string.Format(
                @"
                        <filter>
                            <filterProperty name=""{0}"" label=""{1}"" />
                        </filter>
	            ", propName, label);

            //---------------Execute Test ----------------------
            IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);
            //---------------Test Result -----------------------

            Assert.IsNotNull(filterDef);
            Assert.AreEqual(1, filterDef.FilterPropertyDefs.Count);
            Assert.AreEqual(propName, filterDef.FilterPropertyDefs[0].PropertyName);
            Assert.AreEqual(label, filterDef.FilterPropertyDefs[0].Label);
            Assert.AreEqual("StringTextBoxFilter", filterDef.FilterPropertyDefs[0].FilterType);
            Assert.AreEqual("Habanero.Faces.Base", filterDef.FilterPropertyDefs[0].FilterTypeAssembly);
            Assert.AreEqual(FilterClauseOperator.OpLike, filterDef.FilterPropertyDefs[0].FilterClauseOperator);
            Assert.AreEqual(0, filterDef.FilterPropertyDefs[0].Parameters.Count);
            Assert.AreEqual(FilterModes.Filter, filterDef.FilterMode);
            Assert.AreEqual(0, filterDef.Columns);


            //---------------Tear Down -------------------------          
        }

        private XmlFilterLoader CreateXmlFilterLoader()
        {
            return new XmlFilterLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test]
        public void TestFilterDefWithParameters()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            string paramName = TestUtil.GetRandomString();
            string paramValue = TestUtil.GetRandomString();
            string filterDefXml = string.Format(
                @"
                        <filter>
                            <filterProperty name=""prop1"" label=""label1"" />
                            <filterProperty name=""prop2"" label=""label2"" >
                                <parameter name=""{0}"" value=""{1}"" />
                            </filterProperty>
                        </filter>
	            ", paramName, paramValue);
            
            //---------------Execute Test ----------------------
            IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterDef);
            Assert.AreEqual(2, filterDef.FilterPropertyDefs.Count);
            Assert.AreEqual(1, filterDef.FilterPropertyDefs[1].Parameters.Count);
            Assert.IsTrue(filterDef.FilterPropertyDefs[1].Parameters.ContainsKey(paramName));
            Assert.AreEqual(paramValue, filterDef.FilterPropertyDefs[1].Parameters[paramName]);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterDef_AlternateFormat()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            const string filterDefXml = @"
                            <filter>
			                    <filterProperty name=""name""  label=""label"" >
			                    </filterProperty>
			                </filter>
	            ";

            //---------------Execute Test ----------------------
            IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------
            Assert.IsNotNull(filterDef);
            Assert.AreEqual(1, filterDef.FilterPropertyDefs.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterMode_Search()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            string filterDefXml =
                @"
                        <filter filterMode=""Search"">
                            <filterProperty name=""prop1"" label=""label1"" />
                        </filter>
	            ";

            //---------------Execute Test ----------------------
            IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------

            Assert.AreEqual(FilterModes.Search, filterDef.FilterMode);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestColumns()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            string filterDefXml =
                @"
                        <filter columns=""3"">
                            <filterProperty name=""prop1"" label=""label1"" />
                        </filter>
	            ";

            //---------------Execute Test ----------------------
            IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, filterDef.Columns);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterClauseOperator()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            string filterDefXml =
                @"
                        <filter>
                            <filterProperty name=""prop1"" label=""label1"" operator=""OpEquals""/>
                        </filter>
	            ";

            //---------------Execute Test ----------------------
            IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------
            Assert.AreEqual(FilterClauseOperator.OpEquals, filterDef.FilterPropertyDefs[0].FilterClauseOperator);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Invalid_NoProperties()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = CreateXmlFilterLoader();
            const string filterDefXml = @"<filter />";

            //---------------Execute Test ----------------------
            try
            {
                IFilterDef filterDef = loader.LoadFilterDef(filterDefXml);
                Assert.Fail("An error should have occurred because a filter requires at least on filterProperty.");
            
            //---------------Test Result -----------------------
            }
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'filter' node does not conform", ex.Message);
            }      
        }

    }

}
