using System;
using System.Drawing;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestMainTitleIconControlWin : TestMainTitleIconControl
    {
        protected virtual IMainTitleIconControl CreateControl()
        {
            GetControlFactory();
            return new MainTitleIconControlWin();
        }
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }

        public override void Test_Construction_WithControlFactory_ShouldSetControlFactory()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateNewControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainTitleIconControl outlookStyleMenu = new MainTitleIconControlWin(factory);
            //---------------Test Result -----------------------
            Assert.AreSame(factory, outlookStyleMenu.ControlFactory);
        }

        public override void Test_Construction_WithNoControlFactory_ShouldSetControlFactory_GlobalUIFactory()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainTitleIconControl outlookStyleMenu = new MainTitleIconControlWin();
            //---------------Test Result -----------------------
            Assert.AreSame(factory, outlookStyleMenu.ControlFactory);
        }

        public override void Test_Construction_WithControlFactory_Null_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new MainTitleIconControlWin(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }
        [Test]
        public override void Test_CreateMainTitleIconControl()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin();
            //---------------Test Result -----------------------

            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(DockStyle.Top, DockStyleWin.GetDockStyle(titleIconControl.Dock));

            Assert.IsInstanceOfType(typeof(IPanel), titleIconControl.Panel);
            const string headerImage = "Images.headergradient.png";
            AssertBackGroundImageIsSet(titleIconControl, headerImage);
            AssertBackGroundimageIsTile(titleIconControl);
            Assert.AreEqual(Color.Transparent, titleIconControl.Panel.BackColor);
            Assert.AreEqual(DockStyle.Top, titleIconControl.Panel.Dock);
            Assert.AreEqual(23, titleIconControl.Panel.Height);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);

            Assert.IsInstanceOfType(typeof(ILabel), titleIconControl.Icon);
            Assert.IsNull(GetBackGroundImage(titleIconControl));
            Assert.AreEqual(Color.Transparent, titleIconControl.Icon.BackColor);
            AssertBackGroundImagelayoutCentre(titleIconControl);
            Assert.AreEqual(DockStyle.Left, titleIconControl.Icon.Dock);


            Assert.IsInstanceOfType(typeof(ILabel), titleIconControl.Title);
            Assert.IsEmpty("", titleIconControl.Title.Text);
            Assert.AreEqual(DockStyle.Fill, titleIconControl.Title.Dock);
            Assert.AreEqual(Color.Transparent, titleIconControl.Title.BackColor);
            Assert.AreEqual(ContentAlignment.MiddleLeft, titleIconControl.Title.TextAlign);
        }



        protected override void AssertBackGroundImageIsSet(IMainTitleIconControl titleIconControl, string headerImage)
        {
            //TODO Brett 20 Apr 2009: Nubb to fix
            //Assert.AreEqual
            //    (headerImage, ((PanelWin)titleIconControl.Panel).BackgroundImage.ToString());
        }

        protected override object GetBackGroundImage(IMainTitleIconControl titleIconControl)
        {
            return ((LabelWin)titleIconControl.Icon).BackgroundImage;
        }

        protected override void AssertBackGroundImagelayoutCentre(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (System.Windows.Forms.ImageLayout.Center, ((LabelWin)titleIconControl.Icon).BackgroundImageLayout);
        }

        protected override void AssertBackGroundimageIsTile(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (System.Windows.Forms.ImageLayout.Tile, ((PanelWin)titleIconControl.Panel).BackgroundImageLayout);
        }

        [Test]
        public override void TestSetIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetIconImage("Images.Valid.gif");
            //---------------Test Result -----------------------
            //TODO Brett 20 Apr 2009: Nubb to fix
            //Assert.AreEqual("Images.Valid.gif", ((LabelWin)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public override void TestSetValidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetValidImage();
            //---------------Test Result -----------------------
            //TODO Brett 20 Apr 2009: Nubb to fix  Assert.AreEqual("Images.Valid.gif", ((LabelWin)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public override void TestSetInvalidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetInvalidImage();
            //---------------Test Result -----------------------
            //TODO Brett 20 Apr 2009: Nubb to fix  Assert.AreEqual("Images.Invalid.gif", ((LabelWin)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public override void TestRemoveIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.RemoveIconImage();
            //---------------Test Result -----------------------
            Assert.IsNull(((LabelWin)titleIconControl.Icon).BackgroundImage);
        }

    }
}