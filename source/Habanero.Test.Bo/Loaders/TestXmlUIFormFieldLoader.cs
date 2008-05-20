//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormFieldLoader
    {

        private XmlUIFormFieldLoader loader;


        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormFieldLoader();
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(
                    @"<field label=""testlabel"" property=""testpropname"" type=""Button"" assembly=""System.Windows.Forms"" mapperType=""testmappertypename"" mapperAssembly=""testmapperassembly"" editable=""false"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("Button", uiProp.ControlTypeName);
			Assert.AreEqual("System.Windows.Forms", uiProp.ControlAssemblyName);
            Assert.AreEqual("testmappertypename", uiProp.MapperTypeName);
            Assert.AreEqual("testmapperassembly", uiProp.MapperAssembly);
            Assert.AreEqual(false, uiProp.Editable);
        }

        [Test]
        public void TestDefaults()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(@"<field label=""testlabel"" property=""testpropname"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual(true, uiProp.Editable);
            Assert.AreEqual(null, uiProp.ToolTipText);
            Assert.AreEqual(0, uiProp.Triggers.Count);
        }

		// Deciding default types/mappers must be done by the appropriate UI layer
        [Test]
        public void TestTypeDefaultsNotSpecified()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(@"<field label=""testlabel"" property=""testpropname"" />");
            Assert.IsNull(uiProp.ControlTypeName);
            Assert.IsNull(uiProp.ControlAssemblyName);
            Assert.IsNull(uiProp.MapperTypeName);
            Assert.IsNull(uiProp.MapperAssembly);
        }

        [Test]
        public void TestToolTip()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(@"<field property=""testpropname"" toolTipText=""My Tool Tip"" />");
            Assert.AreEqual("My Tool Tip", uiProp.ToolTipText);
        }

        [Test]
        public void TestPropertyAttributes()
        {
            UIFormField uiProp =
                loader.LoadUIProperty(
                    @"<field label=""testlabel"" property=""testpropname"" ><parameter name=""TestAtt"" value=""TestValue"" /><parameter name=""TestAtt2"" value=""TestValue2"" /></field>");
            Assert.AreEqual("TestValue", uiProp.GetParameterValue("TestAtt"));
            Assert.AreEqual("TestValue2", uiProp.GetParameterValue("TestAtt2"));
        }

        [Test]
        public void TestAutomaticLabelCreation()
        {
            UIFormField uiProp = loader.LoadUIProperty(@"<field property=""testpropname"" />");
            Assert.AreEqual(null, uiProp.Label);
            Assert.AreEqual("testpropname:", uiProp.GetLabel());

            uiProp = loader.LoadUIProperty(@"<field property=""TestPropName"" />");
            Assert.AreEqual(null, uiProp.Label);
            Assert.AreEqual("Test Prop Name:", uiProp.GetLabel());

			uiProp = loader.LoadUIProperty(@"<field property=""TestPropName"" type=""CheckBox"" />");
            Assert.AreEqual(null, uiProp.Label);
            Assert.AreEqual("Test Prop Name?", uiProp.GetLabel());
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidEditableValue()
        {
            loader.LoadUIProperty(@"<field property=""testpropname"" editable=""123"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDuplicateParameters()
        {
            loader.LoadUIProperty(@"
                <field property=""testpropname"" >
                    <parameter name=""TestAtt"" value=""TestValue"" />
                    <parameter name=""TestAtt"" value=""TestValue2"" />
                </field>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestParameterMissingName()
        {
            loader.LoadUIProperty(@"
                <field property=""testpropname"" >
                    <parameter value=""TestValue"" />
                </field>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestParameterMissingValue()
        {
            loader.LoadUIProperty(@"
                <field property=""testpropname"" >
                    <parameter name=""TestAtt"" />
                </field>");
        }

        [Test]
        public void TestTriggers()
        {
            UIFormField field =
                loader.LoadUIProperty(@"<field property=""prop""><trigger action=""action"" value=""value"" /></field>");
            Assert.AreEqual(1, field.Triggers.Count);
            Assert.AreEqual("action", field.Triggers[0].Action);

            loader = new XmlUIFormFieldLoader();
            field = loader.LoadUIProperty(@"
                <field property=""prop"">
                    <trigger action=""action1"" value=""value"" />
                    <trigger action=""action2"" value=""value2"" />
                </field>");
            Assert.AreEqual(2, field.Triggers.Count);
            Assert.AreEqual("action1", field.Triggers[0].Action);
            Assert.AreEqual("action2", field.Triggers[1].Action);
        }

        [Test]
        public void TestTriggersAndParameters()
        {
            UIFormField field = loader.LoadUIProperty(@"
                <field property=""prop"">
                    <parameter name=""TestAtt"" value=""TestValue"" />
                    <parameter name=""TestAtt2"" value=""TestValue"" />
                    <trigger action=""action"" value=""value"" />
                    <trigger action=""action2"" value=""value2"" />                    
                </field>");
            Assert.AreEqual(2, field.Triggers.Count);
            Assert.AreEqual("action", field.Triggers[0].Action);
            Assert.AreEqual("action2", field.Triggers[1].Action);
            Assert.AreEqual(2, field.Parameters.Count);
        }
    }
}