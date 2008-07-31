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

using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Base;
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
                true, null, null, null);
            Assert.IsNotNull(field.Triggers);
            Assert.AreEqual(0, field.Triggers.Count);

            Trigger trigger = new Trigger("prop1", null, null, "action", "value");
            field.Triggers.Add(trigger);
            Assert.AreEqual(1, field.Triggers.Count);
        }

        [Test]
        public void TestFieldDefaultLabel()
        {
            UIFormField uiFormField;
            uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, null, null, null);
            Assert.AreEqual("Test Property:", uiFormField.GetLabel());
            uiFormField = new UIFormField(null, "TestProperty", typeof(CheckBox), null, null, true, null, null, null);
            Assert.AreEqual("Test Property?", uiFormField.GetLabel());
        }

        [Test]
        public void TestFieldDefaultLabelFromClassDef()
        {
            ClassDef classDef = CreateTestClassDef("");
            UIFormField uiFormField;
            uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, null, null, null);
            Assert.AreEqual("Tested Property:", uiFormField.GetLabel(classDef));
            uiFormField = new UIFormField(null, "TestProperty", typeof(CheckBox), null, null, true, null, null, null);
            Assert.AreEqual("Tested Property?", uiFormField.GetLabel(classDef));
        }

        [Test]
        public void Test_DisplaynameFull_NoPropDef_DisplayName()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = CreateTestClassDef("");
            string testPropertyName = "TestPropertyNoDisplay";
            PropDef propDef = new PropDef(testPropertyName, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "", "This is a property for testing.");
            classDef.PropDefcol.Add(propDef);

            //---------------Assert Precondition----------------
            Assert.AreEqual("", propDef.DisplayName);

            //---------------Execute Test ----------------------
            string actualDisplayNameFull = propDef.DisplayNameFull;

            //---------------Test Result -----------------------
            Assert.AreEqual("Test Property No Display", actualDisplayNameFull);
        }

        [Test]
        public void Test_PropDef_NoUOM_UpdatedToDisplayNameFull()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = CreateTestClassDef("");
            string testPropertyName = "TestProperty";
            PropDef propDefUOM = (PropDef)classDef.PropDefcol[testPropertyName];
            propDefUOM.UnitOfMeasure = "";
            //---------------Assert Precondition----------------
            Assert.AreEqual("Tested Property", propDefUOM.DisplayName);
            //---------------Execute Test ----------------------
            string actualDisplayNameFull = propDefUOM.DisplayNameFull;
            //---------------Test Result -----------------------
            Assert.AreEqual("Tested Property", actualDisplayNameFull);
        }

        [Test]
        public void Test_PropDefUOM_UpdatedToDisplayNameFull()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = CreateTestClassDef("");
            string testPropertyName = "TestProperty";
            PropDef propDefUOM = (PropDef)classDef.PropDefcol[testPropertyName];
            propDefUOM.UnitOfMeasure = "NewUOM";
            //---------------Assert Precondition----------------
            Assert.AreEqual("Tested Property", propDefUOM.DisplayName);
            //---------------Execute Test ----------------------
            string actualDisplayNameFull = propDefUOM.DisplayNameFull;
            //---------------Test Result -----------------------
            Assert.AreEqual("Tested Property (NewUOM)", actualDisplayNameFull);
        }


        [Test] 
        public void Test_PropDefUnitOfMeasure_Updates_FormFieldLabel()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = CreateTestClassDef("");
            string testPropertyName = "TestProperty";
            PropDef propDefUOM = (PropDef) classDef.PropDefcol[testPropertyName];
            propDefUOM.UnitOfMeasure = "NewUOM";
            UIFormField uiFormField = new UIFormField(null, testPropertyName, typeof(TextBox), null, null, true, null, null, null);

            //---------------Assert Precondition----------------
            Assert.AreEqual("Tested Property", propDefUOM.DisplayName);
            //---------------Execute Test ----------------------
            string labelName = uiFormField.GetLabel(classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual("Tested Property (NewUOM):", labelName);

        }
        [Test]
        public void TestFieldToolTip()
        {
            UIFormField uiFormField;
            uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, "This is my ToolTipText", null, null);
            Assert.AreEqual("This is my ToolTipText", uiFormField.GetToolTipText());
        }

        [Test]
        public void TestFieldToolTipFromClassDef()
        {
            ClassDef classDef = CreateTestClassDef("");
            UIFormField uiFormField;
            uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, null, null, null);
            Assert.AreEqual("This is a property for testing.", uiFormField.GetToolTipText(classDef));
        }

        [Test]
        public void TestFieldDefaultLabelFromRelatedClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = CreateTestClassDef("");
            ClassDef classDef2 = CreateTestClassDef("2");
            ClassDef.ClassDefs.Add(classDef2);
            RelKeyDef relKeyDef = new RelKeyDef();
            RelPropDef relPropDef = new RelPropDef(classDef.PropDefcol["TestProperty"], "TestProperty2");
            relKeyDef.Add(relPropDef);
            SingleRelationshipDef def = new SingleRelationshipDef("TestRel", classDef2.AssemblyName, classDef2.ClassName, relKeyDef, false, DeleteParentAction.Prevent);
            classDef.RelationshipDefCol.Add(def);

            UIFormField uiFormField;
            uiFormField = new UIFormField(null, "TestRel.TestProperty2", typeof(TextBox), null, null, true, null, null, null);
            Assert.AreEqual("Tested Property2:", uiFormField.GetLabel(classDef));
        }

        private static ClassDef CreateTestClassDef(string suffix)
        {
            PropDefCol propDefCol = new PropDefCol();
            PropDef propDef = new PropDef("TestProperty" + suffix, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "Tested Property" + suffix, "This is a property for testing.");
            propDefCol.Add(propDef);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef();
            primaryKeyDef.Add(propDef);
            return new ClassDef("TestAssembly", "TestClass" + suffix, primaryKeyDef, 
                                propDefCol, new KeyDefCol(), new RelationshipDefCol(), new UIDefCol());
        }

        [Test]
        public void TestProtectedSets()
        {
            UIFormFieldInheritorStub field = new UIFormFieldInheritorStub();
            
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

            Assert.AreEqual("ComboBox", field.ControlTypeName);
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

        [Test]
        public void Test_Not_EqualsNull()
        {
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, "",new Hashtable(),null );
            UIFormField uiFormField2 = null;
            Assert.IsFalse(uiFormField1 == uiFormField2);
            Assert.IsFalse(uiFormField2 == uiFormField1);
            Assert.IsFalse(uiFormField1.Equals(uiFormField2));
            //Assert.AreNotEqual(uiFormField2, uiFormField1);
        }

        [Test]
        public void TestEquals()
        {
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, "", new Hashtable(), null);
            UIFormField uiFormField2 = new UIFormField("L", "L", "", "", "", "", true, "", new Hashtable(), null);
            Assert.IsTrue(uiFormField1 == uiFormField2);
            Assert.IsFalse(uiFormField1 != uiFormField2);
            Assert.IsTrue(uiFormField1.Equals(uiFormField2));
            //Assert.AreEqual(uiFormField1, uiFormField2);
        }
        [Test]
        public void Test_NotEquals()
        {
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, "", new Hashtable(), null);
            UIFormField uiFormField2 = new UIFormField("L", "L", "G", "", "", "", true, "", new Hashtable(), null);
            Assert.IsFalse(uiFormField1 == uiFormField2);
            Assert.IsTrue(uiFormField1 != uiFormField2);
            Assert.IsFalse(uiFormField1.Equals(uiFormField2));
            //Assert.AreNotEqual(uiFormField1, uiFormField2);
        }
        [Test]
        public void Test_NotEquals_LabelDiff()
        {
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, "", new Hashtable(), null);
            UIFormField uiFormField2 = new UIFormField("G", "L", "", "", "", "", true, "", new Hashtable(), null);
            Assert.IsFalse(uiFormField1.Equals(uiFormField2));
            Assert.IsFalse(uiFormField1 == uiFormField2);
            //Assert.AreNotEqual(uiFormField1, uiFormField2);
        }
        [Test]
        public void TestEqualsDifferentType()
        {
            PropDefCol uiFormField1 = new PropDefCol();

            Assert.AreNotEqual(uiFormField1, "bob");
        }

        // Grants access to protected fields
        private class UIFormFieldInheritorStub : UIFormField
        {
            public UIFormFieldInheritorStub()
                : base("label", "prop", "control", null, null, null, true, null, null, null)
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