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

            [Test]
            public void Test_Constructor()
            {
                //---------------Set up test pack-------------------
                IFormChilli helpAbout = GetControlFactory().CreateForm();
                helpAbout.Show();
                //---------------Execute Test ----------------------

                string programName = "Test";
                string producedForName = "Chillisoft";
                string producedByName = "Habanero";
                string versionNumber = "1.00";

                HelpAboutBoxManager aboutBoxManager =
                    new HelpAboutBoxManager(GetControlFactory(), helpAbout, programName, producedForName, producedByName, versionNumber);

                //---------------Test Result -----------------------
                IPanel panel = aboutBoxManager.MainPanel;
                Assert.IsNotNull(panel);
                Assert.AreEqual(8, panel.Controls.Count);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[1]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[2]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[3]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[4]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[5]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[6]);
                Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[7]);

                Assert.AreEqual("Programme Name:", panel.Controls[0].Text);
                Assert.AreEqual(programName, panel.Controls[1].Text);
                Assert.AreEqual("Produced For:", panel.Controls[2].Text);
                Assert.AreEqual(producedForName, panel.Controls[3].Text);
                Assert.AreEqual("Produced By:", panel.Controls[4].Text);
                Assert.AreEqual(producedByName, panel.Controls[5].Text);
                Assert.AreEqual("Version:", panel.Controls[6].Text);
                Assert.AreEqual(versionNumber, panel.Controls[7].Text);
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
}