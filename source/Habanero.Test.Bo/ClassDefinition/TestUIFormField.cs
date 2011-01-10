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

using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIFormField
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            
        }

        
        [Test]
        public void TestTriggerAccess()
        {
            //UIFormField field = new UIFormField("label", "prop", "control", "ass", "mapper", "mapass",
            //    true, null, null, null, LayoutStyle.Label);
            //Assert.IsNotNull(field.Triggers);
            //Assert.AreEqual(0, field.Triggers.Count);

            //Trigger trigger = new Trigger("prop1", null, null, "action", "value");
            //field.Triggers.Add(trigger);
            //Assert.AreEqual(1, field.Triggers.Count);
        }

        [Test]
        public void TestFieldDefaultLabel()
        {
            UIFormField uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            Assert.AreEqual("Test Property:", uiFormField.GetLabel());
            uiFormField = new UIFormField(null, "TestProperty", typeof(CheckBox), null, null, true, null, null, LayoutStyle.Label);
            Assert.AreEqual("Test Property?", uiFormField.GetLabel());
        }

        [Test]
        public void TestFieldDefaultLabelFromClassDef()
        {
            ClassDef classDef = CreateTestClassDef("");
            UIFormField uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            Assert.AreEqual("Tested Property:", uiFormField.GetLabel(classDef));
            uiFormField = new UIFormField(null, "TestProperty", typeof(CheckBox), null, null, true, null, null, LayoutStyle.Label);
            Assert.AreEqual("Tested Property?", uiFormField.GetLabel(classDef));
        }

        [Test]
        public void Test_GetLabel_WhenUIFormFieldHasClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = CreateTestClassDef("");
            var uiFormField = new UIFormFieldStub(null, "TestProperty");
            uiFormField.SetLabel(null);
            uiFormField.SetClassDef(classDef);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(classDef.GetPropDef(uiFormField.PropertyName));
            Assert.AreSame(classDef, uiFormField.GetClassDef());
            Assert.IsNull(uiFormField.Label);
            //---------------Execute Test ----------------------
            var actualLabel = uiFormField.GetLabel();
            //---------------Test Result -----------------------
            Assert.AreEqual("Tested Property:", actualLabel);
        }

        [Test]
        public void Test_DisplaynameFull_NoPropDef_DisplayName()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = CreateTestClassDef("");
            const string testPropertyName = "TestPropertyNoDisplay";
            PropDef propDef = new PropDef(testPropertyName, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "", "This is a property for testing.");
            classDef.PropDefcol.Add(propDef);

            //---------------Assert Precondition----------------
            Assert.AreEqual("Test Property No Display", propDef.DisplayName);

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
            const string testPropertyName = "TestProperty";
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
            const string testPropertyName = "TestProperty";
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
            const string testPropertyName = "TestProperty";
            PropDef propDefUOM = (PropDef) classDef.PropDefcol[testPropertyName];
            propDefUOM.UnitOfMeasure = "NewUOM";
            UIFormField uiFormField = new UIFormField(null, testPropertyName, typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);

            //---------------Assert Precondition----------------
            Assert.AreEqual("Tested Property", propDefUOM.DisplayName);
            //---------------Execute Test ----------------------
            string labelName = uiFormField.GetLabel(classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual("Tested Property (NewUOM):", labelName);

        }
        [Test] 
        public void Test_PropDefKeepValuePrivate_Updates_ShouldSetFormFieldKeepValuePrivateTrue()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();

            const string testPropertyName = "TestPropertyKeepValuePrivate";
            PropDef propDef = new PropDef(testPropertyName, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "", "This is a property for testing.", true);

            classDef.PropDefcol.Add(propDef);

            UIFormField uiFormField = new UIFormField(null, testPropertyName, typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            AddFieldToClassDef(classDef, uiFormField);
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.KeepValuePrivate);
            //---------------Execute Test ----------------------
            bool keepValuePrivate = uiFormField.KeepValuePrivate;

            //---------------Test Result -----------------------
            Assert.IsTrue(keepValuePrivate);

        }
        [Test] 
        public void Test_PropDefNotKeepValuePrivate_ShouldSetFormFieldKeepValuePrivateFalse()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();

            const string testPropertyName = "TestPropertyKeepValueNotPrivate";
            PropDef propDef = new PropDef(testPropertyName, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "", "This is a property for testing.", false);

            classDef.PropDefcol.Add(propDef);

            UIFormField uiFormField = new UIFormField(null, testPropertyName, typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            AddFieldToClassDef(classDef, uiFormField);
            //---------------Assert Precondition----------------
            Assert.IsFalse(propDef.KeepValuePrivate);
            //---------------Execute Test ----------------------
            bool keepValuePrivate = uiFormField.KeepValuePrivate;
            //---------------Test Result -----------------------
            Assert.IsFalse(keepValuePrivate);
        }
        [Test] 
        public void Test_NoPropDef_ShouldSetKeepValuePrivateFalse()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();

            const string testPropertyName = "TestPropertyKeepValueNotPrivate";


            UIFormField uiFormField = new UIFormField(null, testPropertyName, typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            AddFieldToClassDef(classDef, uiFormField);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool keepValuePrivate = uiFormField.KeepValuePrivate;
            //---------------Test Result -----------------------
            Assert.IsFalse(keepValuePrivate);
        } 
        [Test] 
        public void Test_NoClassDef_ShouldSetKeepValuePrivateFalse()
        {
            //---------------Set up test pack-------------------

            const string testPropertyName = "TestPropertyKeepValueNotPrivate";


            UIFormField uiFormField = new UIFormField(null, testPropertyName, typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool keepValuePrivate = uiFormField.KeepValuePrivate;
            //---------------Test Result -----------------------
            Assert.IsFalse(keepValuePrivate);
        }

        private void AddFieldToClassDef(IClassDef classDef, IUIFormField uiFormField)
        {
            IUIDef def = classDef.UIDefCol["default"];
            IUIForm form = def.UIForm;
            IUIFormTab tab = form[0];
            IUIFormColumn column = tab[0];
            column.Add(uiFormField);
        }
        [Test]
        public void TestFieldToolTip()
        {
            UIFormField uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, "This is my ToolTipText", null, LayoutStyle.Label);
            Assert.AreEqual("This is my ToolTipText", uiFormField.GetToolTipText());
        }

        [Test]
        public void TestFieldToolTipFromClassDef()
        {
            ClassDef classDef = CreateTestClassDef("");
            UIFormField uiFormField = new UIFormField(null, "TestProperty", typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
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

            UIFormField uiFormField = new UIFormField(null, "TestRel.TestProperty2", typeof(TextBox), null, null, true, null, null, LayoutStyle.Label);
            Assert.AreEqual("Tested Property2:", uiFormField.GetLabel(classDef));
        }

        private static ClassDef CreateTestClassDef(string suffix)
        {
            PropDefCol propDefCol = new PropDefCol();
            PropDef propDef = new PropDef("TestProperty" + suffix, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "Tested Property" + suffix, "This is a property for testing.");
            propDefCol.Add(propDef);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef {propDef};
            return new ClassDef("TestAssembly", "TestClass" + suffix, primaryKeyDef, 
                                propDefCol, new KeyDefCol(), new RelationshipDefCol(), new UIDefCol());
        }

        [Test]
        public void TestProtectedSets()
        {
            UIFormFieldStub field = new UIFormFieldStub();
            
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
            UIFormField uiFormField1 = CreateFormField();
            const UIFormField uiFormField2 = null;
            Assert.IsFalse(uiFormField1 == uiFormField2);
            Assert.IsFalse(uiFormField2 == uiFormField1);
            Assert.IsFalse(uiFormField1.Equals(uiFormField2));
            //Assert.AreNotEqual(uiFormField2, uiFormField1);
        }


        [Test]
        public void TestEquals()
        {
            UIFormField uiFormField1 = CreateFormField();
            UIFormField uiFormField2 = CreateFormField();
            Assert.IsTrue(uiFormField1 == uiFormField2);
            Assert.IsFalse(uiFormField1 != uiFormField2);
            Assert.IsTrue(uiFormField1.Equals(uiFormField2));
            //Assert.AreEqual(uiFormField1, uiFormField2);
        }

        [Test]
        public void Test_HashCode_Equals()
        {
            //--------------- Set up test pack ------------------
            UIFormField uiFormField1 = CreateFormField();
            UIFormField uiFormField2 = CreateFormField();

            //--------------- Test Preconditions ----------------
            Assert.IsTrue(uiFormField1.Equals(uiFormField2));
            //--------------- Execute Test ----------------------
            
            //--------------- Test Result -----------------------
            Assert.AreEqual(uiFormField1.GetHashCode(), uiFormField2.GetHashCode());
        }

        [Test]
        public void Test_HashCode_NotEquals()
        {
            //--------------- Set up test pack ------------------
            UIFormField uiFormField1 = CreateFormField();
            UIFormField uiFormField2 = CreateFormField("otherPropName");

            //--------------- Test Preconditions ----------------
            Assert.IsFalse(uiFormField1.Equals(uiFormField2));
            //--------------- Execute Test ----------------------

            //--------------- Test Result -----------------------
            Assert.AreNotEqual(uiFormField1.GetHashCode(), uiFormField2.GetHashCode());
        }

        [Test]
        public void Test_NotEquals()
        {
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            UIFormField uiFormField2 = new UIFormField("L", "L", "G", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            Assert.IsFalse(uiFormField1 == uiFormField2);
            Assert.IsTrue(uiFormField1 != uiFormField2);
            Assert.IsFalse(uiFormField1.Equals(uiFormField2));
            //Assert.AreNotEqual(uiFormField1, uiFormField2);
        }
        [Test]
        public void Test_NotEquals_LabelDiff()
        {
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            UIFormField uiFormField2 = new UIFormField("G", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
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

        [Test]
        public void TestHasParameterValue_False()
        {
            //---------------Set up test pack-------------------
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool hasParameterValue = uiFormField1.HasParameterValue(TestUtil.GetRandomString());
            //---------------Test Result -----------------------
            Assert.IsFalse(hasParameterValue);

        }       
        
        [Test]
        public void TestHasParameterValue_True()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable();
            const string parameterName = "bob";
            parameters.Add(parameterName, "I can like to have a value");
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool hasParameterValue = uiFormField1.HasParameterValue(parameterName);
            //---------------Test Result -----------------------
            Assert.IsTrue(hasParameterValue);

        }

        [Test]
        public void TestRowSpan_NotSet()
        {
            //---------------Set up test pack-------------------
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int rowSpan = uiFormField1.RowSpan;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, rowSpan);

        }

        [Test]
        public void TestRowSpan_Set()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable {{"rowSpan", 3}};
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int rowSpan = uiFormField1.RowSpan;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, rowSpan);
        }    
        
        [Test]
        public void TestColSpan_NotSet()
        {
            //---------------Set up test pack-------------------
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int colSpan = uiFormField1.ColSpan;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, colSpan);

        }

        [Test]
        public void TestColSpan_Set()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable {{"colSpan", 3}};
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int colSpan = uiFormField1.ColSpan;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, colSpan);
        }

        [Test]
        public void TestNumLines_Set()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable {{"numLines", 3}};
            UIFormField uiFormField1 = new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            int rowSpan = uiFormField1.RowSpan;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, rowSpan);
        }

        [Test]
        public void TestNumLines_Set_RowSpan_Set()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable {{"numLines", 3}, {"rowSpan", 2}};
            UIFormField uiFormField = new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            int rowSpan = uiFormField.RowSpan;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, rowSpan);
        }

        [Test]
        public void TestAlignment_NotSet()
        {
            //---------------Set up test pack-------------------
            UIFormField uiFormField = new UIFormField("L", "L", "", "", "", "", true, null, "", new Hashtable(), LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string alignment = uiFormField.Alignment;
            //---------------Test Result -----------------------
            Assert.IsNull(alignment);

        }

        [Test]
        public void TestAlignment_Set()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable {{"alignment", "right"}};
            UIFormField uiFormField = new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string alignment = uiFormField.Alignment;
            //---------------Test Result -----------------------
            Assert.AreEqual("right", alignment);
        }

        [Test]
        public void Test_showAsCompulsory_WhenUseCompNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            UIFormField uiFormField = new UIFormField("L", "L", "", "", "", "", true, null
                    , "", null, LayoutStyle.Label);
            //---------------Test Result -----------------------
            var showAsCompulsory = uiFormField.ShowAsCompulsory;
            Assert.IsNull(showAsCompulsory);
        }

        [Test]
        public void Test_showAsCompulsory_WhenUseCompFalse_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            UIFormField uiFormField = new UIFormField("L", "L", "", "", "", "", true, false
                    , "", null, LayoutStyle.Label);
            //---------------Test Result -----------------------
            var showAsCompulsory = uiFormField.ShowAsCompulsory;
            Assert.IsNotNull(showAsCompulsory);
            Assert.IsFalse((bool) showAsCompulsory);
        }

        [Test]
        public void Test_showAsCompulsory_WhenUseCompTrue_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            UIFormField uiFormField = new UIFormField("L", "L", "", "", "", "", true, true
                    , "", null, LayoutStyle.Label);
            //---------------Test Result -----------------------
            var showAsCompulsory = uiFormField.ShowAsCompulsory;
            Assert.IsNotNull(showAsCompulsory);
            Assert.IsTrue((bool)showAsCompulsory);
        }

        [Test]
        public void TestIsCompulsory_WhenBOPropFalse_AndShowAsCompTrue_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propertyName = "TestProp";
            IUIFormField field = classDef.UIDefCol["default"].GetFormField(propertyName);
            field.ShowAsCompulsory = true;
            var propDef = classDef.PropDefcol[propertyName];
            //---------------Assert Precondition----------------
            Assert.IsFalse(propDef.Compulsory);
            var showAsCompulsory = field.ShowAsCompulsory;
            Assert.IsTrue((bool)showAsCompulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }
        [Test]
        public void TestIsCompulsory_WhenBOPropTrue_AndShowAsCompFalse_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propertyName = "TestProp";
            IUIFormField field = classDef.UIDefCol["default"].GetFormField(propertyName);
            field.ShowAsCompulsory = false;
            var propDef = classDef.PropDefcol[propertyName];
            propDef.Compulsory = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory);
            var showAsCompulsory = field.ShowAsCompulsory;
            Assert.IsFalse((bool)showAsCompulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }
        [Test]
        public void TestIsCompulsory_WhenBOPropTrue_AndShowAsCompNull_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propertyName = "TestProp";
            IUIFormField field = classDef.UIDefCol["default"].GetFormField(propertyName);
            field.ShowAsCompulsory = null;
            var propDef = classDef.PropDefcol[propertyName];
            propDef.Compulsory = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory);
            var showAsCompulsory = field.ShowAsCompulsory;
            Assert.IsNull(showAsCompulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }
        [Test]
        public void TestIsCompulsory_WhenBOPropfalse_AndShowAsCompNull_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propertyName = "TestProp";
            IUIFormField field = classDef.UIDefCol["default"].GetFormField(propertyName);
            field.ShowAsCompulsory = null;
            var propDef = classDef.PropDefcol[propertyName];
            propDef.Compulsory = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory);
            var showAsCompulsory = field.ShowAsCompulsory;
            Assert.IsNull(showAsCompulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }

        [Test]
        public void TestIsCompulsory_False()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propertyName = "TestProp";
            IUIFormField field = classDef.UIDefCol["default"].GetFormField(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsFalse(classDef.PropDefcol[propertyName].Compulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory);
        }

        [Test]
        public void TestIsCompulsory_True()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            const string propertyName = "TestProp";
            IUIFormField field = classDef.UIDefCol["default"].GetFormField(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsTrue(classDef.PropDefcol[propertyName].Compulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }

        [Test]
        public void TestIsCompulsory_VirtualProp()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IUIFormField field = classDef.UIDefCol["AlternateVirtualProp"].GetFormField("-MyTestProp-");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory);
        }

        [Test]
        public void TestIsCompulsory_WhenRelationshipField_AndRelationshipIsCompulsory()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithAssociationRelationship();
            classDef.PropDefcol["RelatedID"].Compulsory = true;
            IUIFormField field = classDef.UIDefCol["default"].GetFormField("MyRelationship");
            //---------------Assert Precondition----------------
            Assert.IsTrue(classDef.RelationshipDefCol["MyRelationship"].IsCompulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }

        [Test]
        public void TestIsCompulsory_WhenRelationshipField_AndRelationshipIsNotCompulsory()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithAssociationRelationship();
            classDef.PropDefcol["RelatedID"].Compulsory = false;
            IUIFormField field = classDef.UIDefCol["default"].GetFormField("MyRelationship");
            //---------------Assert Precondition----------------
            Assert.IsFalse(classDef.RelationshipDefCol["MyRelationship"].IsCompulsory);
            //---------------Execute Test ----------------------
            bool isCompulsory = field.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory);
        }
        
        [Test]
        public void TestLabelTextHasStarIfCompulsory()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            IUIFormField field = classDef.UIDefCol["default"].GetFormField("TestProp");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string labelText = field.GetLabel();
            //---------------Test Result -----------------------
            StringAssert.EndsWith(" *", labelText);

        }

        [Test]
        public void TestLabelTextHasStarIfCompulsory_Generated()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            IUIFormField field = classDef.UIDefCol["default"].GetFormField("TestProp");
            field.Label = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string labelText = field.GetLabel();
            //---------------Test Result -----------------------
            StringAssert.EndsWith(": *", labelText);
        }

        [Test]
        public void TestLabelText_UsesPropertyName()
        {
            XmlUIFormFieldLoader loader = new XmlUIFormFieldLoader(new DtdLoader(), new DefClassFactory());
            UIFormField uiProp = (UIFormField) loader.LoadUIProperty(@"<field property=""testpropname"" />");
            Assert.AreEqual(null, uiProp.Label);
            Assert.AreEqual("testpropname:", uiProp.GetLabel());
        }

        [Test]
        public void TestLabelText_UsesPropertyNameWithCamelCase()
        {
            XmlUIFormFieldLoader loader = new XmlUIFormFieldLoader(new DtdLoader(), new DefClassFactory());
            UIFormField uiProp = (UIFormField) loader.LoadUIProperty(@"<field property=""TestPropName"" />");
            Assert.AreEqual(null, uiProp.Label);
            Assert.AreEqual("Test Prop Name:", uiProp.GetLabel());
        }

        [Test]
        public void TestLabelText_UsesQuestionMark_WhenCheckBoxField()
        {
            XmlUIFormFieldLoader loader = new XmlUIFormFieldLoader(new DtdLoader(), new DefClassFactory());
            UIFormField uiProp = (UIFormField) loader.LoadUIProperty(@"<field property=""TestPropName"" type=""CheckBox"" />");
            Assert.AreEqual(null, uiProp.Label);
            Assert.AreEqual("Test Prop Name?", uiProp.GetLabel());
        }



        [Test]
        public void TestFormColumn()
        {
            //---------------Set up test pack-------------------
            UIFormField uiFormField1 = CreateFormField();
            UIFormColumn column = new UIFormColumn();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            uiFormField1.UIFormColumn = column;
            //---------------Test Result -----------------------
            Assert.AreSame(column, uiFormField1.UIFormColumn);
        }

        [Test]
        public void Test_DecimalPlaces_WhenSetShouldReturn()
        {
            //---------------Set up test pack-------------------
            const string expectedDecimalPlace = "2";
            Hashtable parameters = new Hashtable {{"decimalPlaces", expectedDecimalPlace}};
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var decimalPlaces = uiFormField1.DecimalPlaces;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDecimalPlace, decimalPlaces);
        }
        [Test]
        public void Test_DecimalPlaces_WhenNotSet_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable();
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var decimalPlaces = uiFormField1.DecimalPlaces;
            //---------------Test Result -----------------------
            Assert.IsNull(decimalPlaces);
        }
        [Test]
        public void Test_Options_WhenSetShouldReturn()
        {
            //---------------Set up test pack-------------------
            const string expectedOptions = "2";
            Hashtable parameters = new Hashtable { { "options", expectedOptions } };
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var options = uiFormField1.Options;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedOptions, options);
        }
        [Test]
        public void Test_Options_WhenNotSet_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable();
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var options = uiFormField1.Options;
            //---------------Test Result -----------------------
            Assert.IsNull(options);
        }
        [Test]
        public void Test_IsEmail_WhenSetShouldReturn()
        {
            //---------------Set up test pack-------------------
            const string expectedIsEmail = "2";
            Hashtable parameters = new Hashtable { { "isEmail", expectedIsEmail } };
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var isEmail = uiFormField1.IsEmail;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedIsEmail, isEmail);
        }
        [Test]
        public void Test_IsEmail_WhenNotSet_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable();
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var isEmail = uiFormField1.IsEmail;
            //---------------Test Result -----------------------
            Assert.IsNull(isEmail);
        }
        [Test]
        public void Test_DateFormat_WhenSetShouldReturn()
        {
            //---------------Set up test pack-------------------
            const string expectedDateFormat = "2";
            Hashtable parameters = new Hashtable { { "dateFormat", expectedDateFormat } };
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dateFormat = uiFormField1.DateFormat;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateFormat, dateFormat);
        }
        [Test]
        public void Test_DateFormat_WhenNotSet_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            Hashtable parameters = new Hashtable();
            UIFormField uiFormField1 = CreateFormField(parameters);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dateFormat = uiFormField1.DateFormat;
            //---------------Test Result -----------------------
            Assert.IsNull(dateFormat);
        }

        private static UIFormField CreateFormField(Hashtable parameters)
        {
            return new UIFormField("L", "L", "", "", "", "", true, null, "", parameters, LayoutStyle.Label);
        }

        [Test]
        public void Test_Layout()
        {
            UIFormField uiFormField1 = CreateFormField();
            //---------------Execute Test ----------------------
            uiFormField1.Layout = LayoutStyle.GroupBox;
            //---------------Test Result -----------------------
            Assert.AreEqual(LayoutStyle.GroupBox, uiFormField1.Layout);
        }
        [Test]
        public void Test_Layout_Default()
        {
            //---------------Execute Test ----------------------
            UIFormField uiFormField1 = CreateFormField();
            //---------------Test Result -----------------------
            Assert.AreEqual(LayoutStyle.Label, uiFormField1.Layout);
        }

        [Test]
        public void Test_SetClassDef_ShouldSet()
        {
            //---------------Set up test pack-------------------
            UIFormField uiFormField = CreateFormField();
            var classDef = MockRepository.GenerateStub<IClassDef>();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiFormField.ClassDef);
            //---------------Execute Test ----------------------
            uiFormField.ClassDef = classDef;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, uiFormField.ClassDef);
        }

        private static UIFormField CreateFormField() { return new UIFormField("L", "L", "", "", "", "", true, null, "", null, LayoutStyle.Label); }
        private static UIFormField CreateFormField(string propName) { return new UIFormField("L", propName, "", "", "", "", true, null, "", null, LayoutStyle.Label); }

        // Grants access to protected fields
        private class UIFormFieldStub : UIFormField
        {
            private IClassDef _classDef;

            public UIFormFieldStub()
                : base("label", "prop", "control", null, null, null, true, null, null, null, LayoutStyle.Label)
            {}
            public UIFormFieldStub(string propName)
                : base("label", propName, "control", null, null, null, true, null, null, null, LayoutStyle.Label)
            {}
            public UIFormFieldStub(string propLabel, string propName)
                : base(propLabel, propName, "control", null, null, null, true, null, null, null, LayoutStyle.Label)
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
            public void SetClassDef(IClassDef classDef)
            {
                _classDef = classDef;
            }
            public override IClassDef GetClassDef()
            {
                return _classDef;
            }
        }
    }
}
// ReSharper restore InconsistentNaming