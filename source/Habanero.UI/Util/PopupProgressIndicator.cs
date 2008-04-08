using System.Threading;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Util
{
    /// <summary>
    /// Provides a form that displays a progress indicator to the user
    /// </summary>
    public class PopupProgressIndicator : IProgressIndicator
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
            private ProgressBar _progressIndicator;
            private Label _descriptionLabel;

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
                _descriptionLabel = ControlFactory.CreateLabel(message, false);
                manager.AddControl(_descriptionLabel);
                _progressIndicator = ControlFactory.CreateProgressBar();
                manager.AddControl(_progressIndicator);
                contentPanel.Dock = DockStyle.Fill;
                this.Controls.Add(contentPanel);
                _progressIndicator.Maximum = 100;
                _progressIndicator.Minimum = 0;
                this.Text = message;
            }

            /// <summary>
            /// Sets the progress as a percentage
            /// </summary>
            /// <param name="percentage">The progress percentage</param>
            public void SetProgress(int percentage)
            {
                _progressIndicator.Value = percentage;
            }

            /// <summary>
            /// Sets the description to display
            /// </summary>
            /// <param name="description">The description</param>
            public void SetDescription(string description)
            {
                _descriptionLabel.Text = description;
            }
        }
    }
}