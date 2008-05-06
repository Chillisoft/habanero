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

        private readonly Dictionary<MultiSelectorButton, IButton> _buttons;


        public MultiSelectorManager(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _availableOptionsListBox = _controlFactory.CreateListBox();
            _selectionsListBox = _controlFactory.CreateListBox();
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

            _buttons = new Dictionary<MultiSelectorButton, IButton>();
            IButton selectButton = _controlFactory.CreateButton();
            selectButton.Click += delegate(object sender, EventArgs e)
            {
                //List<T> items = new List<T>();
                //foreach (T item in AvailableOptionsListBox.SelectedItems) items.Add(item);
                _model.Select((T)AvailableOptionsListBox.SelectedItem);
            };
            _buttons.Add(MultiSelectorButton.Select, selectButton);

            AvailableOptionsListBox.SelectedIndexChanged += delegate
            {
                selectButton.Enabled = (AvailableOptionsListBox.SelectedIndex != -1);
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
            set { _model.Options = value;
                _buttons[MultiSelectorButton.Select].Enabled = false; }
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

        public IButton GetButton(MultiSelectorButton button)
        {
            return _buttons[button];
        }
    }
}