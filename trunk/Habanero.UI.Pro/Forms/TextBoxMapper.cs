using System;
using System.Windows.Forms;
using Habanero.Ui.Forms;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Maps a TextBox object in a user interface
    /// </summary>
    public class TextBoxMapper : ControlMapper
    {
        private TextBox _textBox;
        private string _oldText;

        /// <summary>
        /// Constructor to initialise a new instance of the mapper
        /// </summary>
        /// <param name="tb">The TextBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public TextBoxMapper(TextBox tb, string propName, bool isReadOnceOnly) : base(tb, propName, isReadOnceOnly)
        {
            _textBox = tb;
            //_textBox.Enabled = false;
            _textBox.TextChanged += new EventHandler(ValueChangedHandler);
            _oldText = "";
        }

        /// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValueChangedHandler(object sender, EventArgs e)
        {
            if (_businessObject != null && !_isReadOnceOnly)
            {
                try
                {
                    _businessObject.SetPropertyValue(_propertyName, _textBox.Text);
                }
                catch (FormatException)
                {
                    _textBox.Text = _oldText;
                }
                _oldText = _textBox.Text;
            }
        }

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            _textBox.Text = Convert.ToString(this.GetPropertyValue());
        }
    }
}