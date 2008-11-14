using System;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.PanelBuilder
{
    [TestFixture]
    public class TestPanelBuilder
    {
        private const int DEFAULT_CONTROLS_PER_FIELD = 3;

        private IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(ControlFactoryWin), panelBuilder.Factory.GetType());
        }

        [Test]
        public void Test_BuildPanel_1Field_String()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(singleFieldTab);
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(ITextBox), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof(IPanel), panel.Controls[2]);
            
            ILabel label = (ILabel)panel.Controls[0];
            Assert.AreEqual("Text:", label.Text);

        }

        [Test]
        public void Test_BuildPanel_1Field_Integer()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneIntegerField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(singleIntegerFieldTab);
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(INumericUpDown), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof(IPanel), panel.Controls[2]);
        }

        [Test]
        public void Test_BuildPanel_1Field_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneField();
            UIFormColumn column = singleIntegerFieldTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(singleIntegerFieldTab);
            //---------------Test Result -----------------------
            ILabel label = (ILabel) panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];

            //--- check horizontal position of label (should be left aligned and sized according to its preferred size) -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize, label.Left);
            Assert.AreEqual(label.PreferredWidth, label.Width);

            //--- check horizontal position of error provider (should be right aligned and a specified width) -----
            Assert.AreEqual(column.Width - LayoutManager.DefaultBorderSize, errorProviderPanel.Right);
            Assert.AreEqual(PanelBuilder.DefaultErrorProviderWidth, errorProviderPanel.Width);
           
            //--- check horizontal position of text box (should fill the rest of the row -----
            Assert.AreEqual(label.Right + LayoutManager.DefaultGapSize, textbox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, textbox.Right);
        }

        [Test]
        public void Test_BuildPanel_2Fields_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoFieldTab = interfaceMapper.GetFormTabTwoFields();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoFieldTab);
            //---------------Test Result -----------------------
            ILabel label1 = (ILabel)panel.Controls[0];
            IControlHabanero textbox1 = panel.Controls[1];
            IPanel errorProviderPanel1 = (IPanel)panel.Controls[2];

            Assert.AreEqual(LayoutManager.DefaultBorderSize, label1.Top);
            Assert.AreEqual(LayoutManager.DefaultBorderSize, textbox1.Top);
            Assert.AreEqual(LayoutManager.DefaultBorderSize, errorProviderPanel1.Top);

            Assert.AreEqual(textbox1.Height, label1.Height);
            Assert.AreEqual(textbox1.Height, errorProviderPanel1.Height);

            ILabel label2 = (ILabel)panel.Controls[3];
            IControlHabanero textbox2 = panel.Controls[4];
            IPanel errorProviderPanel2 = (IPanel)panel.Controls[5];

            int expectedSecondRowTop = textbox1.Bottom + LayoutManager.DefaultGapSize;
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
        public void Test_BuildPanel_2Fields()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoFieldTab = interfaceMapper.GetFormTabTwoFields();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            int expectedFields = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedFields, twoFieldTab[0].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoFieldTab);
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD * expectedFields, panel.Controls.Count);

            //-- Row 1
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            ILabel row1Label = (ILabel)panel.Controls[0];
            Assert.AreEqual("Text:", row1Label.Text);
            Assert.IsInstanceOfType(typeof(ITextBox), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof(IPanel), panel.Controls[2]);

            //-- Row 2
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[3]);
            ILabel row2Label = (ILabel)panel.Controls[3];
            Assert.AreEqual("Integer:", row2Label.Text);
            Assert.IsInstanceOfType(typeof(NumericUpDownWin), panel.Controls[4]);
            Assert.IsInstanceOfType(typeof(IPanel), panel.Controls[5]);

        }

        [Test]
        public void Test_BuildPanel_2Columns_1_1()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_1();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            int expectedColumns = 2;
            int expectedFieldsInEachColumn = 1;
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedColumns, twoColumnTab.Count);
            Assert.AreEqual(expectedFieldsInEachColumn, twoColumnTab[0].Count);
            Assert.AreEqual(expectedFieldsInEachColumn, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD * expectedColumns * expectedFieldsInEachColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanel_2Columns_1_2()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_2();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            int expectedColumns = 2;
            int maxFieldsInAColumn = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            Assert.AreEqual(2, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD * expectedColumns * maxFieldsInAColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanel_2Columns_1_2_CorrectLayout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_2();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            Assert.AreEqual(2, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);
            //---------------Test Result -----------------------
            IControlCollection panelControls = panel.Controls;
            //-----Row 1 Column 1
            Assert.IsInstanceOfType(typeof(ILabel), panelControls[0]);
            Assert.AreEqual("Text:",panelControls[0].Text);
            Assert.IsInstanceOfType(typeof(ITextBox),panelControls[1]);

            Assert.IsInstanceOfType(typeof (IPanel), panelControls[2]);
            Assert.AreEqual(PanelBuilder.DefaultErrorProviderWidth, panelControls[2].Width);
            //----Row 1 Column 2
            Assert.IsInstanceOfType(typeof(ILabel),panelControls[3]);
            Assert.AreEqual("Integer:", panelControls[3].Text);
            Assert.IsInstanceOfType(typeof(INumericUpDown), panelControls[4]);
            Assert.IsInstanceOfType(typeof(IPanel), panelControls[5]);
            Assert.AreEqual(PanelBuilder.DefaultErrorProviderWidth, panelControls[5].Width);
            //---Row 2 Column 1
            Assert.IsInstanceOfType(typeof(IControlHabanero),panelControls[6]);
            Assert.IsInstanceOfType(typeof(IControlHabanero),panelControls[7]);
            Assert.IsInstanceOfType(typeof(IControlHabanero),panelControls[8]);
            //---Row 2 Column 2
            Assert.AreEqual("Date:", panelControls[9].Text);
            Assert.IsInstanceOfType(typeof(IDateTimePicker), panelControls[10]);
            Assert.IsInstanceOfType(typeof(IPanel), panelControls[11]);
            Assert.AreEqual(PanelBuilder.DefaultErrorProviderWidth, panelControls[11].Width);
        }

        [Test]
        public void Test_BuildPanel_ColumnWidths_SingleColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabOneColumnOneRowWithWidth();
            UIFormColumn column1 = twoColumnTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);

            //---------------Test Result -----------------------
            IControlHabanero column1Control1 = panel.Controls[0];
            IControlHabanero column1LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD - 1];

            // test the width of the entire panel
            Assert.AreEqual(column1.Width, panel.Width);

            // check that the left control is at the correct position (0 + Border size)
            Assert.AreEqual(LayoutManager.DefaultBorderSize, column1Control1.Left);

            // check that the last control of column 1 has its right edge at the correct position (column width)
            Assert.AreEqual(column1.Width - LayoutManager.DefaultBorderSize, column1LastControl.Right);
        } 

        [Test]
        public void Test_BuildPanel_ColumnWidths_FirstColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();
            UIFormColumn column1 = twoColumnTab[0];
            UIFormColumn column2 = twoColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);

            //---------------Test Result -----------------------
            IControlHabanero column1Control1 = panel.Controls[0];
            IControlHabanero column1LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD - 1];
            
            // test the width of the entire panel
            Assert.AreEqual(column1.Width + column2.Width, panel.Width);

            // check that the left control is at the correct position (0 + Border size)
            Assert.AreEqual(LayoutManager.DefaultBorderSize, column1Control1.Left);

            // check that the last control of column 1 has its right edge at the correct position (column width)
            Assert.AreEqual(column1.Width, column1LastControl.Right);
        }        
        
        [Test]
        public void Test_BuildPanel_ColumnWidths_LastColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();

            UIFormColumn column1 = twoColumnTab[0];
            UIFormColumn column2 = twoColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD*2 - 1];
            IControlHabanero column2Control1 = panel.Controls[DEFAULT_CONTROLS_PER_FIELD];

            // check that the first control of the second column is at correct left position (column 1 width + gap)
            Assert.AreEqual(column1.Width + LayoutManager.DefaultGapSize, column2Control1.Left);

            // check that the last control of column 2 has its right edge at the correct position (panel width - border)
            Assert.AreEqual(column1.Width + column2.Width - LayoutManager.DefaultBorderSize, column2LastControl.Right);
        }

        [Test]
        public void Test_BuildPanel_ColumnWidths_MiddleColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab threeColumnTab = interfaceMapper.GetFormTabThreeColumnsOneRowWithWidths();

            UIFormColumn column1 = threeColumnTab[0];
            UIFormColumn column2 = threeColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(threeColumnTab);

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD * 2 - 1];
            IControlHabanero column2Control1 = panel.Controls[DEFAULT_CONTROLS_PER_FIELD];

            // check that the first control of the second column is at correct left position (column 1 width + gap)
            Assert.AreEqual(column1.Width + LayoutManager.DefaultGapSize, column2Control1.Left);

            // check that the last control of column 2 has its right edge at the correct position (panel width - border)
            Assert.AreEqual(column1.Width + column2.Width, column2LastControl.Right);
        }    

        [Test]
        public void Test_BuildPanel_ColumnWidths_DataColumnResizes()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneFieldNoColumnWidth();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanel panel = panelBuilder.BuildPanel(singleIntegerFieldTab);
            
            int columnWidthOrig = 300;
            int columnWidthAfter = 500;
            panel.Width = columnWidthOrig;
            ILabel label = (ILabel)panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel)panel.Controls[2];
            int originalWidth = textbox.Width;
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(label.Right + LayoutManager.DefaultGapSize, textbox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, textbox.Right);
            //---------------Execute Test ----------------------
            panel.Width = columnWidthAfter;
            //---------------Test Result -----------------------

            Assert.AreEqual(originalWidth + columnWidthAfter - columnWidthOrig, textbox.Width);
        }

        [Test]
        public void Test_BuildPanel_RowSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab formTab = interfaceMapper.GetFormTabOneColumnThreeRowsWithRowSpan();

            UIFormColumn column1 = formTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(formTab);
            //---------------Test Result -----------------------

            IControlHabanero row1InputControl = panel.Controls[PanelBuilder.InputControlPosition];
            ITextBox row2InputControl =
                (ITextBox) panel.Controls[PanelBuilder.InputControlPosition + PanelBuilder.NumberOfControlsInColumn];

            Assert.IsTrue(row2InputControl.Multiline);
            Assert.AreEqual(row1InputControl.Height*2 + LayoutManager.DefaultGapSize,row2InputControl.Height);

        }    
        
        [Test]
        public void Test_BuildPanel_ColumnSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab formTab = interfaceMapper.GetFormTabTwoColumns_2_1_ColSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(formTab);
            //---------------Test Result -----------------------

            IControlHabanero columnSpanningControl = panel.Controls[PanelBuilder.InputControlPosition];
            IControlHabanero columnSpanningErrorProviderControl = panel.Controls[PanelBuilder.ErrorProviderPosition];
            IControlHabanero row2Col1InputControl = panel.Controls[PanelBuilder.InputControlPosition + PanelBuilder.NumberOfControlsInColumn*2];
            IControlHabanero row2Col2InputControl = panel.Controls[PanelBuilder.InputControlPosition + PanelBuilder.NumberOfControlsInColumn*3];
            IControlHabanero row2Col2ErrorProviderControl = panel.Controls[PanelBuilder.ErrorProviderPosition + PanelBuilder.NumberOfControlsInColumn * 3];

            // -- check that the col spanning control is the correct width
            Assert.AreEqual(row2Col1InputControl.Left, columnSpanningControl.Left);
            Assert.AreEqual(row2Col2InputControl.Right, columnSpanningControl.Right);

            // check that the error provider control is in the correct position (on the right of the col spanning control)
            Assert.AreEqual(row2Col2ErrorProviderControl.Left, columnSpanningErrorProviderControl.Left);

            // check that the first control in the second column is moved down to accommodate the col spanning control
            Assert.AreEqual(row2Col1InputControl.Top, row2Col2InputControl.Top);
        }
        
        internal class PanelBuilder
        {
            private IControlFactory _factory;
            public const int DefaultErrorProviderWidth = 20;
            public const int NumberOfControlsInColumn = 3;
            public const int LabelControlPosition = 0;
            public const int InputControlPosition = 1;
            public const int ErrorProviderPosition = NumberOfControlsInColumn - 1;
            

            public PanelBuilder(IControlFactory factory)
            {
                _factory = factory;
            }

            public IControlFactory Factory
            {
                get { return _factory; }
                set { _factory = value; }
            }

            public IPanel BuildPanel(UIFormTab formTab)
            {
                IPanel panel = Factory.CreatePanel();

                GridLayoutManager layoutManager = SetupLayoutManager(formTab, panel);
                AddFieldsToLayoutManager(formTab, layoutManager);
                SetupInputControlColumnWidth(formTab, layoutManager);

                panel.Width = layoutManager.GetFixedWidthIncludingGaps();
                return panel;
            }

            private void SetupInputControlColumnWidth(UIFormTab formTab, GridLayoutManager layoutManager)
            {
                int formColCount = 0;
                foreach (UIFormColumn formColumn in formTab)
                {
                    if (formColumn.Width < 0) continue;
                    int gridCol = formColCount * NumberOfControlsInColumn;
                    int labelColumnWidth = layoutManager.GetFixedColumnWidth(gridCol + LabelControlPosition);
                    int errorProviderColumnWidth = layoutManager.GetFixedColumnWidth(gridCol + ErrorProviderPosition);
                    int totalGap = (NumberOfControlsInColumn - 1)*layoutManager.GapSize;
                    if (formTab.Count == 1) totalGap += 2 * layoutManager.BorderSize; // add extra border for single column
                    else if (formColCount == formTab.Count - 1) totalGap += layoutManager.BorderSize + layoutManager.GapSize; // last column in multi-column
                    else if (formColCount > 0 && formTab.Count > 0) totalGap += layoutManager.GapSize; //2 More gaps for internal column in multi-column
                    else if (formColCount == 0 && formTab.Count > 0) totalGap += layoutManager.BorderSize;

                    layoutManager.FixColumn(gridCol + InputControlPosition, formColumn.Width - labelColumnWidth - errorProviderColumnWidth - totalGap);
                    formColCount++;
                }
            }

            private void AddFieldsToLayoutManager(UIFormTab formTab, GridLayoutManager layoutManager)
            {
                int[] columnPos = new int[formTab.Count];
                int[] columnRowSpanCount = new int[formTab.Count];
                for (int currentRow = 0; currentRow < formTab.GetMaxRowsInColumns(); currentRow++)
                {
                    layoutManager.FixRow(currentRow, Factory.CreateTextBox().Height);
                    int col = 0;
                    int lastColSpan = 0;
                    foreach (UIFormColumn formColumn in formTab)
                    {
                        if (--columnRowSpanCount[col] > 0) continue;
                        if (--lastColSpan > 0) continue;
                        
                        int currentFieldInColumn = columnPos[col];
                        if (currentFieldInColumn < formColumn.Count) // there exists a field in this row in this column
                        {
                            UIFormField formField = formColumn[currentFieldInColumn];
                            columnRowSpanCount[col] = formField.RowSpan;
                            lastColSpan = formField.ColSpan;
                            layoutManager.AddControl(Factory.CreateLabel(formField.GetLabel()));
                            IControlHabanero inputControl = Factory.CreateControl(formField.ControlTypeName,
                                                                                  formField.ControlAssemblyName);
                            int rowSpan = 1;
                            int colSpan = 1;
                            if (formField.HasParameterValue("rowSpan"))
                            {
                                if (inputControl is ITextBox) ((ITextBox) inputControl).Multiline = true;
                                rowSpan = Convert.ToInt32(formField.GetParameterValue("rowSpan"));
                            }
                            if (formField.HasParameterValue("colSpan"))
                            {
                                colSpan = Convert.ToInt32(formField.GetParameterValue("colSpan"));
                            }
                            layoutManager.AddControl(new GridLayoutManager.ControlInfo(inputControl, 1 + (NumberOfControlsInColumn * (colSpan-1)), rowSpan));
                            layoutManager.AddControl(Factory.CreatePanel());
                        }
                        else
                        {
                            for (int i = 0; i < NumberOfControlsInColumn; i++) layoutManager.AddControl(null);
                        }
                        columnPos[col]++;
                        col++;
                    }
                }
            }

            private GridLayoutManager SetupLayoutManager(UIFormTab formTab, IPanel panel)
            {
                GridLayoutManager layoutManager = new GridLayoutManager(panel, Factory);
                int maxRowsInColumns = formTab.GetMaxRowsInColumns();
                int colCount = formTab.Count * NumberOfControlsInColumn;
                layoutManager.SetGridSize(maxRowsInColumns, colCount);
                layoutManager.FixColumnBasedOnContents(0);
                for (int i = 0; i < colCount; i += NumberOfControlsInColumn)
                {
                    layoutManager.FixColumnBasedOnContents(i + LabelControlPosition);
                    layoutManager.FixColumn(i + ErrorProviderPosition, DefaultErrorProviderWidth);
                }
                return layoutManager;
            }
        }
    }
}


