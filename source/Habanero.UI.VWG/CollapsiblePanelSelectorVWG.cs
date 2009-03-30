using System;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    ///<summary>
    /// Provides an implementation of a control for
    /// the interface <see cref="IBOCollapsiblePanelSelector"/> for a control that specialises 
    /// in showing a list of 
    /// Business Objects <see cref="IBusinessObjectCollection"/>.
    /// This control shows each business object in its own collapsible Panel.
    /// This is a very powerfull control for easily adding or viewing a fiew items E.g. for 
    /// a list of addresses for a person.
    ///</summary>
    internal class CollapsiblePanelSelectorVWG : CollapsiblePanelGroupControlVWG, IBOCollapsiblePanelSelector
    {
        public CollapsiblePanelSelectorVWG(IControlFactory controlFactory)
        {

        }

        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IBusinessObject SelectedBusinessObject
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
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
            FireBusinessObjectSelected();
            throw new System.NotImplementedException();
        }

        private void FireBusinessObjectSelected()
        {
            BusinessObjectSelected(this, new BOEventArgs(null));
        }

        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}