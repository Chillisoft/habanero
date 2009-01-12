using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestPanelBuilder
    {
        private const int DEFAULT_CONTROLS_PER_FIELD = 3;

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }
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
            Assert.AreEqual(typeof (ControlFactoryWin), panelBuilder.Factory.GetType());
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
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
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
        public void Test_BuildPanel_1Field_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
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
            Assert.AreEqual(column.Width - LayoutManager.DefaultBorderSize, errorProviderPanel.Right);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, errorProviderPanel.Width);

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
            Assert.IsInstanceOfType(typeof (NumericUpDownWin), panel.Controls[4]);
            Assert.IsInstanceOfType(typeof (IPanel), panel.Controls[5]);
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
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedColumns*expectedFieldsInEachColumn, panel.Controls.Count);
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
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedColumns*maxFieldsInAColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanel_2Columns_1_2_CorrectLayout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
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
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
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
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

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
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

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
            IPanel panel = panelBuilder.BuildPanelForTab(threeColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD*2 - 1];
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
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;

            int columnWidthOrig = 300;
            int columnWidthAfter = 500;
            panel.Width = columnWidthOrig;
            ILabel label = (ILabel) panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];
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
        public void Test_BuildPanel_3Columns_1Column_RowSpan2()
        {
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabThreeColumnsOneColumnWithRowSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IControlHabanero textBoxCol1 = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlHabanero col1Text1RowSpan2Label = panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            IControlHabanero textBoxCol2 = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            IControlHabanero nullControl = panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN * 3];
            ILabel col2TextBox2Label = (ILabel)panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN * 4];
            
            Assert.IsNotInstanceOfType(typeof(ILabel),nullControl);
            Assert.AreEqual(textBoxCol2.Height * 2 + LayoutManager.DefaultGapSize, textBoxCol1.Height);
            Assert.AreEqual(col1Text1RowSpan2Label.Left, nullControl.Left);

            Assert.IsInstanceOfType(typeof(LabelWin), col2TextBox2Label);
            Assert.AreEqual("Col2TextBox2",col2TextBox2Label.Text);
            Assert.AreEqual(textBoxCol1.Right+PanelBuilder.ERROR_PROVIDER_WIDTH+GridLayoutManager.DefaultGapSize*2,col2TextBox2Label.Left);
        }

        [Test]
        public void Test_BuildPanel_Parameter_RowSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
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
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
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
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithNoAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsNull(singleFieldTab[0][0].Alignment);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------
           
            Assert.IsInstanceOfType(typeof(ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Left, control.TextAlign);
        }

        [Test]
        public void Test_BuildPanel_Parameter_SetAlignment_Right()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithRightAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual("right", singleFieldTab[0][0].Alignment);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOfType(typeof(ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox)panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Right, control.TextAlign);
        }

        // TODO:
        //   - build test subclass to test VWG
        //   - extract method that converts parameter alignment strings into HorizontalAlignment and write tests for all cases (left/right/centre/center/upper-lower)
        //   - test that alignment gets converted correctly for vwg controls
        //   - add tests in places like TestTextBoxVWG to prove that TextAlign gets converted correctly (look at all controls that have TextAlign)

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
            Assert.IsInstanceOfType(typeof(ITabControl), panelInfo.Panel.Controls[0]);
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
            ITabControl tabControl = (ITabControl)panelInfo.Panel.Controls[0];
            ITabPage tabPage1 = tabControl.TabPages[0];
            ITabPage tabPage2 = tabControl.TabPages[1];
            Assert.AreEqual(1, tabPage1.Controls.Count);
            Assert.IsInstanceOfType(typeof(IPanel), tabPage1.Controls[0]);
            IPanel tabPage1Panel = (IPanel) tabPage1.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage1Panel.Controls.Count);
            Assert.AreEqual(1, tabPage2.Controls.Count);
            Assert.IsInstanceOfType(typeof(IPanel), tabPage2.Controls[0]);
            IPanel tabPage2Panel = (IPanel)tabPage2.Controls[0];
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
            Assert.AreEqual(panelInfo.FieldInfos.Count, panelInfo.PanelInfos[0].FieldInfos.Count + panelInfo.PanelInfos[1].FieldInfos.Count);

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
           Assert.AreEqual(6,panelInfo.Panel.Controls.Count);
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
}