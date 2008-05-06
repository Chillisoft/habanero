using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Gizmox
{
    internal class MultiSelectorGiz<T> : UserControl, IMultiSelector<T>
    {
        private readonly MultiSelectorManager<T> _manager;

        public MultiSelectorGiz(IControlFactory controlFactory)
        {
            _manager = new MultiSelectorManager<T>(controlFactory);

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