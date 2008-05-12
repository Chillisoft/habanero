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

using System.Windows.Forms;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestGridLayoutManager
    {
        private Control ctl;
        private GridLayoutManager manager;

        [SetUp]
        public void Setup()
        {
            ctl = new Control();
            ctl.Width = 74;
            ctl.Height = 72;
            manager = new GridLayoutManager(ctl);
            manager.SetGridSize(2, 3);
            manager.GapSize = 2;
        }

        [Test]
        public void TestControl()
        {
            Assert.AreSame(ctl, manager.ManagedControl, "ManagedControl should return same object.");
        }

        [Test]
        public void TestSetGridSize()
        {
            Assert.AreEqual(2, manager.Rows.Count, "count of rows should be two after setting grid size");
            Assert.AreEqual(3, manager.Columns.Count, "count of cols should be 3 after setting grid size");
        }

        [Test]
        public void TestAddControl()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreSame(ctl1, ((Habanero.UI.ControlCollection)manager.Rows[0])[0],
                           "Control at position zero of row zero should be same as one first added");
            Assert.AreSame(ctl1, ((Habanero.UI.ControlCollection)manager.Columns[0])[0],
                           "Control at position zero of column zero should be same as one first added");
        }

        [Test]
        public void TestAddControls()
        {
            manager.AddControl(new Control());
            manager.AddControl(new Control());
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Control ctl2 = new Control();
            manager.AddControl(ctl2);
            Assert.AreSame(ctl1, ((Habanero.UI.ControlCollection)manager.Rows[0])[2],
                           "Control at position 2 of row 0 should be third control added.");
            Assert.AreSame(ctl2, ((Habanero.UI.ControlCollection)manager.Rows[1])[0],
                           "Control at pos 0 of row 1 should be fourth control added.");
            Assert.AreSame(ctl1, ((Habanero.UI.ControlCollection)manager.Columns[2])[0],
                           "Control at pos 0 of col 2 should be third control added.");
            Assert.AreSame(ctl2, ((Habanero.UI.ControlCollection)manager.Columns[0])[1],
                           "Control as pos 1 of col 0 should be fourth control added.");
        }

        [Test]
        public void TestOneControl()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(5, ctl1.Left, "Left of control should be 5 due to default border.");
            Assert.AreEqual(5, ctl1.Top, "Top of control should be 5 due to default border");
            Assert.AreEqual(20, ctl1.Width, "Width of control should be 20 (parent control width - (2*border) - (2*gap)");
            Assert.AreEqual(30, ctl1.Height,
                            "Height of control should be 30 (parent control height - (2*border) - (1*gap)");
        }

        [Test]
        public void TestTwoControlPos()
        {
            manager.AddControl(new Control());
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(27, ctl1.Left, "Left of control should be 27 : border + 1 control width + gap.");
        }

        [Test]
        public void TestTwoControlPosDifferentWidth()
        {
            manager.ManagedControl.Width = 104;
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(30, ctl1.Width,
                            "Width of control should be 30 (parent control width - (2*border) - (2*gap) / 3");
        }

        [Test]
        public void TestControlPosDifferentHeight()
        {
            manager.ManagedControl.Height = 42;
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(15, ctl1.Height,
                            "Height of control should be 15 (parent control height - (2*border) - (gap)) / 2");
        }

        [Test]
        public void TestDifferentGridSize()
        {
            manager.SetGridSize(3, 2);
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(19, ctl1.Height, "Height of control should depend on number of rows in grid.");
            Assert.AreEqual(31, ctl1.Width, "Width of control should depend on number of cols in grid.");
        }

        [Test]
        public void TestSecondRowPos()
        {
            manager.AddControl(new Control());
            manager.AddControl(new Control());
            manager.AddControl(new Control());
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(37, ctl1.Top,
                            "Control should be in second row.  Top should be 37 : border + 1 control height + gap.");
            Assert.AreEqual(5, ctl1.Left, "Control should be in second row.  Left should be borderSize.");
        }

        [Test]
        public void TestFixedColumnWithSize()
        {
            manager.FixColumn(1, 30);
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Control ctl2 = new Control();
            manager.AddControl(ctl2);
            Assert.AreEqual(30, ctl2.Width, "Column is fixed at 30.");
            Assert.AreEqual(15, ctl1.Width, "Fixed column should change the size of the other columns.");
        }

        [Test]
        public void TestFixedRowWithSize()
        {
            manager.FixRow(1, 20);
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            manager.AddControl(new Control());
            manager.AddControl(new Control());
            Control ctl2 = new Control();
            manager.AddControl(ctl2);
            Assert.AreEqual(20, ctl2.Height, "Row is Fixed at 20");
            Assert.AreEqual(40, ctl1.Height, "Fixed row should change the size of other rows");
        }

        [Test]
        public void TestGapSize()
        {
            manager.GapSize = 3;
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(19, ctl1.Width, "Gap size should affect size of controls.");
        }

        [Test]
        public void TestSetGapSizeAfterControls()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            manager.GapSize = 3;
            Assert.AreEqual(19, ctl1.Width, "Setting Gap size should refresh controls.");
        }

        [Test]
        public void TestBorderSize()
        {
            manager.BorderSize = 8;
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            Assert.AreEqual(18, ctl1.Width, "Border size should affect size of controls.");
        }

        [Test]
        public void TestSetBorderSizeAfterControls()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            manager.BorderSize = 8;
            Assert.AreEqual(18, ctl1.Width, "Setting border size should refresh controls.");
        }

        [Test]
        public void TestResizeRefreshesControls()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            ctl.Width = 104;
            Assert.AreEqual(30, ctl1.Width, "Changing size of managed control should cause refresh.");
        }

        [Test]
        public void TestFixColumnRefreshesControls()
        {
            manager.AddControl(new Control());
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            manager.FixColumn(1, 25);
            Assert.AreEqual(25, ctl1.Width, "FixColumn should cause refresh.");
        }

        [Test]
        public void TestFixRowRefreshesControls()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1);
            manager.FixRow(0, 10);
            Assert.AreEqual(10, ctl1.Height, "FixRow should cause refresh.");
        }

        [Test]
        public void TestFixColumnBasedOnContents()
        {
            Label myLabel = ControlFactory.CreateLabel("test", false);
            manager.FixColumnBasedOnContents(0);
            manager.AddControl(myLabel);
            Assert.AreEqual(myLabel.PreferredWidth, myLabel.Width,
                            "Width of column should be preferred width of largest control");
            Label myLongLabel = ControlFactory.CreateLabel("This is a long label", false);
            manager.AddControl(null);
            manager.AddControl(null);
            manager.AddControl(myLongLabel);
            Assert.AreEqual(myLongLabel.PreferredWidth, myLongLabel.Width,
                            "Width of column should be preferred width (or width) of largest control");
            Assert.AreEqual(myLongLabel.PreferredWidth, myLabel.Width,
                            "Width of column should be preferred width (or width) of largest control");
        }

        [Test]
        public void TestFixRowsBasedOnContents()
        {
            Control ctl1 = new Control();
            ctl1.Height = 10;
            Control ctl2 = new Control();
            ctl2.Height = 15;
            manager.FixAllRowsBasedOnContents();
            manager.AddControl(ctl1);
            manager.AddControl(null);
            manager.AddControl(null);
            manager.AddControl(null);
            manager.AddControl(ctl2);
            Assert.AreEqual(10, ctl1.Height, "Height should remain the same if we FixRowsBasedOnContents");
            Assert.AreEqual(15, ctl2.Height, "Height should remain the same if we FixRowsBasedOnContents");
        }

        [Test]
        public void TestAddNullControl()
        {
            manager.AddControl(null);
        }

        [Test]
        public void TestColumnSpan()
        {
            Control ctl1 = new Control();
            ctl1.Height = 30;
            Control ctl2 = new Control();
            manager.AddControl(ctl1, 1, 2);
            manager.AddControl(ctl2);
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(42, ctl1.Width);
            Assert.AreEqual(49, ctl2.Left);
            Assert.AreEqual(20, ctl2.Width);
        }

        [Test]
        public void TestColumnSpan2()
        {
            Control ctl1 = new Control();
            ctl1.Height = 30;
            Control ctl2 = new Control();
            manager.AddControl(ctl1);
            manager.AddControl(ctl2, 1, 2);
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(20, ctl1.Width);
            Assert.AreEqual(27, ctl2.Left);
            Assert.AreEqual(42, ctl2.Width);
        }

        [Test]
        public void TestColumnSpan3()
        {
            Control ctl1 = new Control();
            ctl1.Height = 30;
            manager.AddControl(ctl1, 1, 3);
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(64, ctl1.Width);
        }

        [Test]
        public void TestRowSpan()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1, 2, 1);
            Assert.AreEqual(5, ctl1.Top);
            Assert.AreEqual(62, ctl1.Height);
        }

        [Test]
        public void TestRowAndColumnSpan()
        {
            Control ctl1 = new Control();
            manager.AddControl(ctl1, 2, 3);
            Assert.AreEqual(5, ctl1.Top);
            Assert.AreEqual(62, ctl1.Height);
            Assert.AreEqual(5, ctl1.Left);
            Assert.AreEqual(64, ctl1.Width);
        }
    }
}