#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPropertyLoader
    {
        private XmlPropertyLoader _loader;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected void Initialise() {
            _loader = new XmlPropertyLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public virtual void TestSimpleProperty()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" />");
            Assert.AreEqual("TestProp", def.PropertyName, "Property name should be same as that specified in xml");
            Assert.AreEqual("System", def.PropertyTypeAssemblyName, "Property type should be the default as defined in the dtd");
            Assert.AreEqual("String", def.PropertyTypeName, "Property type should be the default as defined in the dtd");
            Assert.AreEqual(null, def.DefaultValue, "The default default is null");
            Assert.AreEqual("TestProp", def.DatabaseFieldName,
                            "The field name should be the same as the property name by default");
            Assert.AreEqual(false, def.AutoIncrementing, "autoIncrementing should be false by default");
            Assert.AreEqual(null, def.Description, "The description is null");
            Assert.AreEqual(false, def.KeepValuePrivate, "keepValuePrivate should be false by default");
        }

        [Test]
        public virtual void TestNoPropsInString()
        {
            try
            {
                _loader.LoadProperty(@"");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An error has occurred while attempting to load a property definition, contained in a 'property' element. Check that you have correctly spe", ex.Message);
            }
        }

        [Test]
        public virtual void TestPropertyWithNoName()
        {
            try
            {
                _loader.LoadProperty(@"<property />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A 'property' element has no 'name' attribute set. Each 'property' element requires a 'name' attribute that specifies the name of the property ", ex.Message);
            }
        }

        [Test]
        public virtual void TestPropertyWithType()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" type=""Int32"" />");
            Assert.AreEqual("Int32", def.PropertyTypeName, "Property type should be same as that specified in xml");
            Assert.AreEqual("System", def.PropertyTypeAssemblyName, "Property type should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithCustomType()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" type=""MyType"" assembly=""MyAssembly"" />");
            Assert.AreEqual("MyType", def.PropertyTypeName, "Property type should be same as that specified in xml");
            Assert.AreEqual("MyAssembly", def.PropertyTypeAssemblyName, "Property type should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithDescription()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" description=""Property for Testing"" />");
            Assert.AreEqual("Property for Testing", def.Description, "Property description should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithDisplayValue()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" displayName=""Test Property"" />");
            Assert.AreEqual("Test Property", def.DisplayName, "Property description should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithKeepValuePrivate()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" keepValuePrivate=""true"" />");
            Assert.AreEqual(true, def.KeepValuePrivate, "Property 'keep value private' attribute should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithReadWriteRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadWrite"" />");
            Assert.AreEqual(PropReadWriteRule.ReadWrite, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithReadOnlyRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadOnly"" />");
            Assert.AreEqual(PropReadWriteRule.ReadOnly, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithWriteOnceRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteOnce"" />");
            Assert.AreEqual(PropReadWriteRule.WriteOnce, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithWriteNotNewRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteNotNew"" />");
            Assert.AreEqual(PropReadWriteRule.WriteNotNew, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithWriteNewRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteNew"" />");
            Assert.AreEqual(PropReadWriteRule.WriteNew, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithInvalidReadWriterule()
        {
            try
            {
                _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""invalid"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In the property definition for 'TestProp', the 'readWriteRule' was set to an invalid value. The valid options are ReadWrite, ReadOnly, WriteOnly", ex.Message);
            }
        }

        [Test]
        public virtual void TestPropertyWithDefaultValue()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" default=""TestValue"" />");
            Assert.AreEqual("TestValue", def.DefaultValue, "Default value should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithGuidDefaultValue()
        {
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""Guid"" default=""{38373667-B06A-40c5-B4CE-299CE925E121}"" />");
            Assert.AreEqual(new Guid("{38373667-B06A-40c5-B4CE-299CE925E121}"), new Guid(def.DefaultValue.ToString()),
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithDateTimeDefaultValueToday()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""Today"" />");
            Assert.AreEqual("Today", def.DefaultValue,
                            "Default value should be same as that specified in xml");
        }
        [Test]
        public virtual void TestPropertyWithDateTimeDefaultValueYesterday()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""yesterday"" />");
            Assert.AreEqual("yesterday", def.DefaultValue,
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithDateTimeDefaultValueNow()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""Now"" />");
            Assert.AreEqual("Now", def.DefaultValue,
                "Default value should be same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithDatabaseFieldName()
        {
            IPropDef def =
                _loader.LoadProperty(@"<property  name=""TestProp"" databaseField=""TestFieldName"" />");
            Assert.AreEqual("TestFieldName", def.DatabaseFieldName, "Field Name should be the same as that specified in xml");
        }

        [Test]
        public virtual void TestPropertyWithDatabaseFieldNameWithSpaces()
        {
            IPropDef def =
                _loader.LoadProperty(@"<property name=""TestProp"" databaseField=""Test FieldName"" />");
            Assert.AreEqual("Test FieldName", def.DatabaseFieldName);
        }

        [Test]
        public virtual void TestPropertyWithAutoIncrementingField()
        {
            IPropDef def = _loader.LoadProperty(
                      @"<property name=""TestProp"" autoIncrementing=""true"" />");
            Assert.AreEqual(true, def.AutoIncrementing);
        }

        [Test]
        public virtual void TestPropertyWithLength()
        {
            IPropDef def = _loader.LoadProperty(
                      @"<property name=""TestProp"" length=""5"" />");
            Assert.AreEqual(5, def.Length);
        }

        [Test]
        public virtual void TestPropertyWithInvalidLengthException()
        {
            try
            {
                _loader.LoadProperty(@"<property name=""TestProp"" length=""fff"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In the property definition for 'TestProp', the 'length' was set to an invalid integer value", ex.Message);
            }
        }

        [Test]
        public virtual void TestPropertyWithNegativeLengthException()
        {
            try
            {
                _loader.LoadProperty(@"<property name=""TestProp"" length=""-1"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In the property definition for 'TestProp', the 'length' was set to an invalid negative value", ex.Message);
            }
        }

        [Test]
        public virtual void TestPropertyWithLengthForNonString()
        {
            try
            {
                _loader.LoadProperty(@"<property name=""TestProp"" type=""bool"" length=""5"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In the property definition for 'TestProp', a 'length' attribute was provided for a property type that cannot use the attribute", ex.Message);
            }
        }

        [Test]
        public virtual void TestPropertyWithPropRule()
        {
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp""><rule name=""StringRule""><add key=""minLength"" value=""8""/><add key=""maxLength"" value=""8"" /></rule></property>");
            Assert.AreEqual(1, def.PropRules.Count);
        }

        [Test]
        public virtual void TestPropertyWithInvalidPropRule()
        {
            // this should not work as min is an invalid setting for a string rule.
            try
            {
                _loader.LoadProperty(
                        @"<property  name=""TestProp""><rule name=""StringRule""><add key=""min"" value=""8""/></rule></property>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The specified 'add' attribute was 'min' but the allowed attributes are minLength, maxLength, patternMatch", ex.Message);
            }
           
        }

        [Test]
        public virtual void TestPropertyWithTwoPropRules()
        {
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp"">
                        <rule name=""StringRule""><add key=""maxLength"" value=""8""/></rule>
                        <rule name=""StringRule2""><add key=""minLength"" value=""3""/></rule>
                      </property>");
            Assert.AreEqual(2, def.PropRules.Count);
        }


        [Test]
        public virtual void TestPropertyWithDatabaseLookupList()
        {
            MyBO.LoadDefaultClassDef();
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp""><databaseLookupList sql=""Source"" timeout=""100"" class=""MyBO"" assembly=""Habanero.Test"" /></property>");
            Assert.IsInstanceOf(typeof (IDatabaseLookupList), def.LookupList,
                           "LookupList should be of type IDatabaseLookupList but is of type " +
                           def.LookupList.GetType().Name);
            IDatabaseLookupList source = (IDatabaseLookupList) def.LookupList;
            Assert.AreEqual("Source", source.SqlString, "LookupList should be the same as that specified in xml");
            Assert.AreEqual(100, source.TimeOut);
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
        }

        

        [Test]
        public virtual void TestPropertyWithSimpleLookupList()
        {
            IPropDef def =
                _loader.LoadProperty(
                    @"
					<property  name=""TestProp"">
						<simpleLookupList>
							<item display=""s1"" value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
							<item display=""s2"" value=""{B89CC2C9-4CBB-4519-862D-82AB64796A58}"" />
						</simpleLookupList>
					</property>");
            Assert.IsInstanceOf(typeof (ISimpleLookupList), def.LookupList,
                           "LookupList should be of type SimpleLookupList");
            ISimpleLookupList source = (ISimpleLookupList) def.LookupList;
            Assert.AreEqual(2, source.GetLookupList().Count, "LookupList should have two keyvaluepairs");
        }

         
        [Test]
        public virtual void TestPropertyWithBusinessObjectLookupList()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"">
						<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" />
					</property>");
            Assert.IsNotNull(def.LookupList);
            Assert.IsInstanceOf(typeof(IBusinessObjectLookupList), def.LookupList);
            IBusinessObjectLookupList source = (IBusinessObjectLookupList)def.LookupList;
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
        }

        [Test]
        public virtual void TestLookupListLoaderConstructors()
        {
            XmlBusinessObjectLookupListLoader bollLoader = new XmlBusinessObjectLookupListLoader(new DtdLoader(), GetDefClassFactory());
            Assert.AreEqual(typeof(XmlBusinessObjectLookupListLoader), bollLoader.GetType());

            XmlDatabaseLookupListLoader dllLoader = new XmlDatabaseLookupListLoader(new DtdLoader(), GetDefClassFactory());
            Assert.AreEqual(typeof(XmlDatabaseLookupListLoader), dllLoader.GetType());
        }
    }
}
