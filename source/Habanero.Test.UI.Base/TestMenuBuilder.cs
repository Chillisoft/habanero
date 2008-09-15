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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
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

            [Test]
            public void TestClickMenuItemSetsControlInMdiForm()
            {
                //---------------Set up test pack-------------------
                IFormHabanero frm = GetControlFactory().CreateForm();
                frm.IsMdiContainer = true;
                HabaneroMenu habaneroMenu = new HabaneroMenu("Main", frm, GetControlFactory());
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
                IFormControl expectedFormControl = new FormControlStubWin();

                menuItem.FormControlCreator += delegate { return expectedFormControl; };
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                //---------------Execute Test ----------------------
                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
                formsMenuItem.PerformClick();
            
                //---------------Test Result -----------------------
                System.Windows.Forms.Form winForm = (Form) frm;
                Assert.AreEqual(1, winForm.MdiChildren.Length);
                System.Windows.Forms.Form childForm = winForm.MdiChildren[0];
                Assert.AreEqual(1, childForm.Controls.Count);
                Assert.AreSame(expectedFormControl, childForm.Controls[0]);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestClickMenuItemTwiceOnlyCreatesOneForm()
            {
                //---------------Set up test pack-------------------
                IFormHabanero frm = GetControlFactory().CreateForm();
                frm.IsMdiContainer = true;
                HabaneroMenu habaneroMenu = new HabaneroMenu("Main", frm, GetControlFactory());
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
                IFormControl expectedFormControl = new FormControlStubWin();

                menuItem.FormControlCreator += delegate { return expectedFormControl; };
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                //---------------Execute Test ----------------------
                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
                formsMenuItem.PerformClick();
                formsMenuItem.PerformClick();

                //---------------Test Result -----------------------
                System.Windows.Forms.Form winForm = (Form)frm;
                Assert.AreEqual(1, winForm.MdiChildren.Length);
                System.Windows.Forms.Form childForm = winForm.MdiChildren[0];
                Assert.AreEqual(1, childForm.Controls.Count);
                Assert.AreSame(expectedFormControl, childForm.Controls[0]);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestCloseFormAndClickCreatesNewForm()
            {
                //---------------Set up test pack-------------------
                IFormHabanero frm = GetControlFactory().CreateForm();
                frm.IsMdiContainer = true;
                HabaneroMenu habaneroMenu = new HabaneroMenu("Main", frm, GetControlFactory());
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
    

                menuItem.FormControlCreator += delegate { return new FormControlStubWin(); ; };
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
                formsMenuItem.PerformClick();
                System.Windows.Forms.Form winForm = (Form)frm;
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

            [Test]
            public void TestClickMenuItemDocksControlInForm()
            {
                //---------------Set up test pack-------------------
                IControlHabanero contentControl;
                IFormHabanero frm = CreateMainForm(out contentControl);
                HabaneroMenu habaneroMenu = new HabaneroMenu("Main", frm, GetControlFactory());
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
                IFormControl expectedFormControl = new FormControlStubVWG();

                menuItem.FormControlCreator += delegate { return expectedFormControl; };
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                //---------------Execute Test ----------------------
                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
                formsMenuItem.PerformClick();

                //---------------Test Result -----------------------
                Assert.AreEqual(1, contentControl.Controls.Count);
                Assert.AreSame(expectedFormControl, contentControl.Controls[0]);
                Assert.AreEqual(Habanero.UI.Base.DockStyle.Fill, ((IControlHabanero)expectedFormControl).Dock);
                //---------------Tear Down -------------------------          
            }

            private IFormHabanero CreateMainForm(out IControlHabanero contentControl)
            {
                IFormHabanero frm = GetControlFactory().CreateForm();
                contentControl = GetControlFactory().CreatePanel();
                frm.Controls.Add(contentControl);
                return frm;
            }

            [Test]
            public void TestClickMenuItemTwiceLeavesSameControlInForm()
            {
                //---------------Set up test pack-------------------
                IControlHabanero contentControl;
                IFormHabanero frm = CreateMainForm(out contentControl);
                HabaneroMenu habaneroMenu = new HabaneroMenu("Main", frm, GetControlFactory());
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
 

                menuItem.FormControlCreator += delegate { return new FormControlStubVWG(); };
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                //---------------Execute Test ----------------------
                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
                formsMenuItem.PerformClick();
                IControlHabanero expectedFormControl = contentControl.Controls[0];
                formsMenuItem.PerformClick();
                //---------------Test Result -----------------------
                Assert.AreSame(expectedFormControl, contentControl.Controls[0]);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestClickSecondItemDocksNewControlInForm()
            {
                //---------------Set up test pack-------------------
                IControlHabanero contentControl;
                IFormHabanero frm = CreateMainForm(out contentControl);
                HabaneroMenu habaneroMenu = new HabaneroMenu("Main", frm, GetControlFactory());
                HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
                HabaneroMenu.Item menuItem1 = submenu.AddMenuItem(TestUtil.CreateRandomString());
                IFormControl expectedFormControl1 = new FormControlStubVWG();
                menuItem1.FormControlCreator += delegate { return expectedFormControl1; };
                HabaneroMenu.Item menuItem2 = submenu.AddMenuItem(TestUtil.CreateRandomString());
                IFormControl expectedFormControl2 = new FormControlStubVWG();
                menuItem2.FormControlCreator += delegate { return expectedFormControl2; };
 
                IMenuBuilder menuBuilder = CreateMenuBuilder();
                //---------------Execute Test ----------------------
                IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
                IMenuItem formsMenuItem1 = menu.MenuItems[0].MenuItems[0];
                formsMenuItem1.PerformClick();
                IMenuItem formsMenuItem2 = menu.MenuItems[0].MenuItems[1];
                formsMenuItem2.PerformClick();

                //---------------Test Result -----------------------
                Assert.AreEqual(1, contentControl.Controls.Count);
                Assert.AreSame(expectedFormControl2, contentControl.Controls[0]);
                //---------------Tear Down -------------------------          
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
        public void TestFormControlCreatorCalledOnClickIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubmenu(TestUtil.CreateRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.CreateRandomString());
            bool called = false;
            FormControlCreator formControlCreatorDelegate = delegate
            {
                called = true;
                return new FormControlStub();
            };
            menuItem.FormControlCreator += formControlCreatorDelegate;
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
                return new FormControlStub();
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

        public class FormControlStub : IFormControl
        {
            public void SetForm(IFormHabanero form)
            {
            }
        }
    }
}