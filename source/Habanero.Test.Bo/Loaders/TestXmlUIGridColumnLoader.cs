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

using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Base;
using Habanero.UI.Grid;
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
        public void TestSimpleUIProperty()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(
                    @"<column heading=""testheading"" property=""testpropname"" type=""DataGridViewCheckBoxColumn"" width=""40"" />");
            Assert.AreEqual("testheading", uiProp.Heading);
            Assert.AreEqual("testpropname", uiProp.PropertyName);
            Assert.AreEqual(40, uiProp.Width);
            Assert.AreSame(typeof (DataGridViewCheckBoxColumn), uiProp.GridControlType);
        }

        [Test]
        public void TestDefaults()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" />");
            Assert.AreSame(typeof (DataGridViewTextBoxColumn), uiProp.GridControlType);
            Assert.AreEqual(100, uiProp.Width);
        }

        [Test]
        public void TestAssemblyAttributeForSystem()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" assembly=""System.Windows.Forms"" />");
            Assert.AreEqual(typeof(DataGridViewTextBoxColumn), uiProp.GridControlType);
        }

        [Test]
        public void TestAssemblyAttributeForHabaneroTypes()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" type=""DataGridViewNumericUpDownColumn"" />");
            Assert.AreEqual(typeof(DataGridViewNumericUpDownColumn), uiProp.GridControlType);
        }

        [Test]
        public void TestCustomColumnType()
        {
            UIGridColumn uiProp =
                loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" type=""MyBO"" assembly=""Habanero.Test"" />");
            Assert.AreEqual(typeof(MyBO), uiProp.GridControlType);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidAssemblyAttribute()
        {
            loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" assembly=""testx"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidColumnType()
        {
            loader.LoadUIProperty(@"<column heading=""testheading"" property=""testpropname"" type=""testx"" assembly=""System.Windows.Forms"" />");
        }
    }
}