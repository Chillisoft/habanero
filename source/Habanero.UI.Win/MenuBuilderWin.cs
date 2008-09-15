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
using System.ComponentModel;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class MenuBuilderWin : IMenuBuilder
    {

        public IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu)
        {
            MainMenuWin mainMenu = new MainMenuWin();
            mainMenu.Name = habaneroMenu.Name;
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                mainMenu.MenuItems.Add(BuildMenu(submenu));
            }
            return mainMenu;
        }

        private IMenuItem BuildMenu(HabaneroMenu habaneroMenu)
        {
            MenuItemWin menuItem = new MenuItemWin(habaneroMenu.Name);
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                menuItem.MenuItems.Add(BuildMenu(submenu));
            }
            foreach (HabaneroMenu.Item habaneroMenuItem in habaneroMenu.MenuItems)
            {
                MenuItemWin childMenuItem = new MenuItemWin(habaneroMenuItem);
                childMenuItem.Click += delegate { childMenuItem.DoClick(); };
                menuItem.MenuItems.Add(childMenuItem);
            }

  

    return menuItem;
        }

   
    }
      


    internal class MainMenuWin : MainMenu, IMainMenuHabanero
    {
        public new IMenuItemCollection MenuItems
        {
            get { return new MenuItemCollectionWin(base.MenuItems); }
        }
    }

    internal class MenuItemCollectionWin : IMenuItemCollection
    {
        private readonly Menu.MenuItemCollection _menuItemCollection;

        public MenuItemCollectionWin(Menu.MenuItemCollection menuItemCollection)
        {
            _menuItemCollection = menuItemCollection;
        }

        public int Count
        {
            get { return _menuItemCollection.Count; }
        }

        public IMenuItem this[int index]
        {
            get { return (IMenuItem)_menuItemCollection[index]; }
        }

        public void Add(IMenuItem menuItem)
        {
            _menuItemCollection.Add((MenuItem)menuItem);
        }
    }

    internal class MenuItemWin : MenuItem, IMenuItem
    {
        private HabaneroMenu.Item _habaneroMenuItem;
        private IFormHabanero _createdForm;
        private IFormControl _formControl;

        public MenuItemWin(HabaneroMenu.Item habaneroMenuItem)
            : this(habaneroMenuItem.Name)
        {
            _habaneroMenuItem = habaneroMenuItem;
        }
        public MenuItemWin(string text)
            : base(text)
        {
        }

        public new IMenuItemCollection MenuItems
        {
            get { return new MenuItemCollectionWin(base.MenuItems); }
        }
        public void DoClick()
        {
            if (_habaneroMenuItem.CustomHandler != null)
            {
                _habaneroMenuItem.CustomHandler(this, new EventArgs());

            } else if (_habaneroMenuItem.FormControlCreator != null)
            {
                if (_createdForm != null)
                {
                    try
                    {
                        _createdForm.Show();
                        _createdForm.Refresh();
                        _createdForm.Focus();
                        _createdForm.PerformLayout();
                        return;
                    }
                    catch (Win32Exception)
                    {
                        //note: it will throw this error in testing.
                        return;
                    }
                    catch (ObjectDisposedException)
                    {
                        //the window has been disposed, we need to create a new one
                    }
                }

                _formControl = _habaneroMenuItem.FormControlCreator();
                    if (_habaneroMenuItem.Form == null) return;
                    _createdForm = _habaneroMenuItem.ControlFactory.CreateForm();
                    _createdForm.Width = 800;
                    _createdForm.Height = 600;
                    _createdForm.MdiParent = _habaneroMenuItem.Form;
                    _createdForm.WindowState = Habanero.UI.Base.FormWindowState.Maximized;
                    _createdForm.Text = _habaneroMenuItem.Name;
                    _createdForm.Controls.Clear();
             
                    BorderLayoutManager layoutManager =
                        _habaneroMenuItem.ControlFactory.CreateBorderLayoutManager(_createdForm);

                    layoutManager.AddControl((IControlHabanero) _formControl, BorderLayoutManager.Position.Centre);
                    _createdForm.Show();

                    _createdForm.Closed += delegate
                    {
                        _createdForm = null;
                        _formControl = null;
                    };
                

            }
        }
 
    }
    
}