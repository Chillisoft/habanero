using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
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
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
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
                return new ControlFactoryVWG();
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
