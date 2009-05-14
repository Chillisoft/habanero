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

using System;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestEnumerations
    {
        [Test]
        public void TestHorizontalAlignment_HabaneroSameAsWin()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Center),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Center));
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Left),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Left));
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Right),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Right));
        }

        // VWG has the HorizontalAlignment done in different order to WinForms
        [Test]
        public void TestHorizontalAlignment_HabaneroDifferentToVWG()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Center),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Left));
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Right),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Center));
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Left),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Right));
        }
    }
}