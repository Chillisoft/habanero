using System.Windows.Forms;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// An interface to model an interface control for a form
    /// </summary>
    /// TODO ERIC - rename to IFormControl
    public interface FormControl
    {
        /// <summary>
        /// Sets the form to control
        /// </summary>
        /// <param name="form">The form to control</param>
        void SetForm(Form form);
    }
}