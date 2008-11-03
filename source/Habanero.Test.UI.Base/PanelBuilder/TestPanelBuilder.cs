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
            Sample.SampleUserInterfaceMapperWin sampleUserInterfaceMapperWin = new Sample.SampleUserInterfaceMapperWin();
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
            Assert.AreEqual(9, panel.Controls.Count);

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
            layoutManager.SetGridSize(rowCount, form[0].Count * 3);
            foreach (UIFormTab formTab in form)
            {
                foreach (UIFormColumn formColumn in formTab)
                {
                    foreach (UIFormField formField in formColumn)
                    {
                        ILabel label = Factory.CreateLabel(formField.GetLabel());
                        layoutManager.AddControl(label);
                        IControlHabanero controlHabanero = Factory.CreateControl(formField.ControlTypeName, formField.ControlAssemblyName);
                        layoutManager.AddControl(controlHabanero);
                        layoutManager.AddControl(Factory.CreatePanel());
                    }
                }
            }
            return panel;
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


