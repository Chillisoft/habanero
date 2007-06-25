using System;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Maps the lookup ComboBox
    /// </summary>
    /// TODO ERIC - what's the dif btw these? what is lookup?
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
                Guid newValue = ((StringGuidPair) _comboBox.SelectedItem).Id;
                if (newValue.Equals(Guid.Empty))
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
                        _comboBox.SelectedItem =
                            _collection.FindByGuid((Guid) _businessObject.GetPropertyValue(_propertyName));
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
            StringGuidPairCollection col = mapper.GetLookupList(_propertyName);
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
                _comboBox.SelectedItem =
                    _collection.FindByGuid((Guid) _businessObject.GetPropertyValue(_propertyName));
                _comboBox.SelectionStart = 0;
                _comboBox.SelectionLength = 0;
            }
        }

        /// <summary>
        /// This method is called by SetupLookupList() and populates the
        /// ComboBox with the collection of items provided
        /// </summary>
        /// <param name="col">The items used to populate the list</param>
        public void SetLookupList(StringGuidPairCollection col)
        {
            int width = _comboBox.Width;
            Label lbl = ControlFactory.CreateLabel("", false);
            _collection = col;
            _comboBox.Items.Clear();
            _comboBox.Items.Add(new StringGuidPair("", Guid.Empty));
            foreach (StringGuidPair pair in col)
            {
                lbl.Text = pair.Str;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                _comboBox.Items.Add(pair);
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