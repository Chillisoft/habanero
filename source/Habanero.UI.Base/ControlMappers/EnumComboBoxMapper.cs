using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Wraps/Decorates a ComboBox in order to display and capture a property of a
    /// business object that is an enumeration.  A blank item is inserted at the top
    /// of the list.
    /// </summary>
    public class EnumComboBoxMapper : ComboBoxMapper, IComboBoxMapper
    {
        private readonly IComboBoxMapperStrategy _mapperStrategy;

        /// <summary>
        /// Instantiates a new mapper
        /// </summary>
        /// <param name="comboBox">The ComboBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">The control factory to be used when creating the controlMapperStrategy</param>
        public EnumComboBoxMapper(IComboBox comboBox, string propName, bool isReadOnly, IControlFactory factory) : base(comboBox, propName, isReadOnly, factory)
        {
            _mapperStrategy = factory.CreateLookupComboBoxDefaultMapperStrategy();
            _mapperStrategy.AddHandlers(this);
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            if (_comboBox.SelectedIndex <= 0) SetPropertyValue(null);
            else
            {
                PropDef propDef = (PropDef) _businessObject.ClassDef.PropDefcol[_propertyName];
                string selectedItem = (_comboBox.SelectedItem.ToString()).Replace(" ","");
                SetPropertyValue(Enum.Parse(propDef.PropertyType, selectedItem));
            }
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            if (_comboBox.Items.Count == 0)
            {
                SetupComboBoxItems();
            }
            
            object value = GetPropertyValue();
            if (value == null) _comboBox.SelectedIndex = 0;
            else
            {
                string delimitedValue = StringUtilities.DelimitPascalCase(value.ToString(), " ");
                _comboBox.SelectedItem = delimitedValue;
            }
        }

        /// <summary>
        /// Sets up the items to be listed in the ComboBox
        /// </summary>
        protected internal override void SetupComboBoxItems()
        {
            if (_businessObject == null)
            {
                throw new InvalidOperationException("The BusinessObject must be set on the EnumComboBoxMapper before calling SetupComboBoxItems");
            }

            _comboBox.Items.Clear();

            PropDef propDef = (PropDef) _businessObject.ClassDef.PropDefcol[_propertyName];
            if (!propDef.PropertyType.IsEnum)
            {
                throw new InvalidPropertyException("EnumComboBoxMapper can only be used for an enum property type");
            }

            _comboBox.Items.Add("");

            string[] names = Enum.GetNames(propDef.PropertyType);
            foreach (string name in names)
            {
                string spacedName= StringUtilities.DelimitPascalCase(name, " ");
                _comboBox.Items.Add(spacedName);
            }
        }

        /// <summary>
        /// Gets or sets the SelectedIndexChanged event handler assigned to this mapper
        /// </summary>
        public EventHandler SelectedIndexChangedHandler { get; set; }
    }
}
