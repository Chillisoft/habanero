using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Habanero.UI.Base
{
    public enum MultiSelectorButton
    {
        Select,
        Deselect,
        SelectAll,
        DeselectAll
    }
    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.
    /// </summary>
    public interface IMultiSelector<T> :IControlChilli
    {
        List<T> Options { get; set; }

        IListBox AvailableOptionsListBox { get; }

        MultiSelectorModel<T> Model { get; }
        ///<summary>
        /// Gets or sets the list of selectioned items (i.e. the items in the right hand listbox
        ///</summary>
        List<T> Selections { get; set; }
        IListBox SelectionsListBox { get; }
        IButton GetButton(MultiSelectorButton buttonType);
        ReadOnlyCollection<T> SelectionsView { get;}
    }
}
