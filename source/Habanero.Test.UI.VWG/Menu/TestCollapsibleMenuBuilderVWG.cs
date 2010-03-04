using System.Drawing;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuBuilderVWG : TestCollapsibleMenuBuilder
    {
        protected override IMenuBuilder CreateMenuBuilder()
        {
            return new CollapsibleMenuBuilderVWG(GetControlFactory());
        }

        protected override IFormControlStub CreateFormControlStub()
        {
            return new FormControlStubVWG();
        }

        private class FormControlStubVWG : UserControlVWG, IFormControlStub
        {
            public void SetForm(IFormHabanero form)
            {
                SetFormCalled = true;
                SetFormArgument = form;
            }

            public IFormHabanero SetFormArgument { get; private set; }

            public bool SetFormCalled { get; private set; }
        }

        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryVWG();
        }

        [Test]
        public virtual void Test_CreateLeafMenuItems_ShouldCreatePanelWithLeafMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.GetRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(subMenuName);
            string menuItemName1 = TestUtil.GetRandomString();
            submenu.AddMenuItem(menuItemName1);
            submenu.AddMenuItem(TestUtil.GetRandomString());
            CollapsibleMenuBuilderVWG menuBuilder = (CollapsibleMenuBuilderVWG)CreateMenuBuilder();
            IMenuItem menuItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), "Some Sub Menu");
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, menuItem.MenuItems.Count);
            ICollapsiblePanel menuItemAsControl = (ICollapsiblePanel)menuItem;
            Assert.AreEqual(1, menuItemAsControl.Controls.Count);
            Assert.AreEqual(2, submenu.MenuItems.Count);
            //---------------Execute Test ----------------------
            menuBuilder.CreateLeafMenuItems(submenu, menuItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, menuItem.MenuItems.Count);
            Assert.AreEqual(2, menuItemAsControl.Controls.Count);
            IControlHabanero contentControl = menuItemAsControl.ContentControl;
            Assert.AreEqual(2, contentControl.Controls.Count);
            IControlHabanero firstControl = contentControl.Controls[0];
            IControlHabanero secondControl = contentControl.Controls[1];
            Assert.GreaterOrEqual(secondControl.Top, firstControl.Top + firstControl.Height);
        }


        [Test]
        public virtual void Test_CreateSubMenuItems_ShouldCreatePanelWithLeafMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.GetRandomString();
            habaneroMenu.AddSubMenu(subMenuName);
            habaneroMenu.AddSubMenu("SecondSubMenu");
            CollapsibleMenuBuilderVWG menuBuilder = (CollapsibleMenuBuilderVWG)CreateMenuBuilder();
            IMenuItem menuItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), "Some Sub Menu");
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, menuItem.MenuItems.Count);
            ICollapsiblePanel menuItemAsControl = (ICollapsiblePanel)menuItem;
            Assert.AreEqual(1, menuItemAsControl.Controls.Count);
            Assert.AreEqual(2, habaneroMenu.Submenus.Count);
            //---------------Execute Test ----------------------
            menuBuilder.BuildSubMenu(habaneroMenu, menuItem.MenuItems);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, menuItem.MenuItems.Count);
            Assert.AreEqual(1, menuItemAsControl.Controls.Count);
        }
        [Test]
        public virtual void Test_DockMenuInForm_ShouldSetUpSplitterPanels()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            submenu.AddMenuItem(TestUtil.GetRandomString());
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IControlHabanero form = habaneroMenu.Form;
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            form.Size = new Size(460, 900);
            //-------------Assert Preconditions -------------
            Assert.IsFalse(IsMenuDocked(menu, form));
            //---------------Execute Test ----------------------
            menu.DockInForm(form);
            //---------------Test Result -----------------------
            IControlHabanero control = form.Controls[0];
            Assert.IsInstanceOf(typeof(ISplitContainer), control);
            Gizmox.WebGUI.Forms.SplitContainer splitContainerVWG = (Gizmox.WebGUI.Forms.SplitContainer)control;
            Gizmox.WebGUI.Forms.SplitterPanel panel1 = splitContainerVWG.Panel1;
            Assert.AreEqual(250, panel1.Width);
            Assert.AreEqual(1, panel1.Controls.Count);
            IControlHabanero menuControl = (IControlHabanero)panel1.Controls[0];
            Assert.IsInstanceOf(typeof(ICollapsiblePanelGroupControl), menuControl);
            panel1.Size = new Size(121, 333);
            Assert.AreEqual(panel1.Width, menuControl.Width);

            Gizmox.WebGUI.Forms.SplitterPanel panel2 = splitContainerVWG.Panel2;
            Assert.AreEqual(1, panel2.Controls.Count);
            IControlHabanero editorControl = (IControlHabanero)panel2.Controls[0];
            Assert.IsInstanceOf(typeof(MainEditorPanelVWG), editorControl);
            panel2.Size = new Size(321, 514);
            Assert.AreEqual(panel2.Width, editorControl.Width);
            Assert.AreEqual(panel2.Height, editorControl.Height);
        }

        protected override void AssertControlDockedInForm(IControlHabanero control, IControlHabanero form)
        {
            Assert.AreEqual(1, form.Controls.Count, "No container control found in form");
            IControlHabanero splitCntrl = form.Controls[0];
            Assert.IsInstanceOf(typeof(ISplitContainer), splitCntrl);
            Gizmox.WebGUI.Forms.SplitContainer splitContainerVWG = (Gizmox.WebGUI.Forms.SplitContainer)splitCntrl;

            Gizmox.WebGUI.Forms.SplitterPanel panel2 = splitContainerVWG.Panel2;
            Assert.AreEqual(1, panel2.Controls.Count);
            IControlHabanero editorControl = (IControlHabanero)panel2.Controls[0];
            Assert.IsInstanceOf(typeof(IMainEditorPanel), editorControl);
            IMainEditorPanel mainEditorPanel = (IMainEditorPanel)editorControl;
            IControlHabanero contentControl = mainEditorPanel.EditorPanel;

            Assert.AreEqual(1, contentControl.Controls.Count);
            Assert.AreSame(control, contentControl.Controls[0]);
            Assert.AreEqual(Habanero.UI.Base.DockStyle.Fill, control.Dock);
        }

    }
}