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
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    [TestFixture]
    public class TestPanelBuilderWin
    {
        protected const int DEFAULT_CONTROLS_PER_FIELD = 3;
        protected virtual IControlFactory GetControlFactory() { return new ControlFactoryWin(); }

        protected virtual Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperWin(); }


        [SetUp]
        public void SetupTest() { ClassDef.ClassDefs.Clear(); }

        [Test]
        public virtual void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(GetControlFactory().GetType(), panelBuilder.Factory.GetType());
        }

        [Test]
        public void Test_BuildPanel_1Field_String()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof (ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof (IPanel), panel.Controls[2]);

            ILabel label = (ILabel) panel.Controls[0];
            Assert.AreEqual("Text:", label.Text);
        }

        [Test]
        public void Test_BuildPanel_1Field_Integer()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneIntegerField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof (ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof (IPanel), panel.Controls[2]);
        }
       

        [Test]
        public void Test_BuildPanel_1Field_GroupBoxLayout_Integer()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneIntegerField();
            UIFormField formField = singleIntegerFieldTab[0][0];
            formField.Layout = UIFormField.LayoutStyle.GroupBox;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);  //still has a null control in place
            Assert.IsInstanceOfType(typeof (IGroupBox), panel.Controls[0]);
            IGroupBox groupBox = (IGroupBox) panel.Controls[0];

            Assert.IsInstanceOfType(typeof (IPanel), panel.Controls[2]);
            Assert.AreEqual(1, groupBox.Controls.Count);
            Assert.IsInstanceOfType(typeof(INumericUpDown), groupBox.Controls[0]);
        }


        [Test]
        public void Test_BuildPanel_1Field_GroupBoxLayout_TextBox_Multiline()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneFieldWithMultiLineParameter();
            UIFormField formField = singleIntegerFieldTab[0][0];
            formField.Layout = UIFormField.LayoutStyle.GroupBox;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IGroupBox groupBox = (IGroupBox)panel.Controls[0];
            Assert.IsInstanceOfType(typeof(ITextBox), groupBox.Controls[0]);
            Assert.AreEqual(3 * GetControlFactory().CreateTextBox().Height + 4, groupBox.Height);
            ITextBox textBox = (ITextBox)groupBox.Controls[0];
            Assert.IsTrue(textBox.Multiline);
        }

        [Test]
        public void Test_BuildPanel_1Field_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneField();
            UIFormColumn column = singleIntegerFieldTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            ILabel label = (ILabel) panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];

            //--- check horizontal position of label (should be left aligned and sized according to its preferred size) -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize, label.Left);
            Assert.AreEqual(label.PreferredWidth, label.Width);

            //--- check horizontal position of error provider (should be right aligned and a specified width) -----
            Assert.AreEqual(column.Width - LayoutManager.DefaultBorderSize, errorProviderPanel.Left + errorProviderPanel.Width);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, errorProviderPanel.Width);

            //--- check horizontal position of text box (should fill the rest of the row -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize + label.Width + LayoutManager.DefaultGapSize, textbox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, textbox.Left + textbox.Width);
        }


        [Test]
        public void Test_BuildPanel_1Field_GroupBoxLayout_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetSimpleUIFormDef()[0];
            UIFormColumn column = singleIntegerFieldTab[0];
            UIFormField formField = column[0];
            formField.Layout = UIFormField.LayoutStyle.GroupBox;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IGroupBox groupBox = (IGroupBox) panel.Controls[0];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];

            //--- check horizontal position of error provider (should be right aligned and a specified width) -----
            Assert.AreEqual(column.Width - LayoutManager.DefaultBorderSize, errorProviderPanel.Left + errorProviderPanel.Width);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, errorProviderPanel.Width);

            //--- check horizontal position of GroupBox (should be left aligned and fill the row) -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize, groupBox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, groupBox.Left + groupBox.Width);
        }


        [Test]
        public void Test_BuildPanel_2Fields_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoFieldTab = interfaceMapper.GetFormTabTwoFields();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTab).Panel;
            //---------------Test Result -----------------------
            ILabel label1 = (ILabel) panel.Controls[0];
            IControlHabanero textbox1 = panel.Controls[1];
            IPanel errorProviderPanel1 = (IPanel) panel.Controls[2];

            Assert.AreEqual(LayoutManager.DefaultBorderSize, label1.Top);
            Assert.AreEqual(LayoutManager.DefaultBorderSize, textbox1.Top);
            Assert.AreEqual(LayoutManager.DefaultBorderSize, errorProviderPanel1.Top);

            Assert.AreEqual(textbox1.Height, label1.Height);
            Assert.AreEqual(textbox1.Height, errorProviderPanel1.Height);

            ILabel label2 = (ILabel) panel.Controls[3];
            IControlHabanero textbox2 = panel.Controls[4];
            IPanel errorProviderPanel2 = (IPanel) panel.Controls[5];

            int expectedSecondRowTop = textbox1.Top + textbox1.Height + LayoutManager.DefaultGapSize;
            Assert.AreEqual(expectedSecondRowTop, label2.Top);
            Assert.AreEqual(expectedSecondRowTop, textbox2.Top);
            Assert.AreEqual(expectedSecondRowTop, errorProviderPanel2.Top);

            Assert.AreEqual(label1.Left, label2.Left);
            Assert.AreEqual(textbox1.Left, textbox2.Left);
            Assert.AreEqual(errorProviderPanel1.Left, errorProviderPanel2.Left);
            Assert.AreEqual(label1.Right, label2.Right);
            Assert.AreEqual(textbox1.Right, textbox2.Right);
            Assert.AreEqual(errorProviderPanel1.Right, errorProviderPanel2.Right);
        }

        [Test]
        public virtual void Test_BuildPanel_2Fields()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoFieldTab = interfaceMapper.GetFormTabTwoFields();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            int expectedFields = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedFields, twoFieldTab[0].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedFields, panel.Controls.Count);

            //-- Row 1
            Assert.IsInstanceOfType(typeof (ILabel), panel.Controls[0]);
            ILabel row1Label = (ILabel) panel.Controls[0];
            Assert.AreEqual("Text:", row1Label.Text);
            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof (IPanel), panel.Controls[2]);

            //-- Row 2
            Assert.IsInstanceOfType(typeof (ILabel), panel.Controls[3]);
            ILabel row2Label = (ILabel) panel.Controls[3];
            Assert.AreEqual("Integer:", row2Label.Text);
            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[4]);
            Assert.IsInstanceOfType(typeof (IPanel), panel.Controls[5]);
        }

        [Test]
        public void Test_BuildPanel_2Columns_1_1()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_1();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            int expectedColumns = 2;
            int expectedFieldsInEachColumn = 1;
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedColumns, twoColumnTab.Count);
            Assert.AreEqual(expectedFieldsInEachColumn, twoColumnTab[0].Count);
            Assert.AreEqual(expectedFieldsInEachColumn, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedColumns*expectedFieldsInEachColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanel_2Columns_1_2()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_2();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            int expectedColumns = 2;
            int maxFieldsInAColumn = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            Assert.AreEqual(2, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedColumns*maxFieldsInAColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanel_2Columns_1_2_CorrectLayout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_2();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            UIFormColumn formColumn = twoColumnTab[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, formColumn.Count);
            Assert.AreEqual(2, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            IControlCollection panelControls = panel.Controls;
            //-----Row 1 Column 1
            Assert.IsInstanceOfType(typeof (ILabel), panelControls[0]);
            Assert.AreEqual("Text:", panelControls[0].Text);
            Assert.AreEqual(formColumn[0].PropertyName, panelControls[0].Name);
            Assert.IsInstanceOfType(typeof (ITextBox), panelControls[1]);

            Assert.IsInstanceOfType(typeof (IPanel), panelControls[2]);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, panelControls[2].Width);
            //----Row 1 Column 2
            Assert.IsInstanceOfType(typeof (ILabel), panelControls[3]);
            Assert.AreEqual("Integer:", panelControls[3].Text);
            Assert.IsInstanceOfType(typeof (INumericUpDown), panelControls[4]);
            Assert.IsInstanceOfType(typeof (IPanel), panelControls[5]);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, panelControls[5].Width);
            //---Row 2 Column 1
            Assert.IsInstanceOfType(typeof (IControlHabanero), panelControls[6]);
            Assert.IsInstanceOfType(typeof (IControlHabanero), panelControls[7]);
            Assert.IsInstanceOfType(typeof (IControlHabanero), panelControls[8]);
            //---Row 2 Column 2
            Assert.AreEqual("Date:", panelControls[9].Text);
            Assert.IsInstanceOfType(typeof (IDateTimePicker), panelControls[10]);
            Assert.IsInstanceOfType(typeof (IPanel), panelControls[11]);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, panelControls[11].Width);
        }

        [Test]
        public void Test_BuildPanel_ColumnWidths_SingleColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabOneColumnOneRowWithWidth();
            UIFormColumn column1 = twoColumnTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column1Control1 = panel.Controls[0];
            IControlHabanero column1LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD - 1];

            // test the width of the entire panel
            Assert.AreEqual(column1.Width, panel.Width);

            // check that the left control is at the correct position (0 + Border size)
            Assert.AreEqual(LayoutManager.DefaultBorderSize, column1Control1.Left);

            // check that the last control of column 1 has its right edge at the correct position (column width)
            Assert.AreEqual(column1.Width - LayoutManager.DefaultBorderSize, column1LastControl.Left + column1LastControl.Width);
        }

        [Test]
        public void Test_BuildPanel_ColumnWidths_FirstColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();
            UIFormColumn column1 = twoColumnTab[0];
            UIFormColumn column2 = twoColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column1Control1 = panel.Controls[0];
            IControlHabanero column1LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD - 1];

            // test the width of the entire panel
            Assert.AreEqual(column1.Width + column2.Width, panel.Width);

            // check that the left control is at the correct position (0 + Border size)
            Assert.AreEqual(LayoutManager.DefaultBorderSize, column1Control1.Left);

            // check that the last control of column 1 has its right edge at the correct position (column width)
            Assert.AreEqual(column1.Width, column1LastControl.Left + column1LastControl.Width);
        }

        [Test]
        public void Test_BuildPanel_ColumnWidths_LastColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();

            UIFormColumn column1 = twoColumnTab[0];
            UIFormColumn column2 = twoColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD*2 - 1];
            IControlHabanero column2Control1 = panel.Controls[DEFAULT_CONTROLS_PER_FIELD];

            // check that the first control of the second column is at correct left position (column 1 width + gap)
            Assert.AreEqual(column1.Width + LayoutManager.DefaultGapSize, column2Control1.Left);

            // check that the last control of column 2 has its right edge at the correct position (panel width - border)
            Assert.AreEqual(column1.Width + column2.Width - LayoutManager.DefaultBorderSize,
                            column2LastControl.Left + column2LastControl.Width);
        }

        [Test]
        public void Test_BuildPanel_ColumnWidths_MiddleColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab threeColumnTab = interfaceMapper.GetFormTabThreeColumnsOneRowWithWidths();

            UIFormColumn column1 = threeColumnTab[0];
            UIFormColumn column2 = threeColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(threeColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD*2 - 1];
            IControlHabanero column2Control1 = panel.Controls[DEFAULT_CONTROLS_PER_FIELD];

            // check that the first control of the second column is at correct left position (column 1 width + gap)
            Assert.AreEqual(column1.Width + LayoutManager.DefaultGapSize, column2Control1.Left);

            // check that the last control of column 2 has its right edge at the correct position (panel width - border)
            Assert.AreEqual(column1.Width + column2.Width, column2LastControl.Left + column2LastControl.Width);
        }


        [Test]
        public void Test_BuildPanel_ColumnWidths_DataColumnResizes()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneFieldNoColumnWidth();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;

            int columnWidthOrig = 300;
            int columnWidthAfter = 500;
            panel.Width = columnWidthOrig;
            ILabel label = (ILabel) panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];
            int originalWidth = textbox.Width;

            //---------------Assert Precondition----------------
            Assert.AreEqual(label.Left + label.Width + LayoutManager.DefaultGapSize, textbox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, textbox.Left + textbox.Width);
            //---------------Execute Test ----------------------
            panel.Width = columnWidthAfter;
            //---------------Test Result -----------------------

            Assert.AreEqual(originalWidth + columnWidthAfter - columnWidthOrig, textbox.Width);
        }

        [Test]
        public virtual void Test_BuildPanel_3Columns_1Column_RowSpan2()
        {
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabThreeColumnsOneColumnWithRowSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IControlHabanero textBoxCol1 = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlHabanero col1Text1RowSpan2Label = panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            IControlHabanero textBoxCol2 = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            IControlHabanero nullControl =
                panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];
            ILabel col2TextBox2Label =
                (ILabel) panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*4];

            Assert.IsNotInstanceOfType(typeof (ILabel), nullControl);
            Assert.AreEqual(textBoxCol2.Height*2 + LayoutManager.DefaultGapSize, textBoxCol1.Height);
            Assert.AreEqual(col1Text1RowSpan2Label.Left, nullControl.Left);

            Assert.IsInstanceOfType(typeof (ILabel), col2TextBox2Label);
            Assert.AreEqual("Col2TextBox2", col2TextBox2Label.Text);
            Assert.AreEqual(
                textBoxCol1.Left + textBoxCol1.Width + PanelBuilder.ERROR_PROVIDER_WIDTH + GridLayoutManager.DefaultGapSize*2,
                col2TextBox2Label.Left);
        }

        [Test]
        public void Test_BuildPanel_RowSpanAndColumnSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab oneFieldRowColSpan = interfaceMapper.GetFormTabOneFieldHasRowAndColSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(oneFieldRowColSpan).Panel;
            //---------------Test Result -----------------------
            IControlCollection controlCollection = panel.Controls;
        }

        [Test]
        public void Test_BuildPanel_RowSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab formTab = interfaceMapper.GetFormTabOneColumnThreeRowsWithRowSpan();

            UIFormColumn column1 = formTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(formTab).Panel;
            //---------------Test Result -----------------------

            IControlHabanero row1InputControl = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            ITextBox row2InputControl =
                (ITextBox) panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];

            Assert.IsTrue(row2InputControl.Multiline);
            Assert.AreEqual(row1InputControl.Height*2 + LayoutManager.DefaultGapSize, row2InputControl.Height);
        }

        [Test]
        public void Test_BuildPanel_Parameter_ColumnSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab formTab = interfaceMapper.GetFormTabTwoColumns_2_1_ColSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(formTab).Panel;
            //---------------Test Result -----------------------

            IControlHabanero columnSpanningControl = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlHabanero columnSpanningErrorProviderControl = panel.Controls[PanelBuilder.ERROR_PROVIDER_COLUMN_NO];
            IControlHabanero row2Col1InputControl =
                panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*2];
            IControlHabanero row2Col2InputControl =
                panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];
            IControlHabanero row2Col2ErrorProviderControl =
                panel.Controls[PanelBuilder.ERROR_PROVIDER_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];

            // -- check that the col spanning control is the correct width
            Assert.AreEqual(row2Col1InputControl.Left, columnSpanningControl.Left);
            Assert.AreEqual(row2Col2InputControl.Right, columnSpanningControl.Right);

            // check that the error provider control is in the correct position (on the right of the col spanning control)
            Assert.AreEqual(row2Col2ErrorProviderControl.Left, columnSpanningErrorProviderControl.Left);

            // check that the first control in the second column is moved down to accommodate the col spanning control
            Assert.AreEqual(row2Col1InputControl.Top, row2Col2InputControl.Top);
        }

        [Test]
        public void Test_BuildPanel_Parameter_DefaultAlignment_Left()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabTwoFieldsWithNoAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsTrue(String.IsNullOrEmpty(singleFieldTab[0][0].Alignment));

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Left, control.TextAlign);

            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            INumericUpDown numericUpDown = (INumericUpDown) panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(HorizontalAlignment.Left, numericUpDown.TextAlign);
        }

        [Test]
        public void Test_BuildPanel_Parameter_Alignment_Right()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithRightAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("right", singleFieldTab[0][0].Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Right, control.TextAlign);
        }


        [Test]
        public void Test_BuildPanel_Parameter_Alignment_Center()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithCenterAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("center", singleFieldTab[0][0].Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Center, control.TextAlign);
        }

        [Test]
        public void Test_BuildPanel_Parameter_Alignment_InvalidAlignment()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithInvalidAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            bool errorThrown = false;
            string errMessage = "";
            //---------------Assert Precondition----------------
            Assert.AreEqual("Top", singleFieldTab[0][0].Alignment);
            //---------------Execute Test ----------------------

            try
            {
                IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            }
            catch (HabaneroDeveloperException ex)
            {
                errorThrown = true;
                errMessage = ex.Message;
            }

            //---------------Test Result -----------------------

            Assert.IsTrue(errorThrown, "The alignment value is invalid and a HabaneroDeveloperException should be thrown.");
            StringAssert.Contains("Invalid alignment property value ", errMessage);
        }

        [Test]
        public void Test_BuildPanel_Parameter_MultiLine()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithMultiLineParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.IsTrue(control.Multiline);
            Assert.IsTrue(control.AcceptsReturn);
            Assert.AreEqual(64, control.Height);
            Assert.AreEqual(ScrollBars.Vertical, control.ScrollBars);
        }

        [Test]
        public void Test_BuildPanel_Parameter_InvalidMultiLineValue()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithInvalidMultiLineParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            bool errorThrown = false;
            string errMessage = "";

            //---------------Execute Test ----------------------
            try
            {
                IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            }
            catch (InvalidXmlDefinitionException ex)
            {
                errorThrown = true;
                errMessage = ex.Message;
            }
            Assert.IsTrue(errorThrown,
                          "An error occurred while reading the 'numLines' parameter from the class definitions.  The 'value' attribute must be a valid integer.");
            StringAssert.Contains(
                "An error occurred while reading the 'numLines' parameter from the class definitions.  The 'value' attribute must be a valid integer.",
                errMessage);
        }

        [Test]
        public void Test_BuildPanel_Parameter_DecimalPlaces_NumericUpDownMoneyMapper()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithDecimalPlacesParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsFalse(String.IsNullOrEmpty(singleFieldTab[0][0].DecimalPlaces));
            Assert.AreEqual("3", singleFieldTab[0][0].DecimalPlaces);
            Assert.AreEqual("NumericUpDownCurrencyMapper", singleFieldTab[0][0].MapperTypeName);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[1]);
            INumericUpDown control = (INumericUpDown) panel.Controls[1];
            Assert.AreEqual(3, control.DecimalPlaces);
        }

        [Test]
        public void Test_BuildPanel_Parameter_Options()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithOptionsParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsFalse(String.IsNullOrEmpty(singleFieldTab[0][0].Options));
            Assert.AreEqual("M|F", singleFieldTab[0][0].Options);
            Assert.AreEqual("ListComboBoxMapper", singleFieldTab[0][0].MapperTypeName);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (IComboBox), panel.Controls[1]);
            IComboBox control = (IComboBox) panel.Controls[1];
            Assert.AreEqual(3, control.Items.Count);
            Assert.AreEqual("", control.Items[0].ToString());
            Assert.AreEqual("M", control.Items[1].ToString());
            Assert.AreEqual("F", control.Items[2].ToString());
        }

        [Test, Ignore("Can not test the event on the TextBox")]
        public void Test_BuildPanel_Parameter_isEmal()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithIsEmailParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsFalse(String.IsNullOrEmpty(singleFieldTab[0][0].IsEmail));
            Assert.IsTrue(Convert.ToBoolean(singleFieldTab[0][0].IsEmail));

            Assert.IsFalse(String.IsNullOrEmpty(singleFieldTab[0][1].IsEmail));
            Assert.IsFalse(Convert.ToBoolean(singleFieldTab[0][1].IsEmail));

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
        }

        [Test]
        public void Test_BuildPanel_Parameter_DateFormat()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithDateFormatParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsFalse(String.IsNullOrEmpty(singleFieldTab[0][0].DateFormat));
            Assert.IsFalse(String.IsNullOrEmpty(singleFieldTab[0][1].DateFormat));

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof(IDateTimePicker), panel.Controls[1]);
            IDateTimePicker control1 = (IDateTimePicker)panel.Controls[1];
            Assert.AreEqual(DateTimePickerFormat.Short, control1.Format);

            Assert.IsInstanceOfType(typeof(IDateTimePicker), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            IDateTimePicker control2 = (IDateTimePicker)panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(DateTimePickerFormat.Custom, control2.Format);
            Assert.AreEqual(singleFieldTab[0][1].DateFormat, control2.CustomFormat);
        }

        [Test]
        public void Test_BuildPanel_Parameter_DateFormat_DateTimePicker_WithUnspecifiedMapper()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithDateFormatParameter(false);
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            UIFormField formField1 = singleFieldTab[0][0];
            UIFormField formField2 = singleFieldTab[0][1];
            //---------------Assert Precondition----------------
            Assert.IsFalse(String.IsNullOrEmpty(formField1.DateFormat));
            Assert.IsFalse(String.IsNullOrEmpty(formField2.DateFormat));
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof(IDateTimePicker), panel.Controls[1]);
            IDateTimePicker control1 = (IDateTimePicker)panel.Controls[1];
            Assert.AreEqual(DateTimePickerFormat.Short, control1.Format);

            Assert.IsInstanceOfType(typeof(IDateTimePicker), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            IDateTimePicker control2 = (IDateTimePicker)panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(DateTimePickerFormat.Custom, control2.Format);
            Assert.AreEqual(formField2.DateFormat, control2.CustomFormat);
        }

        [Test]
        public void Test_BuildPanel_GetAlignmentValueMethod_Left()
        {
            //---------------Set up test pack-------------------
            string alignment = "left";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------

            Assert.AreEqual(HorizontalAlignment.Left, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Left, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanel_GetAlignmentValueMethod_Right()
        {
            //---------------Set up test pack-------------------
            string alignment = "right";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Right, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Right, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanel_GetAlignmentValueMethod_Center()
        {
            //---------------Set up test pack-------------------
            string alignment = "center";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanel_GetAlignmentValueMethod_Centre()
        {
            //---------------Set up test pack-------------------
            string alignment = "centre";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }


        //Throws a HabaneroDeveloperException 
        //if an invalid alignment value is passed into the method.
        [Test]
        public void Test_BuildPanel_GetAlignmentValueMethod_ThrowsADeveloperException()
        {
            //---------------Set up test pack-------------------
            string alignment = "TestAlignment";
            bool errorThrown = false;
            string errMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            try
            {
                HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            }
            catch (HabaneroDeveloperException ex)
            {
                errorThrown = true;
                errMessage = ex.Message;
            }

            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "The alignment value is invalid and a HabaneroDeveloperException should be thrown.");
            StringAssert.Contains("Invalid alignment property value ", errMessage);
        }


        [Test]
        public void Test_BuildPanel_CompulsoryFieldsAreBoldAndStarred()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneCompulsory();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory).Panel;
            //---------------Test Result -----------------------
            IControlCollection controls = panel.Controls;

            ILabel compulsoryLabel = (ILabel) controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            Assert.AreEqual("CompulsorySampleText: *", compulsoryLabel.Text);
            Assert.IsTrue(compulsoryLabel.Font.Bold);

            ILabel nonCompulsoryLabel =
                (ILabel) controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            Assert.AreEqual("SampleTextNotCompulsory:", nonCompulsoryLabel.Text);
            Assert.IsFalse(nonCompulsoryLabel.Font.Bold);
        }

        [Test]
        public void Test_BuildPanel_PopulatesFieldInfoCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            Sample sample = new Sample();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            panelInfo.BusinessObject = sample;
            //---------------Test Result -----------------------

            IControlHabanero sampleTextLabel = panelInfo.Panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            IControlHabanero sampleTextInputControl = panelInfo.Panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            string propertyName = "SampleText";
            IControlMapper sampleTextControlMapper = panelInfo.FieldInfos[propertyName].ControlMapper;

            Assert.AreEqual(2, panelInfo.FieldInfos.Count);

            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos[propertyName];
            Assert.AreSame(sampleTextLabel, fieldInfo.Label);
            Assert.AreSame(sampleTextInputControl, fieldInfo.InputControl);

            Assert.AreSame(sampleTextInputControl, sampleTextControlMapper.Control);
            Assert.AreSame(sample, sampleTextControlMapper.BusinessObject);
            Assert.AreEqual(propertyName, sampleTextControlMapper.PropertyName);
        }


        [Test]
        public void Test_BuildPanel_InputControlsHaveCorrectEnabledState()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            panelInfo.BusinessObject = new Sample();

            //---------------Test Result -----------------------
            Assert.IsTrue(panelInfo.FieldInfos[0].InputControl.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Enabled);
        }

        [Test]
        public void Test_BuildPanel_LayoutManagerIsSet()
        {
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);

            //---------------Test Result -----------------------
            GridLayoutManager layoutManager = panelInfo.LayoutManager;
            Assert.IsNotNull(layoutManager);
            Assert.AreEqual(2, layoutManager.Rows.Count);
        }

        [Test]
        public void Test_BuildTabControl_TwoTabPagesOnTabControl()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, panelInfo.Panel.Controls.Count); // only one control because it's the tab control
            Assert.IsInstanceOfType(typeof (ITabControl), panelInfo.Panel.Controls[0]);
            ITabControl tabControl = (ITabControl) panelInfo.Panel.Controls[0];
            Assert.AreEqual(form.Count, tabControl.TabPages.Count);
        }


        [Test]
        public void Test_BuildTabControl_CorrectControlsOnTabPage()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            ITabControl tabControl = (ITabControl) panelInfo.Panel.Controls[0];
            ITabPage tabPage1 = tabControl.TabPages[0];
            ITabPage tabPage2 = tabControl.TabPages[1];
            Assert.AreEqual(1, tabPage1.Controls.Count);
            Assert.IsInstanceOfType(typeof (IPanel), tabPage1.Controls[0]);
            IPanel tabPage1Panel = (IPanel) tabPage1.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage1Panel.Controls.Count);
            Assert.AreEqual(1, tabPage2.Controls.Count);
            Assert.IsInstanceOfType(typeof (IPanel), tabPage2.Controls[0]);
            IPanel tabPage2Panel = (IPanel) tabPage2.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage2Panel.Controls.Count);
        }

        [Test]
        public void Test_BuildTabControl_PanelInfos()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(form.Count, panelInfo.PanelInfos.Count);
            Assert.AreEqual(panelInfo.FieldInfos.Count,
                            panelInfo.PanelInfos[0].FieldInfos.Count + panelInfo.PanelInfos[1].FieldInfos.Count);
        }

        [Test]
        public void Test_BuildPanelForForm_ReturnsOnlyPanelForOneTab()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIForm form = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, panelInfo.Panel.Controls.Count);
            Assert.AreEqual(form, panelInfo.UIForm);
        }

        [Test]
        public void Test_CreateOnePanelPerUIFormTab_2Panels()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithTwoUITabs();
            UIForm uiForm = classDef.UIDefCol["default"].UIForm;
            MyBO myBo = new MyBO();
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IList<IPanelInfo> panelList = panelBuilder.CreateOnePanelPerUIFormTab(uiForm);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelList.Count);
            Assert.AreEqual("Tab1", panelList[0].UIFormTab.Name);
            Assert.AreEqual("Tab2", panelList[1].UIFormTab.Name);
            Assert.AreSame(uiForm, panelList[0].UIForm);
            Assert.AreSame(uiForm, panelList[1].UIForm);
        }

        [Test]
        public void Test_MinimumPanelHeight()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIForm uiForm = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(uiForm[0]);
            IPanel pnl = panelInfo.Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(pnl.Height, panelInfo.MinimumPanelHeight);
            //---------------Tear Down -------------------------          
        }


        //[Test, Ignore("This doesn't work in code for some reason")]
        //public void Test_BuildPanel_SetToolTip()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneWithToolTipText();
        //    UIFormTab twoFieldTabOneHasToolTip = classDef.UIDefCol["default"].UIForm[0];
        //    PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
        //    //-------------Assert Preconditions -------------

        //    //---------------Execute Test ----------------------
        //    IPanel panel = panelBuilder.BuildPanel(twoFieldTabOneHasToolTip).Panel;
        //    //---------------Test Result -----------------------
        //    IControlCollection controls = panel.Controls;
        //    ILabel labelWithToolTip =
        //        (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
        //    IControlHabanero controlHabanero =
        //        controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
        //    IToolTip toolTip = GetControlFactory().CreateToolTip();

        //    Assert.AreEqual("Test tooltip text", toolTip.GetToolTip(controlHabanero));
        //}
    }

    [TestFixture]
    public class TestPanelBuilderWinOnly
    {
        protected const int DEFAULT_CONTROLS_PER_FIELD = 3;
        protected virtual IControlFactory GetControlFactory() { return new ControlFactoryWin(); }

        protected virtual Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperWin(); }

        [Test]
        public void Test_BuildPanel_Parameter_SetNumericUpDownAlignment()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithAlignment_NumericUpDown();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("left", singleFieldTab[0][0].Alignment);
            Assert.AreEqual("right", singleFieldTab[0][1].Alignment);
            Assert.AreEqual("center", singleFieldTab[0][2].Alignment);
            Assert.AreEqual("centre", singleFieldTab[0][3].Alignment);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[1]);
            INumericUpDown control1 = (INumericUpDown) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Left, control1.TextAlign);

            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            INumericUpDown control2 = (INumericUpDown) panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(HorizontalAlignment.Right, control2.TextAlign);

            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN*2 + 1]);
            INumericUpDown control3 = (INumericUpDown) panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN*2 + 1];
            Assert.AreEqual(HorizontalAlignment.Center, control3.TextAlign);

            Assert.IsInstanceOfType(typeof (INumericUpDown), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN*3 + 1]);
            INumericUpDown control4 = (INumericUpDown) panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN*3 + 1];
            Assert.AreEqual(HorizontalAlignment.Center, control4.TextAlign);
        }
    }

    [TestFixture]
    public class TestPanelBuilderVWG : TestPanelBuilderWin
    {
        protected override IControlFactory GetControlFactory() { return new ControlFactoryVWG(); }

        protected override Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperVWG(); }

        [Test, Ignore("Gizmox does not support changing the TextAlign Property (Default value iss Left) ")]
        public void Test_BuildPanel_Parameter_SetAlignment_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithNumericUpDown();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("right", singleFieldTab[0][0].Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Right, control.TextAlign);
        }
    }
}