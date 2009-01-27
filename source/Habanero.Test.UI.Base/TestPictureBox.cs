using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_PictureBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePictureBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_PictureBox : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePictureBox();
        }
    }

    /// <summary>
    /// This test class tests the Panel class.
    /// </summary>
    public abstract class TestPictureBox
    {
        protected abstract IControlFactory GetControlFactory();

        protected abstract string GetUnderlyingSizeModeToString(IPictureBox pictureBox);

        protected IPictureBox CreatePictureBox()
        {
            IPictureBox pictureBox = GetControlFactory().CreatePictureBox();
            return pictureBox;
        }

        [TestFixture]
        public class TestPictureBoxWin : TestPictureBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
            {
                System.Windows.Forms.PictureBox control = (System.Windows.Forms.PictureBox)pictureBox;
                return control.SizeMode.ToString();
            }
        }

        [TestFixture]
        public class TestPictureBoxVWG : TestPictureBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
            {
                Gizmox.WebGUI.Forms.PictureBox control = (Gizmox.WebGUI.Forms.PictureBox)pictureBox;
                return control.SizeMode.ToString();
            }
        }

        [Test]
        public void Test_SizeMode()
        {
            //---------------Set up test pack-------------------
            IPictureBox pictureBox = CreatePictureBox();
            PictureBoxSizeMode sizeMode = PictureBoxSizeMode.AutoSize;
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            pictureBox.SizeMode = sizeMode;
            //---------------Test Result -----------------------
            Assert.AreEqual(sizeMode.ToString(), GetUnderlyingSizeModeToString(pictureBox));
        }
    }
}
