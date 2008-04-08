using System;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can edit an integer value
    /// </summary>
    public class InputFormInteger : InputFormNumeric
    {
        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="defaultValue">The default integer value to display</param>
        public InputFormInteger(string message, int defaultValue) : base(message)
        {
            _numericUpDown.Value = defaultValue;
        }

        /// <summary>
        /// Returns the integer value from the form
        /// </summary>
        /// TODO ERIC - have a set here?
        public int Value
        {
            get { return Convert.ToInt32(_numericUpDown.Value); }
        }

        /// <summary>
        /// Creates a numeric up-down integer control for the form
        /// </summary>
        /// <returns>Returns the NumericUpDown control created</returns>
        protected override NumericUpDown CreateNumericUpDown()
        {
            return ControlFactory.CreateNumericUpDownInteger();
        }
    }
}