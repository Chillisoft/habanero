using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public class MultiSelectorManager<T>

    {
        private readonly IControlFactory _controlFactory;
        private readonly IListBox _availableOptionsListBox;
        private readonly MultiSelectorModel<T> _model;
        private readonly IListBox _selectionsListBox;


        public MultiSelectorManager(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _availableOptionsListBox = _controlFactory.CreateListBox();
            _selectionsListBox = _controlFactory.CreateListBox();
            _model = new MultiSelectorModel<T>();


            _model.OptionAdded += delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e)
            {
                AvailableOptionsListBox.Items.Add(e.Item);

            };

            _model.OptionRemoved += delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e)
            {
                AvailableOptionsListBox.Items.Remove(e.Item);
            };

            _model.OptionsChanged +=delegate(object sender, EventArgs e)
                                        {
                                            UpdateListBoxes();
                                        };

            _model.SelectionsChanged+=delegate(object sender, EventArgs e)
                                          {
                                              UpdateListBoxes();
                                          };
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
            set
            {
                _model.Options = value;
            }
        }

        public IListBox AvailableOptionsListBox
        {
            get { return _availableOptionsListBox; }
        }

        public MultiSelectorModel<T> Model
        {
            get { return _model; }
        }

        public List<T> Selections
        {
            set { _model.Selections = value; }
        }

        public IListBox SelectionsListBox
        {
            get { return _selectionsListBox; }
        }
    }
}