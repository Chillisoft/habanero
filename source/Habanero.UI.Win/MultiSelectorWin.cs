using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class MultiSelectorWin<T> : UserControl, IMultiSelector<T>
    {
       
        private readonly MultiSelectorManager<T> _manager;

        public MultiSelectorWin()
        {
            InitializeComponent();
            _manager = new MultiSelectorManager<T>(this);
            AvailableOptionsListBox.SelectedIndexChanged += delegate
            {
                GetButton(MultiSelectorButton.Select).Enabled = (AvailableOptionsListBox.SelectedIndex != -1);
            };

            SelectionsListBox.SelectedIndexChanged += delegate
            {
                GetButton(MultiSelectorButton.Deselect).Enabled = (SelectionsListBox.SelectedIndex != -1);
            };
        }

        public List<T> Options
        {
            set
            {
                _manager.Options = value;
                GetButton(MultiSelectorButton.Select).Enabled = false;
            }
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
            set
            {
                _manager.Selections = value;
                GetButton(MultiSelectorButton.Deselect).Enabled = false;
            }
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
