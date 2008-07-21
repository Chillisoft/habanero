using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestDockStyle
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestDockStyleWin : TestDockStyle
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
            [Test]
            public void TestSet_Fill()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlWin = new ControlWin();
                //---------------Execute Test ----------------------
                controlWin.Dock = DockStyle.Fill;
                //---------------Test Result -----------------------
                Assert.AreEqual(System.Windows.Forms.DockStyle.Fill.ToString(), controlWin.Dock.ToString());
                Assert.AreEqual(DockStyle.Fill, controlWin.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Top()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlWin = new ControlWin();
                //---------------Execute Test ----------------------
                controlWin.Dock = DockStyle.Top;
                //---------------Test Result -----------------------
                Assert.AreEqual(System.Windows.Forms.DockStyle.Top.ToString(), controlWin.Dock.ToString());
                Assert.AreEqual(DockStyle.Top, controlWin.Dock);

                //---------------Tear Down   -----------------------
            } 
            [Test]
            public void TestSet_Bottom()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlWin = new ControlWin();
                //---------------Execute Test ----------------------
                controlWin.Dock = DockStyle.Bottom;
                //---------------Test Result -----------------------
                Assert.AreEqual(System.Windows.Forms.DockStyle.Bottom.ToString(), controlWin.Dock.ToString());
                Assert.AreEqual(DockStyle.Bottom, controlWin.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Left()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlWin = new ControlWin();
                //---------------Execute Test ----------------------
                controlWin.Dock = DockStyle.Left;
                //---------------Test Result -----------------------
                Assert.AreEqual(System.Windows.Forms.DockStyle.Left.ToString(), controlWin.Dock.ToString());
                Assert.AreEqual(DockStyle.Left, controlWin.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Right()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlWin = new ControlWin();
                //---------------Execute Test ----------------------
                controlWin.Dock = DockStyle.Right;
                //---------------Test Result -----------------------
                Assert.AreEqual(System.Windows.Forms.DockStyle.Right.ToString(), controlWin.Dock.ToString());
                Assert.AreEqual(DockStyle.Right, controlWin.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_None()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlWin = new ControlWin();
                //---------------Execute Test ----------------------
                controlWin.Dock = DockStyle.None;
                //---------------Test Result -----------------------
                Assert.AreEqual(System.Windows.Forms.DockStyle.None.ToString(), controlWin.Dock.ToString());
                Assert.AreEqual(DockStyle.None, controlWin.Dock);
                //---------------Tear Down   -----------------------
            } 
        }

        [TestFixture]
        public class TestDockStyleGiz : TestDockStyle
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
            [Test]
            public void TestSet_Fill()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlGiz = new ControlGiz();
                //---------------Execute Test ----------------------
                controlGiz.Dock = DockStyle.Fill;
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DockStyle.Fill.ToString(), controlGiz.Dock.ToString());
                Assert.AreEqual(DockStyle.Fill, controlGiz.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Top()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlGiz = new ControlGiz();
                //---------------Execute Test ----------------------
                controlGiz.Dock = DockStyle.Top;
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DockStyle.Top.ToString(), controlGiz.Dock.ToString());
                Assert.AreEqual(DockStyle.Top, controlGiz.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Bottom()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlGiz = new ControlGiz();
                //---------------Execute Test ----------------------
                controlGiz.Dock = DockStyle.Bottom;
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DockStyle.Bottom.ToString(), controlGiz.Dock.ToString());
                Assert.AreEqual(DockStyle.Bottom, controlGiz.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Left()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlGiz = new ControlGiz();
                //---------------Execute Test ----------------------
                controlGiz.Dock = DockStyle.Left;
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DockStyle.Left.ToString(), controlGiz.Dock.ToString());
                Assert.AreEqual(DockStyle.Left, controlGiz.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_Right()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlGiz = new ControlGiz();
                //---------------Execute Test ----------------------
                controlGiz.Dock = DockStyle.Right;
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DockStyle.Right.ToString(), controlGiz.Dock.ToString());
                Assert.AreEqual(DockStyle.Right, controlGiz.Dock);
                //---------------Tear Down   -----------------------
            }
            [Test]
            public void TestSet_None()
            {
                //---------------Set up test pack-------------------
                IControlChilli controlGiz = new ControlGiz();
                //---------------Execute Test ----------------------
                controlGiz.Dock = DockStyle.None;
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DockStyle.None.ToString(), controlGiz.Dock.ToString());
                Assert.AreEqual(DockStyle.None, controlGiz.Dock);
                //---------------Tear Down   -----------------------
            }
        }

        

    }

}
