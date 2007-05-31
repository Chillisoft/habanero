using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
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