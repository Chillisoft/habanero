using System.Drawing;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.LayoutManager
{
    [TestFixture]
    public class TestGridLayoutManagerWin : TestGridLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        private static IPanel CreateColoredPanel(IControlFactory controlFactory, string labelPrefix)
        {
            System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)controlFactory.CreatePanel();
            ILabel label = controlFactory.CreateLabel(labelPrefix);
            label.Height = 15;
            label.BackColor = Color.White;
            panel.Controls.Add((System.Windows.Forms.Control)label);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.BackColor = Color.FromArgb(TestUtil.GetRandomInt(255), TestUtil.GetRandomInt(255), TestUtil.GetRandomInt(255));
            return (IPanel)panel;
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
            GridLayoutManager gridLayoutManager = new GridLayoutManager(panel, controlFactory);
            gridLayoutManager.SetGridSize(6, 2);
            int controlNumber = 1;
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()), 2, 1);
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()), 2, 1);
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            //IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            //buttonGroupControl.Dock = DockStyle.Top;
            //groupBox.Controls.Add(buttonGroupControl);
            //buttonGroupControl.AddButton("Add Control", (sender, e) => gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":")));
            //buttonGroupControl.AddButton("-Columns", (sender, e) =>
            //{
            //    if (gridLayoutManager.ColumnCount > 1)
            //    {
            //        gridLayoutManager.ColumnCount--;
            //        gridLayoutManager.Refresh();
            //    }
            //});
            //buttonGroupControl.AddButton("+Columns", (sender, e) => { gridLayoutManager.ColumnCount++; gridLayoutManager.Refresh(); });
            IFormHabanero form = controlFactory.CreateOKCancelDialogFactory().CreateOKCancelForm(groupBox, "Test Grid Layout Manager");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            form.ShowDialog();
            //---------------Test Result -----------------------
        }

    }
}