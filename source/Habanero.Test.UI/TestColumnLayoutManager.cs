//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Windows.Forms;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Generic
{
    /// <summary>
    /// Summary description for TestColumnLayoutManager.
    /// </summary>
    [TestFixture]
    public class TestColumnLayoutManager
    {
        private ColumnLayoutManager manager;
        private Control managedControl;

        [SetUp]
        public void SetupLayoutManager()
        {
            managedControl = new Control();
            managedControl.Width = 100;
            managedControl.Height = 100;
            manager = new ColumnLayoutManager(managedControl);
            //manager.GapSize = 5;
        }

        [Test]
        public void TestControl()
        {
            Assert.AreSame(managedControl, manager.ManagedControl);
        }

        [Test]
        public void TestAddControl()
        {
            manager.AddControl(CreateControl(10, 10));
            Assert.AreEqual(1, managedControl.Controls.Count);
        }

        [Test]
        public void TestAddControlWithOneColumn()
        {
            Control ctlToAdd = CreateControl(10, 10);
            manager.AddControl(ctlToAdd);
            Assert.AreEqual(5, ctlToAdd.Left, "Added control's Left prop should be 5 because of the default border.");
            Assert.AreEqual(5, ctlToAdd.Top, "Added control's Top prop should be 5 because of the default border.");
            Assert.AreEqual(90, ctlToAdd.Width, "Added control's Width prop should be 10.");
            Assert.AreEqual(10, ctlToAdd.Height, "Added control's Height prop should be 10.");
        }

        [Test]
        public void TestAddControlWithTwoColumn()
        {
            Control ctl1 = CreateControl(10, 10);
            Control ctl2 = CreateControl(10, 10);
            manager.SetColumns(2);
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);

            Assert.AreEqual(5, ctl1.Left, "ctl1's Left prop should be 5 because of the default border.");
            Assert.AreEqual(5, ctl1.Top, "ctl1's Top prop should be 5 because of the default border.");
            Assert.AreEqual(44, ctl1.Width, "ctl1's Width prop should be 44 because of the border and gap.");
            Assert.AreEqual(51, ctl2.Left, "ctl2's Left prop should be 51");
            Assert.AreEqual(5, ctl2.Top, "ctl2's Top prop should be 5 because of the default border.");
            Assert.AreEqual(44, ctl2.Width, "ctl2's Width prop should be 44");
        }

        [Test]
        public void TestAddControlWithMultipleRows()
        {
            manager.SetColumns(2);
            manager.AddControl(CreateControl(10, 10));
            manager.AddControl(CreateControl(10, 20));
            Control ctl1 = CreateControl(10, 10);
            manager.AddControl(ctl1);

            Assert.AreEqual(27, ctl1.Top, "ctl1 should be on the second row since it is the third control added.");
            Assert.AreEqual(5, ctl1.Left, "ctl1 should be on the second row since it is the third control added.");
        }


        public static Control CreateControl(int width, int height)
        {
            Control ctl = new Control();
            ctl.Width = width;
            ctl.Height = height;
            return ctl;
        }
    }
}