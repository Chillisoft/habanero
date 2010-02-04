using System;
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

        protected override Type GetCustomGridColumnType()
        {
            return typeof(CustomDataGridViewColumnWin);
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
    }
}