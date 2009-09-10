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
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBaseMethods
    {
        protected abstract IControlHabanero CreateControl();
        protected abstract IControlFactory GetControlFactory();
        protected abstract string GetUnderlyingDockStyleToString(IControlHabanero controlHabanero);
        protected void AssertDockStylesSame(IControlHabanero controlHabanero)
        {
            DockStyle dockStyle = controlHabanero.Dock;
            string dockStyleToString = GetUnderlyingDockStyleToString(controlHabanero);
            Assert.AreEqual(dockStyle.ToString(), dockStyleToString);
        }

        public abstract class TestBaseMethodsWin : TestBaseMethods
        {
            [STAThread]
            protected override IControlFactory GetControlFactory()
            {
                ControlFactoryWin factory = new ControlFactoryWin();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }

            protected override string GetUnderlyingDockStyleToString(IControlHabanero controlHabanero)
            {
                System.Windows.Forms.Control control = (System.Windows.Forms.Control)controlHabanero;
                return control.Dock.ToString();
            }
        }

        public abstract class TestBaseMethodsVWG : TestBaseMethods
        {
            protected override IControlFactory GetControlFactory()
            {
                ControlFactoryVWG factory = new ControlFactoryVWG();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }

            protected override string GetUnderlyingDockStyleToString(IControlHabanero controlHabanero)
            {
                Gizmox.WebGUI.Forms.Control control = (Gizmox.WebGUI.Forms.Control)controlHabanero;
                return control.Dock.ToString();
            }
        }
  

        [Test]
        public virtual void TestConversion_DockStyle_None()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = CreateControl();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.Dock = DockStyle.None;
            //---------------Test Result -----------------------
            Assert.AreEqual(DockStyle.None, control.Dock);
            AssertDockStylesSame(control);
        }

        [Test]
        public virtual void TestConversion_DockStyle_Bottom()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = CreateControl();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.Dock = DockStyle.Bottom;
            //---------------Test Result -----------------------
            Assert.AreEqual(DockStyle.Bottom, control.Dock);
            AssertDockStylesSame(control);
        }

        [Test]
        public virtual void TestConversion_DockStyle_Top()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = CreateControl();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.Dock = DockStyle.Top;
            //---------------Test Result -----------------------
            Assert.AreEqual(DockStyle.Top, control.Dock);
            AssertDockStylesSame(control);
        }

        [Test]
        public virtual void TestConversion_DockStyle_Left()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = CreateControl();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.Dock = DockStyle.Left;
            //---------------Test Result -----------------------
            Assert.AreEqual(DockStyle.Left, control.Dock);
            AssertDockStylesSame(control);
        }

        [Test]
        public virtual void TestConversion_DockStyle_Right()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = CreateControl();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.Dock = DockStyle.Right;
            //---------------Test Result -----------------------
            Assert.AreEqual(DockStyle.Right, control.Dock);
            AssertDockStylesSame(control);
        }

        [Test]
        public virtual void TestConversion_DockStyle_Fill()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = CreateControl();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.Dock = DockStyle.Fill;
            //---------------Test Result -----------------------
            Assert.AreEqual(DockStyle.Fill, control.Dock);
            AssertDockStylesSame(control);
        }
    }
}
