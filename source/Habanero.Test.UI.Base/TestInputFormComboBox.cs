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
using System.Collections.Generic;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestInputFormComboBox
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestInputFormComboBoxVWG : TestInputFormComboBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
        }


        [TestFixture]
        public class TestInputFormComboBoxWin : TestInputFormComboBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [Test]
        public void TestSimpleConstructor()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            List<object> choices = new List<object>();
            choices.Add("testItem1");
            choices.Add("testItem2");
            choices.Add("testItem3");

            //---------------Execute Test ----------------------
            InputFormComboBox inputFormComboBox = new InputFormComboBox(GetControlFactory(), message, choices);

            //---------------Test Result -----------------------
            Assert.AreEqual(message, inputFormComboBox.Message);
            Assert.AreEqual(choices.Count,inputFormComboBox.ComboBox.Items.Count);
            Assert.AreEqual(choices[0], inputFormComboBox.ComboBox.Items[0]);
            Assert.AreEqual(choices[1], inputFormComboBox.ComboBox.Items[1]);
            Assert.AreEqual(choices[2], inputFormComboBox.ComboBox.Items[2]);
        }

        [Test]
        public void TestLayout()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            List<object> choices = new List<object>();
            choices.Add("testItem1");
            choices.Add("testItem2");
            choices.Add("testItem3");

            InputFormComboBox inputFormComboBox = new InputFormComboBox(GetControlFactory(), message, choices);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormComboBox.createControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(IComboBox), panel.Controls[1]);
            Assert.Greater(panel.Controls[0].Top, panel.Top);
            Assert.IsFalse(panel.Controls[0].Font.Bold);
            Assert.AreEqual(panel.Width, panel.Controls[1].Width + 30);
            int width = GetControlFactory().CreateLabel(message, true).PreferredWidth + 20;
            Assert.AreEqual(panel.Width, width);
        }

        [Test]
        public void TestSelectedItem()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            List<object> choices = new List<object>();
            choices.Add("testItem1");
            object testitem2 = "testItem2";
            choices.Add(testitem2);
            choices.Add("testItem3");
            InputFormComboBox inputFormComboBox = new InputFormComboBox(GetControlFactory(), message, choices);
            //---------------Assert pre conditions--------------
            Assert.AreEqual(null, inputFormComboBox.SelectedItem);
            //---------------Execute Test ----------------------
            inputFormComboBox.SelectedItem = testitem2;
            //---------------Test Result -----------------------
            Assert.AreSame(testitem2, inputFormComboBox.SelectedItem);
            //---------------Tear Down -------------------------
        }
    }
}