using System;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    public class MenuBuilderVWG : IMenuBuilder
    {
        public IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu)
        {
            MainMenuVWG mainMenu = new MainMenuVWG();
            mainMenu.Name = habaneroMenu.Name;
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                mainMenu.MenuItems.Add(BuildMenu(submenu));
            }
            return mainMenu;
        }

        private IMenuItem BuildMenu(HabaneroMenu habaneroMenu)
        {
            MenuItemVWG menuItem = new MenuItemVWG(habaneroMenu.Name);
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                menuItem.MenuItems.Add(BuildMenu(submenu));
            }
            foreach (HabaneroMenu.Item habaneroMenuItem in habaneroMenu.MenuItems)
            {
                MenuItemVWG childMenuItem = new MenuItemVWG(habaneroMenuItem);
                childMenuItem.Click += delegate(object sender, EventArgs e) { childMenuItem.DoClick(); };
                   
                menuItem.MenuItems.Add(childMenuItem);
            }
            return menuItem;
        }

      
    }

    internal class MainMenuVWG : MainMenu, IMainMenuHabanero
    {
        public new IMenuItemCollection MenuItems
        {
            get { return new MenuItemCollectionVWG(base.MenuItems); }
        }


    }

    internal class MenuItemCollectionVWG : IMenuItemCollection
    {
        private readonly MenuItemCollection _menuItemCollection;

        public MenuItemCollectionVWG(MenuItemCollection menuItemCollection)
        {
            _menuItemCollection = menuItemCollection;
        }

        public int Count
        {
            get { return _menuItemCollection.Count; }
        }

        public IMenuItem this[int index]
        {
            get { return ( MenuItemVWG)(_menuItemCollection[index]); }
        }

        public void Add(IMenuItem menuItem)
        {
            _menuItemCollection.Add((MenuItem) menuItem);
        }
    }

    internal class MenuItemVWG : MenuItem, IMenuItem
    {
        private readonly HabaneroMenu.Item _habaneroMenuItem;
        private IFormControl _formControl;

        public MenuItemVWG(HabaneroMenu.Item habaneroMenuItem): this(habaneroMenuItem.Name)
        {
            _habaneroMenuItem = habaneroMenuItem;
        }

        public MenuItemVWG(string text) : base(text)
        {
        }

        public new IMenuItemCollection MenuItems
        {
            get { return new MenuItemCollectionVWG(base.MenuItems); }
        }

        public void PerformClick()
        {
            DoClick();
        }

        public void DoClick()
        {
            if (_habaneroMenuItem.CustomHandler != null)
            {
                _habaneroMenuItem.CustomHandler(this, new EventArgs());

            }
            else if (_habaneroMenuItem.FormControlCreator != null)
            {
                if (_formControl == null) _formControl = _habaneroMenuItem.FormControlCreator();
                if (_habaneroMenuItem.Form == null) return;

                IControlHabanero control = (IControlHabanero)_formControl;
                control.Dock = Base.DockStyle.Fill;
                IControlHabanero controlToNestIn = _habaneroMenuItem.Form.Controls[0];
                controlToNestIn.Controls.Clear();
                controlToNestIn.Controls.Add(control);
         
            }


        }
    }
}