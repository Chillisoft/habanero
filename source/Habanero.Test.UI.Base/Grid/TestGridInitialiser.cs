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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestGridInitialiser
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            // base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IControlHabanero cntrl);


        //[TestFixture]
        //public class TestGridInitialiserWin : TestGridInitialiser
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }

        //    protected override void AddControlToForm(IControlHabanero cntrl)
        //    {
        //        System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //        frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        //    }
        //}

        [TestFixture]
        public class TestGridInitialiserVWG : TestGridInitialiser
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override void AddControlToForm(IControlHabanero cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) cntrl);
            }
        }

        [Test]
        public void Test_InitialiseGrid_NoClassDef_NoColumnsDefined()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
//            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(grid.IsInitialised);

            //---------------Execute Test ----------------------
            try
            {
                grid.Initialise();
                Assert.Fail("Should raise error");
            }
            catch (GridBaseInitialiseException ex)
            {
                StringAssert.Contains
                    ("You cannot call initialise with no classdef since the ID column has not been added to the grid",
                     ex.Message);
            }
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_InitialiseGrid_NoClassDef_IDColumnNotDefined()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
//            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(grid.IsInitialised);
            Assert.AreEqual(0, grid.Grid.Columns.Count);
            //---------------Execute Test ----------------------
            try
            {
                grid.Grid.Columns.Add("new col", "col");
                grid.Initialise();
                Assert.Fail("Should raise error");
            }
            catch (GridBaseInitialiseException ex)
            {
                StringAssert.Contains
                    ("You cannot call initialise with no classdef since the ID column has not been added to the grid",
                     ex.Message);
            }
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_InitialiseGrid_NoClassDef_Twice()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
//            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            grid.Grid.Columns.Add("ID", "ID");
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(grid.IsInitialised);

            //---------------Execute Test ----------------------
            try
            {
                grid.Initialise();
                grid.Initialise();
                Assert.Fail("Should raise error");
            }
            catch (GridBaseSetUpException ex)
            {
                StringAssert.Contains("You cannot initialise the grid more than once", ex.Message);
            }
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitialiseGrid()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
//            Assert.AreEqual("", grid.UiDefName);
            Assert.IsNull(grid.ClassDef);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual("default", grid.UiDefName);
            Assert.AreEqual(classDef, grid.ClassDef);
            Assert.AreEqual
                (uiGridDef.Count + 1, grid.Grid.Columns.Count,
                 "There should be 1 ID column and 2 defined columns in the defaultDef");
            Assert.IsTrue(initialiser.IsInitialised);
            Assert.AreSame(grid, initialiser.Grid);
//            Assert.IsTrue(grid.IsInitialised);
        }

        [Test]
        public void TestInitGrid_DefaultUIDef_VerifyColumnsSetupCorrectly()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            UIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            UIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            //---------------Test Result -----------------------
            IDataGridViewColumn idColumn = grid.Grid.Columns[0];
            AssertVerifyIDFieldSetUpCorrectly(idColumn);

            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);

            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_UIDef_DateFormat_FormatsDateColumn()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            AddControlToForm(grid);

            //--------------Assert PreConditions----------------            
            string formattedPropertyName = "TestDateTimeFormat";
            Assert.IsNotNull(uiGridDef[formattedPropertyName]);
            Assert.IsNotNull(uiGridDef["TestDateTimeNoFormat"]);
            Assert.IsNotNull(uiGridDef["TestDateTime"]);

            Assert.IsNull(uiGridDef["TestDateTimeNoFormat"].GetParameterValue("dateFormat"));
            Assert.IsNull(uiGridDef["TestDateTime"].GetParameterValue("dateFormat"));
            object dateFormatObject = uiGridDef[formattedPropertyName].GetParameterValue("dateFormat");
            string dateFormatParameter = dateFormatObject.ToString();
            string expectedFormat = "dd.MMM.yyyy";
            Assert.AreEqual(expectedFormat, dateFormatParameter);

            MyBO myBo = new MyBO();
            DateTime currentDateTime = DateTime.Now;
            myBo.SetPropertyValue(formattedPropertyName, currentDateTime);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(myBo);

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            grid.SetBusinessObjectCollection(col);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            IDataGridViewCell dataGridViewCell = grid.Grid.Rows[0].Cells[formattedPropertyName];
            //((DataGridViewCellVWG) dataGridViewCell).DataGridViewCell.HasStyle = false;
            Assert.AreSame(typeof (DateTime), dataGridViewCell.ValueType);
            Assert.AreEqual(currentDateTime.ToString(expectedFormat), dataGridViewCell.FormattedValue);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestInitGrid_WithNonDefaultUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            string alternateUIDefName = "Alternate";
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            UIDef uiDef = classDef.UIDefCol[alternateUIDefName];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, uiGridDef.Count, "1 defined column in the alternateUIDef");
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef, alternateUIDefName);

            //---------------Test Result -----------------------
            Assert.AreEqual(alternateUIDefName, grid.UiDefName);
            Assert.AreEqual(classDef, grid.ClassDef);
            Assert.AreEqual
                (uiGridDef.Count + 1, grid.Grid.Columns.Count,
                 "There should be 1 ID column and 1 defined column in the alternateUIDef");
            //---------------Tear Down -------------------------          
        }

        public void TestInitGrid_Twice_Fail()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            ClassDef classDef = LoadMyBoDefaultClassDef();
            //---------------Assert Preconditions---------------
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            UIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            UIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            IDataGridViewColumn idColumn = grid.Grid.Columns[0];
            AssertVerifyIDFieldSetUpCorrectly(idColumn);

            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);

            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);
        }

        [Test]
        public void TestInitGrid_WithInvalidUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());

            //---------------Execute Test ----------------------
            try
            {
                initialiser.InitialiseGrid(classDef, "NonExistantUIDef");
                Assert.Fail("Should raise an error if the class def does not have the UIDef");
                //---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(" does not contain a definition for UIDef ", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_With_NoGridDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            //---------------Execute Test ----------------------
            try
            {
                initialiser.InitialiseGrid(classDef, "AlternateNoGrid");
                Assert.Fail("Should raise an error if the class def does not the GridDef");
                //---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains
                    (" does not contain a grid definition for UIDef AlternateNoGrid for the class def ", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

        private IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return GetControlFactory().CreateReadOnlyGridControl();
        }

        private static ClassDef LoadMyBoDefaultClassDef()
        {
            return MyBO.LoadDefaultClassDef();
        }

        private static void AssertVerifyIDFieldSetUpCorrectly(IDataGridViewColumn column)
        {
            string idPropertyName = "ID";
            Assert.AreEqual(idPropertyName, column.Name);
            Assert.AreEqual(idPropertyName, column.HeaderText);
            Assert.AreEqual(idPropertyName, column.DataPropertyName);
            Assert.IsTrue(column.ReadOnly);
            Assert.IsFalse(column.Visible);
            Assert.AreEqual(typeof (string), column.ValueType);
        }

        private static void AssertThatDataColumnSetupCorrectly
            (ClassDef classDef, UIGridColumn columnDef1, IDataGridViewColumn dataColumn1)
        {
            Assert.AreEqual(columnDef1.PropertyName, dataColumn1.DataPropertyName); //Test Prop
            Assert.AreEqual(columnDef1.PropertyName, dataColumn1.Name);
            Assert.AreEqual(columnDef1.GetHeading(), dataColumn1.HeaderText);
            Assert.IsTrue(dataColumn1.Visible);
//            Assert.IsTrue(dataColumn1.ReadOnly); TODO: put this test into the readonlygridinitialiser
            Assert.AreEqual(columnDef1.Width, dataColumn1.Width);
            IPropDef propDef = GetPropDef(classDef, columnDef1);
            Assert.AreEqual(propDef.PropertyType, dataColumn1.ValueType);
        }

        private static IPropDef GetPropDef(ClassDef classDef, UIGridColumn gridColumn)
        {
            IPropDef propDef = null;
            if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
            {
                propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
            }
            return propDef;
        }
    }
}