using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// This is a control that can be placed ona a form.
    /// Provides an implementation of <see cref="IBOComboBoxSelector"/> that is specialised for showing a collection of 
    /// Business Objects (<see cref="IBusinessObjectCollection"/>) in a <see cref="IComboBox"/> and allowing the user to select one.
    /// This Control works in conjunction with the <see cref="ComboBoxCollectionSelector"/>.
    /// </summary>
    public class ComboBoxSelectorWin : ComboBoxWin, IBOComboBoxSelector
    {
        private readonly ComboBoxCollectionSelector _manager;

        ///<summary>
        /// Constructor for <see cref="ComboBoxSelectorWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public ComboBoxSelectorWin(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _manager = new ComboBoxCollectionSelector(this, controlFactory);
            _manager.BusinessObjectSelected += delegate { FireBusinessObjectSelected(); };
        }

        ///<summary>
        /// Constructor for <see cref="ComboBoxSelectorWin"/>
        ///</summary>
        public ComboBoxSelectorWin() : this(GlobalUIRegistry.ControlFactory)
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
            set { _manager.SelectedBusinessObject = value; }
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

        ///<summary>
        /// Returns the Underlying ComboBoxControl that is used by this selector
        ///</summary>
        public IComboBox ComboBox
        {
            get { return this; }
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
            get { return _manager.AutoSelectFirstItem; }
            set { _manager.AutoSelectFirstItem = value; }
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }
    }
}