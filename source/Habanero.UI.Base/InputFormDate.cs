using System;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a form containing a DateTimePicker in order to get a single
    /// DateTime value back from a user
    /// </summary>
    public class InputFormDate
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _message;
        private IDateTimePicker _dateTimePicker;

        public InputFormDate(IControlFactory controlFactory, string message)
        {
            _controlFactory = controlFactory;
            _message = message;
            _dateTimePicker = _controlFactory.CreateDateTimePicker(DateTime.Now);
        }

        /// <summary>
        /// Gets the DateTimePicker control
        /// </summary>
        public IDateTimePicker DateTimePicker
        {
            get { return _dateTimePicker; }
        }

        /// <summary>
        /// Gets the message to display to the user
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets or sets the DateTime value held in the DateTimePicker control
        /// </summary>
        public DateTime Value
        {
            get { return this.DateTimePicker.Value; }
            set { this.DateTimePicker.Value = value; }
        }

        /// <summary>
        /// Creates the panel on the form
        /// </summary>
        /// <returns>Returns the panel created</returns>
        public IPanel createControlPanel()
        {
            IPanel panel = _controlFactory.CreatePanel();
            ILabel label = _controlFactory.CreateLabel(_message, false);
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(panel, _controlFactory);
            flowLayoutManager.AddControl(label);
            flowLayoutManager.AddControl(_dateTimePicker);
            panel.Height = _dateTimePicker.Height + label.Height;
            panel.Width = _controlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            return panel;
        }

        //this is Currently untestable, the layout has been tested in the createControlPanel method.
        /// <summary>
        /// Shows the form to the user
        /// </summary>
        public DialogResult ShowDialog()
        {
            IPanel panel = createControlPanel();
            IOKCancelDialogFactory okCancelDialogFactory = _controlFactory.CreateOKCancelDialogFactory();
            IFormHabanero form = okCancelDialogFactory.CreateOKCancelForm(panel, "");
            return form.ShowDialog();
        }
    }
}