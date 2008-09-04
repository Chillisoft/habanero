using Habanero.Base;
using Habanero.UI.Base;
using Habanero.Util;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    internal class TextBoxMapperStrategyWin : ITextBoxMapperStrategy
    {
        // Assumes that one strategy is created for each control.
        // These fields exist so that the IsValidCharacter method knows
        //   which prop and textbox it is dealing with
        private IBOProp _boProp;
        private TextBoxWin _textBox;

        /// <summary>
        /// Gets the BOProp being mapped through this control
        /// </summary>
        public IBOProp BoProp
        {
            get { return _boProp; }
        }

        /// <summary>
        /// Gets the textbox control for which the strategy is applied
        /// </summary>
        public TextBoxWin TextBoxControl
        {
            get { return _textBox; }
        }

        /// <summary>
        /// Adds key press event handlers that carry out actions like
        /// limiting the input of certain characters, depending on the type of the
        /// property
        /// </summary>
        /// <param name="mapper">The TextBox mapper</param>
        /// <param name="boProp">The property being mapped</param>
        public void AddKeyPressEventHandler(TextBoxMapper mapper, IBOProp boProp)
        {
            _boProp = boProp;
            if (mapper.Control is ITextBox)
            {
                TextBoxWin tb = (TextBoxWin) mapper.Control;
                tb.KeyPress += KeyPressEventHandler;
                _textBox = tb;
            }
        }

        private void KeyPressEventHandler(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!IsValidCharacter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Indicates if the given character being typed is valid, based on the
        /// text already entered in the textbox.  For instance, if the property
        /// type is an integer, this method will return false for a non-numeric
        /// character (apart from a negative sign).
        /// </summary>
        /// <param name="character">The character being input</param>
        /// <returns>Returns true if valid</returns>
        internal bool IsValidCharacter(char character)
        {
            if (BoProp == null) return true;
            if (TextBoxControl == null) return true;

            if (TypeUtilities.IsInteger(BoProp.PropertyType))
            {
                if ((character < '0' || character > '9') && character != 8 && character != '-')
                {
                    return false;
                }
                if (character == '-' && TextBoxControl.SelectionStart != 0)
                {
                    return false;
                }
            }
            else if (TypeUtilities.IsDecimal(BoProp.PropertyType))
            {
                if ((character < '0' || character > '9') && character != '.' && character != 8 && character != '-')
                {
                    return false;
                }
                if (character == '.' && TextBoxControl.Text.Contains("."))
                {
                    return false;
                }
                // In fact the char is valid, but we want the event to get handled in order to prevent double dots
                if (character == '.' && TextBoxControl.SelectionStart == 0)
                {
                    TextBoxControl.Text = "0." + TextBoxControl.Text;
                    TextBoxControl.SelectionStart = 2;
                    TextBoxControl.SelectionLength = 0;
                    return false;
                }
                if (character == '-' && TextBoxControl.SelectionStart != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}