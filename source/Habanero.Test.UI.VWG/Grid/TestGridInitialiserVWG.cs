using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Grid
{
    [TestFixture]
    public class TestGridInitialiserVWG : TestGridInitialiser
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
        }


        protected override Type GetDateTimeGridColumnType()
        {
            throw new NotImplementedException("Not implemented for VWG");
            //return typeof(Habanero.UI.VWG.DataGridViewDateTimeColumn);
        }

        protected override Type GetComboBoxGridColumnType()
        {
            return typeof(Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn);
        }

        protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
        {
            Habanero.UI.VWG.DataGridViewColumnVWG columnWin = (Habanero.UI.VWG.DataGridViewColumnVWG)createdColumn;
            Gizmox.WebGUI.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
        }

        [Test, Ignore("DateTimeColumn needs to be implemented for VWG")]
        public override void TestInitGrid_LoadsDataGridViewDateTimeColumn()
        {
            base.TestInitGrid_LoadsDataGridViewDateTimeColumn();
        }


        [Test]
        public void TestInitGrid_LoadsCustomColumnType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;

            Type customColumnType = typeof(CustomDataGridViewColumnVWG);
            uiGridDef[2].GridControlTypeName = customColumnType.Name; //"CustomDataGridViewColumn";
            uiGridDef[2].GridControlAssemblyName = "Habanero.Test.UI.VWG";
            AddControlToForm(grid);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, grid.Grid.Columns.Count);
            IDataGridViewColumn column3 = grid.Grid.Columns[3];
            Assert.AreEqual("TestDateTime", column3.Name);
            Assert.IsInstanceOfType(typeof(IDataGridViewColumn), column3);
            AssertGridColumnTypeAfterCast(column3, customColumnType);
        }
    
    }
}