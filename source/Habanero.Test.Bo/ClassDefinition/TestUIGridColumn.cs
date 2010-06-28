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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Testability;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIGridColumn
    {
        [Test]
        public void TestParameters()
        {
            Hashtable parameters = new Hashtable();


            IUIGridColumn column = new UIGridColumn("heading", null, null, null, true, 100,
                                                   PropAlignment.left, parameters);

            Assert.AreEqual(0, column.Parameters.Count);
            column.Parameters.Add("name", "value");
            Assert.IsNull(column.GetParameterValue("somename"));
            Assert.AreEqual("value", column.GetParameterValue("name"));
        }

        [Test]
        public void TestParameters_Null()
        {

            IUIGridColumn column = new UIGridColumn("heading", null, null, null, true, 100,
                                                   PropAlignment.left, null);

            Assert.IsNull(column.GetParameterValue("somename"));
        }

        [Test]
        public void TestFieldDefaultLabel()
        {
            IUIGridColumn uiGridColumn = new UIGridColumn(null, "TestProperty", typeof(DataGridViewTextBoxColumn), false, 100, PropAlignment.left, null);
            Assert.AreEqual("Test Property", uiGridColumn.GetHeading());
        }

        [Test]
        public void TestFieldDefaultLabelFromClassDef()
        {
            ClassDef classDef = CreateTestClassDef("");
            IUIGridColumn uiGridColumn = new UIGridColumn(null, "TestProperty", typeof(DataGridViewTextBoxColumn), false, 100, PropAlignment.left , null);

#pragma warning disable 612,618
            Assert.AreEqual("Tested Property", uiGridColumn.GetHeading(classDef));
#pragma warning restore 612,618
        }

        [Test]
        public void Test_SetUIGrid_ShouldSetUIGrid()
        {
            //---------------Set up test pack-------------------
            var expectedGridDef = MockRepository.GenerateStub<IUIGrid>();
            IUIGridColumn uiGridColumn = new UIGridColumnSpy();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiGridColumn.ClassDef);
            //---------------Execute Test ----------------------
            uiGridColumn.UIGrid = expectedGridDef;
            var actualGridDef = uiGridColumn.UIGrid;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedGridDef, actualGridDef);
        }

        [Test]
        public void Test_ClassDef_ShouldReturnUIGridsClassDef()
        {
            //---------------Set up test pack-------------------
            var expectedGridDef = MockRepository.GenerateStub<IUIGrid>();
            IClassDef expectedClassDef = MockRepository.GenerateStub<IClassDef>();
            expectedGridDef.Stub(grid => grid.ClassDef).Return(expectedClassDef);
            IUIGridColumn uiGridColumn = new UIGridColumnSpy();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiGridColumn.ClassDef);
            Assert.IsNotNull(expectedGridDef.ClassDef);
            //---------------Execute Test ----------------------
            uiGridColumn.UIGrid = expectedGridDef;
            var actualClassDef = uiGridColumn.ClassDef;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, actualClassDef);
        }
        [Test]
        public void Test_ClassDef_WhenUIDefNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IUIGridColumn uiGridColumn = new UIGridColumnSpy();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiGridColumn.ClassDef);
            Assert.IsNull(uiGridColumn.UIGrid);
            //---------------Execute Test ----------------------
            var actualClassDef = uiGridColumn.ClassDef;
            //---------------Test Result -----------------------
            Assert.IsNull(actualClassDef);
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
#pragma warning disable 612,618
            Assert.AreEqual("Tested Property2", uiGridColumn.GetHeading(classDef));
#pragma warning restore 612,618
        }
        
        [Test]
        public void TestAutomaticHeadingCreation()
        {
            XmlUIGridColumnLoader loader = new XmlUIGridColumnLoader(new DtdLoader(), new DefClassFactory());
            IUIGridColumn uiProp = loader.LoadUIProperty(@"<column property=""testpropname"" />");
            Assert.AreEqual(null, uiProp.Heading);
            Assert.AreEqual("testpropname", uiProp.GetHeading());
        }
       
        [Test]
        public void TestAutomaticHeadingCreation_UsingCamelCase()
        {
            XmlUIGridColumnLoader loader = new XmlUIGridColumnLoader(new DtdLoader(), new DefClassFactory());
            IUIGridColumn uiProp = loader.LoadUIProperty(@"<column property=""TestPropName"" />");
            Assert.AreEqual(null, uiProp.Heading);
            Assert.AreEqual("Test Prop Name", uiProp.GetHeading());
        }

        private static ClassDef CreateTestClassDef(string suffix)
        {
            PropDefCol propDefCol = new PropDefCol();
            PropDef propDef = new PropDef("TestProperty" + suffix, typeof(string), PropReadWriteRule.ReadWrite, null, null, false, false, 100,
                                          "Tested Property" + suffix, null);
            propDefCol.Add(propDef);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef {propDef};
            var testClassDef = new ClassDef("TestAssembly", "TestClass" + suffix, primaryKeyDef,
                                            propDefCol, new KeyDefCol(), new RelationshipDefCol(), new UIDefCol());
            var uiGrid = new UIGrid();
            testClassDef.UIDefCol.Add(new UIDef("UIDef1", new UIForm(), uiGrid ));
            return testClassDef;

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
            UIGridColumnSpy column = new UIGridColumnSpy();

            Assert.AreEqual("heading", column.Heading);
            column.SetHeading("newheading");
            Assert.AreEqual("newheading", column.Heading);
            column.SetPropertyName(null);
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

        [Test]
        public void Test_LookupList_ShouldReturnClassDefsLookupList()
        {
            //---------------Set up test pack-------------------
            var expectedLList = MockRepository.GenerateStub<ILookupList>();
            var classDef = MockRepository.GenerateStub<IClassDef>();
            UIGridColumn gridColumn = GetGridColumn(classDef, expectedLList);
            //---------------Assert Precondition----------------
            Assert.AreSame(classDef, gridColumn.ClassDef);
            Assert.AreSame(expectedLList, classDef.GetLookupList(gridColumn.PropertyName));
            //---------------Execute Test ----------------------
            ILookupList actualLList = gridColumn.LookupList;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedLList, actualLList);
        }
        [Test]
        public void Test_LookupList_WhenNullClassDef_ShouldReturnNullLookupList()
        {
            //---------------Set up test pack-------------------
            UIGridColumn gridColumn = new UIGridColumnSpy();
            //---------------Assert Precondition----------------
            Assert.IsNull(gridColumn.ClassDef);
            //---------------Execute Test ----------------------
            ILookupList actualLList = gridColumn.LookupList;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<NullLookupList>(actualLList );
        }

        [Test]
        public void Test_GetPropType_WhenSetPropDefViaSpy_ShouldReturnSetPropDefsType()
        {
            //This is actually testing an optimisation so that we do not
            // continually have to go refetch the PropDef for the Column
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            IPropDef propDef = GetIntPropDef();
            gridColumn.SetPropDef(propDef);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propertyType = gridColumn.GetPropertyType();
            //---------------Test Result -----------------------
            Assert.AreSame(propDef.PropertyType, propertyType);
        }

        private IPropDef GetIntPropDef()
        {
            var propDef = MockRepository.GenerateStub<IPropDef>();
            propDef.PropertyType = typeof (int);
            return propDef;
        }

        [Test]
        public void Test_GetPropertyType_WhenHasPropDef_ButNotSet_ShouldReturnPropDefPropType()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            IClassDef classDef = MockRepository.GenerateStub<IClassDef>();
            classDef.Stub(def => def.GetPropDef(gridColumn.PropertyName, false)).Return(GetIntPropDef());
            gridColumn.SetClassDef(classDef);
            //---------------Assert Precondition----------------
            Assert.AreSame(typeof(int), classDef.GetPropDef(gridColumn.PropertyName, false).PropertyType);
            //---------------Execute Test ----------------------
            var propertyType = gridColumn.GetPropertyType();
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(int), propertyType);
        }
        [Test]
        public void Test_GetPropertyType_WhenReflectiveProp_ReturnReflectivePropType()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            IClassDef classDef = MockRepository.GenerateStub<IClassDef>();
            string propertyName = gridColumn.PropertyName;
            classDef.Stub(def => def.GetPropertyType(propertyName)).Return(typeof(bool));
            gridColumn.SetClassDef(classDef);
            //---------------Assert Precondition----------------
            Assert.AreSame(typeof(bool), classDef.GetPropertyType(gridColumn.PropertyName));
            //---------------Execute Test ----------------------
            var propertyType = gridColumn.GetPropertyType();
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(bool), propertyType);
        }
        [Test]
        public void Test_GetPropertyType_WhenPropDefLookupList_ShouldReturnObjectType()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            var propDef = MockRepository.GenerateStub<IPropDef>();
            propDef.PropertyType = typeof (bool);
            propDef.Stub(def => def.HasLookupList()).Return(true);
            gridColumn.SetPropDef(propDef);
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.HasLookupList());
            //---------------Execute Test ----------------------
            var propertyType = gridColumn.GetPropertyType();
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(object), propertyType);
        }

        [Test]
        public void Test_SetPropName_SetsProtectedPropDefToNull()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            var propDef = MockRepository.GenerateStub<IPropDef>();
            gridColumn.SetPropDef(propDef);
            //---------------Assert Precondition----------------
            Assert.AreSame(propDef, gridColumn.PropDef);
            //---------------Execute Test ----------------------
            gridColumn.PropertyName = RandomValueGen.GetRandomString();
            //---------------Test Result -----------------------
            Assert.IsNull(gridColumn.PropDef);
        }

        [Test]
        public void Test_GetPropertyType_FromInterface_WhenPropDefLookupList_ShouldReturnObjectType()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            var propDef = MockRepository.GenerateStub<IPropDef>();
            propDef.PropertyType = typeof(bool);
            propDef.Stub(def => def.HasLookupList()).Return(true);
            gridColumn.SetPropDef(propDef);
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.HasLookupList());
            //---------------Execute Test ----------------------
            var propertyType = ((IUIGridColumn)gridColumn).GetPropertyType();
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(object), propertyType);
        }

        private IUIGridColumn GetGridColumn()
        {
            ClassDef classDef = CreateTestClassDef("");
            UIGridColumn gridColumn = new UIGridColumn("T P", "TestProperty",  null, null, true, 100, PropAlignment.left, null)
                                          {
                                              Editable = true,
                                              UIGrid = classDef.UIDefCol["UIDef1"].UIGrid
                                          };
            return gridColumn;
        }
        [Test]
        public void Test_PropDef_WhenHas_ShouldReturnPropDef()
        {
            //---------------Set up test pack-------------------
            IUIGridColumn gridColumn = GetGridColumn();
            //---------------Assert Precondition----------------
            Assert.IsTrue(((UIGridColumn)gridColumn).HasPropDef);
            //---------------Execute Test ----------------------
            IPropDef propDef = gridColumn.PropDef;
            //---------------Test Result -----------------------
            Assert.IsNotNull(propDef);
        }
        [Test]
        public void Test_PropDef_WhenNotHas_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IUIGridColumn gridColumn = GetGridColumn();
            gridColumn.PropertyName = "SomeNonExistentName";
            //---------------Assert Precondition----------------
            Assert.IsFalse(((UIGridColumn)gridColumn).HasPropDef);
            //---------------Execute Test ----------------------
            IPropDef propDef = gridColumn.PropDef;
            //---------------Test Result -----------------------
            Assert.IsNull(propDef);
        }
        [Test]
        public void Test_HasPropDef_WhenHas_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            gridColumn.SetPropDef(MockRepository.GenerateMock<IPropDef>());
            //---------------Assert Precondition----------------
            Assert.IsNotNull(gridColumn.PropDef);
            //---------------Execute Test ----------------------
            bool hasPropDef = gridColumn.HasPropDef;
            //---------------Test Result -----------------------
            Assert.IsTrue(hasPropDef);
        }
        [Test]
        public void Test_HasPropDef_WhenNotHas_ShouldRetFalse()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy();
            //---------------Assert Precondition----------------
            Assert.IsNull(gridColumn.PropDef);
            //---------------Execute Test ----------------------
            bool hasPropDef = gridColumn.HasPropDef;
            //---------------Test Result -----------------------
            Assert.IsFalse(hasPropDef);
        }

        [Test]
        public void Test_Editable_WhenSetToFalse_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            IUIGridColumn gridColumn = GetGridColumnSpy();
            //---------------Assert Precondition----------------
            Assert.IsTrue(gridColumn.Editable);
            //---------------Execute Test ----------------------
            gridColumn.Editable = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(gridColumn.Editable);
        }

        private IUIGridColumn GetGridColumnSpy()
        {
            ClassDef classDef = CreateTestClassDef("");
            IUIGridColumn gridColumn = new UIGridColumnSpy("TestProperty") {Editable = true};
            ((UIGridColumnSpy)gridColumn).SetClassDef(classDef);
            return gridColumn;
        }

        [Test]
        public void Test_Editable_WhenSetToTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            IUIGridColumn gridColumn = GetGridColumnSpy();
            gridColumn.Editable = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(gridColumn.Editable);
            //---------------Execute Test ----------------------
            gridColumn.Editable = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(gridColumn.Editable);
        }

        [Test]
        public void Test_Editable_WhenSetToTrue_WhenHasPropDefReadOnly_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy { Editable = true };
            var propDef = MockRepository.GenerateStub<IPropDef>();
            propDef.ReadWriteRule = PropReadWriteRule.ReadOnly;
            //---------------Assert Precondition----------------
            Assert.IsTrue(gridColumn.Editable);
            Assert.IsNull(gridColumn.PropDef);
            Assert.AreEqual(PropReadWriteRule.ReadOnly, propDef.ReadWriteRule);
            //---------------Execute Test ----------------------
            gridColumn.SetPropDef(propDef);
            var editable = gridColumn.Editable;
            //---------------Test Result -----------------------
            Assert.IsFalse(editable, "The PropDef ReadOnly Editability should override the loaded GridColumn Editability.");
        }

        [Test]
        public void Test_Editable_WhenSetToTrue_WhenHasPropDefReadWrite_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy { Editable = true };
            var propDef = MockRepository.GenerateStub<IPropDef>();
            propDef.ReadWriteRule = PropReadWriteRule.ReadWrite;
            //---------------Assert Precondition----------------
            Assert.IsTrue(gridColumn.Editable);
            Assert.IsNull(gridColumn.PropDef);
            Assert.AreEqual(PropReadWriteRule.ReadWrite, propDef.ReadWriteRule);
            //---------------Execute Test ----------------------
            gridColumn.SetPropDef(propDef);
            var editable = gridColumn.Editable;
            //---------------Test Result -----------------------
            Assert.IsTrue(editable, "Both PropDef and GridColumn defined as editable so should be editable.");
        }
        [Test]
        public void Test_Editable_WhenSetToFalse_WhenHasPropDefReadWrite_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var gridColumn = new UIGridColumnSpy { Editable = false };
            var propDef = MockRepository.GenerateStub<IPropDef>();
            propDef.ReadWriteRule = PropReadWriteRule.ReadWrite;
            //---------------Assert Precondition----------------
            Assert.IsFalse(gridColumn.Editable);
            Assert.IsNull(gridColumn.PropDef);
            Assert.AreEqual(PropReadWriteRule.ReadWrite, propDef.ReadWriteRule);
            //---------------Execute Test ----------------------
            gridColumn.SetPropDef(propDef);
            var editable = gridColumn.Editable;
            //---------------Test Result -----------------------
            Assert.IsFalse(editable, "The GridColumn Editable False should override the PropDef ReadWrite.");
        }

        private static UIGridColumn GetGridColumn(IClassDef classDef, ILookupList lookupList)
        {
            UIGridColumnSpy gridColumn = new UIGridColumnSpy();
            classDef.GetLookupList(gridColumn.PropertyName);
            classDef.Stub(def => def.GetLookupList(gridColumn.PropertyName)).Return(lookupList);

            gridColumn.SetClassDef(classDef);
            return gridColumn;
        }

        // Grants access to protected fields
        private class UIGridColumnSpy : UIGridColumn
        {
            private IClassDef _setClassDef;
            

            public UIGridColumnSpy() : base("heading", RandomValueGen.GetRandomString(), null, null, true, 100,
                PropAlignment.left, null)
            {}
            
            public UIGridColumnSpy(string propName)
                : base("label", propName, null, null, true, 100, PropAlignment.left, null)
            {}
            public UIGridColumnSpy(string propLabel, string propName)
                : base(propLabel, propName, null, null, true, 100, PropAlignment.left, null)
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
            public void SetClassDef(IClassDef classDef)
            {
                _setClassDef = classDef;
            }
            public override IClassDef ClassDef
            {
                get
                {
                    return _setClassDef ?? base.ClassDef;
                }
            }

            public IPropDef PropDef
            {
                get { return _propDef; }
            }

            public void SetPropDef(IPropDef propDef)
            {
                _propDef = propDef;
            }
        }
    }
}