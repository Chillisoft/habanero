using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.ButtonsControl
{
    [TestFixture]
    public class TestButtonSizePolicyUserDefined
    {

        [Test]
        public void TestButtonWidthPolicy_UserDefined()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttonGroupControl = new ControlFactoryWin().CreateButtonGroupControl();
            Size buttonSize = new Size(20, 50);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            buttonGroupControl.ButtonSizePolicy = new ButtonSizePolicyUserDefined();
            IButton btnTest1 = buttonGroupControl.AddButton("");
            btnTest1.Size = buttonSize;

            buttonGroupControl.AddButton("Bigger button");
            //---------------Test Result -----------------------

            Assert.AreEqual(buttonSize, btnTest1.Size);

        }
    }


}
