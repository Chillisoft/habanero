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
    public interface IMultiSelector<T> :IControlChilli
    {
        List<T> Options { set; }

        IListBox AvailableOptionsListBox { get; }

        MultiSelectorModel<T> Model { get; }
        List<T> Selections { set; }
        IListBox SelectionsListBox { get; }
        IButton GetButton(MultiSelectorButton buttonType);
        ReadOnlyCollection<T> SelectionsView { get;}
    }
}
