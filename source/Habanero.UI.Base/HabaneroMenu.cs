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

    public delegate IFormControl FormControlCreator();
    public delegate IControlManager ControlManagerCreator(IControlFactory controlFactory);

    public abstract class MenuItemCreator
    {
        public abstract void AddToMenu(HabaneroMenu currentMenu, IControlFactory controlFactory);
    }

    public class HabaneroMenu
    {
        private readonly List<HabaneroMenu> _submenus = new List<HabaneroMenu>();
        private readonly string _name;
        private readonly IFormHabanero _form;
        private readonly IControlFactory _controlFactory;
        private readonly List<HabaneroMenu.Item> _menuItems = new List<HabaneroMenu.Item>();

        public HabaneroMenu(string menuName) : this(menuName, null, null)
        {
        }

        public HabaneroMenu(string menuName, IFormHabanero form, IControlFactory controlFactory)
        {
            _name = menuName;
            _form = form;
            _controlFactory = controlFactory;
        }

        public List<HabaneroMenu> Submenus
        {
            get { return _submenus; }
        }

        public string Name
        {
            get { return _name; }
        }

        public List<HabaneroMenu.Item> MenuItems
        {
            get { return _menuItems; }
        }

        internal IFormHabanero Form
        {
            get { return _form; }
        }

        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        public HabaneroMenu AddSubmenu(string menuName)
        {
            HabaneroMenu submenu = new HabaneroMenu(menuName, _form, _controlFactory);
            this._submenus.Add(submenu);
            return submenu;
        }

        public HabaneroMenu.Item AddMenuItem(string menuItemName)
        {
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(menuItemName, _form, _controlFactory);
            _menuItems.Add(menuItem);
            return menuItem;
        }

        public class Item
        {
            private readonly string _name;
            private readonly IControlFactory _controlFactory;
            private readonly IFormHabanero _form;
            private FormControlCreator _formControlCreator;
            private EventHandler _customHandler;
            private ControlManagerCreator _controlManagerCreator;

            public Item(string name) : this(name, null, null)
            {
     
            }
            public Item(string name, IFormHabanero form, IControlFactory controlFactory)
            {
                _name = name;
                _controlFactory = controlFactory;
                _form = form;
            }

            public string Name
            {
                get { return _name; }
            }

            public FormControlCreator FormControlCreator
            {
                get { return _formControlCreator; }
                set { _formControlCreator = value; }
            }

            public ControlManagerCreator ControlManagerCreator
            {
                get { return _controlManagerCreator; }
                set { _controlManagerCreator = value; }
            }

            public EventHandler CustomHandler
            {
                get { return _customHandler; }
                set { _customHandler = value; }
            }

            public IControlFactory ControlFactory
            {
                get { return _controlFactory; }
            }

            public IFormHabanero Form
            {
                get { return _form; }
            }
        }
    }
}