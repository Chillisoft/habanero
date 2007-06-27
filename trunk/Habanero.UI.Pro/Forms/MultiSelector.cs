using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.
    /// </summary>
    public partial class MultiSelector<T> : UserControl
    {
        private Model _model;

        /// <summary>
        /// A constructor to initialise a new selector
        /// </summary>
        public MultiSelector() {
            InitializeComponent();
            _model = new Model();
			_model.OptionsChanged +=delegate 
			{
				UpdateListBoxes();
            };

			_model.SelectionsChanged += delegate
			{
				UpdateListBoxes();
			};

			_model.OptionAdded += delegate(object sender, Model.ModelEventArgs<T> e)
			{
				 AvailableOptionsListBox.Items.Add(e.Item);
				 UpdateButtonsStatus();
            };

			_model.OptionRemoved += delegate(object sender, Model.ModelEventArgs<T> e)
        	{
        		AvailableOptionsListBox.Items.Remove(e.Item);
        		UpdateButtonsStatus();
        	};

			_model.Selected += delegate(object sender, Model.ModelEventArgs<T> e)
           	{
           		AvailableOptionsListBox.Items.Remove(e.Item);
           		SelectionsListBox.Items.Add(e.Item);
           		SelectionsListBox.SelectedItem = e.Item;
           		UpdateButtonsStatus();
           	};

			_model.Deselected += delegate(object sender, Model.ModelEventArgs<T> e)
         	{
         		AvailableOptionsListBox.Items.Add(e.Item);
         		AvailableOptionsListBox.SelectedItem = e.Item;
         		SelectionsListBox.Items.Remove(e.Item);
         		UpdateButtonsStatus();
         	};

			AvailableOptionsListBox.SelectedIndexChanged += delegate
        	{
        		SelectButton.Enabled = (AvailableOptionsListBox.SelectedIndex != -1);
        	};

            SelectionsListBox.SelectedIndexChanged += delegate
          	{
          		DeselectButton.Enabled = (SelectionsListBox.SelectedIndex != -1);
          	};
        }

        /// <summary>
        /// Updates the enabled status of the buttons
        /// </summary>
        private void UpdateButtonsStatus() {
            SelectAllButton.Enabled = (AvailableOptionsListBox.Items.Count > 0);
            DeselectAllButton.Enabled = (SelectionsListBox.Items.Count > 0);
        }

        /// <summary>
        /// Returns this selector's instance of the Model
        /// </summary>
        public Model ModelInstance
        {
            get { return _model; }
        }

        /// <summary>
        /// Sets the list of options, initially filling the "Available 
        /// Options" listbox with all the options
        /// </summary>
		public List<T> Options
		{
			set
			{
				_model.Options = value;
				//UpdateListBoxes();
			}
		}


    	///<summary>
        /// Gets or sets the list of selections
        ///</summary>
        public List<T> Selections {
            set {
                _model.Selections = value;
				//SelectionsListBox.Items.Clear();
				//foreach (T obj in _model.SelectionsView) SelectionsListBox.Items.Add(obj);
				//UpdateButtonsStatus();
            }
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
			UpdateButtonsStatus();
		}

        /// <summary>
        /// Handles the event of the "Select" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectButton_Click(object sender, EventArgs e) {
            DoSelect();
        }

        /// <summary>
        /// Handles the event of the "Deselect" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DeselectButton_Click(object sender, EventArgs e) {
            DoDeselect();
        }

        /// <summary>
        /// Handles the event of the "Select All" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectAllButton_Click(object sender, EventArgs e) {
            _model.SelectAll();
        }

        /// <summary>
        /// Handles the event of the "Deselect All" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DeselectAllButton_Click(object sender, EventArgs e) {
            _model.DeselectAll();
        }

        /// <summary>
        /// Handles the event of a mouse double-click on the available
        /// options list
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AvailableOptionsListBox_MouseDoubleClick(object sender, MouseEventArgs e) {
            DoSelect();
        }

        /// <summary>
        /// Handles the event of a mouse double-click on the selections list
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectionsListBox_MouseDoubleClick(object sender, MouseEventArgs e) {
            DoDeselect();
        }

        /// <summary>
        /// Carries out a selection
        /// </summary>
        private void DoSelect() {
            List<T> items = new List<T>();
            foreach (T item in AvailableOptionsListBox.SelectedItems) items.Add(item);
            _model.Select(items);
        }

        /// <summary>
        /// Carries out a deselection
        /// </summary>
        private void DoDeselect() {
            List<T> items = new List<T>();
            foreach (T item in SelectionsListBox.SelectedItems) items.Add(item);
            _model.Deselect(items);
        }


        /// <summary>
        /// The model for the multiselector control. The type of the items in 
        /// the lists is set by the template type.
        /// </summary>
        public class Model {
            private List<T> _options;
            private List<T> _selections;
            private List<T> _originalSelections;

			internal event EventHandler OptionsChanged;
			internal event EventHandler SelectionsChanged;
			internal event EventHandler<ModelEventArgs<T>> OptionAdded;
			internal event EventHandler<ModelEventArgs<T>> OptionRemoved;
            internal event EventHandler<ModelEventArgs<T>> Selected;
            internal event EventHandler<ModelEventArgs<T>> Deselected;

            internal class ModelEventArgs<T> : EventArgs {
                private readonly T _item;

                public ModelEventArgs(T item) : base() {
                    _item = item;
                }

                public T Item { get { return _item; } }
            }

            /// <summary>
            /// Constructor to initialise a new model
            /// </summary>
            public Model() {
                _originalSelections = new List<T>();
                _selections = new List<T>();
                _options = new List<T>();
            }

            /// <summary>
            /// Sets the list of options (left hand side list).
            /// Note that this creates a shallow copy of the List.
            /// </summary>
			public List<T> Options
			{
				set
				{
					_options = ShallowCopy(value);
					for (int i = _selections.Count - 1; i >= 0; i--)
					{
						//Remove all selections that dont exist in the options list
						if (!_options.Contains(_selections[i]))
						{
							_selections.RemoveAt(i);
						}
					}
					FireOptionsChanged();
				}
			}

			private void FireOptionsChanged()
			{
				if (OptionsChanged != null) OptionsChanged(this, new EventArgs());
			}

			/// <summary>
            /// Returns a view of the Options collection
            /// </summary>
            public ReadOnlyCollection<T> OptionsView { get { return _options.AsReadOnly(); } }

            /// <summary>
            /// Sets the list of selected items (right hand side list).
            /// </summary>
			public List<T> Selections
			{
				set
				{
					_selections = value;
					_originalSelections = ShallowCopy(_selections);
					//_originalSelections = .GetRange(0, _selections.Count);
					FireSelectionsChanged();
				}
			}

			private void FireSelectionsChanged()
			{
				if (SelectionsChanged != null) SelectionsChanged(this, new EventArgs());
			}

            /// <summary>
            /// Returns a view of the Selections collection
            /// </summary>
            public ReadOnlyCollection<T> SelectionsView { get { return _selections.AsReadOnly(); } }


            private List<T> OriginalSelections { get { return _originalSelections; } }

            /// <summary>
            /// Returns the list of available options, which is the set 
            /// of Options minus the set of Selections
            /// </summary>
            public List<T> AvailableOptions { get { return _options.FindAll(delegate(T obj) { return !_selections.Contains(obj); }); } }

            /// <summary>
            /// Returns the list of added selections (items selected since 
            /// setting the selections)
            /// </summary>
            public List<T> Added { get { return _selections.FindAll(delegate(T obj) { return !OriginalSelections.Contains(obj); }); } }

            /// <summary>
            /// Returns the list of removed selections (items deselected 
            /// since setting the selections)
            /// </summary>
            public List<T> Removed { get { return OriginalSelections.FindAll(delegate(T obj) { return !_selections.Contains(obj); }); } }

            /// <summary>
            /// Selects multiple items at the same time.
            /// </summary>
            /// <param name="items">The list of items to select</param>
            public void Select(List<T> items) {
                items.ForEach(delegate(T obj) { Select(obj); });
            }

            /// <summary>
            /// Selects an option, removing it from the Options and adding 
            /// it to the Selections
            /// </summary>
            public void Select(T item) {
                if (_options.Contains(item) && !_selections.Contains(item)) {
                    _selections.Add(item);
                    FireSelected(item);
                }
            }
			
            private void FireSelected(T item) {
                if (Selected != null) Selected(this, new ModelEventArgs<T>(item));
            }

             /// <summary>
            /// Deselects a list of items at once
            /// </summary>
            /// <param name="items">The list of items to deselect</param>
            public void Deselect(List<T> items) {
                items.ForEach(delegate(T obj) { Deselect(obj); });
            }

           /// <summary>
            /// Deselects an option, removing it from the Selections and 
            /// adding it to the Options
            /// </summary>
            public void Deselect(T item) {
                if (_selections.Remove(item)) FireDeselected(item);
            }

            private void FireDeselected(T item) {
                if (Deselected != null) Deselected(this, new ModelEventArgs<T>(item));
            }

            /// <summary>
            /// Selects all available options
            /// </summary>
            public void SelectAll() {
				Select(AvailableOptions);
                //AvailableOptions.ForEach(delegate(T obj) { Select(obj); });
            }

            /// <summary>
            /// Deselects all options
            /// </summary>
            public void DeselectAll() {
				Deselect(ShallowCopy(_selections));
                //_selections.FindAll(delegate { return true; }).ForEach(delegate(T obj) { Deselect(obj); });
            }

            /// <summary>
            /// Adds an option to the collection of Options
            /// </summary>
            /// <param name="item"></param>
            public void AddOption(T item) {
                _options.Add(item);
                FireOptionAdded(item);
            }

            private void FireOptionAdded(T item) {
                if (OptionAdded != null) OptionAdded(this, new ModelEventArgs<T>(item));
            }

            ///<summary>
            /// Removes an option from the collection of options. 
            ///</summary>
            ///<param name="item">The item to remove</param>
            public void RemoveOption(T item) {
                _options.Remove(item);
                FireOptionRemoved(item);
            }

            private void FireOptionRemoved(T item) {
                if (OptionRemoved != null) OptionRemoved(this, new ModelEventArgs<T>(item));
            }

			private static List<T> ShallowCopy(List<T> list)
			{
				return list.FindAll(delegate { return true; });
			}

        }
    }
}