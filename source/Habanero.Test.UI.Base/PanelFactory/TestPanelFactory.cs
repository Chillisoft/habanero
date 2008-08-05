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
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestPanelFactory.
    /// </summary>
    public abstract class TestPanelFactory
    {
        protected abstract IControlFactory GetControlFactory();

        protected abstract void ApplyChangesToBusinessObject(IPanelFactoryInfo info);

        protected abstract void SetupUserInterfaceMapper();

        protected abstract void SetupClassDef();

        protected ISampleUserInterfaceMapper _sampleUserInterfaceMapper;

        [SetUp]
        public void SetupTest()
        {
            SetupClassDef();
            SetupUserInterfaceMapper();
        }

        [TestFixture]
        public class TestPanelFactoryWin : TestPanelFactory
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }
            
            protected override void SetupClassDef()
            {
                ClassDef.ClassDefs.Clear();
                Sample.CreateClassDefWin();
            }

            protected override void ApplyChangesToBusinessObject(IPanelFactoryInfo info)
            {
                // do nothing - on windows the changes should be applied automatically when a value in a control changes
                //Todo: Remove this line and get this passing for win. This feature should be tested in the mappers. Check this!
                info.ControlMappers.ApplyChangesToBusinessObject();
            }

            protected override void SetupUserInterfaceMapper()
            {
                _sampleUserInterfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            }
        }

        [TestFixture]
        public class TestPanelFactoryGiz : TestPanelFactory
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            protected override void SetupClassDef()
            {
                ClassDef.ClassDefs.Clear();
                Sample.CreateClassDefGiz();
            }

            protected override void ApplyChangesToBusinessObject(IPanelFactoryInfo info)
            {
                info.ControlMappers.ApplyChangesToBusinessObject();
            }

            protected override void SetupUserInterfaceMapper()
            {
                _sampleUserInterfaceMapper = new Sample.SampleUserInterfaceMapperGiz();
            }
        }

        [Test]
        public void TestSetPanelFactoryTo_Bo_DoesNotHaveUIDef_Error()
        {
            //---------------Set up test pack-------------------
            MyBO.GetLoadClassDefsNoUIDef();
            MyBO bo = new MyBO();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------

            try
            {
                new PanelFactory(bo, GetControlFactory());
                Assert.Fail("Should raise an error");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("Cannot create a panel factory for 'MyBO' since the classdefs do not contain a uiDef", ex.Message);
            }
        }

        [Test]
        public void TestSetPanelFactoryTo_Bo_DoesNotHaveFormDef_Error()
        {
            //---------------Set up test pack-------------------
            MyBO.GetLoadClassDefsUIDefNoFormDef();
            MyBO bo = new MyBO();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------

            try
            {
                new PanelFactory(bo, GetControlFactory());
                Assert.Fail("Should raise an error");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("Cannot create a panel factory for 'MyBO' since the classdefs do not contain a form def",
                     ex.Message);
            }
        }

        [Test]
        public void TestSetPanelFactoryTo_Bo_setFormDefNull_Error()
        {
            //---------------Set up test pack-------------------
            MyBO.GetLoadClassDefsUIDefNoFormDef();
            MyBO bo = new MyBO();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            UIForm frm = null;
            try
            {
                new PanelFactory(bo, frm, GetControlFactory());
                Assert.Fail("Should raise an error");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null.", ex.Message);
                StringAssert.Contains("Parameter name: uiForm", ex.Message);
            }
        }

        [Test]
        public void TestCreateOnePanelPerUIFormTab()
        {
            //---------------Set up test pack-------------------
            Sample s = new Sample();
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------
            IPanelFactory factory = new PanelFactory(s, GetControlFactory());
            IList<IPanelFactoryInfo> panelList = factory.CreateOnePanelPerUIFormTab();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, panelList.Count);
        }

        [Test]
        public void TestCreateOnePanelPerUIFormTab_2Panels()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithTwoUITabs();
            MyBO myBo = new MyBO();
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------
            IPanelFactory factory = new PanelFactory(myBo, GetControlFactory());
            IList<IPanelFactoryInfo> panelList = factory.CreateOnePanelPerUIFormTab();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelList.Count);
        }

        [Test]
        public void TestOnePropertyForm()
        {
            Sample s = new Sample();
            IPanelFactory factory = new PanelFactory(s, GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box and a blank space for the error provider.");
            Assert.IsTrue(pnl.Controls[0] is ILabel);
            Assert.AreEqual("Text:", pnl.Controls[0].Text);
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
        }

        [Test]
        public void TestAddOneControlToPanel()
        {
            //---------------Set up test pack-------------------
            Sample s = new Sample();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelFactory factory = new PanelFactory(s, GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;

            //---------------Test Result -----------------------
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box and a blank space for the error provider..");
            Assert.IsTrue(pnl.Controls[0] is ILabel);
            Assert.AreEqual("Text:", pnl.Controls[0].Text);
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
            ITextBox textBox = (ITextBox) pnl.Controls[1];
            int textBoxRight = textBox.Left + textBox.Width;
            Assert.AreEqual(textBoxRight + 22, pnl.Width, "The text box must leave a gap to its right so that the error provider can show.");
        }

        [Test]
        public void TestWithMoreThanOneColumn()
        {
            //---------------Set up test pack-------------------
            Sample s = new Sample();
            //---------------Execute Test ----------------------
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapper2Cols(), GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;

            //---------------Execute Test ----------------------
            Assert.AreEqual(12, pnl.Controls.Count, "The panel should have 8 controls + 4 spaces for the error provider.");
            int labelWidth = pnl.Controls[0].Width;
            int leftPos = pnl.Controls[0].Left;
            IControlChilli label3 = pnl.Controls[6];
            Assert.AreEqual(labelWidth, label3.Width, "All labels in the column should be the same width");
                // control 4 is first one on second row
            Assert.AreEqual
                (leftPos, label3.Left, "All labels in the column should be positioned at the same x position");
            IControlChilli secondLabelInCol2 = pnl.Controls[3];
            Assert.IsTrue(secondLabelInCol2.Left > leftPos + labelWidth, "New column should be started here");
            int column1width = 100;
            int bordersize = 5;
            int gapSize = 2;
            int column2width = 150;
            int column2left = column1width + bordersize + (gapSize * 3) + 15;
            Assert.AreEqual(column2left, secondLabelInCol2.Left);
            Assert.IsTrue(pnl.Controls[3] is ITextBox);
            IControlChilli secondControl_InCol2 = pnl.Controls[4];
            Assert.AreEqual(column2left + column2width + 2, secondControl_InCol2.Left + secondControl_InCol2.Width, "Control in column 2 should be placed on column2 dimensions");
                //  column 2 width is 150
        }

        [Test]
        public void TestMapperIsConnected()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapper3Props(), GetControlFactory());
            IPanelFactoryInfo info = factory.CreatePanel();
            IPanel pnl = info.Panel;
            ITextBox tb = (ITextBox) info.ControlMappers["SampleText"].Control;
            tb.Text = "Test";
            ApplyChangesToBusinessObject(info);

            Assert.AreEqual("Test", s.SampleText);
        }

        [Test]
        public void TestAlternateConstructor()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.GetUIFormProperties(), GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box plus error provider.");
            Assert.IsTrue(pnl.Controls[0] is ILabel);
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
        }

        [Test]
        public void TestWithMoreThanOneProperty()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapper3Props(), GetControlFactory());
            IPanelFactoryInfo pnlInfo = factory.CreatePanel();
            IPanel pnl = pnlInfo.Panel;
            Assert.AreEqual(9, pnl.Controls.Count, "The panel should have 6 controls + 3 spaces for error provider.");
            Assert.AreEqual(3, pnlInfo.ControlMappers.Count, "The PanelInfo should have 3 mappers");
            Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[0]);
            int labelWidth = pnl.Controls[0].Width;
            Assert.IsInstanceOfType(typeof(ITextBox), pnl.Controls[1]);
            Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[3]);
            Assert.AreEqual(labelWidth, pnl.Controls[3].Width);
            Assert.IsInstanceOfType(typeof(IDateTimePicker), pnl.Controls[4]);
            Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[6]);
            Assert.AreEqual(labelWidth, pnl.Controls[6].Width);
            Assert.IsInstanceOfType(typeof(ITextBox), pnl.Controls[7]);
        }

        [Test]
        public void TestWithOnePrivateProperty()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapperPrivatePropOnly(),
                     GetControlFactory());
            IPanelFactoryInfo pnlInfo = factory.CreatePanel();
            IPanel pnl = pnlInfo.Panel;
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls.");
            Assert.AreEqual(1, pnlInfo.ControlMappers.Count, "The PanelInfo should have 1 mappers");
            Assert.IsInstanceOfType(typeof(ILabel), pnl.Controls[0]);
            Assert.IsInstanceOfType(typeof(ITextBox), pnl.Controls[1]);

            Assert.AreEqual('*', ((ITextBox) pnl.Controls[1]).PasswordChar);
        }

        [Test]
        public void TestToolTipWithOneDescribedProperty()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapperDescribedPropOnly(null),
                     GetControlFactory());
            IPanelFactoryInfo pnlInfo = factory.CreatePanel();
            IPanel pnl = pnlInfo.Panel;
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls.");
            Assert.AreEqual(1, pnlInfo.ControlMappers.Count, "The PanelInfo should have 1 mappers");
            Assert.IsTrue(pnl.Controls[0] is ILabel);
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
            IToolTip toolTip = pnlInfo.ToolTip;
            string toolTipText;
            //The label should have the description of the property as it's tooltip.
            toolTipText = toolTip.GetToolTip(pnl.Controls[0]);
            Assert.AreSame("This is a sample text property that has a description.", toolTipText);
            //The textbox should also have the description of the property as it's tooltip.
            toolTipText = toolTip.GetToolTip(pnl.Controls[1]);
            Assert.AreSame("This is a sample text property that has a description.", toolTipText);
        }

        [Test]
        public void TestToolTipWithOneDescribedPropertyWithSpecifiedToolTip()
        {
            Sample s = new Sample();
            string controlToolTipText = "This is my control with a tool tip.";
            IPanelFactory factory =
                new PanelFactory
                    (s,
                     _sampleUserInterfaceMapper.SampleUserInterfaceMapperDescribedPropOnly(controlToolTipText),
                     GetControlFactory());
            IPanelFactoryInfo pnlInfo = factory.CreatePanel();
            IPanel pnl = pnlInfo.Panel;
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls.");
            Assert.AreEqual(1, pnlInfo.ControlMappers.Count, "The PanelInfo should have 1 mappers");
            Assert.IsTrue(pnl.Controls[0] is ILabel);
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
            IToolTip toolTip = pnlInfo.ToolTip;
            string toolTipText;
            //The label should have the description of the property as it's tooltip.
            toolTipText = toolTip.GetToolTip(pnl.Controls[0]);
            Assert.AreSame(controlToolTipText, toolTipText);
            //The textbox should also have the description of the property as it's tooltip.
            toolTipText = toolTip.GetToolTip(pnl.Controls[1]);
            Assert.AreSame(controlToolTipText, toolTipText);
        }


        [Test]
        public void TestWithMoreThanOneTab()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapper2Tabs(), GetControlFactory());
            IPanelFactoryInfo factoryInfo = factory.CreatePanel();
            IPanel pnl = factoryInfo.Panel;
            Assert.AreEqual(1, pnl.Controls.Count, "The panel should have 1 control.");
            Assert.AreEqual(3, factoryInfo.ControlMappers.Count);
            Assert.IsTrue(pnl.Controls[0] is ITabControl, "The control should be a tabcontrol");
            ITabControl tabControl = (ITabControl) pnl.Controls[0];
            Assert.AreEqual(2, tabControl.TabPages.Count, "There should be 2 tabs");
            Assert.AreEqual("mytab1", tabControl.TabPages[0].Text);
            Assert.AreEqual("mytab2", tabControl.TabPages[1].Text);
        }

        [Test]
        public void TestPanelInfoAndPanelSizes()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapper3Props(), GetControlFactory());
            IPanelFactoryInfo pnlInfo = factory.CreatePanel();
            Assert.AreEqual(300, pnlInfo.PreferredHeight);
            Assert.AreEqual(350, pnlInfo.PreferredWidth);
            Assert.IsTrue(pnlInfo.Panel.Height < 300);
        }

        [Test]
        public void TestReadOnlyFields()
        {
            Sample s = new Sample();
            IPanelFactory factory = new PanelFactory(s, GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;
            Assert.AreEqual(3, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box.");
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
            ITextBox tb = (ITextBox) pnl.Controls[1];
            Assert.IsFalse(tb.Enabled);
        }

        [Test]
        public void TestMultiLineTextBox()
        {
            Sample s = new Sample();
            IPanelFactory factory = new PanelFactory(s, GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;
            Assert.IsTrue(pnl.Controls[1] is ITextBox);
            ITextBox tb = (ITextBox) pnl.Controls[1];
            Assert.IsTrue(tb.Multiline, "Textbox should be multiline if NumLines > 1");
            ITextBox myTb = GetControlFactory().CreateTextBox();
            Assert.AreEqual(myTb.Height * 3, tb.Height);
        }

        [Test]
        public void TestUIDefName()
        {
            //---------------Set up test pack-------------------
            Sample s = new Sample();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IPanelFactoryInfo panelFactoryInfo = new PanelFactory(s, GetControlFactory()).CreatePanel();
            //---------------Test Result -----------------------
            Assert.AreEqual("default", panelFactoryInfo.UIDefName);
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("column spanning seems a little dodge")]
        public void TestColumnSpanningTextBox()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapperColSpanning(), GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;
            ILabel lbl = (ILabel) pnl.Controls[0];
            ITextBox tb = (ITextBox) pnl.Controls[1];
            ITextBox tbInSecondColumn = (ITextBox) pnl.Controls[3];
            Assert.AreEqual(tbInSecondColumn.Left + tbInSecondColumn.Width, tb.Left + tb.Width);
        }

        [Test, Ignore("Row spanning is dodgy. Need to rewrite and refactor this code to make it work")]
        public void TestRowSpanningTextBox()
        {
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapperRowSpanning(), GetControlFactory());
            IPanel pnl = factory.CreatePanel().Panel;
            ITextBox tb = (ITextBox) pnl.Controls[1];
            Assert.IsTrue(tb.Multiline, "Textbox should be multiline if NumLines > 1");
            Assert.AreEqual(12, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box.");
            IControlChilli lastControl = pnl.Controls[10];
            Assert.IsInstanceOfType(typeof( ITextBox), lastControl);
            ITextBox tb2 = (ITextBox) lastControl;
            int textboxHeight = 20;
            int numLines = 3;
            int borderSize = 5;
            int gapSize = 2;
            Assert.AreEqual(numLines * textboxHeight + borderSize + gapSize, tb2.Top);
        }

        [Test]
        public void TestMinimumPanelSizes()
        {
            //---------------Set up test pack-------------------
            Sample s = new Sample();
            IPanelFactory factory =
                new PanelFactory
                    (s, _sampleUserInterfaceMapper.SampleUserInterfaceMapperRowSpanning(), GetControlFactory());

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IPanelFactoryInfo panelInfo = factory.CreatePanel();
            IPanel pnl = panelInfo.Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(pnl.Height, panelInfo.MinimumPanelHeight);
            Assert.AreEqual(pnl.Width, panelInfo.MinumumPanelWidth);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_ReadWriteRule_ReadOnly()
        {
            //---------------Set up test pack-------------------

            Sample s = new Sample();
            //---------------Execute Test ----------------------
            IPanelFactory panelFactory = new PanelFactory(s, _sampleUserInterfaceMapper.SampleUserInterface_ReadWriteRule(), GetControlFactory());
            IPanelFactoryInfo  panelFactoryInfo = panelFactory.CreatePanel();
            //---------------Test Result -----------------------
            IControlMapperCollection mappers = panelFactoryInfo.ControlMappers;

            
            Assert.IsFalse(mappers["SampleText"].Control.Enabled);
            Assert.IsTrue(mappers["SampleText2"].Control.Enabled);
        }


        [Test]
        public void Test_ReadWriteRule_WriteNew_StateNew()
        {
            //---------------Set up test pack-------------------

            Sample s = new Sample();
            //---------------Execute Test ----------------------
            IPanelFactory panelFactory = new PanelFactory(s, _sampleUserInterfaceMapper.SampleUserInterface_WriteNewRule(), GetControlFactory());
            IPanelFactoryInfo panelFactoryInfo = panelFactory.CreatePanel();
            //---------------Test Result -----------------------
            IControlMapperCollection mappers = panelFactoryInfo.ControlMappers;


            Assert.IsTrue(mappers["SampleText"].Control.Enabled);
            Assert.IsTrue(mappers["SampleText2"].Control.Enabled);
        }


    }
}