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


using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestTextBoxMapper : TestMapperBase
    {
        private TextBox tb;
        private TextBoxMapper mapper;
        private Shape sh;


        [SetUp]
        public void SetupTest()
        {
            tb = new TextBox();
            mapper = new TextBoxMapper(tb, "ShapeName", false);
            sh = new Shape();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(tb, mapper.Control);
            Assert.AreSame("ShapeName", mapper.PropertyName);
        }

        [Test]
        public void TestBusinessObject()
        {
            mapper.BusinessObject = sh;
            Assert.AreSame(sh, mapper.BusinessObject);
        }

        [Test]
        public void TestValueWhenSettingBO()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            Assert.AreEqual("TestShapeName", tb.Text, "TextBox value is not set when bo is set.");
        }

        [Test]
        public void TestValueWhenUpdatingBOValue()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            sh.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName2", tb.Text, "Text property of textbox is not working.");
        }


        [Test]
        public void TestSettingToAnotherBusinessObject()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            Shape sh2 = new Shape();
            sh2.ShapeName = "Different";
            mapper.BusinessObject = sh2;
            Assert.AreEqual("Different", tb.Text, "Setting to another bo doesn't work.");
            sh.ShapeName = "TestShapeName2";
            Assert.AreEqual("Different", tb.Text,
                            "Setting to another bo doesn't remove the property updating event handler of the first.");
        }

        [Test]
        public void TestSettingTextBoxValueUpdatesBO()
        {
            sh.ShapeName = "TestShapeName";
            mapper.BusinessObject = sh;
            tb.Text = "Changed";
            Assert.AreEqual("Changed", sh.ShapeName, "BO property value isn't changed when textbox text is changed.");
        }

        [Test]
        public void TestSettingTextBoxValueWhenBOIsNotSet()
        {
            sh.ShapeName = "TestShapeName";
            tb.Text = "Changed";
            Assert.AreEqual("TestShapeName", sh.ShapeName,
                            "BO property value shouldn't change since bo of mapper wasn't set.");
            Assert.AreEqual("Changed", tb.Text, "Textbox value shouldn't change.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("MyValue");
            mapper = new TextBoxMapper(tb, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual("MyValue", tb.Text);
        }
    }
}