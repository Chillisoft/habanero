using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This class provides mapping from a lookup-list to a
    /// user interface ComboBox.  This mapper is used when you have specified
    /// a lookup-list for a property definition in the class definitions.
    /// </summary>
    public class LookupComboBoxMapper : ComboBoxMapper
    {
        private bool _allowRightClick = true;
        private bool _isRightClickInitialised;

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="cbx">The ComboBox to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public LookupComboBoxMapper(ComboBox cbx, string propName, bool isReadOnly)
            : base(cbx, propName, isReadOnly)
        {
            Permission.Check(this);
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
                string selectedOption = (string) _comboBox.SelectedItem;
                Object newValue = null;
                if (selectedOption != null && selectedOption.Length > 0)
                {
                    newValue = _collection[selectedOption];
                }
                else
                {
                    newValue = null;
                }
                if (newValue != null)
                {
                    object propertyValue = GetPropertyValue();
                    if (newValue.Equals(Guid.Empty))
                    {
                        if (propertyValue != null)
                        {
                            SetPropertyValue(null);
                            //_businessObject.SetPropertyValue(_propertyName, null);
                        }
                    }
                    else if (propertyValue == null ||
                             !newValue.Equals(propertyValue))
                    {
                        SetPropertyValue(newValue);
                        //_businessObject.SetPropertyValue(_propertyName, newValue);
                    }
                }
                else
                {
                    SetPropertyValue(null);
                    //_businessObject.SetPropertyValue(_propertyName, null);
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
                if (GetPropertyValue() == null)
                {
                    _comboBox.SelectedIndex = -1;
                }
                else
                {
                    SetValueFromLookupList();
                }
            }
        }

        /// <summary>
        /// Populates the ComboBox's list of items using the
        /// strings provided by the colleciton
        /// </summary>
        private void SetValueFromLookupList()
        {
            try
            {
                object boPropertyValue = GetPropertyValue();
                foreach (KeyValuePair<string, object> pair in _collection)
                {
                    if (pair.Value == null) continue;
                    if (pair.Value is BusinessObject)
                    {
                        BusinessObject pairValueBo = (BusinessObject)pair.Value;
                        if (pairValueBo.ClassDef.PrimaryKeyDef.IsObjectID
                            && pairValueBo.ID.GetGuid().Equals(boPropertyValue))
                        {
                            _comboBox.SelectedItem = pair.Key;
                            break;
                        }
                        else if (boPropertyValue != null && String.Compare(pairValueBo.ID.ToString(), boPropertyValue.ToString()) == 0)
                        {
                            _comboBox.SelectedItem = pair.Key;
                            break;
                        }
                        else if (boPropertyValue != null &&
                            pairValueBo.ID[0].Value != null &&
                            String.Compare(pairValueBo.ID[0].Value.ToString(), boPropertyValue.ToString()) == 0)
                        {
                            _comboBox.SelectedItem = pair.Key;
                            break;
                        }
                    }
                    if (pair.Value != null && pair.Value.Equals(boPropertyValue))
                    {
                        _comboBox.SelectedItem = pair.Key;
                        break;
                    }
                }
                _comboBox.SelectionStart = 0;
                _comboBox.SelectionLength = 0;
            }
            catch (System.ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Gets or sets whether the user is able to right-click to
        /// add additional items to the drop-down list
        /// </summary>
        public override bool RightClickEnabled
        {
            get { return base.RightClickEnabled && _allowRightClick; }
            set
            {
                _allowRightClick = value;
                base.RightClickEnabled = value;
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
            if (!_isRightClickInitialised)
            {
                //SetupRightClickBehaviour();
                if (_attributes != null && !_attributes.Contains("rightClickEnabled") &&
                    GlobalUIRegistry.UISettings != null &&
                    GlobalUIRegistry.UISettings.PermitComboBoxRightClick != null)
                {
                    ClassDef lookupClassDef = mapper.GetLookupListClassDef(_propertyName);
                    if (lookupClassDef != null)
                    {
                        Type boType = lookupClassDef.ClassType;
                        if (GlobalUIRegistry.UISettings.PermitComboBoxRightClick(boType, this))
                        {
                            RightClickEnabled = _allowRightClick;
                        }
                    }
                }
                else
                {
                    RightClickEnabled = _allowRightClick;
                }
                _isRightClickInitialised = true;
            }
            SetLookupList(col);
            if (col.Count > 0 && GetPropertyValue() != null)
            {
                SetValueFromLookupList();
            }
        }

        /// <summary>
        /// This method is called by SetupLookupList() and populates the
        /// ComboBox with the collection of items provided
        /// </summary>
        /// <param name="col">The items used to populate the list</param>
        public override void SetLookupList(Dictionary<string, object> col)
        {
            int width = _comboBox.Width;
            Label lbl = ControlFactory.CreateLabel("", false);
            _collection = col;
            _comboBox.Items.Clear();
            _comboBox.Items.Add("");
            foreach (KeyValuePair<string, object> pair in _collection)
            {
                lbl.Text = pair.Key;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                _comboBox.Items.Add(pair.Key);
            }
            _comboBox.DropDownWidth = width;
        }


        protected override object GetPropertyValue()
        {
            if (_propertyName.IndexOf(".") != -1 || _propertyName.IndexOf("-") != -1)
            {
                return base.GetPropertyValue();
            } else
            {
                return _businessObject.GetPropertyValue(_propertyName);
            }
        }

        /// <summary>
        /// An overridden method from the parent that simply redirects to
        /// SetupLookupList()
        /// </summary>
        protected override void SetupComboBoxItems()
        {
            SetupLookupList();
        }

        /// <summary>
        /// Initialises the control using the attributes already provided
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (_attributes["rightClickEnabled"] != null)
            {
                string rightClickEnabled = (string)_attributes["rightClickEnabled"];
                if (rightClickEnabled != "true" && rightClickEnabled != "false")
                {
                    throw new InvalidXmlDefinitionException("An error " +
                        "occurred while reading the 'rightClickEnabled' parameter " +
                        "from the class definitions.  The 'value' " +
                        "attribute must hold either 'true' or 'false'.");
                }
                _allowRightClick = Convert.ToBoolean(rightClickEnabled);
            }
        }
    }
}