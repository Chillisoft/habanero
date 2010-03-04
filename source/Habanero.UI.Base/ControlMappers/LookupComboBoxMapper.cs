// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// An interface for a mapper that <br/>
    /// Wraps/Decorates a ComboBox in order to display and capture a lookup property of the business object 
    /// </summary>
    public interface IComboBoxMapper : IControlMapper
    {
        /// <summary>
        /// Gets or sets the SelectedIndexChanged event handler assigned to this mapper
        /// </summary>
        EventHandler SelectedIndexChangedHandler { get; set; }

//        /// <summary>
//        /// Returns the control being mapped
//        /// </summary>
//        IControlHabanero Control { get; }
//
//        /// <summary>
//        /// Updates the properties on the represented business object
//        /// </summary>
//        void ApplyChangesToBusinessObject();
//
//        /// <summary>
//        /// Updates the value on the control from the corresponding property
//        /// on the represented <see cref="IControlMapper.BusinessObject"/>
//        /// </summary>
//        void UpdateControlValueFromBusinessObject();
    }

    /// <summary>
    /// Wraps/Decorates a ComboBox in order to display and capture a lookup property of the business object 
    /// </summary>
    public class LookupComboBoxMapper : ComboBoxMapper, IComboBoxMapper
    {
        private IComboBoxMapperStrategy _mapperStrategy;

        /// <summary>
        /// Gets or sets the KeyPress event handler assigned to this mapper
        /// </summary>
// ReSharper disable UnusedMember.Global
// This is an implementation of an interface and doew not have to be used.
        public EventHandler KeyPressHandler { get; set; }
// ReSharper restore UnusedMember.Global

        /// <summary>
        /// Gets or sets the SelectedIndexChanged event handler assigned to this mapper
        /// </summary>
        public EventHandler SelectedIndexChangedHandler { get; set; }

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="cbx">The ComboBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">The control factory to be used when creating the controlMapperStrategy</param>
        public LookupComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory factory)
            : base(cbx, propName, isReadOnly, factory)
        {
            _comboBox = cbx;
            _mapperStrategy = factory.CreateLookupComboBoxDefaultMapperStrategy();
            if (_mapperStrategy == null) return;
            _mapperStrategy.AddHandlers(this);
            _collection = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets and sets the lookup list used to populate the items in the
        /// ComboBox.  This method is typically called by SetupLookupList().
        /// </summary>
        public virtual Dictionary<string, string> LookupList
        {
            get { return _collection; }
            set
            {
                _collection = value;
                _comboBox.Items.Clear();
                _comboBox.Items.Add(new ComboPair("", null));
                if (_collection == null) _collection  = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> pair in _collection)
                {
                    _comboBox.Items.Add(new ComboPair(pair.Key, pair.Value));
                }
            }
        }

// ReSharper disable UnusedMember.Global
//This is maintained for backward compatibility
        /// <summary>
        /// Sets the lookup list to the lookupList Values
        /// </summary>
        /// <param name="lookupList"></param>
        [Obsolete("Use Lookuplist property")]
        public virtual void SetupLookupList(Dictionary<string, string> lookupList)
        {
            LookupList = lookupList;
        }

        ///<summary>
        /// Sets the lookuplist used to populate the items in the ComboBox.
        ///</summary>
        ///<param name="lookupList">The items used to populate the list</param>
        [Obsolete(
            "This method is to be replaced with the property LookupList. Please use the property as this method will be removed in a future version."
            )]

        public void SetLookupList(Dictionary<string, string> lookupList)
        {
            this.LookupList = lookupList;
        }
// ReSharper restore UnusedMember.Global

        /// <summary>
        /// Gets or sets the strategy assigned to this mapper <see cref="IComboBoxMapperStrategy"/>
        /// </summary>
        public IComboBoxMapperStrategy MapperStrategy
        {
            get { return _mapperStrategy; }
            set
            {
                _mapperStrategy = value;
                if (_mapperStrategy == null) return;
                _mapperStrategy.RemoveCurrentHandlers(this);
                _mapperStrategy.AddHandlers(this);
            }
        }


        //
        //		private void ComboBoxDoubleClickHandler(object sender, EventArgs e) {
        //
        //		}
        /// <summary>
        /// This is a hack that allows Extended Combo box mapper to use Lookup ComboBox mapper code
        ///  without having to inherit from it.
        /// </summary>
        internal void DoUpdateControlValueFromBO()
        {
            this.InternalUpdateControlValueFromBo();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            if (_collection == null || _collection.Count == 0)
            {
                SetupLookupList();
            }
            else
            {
                if (!PropertyHasAValue())
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
        //Does the Business Object have a value for the mapped property.
        private bool PropertyHasAValue()
        {
            return !string.IsNullOrEmpty(Convert.ToString(GetPropertyValue()));
        }

        /// <summary>
        /// Sets the ComboBox's Selected Item based on the
        /// Value from the Business Object.
        /// </summary>
        private void SetValueFromLookupList()
        {
            try
            {
                object boPropertyValue = GetPropertyValue();
                foreach (KeyValuePair<string, string> pair in _collection)
                {
                    if (pair.Value == null) continue;

                    bool found = pair.Value.Equals(Convert.ToString(boPropertyValue));
                    if (!found) continue;
                    _comboBox.SelectedItem = pair.Key;
                    break;
                }
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
                Dictionary<string, string> emptyList = new Dictionary<string, string>();
                LookupList = emptyList;
                return;
            }
            BOMapper mapper = new BOMapper(_businessObject);
            Dictionary<string, string> col = mapper.GetLookupList(PropertyName);
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
            CustomiseLookupList(col);
            LookupList = col;
            if (col.Count > 0 && GetPropertyValue() != null)
            {
                SetValueFromLookupList();
            }
        }

        /// <summary>
        /// Do customisation of the Lookup list by overriding this method in an inheritor.
        /// </summary>
        /// <param name="col">The look up list retrieved from the businessobject that will be customised</param>
        protected virtual void CustomiseLookupList(Dictionary<string, string> col)
        {
        }

        /// <summary>
        /// Returns the property value of the business object being mapped
        /// </summary>
        /// <returns>Returns the property value in appropriate object form</returns>
        protected override object GetPropertyValue()
        {
            if (IsPropertyVirtual())
            {
                return base.GetPropertyValue();
            }
            return _businessObject == null
                       ? null
                       : _businessObject.GetPropertyValueString(PropertyName);
        }


        /// <summary>
        /// Sets up the items to be listed in the ComboBox
        /// </summary>
        public override void SetupComboBoxItems()
        {
            SetupLookupList();
        }

        /// <summary>
        /// Initialises the control using the attributes already provided, using
        /// <see cref="ControlMapper.SetPropertyAttributes"/>.
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (_attributes["rightClickEnabled"] == null) return;

            string rightClickEnabled = (string) _attributes["rightClickEnabled"];
            if (rightClickEnabled != "true" && rightClickEnabled != "false")
            {
                throw new InvalidXmlDefinitionException("An error " +
                                                        "occurred while reading the 'rightClickEnabled' parameter " +
                                                        "from the class definitions.  The 'value' " +
                                                        "attribute must hold either 'true' or 'false'.");
            }
            //_allowRightClick = Convert.ToBoolean(rightClickEnabled);
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            if (_businessObject == null || _comboBox.SelectedIndex == -1) return;
            string selectedOption = (string) _comboBox.SelectedItem;
            object newValue = null;
            if (LookupList.ContainsKey(selectedOption))
            {
                newValue = !string.IsNullOrEmpty(selectedOption)
                               ? LookupList[selectedOption]
                               : null;
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