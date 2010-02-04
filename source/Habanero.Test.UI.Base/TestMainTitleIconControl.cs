// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Drawing;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the MainTitleIconControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_MainTitleIconControl : TestBaseMethods.TestBaseMethodsVWG
    {
        private IControlFactory _factory;

        protected override IControlFactory GetControlFactory()
        {
            if (_factory == null)
            {
                _factory = CreateNewControlFactory();
                GlobalUIRegistry.ControlFactory = _factory;
            }
            return _factory;
        }

        protected virtual ControlFactoryVWG CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IControlHabanero CreateControl()
        {
            return new MainTitleIconControlVWG(GetControlFactory());
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the MainTitleIconControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_MainTitleIconControl : TestBaseMethods.TestBaseMethodsWin
    {
        private IControlFactory _factory;

        protected override IControlFactory GetControlFactory()
        {
            if (_factory == null)
            {
                _factory = CreateNewControlFactory();
                GlobalUIRegistry.ControlFactory = _factory;
            }
            return _factory;
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IControlHabanero CreateControl()
        {
            return new MainTitleIconControlWin(GetControlFactory());
        }
    }

    [TestFixture]
    public class TestMainTitleIconControlVWG
    {
        private IControlFactory _factory;

        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetControlFactory();
        }

        protected virtual IMainTitleIconControl CreateControl()
        {
            //            return GetControlFactory().CreateMainMenu();
            GetControlFactory();
            return new MainTitleIconControlVWG();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            if (_factory == null)
            {
                _factory = CreateNewControlFactory();
                GlobalUIRegistry.ControlFactory = _factory;
            }
            return _factory;
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        [Test]
        public virtual void Test_Construction_WithControlFactory_ShouldSetControlFactory()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateNewControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            MainTitleIconControlVWG outlookStyleMenu = new MainTitleIconControlVWG(factory);
            //---------------Test Result -----------------------
            Assert.AreSame(factory, outlookStyleMenu.ControlFactory);
        }

        [Test]
        public virtual void Test_Construction_WithNoControlFactory_ShouldSetControlFactory_GlobalUIFactory()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            MainTitleIconControlVWG outlookStyleMenu = new MainTitleIconControlVWG();
            //---------------Test Result -----------------------
            Assert.AreSame(factory, outlookStyleMenu.ControlFactory);
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

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG();
            //---------------Test Result -----------------------

            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(DockStyle.Top, DockStyleVWG.GetDockStyle(titleIconControl.Dock));

            Assert.IsInstanceOfType(typeof (IPanel), titleIconControl.Panel);
            const string headerImage = "Images.headergradient.png";
            AssertBackGroundImageIsSet(titleIconControl, headerImage);
            AssertBackGroundimageIsTile(titleIconControl);
            Assert.AreEqual(Color.Transparent, titleIconControl.Panel.BackColor);
            Assert.AreEqual(DockStyle.Top, titleIconControl.Panel.Dock);
            Assert.AreEqual(23, titleIconControl.Panel.Height);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);

            Assert.IsInstanceOfType(typeof (ILabel), titleIconControl.Icon);
            Assert.IsNull(GetBackGroundImage(titleIconControl));
            Assert.AreEqual(Color.Transparent, titleIconControl.Icon.BackColor);
            AssertBackGroundImagelayoutCentre(titleIconControl);
            Assert.AreEqual(DockStyle.Left, titleIconControl.Icon.Dock);


            Assert.IsInstanceOfType(typeof (ILabel), titleIconControl.Title);
            Assert.IsEmpty("", titleIconControl.Title.Text);
            Assert.AreEqual(DockStyle.Fill, titleIconControl.Title.Dock);
            Assert.AreEqual(Color.Transparent, titleIconControl.Title.BackColor);
            Assert.AreEqual(ContentAlignment.MiddleLeft, titleIconControl.Title.TextAlign);
        }

        protected virtual void AssertBackGroundImageIsSet(IMainTitleIconControl titleIconControl, string headerImage)
        {
            Assert.AreEqual
                (headerImage, ((PanelVWG) titleIconControl.Panel).BackgroundImage.ToString());
        }

        protected virtual object GetBackGroundImage(IMainTitleIconControl titleIconControl)
        {
            return ((LabelVWG) titleIconControl.Icon).BackgroundImage;
        }

        protected virtual void AssertBackGroundImagelayoutCentre(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (Gizmox.WebGUI.Forms.ImageLayout.Center, ((LabelVWG) titleIconControl.Icon).BackgroundImageLayout);
        }

        protected virtual void AssertBackGroundimageIsTile(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (Gizmox.WebGUI.Forms.ImageLayout.Tile, ((PanelVWG) titleIconControl.Panel).BackgroundImageLayout);
        }

        [Test]
        public virtual void TestSetIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetIconImage("Images.Valid.gif");
            //---------------Test Result -----------------------
            Assert.AreEqual("Images.Valid.gif", ((LabelVWG) titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public virtual void TestSetValidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetValidImage();
            //---------------Test Result -----------------------
            Assert.AreEqual("Images.Valid.gif", ((LabelVWG) titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public virtual void TestSetInvalidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetInvalidImage();
            //---------------Test Result -----------------------
            Assert.AreEqual("Images.Invalid.gif", ((LabelVWG) titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public virtual void TestRemoveIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlVWG titleIconControl = new MainTitleIconControlVWG();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.RemoveIconImage();
            //---------------Test Result -----------------------
            Assert.IsNull(((LabelVWG) titleIconControl.Icon).BackgroundImage);
        }

        [Test]
        public virtual void TestSetTitle()
        {
            //---------------Set up test pack-------------------
            IMainTitleIconControl titleIconControl = GetControlFactory().CreateMainTitleIconControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.Title.Text = "Test";
            //---------------Test Result -----------------------
            Assert.AreEqual("Test", titleIconControl.Title.Text);
        }
    }

    [TestFixture]
    public class TestMainTitleIconControlWin : TestMainTitleIconControlVWG
    {
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
        protected override IMainTitleIconControl CreateControl()
        {
            //            return GetControlFactory().CreateMainMenu();
            GetControlFactory();
            return new MainTitleIconControlWin();
        }
    }
}