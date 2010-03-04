using Habanero.Base.Exceptions;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Selectors
{
    [TestFixture]
    public class TestBOSelectorEditableGridControlVWG : TestBOSelectorEditableGridControl
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)gridControl);
            return gridControl;
        }

        [Test]
        public void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new EditableGridControlVWG(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("Cannot create an editable grid control if the control factory is null", ex.Message);
            }
        }

    }
}