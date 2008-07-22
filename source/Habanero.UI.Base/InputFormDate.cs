using System;

namespace Habanero.UI.Base
{
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

        public IDateTimePicker DateTimePicker
        {
            get { return _dateTimePicker; }
        }

        public string Message
        {
            get { return _message; }
        }

        public DateTime Value
        {
            get { return this.DateTimePicker.Value; }
            set { this.DateTimePicker.Value = value; }
        }

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
        public DialogResult ShowDialog()
        {
            IPanel panel = createControlPanel();
            IOKCancelDialogFactory okCancelDialogFactory = _controlFactory.CreateOKCancelDialogFactory();
            IFormChilli form = okCancelDialogFactory.CreateOKCancelForm(panel);
            return form.ShowDialog();
        }
    }
}