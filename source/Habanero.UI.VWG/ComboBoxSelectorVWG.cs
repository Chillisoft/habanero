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
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Provides an implementation of <see cref="IBOComboBoxSelector"/> that is specialised for showing a collection of 
    /// Business Objects in a <see cref="IComboBox"/> and allowing the user to select one.
    /// </summary>
    public class ComboBoxSelectorVWG : ComboBoxVWG, IBOComboBoxSelector
    {
        private readonly ComboBoxCollectionSelector _manager;

        ///<summary>
        /// Constructor for <see cref="ComboBoxSelectorVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public ComboBoxSelectorVWG(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _manager = new ComboBoxCollectionSelector(this, controlFactory);
            _manager.BusinessObjectSelected += delegate { FireBusinessObjectSelected(); };
        }

        ///<summary>
        /// Constructor for <see cref="ComboBoxSelectorVWG"/>
        ///</summary>
        public ComboBoxSelectorVWG()
            : this(GlobalUIRegistry.ControlFactory)
        {
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }
        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return _manager.Collection; }
            set { _manager.SetCollection(value, true); }
        }

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get { return _manager.SelectedBusinessObject; }
            set
            {
                _manager.SelectedBusinessObject = value;
                if (value == null)
                {
                    this.SelectedValue = null;
                    this.Text = null;
        }
            }
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            _manager.Clear();
        }

        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { return this.Items.Count; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            if (IndexOutOfRange(row)) return null;
            return (IBusinessObject) this.Items[row];
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem
        {
            get { return this._manager.AutoSelectFirstItem; }
            set { this._manager.AutoSelectFirstItem = value; }
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }

        ///<summary>
        /// Returns the Underlying ComboBoxControl that is used by this selector
        ///</summary>
        public IComboBox ComboBox
        {
            get { return this; }
        }

        ///<summary>
        /// Gets or sets whether the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> should be preserved in the selector when the 
        /// <see cref="IBOColSelectorControl.BusinessObjectCollection">BusinessObjectCollection</see> 
        /// is changed to a new collection which contains the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see>.
        /// If the <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> doesn't exist in the new collection then the
        /// <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is set to null.
        /// If the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is null then this will also be preserved.
        /// This overrides the <see cref="IBOColSelectorControl.AutoSelectFirstItem">AutoSelectFirstItem</see> property.
        ///</summary>
        public bool PreserveSelectedItem
        {
            get { return _manager.PreserveSelectedItem; }
            set { _manager.PreserveSelectedItem = value; }
        }
    }
}