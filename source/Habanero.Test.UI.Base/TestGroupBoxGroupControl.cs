// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestGroupBoxGroupControl
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public virtual void Test_TestAddControl()
        {
            //---------------Set up test pack-------------------
            IGroupBoxGroupControl groupBox = GetControlFactory().CreateGroupBoxGroupControl();
            IPanel contentControl = GetControlFactory().CreatePanel();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, groupBox.Controls.Count);
            //---------------Execute Test ----------------------
            const int minHeight = 110;
            const int minimumControlWidth = 150;
            IControlHabanero control = groupBox.AddControl(contentControl, "this", minHeight, minimumControlWidth);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, control.Controls.Count);
            Assert.AreEqual(minHeight + 30, control.Height);
            Assert.AreEqual(minimumControlWidth + 30, control.Width);
            Assert.AreSame(contentControl, control.Controls[0]);
            Assert.AreEqual(minHeight - 10, contentControl.Height);
//            Assert.AreEqual(minimumControlWidth, contentControl.Width);
            Assert.AreEqual(1, groupBox.Controls.Count);
        }
    }


}
