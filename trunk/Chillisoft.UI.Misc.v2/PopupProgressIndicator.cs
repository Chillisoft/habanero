using System.Threading;
using System.Windows.Forms;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form that displays a progress indicator to the user
    /// </summary>
    public class PopupProgressIndicator : ProgressIndicator
    {

        private PopupProgressForm _popupProgressForm;

        /// <summary>
        /// Constructor to initialise the indicator with a message to display
        /// </summary>
        /// <param name="message">The message to display</param>
        public PopupProgressIndicator(string message)
        {
            _popupProgressForm = new PopupProgressForm(message);
            _popupProgressForm.SetProgress(0);
            _popupProgressForm.Height = 100;
            _popupProgressForm.Width = 400;
            _popupProgressForm.Show();
            _popupProgressForm.BringToFront();
        }


        /// <summary>
        /// Updates the indicator with progress information
        /// </summary>
        /// <param name="amountComplete">The amount complete already</param>
        /// <param name="totalToComplete">The total amount to be completed</param>
        /// <param name="description">A description</param>
        public void UpdateProgress(int amountComplete, int totalToComplete, string description)
        {
				_popupProgressForm.SetProgress(amountComplete*100/totalToComplete);
                _popupProgressForm.SetDescription(description);
                _popupProgressForm.Refresh();

		}


            
        

        /// <summary>
        /// Completes the progess and closes the dialog
        /// </summary>
        public void Complete()
        {
            _popupProgressForm.Close();
        }
        
        /// <summary>
        /// Provides a form that displays a progress indicator
        /// </summary>
        private class PopupProgressForm : Form
        {
            private ProgressBar itsProgressIndicator;
            private Label itsDescriptionLabel;

            /// <summary>
            /// Constructor to initialise the form with a message to display
            /// </summary>
            /// <param name="message">The message to display</param>
            public PopupProgressForm(string message)
            {
                Panel contentPanel = new Panel();
                GridLayoutManager manager = new GridLayoutManager(contentPanel);
                manager.SetGridSize(2, 1);
                manager.FixAllRowsBasedOnContents();
                itsDescriptionLabel = ControlFactory.CreateLabel(message, false);
                manager.AddControl(itsDescriptionLabel);
                itsProgressIndicator = ControlFactory.CreateProgressBar();
                manager.AddControl(itsProgressIndicator);
                contentPanel.Dock = DockStyle.Fill;
                this.Controls.Add(contentPanel);
                itsProgressIndicator.Maximum = 100;
                itsProgressIndicator.Minimum = 0;
                this.Text = message;
            }

            /// <summary>
            /// Sets the progress as a percentage
            /// </summary>
            /// <param name="percentage">The progress percentage</param>
            public void SetProgress(int percentage)
            {
                itsProgressIndicator.Value = percentage;
            }

            /// <summary>
            /// Sets the description to display
            /// </summary>
            /// <param name="description">The description</param>
            public void SetDescription(string description)
            {
                itsDescriptionLabel.Text = description;
            }
        }
    }
}