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
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Generic
{
    [TestFixture]
    public class TestButtonControl
    {
        private ButtonControl buttons;

        [SetUp]
        public void SetupTest()
        {
            buttons = new ButtonControl();
        }


        [Test]
        public void TestAddButton()
        {
            buttons.Width = 200;
            Button btnTest = buttons.AddButton("Test");
            Assert.AreEqual(195, btnTest.Right, "Button should be right aligned.");
        }

        [Test]
        public void TestButtonWidthOneButton()
        {
            Button btnTest = buttons.AddButton("TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen");
            Label lbl = new Label();
            lbl.Text = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            Assert.AreEqual(lbl.PreferredWidth + 10, btnTest.Width, "Button width is incorrect.");
        }

        [Test]
        public void TestButtonWidthOneSmallButton()
        {
            Button btnTest = buttons.AddButton("A");
            Assert.AreEqual(Screen.PrimaryScreen.Bounds.Width/16, btnTest.Width,
                            "Button width is incorrect - when buttons are very small they should instead be 1 12th of screen width.");
        }


        [Test]
        public void TestButtonWidthTwoButtons()
        {
            Button btnTest1 = buttons.AddButton("Test");
            buttons.AddButton("TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen");
            Label lbl = new Label();
            lbl.Text = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            Assert.AreEqual(lbl.PreferredWidth + 10, btnTest1.Width, "Button width is incorrect.");
        }


        [Test]
        public void TestButtonControlHeight()
        {
            Button btn = buttons.AddButton("Test");
            buttons.AddButton("Text2");
            Assert.AreEqual(btn.Height + 10, buttons.Height, "Height should be button height + 10.");
        }

        [Test]
        public void TestHideButton()
        {
            Button btn = buttons.AddButton("Test");
            Assert.IsTrue(btn.Visible);
            buttons.HideButton("Test");
            Assert.IsFalse(btn.Visible);
        }

        [Test]
        public void TestSetDefaultButton()
        {
            Button btn = buttons.AddButton("Test");
            Form frm = new Form();
            frm.Controls.Add(buttons);
            buttons.SetDefaultButton("Test");
            Assert.AreSame(btn, frm.AcceptButton);
        }
    }
}