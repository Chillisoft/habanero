using System.Windows.Forms;
using Habanero.Ui.Generic;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
{
    /// <summary>
    /// Summary description for TestControlFactory.
    /// </summary>
    [TestFixture]
    public class TestControlFactory
    {
        public TestControlFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [Test]
        public void TestCreateLabel()
        {
            Label lbl2 = ControlFactory.CreateLabel("Test2", false);
            Assert.AreEqual("Test2", lbl2.Text, "Text of label not set properly in CreateLabel.");
            Assert.AreEqual(FlatStyle.System, lbl2.FlatStyle, "FlatStyle should be flat for all labels.");
            Assert.AreEqual(lbl2.PreferredWidth, lbl2.Width, "Width should equal preferredwidth");

            //			Label lbl = ControlFactory.CreateLabel("Test", true);
            //			Assert.AreEqual("Test", lbl.Text, "Text of label not set properly in CreateLabel.");
            //			Assert.IsTrue(lbl.Font.Bold, "Font of label should be bold.");
        }

        [Test]
        public void TestCreateButton()
        {
            Button btn = ControlFactory.CreateButton("Button1");
            Assert.AreEqual("Button1", btn.Text, "Text of button not set properly in CreateButton");
            Assert.AreEqual("Button1", btn.Name, "Name of button not set properly in CreateButton");
            Assert.AreEqual(FlatStyle.System, btn.FlatStyle, "FlatStyle should be set in CreateButton");
            Assert.AreEqual(ControlFactory.CreateLabel("Button1", false).PreferredWidth + 20, btn.Width,
                            "Width of button from CreateButton should be 20 more than the preferredwidth of a label with the same text.");
        }

        [Test]
        public void TestCreateControl()
        {
            Control ctl = ControlFactory.CreateControl(typeof (Button));
            Assert.AreSame(typeof (Button), ctl.GetType(), "Create Control is not creating controls.");
        }
    }
}