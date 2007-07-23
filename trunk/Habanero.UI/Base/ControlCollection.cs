using System;
using System.Collections;
using System.Windows.Forms;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a collection of controls
    /// </summary>
    public class ControlCollection : CollectionBase
    {
        /// <summary>
        /// An indexing facility for the collection so that it can be
        /// accessed like an array with square brackets
        /// </summary>
        /// <param name="index">The numerical index position</param>
        /// <returns>Returns the control at the position specified</returns>
        public Control this[int index]
        {
            get { return ((Control) List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds a control to the collection
        /// </summary>
        /// <param name="value">The control to add</param>
        /// <returns>Returns the position at which the control was added</returns>
        public int Add(Control value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Provides the index position of the control specified
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns the index position if found, or -1</returns>
        public int IndexOf(Control value)
        {
            return (List.IndexOf(value));
        }

        /// <summary>
        /// Insert a control at a specified index position
        /// </summary>
        /// <param name="index">The index position at which to insert</param>
        /// <param name="value">The control to insert</param>
        public void Insert(int index, Control value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified control from the collection
        /// </summary>
        /// <param name="value">The control to remove</param>
        public void Remove(Control value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Indicates whether the collection contains the specified control
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns a boolean indicating whether that control is 
        /// found in the collection</returns>
        public bool Contains(Control value)
        {
            return (List.Contains(value));
        }

        /// <summary>
        /// Carries out additional checks and preparation before inserting
        /// a new control
        /// </summary>
        /// <param name="index">The position at which to insert</param>
        /// <param name="value">The control to insert</param>
        protected override void OnInsert(int index, Object value)
        {
            if (!(value == null) && !(value is Control))
            {
                throw new ArgumentException("value must be of type Control.", "value");
            }
        }

        /// <summary>
        /// Carries out additional checks and preparation when removing 
        /// a control
        /// </summary>
        /// <param name="index">The index position at which the control 
        /// may be found</param>
        /// <param name="value">The control to remove</param>
        protected override void OnRemove(int index, Object value)
        {
            if (!(value is Control))
            {
                throw new ArgumentException("value must be of type Control.", "value");
            }
        }

        /// <summary>
        /// Carries out additional checks and preparation before allowing
        /// a control in the collection to be reassigned
        /// </summary>
        /// <param name="index">The index position at which the control
        /// may be found</param>
        /// <param name="oldValue">The previous control</param>
        /// <param name="newValue">The new control to replace it</param>
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (!(newValue is Control))
            {
                throw new ArgumentException("newValue must be of type Control.", "newValue");
            }
        }

        /// <summary>
        /// Checks that the specified object is a type of Control and not null
        /// </summary>
        /// <param name="value">The control to validate</param>
        /// <exception cref="ArgumentException">Thrown if validation fails
        /// </exception>
        protected override void OnValidate(Object value)
        {
            if (!(value == null) && !(value is Control))
            {
                throw new ArgumentException("value must be of type Control.");
            }
        }
    }
}