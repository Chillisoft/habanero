using System.Windows.Forms;

namespace Habanero.Ui.Misc
{
    /// <summary>
    /// An interface to model an interface control for a form
    /// </summary>
    public interface IFormControl
    {
        /// <summary>
        /// Sets the form to control
        /// </summary>
        /// <param name="form">The form to control</param>
        void SetForm(Form form);
    }
}