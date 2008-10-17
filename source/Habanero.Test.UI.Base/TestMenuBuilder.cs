//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestMenuBuilder
    {
        protected abstract IControlFactory GetControlFactory();
        protected abstract IMenuBuilder CreateMenuBuilder();
        protected abstract IFormControl CreateFormControlStub();
        protected abstract bool IsMenuDocked(IMainMenuHabanero menu, IFormHabanero form);
        protected abstract void AssertControlDockedInForm(IControlHabanero habanero, IFormHabanero frm);

        protected HabaneroMenu CreateHabaneroMenuFullySetup()
        {
            IControlFactory controlFactory = GetControlFactory();
            IFormHabanero form = controlFactory.CreateForm();
            return new HabaneroMenu("Main", form, controlFactory);
        }

        [TestFixture]
        public class TestMenuBuilderWin : TestMenuBuilder
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override IMenuBuilder CreateMenuBuilder()
            {
                return new MenuBuilderWin();
            }

            protected override IFormControl CreateFormControlStub()
            {
                return new FormControlStubWin();
            }

            protected override bool IsMenuDocked(IMainMenuHabanero menu, IFormHabanero form)
            {
                System.Windows.Forms.Form formWin = (System.Windows.Forms.Form)form;
                return formWin.Menu == menu && form.IsMdiContainer;
            }

            protected override void AssertControlDockedInForm(IControlHabanero control, IFormHabanero form)
            {
                System.Windows.Forms.Form winForm = (System.Windows.Forms.Form)form;
                Assert.LessOrEqual(1, winForm.MdiChildren.Length);
                bool found = false;
                foreach (Form childForm in winForm.MdiChildren)
                {
                    Assert.AreEqual(1, childForm.Controls.Count);
                    Control childFormControl = childForm.Controls[0];
                    if (childFormControl == control)
                    {
                        found = true;
                        //Assert.AreSame(childForm, winForm.ActiveMdiChild,
                        //               "Control found in MDI children, but not the current docked form");
                        break;
                    }
                }
                Assert.IsTrue(found, "Form was not found");
            }

            [Test]
            public void TestCloseFormAndClickCreatesNewForm()
            {
                //---------------Set up test pack-------------------
                HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
                IFormHabanero frm = habaneroMenu.Form;
                frm.Show();
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
    

                menuItem.FormControlCreator += delegate { return new FormControlStubWin(); };
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                menu.DockInForm(habaneroMenu.Form);
                IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
                formsMenuItem.PerformClick();
                System.Windows.Forms.Form winForm = (System.Windows.Forms.Form)frm;
                System.Windows.Forms.Form childForm = winForm.MdiChildren[0];
                System.Windows.Forms.Control expectedFormControl = childForm.Controls[0];
                //---------------Execute Test ----------------------

                childForm.Close();
                formsMenuItem.PerformClick();
                //---------------Test Result -----------------------

                Assert.AreEqual(1, winForm.MdiChildren.Length);
                childForm = winForm.MdiChildren[0];
                Assert.AreEqual(1, childForm.Controls.Count);
                Assert.IsInstanceOfType(typeof(FormControlStubWin), winForm.MdiChildren[0].Controls[0]);
                Assert.AreNotSame(expectedFormControl, winForm.MdiChildren[0].Controls[0]);
                //---------------Tear down -------------------------

            }

            private class FormControlStubWin : UserControlWin, IFormControl
            {
                public void SetForm(IFormHabanero form)
                {

                }
            }
        }


        [TestFixture]
        public class TestMenuBuilderVWG : TestMenuBuilder
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override IMenuBuilder CreateMenuBuilder()
            {
                return new MenuBuilderVWG();
            }

            protected override IFormControl CreateFormControlStub()
            {
                return new FormControlStubVWG();
            }
            protected override bool IsMenuDocked(IMainMenuHabanero menu, IFormHabanero form)
            {
                Gizmox.WebGUI.Forms.Form formWin = (Gizmox.WebGUI.Forms.Form)form;
                return formWin.Menu == menu && form.Controls.Count == 1;
            }
            
            protected override void AssertControlDockedInForm(IControlHabanero control, IFormHabanero form)
            {
                Assert.AreEqual(1, form.Controls.Count, "No container control found in form");
                IControlHabanero contentControl = form.Controls[0];
                Assert.AreEqual(1, contentControl.Controls.Count);
                Assert.AreSame(control, contentControl.Controls[0]);
                Assert.AreEqual(Habanero.UI.Base.DockStyle.Fill, control.Dock);
            }

            private class FormControlStubVWG : UserControlVWG, IFormControl
            {
                public void SetForm(IFormHabanero form)
                {

                }
            }
        }

        [Test]
        public void TestSimpleMenuStructure()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.CreateRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(subMenuName);
            string menuItemName = TestUtil.CreateRandomString();
            submenu.AddMenuItem(menuItemName);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            Assert.AreEqual(subMenuName, menu.MenuItems[0].Text);
            Assert.AreEqual(1, menu.MenuItems[0].MenuItems.Count);
            Assert.AreEqual(menuItemName, menu.MenuItems[0].MenuItems[0].Text);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestMultipleSubmenus()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName1 = TestUtil.CreateRandomString();
            HabaneroMenu submenu1 = habaneroMenu.AddSubmenu(subMenuName1);
            string subMenuName2 = TestUtil.CreateRandomString();
            HabaneroMenu submenu2 = habaneroMenu.AddSubmenu(subMenuName2);
            submenu1.AddMenuItem(TestUtil.CreateRandomString());
            submenu2.AddMenuItem(TestUtil.CreateRandomString());
            submenu2.AddMenuItem(TestUtil.CreateRandomString());
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, menu.MenuItems.Count);
            Assert.AreEqual(subMenuName1, menu.MenuItems[0].Text);
            Assert.AreEqual(1, menu.MenuItems[0].MenuItems.Count);
            Assert.AreEqual(subMenuName2, menu.MenuItems[1].Text);
            Assert.AreEqual(2, menu.MenuItems[1].MenuItems.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestMultipleItems()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.CreateRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(subMenuName);
            string menuItemName1 = TestUtil.CreateRandomString();
            submenu.AddMenuItem(menuItemName1);
            string menuItemName2 = TestUtil.CreateRandomString();
            submenu.AddMenuItem(menuItemName2);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            Assert.AreEqual(2, menu.MenuItems[0].MenuItems.Count);
            Assert.AreEqual(menuItemName1, menu.MenuItems[0].MenuItems[0].Text);
            Assert.AreEqual(menuItemName2, menu.MenuItems[0].MenuItems[1].Text);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestMultiLevels()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.CreateRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(subMenuName);
            string menuItemName1 = TestUtil.CreateRandomString();
            submenu.AddMenuItem(menuItemName1);

            string subsubMenuName = TestUtil.CreateRandomString();
            HabaneroMenu subsubmenu = submenu.AddSubmenu(subsubMenuName);

            string menuItemName2 = TestUtil.CreateRandomString();
            subsubmenu.AddMenuItem(menuItemName2);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            IMenuItem createdSubMenu = menu.MenuItems[0];
            Assert.AreEqual(2, createdSubMenu.MenuItems.Count);
            IMenuItem createdSubsubMenu = createdSubMenu.MenuItems[0];
            Assert.AreEqual(1, createdSubsubMenu.MenuItems.Count);
            Assert.AreEqual(menuItemName1, createdSubMenu.MenuItems[1].Text);
            Assert.AreEqual(menuItemName2, createdSubsubMenu.MenuItems[0].Text);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNoCreatorsCalledWhenMenuFormNotSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            bool called = false;
            menuItem.FormControlCreator = delegate
            {
                called = true;
                return CreateFormControlStub();
            };
            menuItem.ControlManagerCreator = delegate
            {
                called = true;
                return new ControlManagerStub(GetControlFactory());
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsFalse(called);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFormControlCreatorCalledOnClickIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            bool called = false;
            FormControlCreator formControlCreatorDelegate = delegate
            {
                called = true;
                return CreateFormControlStub();
            };
            menuItem.FormControlCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();
            
            //---------------Test Result -----------------------
            Assert.IsTrue(called);
            //---------------Tear Down -------------------------          
        }

        

        [Test]
        public void TestCustomMenuHandlerCalledIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            bool called = false;
            EventHandler customerHandler = delegate
            {
                called = true;
            };
            menuItem.CustomHandler += customerHandler;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
            //---------------Tear Down -------------------------                
        }

        [Test]
        public void TestControlManagerCreatorCalledIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            bool called = false;
            IControlFactory controlFactoryPassedToCreator = null;
            ControlManagerCreator formControlCreatorDelegate = delegate(IControlFactory controlFactory)
            {
                called = true;
                controlFactoryPassedToCreator = controlFactory;
                return new ControlManagerStub(controlFactory);
            };
            menuItem.ControlManagerCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
            Assert.AreSame(habaneroMenu.ControlFactory, controlFactoryPassedToCreator);
            //---------------Tear Down -------------------------         
        }


        [Test]
        public void TestCustomMenuHandlerTakesPrecedence()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            bool customHandlerCalled = false;
            EventHandler customerHandler = delegate
            {
                customHandlerCalled = true;
            };
            bool formControlHandlerCalled = false;
            FormControlCreator formControlCreatorDelegate = delegate
            {
                formControlHandlerCalled = true;
                return CreateFormControlStub();
            };
            menuItem.CustomHandler += customerHandler;
            menuItem.FormControlCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsFalse(formControlHandlerCalled);
            Assert.IsTrue(customHandlerCalled);
            //---------------Tear Down -------------------------                
        }

        [Test]
        public void TestHandlesError_FromCustomHandler()
        {
            //---------------Set up test pack-------------------
            MockExceptionNotifier exceptionNotifier = new MockExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            Exception exception = new Exception();
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            menuItem.CustomHandler += delegate(object sender, EventArgs e)
            {
                throw exception;
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //-------------Assert Preconditions -------------
            Assert.IsNull(exceptionNotifier.Exception);
            Assert.IsNull(exceptionNotifier.FurtherMessage);
            Assert.IsNull(exceptionNotifier.Title);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(exception, exceptionNotifier.Exception);
            Assert.IsNull(null, exceptionNotifier.FurtherMessage);
            Assert.IsNull(null, exceptionNotifier.Title);
        }


        [Test]
        public void TestHandlesError_FromFormControlCreator()
        {
            //---------------Set up test pack-------------------
            MockExceptionNotifier exceptionNotifier = new MockExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            Exception exception = new Exception();
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            menuItem.FormControlCreator += delegate
            {
                throw exception;
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            menu.DockInForm(habaneroMenu.Form);

            //-------------Assert Preconditions -------------
            Assert.IsNull(exceptionNotifier.Exception);
            Assert.IsNull(exceptionNotifier.FurtherMessage);
            Assert.IsNull(exceptionNotifier.Title);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(exception, exceptionNotifier.Exception);
            Assert.IsNull(null, exceptionNotifier.FurtherMessage);
            Assert.IsNull(null, exceptionNotifier.Title);
        }

        [Test]
        public void TestHandlesError_FromControlManagerCreator()
        {
            //---------------Set up test pack-------------------
            MockExceptionNotifier exceptionNotifier = new MockExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            Exception exception = new Exception();
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            menuItem.ControlManagerCreator += delegate
            {
                throw exception;
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            menu.DockInForm(habaneroMenu.Form);

            //-------------Assert Preconditions -------------
            Assert.IsNull(exceptionNotifier.Exception);
            Assert.IsNull(exceptionNotifier.FurtherMessage);
            Assert.IsNull(exceptionNotifier.Title);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(exception, exceptionNotifier.Exception);
            Assert.IsNull(null, exceptionNotifier.FurtherMessage);
            Assert.IsNull(null, exceptionNotifier.Title);
        }

        [Test]
        public void TestDockMenuInForm()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IFormHabanero form = habaneroMenu.Form;
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //-------------Assert Preconditions -------------
            Assert.IsFalse(IsMenuDocked(menu, form));
            //---------------Execute Test ----------------------
            menu.DockInForm(form);
            //---------------Test Result -----------------------
            Assert.IsTrue(IsMenuDocked(menu, form));
        }

        [Test]
        public void TestClickMenuItemDocksControlInForm()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            IFormHabanero frm = habaneroMenu.Form;
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            IFormControl expectedFormControl = CreateFormControlStub();
            menuItem.FormControlCreator += delegate { return expectedFormControl; };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl, frm);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClickMenuItemTwiceOnlyLeavesSameControlDocked()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            IFormControl expectedFormControl = CreateFormControlStub();
            menuItem.FormControlCreator += delegate { return expectedFormControl; };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //-------------Assert Preconditions -------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl, habaneroMenu.Form);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl, habaneroMenu.Form);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClickSecondItemDocksNewControlInForm()
        {
            //---------------Set up test pack-------------------

            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem1 = submenu.AddMenuItem(TestUtil.CreateRandomString());
            IFormControl expectedFormControl1 = CreateFormControlStub();
            menuItem1.FormControlCreator += delegate { return expectedFormControl1; };
            HabaneroMenu.Item menuItem2 = submenu.AddMenuItem(TestUtil.CreateRandomString());
            IFormControl expectedFormControl2 = CreateFormControlStub();
            menuItem2.FormControlCreator += delegate { return expectedFormControl2; };

            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem1 = menu.MenuItems[0].MenuItems[0];
            IMenuItem formsMenuItem2 = menu.MenuItems[0].MenuItems[1];
            formsMenuItem1.PerformClick();

            //-------------Assert Preconditions -------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl1, habaneroMenu.Form);

            //---------------Execute Test ----------------------
            formsMenuItem2.PerformClick();

            //---------------Test Result -----------------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl2, habaneroMenu.Form);
        }

        public class ControlManagerStub : IControlManager
        {
            private readonly IControlFactory _controlFactory;
            private readonly IControlHabanero _control;

            public ControlManagerStub(IControlFactory controlFactory)
            {
                _controlFactory = controlFactory;
                _control = _controlFactory.CreateControl();
            }

            public IControlHabanero Control
            {
                get { return _control; }
            }
        }
    }
}
