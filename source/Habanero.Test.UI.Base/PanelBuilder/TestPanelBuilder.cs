using System;
using Habanero.Base;
using Habanero.BO;
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
            IPanel panel = panelBuilder.BuildPanel(singleFieldTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(singleIntegerFieldTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(singleIntegerFieldTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(twoFieldTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(twoFieldTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab).Panel;
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
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab).Panel;

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
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab).Panel;

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
            IPanel panel = panelBuilder.BuildPanel(twoColumnTab).Panel;

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
            IPanel panel = panelBuilder.BuildPanel(threeColumnTab).Panel;

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
            IPanel panel = panelBuilder.BuildPanel(singleIntegerFieldTab).Panel;

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
        public void Test_BuildPanel_RowSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab formTab = interfaceMapper.GetFormTabOneColumnThreeRowsWithRowSpan();

            UIFormColumn column1 = formTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(formTab).Panel;
            //---------------Test Result -----------------------

            IControlHabanero row1InputControl = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            ITextBox row2InputControl =
                (ITextBox) panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];

            Assert.IsTrue(row2InputControl.Multiline);
            Assert.AreEqual(row1InputControl.Height*2 + LayoutManager.DefaultGapSize, row2InputControl.Height);
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
            IPanel panel = panelBuilder.BuildPanel(formTab).Panel;
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
        public void Test_BuildPanel_CompulsoryFieldsAreBoldAndStarred()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneCompulsory();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(twoFieldTabOneCompulsory).Panel;
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
        public void Test_BuildPanel_CreatesControlMappers()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanel(twoFieldTabOneCompulsory);

            //---------------Test Result -----------------------
            IControlMapperCollection mappers = panelInfo.ControlMappers;
            Assert.AreEqual(2, mappers.Count);

            IControlHabanero sampleTextInputControl = panelInfo.Panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlHabanero sampleIntInputControl = panelInfo.Panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + PanelBuilder.INPUT_CONTROL_COLUMN_NO];

            IControlMapper sampleTextControlMapper = mappers[0];
            Assert.AreEqual("SampleText", sampleTextControlMapper.PropertyName);
            Assert.AreSame(sampleTextInputControl, sampleTextControlMapper.Control);
            Assert.IsInstanceOfType(typeof(TextBoxMapper), sampleTextControlMapper);

            IControlMapper sampleIntControlMapper = mappers[1];
            Assert.AreEqual("SampleInt", sampleIntControlMapper.PropertyName);
            Assert.AreSame(sampleIntInputControl, sampleIntControlMapper.Control);
            Assert.IsInstanceOfType(typeof(NumericUpDownIntegerMapper), sampleIntControlMapper);
        }

        [Test]
        public void Test_BuildPanel_PopulatesFieldInfoCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanel(twoFieldTabOneCompulsory);

            //---------------Test Result -----------------------

            IControlHabanero sampleTextLabel = panelInfo.Panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            IControlHabanero sampleTextInputControl = panelInfo.Panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlMapper sampleTextControlMapper = panelInfo.ControlMappers["SampleText"];

            Assert.AreEqual(2, panelInfo.FieldInfos.Count);

            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["SampleText"];
            Assert.AreSame(sampleTextLabel, fieldInfo.Label);
            Assert.AreSame(sampleTextInputControl, fieldInfo.InputControl);
            Assert.AreSame(sampleTextControlMapper, fieldInfo.ControlMapper);
        }

        //todo: ErrorProvider stuff


        [Test]
        public void Test_BuildPanel_LayoutManagerIsSet()
        {
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanel(twoFieldTabOneCompulsory);

            //---------------Test Result -----------------------
            GridLayoutManager layoutManager = panelInfo.LayoutManager;
            Assert.IsNotNull(layoutManager);
            Assert.AreEqual(2, layoutManager.Rows.Count);
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

        [Test]
        public void Test_BusinessObjectPassedToPanelInfo()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------

        }

        internal class PanelBuilder
        {
            private IControlFactory _factory;
            public const int ERROR_PROVIDER_WIDTH = 20;
            public const int CONTROLS_PER_COLUMN = 3;
            public const int LABEL_CONTROL_COLUMN_NO = 0;
            public const int INPUT_CONTROL_COLUMN_NO = 1;
            public const int ERROR_PROVIDER_COLUMN_NO = CONTROLS_PER_COLUMN - 1;


            public PanelBuilder(IControlFactory factory)
            {
                _factory = factory;
            }

            public IControlFactory Factory
            {
                get { return _factory; }
                set { _factory = value; }
            }

            public IPanelInfo BuildPanel(UIFormTab formTab)
            {
                IPanel panel = Factory.CreatePanel();
                IPanelInfo panelInfo = new PanelInfo();
                GridLayoutManager layoutManager = panelInfo.LayoutManager = SetupLayoutManager(formTab, panel);
                AddFieldsToLayoutManager(formTab, panelInfo);
                SetupInputControlColumnWidth(panelInfo, formTab);

                panel.Width = layoutManager.GetFixedWidthIncludingGaps();
                
                panelInfo.Panel = panel;
                return panelInfo;
            }

            private GridLayoutManager SetupLayoutManager(UIFormTab formTab, IPanel panel)
            {
                GridLayoutManager layoutManager = new GridLayoutManager(panel, Factory);
                int maxRowsInColumns = formTab.GetMaxRowsInColumns();
                int colCount = formTab.Count*CONTROLS_PER_COLUMN;
                layoutManager.SetGridSize(maxRowsInColumns, colCount);
                layoutManager.FixColumnBasedOnContents(0);
                for (int i = 0; i < colCount; i += CONTROLS_PER_COLUMN)
                {
                    layoutManager.FixColumnBasedOnContents(i + LABEL_CONTROL_COLUMN_NO);
                    layoutManager.FixColumn(i + ERROR_PROVIDER_COLUMN_NO, ERROR_PROVIDER_WIDTH);
                }
                ITextBox sampleTextBoxForHeight = Factory.CreateTextBox();
                for (int row = 0; row < maxRowsInColumns; row++)
                {
                    layoutManager.FixRow(row, sampleTextBoxForHeight.Height);
                }
                return layoutManager;
            }

            private void AddFieldsToLayoutManager(UIFormTab formTab, IPanelInfo panelInfo)
            {
                int numberOfColumns = formTab.Count;
                int[] currentFieldPositionInColumns = new int[numberOfColumns];
                int[] rowSpanTrackerForColumn = new int[numberOfColumns];
                for (int currentRowNo = 0; currentRowNo < formTab.GetMaxRowsInColumns(); currentRowNo++)
                {
                    int columnSpanTracker = 0;
                    for (int currentColumnNo = 0; currentColumnNo < numberOfColumns; currentColumnNo++)
                    {
                        UIFormColumn currentFormColumn = formTab[currentColumnNo];

                        if (--rowSpanTrackerForColumn[currentColumnNo] > 0) continue;  // keep skipping this grid position until a previous row span in this column has been decremented 
                        if (--columnSpanTracker > 0) continue;  // keep skipping this grid position until a previous column span in this row has been decremented

                        int currentFieldNoInColumn = currentFieldPositionInColumns[currentColumnNo];
                        int totalFieldsInColumn = currentFormColumn.Count;
                        if (currentFieldNoInColumn < totalFieldsInColumn) // there exists a field in this row in this column
                        {
                            UIFormField formField = currentFormColumn[currentFieldNoInColumn];
                            rowSpanTrackerForColumn[currentColumnNo] = formField.RowSpan;
                            columnSpanTracker = formField.ColSpan;

                            AddControlsForField(formField, panelInfo);
                        }
                        else
                        {
                            AddNullControlsForEmptyField(panelInfo);
                        }
                        currentFieldPositionInColumns[currentColumnNo]++;
                    }
                }
            }

            private void AddControlsForField(UIFormField formField, IPanelInfo panelInfo)
            {
                ILabel label = CreateAndAddLabel(panelInfo, formField);
                IControlMapper controlMapper = CreateAndAddInputControl(panelInfo, formField);
                IPanel errorProviderPanel = CreateAndAddErrorProviderPanel(panelInfo, formField);

                panelInfo.FieldInfos.Add(new PanelInfo.FieldInfo(formField.PropertyName, label, controlMapper, null));
            }

            private void AddNullControlsForEmptyField(IPanelInfo panelInfo)
            {
                for (int i = 0; i < CONTROLS_PER_COLUMN; i++)
                    panelInfo.LayoutManager.AddControl(null);
            }

            private IPanel CreateAndAddErrorProviderPanel(IPanelInfo panelInfo, UIFormField formField)
            {
                IPanel errorProviderPanel = Factory.CreatePanel();
                panelInfo.LayoutManager.AddControl(errorProviderPanel);
                return errorProviderPanel;
            }

            private IControlMapper CreateAndAddInputControl(IPanelInfo panelInfo, UIFormField formField)
            {
                IControlHabanero inputControl = Factory.CreateControl(formField.ControlTypeName,
                                                                      formField.ControlAssemblyName);
                IControlMapper controlMapper = ControlMapper.Create(formField.MapperTypeName,
                                                                    formField.MapperAssembly, inputControl,
                                                                    formField.PropertyName, formField.Editable, _factory);
                panelInfo.ControlMappers.Add(controlMapper);
                            
                if (formField.RowSpan > 1)
                {
                    if (inputControl is ITextBox) ((ITextBox) inputControl).Multiline = true;
                }
                int numberOfGridColumnsToSpan = 1 + (CONTROLS_PER_COLUMN*(formField.ColSpan - 1));
                GridLayoutManager.ControlInfo inputControlInfo =
                    new GridLayoutManager.ControlInfo(inputControl, numberOfGridColumnsToSpan,
                                                      formField.RowSpan);
                SetToolTip(formField, inputControl);
                panelInfo.LayoutManager.AddControl(inputControlInfo);
                return controlMapper;
            }

            private ILabel CreateAndAddLabel(IPanelInfo panelInfo, UIFormField formField)
            {
                ILabel labelControl = Factory.CreateLabel(formField.GetLabel(), formField.IsCompulsory);
                labelControl.Name = formField.PropertyName;
                SetToolTip(formField, labelControl);
                panelInfo.LayoutManager.AddControl(labelControl);
                return labelControl;
            }

            private void SetupInputControlColumnWidth(IPanelInfo panelInfo, UIFormTab formTab)
            {
                GridLayoutManager layoutManager = panelInfo.LayoutManager;
                int formColCount = 0;
                foreach (UIFormColumn formColumn in formTab)
                {
                    if (formColumn.Width < 0) continue;
                    int gridCol = formColCount*CONTROLS_PER_COLUMN;
                    int labelColumnWidth = layoutManager.GetFixedColumnWidth(gridCol + LABEL_CONTROL_COLUMN_NO);
                    int errorProviderColumnWidth = layoutManager.GetFixedColumnWidth(gridCol + ERROR_PROVIDER_COLUMN_NO);
                    int totalGap = (CONTROLS_PER_COLUMN - 1)*layoutManager.GapSize;
                    if (formTab.Count == 1)
                        totalGap += 2*layoutManager.BorderSize; // add extra border for single column
                    else if (formColCount == formTab.Count - 1)
                        totalGap += layoutManager.BorderSize + layoutManager.GapSize; // last column in multi-column
                    else if (formColCount > 0 && formTab.Count > 0)
                        totalGap += layoutManager.GapSize; //2 More gaps for internal column in multi-column
                    else if (formColCount == 0 && formTab.Count > 0) totalGap += layoutManager.BorderSize;

                    layoutManager.FixColumn(gridCol + INPUT_CONTROL_COLUMN_NO,
                                            formColumn.Width - labelColumnWidth - errorProviderColumnWidth - totalGap);
                    formColCount++;
                }
            }

            private void SetToolTip(UIFormField formField, IControlHabanero inputControl)
            {
                string toolTipText = formField.GetToolTipText();
                IToolTip toolTip = _factory.CreateToolTip();
                if (!String.IsNullOrEmpty(toolTipText))
                {
                    toolTip.SetToolTip(inputControl, toolTipText);
                }
            }

        }
    }
}