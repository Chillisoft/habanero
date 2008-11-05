using System;
using System.Collections.Generic;
using System.Text;
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

        //    [Test]
        //    public void Test_TwoColumns_TwoRows_DifferentNumberOfControls_Layout()
        //    {
        //        //---------------Set up test pack-------------------
        //        Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
        //        UIForm simpleUIDefTwoColumns = sampleUserInterfaceMapperWin.GetSimpleUIFormDef2Row2Columns1RowWithMoreControls();
        //        PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
        //        //---------------Assert Precondition----------------

        //        //---------------Execute Test ----------------------
        //        IPanel panel = panelBuilder.BuildPanel(simpleUIDefTwoColumns);
        //        //---------------Test Result -----------------------
        //        IControlHabanero control = panel.Controls[0];
        //        Assert.AreEqual(5, control.Left);


        //        IControlHabanero control1 = panel.Controls[1];
        //        int gap = 2;
        //        Assert.AreEqual(control.Right + gap, control1.Left);


        //        IControlHabanero control2 = panel.Controls[2];
        //        Assert.AreEqual(control1.Right + gap, control2.Left);


        //        IControlHabanero control3 = panel.Controls[3];
        //        Assert.IsInstanceOfType(typeof(IControlHabanero), control3);


        //        IControlHabanero control4 = panel.Controls[4];
        //        Assert.IsInstanceOfType(typeof(IControlHabanero), control4);


        //        IControlHabanero control5 = panel.Controls[5];
        //        Assert.IsInstanceOfType(typeof(IControlHabanero), control5);


        //        IControlHabanero control6 = panel.Controls[6];
        //        Assert.AreEqual(5, control6.Left);


        //        IControlHabanero control7 = panel.Controls[7];
        //        Assert.AreEqual(control6.Right + gap, control7.Left);


        //        IControlHabanero control8 = panel.Controls[8];
        //        Assert.AreEqual(control7.Right + gap, control8.Left);

        //        IControlHabanero control9 = panel.Controls[9];
        //        Assert.AreEqual(control8.Right + gap, control9.Left);


        //        IControlHabanero control10 = panel.Controls[10];
        //        Assert.AreEqual(control9.Right + gap, control10.Left);


        //        IControlHabanero control11 = panel.Controls[11];
        //        Assert.AreEqual(control10.Right + gap, control11.Left);


        //    }
        //}


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
                panel.Width = formTab[0].Width;
                layoutManager.FixColumnBasedOnContents(0);
                layoutManager.FixColumn(2, DefaultErrorProviderWidth);
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
                foreach (UIFormColumn formColumn in formTab)
                {
                    int currentRow = 0;
                    foreach (UIFormField formField in formColumn)
                    {
                        ILabel label = Factory.CreateLabel(formField.GetLabel());
                        controls[currentRow, currentColumn + 0] = new GridLayoutManager.ControlInfo(label);
                        IControlHabanero controlHabanero = Factory.CreateControl(formField.ControlTypeName, formField.ControlAssemblyName);
                        controls[currentRow, currentColumn + 1] = new GridLayoutManager.ControlInfo(controlHabanero);
                        controls[currentRow, currentColumn + 2] = new GridLayoutManager.ControlInfo(Factory.CreatePanel());
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


