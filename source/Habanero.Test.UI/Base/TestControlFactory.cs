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
    /// <summary>
    /// Summary description for TestControlFactory.
    /// </summary>
    [TestFixture]
    public class TestControlFactory
    {
        public TestControlFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [Test]
        public void TestCreateLabel()
        {
            Label lbl2 = ControlFactory.CreateLabel("Test2", false);
            Assert.AreEqual("Test2", lbl2.Text, "Text of label not set properly in CreateLabel.");
            Assert.AreEqual(FlatStyle.System, lbl2.FlatStyle, "FlatStyle should be flat for all labels.");
            Assert.AreEqual(lbl2.PreferredWidth, lbl2.Width, "Width should equal preferredwidth");

            //			Label lbl = ControlFactory.CreateLabel("Test", true);
            //			Assert.AreEqual("Test", lbl.Text, "Text of label not set properly in CreateLabel.");
            //			Assert.IsTrue(lbl.Font.Bold, "Font of label should be bold.");
        }

        [Test]
        public void TestCreateButton()
        {
            Button btn = ControlFactory.CreateButton("Button1");
            Assert.AreEqual("Button1", btn.Text, "Text of button not set properly in CreateButton");
            Assert.AreEqual("Button1", btn.Name, "Name of button not set properly in CreateButton");
            Assert.AreEqual(FlatStyle.System, btn.FlatStyle, "FlatStyle should be set in CreateButton");
            Assert.AreEqual(ControlFactory.CreateLabel("Button1", false).PreferredWidth + 20, btn.Width,
                            "Width of button from CreateButton should be 20 more than the preferredwidth of a label with the same text.");
        }

        [Test]
        public void TestCreateControl()
        {
            Control ctl = ControlFactory.CreateControl(typeof (Button));
            Assert.AreSame(typeof (Button), ctl.GetType(), "Create Control is not creating controls.");
        }
    }
}