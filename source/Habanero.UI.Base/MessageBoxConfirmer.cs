using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// An <see cref="IConfirmer"/> that uses a MessageBox to get confirmation from the user.
    /// </summary>
    public class MessageBoxConfirmer : IConfirmer
    {
        ///<summary>
        /// The <see cref="IControlFactory"/> to use to create the MessageBox.
        ///</summary>
        public IControlFactory ControlFactory { get; private set; }

        ///<summary>
        /// The Title to display in the MessageBox.
        ///</summary>
        public string Title { get; private set; }

        ///<summary>
        /// The <see cref="MessageBoxIcon"/> to display in the MessageBox.
        ///</summary>
        public MessageBoxIcon MessageBoxIcon { get; private set; }

        ///<summary>
        /// Construct a <see cref="MessageBoxConfirmer"/> with the specified information.
        ///</summary>
        ///<param name="controlFactory">The <see cref="IControlFactory"/> to use to create the MessageBox.</param>
        ///<param name="title">The Title to display in the MessageBox.</param>
        ///<param name="messageBoxIcon">The <see cref="MessageBoxIcon"/> to display in the MessageBox.</param>
        public MessageBoxConfirmer(IControlFactory controlFactory, string title, MessageBoxIcon messageBoxIcon)
        {
            ControlFactory = controlFactory;
            Title = title;
            MessageBoxIcon = messageBoxIcon;
        }


        /// <summary>
        /// Gets confirmation from the user after providing them with an option
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>Returns true if the user confirms the choice and false
        /// if they decline the offer</returns>
        public bool Confirm(string message)
        {
            DialogResult dialogResult = ControlFactory.ShowMessageBox(message, Title, MessageBoxButtons.YesNo, MessageBoxIcon);
            return dialogResult == DialogResult.Yes;
        }

        ///<summary>
        /// Gets confirmation from the user after providing them with an option
        /// and executes the provided delegate once the user has responded.
        ///</summary>
        ///<param name="message">The message to display</param>
        ///<param name="confirmationDelegate">The delegate to execute once the user has responded.</param>
        ///<returns>Returns true if the user confirms the choice and false
        /// if they decline the offer</returns>
        public void Confirm(string message, ConfirmationDelegate confirmationDelegate)
        {
            DialogResult dialogResult = ControlFactory.ShowMessageBox(
                message, Title, MessageBoxButtons.YesNo, MessageBoxIcon,
                (sender, result) => confirmationDelegate(result == DialogResult.Yes));
        }
    }
}