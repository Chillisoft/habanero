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
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This class provides mapping from a lookup-list to a
    /// user interface ComboBox.  This mapper is used when you have specified
    /// a lookup-list for a property definition in the class definitions.
    /// </summary>
    public class LookupComboBoxMapper : ComboBoxMapper
    {
        private readonly IControlFactory _controlFactory;
        //private bool _allowRightClick = true;
        //private bool _isRightClickInitialised;

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="cbx">The ComboBox to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		/// <param name="controlFactory"></param>
		public LookupComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(cbx, propName, isReadOnly)
        {
            _controlFactory = controlFactory;
            _comboBox = cbx;
            ////_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //_comboBox.SelectedIndexChanged += ValueChangedHandler;
            //_comboBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
            //                          {
            //                              if (e.KeyChar == 13)
            //                              {
            //                                  ValueChangedHandler(sender, e);
            //                              }
            //                          };
        }

        //
        //		private void ComboBoxDoubleClickHandler(object sender, EventArgs e) {
        //
        //		}


        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected internal override void ValueUpdated()
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
                    _comboBox.Text = "";
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
                    bool found = false;
                    if (pair.Value != null)
                    {
                        if (pair.Value is string)
                        {
                            found = pair.Value.Equals(Convert.ToString(boPropertyValue));
                        }
                        else
                        {
                            found = pair.Value.Equals(boPropertyValue);
                        }
                    }
                    if (found)
                    {
                        _comboBox.SelectedItem = pair.Key;
                        break;
                    }
                }
                //_comboBox.SelectionStart = 0;
                //_comboBox.SelectionLength = 0;
            }
            catch (ObjectDisposedException)
            {
            }
        }

        ///// <summary>
        ///// Gets or sets whether the user is able to right-click to
        ///// add additional items to the drop-down list
        ///// </summary>
        //public override bool RightClickEnabled
        //{
        //    get { return base.RightClickEnabled && _allowRightClick; }
        //    set
        //    {
        //        _allowRightClick = value;
        //        base.RightClickEnabled = value;
        //    }
        //}

        /// <summary>
        /// Sets up the list of items to display and calls SetLookupList()
        /// to populate the ComboBox with this list
        /// </summary>
        private void SetupLookupList()
        {
            if (_businessObject == null)
            {
                Dictionary<string, object> emptyList = new Dictionary<string, object>();
                SetLookupList(emptyList);                
            }
            Dictionary<string, object> col;
            BOMapper mapper = new BOMapper(_businessObject);
            col = mapper.GetLookupList(_propertyName);
            //if (!_isRightClickInitialised)
            //{
            //    //SetupRightClickBehaviour();
            //    if (_attributes != null && !_attributes.Contains("rightClickEnabled") &&
            //        GlobalUIRegistry.UISettings != null &&
            //        GlobalUIRegistry.UISettings.PermitComboBoxRightClick != null)
            //    {
            //        ClassDef lookupClassDef = mapper.GetLookupListClassDef(_propertyName);
            //        if (lookupClassDef != null)
            //        {
            //            Type boType = lookupClassDef.ClassType;
            //            if (GlobalUIRegistry.UISettings.PermitComboBoxRightClick(boType, this))
            //            {
            //                RightClickEnabled = _allowRightClick;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        RightClickEnabled = _allowRightClick;
            //    }
            //    _isRightClickInitialised = true;
            //}
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
            ILabel lbl = _controlFactory.CreateLabel("", false);
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
            }
            else if (_businessObject != null)
            {
                return _businessObject.GetPropertyValue(_propertyName);
            }
            else 
            {
                return null;
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
                //_allowRightClick = Convert.ToBoolean(rightClickEnabled);
            }
        }

        /// <summary>
        /// updates the business object with the value selected in the combo box
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            if (_businessObject != null && _comboBox.SelectedIndex != -1)
            {
                string selectedOption = (string)_comboBox.SelectedItem;
                Object newValue;
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
                        }
                    }
                    else if (propertyValue == null ||
                             !newValue.Equals(propertyValue))
                    {
                        SetPropertyValue(newValue);
                    }
                }
                else
                {
                    SetPropertyValue(null);
                }
            }
        }
    }
}