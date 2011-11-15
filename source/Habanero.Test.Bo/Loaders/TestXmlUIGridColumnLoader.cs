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
using System.Windows.Forms;
using Habanero.Base;
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
    public class TestXmlUIGridColumnLoader
    {
        private XmlUIGridColumnLoader loader;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected void Initialise() {
            loader = new XmlUIGridColumnLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestDefaults()
        {
            IUIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" />");
            Assert.AreEqual(100, uiProp.Width);
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            IUIGridColumn uiProp =
                loader.LoadUIProperty(
                    @"<column heading=""testheading"" property=""testpropname"" type=""DataGridViewCheckBoxColumn"" width=""40"" />");
            Assert.AreEqual("testheading", uiProp.Heading);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual(40, uiProp.Width);
        }

        [Test]
        public void TestNoDefaultColumnType()
        {
            //---------------Set up test pack-------------------
            IUIGridColumn uiProp =
               loader.LoadUIProperty(
                   @"<column heading=""testheading"" property=""testpropname""   />");
            //---------------Verify test pack-------------------

            //---------------Execute Test ----------------------
            //---------------Verify Result -----------------------
            Assert.IsTrue(string.IsNullOrEmpty(uiProp.GridControlTypeName)); 
           Assert.IsTrue(string.IsNullOrEmpty(uiProp.GridControlAssemblyName));
            //---------------Tear Down -------------------------  

        }

        [Test]
        public void TestNoPropertyName()
        {
            try
            {
                loader.LoadUIProperty(@"<column />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'column' element, the 'property' attribute was not specified. This attribute specifies which property of the class to ", ex.Message);
            }
        }

        [Test]
        public void TestCustomColumnType()
        {
            IUIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" type=""DataGridViewComboBoxColumn"" assembly=""System.Windows.Forms"" />");
            Assert.AreEqual("DataGridViewComboBoxColumn", uiProp.GridControlTypeName);
            Assert.AreEqual("System.Windows.Forms", uiProp.GridControlAssemblyName);
        }

        [Test]
        public void Test_LoadUIProperty_WithCustomUnknownColumnType()
        {
            IUIGridColumn uiProp =
                loader.LoadUIProperty(@"<column property=""testpropname"" type=""MyColumnType"" assembly=""MyAssembly"" />");
            Assert.AreEqual("MyColumnType", uiProp.GridControlTypeName);
            Assert.AreEqual("MyAssembly", uiProp.GridControlAssemblyName);
        }

        [Test]
        public void TestAlignment()
        {
            IUIGridColumn uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" />");
            Assert.AreEqual(PropAlignment.left, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""left"" />");
            Assert.AreEqual(PropAlignment.left, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""right"" />");
            Assert.AreEqual(PropAlignment.right, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""centre"" />");
            Assert.AreEqual(PropAlignment.centre, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""center"" />");
            Assert.AreEqual(PropAlignment.centre, uiProp.Alignment);
        }

        [Test]
        public void TestInvalidAlignmentValue()
        {
            try
            {
                loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""123"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'column' node does not conform to its Document Type Definition (DTD). '123' is not in the enumeration list", ex.Message);
            }
        }

        //-- TODO Create Equivalence of these two in the UI assemblies

        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestInvalidAssemblyAttribute()
        //{
        //    loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" assembly=""testx"" />");
        //}

        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestInvalidColumnType()
        //{
        //    loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" type=""testx"" assembly=""System.Windows.Forms"" />");
        //}

        [Test]
        public void TestInvalidEditableValue()
        {
            try
            {
                loader.LoadUIProperty(@"<column property=""testpropname"" editable=""123"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'editable' attribute in a 'column' element is invalid. The valid options are 'true' and 'false'", ex.Message);
            }
        }

        [Test]
        public void TestInvalidWidthValue()
        {
            try
            {
                loader.LoadUIProperty(@"<column property=""testpropname"" width=""abc"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In the 'width' attribute of a 'column' element, the value provided was invalid.  This should be an integer value in pixels", ex.Message);
            }
        }

        [Test]
        public void TestPropertyAttributes()
        {
            IUIGridColumn uiProp =
                loader.LoadUIProperty(
                    @"<column heading=""testlabel"" property=""testpropname"" ><parameter name=""TestAtt"" value=""TestValue"" /><parameter name=""TestAtt2"" value=""TestValue2"" /></column>");
            Assert.AreEqual("TestValue", uiProp.GetParameterValue("TestAtt"));
            Assert.AreEqual("TestValue2", uiProp.GetParameterValue("TestAtt2"));
        }

        [Test]
        public void TestParameterWithNoName()
        {
            try
            {
                loader.LoadUIProperty(@"
                <column property=""testpropname"" >
                    <parameter value=""left"" />
                </column>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'parameter' element, either the 'name' or 'value' attribute has been omitted", ex.Message);
            }
        }

        [Test]
        public void TestParameterWithNoValue()
        {
            try
            {
                loader.LoadUIProperty(@"
                <column property=""testpropname"" >
                    <parameter name=""alignment"" />
                </column>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'parameter' element, either the 'name' or 'value' attribute has been omitted", ex.Message);
            }
        }

        [Test]
        public void TestDuplicateParameterNames()
        {
            try
            {
                loader.LoadUIProperty(@"
                <column property=""testpropname"" >
                    <parameter name=""alignment"" value=""left"" />
                    <parameter name=""alignment"" value=""right"" />
                </column>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An error occurred while loading a 'parameter' element.  There may be duplicates with the same 'name' attribute", ex.Message);
            }
        }
    }
}