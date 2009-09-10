//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Wraps a CheckBox in order to display and capture a boolean property of the business object 
    /// </summary>
    public class CheckBoxMapper : ControlMapper
    {
        private readonly ICheckBox _checkBox;
        private readonly ICheckBoxMapperStrategy _strategy;

        /// <summary>
        /// Constructor to create a new CheckBox mapper object
        /// </summary>
        /// <param name="cb">The CheckBox object to be mapped</param>
        /// <param name="propName">A name for the property</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public CheckBoxMapper(ICheckBox cb, string propName, bool isReadOnly, IControlFactory factory)
            : base(cb, propName, isReadOnly, factory)
        {
            _checkBox = cb;
            _strategy = factory.CreateCheckBoxMapperStrategy();
            _strategy.AddClickEventHandler(this);
        }

        /// <summary>
        /// Updates the appearance of the control when the value of the
        /// property has changed internally
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            object propValue = GetPropertyValue();
            bool newValue;
            if (propValue != null)
            {
                newValue = Convert.ToBoolean(propValue);
            }
            else
            {
                newValue = false;
            }
            if (newValue != _checkBox.Checked)
            {
                _checkBox.Checked = newValue;
            }
        }
        /// <summary>
        /// The event handler that is called to update the property on the 
        /// Business Object when the user has checked or unchecked the CheckBox.
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            if (!_isEditable) return;

            bool newValue = _checkBox.Checked;
            bool valueChanged = false;
            if (GetPropertyValue() == null)
            {
                valueChanged = true;
            }
            else
            {
                bool oldValue = Convert.ToBoolean(GetPropertyValue());
                if (newValue != oldValue)
                {
                    valueChanged = true;
                }
            }
            if (valueChanged)
            {
                SetPropertyValue(newValue);
            }
        }

        /// <summary>
        /// Gets the custom strategy that is applied to this mapper by the
        /// control factory, which determines how the Click event is handled.
        /// See <see cref="ICheckBoxMapperStrategy"/>.
        /// </summary>
        public ICheckBoxMapperStrategy GetStrategy()
        {
            return _strategy;
        }
    }
}