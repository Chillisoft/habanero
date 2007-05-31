using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can edit an integer value
    /// </summary>
    public class InputBoxInteger : InputBoxNumeric
    {
        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="defaultValue">The default integer value to display</param>
        public InputBoxInteger(string message, int defaultValue) : base(message)
        {
            itsNumericUpDown.Value = defaultValue;
        }

        /// <summary>
        /// Returns the integer value from the form
        /// </summary>
        /// TODO ERIC - definitely need a set here
        public int Value
        {
            get { return Convert.ToInt32(itsNumericUpDown.Value); }
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