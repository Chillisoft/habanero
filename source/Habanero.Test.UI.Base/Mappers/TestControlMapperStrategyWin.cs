//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// These are tested in their own class since the windows and gizmox behaviour are
    /// very different.
    /// </summary>
    [TestFixture]
    public class TestControlMapperStrategyWin//:TestBase
    {
        private ControlFactoryWin _factory = new Habanero.UI.Win.ControlFactoryWin();

        [SetUp]
        public  void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public  void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void Test_ControlMapperStrategy_AddBOPropHandlers()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            ControlMapperStrategyWin strategyWin = new ControlMapperStrategyWin();
            ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
            ITextBox tb = factory.CreateTextBox();
            string testprop = "TestProp";
            ControlMapperStub stubMapper = new ControlMapperStub(tb, testprop, false, factory);
            MyBO bo = new MyBO();
            IBOProp prop = bo.Props[testprop];
            string origvalue = "origValue";
            prop.Value = origvalue;
            stubMapper.BusinessObject = bo;
            strategyWin.RemoveCurrentBOPropHandlers(stubMapper, prop);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(origvalue, tb.Text);

            //---------------Execute Test ----------------------
            strategyWin.AddCurrentBOPropHandlers(stubMapper, prop);
            string newValue = "New value";
            prop.Value = newValue;

            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, tb.Text);

        }

        [Test]
        public void Test_ControlMapperStrategy_RemoveBOPropHandlers()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            ControlMapperStrategyWin strategyWin = new ControlMapperStrategyWin();
            ITextBox tb = _factory.CreateTextBox();
            string testprop = "TestProp";
            ControlMapperStub stubMapper = new ControlMapperStub(tb, testprop, false, _factory);
            MyBO bo = new MyBO();
            IBOProp prop = bo.Props[testprop];
            string origvalue = "origValue";
            prop.Value = origvalue;
            stubMapper.BusinessObject = bo;


            string newValue = "New value";

            //--------------Assert PreConditions----------------
            Assert.AreNotEqual(newValue, tb.Text);
            Assert.AreEqual(origvalue, tb.Text);

            //---------------Execute Test ----------------------
            strategyWin.RemoveCurrentBOPropHandlers(stubMapper, prop);
            prop.Value = newValue;

            //---------------Test Result -----------------------
            Assert.AreNotEqual(newValue, tb.Text, "Updating the prop should not update the textbox since the handler has been removed");
            Assert.AreEqual(origvalue, tb.Text);
        }

        [Test]
        public void Test_GetFirstControl_OneControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            parentControl.Controls.Add(childControl);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, parentControl.Controls.Count);
            //---------------Execute Test ----------------------
            Control firstControl = ControlMapperStrategyWin.GetFirstControl((Control) parentControl, (Control) childControl);
            //---------------Test Result -----------------------
            Assert.AreSame(childControl, firstControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetFirstControl_TwoControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            childControl.TabIndex = 0;
            parentControl.Controls.Add(childControl);
            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 1;
            parentControl.Controls.Add(childControl2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control firstControl = ControlMapperStrategyWin.GetFirstControl((Control)parentControl, (Control)childControl);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl, firstControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetFirstControl_TwoControl_ReverseTabOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            childControl.TabIndex = 1;
            parentControl.Controls.Add(childControl);
            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            parentControl.Controls.Add(childControl2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control firstControl = ControlMapperStrategyWin.GetFirstControl((Control)parentControl, (Control)childControl);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl2, firstControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetFirstControl_MultipleControl_MixedTabOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();

            IControlHabanero childControl1 = _factory.CreateControl();
            childControl1.TabIndex = 2;
            parentControl.Controls.Add(childControl1);

            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            childControl2.TabStop = false;
            parentControl.Controls.Add(childControl2);

            IControlHabanero childControl3 = _factory.CreateControl();
            childControl3.TabIndex = 1;
            parentControl.Controls.Add(childControl3);

            IControlHabanero childControl4 = _factory.CreateControl();
            childControl4.TabIndex = 3;
            parentControl.Controls.Add(childControl4);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(4, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control firstControl = ControlMapperStrategyWin.GetFirstControl((Control)parentControl, (Control)childControl1);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl3, firstControl);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_GetNextControl_OneControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            parentControl.Controls.Add(childControl);

//            ControlMapperStrategyWin strategyWin = new ControlMapperStrategyWin();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, parentControl.Controls.Count);
            //---------------Execute Test ----------------------
            Control nextControl = ControlMapperStrategyWin.GetNextControlInTabOrder((Control)parentControl, (Control)childControl);
            //---------------Test Result -----------------------
            Assert.AreSame(childControl, nextControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetNextControl_TwoControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            childControl.TabIndex = 0;
            parentControl.Controls.Add(childControl);
            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 1;
            parentControl.Controls.Add(childControl2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control nextControl = ControlMapperStrategyWin.GetNextControlInTabOrder((Control)parentControl, (Control)childControl);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl2, nextControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetNextControl_TwoControl_ReverseTabOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            childControl.TabIndex = 1;
            parentControl.Controls.Add(childControl);
            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            parentControl.Controls.Add(childControl2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control nextControl = ControlMapperStrategyWin.GetNextControlInTabOrder((Control)parentControl, (Control)childControl);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl2, nextControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetNextControl_MultipleControl_MixedTabOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();

            IControlHabanero childControl1 = _factory.CreateControl();
            childControl1.TabIndex = 2;
            parentControl.Controls.Add(childControl1);

            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            childControl2.TabStop = false;
            parentControl.Controls.Add(childControl2);

            IControlHabanero childControl3 = _factory.CreateControl();
            childControl3.TabIndex = 1;
            parentControl.Controls.Add(childControl3);

            IControlHabanero childControl4 = _factory.CreateControl();
            childControl4.TabIndex = 1;
            childControl2.TabStop = false;
            parentControl.Controls.Add(childControl4);

            IControlHabanero childControl5 = _factory.CreateControl();
            childControl5.TabIndex = 3;
            parentControl.Controls.Add(childControl5);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(5, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control nextControl = ControlMapperStrategyWin.GetNextControlInTabOrder((Control)parentControl, (Control)childControl1);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl5, nextControl);

            //---------------Tear Down -------------------------          
        }

        [Ignore("This test passes on the PC's, but not on the build server")]
        [Test]
        public void Test_KeyPressMovesFocusToNextControl()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            IControlHabanero parentControl = _factory.CreateControl();
            ControlMapperStrategyWin strategyWin = new ControlMapperStrategyWin();
            ITextBox textBox = _factory.CreateTextBox();
            textBox.Name = "TestTextBox";
            strategyWin.AddKeyPressEventHandler(textBox);

            parentControl.Controls.Add(textBox);

            ITextBox textBox2 = _factory.CreateTextBox();
            parentControl.Controls.Add(textBox2);
            TextBoxWin tbWin = (TextBoxWin)textBox2;
            bool gotFocus = false;
            tbWin.GotFocus += delegate { gotFocus = true; };

            Form frm = AddControlToForm(parentControl);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            frm.Show();
            TextBoxTester box = new TextBoxTester("TestTextBox");
            KeyEventArgs eveArgsEnter = new KeyEventArgs(Keys.Enter);
            box.FireEvent("KeyUp", eveArgsEnter);

            //---------------Test Result -----------------------
            Assert.IsTrue(tbWin.ContainsFocus);
             Assert.IsTrue(gotFocus);
        }

        //[Test]
        //public void TestWin_CanPressKey()
        //{
        //    //---------------Set up test pack-------------------
        //    TextBox tb = new TextBox();
        //    tb.Name = "TestTextBox";
        //    Form frm = new Form();
        //    frm.Controls.Clear();
        //    frm.Controls.Add(tb);
        //    bool pressed = false;
        //    tb.KeyPress += delegate { pressed = true; };
        //    //--------------Assert PreConditions----------------            
        //    Assert.IsFalse(pressed);

        //    //---------------Execute Test ----------------------
        //    frm.Show();
        //    TextBoxTester box = new TextBoxTester("TestTextBox");
        //    Char pressChar = (char)0x013;
        //    KeyPressEventArgs eveArgs = new KeyPressEventArgs(pressChar);
        //    box.FireEvent("KeyPress", eveArgs);
        //    //                box.FireEvent("Click");

        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(pressed);
        //}

        private Form AddControlToForm(IControlHabanero parentControl)
        {
            Form frm = new Form();
            frm.Controls.Clear();
            frm.Controls.Add((Control) parentControl);
            return frm;
        }
    }
}
