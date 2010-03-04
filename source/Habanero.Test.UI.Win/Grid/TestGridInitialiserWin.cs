using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestGridInitialiserWin : TestGridInitialiser
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        }

        protected override Type GetDateTimeGridColumnType()
        {
            return typeof(Habanero.UI.Win.DataGridViewDateTimeColumn);
        }

        protected override Type GetComboBoxGridColumnType()
        {
            return typeof(System.Windows.Forms.DataGridViewComboBoxColumn);
        }

        protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
        {
            Habanero.UI.Win.DataGridViewColumnWin columnWin = (Habanero.UI.Win.DataGridViewColumnWin)createdColumn;
            System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
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

            Type customColumnType = typeof(CustomDataGridViewColumnWin);
            uiGridDef[2].GridControlTypeName = customColumnType.Name; //"CustomDataGridViewColumn";
            uiGridDef[2].GridControlAssemblyName = "Habanero.Test.UI.Win";
            AddControlToForm(grid);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, grid.Grid.Columns.Count);
            IDataGridViewColumn column3 = grid.Grid.Columns[3];
            Assert.AreEqual("TestDateTime", column3.Name);
            Assert.IsInstanceOf(typeof(IDataGridViewColumn), column3);
            AssertGridColumnTypeAfterCast(column3, customColumnType);
        }
    }
}