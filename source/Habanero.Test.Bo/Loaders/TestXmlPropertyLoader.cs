//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.BO;
using Habanero.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPropertyLoader
    {
        private XmlPropertyLoader itsLoader;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlPropertyLoader();
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestSimpleProperty()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" />");
            Assert.AreEqual("TestProp", def.PropertyName, "Property name should be same as that specified in xml");
            Assert.AreEqual(typeof (string), def.PropertyType,
                            "Property type should be the default as defined in the dtd");
            Assert.AreEqual(null, def.DefaultValue, "The default default is null");
            Assert.AreEqual("TestProp", def.FieldName,
                            "The field name should be the same as the property name by default");
            Assert.AreEqual(false, def.AutoIncrementing, "autoIncrementing should be false by default");
            Assert.AreEqual(null, def.DisplayName, "The display name is null");
            Assert.AreEqual(null, def.Description, "The description is null");
            Assert.AreEqual(false, def.KeepValuePrivate, "keepValuePrivate should be false by default");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoPropsInString()
        {
            itsLoader.LoadProperty(@"");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithNoName()
        {
            itsLoader.LoadProperty(@"<property />");
        }

        [Test]
        public void TestPropertyWithType()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" type=""Int32"" />");
            Assert.AreEqual(typeof (int), def.PropertyType, "Property type should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDescription()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" description=""Property for Testing"" />");
            Assert.AreEqual("Property for Testing", def.Description, "Property description should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDisplayValue()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" displayName=""Test Property"" />");
            Assert.AreEqual("Test Property", def.DisplayName, "Property description should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithKeepValuePrivate()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" keepValuePrivate=""true"" />");
            Assert.AreEqual(true, def.KeepValuePrivate, "Property 'keep value private' attribute should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithReadWriteRule()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadWrite"" />");
            Assert.AreEqual(PropReadWriteRule.ReadWrite, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithReadOnlyRule()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadOnly"" />");
            Assert.AreEqual(PropReadWriteRule.ReadOnly, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithWriteOnceRule()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteOnce"" />");
            Assert.AreEqual(PropReadWriteRule.WriteOnce, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithWriteNotNewRule()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteNotNew"" />");
            Assert.AreEqual(PropReadWriteRule.WriteNotNew, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithWriteNewRule()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""WriteNew"" />");
            Assert.AreEqual(PropReadWriteRule.WriteNew, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithInvalidReadWriterule()
        {
            itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""invalid"" />");
        }

        [Test]
        public void TestPropertyWithDefaultValue()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" default=""TestValue"" />");
            Assert.AreEqual("TestValue", def.DefaultValue, "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithGuidDefaultValue()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp"" type=""Guid"" default=""{38373667-B06A-40c5-B4CE-299CE925E121}"" />");
            Assert.AreEqual(new Guid("{38373667-B06A-40c5-B4CE-299CE925E121}"), def.DefaultValue,
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDateTimeDefaultValueToday()
        {
            PropDef def = itsLoader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""Today"" />");
            Assert.AreEqual(DateTime.Today, def.DefaultValue,
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDateTimeDefaultValueNow()
        {
            PropDef def = itsLoader.LoadProperty(
                    @"<property  name=""TestProp"" type=""DateTime"" default=""Now"" />");
            DateTime nowBefore = DateTime.Now;
            DateTime defaultValue = Convert.ToDateTime(def.DefaultValue);
            DateTime nowAfter = DateTime.Now;
            Assert.IsTrue(nowBefore <= defaultValue && defaultValue <= nowAfter,
                            "Default value should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDatabaseFieldName()
        {
            PropDef def =
                itsLoader.LoadProperty(@"<property  name=""TestProp"" databaseField=""TestFieldName"" />");
            Assert.AreEqual("TestFieldName", def.FieldName, "Field Name should be the same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithDatabaseFieldNameWithSpaces()
        {
            PropDef def =
                itsLoader.LoadProperty(@"<property name=""TestProp"" databaseField=""Test FieldName"" />");
            Assert.AreEqual("Test FieldName", def.FieldName);
        }

        [Test]
        public void TestPropertyWithPropRule()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp""><rule name=""StringRule""><add key=""minLength"" value=""8""/><add key=""maxLength"" value=""8"" /></rule></property>");
            Assert.AreEqual("PropRuleString", def.PropRule.GetType().Name);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropertyWithInvalidPropRule()
        {
            // this should not work as min is an invalid setting for a string rule.
            PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp""><rule name=""StringRule""><add key=""min"" value=""8""/></rule></property>");

            Assert.IsNotNull(def.PropRule);
           
        }

        [Test]
        public void TestPropertyWithDatabaseLookupList()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp""><databaseLookupList sql=""Source"" timeout=""100"" class=""MyBO"" assembly=""Habanero.Test"" /></property>");
            Assert.AreSame(typeof (DatabaseLookupList), def.LookupList.GetType(),
                           "LookupList should be of type DatabaseLookupList but is of type " +
                           def.LookupList.GetType().Name);
            DatabaseLookupList source = (DatabaseLookupList) def.LookupList;
            Assert.AreEqual("Source", source.SqlString, "LookupList should be the same as that specified in xml");
            Assert.AreEqual(100, source.TimeOut);
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
            Assert.IsNull(source.ClassDef);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDatabaseLookupListWithInvalidTimeout()
        {
            itsLoader.LoadProperty(
                    @"<property name=""TestProp""><databaseLookupList sql=""Source"" timeout=""aaa"" /></property>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDatabaseLookupListWithNegativeTimeout()
        {
            itsLoader.LoadProperty(
                    @"<property name=""TestProp""><databaseLookupList sql=""Source"" timeout=""-1"" /></property>");
        }

        [Test]
        public void TestDatabaseLookupListWithClassDef()
        {
        	ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();

			PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp""><databaseLookupList sql=""Source"" class=""MyBO"" assembly=""Habanero.Test"" /></property>");
            DatabaseLookupList source = (DatabaseLookupList) def.LookupList;
            Assert.IsNotNull(source.ClassDef);
            Assert.AreEqual(classDef.ClassName, source.ClassDef.ClassName);
            Assert.AreEqual(10000, source.TimeOut);
        }


        [Test]
        public void TestPropertyWithSimpleLookupList()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"
					<property  name=""TestProp"">
						<simpleLookupList>
							<item display=""s1"" value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
							<item display=""s2"" value=""{B89CC2C9-4CBB-4519-862D-82AB64796A58}"" />
						</simpleLookupList>
					</property>");
            Assert.AreSame(typeof (SimpleLookupList), def.LookupList.GetType(),
                           "LookupList should be of type SimpleLookupList");
            SimpleLookupList source = (SimpleLookupList) def.LookupList;
            Assert.AreEqual(2, source.GetLookupList().Count, "LookupList should have two keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListOptions()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"
					<property  name=""TestProp"">
						<simpleLookupList options=""option1|option2|option3"" />
					</property>");
            SimpleLookupList source = (SimpleLookupList)def.LookupList;
            Assert.AreEqual(3, source.GetLookupList().Count, "LookupList should have three keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListOptionsAndItems()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"
					<property  name=""TestProp"">
						<simpleLookupList options=""option1|option2|option3"" >
							<item display=""s1"" value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
							<item display=""s2"" value=""{B89CC2C9-4CBB-4519-862D-82AB64796A58}"" />
                        </simpleLookupList>
					</property>");
            SimpleLookupList source = (SimpleLookupList)def.LookupList;
            Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSimpleLookupListNoItems()
        {
            PropDef def = itsLoader.LoadProperty(@"
				<property  name=""TestProp"">
					<simpleLookupList></simpleLookupList>
				</property>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSimpleLookupListItemHasNoDisplay()
        {
            PropDef def = itsLoader.LoadProperty(@"
				<property  name=""TestProp"">
					<simpleLookupList>
						<item value=""{C2887FB1-7F4F-4534-82AB-FED92F954783}"" />
                    </simpleLookupList>
				</property>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSimpleLookupListItemHasNoValue()
        {
            PropDef def = itsLoader.LoadProperty(@"
				<property  name=""TestProp"">
					<simpleLookupList>
						<item display=""s1"" />
                    </simpleLookupList>
				</property>");
        }

        [Test]
        public void TestBusinessObjectLookupList()
        {
            PropDef def = itsLoader.LoadProperty(
                    @"<property  name=""TestProp"">
						<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" />
					</property>");
            Assert.AreSame(typeof(BusinessObjectLookupList), def.LookupList.GetType(),
                           "LookupList should be of type BusinessObjectLookupList but is of type " +
                           def.LookupList.GetType().Name);
            BusinessObjectLookupList source = (BusinessObjectLookupList)def.LookupList;
            //Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
            Assert.AreEqual(null, source.Criteria);
        }

        [Test]
        public void TestBusinessObjectLookupListWithCriteria()
        {
            PropDef def = itsLoader.LoadProperty(
                    @"<property  name=""TestProp"">
						<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" criteria=""Test"" />
					</property>");
            BusinessObjectLookupList source = (BusinessObjectLookupList)def.LookupList;
            Assert.AreEqual("Test", source.Criteria);
        }

        [Test]
        public void TestBusinessObjectLookupListWithSort()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();

            PropDef def = itsLoader.LoadProperty(
                    @"<property  name=""TestProp"">
						<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" sort=""TestProp asc"" />
					</property>");
            BusinessObjectLookupList source = (BusinessObjectLookupList)def.LookupList;
            Assert.AreEqual("TestProp asc", source.Sort);
        }

        [Test]
        public void TestBusinessObjectWithAutoIncrementingField()
        {
            PropDef def = itsLoader.LoadProperty(
                      @"<property name=""TestProp"" autoIncrementing=""true"" />");
            Assert.AreEqual(true, def.AutoIncrementing);
        }

        [Test]
        public void TestBusinessObjectWithLength()
        {
            PropDef def = itsLoader.LoadProperty(
                      @"<property name=""TestProp"" length=""5"" />");
            Assert.AreEqual(5, def.Length);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestBusinessObjectWithInvalidLengthException()
        {
            itsLoader.LoadProperty(@"<property name=""TestProp"" length=""fff"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestBusinessObjectWithNegativeLengthException()
        {
            itsLoader.LoadProperty(@"<property name=""TestProp"" length=""-1"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestBusinessObjectWithLengthForNonString()
        {
            itsLoader.LoadProperty(@"<property name=""TestProp"" type=""bool"" length=""5"" />");
        }

        [Test]
        public void TestLookupListLoaderConstructors()
        {
            XmlBusinessObjectLookupListLoader bollLoader = new XmlBusinessObjectLookupListLoader();
            Assert.AreEqual(typeof(XmlBusinessObjectLookupListLoader), bollLoader.GetType());

            XmlDatabaseLookupListLoader dllLoader = new XmlDatabaseLookupListLoader();
            Assert.AreEqual(typeof(XmlDatabaseLookupListLoader), dllLoader.GetType());
        }
    }
}