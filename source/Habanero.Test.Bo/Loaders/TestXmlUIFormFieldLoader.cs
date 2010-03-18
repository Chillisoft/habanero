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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Util;
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
        public virtual void SetupTest()
        {
            Initialise();
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected  void Initialise() {
            loader = new XmlUIFormFieldLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(
                    @"<field label=""testlabel"" property=""testpropname"" type=""TextBox"" assembly=""System.Windows.Forms"" mapperType=""TextBoxMapper"" mapperAssembly=""Habanero.UI.Base"" editable=""false"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("TextBox", uiProp.ControlTypeName);
            Assert.AreEqual("System.Windows.Forms", uiProp.ControlAssemblyName);
            Assert.AreEqual("Habanero.UI.Base", uiProp.MapperAssembly);
            Assert.AreEqual("TextBoxMapper", uiProp.MapperTypeName);
            Assert.AreEqual(false, uiProp.Editable);
        }

        [Test]
        public void Test_LoadUIProperty_WithCustomControlType()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(@"<field property=""testpropname"" type=""MyCustomType"" assembly=""MyCustomAssembly"" />");
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("MyCustomType", uiProp.ControlTypeName);
            Assert.AreEqual("MyCustomAssembly", uiProp.ControlAssemblyName);
        }

        [Test]
        public void Test_LoadUIProperty_WithCustomMapperType()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(@"<field property=""testpropname"" mapperType=""MyCustomType"" mapperAssembly=""MyCustomAssembly"" />");
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual("MyCustomType", uiProp.MapperTypeName);
            Assert.AreEqual("MyCustomAssembly", uiProp.MapperAssembly);
        }

        [Test]
        public void TestDefaults()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(@"<field label=""testlabel"" property=""testpropname"" />");
            Assert.AreEqual("testlabel", uiProp.Label);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual(true, uiProp.Editable);
            Assert.AreEqual(null, uiProp.ToolTipText);
            //Assert.AreEqual(0, uiProp.Triggers.Count);
        }

		// Deciding default types/mappers must be done by the appropriate UI layer
        [Test]
        public void TestTypeDefaultsNotSpecified()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(@"<field label=""testlabel"" property=""testpropname"" />");
            Assert.IsTrue(string.IsNullOrEmpty(uiProp.ControlTypeName));
            Assert.IsTrue(string.IsNullOrEmpty(uiProp.ControlAssemblyName));
            Assert.IsTrue(string.IsNullOrEmpty(uiProp.MapperTypeName));
            Assert.IsTrue(string.IsNullOrEmpty(uiProp.MapperAssembly));
        }

        [Test]
        public void TestToolTip()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(@"<field property=""testpropname"" toolTipText=""My Tool Tip"" />");

            Assert.AreEqual("My Tool Tip", uiProp.ToolTipText);
        }
        [Test]
        public virtual void Test_Load_WhenShowAsCompulsorySet()
        {
            IUIFormField uiProp = loader.LoadUIProperty(@"<field property=""testpropname"" showAsCompulsory=""true"" />");
            bool? privatePropertyValue = (bool?) ReflectionUtilities.GetPrivatePropertyValue(uiProp, "ShowAsCompulsory");
            Assert.IsTrue(privatePropertyValue.Value);
            Assert.IsTrue(uiProp.IsCompulsory);
        }
        [Test]
        public virtual void Test_Load_WhenShowAsCompulsoryNotSet()
        {
            IUIFormField uiProp = loader.LoadUIProperty(@"<field property=""testpropname"" />");
            bool? privatePropertyValue = (bool?)ReflectionUtilities.GetPrivatePropertyValue(uiProp, "ShowAsCompulsory");
            Assert.IsFalse(privatePropertyValue.GetValueOrDefault());
            Assert.IsFalse(uiProp.IsCompulsory);
        }

        [Test]
        public void TestPropertyAttributes()
        {
            IUIFormField uiProp =
                loader.LoadUIProperty(
                    @"<field label=""testlabel"" property=""testpropname"" ><parameter name=""TestAtt"" value=""TestValue"" /><parameter name=""TestAtt2"" value=""TestValue2"" /></field>");
            Assert.AreEqual("TestValue", uiProp.GetParameterValue("TestAtt"));
            Assert.AreEqual("TestValue2", uiProp.GetParameterValue("TestAtt2"));
        }

        [Test]
        public void TestInvalidEditableValue()
        {
            try
            {
                loader.LoadUIProperty(@"<field property=""testpropname"" editable=""123"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'editable' attribute in a 'field' element is invalid. The valid options are 'true' and 'false'", ex.Message);
            }
        }

        [Test]
        public void TestDuplicateParameters()
        {
            try
            {
                loader.LoadUIProperty(@"
                <field property=""testpropname"" >
                    <parameter name=""TestAtt"" value=""TestValue"" />
                    <parameter name=""TestAtt"" value=""TestValue2"" />
                </field>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An error occurred while loading a 'parameter' element.  There may be duplicates with the same 'name' attribute", ex.Message);
            }
        }

        [Test]
        public void TestParameterMissingName()
        {
            try
            {
                loader.LoadUIProperty(@"
                <field property=""testpropname"" >
                    <parameter value=""TestValue"" />
                </field>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'parameter' element, either the 'name' or 'value' attribute has been omitted", ex.Message);
            }
        }

        [Test]
        public void TestParameterMissingValue()
        {
            try
            {
                loader.LoadUIProperty(@"
                <field property=""testpropname"" >
                    <parameter name=""TestAtt"" />
                </field>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'parameter' element, either the 'name' or 'value' attribute has been omitted", ex.Message);
            }
        }

        [Test]
        public void TestTriggers()
        {
//            UIFormField field =
//                loader.LoadUIProperty(@"<field property=""prop""><trigger action=""action"" value=""value"" /></field>");
//            Assert.AreEqual(1, field.Triggers.Count);
//            Assert.AreEqual("action", field.Triggers[0].Action);

//            loader = new XmlUIFormFieldLoader();
//            field = loader.LoadUIProperty(@"
//                <field property=""prop"">
//                    <trigger action=""action1"" value=""value"" />
//                    <trigger action=""action2"" value=""value2"" />
//                </field>");
//            Assert.AreEqual(2, field.Triggers.Count);
//            Assert.AreEqual("action1", field.Triggers[0].Action);
//            Assert.AreEqual("action2", field.Triggers[1].Action);
        }

//        [Test]
//        public void TestTriggersAndParameters()
//        {
//            UIFormField field = loader.LoadUIProperty(@"
//                <field property=""prop"">
//                    <parameter name=""TestAtt"" value=""TestValue"" />
//                    <parameter name=""TestAtt2"" value=""TestValue"" />
//                    <trigger action=""action"" value=""value"" />
//                    <trigger action=""action2"" value=""value2"" />                    
//                </field>");
//            Assert.AreEqual(2, field.Triggers.Count);
//            Assert.AreEqual("action", field.Triggers[0].Action);
//            Assert.AreEqual("action2", field.Triggers[1].Action);
//            Assert.AreEqual(2, field.Parameters.Count);
//        }

        [Test]
        public void TestLayoutStyle_Default()
        {
            //---------------Set up test pack-------------------
            loader = new XmlUIFormFieldLoader(new DtdLoader(), GetDefClassFactory());
            //---------------Execute Test ----------------------
            IUIFormField field = loader.LoadUIProperty(@"<field property=""prop"" />");

            //---------------Test Result -----------------------
            Assert.AreEqual(LayoutStyle.Label, field.Layout);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLayoutStyle()
        {
            //---------------Set up test pack-------------------
            loader = new XmlUIFormFieldLoader(new DtdLoader(), GetDefClassFactory());
            //---------------Execute Test ----------------------
            IUIFormField field = loader.LoadUIProperty(@"<field property=""prop"" layout=""GroupBox"" />");

            //---------------Test Result -----------------------
            Assert.AreEqual(LayoutStyle.GroupBox, field.Layout);
            //---------------Tear Down -------------------------          
        }       
        
        [Test]
        public void TestLayoutStyle_Invalid()
        {
            //---------------Set up test pack-------------------
            loader = new XmlUIFormFieldLoader(new DtdLoader(), GetDefClassFactory());
            //---------------Execute Test ----------------------
            try
            {
                IUIFormField field = loader.LoadUIProperty(@"<field property=""prop"" layout=""Invalid"" />");
                Assert.Fail("Invalid layout should raise an error");
            } 
            //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In the definition for the field 'prop' the 'layout' " +
                   "was set to an invalid value ('Invalid'). The valid options are " +
                   "Label and GroupBox.", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }
    }
}