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
    public class TestUIFormField
    {
        [Test]
        public void TestTriggerAccess()
        {
            UIFormField field = new UIFormField("label", "prop", "control", "ass", "mapper", "mapass",
                true, null, null);
            Assert.IsNotNull(field.Triggers);
            Assert.AreEqual(0, field.Triggers.Count);

            Trigger trigger = new Trigger("prop1", null, null, "action", "value");
            field.Triggers.Add(trigger);
            Assert.AreEqual(1, field.Triggers.Count);
        }

        [Test]
        public void TestProtectedSets()
        {
            UIFormFieldInheritor field = new UIFormFieldInheritor();
            
            Assert.AreEqual("label", field.Label);
            field.SetLabel("newlabel");
            Assert.AreEqual("newlabel", field.Label);

            Assert.AreEqual("prop", field.PropertyName);
            field.SetPropertyName("newprop");
            Assert.AreEqual("newprop", field.PropertyName);

            Assert.IsNull(field.MapperTypeName);
            field.SetMapperTypeName("mapper");
            Assert.AreEqual("mapper", field.MapperTypeName);

            Assert.IsNull(field.MapperAssembly);

            Assert.IsNull(field.ControlType);
            field.SetControlType(typeof(ComboBox));
            Assert.AreEqual(typeof(ComboBox), field.ControlType);

            Assert.AreEqual("System.Windows.Forms.ComboBox", field.ControlTypeName);
            field.SetControlTypeName("TextBox");
            Assert.AreEqual("TextBox", field.ControlTypeName);
            Assert.AreEqual(typeof(TextBox), field.ControlType);

            Assert.AreEqual("System.Windows.Forms", field.ControlAssemblyName);
            field.SetControlAssemblyName("assem");
            Assert.AreEqual("assem", field.ControlAssemblyName);
            Assert.IsNull(field.ControlType);
            Assert.IsNull(field.ControlTypeName);

            Assert.IsTrue(field.Editable);
            field.SetEditable(false);
            Assert.IsFalse(field.Editable);
        }

        // Grants access to protected fields
        private class UIFormFieldInheritor : UIFormField
        {
            public UIFormFieldInheritor()
                : base("label", "prop", "control", null, null, null, true, null, null)
            {}

            public void SetLabel(string name)
            {
                Label = name;
            }

            public void SetPropertyName(string name)
            {
                PropertyName = name;
            }

            public void SetMapperTypeName(string name)
            {
                MapperTypeName = name;
            }

            public void SetControlAssemblyName(string name)
            {
                ControlAssemblyName = name;
            }

            public void SetControlTypeName(string name)
            {
                ControlTypeName = name;
            }

            public void SetControlType(Type type)
            {
                ControlType = type;
            }

            public void SetEditable(bool editable)
            {
                Editable = editable;
            }
        }
    }
}