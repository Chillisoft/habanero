using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a message box giving the user the choice of proceeding with
    /// an option (such as deleting an object) or cancelling
    /// </summary>
    public class OKCancelConfirmer : IConfirmer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OKCancelConfirmer()
        {
            Permission.Check(this);
        }
        /// <summary>
        /// Gets confirmation from the user as to whether they would like to
        /// proceed and accept the choice given to them.  Displays a message box
        /// with "OK" and "Cancel" buttons.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>Returns true if the user accepts the offer by pressing
        /// "OK" and false if they decline by pressing "Cancel"</returns>
        public bool Confirm(string message)
        {
            return
                (MessageBox.Show(message, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1) == DialogResult.OK);
        }
    }
}