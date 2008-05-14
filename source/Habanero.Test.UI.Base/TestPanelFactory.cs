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

using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestPanelFactory.
    /// </summary>
    [TestFixture]
    public abstract class TestPanelFactory
    {
        protected abstract IControlFactory GetControlFactory();

        //[TestFixture]
        //public class TestPanelFactoryWin : TestPanelFactory
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new Habanero.UI.Win.ControlFactoryWin();
        //    }
        //}

        [TestFixture]
        public class TestPanelFactoryGiz : TestPanelFactory
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }
        }
        //[Test]
        //public void TestOnePropertyForm()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreEqual(2, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box.");
        //    Assert.AreSame(typeof(ILabel), pnl.Controls[0].GetType());
        //    Assert.AreEqual("Text:", ((IControlChilli)pnl.Controls[0]).Text);
        //    Assert.AreSame(typeof(ITextBox), pnl.Controls[1].GetType());
        //}

        //[Test]
        //public void TestMapperIsConnected()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapper3Props(), GetControlFactory());
        //    IPanelFactoryInfo info = factory.CreatePanel();
        //    IPanel pnl = info.Panel;
        //    ITextBox tb = (ITextBox) info.ControlMappers["SampleText"].Control;
        //    tb.Text = "Test";
        //    Assert.AreEqual("Test", s.SampleText);
        //}

        //[Test]
        //public void TestAlternateConstructor()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, new Sample.SampleUserInterfaceMapper().GetUIFormProperties(), GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreEqual(2, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box.");
        //    Assert.AreSame(typeof (ILabel), pnl.Controls[0].GetType());
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[1].GetType());
        //}

        //[Test]
        //public void TestWithMoreThanOneProperty()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapper3Props(), GetControlFactory());
        //    IPanelFactoryInfo pnlInfo = factory.CreatePanel();
        //    IPanel pnl = pnlInfo.Panel;
        //    Assert.AreEqual(6, pnl.Controls.Count, "The panel should have 6 controls.");
        //    Assert.AreEqual(3, pnlInfo.ControlMappers.Count, "The PanelInfo should have 3 mappers");
        //    Assert.AreSame(typeof (ILabel), pnl.Controls[0].GetType());
        //    int labelWidth = ((IControlChilli)pnl.Controls[0]).Width;
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[1].GetType());
        //    Assert.AreSame(typeof (ILabel), pnl.Controls[2].GetType());
        //    Assert.AreEqual(labelWidth, ((IControlChilli)pnl.Controls[2]).Width);
        //    Assert.AreSame(typeof (IDateTimePicker), pnl.Controls[3].GetType());
        //    Assert.AreSame(typeof (ILabel), pnl.Controls[4].GetType());
        //    Assert.AreEqual(labelWidth, ((IControlChilli)pnl.Controls[4]).Width);
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[5].GetType());
        //}

        //[Test]
        //public void TestWithOnePrivateProperty()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapperPrivatePropOnly(), GetControlFactory());
        //    IPanelFactoryInfo pnlInfo = factory.CreatePanel();
        //    IPanel pnl = pnlInfo.Panel;
        //    Assert.AreEqual(2, pnl.Controls.Count, "The panel should have 2 controls.");
        //    Assert.AreEqual(1, pnlInfo.ControlMappers.Count, "The PanelInfo should have 1 mappers");
        //    Assert.AreSame(typeof(ILabel), pnl.Controls[0].GetType());
        //    //TODO_Port Assert.AreSame(typeof(IPasswordTextBox), pnl.Controls[1].GetType());
        //}

        //[Test]
        //public void TestToolTipWithOneDescribedProperty()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapperDescribedPropOnly(null), GetControlFactory());
        //    IPanelFactoryInfo pnlInfo = factory.CreatePanel();
        //    IPanel pnl = pnlInfo.Panel;
        //    Assert.AreEqual(2, pnl.Controls.Count, "The panel should have 2 controls.");
        //    Assert.AreEqual(1, pnlInfo.ControlMappers.Count, "The PanelInfo should have 1 mappers");
        //    Assert.AreSame(typeof(ILabel), pnl.Controls[0].GetType());
        //    Assert.AreSame(typeof(ITextBox), pnl.Controls[1].GetType());
        //    IToolTip toolTip = pnlInfo.ToolTip;
        //    string toolTipText;
        //    //The label should have the description of the property as it's tooltip.
        //    toolTipText = toolTip.GetToolTip((IControlChilli)pnl.Controls[0]);
        //    Assert.AreSame("This is a sample text property that has a description.", toolTipText);
        //    //The textbox should also have the description of the property as it's tooltip.
        //    toolTipText = toolTip.GetToolTip((IControlChilli)pnl.Controls[1]);
        //    Assert.AreSame("This is a sample text property that has a description.", toolTipText);
        //}

        //[Test]
        //public void TestToolTipWithOneDescribedPropertyWithSpecifiedToolTip()
        //{
        //    Sample s = new Sample();
        //    string controlToolTipText = "This is my control with a tool tip.";
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapperDescribedPropOnly(controlToolTipText), GetControlFactory());
        //    IPanelFactoryInfo pnlInfo = factory.CreatePanel();
        //    IPanel pnl = pnlInfo.Panel;
        //    Assert.AreEqual(2, pnl.Controls.Count, "The panel should have 2 controls.");
        //    Assert.AreEqual(1, pnlInfo.ControlMappers.Count, "The PanelInfo should have 1 mappers");
        //    Assert.AreSame(typeof(ILabel), pnl.Controls[0].GetType());
        //    Assert.AreSame(typeof(ITextBox), pnl.Controls[1].GetType());
        //    IToolTip toolTip = pnlInfo.ToolTip;
        //    string toolTipText;
        //    //The label should have the description of the property as it's tooltip.
        //    toolTipText = toolTip.GetToolTip((IControlChilli)pnl.Controls[0]);
        //    Assert.AreSame(controlToolTipText, toolTipText);
        //    //The textbox should also have the description of the property as it's tooltip.
        //    toolTipText = toolTip.GetToolTip((IControlChilli)pnl.Controls[1]);
        //    Assert.AreSame(controlToolTipText, toolTipText);
        //}

        ////TODO! Think about how this can be accomplished
        ////		[Test]
        ////		public void TestWithMoreThanOneBO() {
        ////			Sample s = new Sample();
        ////			Sample s1 = new Sample();
        ////			PanelFactory factory = new PanelFactory(new BusinessObject[] {s, s1}, new IUserInterfaceMapper[] {null, new Sample.SampleUserInterfaceMapper3Props()});
        ////			Panel pnl = factory.CreatePanel();
        ////			Assert.AreEqual(8, pnl.Controls.Count, "The panel should have 8 controls.");
        ////			Assert.AreSame(typeof (Label), pnl.Controls[0].GetType());
        ////			int labelWidth = pnl.Controls[0].Width;
        ////			Assert.AreSame(typeof (TextBox), pnl.Controls[1].GetType());
        ////			Assert.AreSame(typeof (Label), pnl.Controls[2].GetType());
        ////			Assert.AreEqual(labelWidth, pnl.Controls[2].Width);
        ////			Assert.AreSame(typeof (TextBox), pnl.Controls[3].GetType());
        ////			Assert.AreSame(typeof (Label), pnl.Controls[4].GetType());
        ////			Assert.AreEqual(labelWidth, pnl.Controls[4].Width);
        ////			Assert.AreSame(typeof (DateTimePicker), pnl.Controls[5].GetType());
        ////			Assert.AreSame(typeof (Label), pnl.Controls[6].GetType());
        ////			Assert.AreEqual(labelWidth, pnl.Controls[6].Width);
        ////			Assert.AreSame(typeof (TextBox), pnl.Controls[7].GetType());
        ////		}

        //[Test]
        //public void TestWithMoreThanOneColumn()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapper2Cols(), GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreEqual(8, pnl.Controls.Count, "The panel should have 8 controls.");
        //    int labelWidth = ((IControlChilli)pnl.Controls[0]).Width;
        //    int leftPos = ((IControlChilli)pnl.Controls[0]).Left;
        //    Assert.AreEqual(labelWidth, ((IControlChilli)pnl.Controls[4]).Width); // control 4 is first one on second row
        //    Assert.AreEqual(leftPos, ((IControlChilli)pnl.Controls[4]).Left);
        //    Assert.IsTrue(((IControlChilli)pnl.Controls[2]).Left > leftPos, "New column should be started here");
        //    Assert.AreEqual(109, ((IControlChilli)pnl.Controls[2]).Left); //column width is 100
        //    Assert.AreEqual(261, ((IControlChilli)pnl.Controls[3]).Right); //  column 2 width is 150
        //}

        //[Test]
        //public void TestWithMoreThanOneTab()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapper2Tabs(), GetControlFactory());
        //    IPanelFactoryInfo factoryInfo = factory.CreatePanel();
        //    IPanel pnl = factoryInfo.Panel;
        //    Assert.AreEqual(1, pnl.Controls.Count, "The panel should have 1 control.");
        //    Assert.AreEqual(3, factoryInfo.ControlMappers.Count);
        //    Assert.AreSame(typeof (ITabControl), pnl.Controls[0].GetType(), "The control should be a tabcontrol");
        //    ITabControl tabControl = (ITabControl) pnl.Controls[0];
        //    Assert.AreEqual(2, tabControl.TabPages.Count, "There should be 2 tabs");
        //    Assert.AreEqual("mytab1", tabControl.TabPages[0].Text);
        //    Assert.AreEqual("mytab2", tabControl.TabPages[1].Text);
        //}

        //[Test]
        //public void TestPanelInfoAndPanelSizes()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapper3Props(), GetControlFactory());
        //    IPanelFactoryInfo pnlInfo = factory.CreatePanel();
        //    Assert.AreEqual(300, pnlInfo.PreferredHeight);
        //    Assert.AreEqual(350, pnlInfo.PreferredWidth);
        //    Assert.IsTrue(pnlInfo.Panel.Height < 300);
        //}

        //[Test]
        //public void TestReadOnlyFields()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreEqual(2, pnl.Controls.Count, "The panel should have 2 controls - one label and one text box.");
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[1].GetType());
        //    ITextBox tb = (ITextBox) pnl.Controls[1];
        //    Assert.IsFalse(tb.Enabled);
        //}



        //[Test]
        //public void TestMultiLineTextBox()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[1].GetType());
        //    ITextBox tb = (ITextBox) pnl.Controls[1];
        //    Assert.IsTrue(tb.Multiline, "Textbox should be multiline if NumLines > 1");
        //    ITextBox myTb =  GetControlFactory().CreateTextBox();
        //    Assert.AreEqual(myTb.Height*3, tb.Height);
        //}

        //[Test]
        //public void TestColumnSpanningTextBox()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapperColSpanning(), GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[1].GetType());
        //    ITextBox tb = (ITextBox) pnl.Controls[1];
        //    Assert.AreEqual(167, tb.Width);
        //}

        //[Test]
        //public void TestRowSpanningTextBox()
        //{
        //    Sample s = new Sample();
        //    IPanelFactory factory = new PanelFactory(s, Sample.SampleUserInterfaceMapperRowSpanning(), GetControlFactory());
        //    IPanel pnl = factory.CreatePanel().Panel;
        //    Assert.AreSame(typeof (ITextBox), pnl.Controls[1].GetType());
        //    ITextBox tb = (ITextBox) pnl.Controls[1];
        //    Assert.IsTrue(tb.Multiline, "Textbox should be multiline if NumLines > 1");

        //    ITextBox tb2 = (ITextBox) pnl.Controls[7];
        //    Assert.AreEqual(27, tb2.Top);
        //}
    }
}