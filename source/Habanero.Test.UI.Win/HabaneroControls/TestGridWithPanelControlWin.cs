using Habanero.BO;
using Habanero.Test.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestGridWithPanelControlWin : TestGridWithPanelControl
    {

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new BusinessObjectControlStubWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        }

        [Test]
        public void TestCancelButton_RestoreChangesGridText()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];

            MyBO currentBO = gridWithPanelControl.CurrentBusinessObject;
            string originalValue = currentBO.TestProp;
            string newValue = BOTestUtils.RandomString;
            currentBO.TestProp = newValue;
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(newValue, currentBO.TestProp);
            Assert.AreEqual(newValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalValue, currentBO.TestProp);
            Assert.AreEqual(originalValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            Assert.AreSame(currentBO, gridWithPanelControl.CurrentBusinessObject);
        }
    }
}