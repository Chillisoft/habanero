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
        public void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();
        }

        protected void Initialise() {
            _loader = new XmlPropertyLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestSimpleProperty()
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoPropsInString()
        {
            _loader.LoadProperty(@"");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithNoName()
        {
            _loader.LoadProperty(@"<property />");
        }

        [Test]
        public void TestPropertyWithType()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" type=""Int32"" />");
            Assert.AreEqual("Int32", def.PropertyTypeName, "Property type should be same as that specified in xml");
            Assert.AreEqual("System", def.PropertyTypeAssemblyName, "Property type should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDescription()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" description=""Property for Testing"" />");
            Assert.AreEqual("Property for Testing", def.Description, "Property description should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDisplayValue()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" displayName=""Test Property"" />");
            Assert.AreEqual("Test Property", def.DisplayName, "Property description should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithKeepValuePrivate()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" keepValuePrivate=""true"" />");
            Assert.AreEqual(true, def.KeepValuePrivate, "Property 'keep value private' attribute should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithReadWriteRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadWrite"" />");
            Assert.AreEqual(PropReadWriteRule.ReadWrite, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithReadOnlyRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadOnly"" />");
            Assert.AreEqual(PropReadWriteRule.ReadOnly, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithWriteOnceRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteOnce"" />");
            Assert.AreEqual(PropReadWriteRule.WriteOnce, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithWriteNotNewRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteNotNew"" />");
            Assert.AreEqual(PropReadWriteRule.WriteNotNew, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithWriteNewRule()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteNew"" />");
            Assert.AreEqual(PropReadWriteRule.WriteNew, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithInvalidReadWriterule()
        {
            _loader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""invalid"" />");
        }

        [Test]
        public void TestPropertyWithDefaultValue()
        {
            IPropDef def = _loader.LoadProperty(@"<property  name=""TestProp"" default=""TestValue"" />");
            Assert.AreEqual("TestValue", def.DefaultValue, "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithGuidDefaultValue()
        {
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""Guid"" default=""{38373667-B06A-40c5-B4CE-299CE925E121}"" />");
            Assert.AreEqual(new Guid("{38373667-B06A-40c5-B4CE-299CE925E121}"), new Guid(def.DefaultValue.ToString()),
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDateTimeDefaultValueToday()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""Today"" />");
            Assert.AreEqual("Today", def.DefaultValue,
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDateTimeDefaultValueNow()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""Now"" />");
            Assert.AreEqual("Now", def.DefaultValue,
                "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDatabaseFieldName()
        {
            IPropDef def =
                _loader.LoadProperty(@"<property  name=""TestProp"" databaseField=""TestFieldName"" />");
            Assert.AreEqual("TestFieldName", def.DatabaseFieldName, "Field Name should be the same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDatabaseFieldNameWithSpaces()
        {
            IPropDef def =
                _loader.LoadProperty(@"<property name=""TestProp"" databaseField=""Test FieldName"" />");
            Assert.AreEqual("Test FieldName", def.DatabaseFieldName);
        }

        [Test]
        public void TestPropertyWithAutoIncrementingField()
        {
            IPropDef def = _loader.LoadProperty(
                      @"<property name=""TestProp"" autoIncrementing=""true"" />");
            Assert.AreEqual(true, def.AutoIncrementing);
        }

        [Test]
        public void TestPropertyWithLength()
        {
            IPropDef def = _loader.LoadProperty(
                      @"<property name=""TestProp"" length=""5"" />");
            Assert.AreEqual(5, def.Length);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithInvalidLengthException()
        {
            _loader.LoadProperty(@"<property name=""TestProp"" length=""fff"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithNegativeLengthException()
        {
            _loader.LoadProperty(@"<property name=""TestProp"" length=""-1"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithLengthForNonString()
        {
            _loader.LoadProperty(@"<property name=""TestProp"" type=""bool"" length=""5"" />");
        }

        [Test]
        public void TestPropertyWithPropRule()
        {
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp""><rule name=""StringRule""><add key=""minLength"" value=""8""/><add key=""maxLength"" value=""8"" /></rule></property>");
            Assert.AreEqual(1, def.PropRules.Count);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithInvalidPropRule()
        {
            // this should not work as min is an invalid setting for a string rule.
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp""><rule name=""StringRule""><add key=""min"" value=""8""/></rule></property>");
            Assert.AreEqual(1, def.PropRules.Count);
            Assert.IsNotNull(def.PropRules[0]);
           
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
        public void TestPropertyWithDatabaseLookupList()
        {
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IPropDef def =
                _loader.LoadProperty(
                    @"<property  name=""TestProp""><databaseLookupList sql=""Source"" timeout=""100"" class=""MyBO"" assembly=""Habanero.Test"" /></property>");
            Assert.IsInstanceOfType(typeof (IDatabaseLookupList), def.LookupList,
                           "LookupList should be of type IDatabaseLookupList but is of type " +
                           def.LookupList.GetType().Name);
            IDatabaseLookupList source = (IDatabaseLookupList) def.LookupList;
            Assert.AreEqual("Source", source.SqlString, "LookupList should be the same as that specified in xml");
            Assert.AreEqual(100, source.TimeOut);
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
        }

        

        [Test]
        public void TestPropertyWithSimpleLookupList()
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
            Assert.IsInstanceOfType(typeof (ISimpleLookupList), def.LookupList,
                           "LookupList should be of type SimpleLookupList");
            ISimpleLookupList source = (ISimpleLookupList) def.LookupList;
            Assert.AreEqual(2, source.GetLookupList().Count, "LookupList should have two keyvaluepairs");
        }

         
        [Test]
        public void TestPropertyWithBusinessObjectLookupList()
        {
            IPropDef def = _loader.LoadProperty(
                    @"<property  name=""TestProp"">
						<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" />
					</property>");
            Assert.IsNotNull(def.LookupList);
            Assert.IsInstanceOfType(typeof(IBusinessObjectLookupList), def.LookupList);
            IBusinessObjectLookupList source = (IBusinessObjectLookupList)def.LookupList;
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
        }

        [Test]
        public void TestLookupListLoaderConstructors()
        {
            XmlBusinessObjectLookupListLoader bollLoader = new XmlBusinessObjectLookupListLoader(new DtdLoader(), GetDefClassFactory());
            Assert.AreEqual(typeof(XmlBusinessObjectLookupListLoader), bollLoader.GetType());

            XmlDatabaseLookupListLoader dllLoader = new XmlDatabaseLookupListLoader(new DtdLoader(), GetDefClassFactory());
            Assert.AreEqual(typeof(XmlDatabaseLookupListLoader), dllLoader.GetType());
        }
    }
}
