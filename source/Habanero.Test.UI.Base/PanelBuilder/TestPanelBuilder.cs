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
        public void Test_ColumnWidthSetCorrectly_TwoColumns()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();

            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            Assert.AreEqual(1, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);

            //---------------Test Result -----------------------
            Assert.AreEqual(150,panel.Width);

        }

        [Test, Ignore("Need to work out how the widths work.")]
        public void Test_ControlWidthSetCorrectlyBasedOnColumnWidth()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();

            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            Assert.AreEqual(1, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);

            //---------------Test Result -----------------------
            IControlCollection controls = panel.Controls;
            Assert.AreEqual(100 - (controls[0].Width + controls[2].Width + LayoutManager.DefaultBorderSize * 2 + LayoutManager.DefaultGapSize * 5), controls[1].Width);
            Assert.AreEqual(50 - (controls[3].Width+controls[5].Width + LayoutManager.DefaultBorderSize*2 + LayoutManager.DefaultGapSize * 2),controls[4].Width);

        }
        
        [Test]
        public void Test_ControlWidthSetCorrectlyBasedOnColumnWidthOneColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabOneColumnOneRowWithWidth();

            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab);

            //---------------Test Result -----------------------
            IControlCollection controls = panel.Controls;
            Assert.AreEqual(100 - (controls[0].Width + controls[2].Width + LayoutManager.DefaultGapSize + LayoutManager.DefaultGapSize+LayoutManager.DefaultBorderSize+LayoutManager.DefaultBorderSize), controls[1].Width);

        }
        internal class PanelBuilder
        {
            private IControlFactory _factory;
            public const int DefaultErrorProviderWidth = 20;
            public const int NumberOfControlsInColumn = 3;

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
                GridLayoutManager layoutManager = new GridLayoutManager(panel, Factory);
                int rowCount =  GetRowCount(formTab);
                int colCount = formTab.Count * NumberOfControlsInColumn;
                GridLayoutManager.ControlInfo[,] controls =
                    new GridLayoutManager.ControlInfo[rowCount, colCount];
                layoutManager.SetGridSize(rowCount, colCount);
                GetControls(formTab, controls);
                int width = 0;
                foreach (UIFormColumn formColumn in formTab)
                {
                    width += formColumn.Width;
                }
                panel.Width = width;
                layoutManager.FixColumnBasedOnContents(0);
                
                for (int i = 2; i < colCount; i+=3)
                {
                    layoutManager.FixColumnBasedOnContents(i-2);
                    layoutManager.FixColumn(i,DefaultErrorProviderWidth);
                }
                AddControlsToLayoutManager(rowCount, colCount, layoutManager, controls);
                return panel;
            }

            private void AddControlsToLayoutManager(int rowCount, int cols, GridLayoutManager layoutManager, GridLayoutManager.ControlInfo[,] controls)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    layoutManager.FixRow(i, Factory.CreateTextBox().Height);

                    for (int j = 0; j < cols; j++)
                    {
                        if (controls[i, j] == null)
                        {
                            layoutManager.AddControl(null);
                        }
                        else
                        {
                            layoutManager.AddControl(controls[i, j]);
                            //controls[i, j].Control.TabIndex = rowCount * j + i;
                        }
                    }
                }
            }

            private void GetControls(UIFormTab formTab, GridLayoutManager.ControlInfo[,] controls)
            {
                int currentColumn = 0;
                int currentRow = 0;
                foreach (UIFormColumn formColumn in formTab)
                {
                    currentRow = 0;
                    foreach (UIFormField formField in formColumn)
                    {
                        ILabel label = Factory.CreateLabel(formField.GetLabel());
                        controls[currentRow, currentColumn + 0] = new GridLayoutManager.ControlInfo(label);
                        IControlHabanero controlHabanero = Factory.CreateControl(formField.ControlTypeName, formField.ControlAssemblyName);
                        controlHabanero.Width = formColumn.Width - label.Width - DefaultErrorProviderWidth;
                        controls[currentRow, currentColumn + 1] = new GridLayoutManager.ControlInfo(controlHabanero);
                        controls[currentRow, currentColumn + 2] = new GridLayoutManager.ControlInfo(Factory.CreatePanel());
                        controls[currentRow, currentColumn + 2].Control.Width = DefaultErrorProviderWidth;
                        currentRow++;
                    }
                    currentColumn += NumberOfControlsInColumn;
                    
                }
            }

            private int GetRowCount(UIFormTab formTab)
            {
                int rowCount = 0;
                foreach (UIFormColumn column in formTab)
                {

                    if (column.Count > rowCount)
                        rowCount = column.Count;
                }
                return rowCount;
            }
        }
    }
}


