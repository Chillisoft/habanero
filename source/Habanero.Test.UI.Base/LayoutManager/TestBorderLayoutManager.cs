//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBorderLayoutManager
    {
        protected const int _STD_MANAGEDCONTROL_WIDTH = 100;
        protected const int _STD_MANAGEDCONTROL_HEIGHT = 100;// note: gizmox doesn't do the centre (fill) pos properly in testing (it sets the height to the width). THis is why the width and height are set the same here.
        private const int _STD_CONTROL_HEIGHT = 10;
        private const int _STD_CONTROL_WIDTH = 11;
        private const int _DEFAULT_BORDER = 5;//setupIn LayoutManager
        private const int _STD_GAP = 4;
        private const int _STD_BORDER = 5;

        protected abstract IControlFactory GetControlFactory();


        [SetUp]
        public void SetupLayoutManager()
        {

        }

        [Test]
        public void TestAddControl_Null_ThrowsAppropriateError()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            try
            {
                manager.AddControl(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("control", ex.ParamName);
            }
        }
        [Test]
        public void TestAddControl_Null_WithPos_ThrowsAppropriateError()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            try
            {
                manager.AddControl(null, BorderLayoutManager.Position.Centre);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("control", ex.ParamName);
            }
        }
        [Test]
        public void TestAddControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            IControlHabanero ctl = GetControlFactory().CreateTextBox();
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(ctl);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, managedControl.Controls.Count);  
        }

        [Test]
        public void TestAddControlWithPosition()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
            managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
            IControlHabanero ctl = GetControlFactory().CreatePanel();
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(ctl, BorderLayoutManager.Position.Centre);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, managedControl.Controls.Count);  
            Assert.AreEqual(manager.ManagedControl.Width, ctl.Width, "Control's width should be the width of the control");
            Assert.AreEqual(manager.ManagedControl.Height, ctl.Height , "Control's height should be the height of the control");
        }

        [Test]
        public void TestAddTwoControls()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
            managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
            IControlHabanero ctl1 = GetControlFactory().CreatePanel();
            ctl1.Height = 20;
            IControlHabanero ctl2 = GetControlFactory().CreatePanel();
            ctl2.Height = 10;
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(ctl1, BorderLayoutManager.Position.Centre);
            manager.AddControl(ctl2, BorderLayoutManager.Position.North);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, managedControl.Controls.Count);
            Assert.AreEqual(managedControl.Height - ctl2.Height, ctl1.Height, "Control at centre should fill the rest of the space.");
            Assert.AreEqual(10, ctl2.Height, "Control at position North should retain size.");
        }

        [Test]
        public void TestAddTwoControlsInWrongOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
            managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
            IControlHabanero ctl1 = GetControlFactory().CreatePanel();
            ctl1.Height = 20;
            IControlHabanero ctl2 = GetControlFactory().CreatePanel();
            ctl2.Height = 10;
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(ctl2, BorderLayoutManager.Position.North);
            manager.AddControl(ctl1, BorderLayoutManager.Position.Centre);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, managedControl.Controls.Count);
            Assert.AreEqual(managedControl.Height - ctl2.Height, ctl1.Height, "Control at centre should fill the rest of the space.");
            Assert.AreEqual(10, ctl2.Height, "Control at position North should retain size.");
        }



        [Test]
        public void TestOrders()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
            managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            IControlHabanero ctlEast = CreateControl(30, 50);
            IControlHabanero ctlNorth = CreateControl(20, 20);
            IControlHabanero ctlSouth = CreateControl(50, 50);
            IControlHabanero ctlCentre = CreateControl(1, 1);
            IControlHabanero ctlWest = CreateControl(10, 10);
            //---------------Execute Test ----------------------
            manager.AddControl(ctlEast, BorderLayoutManager.Position.East);
            manager.AddControl(ctlNorth, BorderLayoutManager.Position.North);
            manager.AddControl(ctlSouth, BorderLayoutManager.Position.South);
            manager.AddControl(ctlCentre, BorderLayoutManager.Position.Centre);
            manager.AddControl(ctlWest, BorderLayoutManager.Position.West);
            //---------------Test Result -----------------------
            Assert.AreEqual(30, ctlEast.Width);
            Assert.AreEqual(30, ctlEast.Height);
            Assert.AreEqual(20, ctlNorth.Height);
            Assert.AreEqual(100, ctlNorth.Width);
            Assert.AreEqual(50, ctlSouth.Height);
            Assert.AreEqual(100, ctlSouth.Width);
            Assert.AreEqual(10, ctlWest.Width);
            Assert.AreEqual(30, ctlWest.Height);
            Assert.AreEqual(30, ctlCentre.Height);
            Assert.AreEqual(60, ctlCentre.Width);
        }


        protected IControlHabanero CreateControl(int width, int height)
        {
            IControlHabanero ctl = GetControlFactory().CreatePanel();
            ctl.Height = height;
            ctl.Width = width;
            return ctl;
        }

        


        //public IControlHabanero CreateManagedControl()
        //{
        //    IControlChilli managedControl = GetControlFactory().CreateControl();
        //    managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
        //    managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
        //    return managedControl;
        //}

        //public IControlChilli CreateControl(int width, int height)
        //{
        //    IControlChilli ctl = GetControlFactory().CreateControl();
        //    ctl.Width = width;
        //    ctl.Height = height;
        //    return ctl;
        //}

        //private IControlChilli CreateStandardControl()
        //{
        //    return CreateControl(_STD_CONTROL_WIDTH, _STD_CONTROL_HEIGHT);
        //}
    }
}