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
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This is used to map a collection of Business objects to a particular property of the Business object.
    /// This is typically used when you are wanting to load a collection of business objects via some custom 
    /// collection loading code that does not allow you to use either 
    /// the <see cref="LookupComboBoxMapper"/> or the <see cref="RelationshipComboBoxMapper"/>.
    /// </summary>
    public class CollectionComboBoxMapper : ControlMapper, IComboBoxMapper
    {
        /// <summary>
        /// The actual <see cref="IComboBox"/> control that is being mapped to the Business Object Property identified by PropertyName.
        /// </summary>
        protected IComboBox _comboBox;
        private IComboBoxMapperStrategy _mapperStrategy;

        private IBusinessObjectCollection _businessObjectCollection;
        private readonly ComboBoxCollectionSelector _comboBoxCollectionSelector;

        /// <summary>
        /// Gets or sets the KeyPress event handler assigned to this mapper
        /// </summary>
        public EventHandler KeyPressHandler { get; set; }

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
        public CollectionComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory factory)
            : base(cbx, propName, isReadOnly, factory)
        {
            _comboBox = cbx;
            _mapperStrategy = factory.CreateLookupComboBoxDefaultMapperStrategy();
            _mapperStrategy.AddHandlers(this);
            _comboBoxCollectionSelector = new ComboBoxCollectionSelector(cbx, factory, false);
        }

        /// <summary>
        /// Gets or sets the strategy assigned to this mapper <see cref="IComboBoxMapperStrategy"/>
        /// </summary>
        public IComboBoxMapperStrategy MapperStrategy
        {
            get { return _mapperStrategy; }
            set
            {
                _mapperStrategy = value;
                _mapperStrategy.RemoveCurrentHandlers(this);
                _mapperStrategy.AddHandlers(this);
            }
        }

        /// <summary>
        /// The business object collection that is being used to fill the combo box with values.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return _businessObjectCollection; }
            set
            {
                _businessObjectCollection = value;
                _comboBoxCollectionSelector.SetCollection(value, true);
                UpdateControlValueFromBusinessObject();
                ApplyChangesToBusinessObject();
            }
        }

        /// <summary>
        /// This is the PropertyName on the selected Business Object, which should be used to get the value to be set to the
        /// Property. See tests.
        /// </summary>
        public string OwningBoPropertyName { get; set; }


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
            if (BusinessObjectCollection == null && this.BusinessObject != null)
            {
                const string message = "The BusinessObjectCollection is null when in the CollectionComboBoxMapper when the BusinessObject is set ";
                throw new HabaneroDeveloperException(message, message);
            }
            if (PropertyHasAValue())
            {
                SetValueFromLookupList();
            }
            else
            {
                ClearComboBox();
            }
        }

        private void ClearComboBox()
        {
            _comboBox.SelectedIndex = -1;
            _comboBox.SelectedItem = null;
            _comboBox.Text = null;
        }

        private bool PropertyHasAValue()
        {
            return !string.IsNullOrEmpty(Convert.ToString(GetPropertyValue()));
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
                foreach (IBusinessObject bo in this.BusinessObjectCollection)
                {
                    if (bo == null) continue;
                    //TODO Eric 30 Jul 2009: use a properties value
                    bool found = false;
                    found = String.IsNullOrEmpty(OwningBoPropertyName)
                                ? bo.ID.ToString().Equals(Convert.ToString(boPropertyValue))
                                : Object.Equals(bo.Props[OwningBoPropertyName].Value, boPropertyValue);
                    if (found)
                    {
                        _comboBox.SelectedItem = bo;
                        break;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
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
                       : _businessObject.GetPropertyValueString(_propertyName);
        }

        /// <summary>
        /// Initialises the control using the attributes already provided, using
        /// <see cref="ControlMapper.SetPropertyAttributes"/>.
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
            }
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            IBusinessObject selectedOption = _comboBox.SelectedItem as IBusinessObject;
            //TODO Eric 30 Jul 2009: use the Property of the Business object.
            object newValue = null;
            if (selectedOption != null)
            {
                newValue = !string.IsNullOrEmpty(OwningBoPropertyName) ? selectedOption.Props[OwningBoPropertyName].Value: selectedOption.ID.GetAsGuid();
            }
            SetPropertyValue(newValue);
        }
    }
}