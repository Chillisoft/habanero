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
    public abstract class TestHelpAboutBox
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestHelpAboutBoxWin : TestHelpAboutBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            [Test, Ignore("Currently working on this")]
            public void Test_Constructor()
            {
                //---------------Set up test pack-------------------
                IFormChilli helpAbout = GetControlFactory().CreateForm();
                helpAbout.Show();
                //---------------Execute Test ----------------------

                HelpAboutBoxManager aboutBoxManager =
                    new HelpAboutBoxManager(GetControlFactory(), helpAbout, "Test", "Chillisoft", "Habanero", "1.00");

                //---------------Test Result -----------------------
                helpAbout.Show();
            }
        }
    }

    [TestFixture]
    public class TestHelpAboutBoxWinGiz : TestHelpAboutBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.WebGUI.ControlFactoryGizmox();
        }
    }
}