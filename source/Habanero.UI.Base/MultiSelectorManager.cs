using System.Collections.Generic;

namespace Habanero.UI.Base
{
    public class MultiSelectorManager<T>

    {
        private readonly IMultiSelector<T> _multiSelector;
        private readonly MultiSelectorModel<T> _model;

        //TODO: add double click behaviour

        public MultiSelectorManager(IMultiSelector<T> multiSelector)
        {
            _multiSelector = multiSelector;
            _model = new MultiSelectorModel<T>();
            _model.OptionAdded +=
                delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e) { AvailableOptionsListBox.Items.Add(e.Item); };

            _model.OptionRemoved +=
                delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e) { AvailableOptionsListBox.Items.Remove(e.Item); };

            _model.OptionsChanged += delegate { UpdateListBoxes(); };

            _model.SelectionsChanged += delegate { UpdateListBoxes(); };

            _model.Selected += delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e)
                                   {
                                       AvailableOptionsListBox.Items.Remove(e.Item);
                                       SelectionsListBox.Items.Add(e.Item);
                                   };
            _model.Deselected += delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e)
                                     {
                                         AvailableOptionsListBox.Items.Add(e.Item);
                                         SelectionsListBox.Items.Remove(e.Item);
                                     };

            IButton selectButton = GetButton(MultiSelectorButton.Select);
            selectButton.Click += delegate
                                      {
                                          List<T> items = new List<T>();
                                          foreach (T item in AvailableOptionsListBox.SelectedItems) items.Add(item);
                                          _model.Select(items);
                                      };

            IButton deselectButton = GetButton(MultiSelectorButton.Deselect);
            deselectButton.Click += delegate
                                        {
                                            List<T> items = new List<T>();
                                            foreach (T item in SelectionsListBox.SelectedItems) items.Add(item);
                                            _model.Deselect(items);
                                        };

            IButton selectAllButton = GetButton(MultiSelectorButton.SelectAll);
            selectAllButton.Click += delegate { _model.SelectAll(); };

            IButton deselectAllButton = GetButton(MultiSelectorButton.DeselectAll);
            deselectAllButton.Click += delegate { _model.DeselectAll(); };
        }


        private void UpdateListBoxes()
        {
            AvailableOptionsListBox.Items.Clear();
            _model.AvailableOptions.ForEach(delegate(T obj) { AvailableOptionsListBox.Items.Add(obj); });
            SelectionsListBox.Items.Clear();
            foreach (T obj in _model.SelectionsView)
            {
                SelectionsListBox.Items.Add(obj);
            }
        }

        public List<T> Options
        {
            set { _model.Options = value; }
        }

        private IListBox AvailableOptionsListBox
        {
            get { return _multiSelector.AvailableOptionsListBox; }
        }

        public MultiSelectorModel<T> Model
        {
            get { return _model; }
        }

        public List<T> Selections
        {
            set { _model.Selections = value; }
        }

        private IListBox SelectionsListBox
        {
            get { return _multiSelector.SelectionsListBox; }
        }

        private IButton GetButton(MultiSelectorButton buttonType)
        {
            return _multiSelector.GetButton(buttonType);
        }
    }
}