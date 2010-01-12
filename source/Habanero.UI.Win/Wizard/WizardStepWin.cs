using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// A basic implementation of WizardStep that can be used for simply displaying a message.  
    /// Should a step be required that is a simple message for the user (such as at the end of a wizard), this step can be used
    /// </summary>
    public partial class WizardStepWin : UserControlWin, IWizardStep
    {
        /// <summary>
        /// Constructs the MessageWizardStep
        /// </summary>
        public WizardStepWin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises the step. Run when the step is reached.
        /// </summary>
        public virtual void InitialiseStep() { }

        /// <summary>
        /// Always returns true as this wizard step is simply for displaying a message to a user.
        /// </summary>
        /// <param name="message">Out parameter that will always be the empty string</param>
        /// <returns>true</returns>
        public virtual bool CanMoveOn(out string message)
        {
            message = "";
            return true;
        }

        /// <summary>
        /// Verifies whether the user can move back from this step.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanMoveBack()
        {
            return true;
        }

        public virtual void MoveOn()
        {
            //Do Nothing
        }

        public virtual void MoveBack()
        {
            //Do Nothing
        }

        /// <summary>
        /// The text that you want displayed at the top of the wizard control when this step is active.
        /// </summary>
        public virtual string HeaderText
        {
            get { return ""; }
        }

        /// <summary>
        /// Provides an interface for the developer to implement functionality to cancel any edits made as part of this
        /// wizard step. The default wizard controller functionality is to call all wizard steps cancelStep methods when
        /// its Cancel method is called.
        /// </summary>
        public virtual void CancelStep()
        {   
        }
    }
}