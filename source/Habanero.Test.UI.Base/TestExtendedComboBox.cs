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

using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestExtendedComboBox
    {
        private static IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_Layout()
        {
            //--------------- Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            //--------------- Test Result -----------------------
            Assert.AreEqual(2, extendedComboBox.Controls.Count);
            Control control1 = extendedComboBox.Controls[0];
            Control control2 = extendedComboBox.Controls[1];
            Assert.IsInstanceOf(typeof(IComboBox), control1);
            Assert.IsInstanceOf(typeof(IButton), control2);
            Assert.AreEqual("...", control2.Text);
            Assert.AreEqual(0, control1.Left);
            Assert.LessOrEqual(control1.Width, control2.Left);
            Assert.GreaterOrEqual(extendedComboBox.Width, control2.Left + control2.Width);
        }

        [Test]
        public void Test_GetCombo()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IComboBox comboBox = extendedComboBox.ComboBox;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(comboBox);
        }

        [Test]
        public void Test_GetButton()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IButton button = extendedComboBox.Button;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(button);
        }
    }
}