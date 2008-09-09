using System.Windows.Forms;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestDialogResult
    {
        [Test]
        public void TestDialogResultVWG_Abort()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.Abort;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.Abort, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Abort.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDialogResultVWG_Cancel()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.Cancel;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.Cancel, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Cancel.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_Ignore()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.Ignore;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.Ignore, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Ignore.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_No()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.No;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.No, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.No.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_None()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.None;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.None, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.None.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_OK()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.OK;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.OK, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.OK.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_Retry()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.Retry;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.Retry, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Retry.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_Yes()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = (Gizmox.WebGUI.Forms.DialogResult) Habanero.UI.Base.DialogResult.Yes;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) Gizmox.WebGUI.Forms.DialogResult.Yes, (int) formVWG.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Yes.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDialogResultWin_Abort()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.Abort;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.Abort, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Abort.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDialogResultWin_Cancel()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.Cancel;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.Cancel, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Cancel.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultWin_Ignore()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.Ignore;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.Ignore, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Ignore.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultWin_No()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.No;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.No, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.No.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultWin_None()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.None;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.None, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.None.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultWin_OK()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.OK;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.OK, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.OK.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultWin_Retry()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.Retry;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.Retry, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Retry.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultWin_Yes()
        {
            //---------------Set up test pack-------------------
            FormWin formWin = new FormWin();

            //---------------Execute Test ----------------------
            formWin.DialogResult = (DialogResult) Habanero.UI.Base.DialogResult.Yes;
            //---------------Test Result -----------------------
            Assert.AreEqual((int) DialogResult.Yes, (int) formWin.DialogResult);
            Assert.AreEqual(Habanero.UI.Base.DialogResult.Yes.ToString(), formWin.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }
    }
}