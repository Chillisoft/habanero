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

using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestLabel
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestLabelWin : TestLabel
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestLabelGiz : TestLabel
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            [Test]
            public void TestPreferredSize()
            {
                //---------------Set up test pack-------------------
                ILabel myLabel = GetControlFactory().CreateLabel();
                string labelText = "sometext";
                myLabel.Text = labelText;

                //---------------Execute Test ----------------------
                int preferredWidth = myLabel.PreferredWidth;
                //---------------Test Result -----------------------

                Assert.AreEqual(labelText.Length * 6, preferredWidth);
                //---------------Tear Down -------------------------          
            }
        }

        [Test]
        public void TestCreateLabel()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            ILabel myLabel = GetControlFactory().CreateLabel();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myLabel);

            //---------------Tear Down -------------------------   
        }

   




    }
}
