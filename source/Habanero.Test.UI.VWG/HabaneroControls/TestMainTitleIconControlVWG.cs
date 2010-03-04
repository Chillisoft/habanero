using System;
using System.Drawing;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestMainTitleIconControlVWG : TestMainTitleIconControl
    {
        protected virtual IMainTitleIconControl CreateControl()
        {
            IControlFactory factory = GetControlFactory();
            return new MainTitleIconControlVWG(factory);
        }

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
        protected virtual void AssertBackGroundImageIsSet(IMainTitleIconControl titleIconControl, string headerImage)
        {
            Assert.AreEqual
                (headerImage, ((PanelVWG)titleIconControl.Panel).BackgroundImage.ToString());
        }

        protected virtual object GetBackGroundImage(IMainTitleIconControl titleIconControl)
        {
            return ((LabelVWG)titleIconControl.Icon).BackgroundImage;
        }

        protected virtual void AssertBackGroundImagelayoutCentre(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (Gizmox.WebGUI.Forms.ImageLayout.Center, ((LabelVWG)titleIconControl.Icon).BackgroundImageLayout);
        }

        protected virtual void AssertBackGroundimageIsTile(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (Gizmox.WebGUI.Forms.ImageLayout.Tile, ((PanelVWG)titleIconControl.Panel).BackgroundImageLayout);
        }

        [Test]
        public virtual void Test_Construction_WithControlFactory_Null_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new MainTitleIconControlVWG(null);
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
        public virtual void Test_CreateMainTitleIconControl()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG(factory);
            //---------------Test Result -----------------------

            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(DockStyle.Top, DockStyleVWG.GetDockStyle(titleIconControl.Dock));

            Assert.IsInstanceOf(typeof(IPanel), titleIconControl.Panel);
            const string headerImage = "Images.headergradient.png";
            AssertBackGroundImageIsSet(titleIconControl, headerImage);
            AssertBackGroundimageIsTile(titleIconControl);
            Assert.AreEqual(Color.Transparent, titleIconControl.Panel.BackColor);
            Assert.AreEqual(DockStyle.Top, titleIconControl.Panel.Dock);
            Assert.AreEqual(23, titleIconControl.Panel.Height);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);

            Assert.IsInstanceOf(typeof(ILabel), titleIconControl.Icon);
            Assert.IsNull(GetBackGroundImage(titleIconControl));
            Assert.AreEqual(Color.Transparent, titleIconControl.Icon.BackColor);
            AssertBackGroundImagelayoutCentre(titleIconControl);
            Assert.AreEqual(DockStyle.Left, titleIconControl.Icon.Dock);


            Assert.IsInstanceOf(typeof(ILabel), titleIconControl.Title);
            Assert.IsEmpty("", titleIconControl.Title.Text);
            Assert.AreEqual(DockStyle.Fill, titleIconControl.Title.Dock);
            Assert.AreEqual(Color.Transparent, titleIconControl.Title.BackColor);
            Assert.AreEqual(ContentAlignment.MiddleLeft, titleIconControl.Title.TextAlign);
        }
        [Test]
        public virtual void TestSetIcon()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG(factory);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetIconImage("Images.Valid.gif");
            //---------------Test Result -----------------------
            Assert.AreEqual("Images.Valid.gif", ((LabelVWG)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public virtual void TestSetValidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetValidImage();
            //---------------Test Result -----------------------
            Assert.AreEqual("Images.Valid.gif", ((LabelVWG)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public virtual void TestSetInvalidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetInvalidImage();
            //---------------Test Result -----------------------
            Assert.AreEqual("Images.Invalid.gif", ((LabelVWG)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public virtual void TestRemoveIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.RemoveIconImage();
            //---------------Test Result -----------------------
            Assert.IsNull(((LabelVWG)titleIconControl.Icon).BackgroundImage);
        }

    }
}