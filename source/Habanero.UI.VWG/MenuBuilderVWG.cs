//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
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
        private IControlManager _controlManager;

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
            try
            {
                if (_habaneroMenuItem.CustomHandler != null)
                {
                    _habaneroMenuItem.CustomHandler(this, new EventArgs());
                }
                else
                {
                    IControlHabanero control;
                    if (_habaneroMenuItem.Form == null) return;
                    if (_habaneroMenuItem.FormControlCreator != null)
                    {
                        if (_formControl == null) _formControl = _habaneroMenuItem.FormControlCreator();
                        control = (IControlHabanero) _formControl;
                    }
                    else if (_habaneroMenuItem.ControlManagerCreator != null)
                    {
                        if (_controlManager == null)
                            _controlManager = _habaneroMenuItem.ControlManagerCreator(_habaneroMenuItem.ControlFactory);
                        control = _controlManager.Control;
                    }
                    else
                    {
                        throw new Exception(
                            "Please set up the MenuItem with at least one Creational or custom handling delegate");
                    }
                    control.Dock = Base.DockStyle.Fill;
                    IControlHabanero controlToNestIn = _habaneroMenuItem.Form.Controls[0];
                    controlToNestIn.Controls.Clear();
                    controlToNestIn.Controls.Add(control);
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, null, null);
            }
        }
    }
}