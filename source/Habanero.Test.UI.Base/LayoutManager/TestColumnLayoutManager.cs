//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System.Drawing;
using Habanero.UI.Base;
using NUnit.Framework;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.Test.UI.Base
{
    public abstract class TestColumnLayoutManager
    {
        private const int GAP_SIZE = 2;
        private const int BORDER_SIZE = 5;

        protected abstract IControlFactory GetControlFactory();

        //[TestFixture]
        //public class TestColumnLayoutManagerWin : TestColumnLayoutManager
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new Habanero.UI.Win.ControlFactoryWin();
        //    }
        //}

        [TestFixture]
        public class TestColumnLayoutManagerVWG : TestColumnLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.VWG.ControlFactoryVWG();
            }
        }

        [TestFixture]
        public class TestColumnLayoutManagerWin : TestColumnLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            private static IPanel CreateColoredPanel(IControlFactory controlFactory, string labelPrefix)
            {
                System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel) controlFactory.CreatePanel();
                ILabel label = controlFactory.CreateLabel(labelPrefix + "Left-Click to grow, Right-Click to shrink.");
                label.Height = 15;
                label.BackColor = Color.White;
                panel.Controls.Add((System.Windows.Forms.Control)label);
                panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                panel.MouseDown += (sender, e) =>
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left) panel.Height += 5;
                    if (e.Button == System.Windows.Forms.MouseButtons.Right) panel.Height -= 5;
                };
                panel.BackColor = Color.FromArgb(TestUtil.GetRandomInt(255), TestUtil.GetRandomInt(255), TestUtil.GetRandomInt(255));
                return (IPanel) panel;
            }

            [Test]
            [Ignore("This is for visual testing purposes")]
            public void Test_Visually()
            {
                //---------------Set up test pack-------------------
                IControlFactory controlFactory = GetControlFactory();
                IGroupBox groupBox = controlFactory.CreateGroupBox("Test Layout");
                IPanel panel = controlFactory.CreatePanel();
                panel.Dock = DockStyle.Fill;
                groupBox.Controls.Add(panel);
                ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(panel, controlFactory) { ColumnCount = 2 };
                int controlNumber = 1;
                columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
                columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
                columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
                IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
                buttonGroupControl.Dock = DockStyle.Top;
                groupBox.Controls.Add(buttonGroupControl);
                buttonGroupControl.AddButton("Add Control", (sender,e) => columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":")));
                buttonGroupControl.AddButton("-Columns", (sender,e) =>
                {
                    if (columnLayoutManager.ColumnCount > 1)
                    {
                        columnLayoutManager.ColumnCount--;
                        columnLayoutManager.Refresh();
                    }
                });
                buttonGroupControl.AddButton("+Columns", (sender, e) => { columnLayoutManager.ColumnCount++; columnLayoutManager.Refresh();});
                IFormHabanero form = controlFactory.CreateOKCancelDialogFactory().CreateOKCancelForm(groupBox, "Test Column Layout Manager");
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                form.ShowDialog();
                //---------------Test Result -----------------------
            }

            [Test]
            [Ignore("This is for visual testing purposes")]
            public void Test_Visually_Advanced()
            {
                //---------------Set up test pack-------------------
                IControlFactory controlFactory = GetControlFactory();
                IGroupBox groupBox = controlFactory.CreateGroupBox("Test Layout");
                IPanel panel = controlFactory.CreatePanel();
                panel.Dock = DockStyle.Fill;
                groupBox.Controls.Add(panel);
                ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(panel, controlFactory) { ColumnCount = 1 };
                int controlNumber = 1;
                IPanel panel1 = CreateColoredPanel(controlFactory, controlNumber++ + ":");
                panel1.Controls.Clear();
                GridLayoutManager gridLayoutManager = new GridLayoutManager(panel1, controlFactory);
                gridLayoutManager.SetGridSize(4,3);
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "a:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "b:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "c:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "d:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "e:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "f:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "g:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "h:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "i:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "j:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "k:"));
                gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "l:"));


                columnLayoutManager.AddControl(panel1);
                columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
                IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
                buttonGroupControl.Dock = DockStyle.Top;
                groupBox.Controls.Add(buttonGroupControl);
                buttonGroupControl.AddButton("Add Control", (sender,e) => columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":")));
                buttonGroupControl.AddButton("-Columns", (sender,e) =>
                {
                    if (columnLayoutManager.ColumnCount > 1)
                    {
                        columnLayoutManager.ColumnCount--;
                        columnLayoutManager.Refresh();
                    }
                });
                buttonGroupControl.AddButton("+Columns", (sender, e) => { columnLayoutManager.ColumnCount++; columnLayoutManager.Refresh();});
                IFormHabanero form = controlFactory.CreateOKCancelDialogFactory().CreateOKCancelForm(groupBox, "Test Column Layout Manager");
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                form.ShowDialog();
                //---------------Test Result -----------------------
            }
        }

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void TestCreateColumnLayoutManager()
        {
            //---------------Set up test pack-------------------
            IControlHabanero controlHabanero = GetControlFactory().CreatePanel();
            //---------------Execute Test ----------------------
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(controlHabanero, GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsNotNull(columnLayoutManager.ManagedControl);
            Assert.AreSame(controlHabanero, columnLayoutManager.ManagedControl);
            Assert.AreEqual(1, columnLayoutManager.ColumnCount);
            //---------------Tear Down   -----------------------
        }

        [Test]
        public void TestAddControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreatePanel();
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(managedControl, GetControlFactory());
            //---------------Execute Test ----------------------
            IControlHabanero createdControl = GetControlFactory().CreateControl();
            IControlHabanero addedControl = columnLayoutManager.AddControl(createdControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, managedControl.Controls.Count);
            Assert.AreSame(addedControl, createdControl);
        }

        [Test]
        public void TestRemoveControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreatePanel();
            IControlHabanero createdControl = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(managedControl, GetControlFactory());
            columnLayoutManager.AddControl(createdControl);
            //---------------Assert Pre-Conditions--------------
            Assert.AreEqual(1, managedControl.Controls.Count);
            //---------------Execute Test ----------------------
            columnLayoutManager.RemoveControl(createdControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, managedControl.Controls.Count);
        }


        private ColumnLayoutManager GetColumnLayoutManager()
        {
            IControlHabanero controlHabanero = GetControlFactory().CreatePanel();
            //---------------Execute Test ----------------------
            return new ColumnLayoutManager(controlHabanero, GetControlFactory());
        }

        [Test]
        public void TestSetColumnCount()
        {
            //---------------Set up test pack-------------------
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, columnLayoutManager.ColumnCount);
            //---------------Execute Test ----------------------
            columnLayoutManager.ColumnCount = 2;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, columnLayoutManager.ColumnCount);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddControl_LayoutWithOneColumn()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            //managedControl.Width = 200;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, managedControl.Controls.Count);
            Assert.AreEqual(0, control.Left);
            Assert.AreEqual(0, control.Top);
            Assert.AreEqual(100, control.Width);
            Assert.AreEqual(10, control.Height);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, managedControl.Controls.Count);
            Assert.AreEqual(BORDER_SIZE, control.Left);
            Assert.AreEqual(BORDER_SIZE, control.Top);
            Assert.AreEqual(managedControl.Width - BORDER_SIZE*2, control.Width);
            Assert.AreEqual(10, control.Height);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_LayoutWithOneColumn_ChangeBorderSize()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            //managedControl.Width = 200;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, managedControl.Controls.Count);
            Assert.AreEqual(0, control.Left);
            Assert.AreEqual(0, control.Top);
            Assert.AreEqual(100, control.Width);
            Assert.AreEqual(10, control.Height);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control);
            columnLayoutManager.BorderSize = 10;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, managedControl.Controls.Count);
            Assert.AreEqual(columnLayoutManager.BorderSize, control.Left);
            Assert.AreEqual(columnLayoutManager.BorderSize, control.Top);
            Assert.AreEqual(managedControl.Width - columnLayoutManager.BorderSize*2, control.Width);
            Assert.AreEqual(10, control.Height);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddTwoControls_LayoutWithOneColumn()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control1 = GetControlFactory().CreateControl();
            IControlHabanero control2 = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            //managedControl.Width = 200;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, managedControl.Controls.Count);
            Assert.AreEqual(0, control1.Left);
            Assert.AreEqual(0, control1.Top);
            Assert.AreEqual(100, control1.Width);
            Assert.AreEqual(0, control2.Left);
            Assert.AreEqual(0, control2.Top);
            Assert.AreEqual(100, control2.Width);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control1);
            columnLayoutManager.AddControl(control2);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, managedControl.Controls.Count);
            Assert.AreEqual(columnLayoutManager.BorderSize, control1.Left);
            Assert.AreEqual(columnLayoutManager.BorderSize, control1.Top);
            Assert.AreEqual(managedControl.Width - columnLayoutManager.BorderSize*2, control1.Width);

            Assert.AreEqual(columnLayoutManager.BorderSize, control2.Left);
            Assert.AreEqual(columnLayoutManager.BorderSize + control1.Height +
                            columnLayoutManager.GapSize, control2.Top);
            Assert.AreEqual(managedControl.Width - columnLayoutManager.BorderSize*2, control2.Width);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddControl_TwoColumns_OneControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2*BORDER_SIZE - GAP_SIZE)/NoColumns;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, control.Left);
            Assert.AreEqual(0, control.Top);
            Assert.AreEqual(100, control.Width);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control.Left);
            Assert.AreEqual(BORDER_SIZE, control.Top);
            Assert.AreEqual(expectedControlWidth, control.Width);
            //---------------Tear Down -------------------------          
        }

        
        [Test]
        public void TestAddControl_TwoColumns_OneControl_NonIntegerColumnWidth()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            double expectedControlWidth = 42.5;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, control.Left);
            Assert.AreEqual(0, control.Top);
            Assert.AreEqual(100, control.Width);
            //---------------Execute Test ----------------------

            double combinedWidth = expectedControlWidth * 2 + BORDER_SIZE * 2 + GAP_SIZE;
            managedControl.Width = (int)combinedWidth; 
            columnLayoutManager.AddControl(control);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control.Left);
            Assert.AreEqual(BORDER_SIZE, control.Top);
            Assert.AreEqual((int) expectedControlWidth, control.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_ThreeColumns_OneControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 3;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2*BORDER_SIZE - 2*GAP_SIZE)/NoColumns;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, control.Left);
            Assert.AreEqual(0, control.Top);
            Assert.AreEqual(100, control.Width);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control.Left);
            Assert.AreEqual(BORDER_SIZE, control.Top);
            Assert.AreEqual(expectedControlWidth, control.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_TwoColumns_TwoControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control1 = GetControlFactory().CreateControl();
            IControlHabanero control2 = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2*BORDER_SIZE - 1*GAP_SIZE)/NoColumns;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, control1.Left);
            Assert.AreEqual(0, control1.Top);
            Assert.AreEqual(100, control1.Width);

            Assert.AreEqual(0, control2.Left);
            Assert.AreEqual(0, control2.Top);
            Assert.AreEqual(100, control2.Width);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control1);
            columnLayoutManager.AddControl(control2);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control1.Left);
            Assert.AreEqual(BORDER_SIZE, control1.Top);
            Assert.AreEqual(expectedControlWidth, control1.Width);


            Assert.AreEqual(expectedControlWidth + BORDER_SIZE + GAP_SIZE, control2.Left);
            Assert.AreEqual(BORDER_SIZE, control2.Top);
            Assert.AreEqual(expectedControlWidth, control2.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_TwoColumns_ThreeControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control1 = GetControlFactory().CreateControl();
            IControlHabanero control2 = GetControlFactory().CreateControl();
            IControlHabanero control3 = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2*BORDER_SIZE - 1*GAP_SIZE)/NoColumns;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, control1.Left);
            Assert.AreEqual(0, control1.Top);
            Assert.AreEqual(100, control1.Width);

            Assert.AreEqual(0, control2.Left);
            Assert.AreEqual(0, control2.Top);
            Assert.AreEqual(100, control2.Width);

            Assert.AreEqual(0, control3.Left);
            Assert.AreEqual(0, control3.Top);
            Assert.AreEqual(100, control3.Width);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control1);
            columnLayoutManager.AddControl(control2);
            columnLayoutManager.AddControl(control3);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control1.Left);
            Assert.AreEqual(BORDER_SIZE, control1.Top);
            Assert.AreEqual(expectedControlWidth, control1.Width);


            Assert.AreEqual(expectedControlWidth + BORDER_SIZE + GAP_SIZE, control2.Left);
            Assert.AreEqual(BORDER_SIZE, control2.Top);
            Assert.AreEqual(expectedControlWidth, control2.Width);

            Assert.AreEqual(BORDER_SIZE, control3.Left);
            Assert.AreEqual(BORDER_SIZE + control1.Height + GAP_SIZE, control3.Top);
            Assert.AreEqual(expectedControlWidth, control3.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_TwoColumns_ThreeControl_Control2DiffHeight()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control1 = GetControlFactory().CreateControl();
            IControlHabanero control2 = GetControlFactory().CreateControl();
            IControlHabanero control3 = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2*BORDER_SIZE - 1*GAP_SIZE)/NoColumns;
            control2.Height = 40;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, control1.Left);
            Assert.AreEqual(0, control1.Top);
            Assert.AreEqual(100, control1.Width);

            Assert.AreEqual(0, control2.Left);
            Assert.AreEqual(0, control2.Top);
            Assert.AreEqual(100, control2.Width);
            Assert.AreEqual(40, control2.Height);

            Assert.AreEqual(0, control3.Left);
            Assert.AreEqual(0, control3.Top);
            Assert.AreEqual(100, control3.Width);
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control1);
            columnLayoutManager.AddControl(control2);
            columnLayoutManager.AddControl(control3);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control1.Left);
            Assert.AreEqual(BORDER_SIZE, control1.Top);
            Assert.AreEqual(expectedControlWidth, control1.Width);


            Assert.AreEqual(expectedControlWidth + BORDER_SIZE + GAP_SIZE, control2.Left);
            Assert.AreEqual(BORDER_SIZE, control2.Top);
            Assert.AreEqual(expectedControlWidth, control2.Width);

            Assert.AreEqual(BORDER_SIZE, control3.Left);
            Assert.AreEqual(BORDER_SIZE + control2.Height + GAP_SIZE, control3.Top);
            Assert.AreEqual(expectedControlWidth, control3.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_TwoColumns_FiveControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control1 = GetControlFactory().CreateControl();
            IControlHabanero control2 = GetControlFactory().CreateControl();
            IControlHabanero control5 = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2 * BORDER_SIZE - 1 * GAP_SIZE) / NoColumns;
            control2.Height = 40;
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(control1);
            columnLayoutManager.AddControl(control2);
            IControlHabanero control3 = columnLayoutManager.AddControl(GetControlFactory().CreateControl());
            columnLayoutManager.AddControl(GetControlFactory().CreateControl());
            columnLayoutManager.AddControl(control5);
            //---------------Test Result -----------------------
            Assert.AreEqual(BORDER_SIZE, control1.Left);
            Assert.AreEqual(BORDER_SIZE, control1.Top);
            Assert.AreEqual(expectedControlWidth, control1.Width);


            Assert.AreEqual(expectedControlWidth + BORDER_SIZE + GAP_SIZE, control2.Left);
            Assert.AreEqual(BORDER_SIZE, control2.Top);
            Assert.AreEqual(expectedControlWidth, control2.Width);

            Assert.AreEqual(BORDER_SIZE, control5.Left);
            int thirdRowTop = BORDER_SIZE + control2.Height + GAP_SIZE * 2 + control3.Height;
            Assert.AreEqual(thirdRowTop, control5.Top);
            Assert.AreEqual(expectedControlWidth, control5.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestAddControl_TwoColumns_FiveControl_ResizeManagedControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero control1 = GetControlFactory().CreateControl();
            IControlHabanero control2 = GetControlFactory().CreateControl();
            IControlHabanero control5 = GetControlFactory().CreateControl();
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            int NoColumns = 2;
            columnLayoutManager.ColumnCount = NoColumns;
            IControlHabanero managedControl = columnLayoutManager.ManagedControl;
            int expectedControlWidth = (managedControl.Width - 2 * BORDER_SIZE - 1 * GAP_SIZE) / NoColumns;
            control2.Height = 40;
            columnLayoutManager.AddControl(control1);
            columnLayoutManager.AddControl(control2);
            IControlHabanero control3 = columnLayoutManager.AddControl(GetControlFactory().CreateControl());
            columnLayoutManager.AddControl(GetControlFactory().CreateControl());
            columnLayoutManager.AddControl(control5);

            //--------------Assert PreConditions----------------  
            Assert.AreEqual(BORDER_SIZE, control1.Left);
            Assert.AreEqual(BORDER_SIZE, control1.Top);
            Assert.AreEqual(expectedControlWidth, control1.Width);


            Assert.AreEqual(expectedControlWidth + BORDER_SIZE + GAP_SIZE, control2.Left);
            Assert.AreEqual(BORDER_SIZE, control2.Top);
            Assert.AreEqual(expectedControlWidth, control2.Width);

            Assert.AreEqual(BORDER_SIZE, control5.Left);
            int thirdRowTop = BORDER_SIZE + control2.Height + GAP_SIZE * 2 + control3.Height;
            Assert.AreEqual(thirdRowTop, control5.Top);
            Assert.AreEqual(expectedControlWidth, control5.Width);

            //---------------Execute Test ----------------------
            managedControl.Height = 330;
            managedControl.Width = 195;

            //---------------Test Result -----------------------
            int expectedControlWidth_afterResize = (managedControl.Width - 2 * BORDER_SIZE - 1 * GAP_SIZE) / NoColumns;
            Assert.AreEqual(BORDER_SIZE, control1.Left);
            Assert.AreEqual(BORDER_SIZE, control1.Top);
            Assert.AreEqual(expectedControlWidth_afterResize, control1.Width);


            Assert.AreEqual(expectedControlWidth_afterResize + BORDER_SIZE + GAP_SIZE, control2.Left);
            Assert.AreEqual(BORDER_SIZE, control2.Top);
            Assert.AreEqual(expectedControlWidth_afterResize, control2.Width);

            Assert.AreEqual(BORDER_SIZE, control5.Left);
            Assert.AreEqual(thirdRowTop, control5.Top);
            Assert.AreEqual(expectedControlWidth_afterResize, control5.Width);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestSetColumnCount_Zero()
        {
            //---------------Set up test pack-------------------
            ColumnLayoutManager columnLayoutManager = GetColumnLayoutManager();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            try
            {
                columnLayoutManager.ColumnCount = 0;
                Assert.Fail("This should throw an error");
            }
                //---------------Test Result -----------------------
            catch (LayoutManagerException ex)
            {
                StringAssert.Contains("You cannot set the column count for a column layout manager to less than 1",
                                      ex.Message);
            }

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestResizeControlOnManagedControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreatePanel();
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(managedControl, GetControlFactory());
            columnLayoutManager.GapSize = 0;
            columnLayoutManager.BorderSize = 0;
            IControlHabanero createdControl1 = GetControlFactory().CreateControl();
            IControlHabanero createdControl2 = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            columnLayoutManager.AddControl(createdControl1);
            columnLayoutManager.AddControl(createdControl2);
            createdControl1.Height += 10;
            //---------------Test Result -----------------------
            Assert.AreEqual(createdControl1.Height, createdControl2.Top );
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestResizeManagedControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreatePanel();
            int originalWidth = 400;
            managedControl.Width = originalWidth;
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(managedControl, GetControlFactory());
            columnLayoutManager.GapSize = 0;
            columnLayoutManager.BorderSize = 0;
            IControlHabanero createdControl1 = GetControlFactory().CreateControl();
            IControlHabanero createdControl2 = GetControlFactory().CreateControl();
            columnLayoutManager.AddControl(createdControl1);
            columnLayoutManager.AddControl(createdControl2);
            //---------------Assert preconditions---------------
            Assert.AreEqual(originalWidth, createdControl1.Width);
            Assert.AreEqual(originalWidth, createdControl2.Width);
            //---------------Execute Test ----------------------
            int newWidth = 500;
            managedControl.Width = newWidth;
            //---------------Test Result -----------------------
            Assert.AreEqual(newWidth, createdControl1.Width);
            Assert.AreEqual(newWidth, createdControl2.Width);

            //---------------Tear Down -------------------------
        }
        
    }
}
