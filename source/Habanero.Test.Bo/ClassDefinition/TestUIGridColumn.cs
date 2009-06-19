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
    public class TestUIGridColumn
    {
        [Test]
        public void TestParameters()
        {
            Hashtable parameters = new Hashtable();


            UIGridColumn column = new UIGridColumn("heading", null, null, null, true, 100,
                                                   PropAlignment.left, parameters);

            Assert.AreEqual(0, column.Parameters.Count);
            column.Parameters.Add("name", "value");
            Assert.IsNull(column.GetParameterValue("somename"));
            Assert.AreEqual("value", column.GetParameterValue("name"));
        }

        [Test]
        public void TestParameters_Null()
        {
            Hashtable parameters = new Hashtable();

            UIGridColumn column = new UIGridColumn("heading", null, null, null, true, 100,
                                                   PropAlignment.left, null);

            Assert.IsNull(column.GetParameterValue("somename"));
        }

        [Test]
        public void TestFieldDefaultLabel()
        {
            UIGridColumn uiGridColumn;
            uiGridColumn = new UIGridColumn(null, "TestProperty", typeof(DataGridViewTextBoxColumn), false, 100, PropAlignment.left, null);
            Assert.AreEqual("Test Property", uiGridColumn.GetHeading());
        }

        [Test]
        public void TestFieldDefaultLabelFromClassDef()
        {
            ClassDef classDef = CreateTestClassDef("");
            UIGridColumn uiGridColumn;
            uiGridColumn = new UIGridColumn(null, "TestProperty", typeof(DataGridViewTextBoxColumn), false, 100, PropAlignment.left , null);
            Assert.AreEqual("Tested Property", uiGridColumn.GetHeading(classDef));
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

            UIGridColumn uiGridColumn;
            uiGridColumn = new UIGridColumn(null, "TestRel.TestProperty2", typeof(DataGridViewTextBoxColumn), false, 100, PropAlignment.left, null);
            Assert.AreEqual("Tested Property2", uiGridColumn.GetHeading(classDef));
        }

        private static ClassDef CreateTestClassDef(string suffix)
        {
            PropDefCol propDefCol = new PropDefCol();
            PropDef propDef = new PropDef("TestProperty" + suffix, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "Tested Property" + suffix, null);
            propDefCol.Add(propDef);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef();
            primaryKeyDef.Add(propDef);
            return new ClassDef("TestAssembly", "TestClass" + suffix, primaryKeyDef,
                                propDefCol, new KeyDefCol(), new RelationshipDefCol(), new UIDefCol());
        }

        [Test]
        public void TestSettingControlTypeSetsTypeNames()
        {
            UIGridColumn uiGridColumn = new UIGridColumn(null, "TestProperty",
                "DataGridViewTextBoxColumn", "System.Windows.Forms", false, 100, PropAlignment.left, null);
            Assert.AreEqual("DataGridViewTextBoxColumn", uiGridColumn.GridControlTypeName);
            Assert.IsNull(uiGridColumn.GridControlType);

            uiGridColumn.GridControlType = typeof (MyBO);
            Assert.AreEqual("MyBO", uiGridColumn.GridControlTypeName);
            Assert.AreEqual("Habanero.Test", uiGridColumn.GridControlAssemblyName);
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

            Assert.AreEqual(PropAlignment.left, column.Alignment);
            column.SetAlignment(PropAlignment.right);
            Assert.AreEqual(PropAlignment.right, column.Alignment);
        }

        [Test]
        public void TestCloneGridColumn()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn = new UIGridColumn("pp", "pp", "", "", false, 0, PropAlignment.centre, null);

            //---------------Execute Test ----------------------
            IUIGridColumn clonedGridColumn = gridColumn.Clone();

            //---------------Test Result -----------------------
            Assert.IsTrue(gridColumn.Equals(clonedGridColumn));
            Assert.IsTrue(gridColumn == (UIGridColumn) clonedGridColumn);
            Assert.IsFalse(gridColumn != (UIGridColumn) clonedGridColumn);
        }

        [Test]
        public void Test_HashCodeEquals()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn = new UIGridColumn("pp", "pp", "", "", false, 0, PropAlignment.centre, null);
            IUIGridColumn clonedGridColumn = gridColumn.Clone();

            //---------------Assert preconditions----------------
            Assert.IsTrue(gridColumn.Equals(clonedGridColumn));
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(gridColumn.GetHashCode(), clonedGridColumn.GetHashCode());
        }

        [Test]
        public void Test_HashCodeNotEquals()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn = new UIGridColumn("pp", "pp", "", "", false, 0, PropAlignment.centre, null);
            UIGridColumn otherColumn = new UIGridColumn("pp", "qq", "", "", false, 0, PropAlignment.centre, null);

            //---------------Assert preconditions----------------
            Assert.IsFalse(gridColumn.Equals(otherColumn));
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreNotEqual(gridColumn.GetHashCode(), otherColumn.GetHashCode());
        }



        [Test]
        public void Test_NotEqualsNull()
        {
            UIGridColumn uiGridColumn1 = new UIGridColumn("", "", "", "",false,0,PropAlignment.centre, null);
            UIGridColumn uiGridColumn2 = null;
            Assert.IsFalse(uiGridColumn1 == uiGridColumn2);
            Assert.IsTrue(uiGridColumn1 != uiGridColumn2);
            Assert.IsFalse(uiGridColumn1.Equals(uiGridColumn2));
            Assert.AreNotEqual(uiGridColumn1, uiGridColumn2);
        }

        [Test]
        public void Test_NotSameType()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn = new UIGridColumn("", "", "", "", false, 0, PropAlignment.centre, null);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool methodEquals = gridColumn.Equals("fedafds");

            //---------------Test Result -----------------------
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_NotEqual_DifName()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn1 = new UIGridColumn("pp", "pp", "", "", false, 0, PropAlignment.centre, null);
            UIGridColumn gridColumn2 = new UIGridColumn("pp", "mm", "", "", false, 0, PropAlignment.centre, null);

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = gridColumn1 == gridColumn2;
            bool operatorNotEquals = gridColumn1 != gridColumn2;
            bool methodEquals = gridColumn1.Equals(gridColumn2);

            //---------------Test Result -----------------------
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
            Assert.IsFalse(methodEquals);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_NotEqual_Heading()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn1 = new UIGridColumn("pp", "pp", "", "", false, 0, PropAlignment.centre, null);
            UIGridColumn gridColumn2 = new UIGridColumn("pp1", "pp", "", "", false, 0, PropAlignment.centre, null);

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            bool operatorEquals = gridColumn1 == gridColumn2;
            bool operatorNotEquals = gridColumn1 != gridColumn2;
            bool methodEquals = gridColumn1.Equals(gridColumn2);

            //---------------Test Result -----------------------
            Assert.IsFalse(methodEquals);
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
            //---------------Tear Down -------------------------          
        }

        // Grants access to protected fields
        private class UIGridColumnInheritor : UIGridColumn
        {
            public UIGridColumnInheritor() : base("heading", null, null, null, true, 100,
                PropAlignment.left, null)
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