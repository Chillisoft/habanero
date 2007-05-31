using System;
using System.Collections;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Maps a ComboBox that display a list of options
    /// </summary>
    /// TODO ERIC - clarify the difference between the combo box types
    /// - is list just a straight 1-d array and lookup a string-guid col?
    public class ListComboBoxMapper : ControlMapper
    {
        private ComboBox itsComboBox;
        IList itsList;

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
            itsComboBox = cbx;
            itsComboBox.SelectedIndexChanged += new EventHandler(ValueChangedHandler);
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
            if (itsBusinessObject != null && itsComboBox.SelectedIndex != -1)
            {
                string newValue = (string)itsComboBox.SelectedItem;
                if (newValue == null || newValue == "") 
                {
                    if (itsBusinessObject.GetPropertyValue(itsPropertyName) != null)
                    {
                        itsBusinessObject.SetPropertyValue(itsPropertyName, null);
                    }
                }
                else if (itsBusinessObject.GetPropertyValue(itsPropertyName) == null ||
                         !newValue.Equals(itsBusinessObject.GetPropertyValue(itsPropertyName)))
                {
                    itsBusinessObject.SetPropertyValue(itsPropertyName, newValue);
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
            if (itsList == null)
            {
                SetupList();
            }
            itsComboBox.SelectedItem = itsBusinessObject.GetPropertyValueString(itsPropertyName);
        }

        /// <summary>
        /// Set up the list of objects in the combo box.  This method
        /// automatically calls SetList().
        /// </summary>
        private void SetupList()
        {
            itsList = new ArrayList();
            string optionList = (string)itsAttributes["options"];
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
            itsList = new ArrayList();
            if (includeBlank) itsList.Add("");
            int width = itsComboBox.Width;
            Label lbl = ControlFactory.CreateLabel("", false);
            if (optionList != null && optionList.Length > 0)
            {
                string[] options = optionList.Split(new Char[] {';'});
                foreach (string s in options)
                {
                    if (s.Trim().Length > 0)
                    {
                        itsList.Add(s);
                    }
                }
            }
            itsComboBox.Items.Clear();
            foreach (string str in itsList)
            {
                lbl.Text = str;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                itsComboBox.Items.Add(str);
            }
            itsComboBox.DropDownWidth = width;
        }
    }
}