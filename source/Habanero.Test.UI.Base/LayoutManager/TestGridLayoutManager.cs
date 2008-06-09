//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestGridLayoutManager
    {
        private IControlChilli _ctl;
        private GridLayoutManager _manager;

        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestGridLayoutManagerWin : TestGridLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestGridLayoutManagerGiz : TestGridLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [SetUp]
        public void Setup()
        {
            SetupControlAndGridLayout();
        }

        private void SetupControlAndGridLayout()
        {
            _ctl = GetControlFactory().CreateControl();
            _ctl.Width = 74;
            _ctl.Height = 72;
            _manager = new GridLayoutManager(_ctl, GetControlFactory());
            _manager.SetGridSize(2, 3);
            _manager.GapSize = 2;
        }

        [Test]
        public void TestAddNullControl()
        {
            _manager.AddControl(null);
        }

        [Test]
        public void TestControl()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreSame(_ctl, _manager.ManagedControl, "ManagedControl should return same object.");
        }

        [Test]
        public void TestSetupUpGridSize()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(2, _manager.Rows.Count, "count of rows should be two after setting grid size");
            Assert.AreEqual(3, _manager.Columns.Count, "count of cols should be 3 after setting grid size");
        }

        [Test]
        public void TestAddControl()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreSame(ctl1, ((IList) _manager.Rows[0])[0],
                           "Control at position zero of row zero should be same as one first added");
            Assert.AreSame(ctl1, ((IList)_manager.Columns[0])[0],
                           "Control at position zero of column zero should be same as one first added");
        }

        [Test]
        public void TestAddMultipleControls()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            IControlChilli ctl2 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(ctl1);
            _manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreSame(ctl1, ((IList)_manager.Rows[0])[2],
                           "Control at position 2 of row 0 should be third control added.");
            Assert.AreSame(ctl2, ((IList)_manager.Rows[1])[0],
                           "Control at pos 0 of row 1 should be fourth control added.");
            Assert.AreSame(ctl1, ((IList)_manager.Columns[2])[0],
                           "Control at pos 0 of col 2 should be third control added.");
            Assert.AreSame(ctl2, ((IList)_manager.Columns[0])[1],
                           "Control as pos 1 of col 0 should be fourth control added.");
        }

        [Test]
        public void TestOneControl()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, ctl1.Left, "Left of control should be 5 due to default border.");
            Assert.AreEqual(5, ctl1.Top, "Top of control should be 5 due to default border");
            Assert.AreEqual(20, ctl1.Width, "Width of control should be 20 (parent control width - (2*border) - (2*gap)");
            Assert.AreEqual(30, ctl1.Height,
                            "Height of control should be 30 (parent control height - (2*border) - (1*gap)");
        }

        [Test]
        public void TestTwoControlPos()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(27, ctl1.Left, "Left of control should be 27 : border + 1 control width + gap.");
        }

        [Test]
        public void TestTwoControlPosDifferentWidth()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            _manager.ManagedControl.Width = 104;
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(30, ctl1.Width,
                            "Width of control should be 30 (parent control width - (2*border) - (2*gap) / 3");
        }

        [Test]
        public void TestControlPosDifferentHeight()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            _manager.ManagedControl.Height = 42;
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(15, ctl1.Height,
                            "Height of control should be 15 (parent control height - (2*border) - (gap)) / 2");
        }

        [Test]
        public void TestDifferentGridSize()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            _manager.SetGridSize(3, 2);
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(19, ctl1.Height, "Height of control should depend on number of rows in grid.");
            Assert.AreEqual(31, ctl1.Width, "Width of control should depend on number of cols in grid.");
        }

        [Test]
        public void TestSecondRowPos()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(GetControlFactory().CreateControl());
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(37, ctl1.Top,
                            "Control should be in second row.  Top should be 37 : border + 1 control height + gap.");
            Assert.AreEqual(5, ctl1.Left, "Control should be in second row.  Left should be borderSize.");
        }

        [Test]
        public void TestFixedColumnWithSize()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            IControlChilli ctl2 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.FixColumn(1, 30);
            _manager.AddControl(ctl1);
            _manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(30, ctl2.Width, "Column is fixed at 30.");
            Assert.AreEqual(15, ctl1.Width, "Fixed column should change the size of the other columns.");
        }

        [Test]
        public void TestFixedRowWithSize()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            IControlChilli ctl2 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.FixRow(1, 20);
            _manager.AddControl(ctl1);
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(20, ctl2.Height, "Row is Fixed at 20");
            Assert.AreEqual(40, ctl1.Height, "Fixed row should change the size of other rows");
        }

        [Test]
        public void TestGapSize()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.GapSize = 3;
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(19, ctl1.Width, "Gap size should affect size of controls.");
        }

        [Test]
        public void TestSetGapSizeAfterControls()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            _manager.GapSize = 3;
            //---------------Test Result -----------------------
            Assert.AreEqual(19, ctl1.Width, "Setting Gap size should refresh controls.");
        }

        [Test]
        public void TestBorderSize()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.BorderSize = 8;
            _manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(18, ctl1.Width, "Border size should affect size of controls.");
        }

        [Test]
        public void TestSetBorderSizeAfterControls()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            _manager.BorderSize = 8;
            //---------------Test Result -----------------------
            Assert.AreEqual(18, ctl1.Width, "Setting border size should refresh controls.");
        }

        [Test]
        public void TestResizeRefreshesControls()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            _ctl.Width = 104;
            //---------------Test Result -----------------------
            Assert.AreEqual(30, ctl1.Width, "Changing size of managed control should cause refresh.");
        }

        [Test]
        public void TestFixColumnRefreshesControls()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(GetControlFactory().CreateControl());
            _manager.AddControl(ctl1);
            _manager.FixColumn(1, 25);
            //---------------Test Result -----------------------
            Assert.AreEqual(25, ctl1.Width, "FixColumn should cause refresh.");
        }

        [Test]
        public void TestFixRowRefreshesControls()
        {
            //---------------Set up test pack-------------------
            SetupControlAndGridLayout();
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            _manager.AddControl(ctl1);
            _manager.FixRow(0, 10);
            //---------------Test Result -----------------------
            Assert.AreEqual(10, ctl1.Height, "FixRow should cause refresh.");
        }

        [Test]
        public void TestFixColumnBasedOnContents()
        {
            //----------------------Setup ------------------------------
            ILabel myLabel = GetControlFactory().CreateLabel("test", false);
            _manager.FixColumnBasedOnContents(0);
            ILabel myLongLabel = GetControlFactory().CreateLabel("This is a long label", false);
            //--------------------- verify setup -----------------------
            Assert.AreEqual(myLabel.PreferredWidth, myLabel.Width);
            Assert.AreEqual(myLongLabel.PreferredWidth, myLongLabel.Width);

            //--------------------- Execute Tests-----------------------
            _manager.AddControl(myLabel);
            _manager.AddControl(null);
            _manager.AddControl(null);
            _manager.AddControl(myLongLabel);
            //--------------------- Verify results-----------------------
            Assert.AreEqual(myLongLabel.PreferredWidth, myLabel.Width,
                            "Width of column should be preferred width (or width) of largest control");
        }

//
        [Test]
        public void TestFixRowsBasedOnContents_DoesNotChangeControlsHeights()
        {
            //----------------------Setup ------------------------------
            int control1Height = 10;
            IControlChilli ctl1 = CreateControl(10);
            int control2Height = 15;
            IControlChilli ctl2 = CreateControl(control2Height);
            //--------------------- verify setup -----------------------
            Assert.AreEqual(control1Height, ctl1.Height);
            Assert.AreEqual(control2Height, ctl2.Height);

            //--------------------- Execute Tests-----------------------
            _manager.FixAllRowsBasedOnContents();
            _manager.AddControl(ctl1);
            _manager.AddControl(null);
            _manager.AddControl(null);
            _manager.AddControl(null);
            _manager.AddControl(ctl2);
            //--------------------- Verify results-----------------------
            Assert.AreEqual(control1Height, ctl1.Height, "Height should remain the same if we FixRowsBasedOnContents");
            Assert.AreEqual(control2Height, ctl2.Height, "Height should remain the same if we FixRowsBasedOnContents");
        }



        [Test]
        public void TestColumnSpan()
        {
            //----------------------Setup ------------------------------
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            ctl1.Height = 30;
            IControlChilli ctl2 = GetControlFactory().CreateControl();
            //--------------------- Execute Tests-----------------------
            _manager.AddControl(ctl1, 1, 2);
            _manager.AddControl(ctl2);
            //--------------------- Verify results-----------------------
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(42, ctl1.Width);
            Assert.AreEqual(49, ctl2.Left);
            Assert.AreEqual(20, ctl2.Width);
        }

        [Test]
        public void TestColumnSpan2()
        {
            //----------------------Setup ------------------------------
            IControlChilli ctl1 = GetControlFactory().CreateControl();
            ctl1.Height = 30;
            IControlChilli ctl2 = GetControlFactory().CreateControl();
            //--------------------- Execute Tests-----------------------
            _manager.AddControl(ctl1);
            _manager.AddControl(ctl2, 1, 2);
            //--------------------- Verify results-----------------------
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(20, ctl1.Width);
            Assert.AreEqual(27, ctl2.Left);
            Assert.AreEqual(42, ctl2.Width);
        }

        [Test]
        public void TestColumnSpan3()
        {
            //----------------------Setup ------------------------------

            int control1Height = 30;
            int controlInitialWidth = 10;
            int controlInitialLeft = -5;
            IControlChilli ctl1 = CreateControl(control1Height, controlInitialWidth, controlInitialLeft);
            //--------------------- verify setup -----------------------
            AssertControlsDimensions(control1Height, controlInitialWidth, controlInitialLeft, ctl1);
            //--------------------- Execute Tests-----------------------

            _manager.AddControl(ctl1, 1, 3);
            //--------------------- Verify results-----------------------
            int borderWidth = 5;
            Assert.AreEqual(borderWidth, ctl1.Left);
            Assert.AreEqual(64, ctl1.Width);
        }

        [Test]
        public void TestRowSpan()
        {
            IControlChilli ctl1 = CreateControl(10, 11, -5);

            _manager.AddControl(ctl1, 2, 1);

            Assert.AreEqual(5, ctl1.Top);
            Assert.AreEqual(62, ctl1.Height);
        }
        private static void AssertControlsDimensions(int height, int width, int left, IControlChilli control)
        {
            Assert.AreEqual(width, control.Width, "width is not correct");
            Assert.AreEqual(left, control.Left, "left is not correct");
            Assert.AreEqual(height, control.Height, "Height is not correct");
        }

        [Test]
        public void TestRowAndColumnSpan()
        {
            IControlChilli ctl1 = CreateControl(10, 11, -5,-5);

            _manager.AddControl(ctl1, 2, 3);

            Assert.AreEqual(5, ctl1.Top);
            Assert.AreEqual(62, ctl1.Height);
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(64, ctl1.Width);
        }

        [Test]
        public void TestTooManyRows()
        {
            //---------------Set up test pack-------------------
            IControlChilli ctl1 = CreateControl(10, 11, -5, -5);
            GridLayoutManager gridLayoutManager = new GridLayoutManager(ctl1, GetControlFactory());
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridLayoutManager.SetGridSize(1, 1);
            gridLayoutManager.AddControl(GetControlFactory().CreateTextBox());
            try
            {
                gridLayoutManager.AddControl(GetControlFactory().CreateTextBox());
                Assert.Fail("err expected");
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("You cannot add a control to the grid layout manager since it exceeds the grids size of '1' row and '1' column", ex.Message);
            }
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestTooManyRows_2Rows()
        {
            //---------------Set up test pack-------------------
            IControlChilli ctl1 = CreateControl(10, 11, -5, -5);
            GridLayoutManager gridLayoutManager = new GridLayoutManager(ctl1, GetControlFactory());
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridLayoutManager.SetGridSize(2, 1);
            gridLayoutManager.AddControl(GetControlFactory().CreateTextBox());
            gridLayoutManager.AddControl(GetControlFactory().CreateTextBox());
            try
            {
                gridLayoutManager.AddControl(GetControlFactory().CreateTextBox());
                Assert.Fail("err expected");
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("You cannot add a control to the grid layout manager since it exceeds the grids size of '2' row and '1' column", ex.Message);
            }
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        private IControlChilli CreateControl(int height)
        {
            return CreateControl(height, 10);
        }

        private IControlChilli CreateControl(int height, int width)
        {
            return CreateControl(height, width, 0);
        }

        private IControlChilli CreateControl(int height, int width, int left)
        {
            return CreateControl(height, width, left, -5);
        }

        private IControlChilli CreateControl(int height, int width, int left, int top)
        {
            IControlChilli control = GetControlFactory().CreateControl();
            control.Height = height;
            control.Width = width;
            control.Left = left;
            control.Top = top;
            return control;
        }
    }
}
