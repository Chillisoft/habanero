using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Habanero.UI.Base
{
    /// <summary>
    /// The model for the multiselector control. The type of the items in 
    /// the lists is set by the template type.
    /// </summary>
    public class MultiSelectorModel<T>
    {
        private List<T> _options;
        private List<T> _selections;
        private List<T> _originalSelections;

        public event EventHandler OptionsChanged;
        public event EventHandler SelectionsChanged;
        public event EventHandler<ModelEventArgs<T>> OptionAdded;
        public event EventHandler<ModelEventArgs<T>> OptionRemoved;
        public event EventHandler<ModelEventArgs<T>> Selected;
        public event EventHandler<ModelEventArgs<T>> Deselected;

        ///<summary>
        /// The Event Arguements for the Multiselector Model
        ///</summary>
        ///<typeparam name="T"></typeparam>
        public class ModelEventArgs<T> : EventArgs
        {
            private readonly T _item;

            ///<summary>
            /// The constructor for the Event Arguements for the Multiselector Model
            ///</summary>
            ///<param name="item">The item in the model to which the event applies</param>
            public ModelEventArgs(T item)
            {
                _item = item;
            }

            public T Item
            {
                get { return _item; }
            }
        }

        /// <summary>
        /// Constructor to initialise a new model
        /// </summary>
        public MultiSelectorModel()
        {
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
                if (_selections != null)
                {
                    for (int i = _selections.Count - 1; i >= 0; i--)
                    {
                        //Remove all selections that dont exist in the options list
                        if (!_options.Contains(_selections[i]))
                        {
                            _selections.RemoveAt(i);
                        }
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
        public ReadOnlyCollection<T> OptionsView
        {
            get { return _options.AsReadOnly(); }
        }

        /// <summary>
        /// Sets the list of selected items (right hand side list).
        /// </summary>
        public List<T> Selections
        {
            set
            {
                _selections = value;
                if (value == null)
                {
                    _selections = new List<T>();
                }
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
        public ReadOnlyCollection<T> SelectionsView
        {
            get
            {
                if (_selections == null)
                {
                    return null;
                }
                return _selections.AsReadOnly();
            }
        }


        private List<T> OriginalSelections
        {
            get { return _originalSelections; }
        }

        /// <summary>
        /// Returns the list of available options, which is the set 
        /// of Options minus the set of Selections
        /// </summary>
        public List<T> AvailableOptions
        {
            get { return _options.FindAll(delegate(T obj) { return !_selections.Contains(obj); }); }
        }

        /// <summary>
        /// Returns the list of added selections (items selected since 
        /// setting the selections)
        /// </summary>
        public List<T> Added
        {
            get { return _selections.FindAll(delegate(T obj) { return !OriginalSelections.Contains(obj); }); }
        }

        /// <summary>
        /// Returns the list of removed selections (items deselected 
        /// since setting the selections)
        /// </summary>
        public List<T> Removed
        {
            get { return OriginalSelections.FindAll(delegate(T obj) { return !_selections.Contains(obj); }); }
        }

        /// <summary>
        /// Selects multiple items at the same time.
        /// </summary>
        /// <param name="items">The list of items to select</param>
        public void Select(List<T> items)
        {
            items.ForEach(delegate(T obj) { Select(obj); });
        }

        /// <summary>
        /// Selects an option, removing it from the Options and adding 
        /// it to the Selections
        /// </summary>
        public void Select(T item)
        {
            if (_options.Contains(item) && !_selections.Contains(item))
            {
                _selections.Add(item);
                FireSelected(item);
            }
        }

        private void FireSelected(T item)
        {
            if (Selected != null) Selected(this, new ModelEventArgs<T>(item));
        }

        /// <summary>
        /// Deselects a list of items at once
        /// </summary>
        /// <param name="items">The list of items to deselect</param>
        public void Deselect(List<T> items)
        {
            items.ForEach(delegate(T obj) { Deselect(obj); });
        }

        /// <summary>
        /// Deselects an option, removing it from the Selections and 
        /// adding it to the Options
        /// </summary>
        public void Deselect(T item)
        {
            if (_selections.Remove(item)) FireDeselected(item);
        }

        private void FireDeselected(T item)
        {
            if (Deselected != null) Deselected(this, new ModelEventArgs<T>(item));
        }

        /// <summary>
        /// Selects all available options
        /// </summary>
        public void SelectAll()
        {
            Select(AvailableOptions);
            //AvailableOptions.ForEach(delegate(T obj) { Select(obj); });
        }

        /// <summary>
        /// Deselects all options
        /// </summary>
        public void DeselectAll()
        {
            Deselect(ShallowCopy(_selections));
            //_selections.FindAll(delegate { return true; }).ForEach(delegate(T obj) { Deselect(obj); });
        }

        /// <summary>
        /// Adds an option to the collection of Options
        /// </summary>
        /// <param name="item"></param>
        public void AddOption(T item)
        {
            _options.Add(item);
            FireOptionAdded(item);
        }

        private void FireOptionAdded(T item)
        {
            if (OptionAdded != null) OptionAdded(this, new ModelEventArgs<T>(item));
        }

        ///<summary>
        /// Removes an option from the collection of options. 
        ///</summary>
        ///<param name="item">The item to remove</param>
        public void RemoveOption(T item)
        {
            Deselect(item);
            _options.Remove(item);
            FireOptionRemoved(item);
        }

        private void FireOptionRemoved(T item)
        {
            if (OptionRemoved != null) OptionRemoved(this, new ModelEventArgs<T>(item));
        }

        private static List<T> ShallowCopy(List<T> list)
        {
            return list.FindAll(delegate { return true; });
        }
    }
}