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

using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestHabaneroMenu
    {
        [Test]
        public void TestCreateMenu()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            HabaneroMenu menu = new HabaneroMenu("Main");

            //---------------Test Result -----------------------
            Assert.AreEqual(0, menu.Submenus.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestAddSubMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu menu = new HabaneroMenu("Main");
            string menuName = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            HabaneroMenu submenu = menu.AddSubmenu(menuName);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.Submenus.Count);
            Assert.AreSame(submenu, menu.Submenus[0]);
            Assert.AreEqual(menuName, submenu.Name);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddMenuItem()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu submenu = new HabaneroMenu("Main").AddSubmenu("Submenu");
            string menuItemName = TestUtil.GetRandomString();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(menuItemName);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, submenu.MenuItems.Count);
            Assert.AreSame(menuItem, submenu.MenuItems[0]);
            Assert.AreEqual(menuItemName, menuItem.Name);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetFormControlCreator()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(TestUtil.GetRandomString());
            IFormControl formControl = null;
            //---------------Assert PreConditions---------------            
            Assert.IsNull(menuItem.FormControlCreator);
            //---------------Execute Test ----------------------
            menuItem.FormControlCreator += delegate { return new FormControlStub(); };
            formControl = menuItem.FormControlCreator();
            //---------------Test Result -----------------------
            Assert.IsNotNull(menuItem.FormControlCreator);
            Assert.IsNotNull(formControl);
            Assert.IsInstanceOfType(typeof(FormControlStub), formControl);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetControlManagerCreator()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(TestUtil.GetRandomString());
            IControlManager controlManager = null;
            //---------------Assert PreConditions---------------            
            Assert.IsNull(menuItem.ControlManagerCreator);
            //---------------Execute Test ----------------------
            menuItem.ControlManagerCreator += delegate { return new ControlManagerStub(); };
            controlManager = menuItem.ControlManagerCreator(null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(menuItem.ControlManagerCreator);
            Assert.IsNotNull(controlManager);
            Assert.IsInstanceOfType(typeof(ControlManagerStub), controlManager);
            //---------------Tear Down -------------------------          
        }

        public class ControlManagerStub: IControlManager
        {
            public IControlHabanero Control
            {
                get { return null;}
            }
        }


        private class FormControlStub : IFormControl
        {
            public void SetForm(IFormHabanero form)
            {
                
            }
        }
    }



   
}
