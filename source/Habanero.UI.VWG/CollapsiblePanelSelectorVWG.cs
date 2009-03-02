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

        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public int NoOfItems
        {
            get { throw new System.NotImplementedException(); }
        }

        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            throw new System.NotImplementedException();
        }
    }
}