#region Using

using System;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

#endregion

namespace Habanero.UI.WebGUI
{
    public partial class MultiSelectorGiz<T> : UserControl, IMultiSelector<T>
    {

        private readonly MultiSelectorManager<T> _manager;

        public MultiSelectorGiz()
        {
            InitializeComponent();
            _manager = new MultiSelectorManager<T>(this);

        }

        public List<T> Options
        {
            set { _manager.Options = value; }
        }

        public IListBox AvailableOptionsListBox
        {
            get { return _availableOptionsListbox; }
        }

        public MultiSelectorModel<T> Model
        {
            get { return _manager.Model; }
        }

        public List<T> Selections
        {
            set { _manager.Selections = value; }
        }

        public IListBox SelectionsListBox
        {
            get { return _selectionsListbox; }
        }

        public IButton GetButton(MultiSelectorButton buttonType)
        {
            switch (buttonType)
            {
                case MultiSelectorButton.Select:
                    return _btnSelect;
              
                case MultiSelectorButton.Deselect:
                    return _btnDeselect;
                case MultiSelectorButton.SelectAll:
                    return _btnSelectAll;
                case MultiSelectorButton.DeselectAll:
                    return _btnDeselectAll;
                default:
                    throw new ArgumentOutOfRangeException("buttonType");
            }
        }
    }
}