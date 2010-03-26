using Habanero.Test.UI.Base.Grid;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestDataGridViewWin : TestDataGridView
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
        {
            System.Windows.Forms.DataGridView control = (System.Windows.Forms.DataGridView)dataGridView;
            return control.SelectionMode.ToString();
        }

        protected override void AddToForm(IDataGridView dgv)
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Controls.Add((System.Windows.Forms.Control)dgv);
        }

        [Test]
        public void Test_GetItemsPerPage_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int itemsPerPage = gridViewWin.ItemsPerPage;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, itemsPerPage);
        }
        [Test]
        public void Test_SetItemsPerPage_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridViewWin.ItemsPerPage = 22;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, gridViewWin.ItemsPerPage);
        }
        [Test]
        public void Test_GetTotalItems_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int totalItems = gridViewWin.TotalItems;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, totalItems);
        }
        [Test]
        public void Test_SetTotalItems_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridViewWin.TotalItems = 22;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, gridViewWin.TotalItems);
        }
        [Test]
        public void Test_GetTotalPages_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int totalPages = gridViewWin.TotalPages;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, totalPages);
        }
        [Test]
        public void Test_SetTotalPages_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridViewWin.TotalPages = 22;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, gridViewWin.TotalPages);
        }
    }
}