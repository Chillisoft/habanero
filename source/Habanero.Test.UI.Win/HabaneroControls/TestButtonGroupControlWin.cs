using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestButtonGroupControlWin : TestButtonGroupControl
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

        //    //protected override IReadOnlyGridControl CreateReadOnlyGridControl()
        //    //{
        //    //    ReadOnlyGridControlWin readOnlyGridControlWin = new ReadOnlyGridControlWin();
        //    //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //    //    frm.Controls.Add(readOnlyGridControlWin);
        //    //    return readOnlyGridControlWin;
        //    //}
        [Test]
        public void Test_SetDefaultButton_WinOnly()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            IButton btn = buttons.AddButton("Test");
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)buttons);
            //---------------Execute Test ----------------------
            buttons.SetDefaultButton("Test");
            //---------------Test Result -----------------------
            Assert.AreSame(btn, frm.AcceptButton);
        }

        [Test]
        public void Test_UseMnemonic_WinOnly()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();

            //---------------Execute Test ----------------------
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)buttons.AddButton("Test", delegate { });
            //---------------Test Result -----------------------
            Assert.IsTrue(btn.UseMnemonic);
        }

        [Test]
        public void Test_ButtonIndexer_WithASpecialCharactersInTheName_Failing()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            const string buttonText = "T est@";
            IButton btn = buttons.AddButton(buttonText);
            //---------------Test Result -----------------------
            Assert.AreSame(btn, buttons["T est@"]);
        }
    }
}