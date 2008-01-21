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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIGridColumn
    {
        [Test]
        public void TestParameters()
        {
            Hashtable parameters = new Hashtable();


            UIGridColumn column = new UIGridColumn("heading", null, null, true, 100,
                                                   UIGridColumn.PropAlignment.left, parameters);

            Assert.AreEqual(0, column.Parameters.Count);
            column.Parameters.Add("name", "value");
            Assert.IsNull(column.GetParameterValue("somename"));
            Assert.AreEqual("value", column.GetParameterValue("name"));
        }

        [Test]
        public void TestProtectedSets()
        {
            UIGridColumnInheritor column = new UIGridColumnInheritor();

            Assert.AreEqual("heading", column.Heading);
            column.SetHeading("newheading");
            Assert.AreEqual("newheading", column.Heading);

            Assert.IsNull(column.PropertyName);
            column.SetPropertyName("prop");
            Assert.AreEqual("prop", column.PropertyName);

            Assert.IsNull(column.GridControlType);
            column.SetGridControlType(typeof(DataGridViewTextBoxColumn));
            Assert.AreEqual(typeof(DataGridViewTextBoxColumn), column.GridControlType);

            Assert.IsTrue(column.Editable);
            column.SetEditable(false);
            Assert.IsFalse(column.Editable);

            Assert.AreEqual(100, column.Width);
            column.SetWidth(200);
            Assert.AreEqual(200, column.Width);

            Assert.AreEqual(UIGridColumn.PropAlignment.left, column.Alignment);
            column.SetAlignment(UIGridColumn.PropAlignment.right);
            Assert.AreEqual(UIGridColumn.PropAlignment.right, column.Alignment);
        }

        // Grants access to protected fields
        private class UIGridColumnInheritor : UIGridColumn
        {
            public UIGridColumnInheritor() : base("heading", null, null, true, 100,
                UIGridColumn.PropAlignment.left, null)
            {}

            public void SetHeading(string name)
            {
                Heading = name;
            }

            public void SetPropertyName(string name)
            {
                PropertyName = name;
            }

            public void SetGridControlType(Type type)
            {
                GridControlType = type;
            }

            public void SetEditable(bool editable)
            {
                Editable = editable;
            }

            public void SetWidth(int width)
            {
                Width = width;
            }

            public void SetAlignment(PropAlignment alignment)
            {
                Alignment = alignment;
            }
        }
    }
}