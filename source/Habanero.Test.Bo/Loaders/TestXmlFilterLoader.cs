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

        [Test]
        public void TestConstruction()
        {
            //---------------Set up test pack-------------------
       
            //---------------Execute Test ----------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            //---------------Test Result -----------------------
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestFilterDef()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            string propName = TestUtil.CreateRandomString();
            string label = TestUtil.CreateRandomString();
            string filterDefXml = string.Format(
                @"
                        <filter>
                            <filterProperty name=""{0}"" label=""{1}"" />
                        </filter>
	            ", propName, label);

            //---------------Execute Test ----------------------
            FilterDef filterDef = loader.LoadFilterDef(filterDefXml);
            //---------------Test Result -----------------------

            Assert.IsNotNull(filterDef);
            Assert.AreEqual(1, filterDef.FilterPropertyDefs.Count);
            Assert.AreEqual(propName, filterDef.FilterPropertyDefs[0].PropertyName);
            Assert.AreEqual(label, filterDef.FilterPropertyDefs[0].Label);
            Assert.AreEqual("StringTextBoxFilter", filterDef.FilterPropertyDefs[0].FilterType);
            Assert.AreEqual("Habanero.UI.Base", filterDef.FilterPropertyDefs[0].FilterTypeAssembly);
            Assert.AreEqual(FilterClauseOperator.OpLike, filterDef.FilterPropertyDefs[0].FilterClauseOperator);
            Assert.AreEqual(0, filterDef.FilterPropertyDefs[0].Parameters.Count);
            Assert.AreEqual(FilterModes.Filter, filterDef.FilterMode);
            Assert.AreEqual(0, filterDef.Columns);


            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterDefWithParameters()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            string paramName = TestUtil.CreateRandomString();
            string paramValue = TestUtil.CreateRandomString();
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
            FilterDef filterDef = loader.LoadFilterDef(filterDefXml);
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterDef);
            Assert.AreEqual(2, filterDef.FilterPropertyDefs.Count);
            Assert.AreEqual(1, filterDef.FilterPropertyDefs[1].Parameters.Count);
            Assert.IsTrue(filterDef.FilterPropertyDefs[1].Parameters.ContainsKey(paramName));
            Assert.AreEqual(paramValue, filterDef.FilterPropertyDefs[1].Parameters[paramName]);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterMode_Search()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            string filterDefXml =
                @"
                        <filter filterMode=""Search"">
                            <filterProperty name=""prop1"" label=""label1"" />
                        </filter>
	            ";

            //---------------Execute Test ----------------------
            FilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------

            Assert.AreEqual(FilterModes.Search, filterDef.FilterMode);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestColumns()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            string filterDefXml =
                @"
                        <filter columns=""3"">
                            <filterProperty name=""prop1"" label=""label1"" />
                        </filter>
	            ";

            //---------------Execute Test ----------------------
            FilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, filterDef.Columns);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterClauseOperator()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            string filterDefXml =
                @"
                        <filter>
                            <filterProperty name=""prop1"" label=""label1"" operator=""OpEquals""/>
                        </filter>
	            ";

            //---------------Execute Test ----------------------
            FilterDef filterDef = loader.LoadFilterDef(filterDefXml);

            //---------------Test Result -----------------------
            Assert.AreEqual(FilterClauseOperator.OpEquals, filterDef.FilterPropertyDefs[0].FilterClauseOperator);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Invalid_NoProperties()
        {
            //---------------Set up test pack-------------------
            XmlFilterLoader loader = new XmlFilterLoader();
            const string filterDefXml = @"<filter />";

            //---------------Execute Test ----------------------
            try
            {
                FilterDef filterDef = loader.LoadFilterDef(filterDefXml);
                Assert.Fail("An error should have occurred because a filter requires at least on filterProperty.");
            
            //---------------Test Result -----------------------
            } catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'filter' node does not conform", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

    }

}
