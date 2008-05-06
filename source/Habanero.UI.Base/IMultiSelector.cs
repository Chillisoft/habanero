using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public enum MultiSelectorButton
    {
        Select
    }
    public interface IMultiSelector<T> 
    {
        List<T> Options { set; }

        IListBox AvailableOptionsListBox { get; }

        MultiSelectorModel<T> Model { get; }

        List<T> Selections { set; }

        IListBox SelectionsListBox { get; }
        IButton GetButton(MultiSelectorButton button);
    }
}
