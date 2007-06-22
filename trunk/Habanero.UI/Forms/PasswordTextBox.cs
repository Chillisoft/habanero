using System.Windows.Forms;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// Provides a variation of a standard TextBox for passwords, masking
    /// the letters typed with a '*'
    /// </summary>
    public class PasswordTextBox : TextBox
    {
        /// <summary>
        /// Constructor to initialise the TextBox
        /// </summary>
        public PasswordTextBox()
        {
            this.PasswordChar = '*';
        }
    }
}