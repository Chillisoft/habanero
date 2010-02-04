using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.StandardControls
{
    [TestFixture]
    public class TestLabelVWG : TestLabel
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }

        [Test]
        public void TestPreferredSize()
        {
            //---------------Set up test pack-------------------
            ILabel myLabel = GetControlFactory().CreateLabel();
            string labelText = "sometext";
            myLabel.Text = labelText;

            //---------------Execute Test ----------------------
            int preferredWidth = myLabel.PreferredWidth;
            //---------------Test Result -----------------------

            Assert.AreEqual(labelText.Length * 6, preferredWidth);
            //---------------Tear Down -------------------------          
        }
    }
}