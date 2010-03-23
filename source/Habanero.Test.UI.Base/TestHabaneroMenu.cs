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
using Habanero.Base.Exceptions;
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
            HabaneroMenu submenu = menu.AddSubMenu(menuName);
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
            HabaneroMenu submenu = new HabaneroMenu("Main").AddSubMenu("Submenu");
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
            FormControlStub formControlStub = new FormControlStub();
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(null, TestUtil.GetRandomString());
            //---------------Assert PreConditions---------------            
            Assert.IsNull(menuItem.FormControlCreator);
            //---------------Execute Test ----------------------
            menuItem.FormControlCreator += (() => formControlStub);
            IFormControl formControl = menuItem.FormControlCreator();
            //---------------Test Result -----------------------
            Assert.IsNotNull(menuItem.FormControlCreator);
            Assert.IsNotNull(formControl);
            Assert.IsInstanceOf(typeof(FormControlStub), formControl);
            Assert.AreSame(formControlStub,formControl);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestSetManagedControl_ShouldSetADefaultControlManagerCreator()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(null, TestUtil.GetRandomString());
            var control = new ControlHabaneroFake();
            //---------------Assert PreConditions---------------            
            Assert.IsNull(menuItem.ControlManagerCreator);
            //---------------Execute Test ----------------------
            menuItem.ManagedControl = control;
            Assert.IsNotNull(menuItem.ControlManagerCreator);
            var controlManager = menuItem.ControlManagerCreator(null);
            //---------------Test Result -----------------------
            Assert.AreSame(control, controlManager.Control);
        }
        [Test]
        public void TestSetManagedControl_WithNull_ShouldClearControlManagerCreator()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(null, TestUtil.GetRandomString());
            ControlHabaneroFake control = new ControlHabaneroFake();
            menuItem.ManagedControl = control;
            //---------------Assert PreConditions---------------            
            Assert.IsNotNull(menuItem.ControlManagerCreator);
            //---------------Execute Test ----------------------
            menuItem.ManagedControl = null;
            //---------------Test Result -----------------------
            Assert.IsNull(menuItem.ControlManagerCreator);
        }

        [Test]
        public void TestSetControlManagerCreator()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(null, TestUtil.GetRandomString());
            //---------------Assert PreConditions---------------            
            Assert.IsNull(menuItem.ControlManagerCreator);
            //---------------Execute Test ----------------------
            menuItem.ControlManagerCreator += delegate { return new ControlManagerStub(); };
            IControlManager controlManager = menuItem.ControlManagerCreator(null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(menuItem.ControlManagerCreator);
            Assert.IsNotNull(controlManager);
            Assert.IsInstanceOf(typeof(ControlManagerStub), controlManager);
        }

        [Test]
        public void Test_ConstructMenuControlManagerDefault_WithAControl_ShouldReturnControl()
        {
            //---------------Set up test pack-------------------
            var controlHabaneroFake = new ControlHabaneroFake();            
            //---------------Assert Precondition----------------
            Assert.IsNotNull(controlHabaneroFake);
            //---------------Execute Test ----------------------
            HabaneroMenu.MenuControlManagerDefault controlManager = new HabaneroMenu.MenuControlManagerDefault(controlHabaneroFake);
            //---------------Test Result -----------------------
            Assert.AreSame(controlHabaneroFake, controlManager.Control);
        }
        [Test]
        public void Test_ConstructMenuControlManagerDefault_WhenNullControl_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ControlHabaneroFake controlHabaneroFake = null;            
            //---------------Assert Precondition----------------
            Assert.IsNull(controlHabaneroFake);
            //---------------Execute Test ----------------------

            try
            {
                new HabaneroMenu.MenuControlManagerDefault(controlHabaneroFake);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("control", ex.ParamName);
            }
        }

        private class ControlManagerStub: IControlManager
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
        private class ControlHabaneroFake : IControlHabanero
        {
            #region ImplementedMethods

            public event EventHandler Click;
            public event EventHandler DoubleClick;
            public event EventHandler Resize;
            public event EventHandler VisibleChanged;

            public AnchorStyles Anchor
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public int Width
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public IControlCollection Controls
            {
                get { throw new NotImplementedException(); }
            }

            public bool Visible
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public int TabIndex
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public bool Focus()
            {
                throw new NotImplementedException();
            }

            public bool Focused
            {
                get { throw new NotImplementedException(); }
            }

            public int Height
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public int Top
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public int Bottom
            {
                get { throw new NotImplementedException(); }
            }

            public int Left
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public int Right
            {
                get { throw new NotImplementedException(); }
            }

            public string Text
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public bool Enabled{get; set;}

            public Color ForeColor
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public Color BackColor
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public bool TabStop
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public Size Size
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public Size ClientSize
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public void Select()
            {
                throw new NotImplementedException();
            }

            public bool HasChildren
            {
                get { throw new NotImplementedException(); }
            }

            public Size MaximumSize
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public Size MinimumSize
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public Font Font
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public void SuspendLayout()
            {
                throw new NotImplementedException();
            }

            public void ResumeLayout(bool performLayout)
            {
                throw new NotImplementedException();
            }

            public void Invalidate()
            {
                throw new NotImplementedException();
            }

            public Point Location
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public DockStyle Dock
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public event EventHandler TextChanged;

            #endregion

        }
    }



   
}
