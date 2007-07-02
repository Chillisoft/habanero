using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Maps a ComboBox that display a list of options
    /// </summary>
    /// TODO ERIC - clarify the difference between the combo box types
    /// - is list just a straight 1-d array and lookup a string-guid col?
    public class ListComboBoxMapper : ControlMapper
    {
        private ComboBox _comboBox;
        IList _list;

        /// <summary>
        /// Constructor to initialise a new mapper, attaching a handler to
        /// deal with value changes
        /// </summary>
        /// <param name="cbx">The ComboBox object</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the control is read-once only
        /// </param>
        public ListComboBoxMapper(ComboBox cbx, string propName, bool isReadOnceOnly)
            : base(cbx, propName, isReadOnceOnly)
        {
            _comboBox = cbx;
            _comboBox.SelectedIndexChanged += new EventHandler(ValueChangedHandler);
        }

        /// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValueChangedHandler(object sender, EventArgs e)
        {
            //log.Debug("ValueChanged in LookupComboBoxMapper") ;
            if (_businessObject != null && _comboBox.SelectedIndex != -1)
            {
                string newValue = (string)_comboBox.SelectedItem;
                if (newValue == null || newValue == "") 
                {
                    if (_businessObject.GetPropertyValue(_propertyName) != null)
                    {
                        _businessObject.SetPropertyValue(_propertyName, null);
                    }
                }
                else if (_businessObject.GetPropertyValue(_propertyName) == null ||
                         !newValue.Equals(_businessObject.GetPropertyValue(_propertyName)))
                {
                    _businessObject.SetPropertyValue(_propertyName, newValue);
                }
            }
            //log.Debug("ValueChanged in LookupComboBoxMapper complete") ;
        }

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            if (_list == null)
            {
                SetupList();
            }
            _comboBox.SelectedItem = _businessObject.GetPropertyValueString(_propertyName);
        }

        /// <summary>
        /// Set up the list of objects in the combo box.  This method
        /// automatically calls SetList().
        /// </summary>
        private void SetupList()
        {
            _list = new ArrayList();
            string optionList = (string)_attributes["options"];
            SetList(optionList, false);
        }

        /// <summary>
        /// Populates the combo box with the list of items.<br/>
        /// NOTE: This method is called by SetupList() - rather call
        /// SetupList() when you choose to populate the combo box.
        /// </summary>
        /// <param name="optionList">The list of items to add</param>
        /// <param name="includeBlank">Whether to include a blank item at the
        /// top of the list</param>
        public void SetList(string optionList, bool includeBlank)
        {
            _list = new ArrayList();
            if (includeBlank) _list.Add("");
            int width = _comboBox.Width;
            Label lbl = ControlFactory.CreateLabel("", false);
            if (optionList != null && optionList.Length > 0)
            {
                string[] options = optionList.Split(new Char[] {'|'});
                foreach (string s in options)
                {
                    if (s.Trim().Length > 0)
                    {
                        _list.Add(s);
                    }
                }
            }
            _comboBox.Items.Clear();
            foreach (string str in _list)
            {
                lbl.Text = str;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                _comboBox.Items.Add(str);
            }
            _comboBox.DropDownWidth = width;
        }
    }
}