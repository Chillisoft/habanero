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
    public  class TestPanelBuilder
    {
      
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
        public void TestCreatePanel()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDef = sampleUserInterfaceMapperWin.GetSimpleUIFormDef();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(ITextBox), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof(IPanel), panel.Controls[2]);

        }

        [Test]
        public void TestCreatePanel_WithIntUiDef()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefInt = sampleUserInterfaceMapperWin.GetSimpleUIFormDefInt();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefInt);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof(INumericUpDown), panel.Controls[1]);
            Assert.IsInstanceOfType(typeof(IPanel), panel.Controls[2]);
        }

        [Test]
        public void TestControlLayout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefInt = sampleUserInterfaceMapperWin.GetSimpleUIFormDefInt();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefInt);
            //---------------Test Result -----------------------
            IControlHabanero control = panel.Controls[0];
            Assert.AreEqual(5,control.Left);
            Assert.AreEqual(62, control.Width);
            Assert.AreEqual(90, control.Height);

            IControlHabanero control1 = panel.Controls[1];
            int gap = 2;
            Assert.AreEqual(control.Right+gap, control1.Left);
            Assert.AreEqual(62, control1.Width);
            Assert.AreEqual(20,control1.Height);

            IControlHabanero control2 = panel.Controls[2];
            Assert.AreEqual(control1.Right + gap, control2.Left);
            Assert.AreEqual(62, control2.Width);
            Assert.AreEqual(90, control2.Height);

        }

        [Test]
        public void Test_LabelTextIsCorrect()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefInt = sampleUserInterfaceMapperWin.GetSimpleUIFormDefInt();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefInt);
            //---------------Test Result -----------------------
            IControlHabanero control = panel.Controls[0];
            ILabel label = (ILabel) control;
            Assert.AreEqual("Integer:", label.Text);

        }

        [Test]
        public void Test_TwoRowsOfControls()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefTwoRows = sampleUserInterfaceMapperWin.GetSimpleUIFormDefTwoRows();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefTwoRows);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, panel.Controls.Count);
            
            //-- Row 1
            Assert.IsInstanceOfType(typeof(ILabel), panel.Controls[0]);
            ILabel row1Label = (ILabel) panel.Controls[0];
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
        public void Test_TwoColumnsDef()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefTwoColumns = sampleUserInterfaceMapperWin.GetSimpleUIFormDef1Row2Columns();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefTwoColumns);
            //---------------Test Result -----------------------
            Assert.AreEqual(6,panel.Controls.Count);
        }

        [Test]
        public void Test_TwoColumns_TwoRows_DifferentNumberOfControls()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefTwoColumns = sampleUserInterfaceMapperWin.GetSimpleUIFormDef2Row2Columns1RowWithMoreControls();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefTwoColumns);
            //---------------Test Result -----------------------
            Assert.AreEqual(12, panel.Controls.Count);
        }

        [Test]
        public void Test_TwoColumns_TwoRows_DifferentNumberOfControls_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
            UIForm simpleUIDefTwoColumns = sampleUserInterfaceMapperWin.GetSimpleUIFormDef2Row2Columns1RowWithMoreControls();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanel(simpleUIDefTwoColumns);
            //---------------Test Result -----------------------
            IControlHabanero control = panel.Controls[0];
            Assert.AreEqual(5, control.Left);
          

            IControlHabanero control1 = panel.Controls[1];
            int gap = 2;
            Assert.AreEqual(control.Right + gap, control1.Left);
    

            IControlHabanero control2 = panel.Controls[2];
            Assert.AreEqual(control1.Right + gap, control2.Left);
          

            IControlHabanero control3 = panel.Controls[3];
            Assert.IsInstanceOfType(typeof(IControlHabanero), control3);
      

            IControlHabanero control4 = panel.Controls[4];
            Assert.IsInstanceOfType(typeof(IControlHabanero), control4);
       

            IControlHabanero control5 = panel.Controls[5];
            Assert.IsInstanceOfType(typeof(IControlHabanero), control5);
   

            IControlHabanero control6 = panel.Controls[6];
            Assert.AreEqual(5, control6.Left);
          

            IControlHabanero control7 = panel.Controls[7];
            Assert.AreEqual(control6.Right + gap, control7.Left);
       

            IControlHabanero control8 = panel.Controls[8];
            Assert.AreEqual(control7.Right + gap, control8.Left);

            IControlHabanero control9 = panel.Controls[9];
            Assert.AreEqual(control8.Right + gap, control9.Left);


            IControlHabanero control10 = panel.Controls[10];
            Assert.AreEqual(control9.Right + gap, control10.Left);


            IControlHabanero control11 = panel.Controls[11];
            Assert.AreEqual(control10.Right + gap, control11.Left);
        

        }
    }


    internal class PanelBuilder
    {
        private IControlFactory _factory;

        public PanelBuilder (IControlFactory factory)
        {
       
            _factory = factory;
           
        }

        public IControlFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public IPanel BuildPanel(UIForm form)
        {
            IPanel panel = Factory.CreatePanel();
            GridLayoutManager layoutManager = new GridLayoutManager(panel,Factory);
            int rowCount=0;
            rowCount = GetRowCount(form, rowCount);
            int columns = form[0].Count;
            int cols = rowCount * 3;
            GridLayoutManager.ControlInfo[,] controls = new GridLayoutManager.ControlInfo[columns, cols];
            layoutManager.SetGridSize(columns, cols);
            GetControls(form, controls);
            AddControlsToLayoutManager(columns, cols, layoutManager, controls);
            return panel;
        }

        private void AddControlsToLayoutManager(int rowCount, int cols, GridLayoutManager layoutManager, GridLayoutManager.ControlInfo[,] controls)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (controls[i, j] == null)
                    {
                        layoutManager.AddControl(null);
                    }
                    else
                    {
                        layoutManager.AddControl(controls[i, j].Control, controls[i, j].ColumnSpan, controls[i, j].RowSpan);
                        controls[i, j].Control.TabIndex = rowCount * j + i;
                    }
                }
            }
        }

        private void GetControls(UIForm form, GridLayoutManager.ControlInfo[,] controls)
        {
            foreach (UIFormTab formTab in form)
            {
                int currentRow = 0;
                foreach (UIFormColumn formColumn in formTab)
                {
                    int currentColumn = 0;
                    foreach (UIFormField formField in formColumn)
                    {
                        ILabel label = Factory.CreateLabel(formField.GetLabel());
                        controls[currentRow, currentColumn + 0] = new GridLayoutManager.ControlInfo(label);
                        IControlHabanero controlHabanero = Factory.CreateControl(formField.ControlTypeName, formField.ControlAssemblyName);
                        controls[currentRow, currentColumn + 1] = new GridLayoutManager.ControlInfo(controlHabanero);
                        controls[currentRow, currentColumn + 2] = new GridLayoutManager.ControlInfo(Factory.CreatePanel());
                        currentColumn += 3;
                    }
                    currentRow++;
                }
            }
        }

        private int GetRowCount(UIForm form, int rowCount)
        {
            foreach (UIFormTab tab in form)
            {
                foreach (UIFormColumn column in tab)
                {

                    if (column.Count > rowCount)
                        rowCount = column.Count;
                }
            }
            return rowCount;
        }
    }
}


