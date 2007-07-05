using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
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
        }


        [Test]
        public void TestPropertyWithType()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" type=""Int32"" />");
            Assert.AreEqual(typeof (int), def.PropertyType, "Property type should be same as that specified in xml");
        }

        [Test]
        public void TestPropertyWithReadWriteRule()
        {
            PropDef def = itsLoader.LoadProperty(@"<property  name=""TestProp"" readWriteRule=""ReadOnly"" />");
            Assert.AreEqual(PropReadWriteRule.ReadOnly, def.ReadWriteRule,
                            "Property read write rule should be same as that specified in xml");
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
        public void TestPropertyWithDatabaseFieldName()
        {
            PropDef def =
                itsLoader.LoadProperty(@"<property  name=""TestProp"" databaseField=""TestFieldName"" />");
            Assert.AreEqual("TestFieldName", def.FieldName, "Field Name should be the same as that specified in xml");
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
        public void TestPropertyWithDatabaseLookupListSource()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp""><databaseLookupList sql=""Source"" /></property>");
            Assert.AreSame(typeof (DatabaseLookupListSource), def.LookupListSource.GetType(),
                           "LookupListSource should be of type DatabaseLookupList but is of type " +
                           def.LookupListSource.GetType().Name);
            DatabaseLookupListSource source = (DatabaseLookupListSource) def.LookupListSource;
            Assert.AreEqual("Source", source.SqlString, "LookupListSource should be the same as that specified in xml");
            Assert.IsNull(source.ClassDef);
        }

        [Test]
        public void TestDatabaseLookupListSourceWithClassDef()
        {
        	ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();

            ClassDef classDef = MyBo.LoadDefaultClassDef();

			PropDef def =
                itsLoader.LoadProperty(
                    @"<property  name=""TestProp""><databaseLookupList sql=""Source"" class=""MyBo"" assembly=""Habanero.Test"" /></property>");
            DatabaseLookupListSource source = (DatabaseLookupListSource) def.LookupListSource;
            Assert.IsNotNull(source.ClassDef);
            Assert.AreEqual(classDef.ClassName, source.ClassDef.ClassName);
        }


        [Test]
        public void TestPropertyWithSimpleLookupListSource()
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
            Assert.AreSame(typeof (SimpleLookupListSource), def.LookupListSource.GetType(),
                           "LookupListSource should be of type SimpleLookupListSource");
            SimpleLookupListSource source = (SimpleLookupListSource) def.LookupListSource;
            Assert.AreEqual(2, source.GetLookupList().Count, "LookupList should have two keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListSourceOptions()
        {
            PropDef def =
                itsLoader.LoadProperty(
                    @"
					<property  name=""TestProp"">
						<simpleLookupList options=""option1|option2|option3"" />
					</property>");
            SimpleLookupListSource source = (SimpleLookupListSource)def.LookupListSource;
            Assert.AreEqual(3, source.GetLookupList().Count, "LookupList should have three keyvaluepairs");
        }

        [Test]
        public void TestSimpleLookupListSourceOptionsAndItems()
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
            SimpleLookupListSource source = (SimpleLookupListSource)def.LookupListSource;
            Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
        }


    }
}