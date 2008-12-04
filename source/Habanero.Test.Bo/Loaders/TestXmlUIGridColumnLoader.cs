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

using System.Windows.Forms;
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
        public void SetupTest()
        {
            loader = new XmlUIGridColumnLoader();
        }

        [Test]
        public void TestDefaults()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" />");
            Assert.AreEqual(100, uiProp.Width);
        }

        [Test]
        public void TestSimpleUIProperty()
        {
            UIGridColumn uiProp =
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
            UIGridColumn uiProp =
               loader.LoadUIProperty(
                   @"<column heading=""testheading"" property=""testpropname""   />");
            //---------------Verify test pack-------------------

            //---------------Execute Test ----------------------
            //---------------Verify Result -----------------------
            Assert.IsNull(uiProp.GridControlType);
            Assert.IsNull(uiProp.GridControlTypeName); 
            Assert.IsNull(uiProp.GridControlAssemblyName);
            //---------------Tear Down -------------------------  

        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoPropertyName()
        {
            loader.LoadUIProperty(@"<column />");
        }
       
        [Test]
        public void TestCustomColumnType()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" type=""MyBO"" assembly=""Habanero.Test"" />");
            Assert.IsNull(uiProp.GridControlType);
            Assert.AreEqual("MyBO",uiProp.GridControlTypeName);
            Assert.AreEqual("Habanero.Test", uiProp.GridControlAssemblyName);
        }

        [Test]
        public void TestAutomaticHeadingCreation()
        {
            UIGridColumn uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" />");
            Assert.AreEqual(null, uiProp.Heading);
            Assert.AreEqual("testpropname", uiProp.GetHeading());

            uiProp = loader.LoadUIProperty(@"<column property=""TestPropName"" />");
            Assert.AreEqual(null, uiProp.Heading);
            Assert.AreEqual("Test Prop Name", uiProp.GetHeading());
        }

        [Test]
        public void TestAlignment()
        {
            UIGridColumn uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" />");
            Assert.AreEqual(UIGridColumn.PropAlignment.left, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""left"" />");
            Assert.AreEqual(UIGridColumn.PropAlignment.left, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""right"" />");
            Assert.AreEqual(UIGridColumn.PropAlignment.right, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""centre"" />");
            Assert.AreEqual(UIGridColumn.PropAlignment.centre, uiProp.Alignment);

            uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""center"" />");
            Assert.AreEqual(UIGridColumn.PropAlignment.centre, uiProp.Alignment);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidAlignmentValue()
        {
            loader.LoadUIProperty(@"<column property=""testpropname"" alignment=""123"" />");
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidEditableValue()
        {
            loader.LoadUIProperty(@"<column property=""testpropname"" editable=""123"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidWidthValue()
        {
            loader.LoadUIProperty(@"<column property=""testpropname"" width=""abc"" />");
        }

        [Test]
        public void TestPropertyAttributes()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(
                    @"<column heading=""testlabel"" property=""testpropname"" ><parameter name=""TestAtt"" value=""TestValue"" /><parameter name=""TestAtt2"" value=""TestValue2"" /></column>");
            Assert.AreEqual("TestValue", uiProp.GetParameterValue("TestAtt"));
            Assert.AreEqual("TestValue2", uiProp.GetParameterValue("TestAtt2"));
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestParameterWithNoName()
        {
            loader.LoadUIProperty(@"
                <column property=""testpropname"" >
                    <parameter value=""left"" />
                </column>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestParameterWithNoValue()
        {
            loader.LoadUIProperty(@"
                <column property=""testpropname"" >
                    <parameter name=""alignment"" />
                </column>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDuplicateParameterNames()
        {
            loader.LoadUIProperty(@"
                <column property=""testpropname"" >
                    <parameter name=""alignment"" value=""left"" />
                    <parameter name=""alignment"" value=""right"" />
                </column>");
        }
    }
}