//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This class provides mapping from an explicit list of options to a
    /// user interface ComboBox.  This mapper is used when you have specified
    /// an "options" parameter for a ui field in the class definitions.
    /// </summary>
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
		/// <param name="isReadOnly">Whether this control is read only</param>
		public ListComboBoxMapper(ComboBox cbx, string propName, bool isReadOnly)
            : base(cbx, propName, isReadOnly)
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
                        SetPropertyValue(null);
                        //_businessObject.SetPropertyValue(_propertyName, null);
                    }
                }
                else if (_businessObject.GetPropertyValue(_propertyName) == null ||
                         !newValue.Equals(_businessObject.GetPropertyValue(_propertyName)))
                {
                    SetPropertyValue(newValue);
                    //_businessObject.SetPropertyValue(_propertyName, newValue);
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
            _comboBox.SelectedItem = GetPropertyValue();// _businessObject.GetPropertyValue(_propertyName);
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