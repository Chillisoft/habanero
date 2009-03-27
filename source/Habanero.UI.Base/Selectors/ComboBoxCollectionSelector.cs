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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This class provides mapping from a business object collection to a
    /// user interface ComboBox.  This mapper is used at code level when
    /// you are explicitly providing a business object collection.
    /// </summary>
    public class ComboBoxCollectionSelector
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructor to create a new collection ComboBox mapper object.
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="controlFactory">The control factory used to create controls</param>
        public ComboBoxCollectionSelector(IComboBox comboBox, IControlFactory controlFactory) : this(comboBox, controlFactory, true)
        {
        }

        /// <summary>
        /// Constructor to create a new collection ComboBox mapper object.
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="controlFactory">The control factory used to create controls</param>
        /// <param name="autoSelectFirstItem"></param>
        public ComboBoxCollectionSelector(IComboBox comboBox, IControlFactory controlFactory, bool autoSelectFirstItem)
        {
            if (comboBox == null) throw new ArgumentNullException("comboBox");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            Control = comboBox;
            AutoSelectFirstItem = autoSelectFirstItem;
            _controlFactory = controlFactory;
            Control.SelectedIndexChanged += delegate { FireBusinessObjectSelected(); };
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        /// <param name="includeBlank">Whether to a put a blank item at the
        /// top of the list</param>
		public void SetCollection(IBusinessObjectCollection collection, bool includeBlank)
        {
            //TODO Brett 26 Mar 2009: Register for Business Object updated event as well as prop updated.
            if (Collection != null)
            {
                Collection.BusinessObjectAdded -= BusinessObjectAddedHandler;
                Collection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
                Collection.BusinessObjectPropertyUpdated -= Collection_OnBusinessObjectUpdated;
            }
            Collection = collection;
            SetComboBoxCollection(Control, Collection, includeBlank);
            if (Collection == null) return;
            Collection.BusinessObjectAdded += BusinessObjectAddedHandler;
            Collection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
            Collection.BusinessObjectPropertyUpdated += Collection_OnBusinessObjectUpdated;
        }

        private void Collection_OnBusinessObjectUpdated(object sender, BOPropUpdatedEventArgs e)
        {
            IBusinessObject o = e.BusinessObject;
            int selectedIndex = this.Control.SelectedIndex;
            int indexOf = this.Control.Items.IndexOf(o);
            this.Control.Items.Remove(o);
            this.Control.Items.Insert(indexOf, o);
            this.Control.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
                    
        
        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            Control.Items.Remove(e.BusinessObject);
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            IComboBoxObjectCollection items = Control.Items;
            IBusinessObject businessObject = e.BusinessObject;
            if (businessObject == null) return;

//            log.Debug("BusinessObjectAddedHandler : ComboItems.Count = " + items.Count);
            if (items.Contains(businessObject)) return;
            if (string.IsNullOrEmpty(businessObject.ToString()))
            {
                string message = string.Format("Cannot add a business object of type '{0}' to the '{1}' if its ToString is null or zero length", businessObject.ClassDef.ClassName, this.GetType().Name);
                throw new HabaneroDeveloperException(message, message);
            }

            Control.Items.Add(e.BusinessObject);
        }

        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ComboBox list, or null if none is selected
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
                if (NoItemSelected() || NullItemSelected()) return null;

                return (BusinessObject)Control.SelectedItem;
            }
            set
            {
                Control.SelectedItem = ContainsValue(value) ? value : null;
            }
        }

        private bool ContainsValue(IBusinessObject value)
        {
            return (value != null && Control.Items.Contains(value));
        }

        private bool NullItemSelected()
        {
            return Control.SelectedItem is string && (Control.SelectedItem == null || (string)Control.SelectedItem == "");
        }

        private bool NoItemSelected()
        {
            return Control.SelectedIndex == -1;
        }

        /// <summary>
        /// Returns the ComboBox control
        /// </summary>
        public IComboBox Control { get; private set; }
        /// <summary>
        /// Must the first combo box item be auto selected or not. If it is autoselect then this item will be shown as selected
        /// when the combo box is loaded and the Selected event will be fired.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }

        /// <summary>
        /// Returns the control factory used to generate controls
        /// such as the label
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        /// <summary>
        /// Returns the collection used to populate the items shown in the ComboBox
        /// </summary>
        public IBusinessObjectCollection Collection { get; private set; }

        /// <summary>
        /// Set the list of objects in the ComboBox to a specific collection of
        /// business objects.<br/>
        /// Important: If you are changing the business object collection,
        /// use the SetBusinessObjectCollection method instead, which will call this method
        /// automatically.
        /// </summary>
        /// <param name="cbx">The ComboBox being controlled</param>
        /// <param name="col">The business object collection used to populate the items list</param>
        /// <param name="includeBlank">Whether to include a blank item at the
        /// top of the list</param>
        private void SetComboBoxCollection(IComboBox cbx, IBusinessObjectCollection col, bool includeBlank)
        {
            int width = cbx.Width;

            cbx.Items.Clear();
            int numBlankItems = 0;
            if (includeBlank)
            {
                cbx.Items.Add("");
                numBlankItems++;
            }
            if (col == null) return;

            //This is a bit of a hack but is used to get the 
            //width of the dropdown list when it drops down
            // uses the preferedwith calculation on the 
            //Label to do this. Makes drop down width equal to the 
            // width of the longest name shown.
            ILabel lbl = _controlFactory.CreateLabel("", false);
            foreach (IBusinessObject businessObject in col)
            {
                lbl.Text = businessObject.ToString();
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                cbx.Items.Add(businessObject);
            }
            if (col.Count > 0 && AutoSelectFirstItem) cbx.SelectedIndex = numBlankItems;
            cbx.DropDownWidth = width;
        }

        ///<summary>
        /// Clears all items in the Combo Box and sets the selected item and <see cref="Collection"/>
        /// to null
        ///</summary>
        public void Clear()
        {
            SetCollection (null, false);
        }
    }
}