using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class MultiSelectorWin<T> : UserControl, IMultiSelector<T>
    {
        private readonly MultiSelectorManager<T> _manager;

        public MultiSelectorWin(IControlFactory controlFactory)
        {
            _manager= new MultiSelectorManager<T>(controlFactory);
        }

        public List<T> Options
        {
            set { _manager.Options = value; }
        }

        public IListBox AvailableOptionsListBox
        {
            get { return _manager.AvailableOptionsListBox; }
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
            get { return _manager.SelectionsListBox; }
        }
    }
}