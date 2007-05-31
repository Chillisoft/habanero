using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.
    /// </summary>
    public partial class MultiSelector<T> : UserControl
    {
        private Model itsModel;

        /// <summary>
        /// A constructor to initialise a new selector
        /// </summary>
        public MultiSelector() {
            InitializeComponent();
            itsModel = new Model();
            itsModel.OptionAdded += delegate(object sender, Model.ModelEventArgs<T> e) {
                                        AvailableOptionsListBox.Items.Add(e.Item);
                                        UpdateButtonsStatus();
                                    };

            itsModel.OptionRemoved += delegate(object sender, Model.ModelEventArgs<T> e) {
                                          AvailableOptionsListBox.Items.Remove(e.Item);
                                          UpdateButtonsStatus();
                                      };

            itsModel.Selected += delegate(object sender, Model.ModelEventArgs<T> e) {
                                     AvailableOptionsListBox.Items.Remove(e.Item);
                                     SelectionsListBox.Items.Add(e.Item);
                                     SelectionsListBox.SelectedItem = e.Item;
                                     UpdateButtonsStatus();
                                 };
            itsModel.Deselected += delegate(object sender, Model.ModelEventArgs<T> e) {
                                       AvailableOptionsListBox.Items.Add(e.Item);
                                       AvailableOptionsListBox.SelectedItem = e.Item;
                                       SelectionsListBox.Items.Remove(e.Item);
                                       UpdateButtonsStatus();
                                   };

            AvailableOptionsListBox.SelectedIndexChanged += delegate { SelectButton.Enabled = (AvailableOptionsListBox.SelectedIndex != -1); };
            SelectionsListBox.SelectedIndexChanged += delegate { DeselectButton.Enabled = (SelectionsListBox.SelectedIndex != -1); };
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
            get { return itsModel; }
        }

        /// <summary>
        /// Sets the list of options, initially filling the "Available 
        /// Options" listbox with all the options
        /// </summary>
        public List<T> Options {
            set {
                itsModel.Options = value;
                AvailableOptionsListBox.Items.Clear();
                itsModel.AvailableOptions.ForEach(delegate(T obj) { AvailableOptionsListBox.Items.Add(obj); });
                SelectionsListBox.Items.Clear();
                foreach (T obj in itsModel.SelectionsView)
                {
                    SelectionsListBox.Items.Add(obj);
                }
                UpdateButtonsStatus();
            }
        }

        ///<summary>
        /// Gets or sets the list of selections
        ///</summary>
        public List<T> Selections {
            set {
                itsModel.Selections = value;
                SelectionsListBox.Items.Clear();
                foreach (T obj in itsModel.SelectionsView) SelectionsListBox.Items.Add(obj);
                UpdateButtonsStatus();
            }
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
            itsModel.SelectAll();
        }

        /// <summary>
        /// Handles the event of the "Deselect All" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DeselectAllButton_Click(object sender, EventArgs e) {
            itsModel.DeselectAll();
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
            itsModel.Select(items);
        }

        /// <summary>
        /// Carries out a deselection
        /// </summary>
        private void DoDeselect() {
            List<T> items = new List<T>();
            foreach (T item in SelectionsListBox.SelectedItems) items.Add(item);
            itsModel.Deselect(items);
        }


        /// <summary>
        /// The model for the multiselector control. The type of the items in 
        /// the lists is set by the template type.
        /// </summary>
        public class Model {
            private List<T> itsOptions;
            private List<T> itsSelections;
            private List<T> itsOriginalSelections;

            internal event EventHandler<ModelEventArgs<T>> OptionAdded;
            internal event EventHandler<ModelEventArgs<T>> OptionRemoved;
            internal event EventHandler<ModelEventArgs<T>> Selected;
            internal event EventHandler<ModelEventArgs<T>> Deselected;

            internal class ModelEventArgs<T> : EventArgs {
                private readonly T itsItem;

                public ModelEventArgs(T item) : base() {
                    itsItem = item;
                }

                public T Item { get { return itsItem; } }
            }

            /// <summary>
            /// Constructor to initialise a new model
            /// </summary>
            public Model() {
                itsOriginalSelections = new List<T>();
                itsSelections = new List<T>();
                itsOptions = new List<T>();
            }

            /// <summary>
            /// Sets the list of options (left hand side list).
            /// Note that this creates a shallow copy of the List.
            /// </summary>
            public List<T> Options { set
            {
                itsOptions = value.FindAll(delegate { return true; });
                for (int i = itsSelections.Count - 1; i >= 0; i--)
                {
                    if (!itsOptions.Contains(itsSelections[i]))
                    {
                        itsSelections.RemoveAt(i);

                    }
                }
            } }

            /// <summary>
            /// Returns a view of the Options collection
            /// </summary>
            public ReadOnlyCollection<T> OptionsView { get { return new ReadOnlyCollection<T>(itsOptions); } }

            /// <summary>
            /// Sets the list of selected items (right hand side list).
            /// </summary>
            public List<T> Selections {
                set {
                    itsSelections = value;
                    itsOriginalSelections = itsSelections.GetRange(0, itsSelections.Count);
                }
            }

            /// <summary>
            /// Returns a view of the Selections collection
            /// </summary>
            public ReadOnlyCollection<T> SelectionsView { get { return new ReadOnlyCollection<T>(itsSelections); } }


            private List<T> OriginalSelections { get { return itsOriginalSelections; } }

            /// <summary>
            /// Returns the list of available options, which is the set 
            /// of Options minus the set of Selections
            /// </summary>
            public List<T> AvailableOptions { get { return itsOptions.FindAll(delegate(T obj) { return !itsSelections.Contains(obj); }); } }

            /// <summary>
            /// Returns the list of added selections (items selected since 
            /// setting the selections)
            /// </summary>
            public List<T> Added { get { return itsSelections.FindAll(delegate(T obj) { return !OriginalSelections.Contains(obj); }); } }

            /// <summary>
            /// Returns the list of removed selections (items deselected 
            /// since setting the selections)
            /// </summary>
            public List<T> Removed { get { return OriginalSelections.FindAll(delegate(T obj) { return !itsSelections.Contains(obj); }); } }

            /// <summary>
            /// Selects an option, removing it from the Options and adding 
            /// it to the Selections
            /// </summary>
            public void Select(T item) {
                if (itsOptions.Contains(item) && !itsSelections.Contains(item)) {
                    itsSelections.Add(item);
                    FireSelected(item);
                }
            }

            /// <summary>
            /// Selects multiple items at the same time.
            /// </summary>
            /// <param name="items">The list of items to select</param>
            public void Select(List<T> items) {
                items.ForEach(delegate(T obj) { Select(obj); });
            }

            private void FireSelected(T item) {
                if (Selected != null) Selected(this, new ModelEventArgs<T>(item));
            }

            /// <summary>
            /// Deselects an option, removing it from the Selections and 
            /// adding it to the Options
            /// </summary>
            public void Deselect(T item) {
                if (itsSelections.Remove(item)) FireDeselected(item);
            }

            /// <summary>
            /// Deselects a list of items at once
            /// </summary>
            /// <param name="items">The list of items to deselect</param>
            public void Deselect(List<T> items) {
                items.ForEach(delegate(T obj) { Deselect(obj); });
            }

            private void FireDeselected(T item) {
                if (Deselected != null) Deselected(this, new ModelEventArgs<T>(item));
            }

            /// <summary>
            /// Selects all available options
            /// </summary>
            public void SelectAll() {
                AvailableOptions.ForEach(delegate(T obj) { Select(obj); });
            }

            /// <summary>
            /// Deselects all options
            /// </summary>
            public void DeselectAll() {
                itsSelections.FindAll(delegate { return true; }).ForEach(delegate(T obj) { Deselect(obj); });
            }

            /// <summary>
            /// Adds an option to the collection of Options
            /// </summary>
            /// <param name="item"></param>
            public void AddOption(T item) {
                itsOptions.Add(item);
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
                itsOptions.Remove(item);
                FireOptionRemoved(item);
            }

            private void FireOptionRemoved(T item) {
                if (OptionRemoved != null) OptionRemoved(this, new ModelEventArgs<T>(item));
            }
        }
    }
}