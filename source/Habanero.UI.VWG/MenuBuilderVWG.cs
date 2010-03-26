// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    ///<summary>
    /// An Implementation of a <see cref="IMenuBuilder"/> for Visual Web Gui
    ///</summary>
    public class MenuBuilderVWG : IMenuBuilder
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Creates a <see cref="MenuBuilderVWG"/> with the appropriate Control Factory.
        /// </summary>
        /// <param name="controlFactory"></param>
        public MenuBuilderVWG(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Constructor for <see cref="BuildMainMenu"/>
        /// </summary>
        /// <param name="habaneroMenu"></param>
        /// <returns></returns>
        public IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu)
        {
            IMainMenuHabanero mainMenu = this.ControlFactory.CreateMainMenu(habaneroMenu);
//            mainMenu.Name = habaneroMenu.Name;
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                mainMenu.MenuItems.Add(BuildMenu(submenu));
            }
            return mainMenu;
        }

        /// <summary>
        /// Returns the control factory being used to create the Menu and the MenuItems
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }
        /// <summary>
        /// Build an Actual Menu based on the habaneroMenu
        /// </summary>
        /// <param name="habaneroMenu"></param>
        /// <returns></returns>
        protected virtual IMenuItem BuildMenu(HabaneroMenu habaneroMenu)
        {
            IMenuItem menuItem = this.ControlFactory.CreateMenuItem(habaneroMenu.Name);
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                menuItem.MenuItems.Add(BuildMenu(submenu));
            }
            foreach (HabaneroMenu.Item habaneroMenuItem in habaneroMenu.MenuItems)
            {
                IMenuItem childMenuItem = this.ControlFactory.CreateMenuItem((habaneroMenuItem));
                childMenuItem.Click += delegate { childMenuItem.DoClick(); };

                menuItem.MenuItems.Add(childMenuItem);
            }
            return menuItem;
        }
    }

    ///<summary>
    /// The standard VWG main menu structure object.
    ///</summary>
    internal class MainMenuVWG : MainMenu, IMainMenuHabanero
    {
        protected readonly HabaneroMenu _habaneroMenu;
        private readonly MenuItemCollectionVWG _menuItems;

        public MainMenuVWG()
        {
            _menuItems = new MenuItemCollectionVWG(base.MenuItems);
        }

        public MainMenuVWG(HabaneroMenu habaneroMenu) : this()
        {
            _habaneroMenu = habaneroMenu;
            if (_habaneroMenu != null) this.Name = _habaneroMenu.Name;
        }

        private IControlFactory GetControlFactory()
        {
            if (_habaneroMenu != null)
                if (_habaneroMenu.ControlFactory != null)
                    return _habaneroMenu.ControlFactory;
            return GlobalUIRegistry.ControlFactory;
        }

        ///<summary>
        /// The collection of menu items for this menu
        ///</summary>
        public new IMenuItemCollection MenuItems
        {
            get { return _menuItems; }
        }

        /// <summary>
        /// This method sets up the form so that the menu is displayed and the form is able to 
        /// display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        public void DockInForm(IControlHabanero form)
        {
            IControlHabanero panel = GetControlFactory().CreatePanel();
            panel.Dock = Habanero.UI.Base.DockStyle.Fill;
            form.Controls.Add(panel);
            Form formVWG = (Form) form;
            formVWG.Menu = this;
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
            get { return (MenuItemVWG) (_menuItemCollection[index]); }
        }

        public void Add(IMenuItem menuItem)
        {
            _menuItemCollection.Add((MenuItem) menuItem);
        }

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _menuItemCollection.GetEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable<IMenuItem>

        IEnumerator<IMenuItem> IEnumerable<IMenuItem>.GetEnumerator()
        {
            return ((IEnumerable<IMenuItem>)_menuItemCollection).GetEnumerator();
        }

        #endregion

    }

    internal class MenuItemVWG : MenuItem, IMenuItem
    {
        private readonly HabaneroMenu.Item _habaneroMenuItem;
        private IFormControl _formControl;
        private IControlManager _controlManager;

        public MenuItemVWG(HabaneroMenu.Item habaneroMenuItem) : this(habaneroMenuItem.Name)
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
                    if (_habaneroMenuItem.Form == null || _habaneroMenuItem.Form.Controls.Count <= 0) return;
                    if (_habaneroMenuItem.FormControlCreator != null)
                    {
                        if (_formControl == null) _formControl = _habaneroMenuItem.FormControlCreator();
                        _formControl.SetForm(null);
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
                        throw new Exception
                            ("Please set up the MenuItem with at least one Creational or custom handling delegate");
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