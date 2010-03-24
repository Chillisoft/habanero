// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections.Generic;

namespace Habanero.UI.Base
{
    ///<summary>
    /// A delegate for Creating a <see cref="IFormControl"/>
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
    /// A class for managing a menu including its list of sub menus (other Habanero Menus).
    /// This class is used by a menu builder to build a specific Menu e.g. A main menu e.g. MainMenuWin or an outlook style menu e.g. CollapsibleMenuWin.<br/>
    /// With this class you can either <see cref="AddSubMenu"/> or <see cref="AddMenuItem"/>.
    /// Where adding a sub menu will create another HabaneroMenu and adding a MenuItem will add a leaf
    /// item to the menu. Selecting a SubMenu will cause the sub Menu to expand. 
    /// Selecting the MenuItem(<see cref="Item"/>) will cause the relevant controller to be executed the order in 
    /// which this occurs is as follows.
    /// <li>Any CustomMenuHandler (set via the <see cref="Item.CustomHandler"/> will take precidence) i.e. you can make the menu item  do anything you want via a custom handler.</li>
    /// <li>The FormControl Creator takes precedence next this is typically used by the Form Controller to manage the form (usually in an MDI type environment)</li>
    /// <li>The ControlManagerCreator takes Precedence next this is typeically used in an SDI type interface where the menu item swaps out the control. (e.g. in a outlook style menu)</li>
    ///</summary> 
    public class HabaneroMenu
    {
        private readonly List<HabaneroMenu> _submenus = new List<HabaneroMenu>();
        private readonly string _name;
        private readonly IControlHabanero _form;
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
        public HabaneroMenu(string menuName, IControlHabanero form, IControlFactory controlFactory)
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

        /// <summary>
        /// The form that this menu is associated with.
        /// </summary>
        public IControlHabanero Form
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
        [Obsolete("Please use correctly spelt AddSubMenu")]
        public HabaneroMenu AddSubmenu(string menuName)
        {
            return AddSubMenu(menuName);
        }

        ///<summary>
        /// Adds a sub menu to this menu. This method creates a new <see cref="HabaneroMenu"/> with the name
        /// <paramref name="menuName"/> and adds it as a sub menu.
        ///</summary>
        ///<param name="menuName"></param>
        ///<returns></returns>
        public HabaneroMenu AddSubMenu(string menuName)
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
            HabaneroMenu.Item menuItem = new HabaneroMenu.Item(this, menuItemName, _form, _controlFactory);
            _menuItems.Add(menuItem);
            return menuItem;
        }

        ///<summary>
        /// A particular menu item that will be built into a Leaf Node of the Relevant Menu.
        /// The MenuBulder will build a menu item represented by this Item.
        /// This Item can be set up with either a <see cref="CustomHandler"/> <br/>
        ///  or a <see cref="ControlManagerCreator"/> or a <see cref="FormControlCreator"/>.
        /// These are used by the relevant MenuItemControl to respond the its click event. 
        ///</summary>
        public class Item
        {
            private readonly HabaneroMenu _parentMenu;
            private readonly string _name;
            private readonly IControlFactory _controlFactory;
            private readonly IControlHabanero _form;

            ///<summary>
            /// Constructor for an <see cref="Item"/>
            ///</summary>
            ///<param name="parentMenu"></param>
            ///<param name="name"></param>
            public Item(HabaneroMenu parentMenu, string name)
                : this(parentMenu, name, null, null)
            {
            }

            /// <summary>
            /// Constructor for an <see cref="Item"/>
            /// </summary>
            /// <param name="parentMenu"></param>
            /// <param name="name"></param>
            /// <param name="form"></param>
            /// <param name="controlFactory"></param>
            public Item(HabaneroMenu parentMenu, string name, IControlHabanero form, IControlFactory controlFactory)
            {
                _parentMenu = parentMenu;
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
            /// Returns the Parent Manu for the current Menu if there is no parent menu then returns null.
            ///</summary>
            public HabaneroMenu ParentMenu
            {
                get { return _parentMenu; }
            }

            ///<summary>
            /// The Creator that creates the form when this menu Item is selected.
            /// This is used where a pop up form is created in response to the MenuItem Click Event.
            ///</summary>
            public FormControlCreator FormControlCreator { get; set; }

            ///<summary>
            /// Gets and sets the <see cref="ControlManagerCreator"/> delegate.
            /// This is a delegate that creates a <see cref="IControlManager"/>. The control manager
            /// wraps a control.
            ///</summary>
            public ControlManagerCreator ControlManagerCreator { get; set; }

            ///<summary>
            /// Gets and Sets the CustomHandler that is used when the menu is clicked. This allows
            /// the developer to hook into this event to implement custom logic when the menu item is clicked.
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
            /// Gets the <see cref="IFormHabanero"/> that this menu is associated with
            ///</summary>
            public IControlHabanero Form
            {
                get { return _form; }
            }

            ///<summary>
            /// Set the <see cref="IControlHabanero"/> that will be used when the MenuItem is selected.
            /// In reality this is just a shortcut since a <see cref="ControlManagerCreator"/> delegate is created.
            /// that returns this control.
            ///</summary>
            ///<exception cref="NotImplementedException"></exception>
            public IControlHabanero ManagedControl
            {
                set
                {
                    if (value == null)
                    {
                        this.ControlManagerCreator = null;
                        return;
        }
                    var menuControlManagerDefault = new MenuControlManagerDefault(value);
                    this.ControlManagerCreator = delegate(IControlFactory factory) { return menuControlManagerDefault; };
    }
            }
        }

        /// <summary>
        /// A Default Control Manager used for a Menu.
        /// This provides a shortcut to using the <see cref="ControlManagerCreator"/> delegate
        /// when all you want to do is return the exact same control each time 
        /// the menu is clicked
        /// </summary>
        public class MenuControlManagerDefault : IControlManager
        {
            /// <summary>
            /// Create a control manager with the single control that it is managing.
            /// </summary>
            /// <param name="control"></param>
            public MenuControlManagerDefault(IControlHabanero control)
            {
                if (control == null) throw new ArgumentNullException("control");
                control.Enabled = true;
                Control = control;
            }

            public IControlHabanero Control { get; private set; }
        }
    }
}