// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the classes that implement IMainMenuHabanero.
    /// </summary>
    [TestFixture]
    public class TestMainMenuVWG
    {
        protected virtual IMainMenuHabanero CreateControl()
        {
            return GetControlFactory().CreateMainMenu();
        }

        private IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return GetControlFactory().CreateMainMenu(menu);
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_ConstructMainMenu()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mainMenu = CreateControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(mainMenu);
            Assert.IsNotNull(mainMenu.MenuItems);
        }
        [Test]
        public void Test_ConstructMainMenu_WithHabaneroMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu menu = new HabaneroMenu(TestUtil.GetRandomString());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mainMenu = CreateControl(menu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mainMenu);
            Assert.IsNotNull(mainMenu.MenuItems);
            Assert.AreEqual(menu.Name, mainMenu.Name);
        }
        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            IMainMenuHabanero mainMenu = CreateControl();
            IMenuItemCollection expectedMenuItems = mainMenu.MenuItems;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(mainMenu);
            Assert.IsNotNull(expectedMenuItems);
            //---------------Execute Test ----------------------
            IMenuItemCollection secondCallToMenuItems = mainMenu.MenuItems;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedMenuItems, secondCallToMenuItems);
        }
        [Test]
        public void Test_ConstructMainMenu_WithHabaneroMenuNull_ShouldNotSetName()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mainMenu = CreateControl(null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mainMenu);
            TestUtil.AssertStringEmpty(mainMenu.Name, "mainMenu.Name");
            Assert.IsNotNull(mainMenu.MenuItems);
        }
        [Test]
        public void Test_DockInForm()
        {
            //---------------Set up test pack-------------------
            IFormHabanero formHabanero = GetControlFactory().CreateForm();
            IMainMenuHabanero mainMenu = CreateControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, formHabanero.Controls.Count);
            //---------------Execute Test ----------------------
            mainMenu.DockInForm(formHabanero);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, formHabanero.Controls.Count);
        }
    }
    /// <summary>
    /// This test class tests the classes that implement IMainMenuHabanero.
    /// </summary>
    [TestFixture]
    public class TestMainMenuWin : TestMainMenuVWG
    {

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}