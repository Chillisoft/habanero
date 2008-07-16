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

using Habanero.UI.Forms;
using Habanero.UI.Grid;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestEditableGridButtonControl.
    /// </summary>
    [TestFixture]
    public class TestEditableGridButtonControl
    {
        [Test]
        public void TestControlCreation()
        {
            Mock gridMock = new DynamicMock(typeof (IEditableGrid));
            IEditableGrid grid = (IEditableGrid) gridMock.MockInstance;
            EditableGridButtonControl buttons = new EditableGridButtonControl(grid);
            Assert.AreEqual(2, buttons.Controls.Count);
            Assert.AreEqual("Save", buttons.Controls[1].Name);
            Assert.AreEqual("Cancel", buttons.Controls[0].Name);
        }

        [Test]
        public void TestSaveButtonClick()
        {
            Mock gridMock = new DynamicMock(typeof (IEditableGrid));
            IEditableGrid grid = (IEditableGrid) gridMock.MockInstance;
            gridMock.Expect("AcceptChanges");
            EditableGridButtonControl buttons = new EditableGridButtonControl(grid);
            buttons.ClickButton("Save");
            gridMock.Verify();
        }

        [Test]
        public void TestCancelButtonClick()
        {
            Mock gridMock = new DynamicMock(typeof (IEditableGrid));
            IEditableGrid grid = (IEditableGrid) gridMock.MockInstance;
            gridMock.Expect("RejectChanges");
            EditableGridButtonControl buttons = new EditableGridButtonControl(grid);
            buttons.ClickButton("Cancel");
            gridMock.Verify();
        }
    }
}