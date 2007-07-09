using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// This class provides mapping from a lookup-list to a
    /// user interface ComboBox.  This mapper is used when you have specified
    /// a lookup-list for a property definition in the class definitions.
    /// </summary>
    public class LookupComboBoxMapper : ComboBoxMapper
    {

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="cbx">The ComboBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public LookupComboBoxMapper(ComboBox cbx, string propName, bool isReadOnceOnly)
            : base(cbx, propName, isReadOnceOnly)
        {
            _comboBox = cbx;
            //_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBox.SelectedIndexChanged += new EventHandler(ValueChangedHandler);
            _comboBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
                                      {
                                          if (e.KeyChar == 13)
                                          {
                                              ValueChangedHandler(sender, e);
                                          }
                                      };
        }

        //
        //		private void ComboBoxDoubleClickHandler(object sender, EventArgs e) {
        //
        //		}

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
                string selectedOption = (string)_comboBox.SelectedItem;
                Object newValue = null;
                if (selectedOption != null && selectedOption.Length > 0) {
                    newValue = _collection[selectedOption];
                } else {
                    newValue = null;
                }
                if (newValue != null) {
                    if (newValue.Equals(Guid.Empty)) {
                        if (_businessObject.GetPropertyValue(_propertyName) != null) {
                            _businessObject.SetPropertyValue(_propertyName, null);
                        }
                    }
                    else if (_businessObject.GetPropertyValue(_propertyName) == null ||
                             !newValue.Equals(_businessObject.GetPropertyValue(_propertyName))) {
                        _businessObject.SetPropertyValue(_propertyName, newValue);
                    }
                } else {
                    _businessObject.SetPropertyValue(_propertyName, null);
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
            if (_collection == null)
            {
                SetupLookupList();
            }
            else
            {
                if (_businessObject.GetPropertyValue(_propertyName) == null)
                {
                    _comboBox.SelectedIndex = -1;
                }
                else
                {
                    try
                    {
                        foreach (KeyValuePair<string, object> pair in _collection) {
                            if (pair.Value == null) continue;
                            if (pair.Value is BusinessObject) {
                                if (((Bo.BusinessObject)pair.Value).ID.GetGuid().Equals( _businessObject.GetPropertyValue(_propertyName))) {
                                    _comboBox.SelectedItem = pair.Key;
                                    break;
                                }
                                else if (_businessObject.GetPropertyValue(_propertyName) != null && String.Compare(((Bo.BusinessObject)pair.Value).ID.ToString(), _businessObject.GetPropertyValue(_propertyName).ToString()) == 0) {
                                    _comboBox.SelectedItem = pair.Key;
                                    break;
                                }
                            }
                            if (pair.Value != null && pair.Value.Equals( _businessObject.GetPropertyValue(_propertyName))) {
                                _comboBox.SelectedItem = pair.Key;
                                break;
                            }
                        }
                        //_comboBox.SelectedItem =
                        //    _collection.FindByGuid((Guid) _businessObject.GetPropertyValue(_propertyName));
                        _comboBox.SelectionStart = 0;
                        _comboBox.SelectionLength = 0;
                    }
                    catch (System.ObjectDisposedException)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Sets up the list of items to display and calls SetLookupList()
        /// to populate the ComboBox with this list
        /// </summary>
        private void SetupLookupList()
        {
            BOMapper mapper = new BOMapper(_businessObject);
            Dictionary<string, object> col = mapper.GetLookupList(_propertyName);
            if (_lookupTypeClassDef == null)
            {
                SetupRightClickBehaviour();
            }
            //if (col.Count == 0) {
            //throw new LookupListNotSetException();
            //} else {
            SetLookupList(col);
            //}
            if (col.Count > 0 && _businessObject.GetPropertyValue(_propertyName) != null)
            {
                foreach (KeyValuePair<string, object> pair in _collection)
                {
                    if (pair.Value != null && pair.Value.Equals( _businessObject.GetPropertyValue(_propertyName)))
                    {
                        _comboBox.SelectedItem = pair.Key;
                    }
                }
               // _comboBox.SelectedItem =
                //    _collection.FindByGuid((Guid) _businessObject.GetPropertyValue(_propertyName));
                _comboBox.SelectionStart = 0;
                _comboBox.SelectionLength = 0;
            }
        }

        /// <summary>
        /// This method is called by SetupLookupList() and populates the
        /// ComboBox with the collection of items provided
        /// </summary>
        /// <param name="col">The items used to populate the list</param>
        public void SetLookupList(Dictionary<string, object> col)
        {
            int width = _comboBox.Width;
            Label lbl = ControlFactory.CreateLabel("", false);
            _collection = col;
            _comboBox.Items.Clear();
            _comboBox.Items.Add("");
            foreach (KeyValuePair<string, object> pair in _collection) {
                lbl.Text = pair.Key;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                _comboBox.Items.Add(pair.Key);
            }
            _comboBox.DropDownWidth = width;
        }

        /// <summary>
        /// An overridden method from the parent that simply redirects to
        /// SetupLookupList()
        /// </summary>
        protected override void SetupComboBoxItems()
        {
            SetupLookupList();
        }
    }
}