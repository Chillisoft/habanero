using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can edit a monetary value
    /// </summary>
    public class InputFormMoney : InputFormNumeric
    {
        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="defaultValue">The default monetary value 
        /// to display</param>
        public InputFormMoney(string message, decimal defaultValue) : base(message)
        {
            _numericUpDown.Value = defaultValue;
        }

        /// <summary>
        /// Returns the monetary value from the form
        /// </summary>
        /// TODO ERIC - add a set here
        public decimal Value
        {
            get { return _numericUpDown.Value; }
        }

        /// <summary>
        /// Creates a monetary up-down integer control for the form
        /// </summary>
        /// <returns>Returns the NumericUpDown control created</returns>
        protected override NumericUpDown CreateNumericUpDown()
        {
            return ControlFactory.CreateNumericUpDownMoney();
        }
    }
}