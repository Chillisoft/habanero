using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestReadOnlyGridWin : TestReadOnlyGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        }
        [Test]
        public void TestCreateGridBaseWin()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGrid();
            ReadOnlyGridWin readOnlyGrid = (ReadOnlyGridWin)grid;
            ////---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGrid.ReadOnly);
            Assert.IsFalse(readOnlyGrid.AllowUserToAddRows);
            Assert.IsFalse(readOnlyGrid.AllowUserToDeleteRows);
            Assert.IsTrue(readOnlyGrid.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect);
            //---------------Tear Down -------------------------   
        }
    }
}