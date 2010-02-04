using System;
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

        protected override Type GetCustomGridColumnType()
        {
            return typeof(CustomDataGridViewColumnVWG);
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
    }
}