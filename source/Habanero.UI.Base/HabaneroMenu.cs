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
using System.Collections.Generic;

namespace Habanero.UI.Base
{

    ///<summary>
    /// A delegate for Creating a <see cref="IFilterControl"/>
    ///</summary>
    public delegate IFormControl FormControlCreator();
    ///<summary>
    /// A delegate definition for creating a <see cref="IControlManager"/>
    ///</summary>
    ///<param name="controlFactory"></param>
    public delegate IControlManager ControlManagerCreator(IControlFactory controlFactory);

    ///<summary>
    /// An abstract class that provides the functionality to Add <see cref="HabaneroMenu"/>.
    ///</summary>
    public abstract class MenuItemCreator
    {
        ///<summary>
        /// Adds a <see cref="HabaneroMenu"/>
        ///</summary>
        ///<param name="currentMenu"></param>
        ///<param name="controlFactory"></param>
        public abstract void AddToMenu(HabaneroMenu currentMenu, IControlFactory controlFactory);
    }

    ///<summary>
    /// A class for managing a menu including its list of sub menus (other Habanero Menus)
    ///</summary>
    public class HabaneroMenu
    {
        private readonly List<HabaneroMenu> _submenus = new List<HabaneroMenu>();
        private readonly string _name;
        private readonly IFormHabanero _form;
        private readonly IControlFactory _controlFactory;
        private readonly List<Item> _menuItems = new List<Item>();

        ///<summary>
        /// Constrcutor for <see cref="HabaneroMenu"/>
        ///</summary>
        ///<param name="menuName"></param>
        public HabaneroMenu(string menuName) : this(menuName, null, null)
        {
        }

        ///<summary>
        ///  Constrcutor for <see cref="HabaneroMenu"/>
        ///</summary>
        ///<param name="menuName"></param>
        ///<param name="form"></param>
        ///<param name="controlFactory"></param>
        public HabaneroMenu(string menuName, IFormHabanero form, IControlFactory controlFactory)
        {
            _name = menuName;
            _form = form;
            _controlFactory = controlFactory;
        }

        ///<summary>
        /// A list of sub Menus for this menu
        ///</summary>
        public List<HabaneroMenu> Submenus
        {
            get { return _submenus; }
        }

        ///<summary>
        /// The name of this menu.
        ///</summary>
        public string Name
        {
            get { return _name; }
        }

        ///<summary>
        /// A list of <see cref="Item"/>s shown in this menu.
        ///</summary>
        public List<HabaneroMenu.Item> MenuItems
        {
            get { return _menuItems; }
        }

        internal IFormHabanero Form
        {
            get { return _form; }
        }

        ///<summary>
        /// The <see cref="IControlFactory"/> used to Create controls for this menu.
        ///</summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        ///<summary>
        /// Adds a sub menu to this menu. This method creates a new <see cref="HabaneroMenu"/> with the name
        /// <paramref name="menuName"/> and adds it as a sub menu.
        ///</summary>
        ///<param name="menuName"></param>
        ///<returns></returns>
        public HabaneroMenu AddSubmenu(string menuName)
        {
            HabaneroMenu submenu = new HabaneroMenu(menuName, _form, _controlFactory);
            this._submenus.Add(submenu);
            return submenu;
        }

        ///<summary>
        /// Adds a MenuItem. Creates a Menu <see cref="Item"/> with the name <paramref name="menuItemName"/>
        ///</summary>
        ///<param name="menuItemName"></param>
        ///<returns></returns>
        public Item AddMenuItem(string menuItemName)
        {
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(menuItemName, _form, _controlFactory);
            _menuItems.Add(menuItem);
            return menuItem;
        }

        ///<summary>
        /// A particular menu item that ????
        ///</summary>
        public class Item
        {
            private readonly string _name;
            private readonly IControlFactory _controlFactory;
            private readonly IFormHabanero _form;

            ///<summary>
            /// Consructor for an <see cref="Item"/>
            ///</summary>
            ///<param name="name"></param>
            public Item(string name) : this(name, null, null)
            {
     
            }
            /// <summary>
            /// Construcotr for an <see cref="Item"/>
            /// </summary>
            /// <param name="name"></param>
            /// <param name="form"></param>
            /// <param name="controlFactory"></param>
            public Item(string name, IFormHabanero form, IControlFactory controlFactory)
            {
                _name = name;
                _controlFactory = controlFactory;
                _form = form;
            }

            ///<summary>
            /// The menu item name.
            ///</summary>
            public string Name
            {
                get { return _name; }
            }

            ///<summary>
            /// The Creator that creates the form when this menu is selected
            ///</summary>
            public FormControlCreator FormControlCreator { get; set; }

            ///<summary>
            /// Gets and sets the <see cref="ControlManagerCreator"/> delegate.
            ///</summary>
            public ControlManagerCreator ControlManagerCreator { get; set; }

            ///<summary>
            /// Gets and Sets the CustomHandler that is used when the menu is clicked. This allows
            /// the developer to hook into this event to imlement custom logic when the menu item is clicked.
            ///</summary>
            public EventHandler CustomHandler { get; set; }

            ///<summary>
            /// Gets the <see cref="IControlFactory"/> that is used to create control for htis menu.
            ///</summary>
            public IControlFactory ControlFactory
            {
                get { return _controlFactory; }
            }

            ///<summary>
            /// Gest the <see cref="IFormHabanero"/> that this menu is associated with
            ///</summary>
            public IFormHabanero Form
            {
                get { return _form; }
            }
        }
    }
}