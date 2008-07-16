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

////---------------------------------------------------------------------------------
//// Copyright (C) 2007 Chillisoft Solutions
//// 
//// This file is part of the Habanero framework.
//// 
////     Habanero is a free framework: you can redistribute it and/or modify
////     it under the terms of the GNU Lesser General Public License as published by
////     the Free Software Foundation, either version 3 of the License, or
////     (at your option) any later version.
//// 
////     The Habanero framework is distributed in the hope that it will be useful,
////     but WITHOUT ANY WARRANTY; without even the implied warranty of
////     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////     GNU Lesser General Public License for more details.
//// 
////     You should have received a copy of the GNU Lesser General Public License
////     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
////---------------------------------------------------------------------------------

//using System.Windows.Forms;
//using Habanero.UI.Base;
//using NUnit.Framework;

//namespace Habanero.Test.UI.Base
//{
//    [TestFixture]
//    public class TestBorderLayoutManager
//    {
//        private Control managedControl;
//        private BorderLayoutManager manager;

//        [SetUp]
//        public void Setup()
//        {
//            managedControl = CreateControl(100, 100);
//            manager = new BorderLayoutManager(managedControl);
//        }

//        [Test]
//        public void TestAddControl()
//        {
//            Control ctl = new Control();
//            manager.AddControl(ctl, BorderLayoutManager.Position.Centre);
//            Assert.AreEqual(ctl.Width, manager.ManagedControl.Width,
//                            "Control's width should be the width of the control");
//            Assert.AreEqual(ctl.Height, manager.ManagedControl.Height,
//                            "Control's height should be the height of the control");
//        }

//        [Test]
//        public void TestAddInWrongOrder()
//        {
//            Control ctl1 = CreateControl(20, 20);
//            manager.AddControl(ctl1, BorderLayoutManager.Position.North);
//            Control ctl2 = CreateControl(10, 10);
//            manager.AddControl(ctl2, BorderLayoutManager.Position.Centre);
//            Assert.AreEqual(20, ctl1.Height, "Control at position North should retain size.");
//            Assert.AreEqual(managedControl.Height - 20, ctl2.Height,
//                            "Control at centre should fill the rest of the space.");
//        }

//        [Test]
//        public void TestOrders()
//        {
//            Control ctlEast = CreateControl(30, 50);
//            manager.AddControl(ctlEast, BorderLayoutManager.Position.East);
//            Control ctlNorth = CreateControl(20, 20);
//            manager.AddControl(ctlNorth, BorderLayoutManager.Position.North);
//            Control ctlSouth = CreateControl(50, 50);
//            manager.AddControl(ctlSouth, BorderLayoutManager.Position.South);
//            Control ctlCentre = CreateControl(1, 1);
//            manager.AddControl(ctlCentre, BorderLayoutManager.Position.Centre);
//            Control ctlWest = CreateControl(10, 10);
//            manager.AddControl(ctlWest, BorderLayoutManager.Position.West);
//            Assert.AreEqual(30, ctlEast.Width);
//            Assert.AreEqual(30, ctlEast.Height);
//            Assert.AreEqual(20, ctlNorth.Height);
//            Assert.AreEqual(100, ctlNorth.Width);
//            Assert.AreEqual(50, ctlSouth.Height);
//            Assert.AreEqual(100, ctlSouth.Width);
//            Assert.AreEqual(10, ctlWest.Width);
//            Assert.AreEqual(30, ctlWest.Height);
//            Assert.AreEqual(30, ctlCentre.Height);
//            Assert.AreEqual(60, ctlCentre.Width);
//        }

//        [Test]
//        public void TestSplitter()
//        {
//            Control ctlEast = CreateControl(20, 20);
//            manager.AddControl(ctlEast, BorderLayoutManager.Position.East, true);
//            Control ctlCentre = CreateControl(1, 1);
//            manager.AddControl(ctlCentre, BorderLayoutManager.Position.Centre);
//            Assert.AreEqual(managedControl.Controls.Count, 3, "There should be 3 controls because of the splitter.");
//            Assert.AreEqual(80, ctlEast.Left, "East positioned control doesn't change width when splitter is added.");
//            Assert.AreEqual(80 - 3, ctlCentre.Width,
//                            "Splitter is 3 wide, so centre control should be 3 less than it would be");
//        }


//        public static Control CreateControl(int width, int height)
//        {
//            Control ctl = new Control();
//            ctl.Width = width;
//            ctl.Height = height;
//            return ctl;
//        }
//    }
//}