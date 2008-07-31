using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.Test.UI.Base
{
    public abstract class TestOKCancelDialog
    {
        //TODO: refactor - WIN and GIZ are copied and pasted.
        protected abstract IControlFactory GetControlFactory();


        [TestFixture]
        public class TestOKCancelDialogGiz : TestOKCancelDialog
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [TestFixture]
        public class TestOKCancelDialogWin : TestOKCancelDialog
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            [Test]
            public void TestCreateOKCancelForm()
            {
                //---------------Set up test pack-------------------
                IOKCancelDialogFactory okCancelDialogFactory = GetControlFactory().CreateOKCancelDialogFactory();
                    //---------------Execute Test ----------------------
                IFormChilli dialogForm = okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel());

                //---------------Test Result -----------------------
                Assert.AreEqual(1, dialogForm.Controls.Count);
                Assert.AreEqual(DockStyle.Fill, dialogForm.Controls[0].Dock);
                //---------------Tear Down -------------------------
            }

            [Test]
            public void TestOKButtonAcceptButton()
            {
                //---------------Set up test pack-------------------
                IOKCancelDialogFactory okCancelDialogFactory = GetControlFactory().CreateOKCancelDialogFactory();
                //---------------Execute Test ----------------------
                FormWin dialogForm = (FormWin) okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel());
                //---------------Test Result -----------------------
                IButtonGroupControl buttons = (IButtonGroupControl) dialogForm.Controls[0].Controls[1];
                Assert.AreSame(buttons["OK"], dialogForm.AcceptButton);
                //---------------Tear Down -------------------------
            }

            [Test]
            public void TestDialogResultWhenOkClicked()
            {
                //---------------Set up test pack-------------------
                OKCancelDialogFactoryWin okCancelDialogFactory = (OKCancelDialogFactoryWin) GetControlFactory().CreateOKCancelDialogFactory();
                FormWin dialogForm = (FormWin) okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel());

                //---------------Execute Test ----------------------
                okCancelDialogFactory.OkButton_ClickHandler(dialogForm);
                //---------------Test Result -----------------------
                Assert.AreEqual(dialogForm.DialogResult, System.Windows.Forms.DialogResult.OK);
                //---------------Tear Down -------------------------
            }
            [Test]
            public void TestDialogResultWhenCancelClicked()
            {
                //---------------Set up test pack-------------------
                OKCancelDialogFactoryWin okCancelDialogFactory = (OKCancelDialogFactoryWin)GetControlFactory().CreateOKCancelDialogFactory();
                FormWin dialogForm = (FormWin)okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel());

                //---------------Execute Test ----------------------
                okCancelDialogFactory.CancelButton_ClickHandler(dialogForm);
                //---------------Test Result -----------------------
                Assert.AreEqual(dialogForm.DialogResult, System.Windows.Forms.DialogResult.Cancel);
                //---------------Tear Down -------------------------
            }
        }

        [Test]
        public void TestLayout()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = GetControlFactory().CreateOKCancelDialogFactory();

            //---------------Execute Test ----------------------
            IControlChilli dialogControl = okCancelDialogFactory.CreateOKCancelPanel(GetControlFactory().CreatePanel());
            //---------------Test Result -----------------------
            Assert.AreEqual(2, dialogControl.Controls.Count);
            Assert.IsInstanceOfType(typeof (IPanel), dialogControl.Controls[0]);
            IPanel contentPanel = (IPanel) dialogControl.Controls[0];
            Assert.AreEqual(DockStyle.Fill, contentPanel.Dock);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), dialogControl.Controls[1]);
            IButtonGroupControl buttons = (IButtonGroupControl) dialogControl.Controls[1];
            Assert.AreEqual(DockStyle.Bottom, buttons.Dock);
            Assert.AreEqual(2, buttons.Controls.Count);
            Assert.IsNotNull(buttons["OK"]);
            Assert.IsNotNull(buttons["Cancel"]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestNestedControl()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = GetControlFactory().CreateOKCancelDialogFactory();
            IControlChilli nestedControl = GetControlFactory().CreatePanel();

            //---------------Execute Test ----------------------
            IControlChilli dialogControl = okCancelDialogFactory.CreateOKCancelPanel(nestedControl);
            IPanel contentPanel = (IPanel) dialogControl.Controls[0];

            //---------------Test Result -----------------------
            Assert.AreEqual(1, contentPanel.Controls.Count);
            Assert.AreSame(nestedControl, contentPanel.Controls[0]);
            Assert.AreEqual(DockStyle.Fill, nestedControl.Dock);
            //---------------Tear Down -------------------------
        }
    }
}