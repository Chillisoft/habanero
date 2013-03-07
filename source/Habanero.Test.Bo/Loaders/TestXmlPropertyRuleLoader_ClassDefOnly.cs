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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlRuleLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPropertyRuleLoader_ClassDefOnly
    {
        //private XmlRuleLoader _loader;

        [SetUp]
        public virtual void SetupTest()
        {
            //Initialise();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

//        protected void Initialise()
//        {
//            _loader = CreateXmlRuleLoader();
//        }

        protected virtual XmlRuleLoader CreateXmlRuleLoader()
        {
            return new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }
        
        [Test]
        public void TestPropRuleInteger_NullValues()
        {
            //---------------Execute Test ----------------------
            var loader = CreateXmlRuleLoader();
            IPropRule rule = loader.LoadRule(typeof(int).Name,
                                              @"<rule name=""Test Rule"" message=""Test Message"">
                        <add key=""min"" value=""""/>
                        <add key=""max"" value="""" /></rule>
");
            //---------------Test Result -----------------------
            Assert.AreEqual(Int32.MinValue, rule.Parameters["min"]);
            Assert.AreEqual(Int32.MaxValue, rule.Parameters["max"]);
        }
        
        [Test]
        public void TestPropRuleStringBlankAttributesTranslateToValidValues()
        {
            //---------------Execute Test ----------------------
            var loader = CreateXmlRuleLoader();
            IPropRule rule = loader.LoadRule(typeof (string).Name,
                                              @"<rule name=""TestString"" message=""String Test Message"" >
                            <add key=""minLength"" value="""" />          
                            <add key=""maxLength"" value="""" />
                        </rule>                          
");
            //---------------Test Result -----------------------
            Assert.AreEqual(0, Convert.ToInt32(rule.Parameters["minLength"]));
            Assert.AreEqual(-1, Convert.ToInt32(rule.Parameters["maxLength"]));
        }

        [Test]
        public void TestPropRuleDate_BlankValues()
        {
            //---------------Execute Test ----------------------
            var loader = CreateXmlRuleLoader();
            PropRuleDate rule = (PropRuleDate) loader.LoadRule(typeof(DateTime).Name,
                                                                @"<rule name=""TestDate""  >
                            <add key=""min"" value="""" />
                            <add key=""max"" value="""" />
                        </rule>                          
");
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MinValue, rule.MinValue);
            Assert.AreEqual(DateTime.MaxValue, rule.MaxValue);
        }

        [Test]
        public void TestPropRuleDecimal_Null()
        {
            //---------------Set up test pack-------------------
            var loader = CreateXmlRuleLoader();
            IPropRule rule = loader.LoadRule(typeof(Decimal).Name,
                                              @"<rule name=""TestDec"" >
                            <add key=""min"" value="""" />
                            <add key=""max"" value="""" />
                        </rule>                          
");
            //---------------Execute Test ----------------------
            Assert.AreEqual(Decimal.MinValue, Convert.ToDecimal(rule.Parameters["min"]));
            Assert.AreEqual(Decimal.MaxValue, Convert.ToDecimal(rule.Parameters["max"]));
        }
    }
}