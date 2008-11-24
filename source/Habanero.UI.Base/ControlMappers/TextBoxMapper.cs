//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Wraps a TextBox control in order to display and capture a property of the business object.
    /// There are some  limitations with using a TextBox for numbers.  For greater control 
    /// of user input with numbers, you should consider using a NumericUpDown 
    /// control, failing this the appropriate <see cref="ITextBoxMapperStrategy"/> can be used.
    /// </summary>
    public class TextBoxMapper : ControlMapper
    {
        private readonly ITextBox _textBox;
        private string _oldText;
        private readonly ITextBoxMapperStrategy _textBoxMapperStrategy;

        /// <summary>
        /// Constructor to initialise a new instance of the mapper
        /// </summary>
        /// <param name="tb">The TextBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public TextBoxMapper(ITextBox tb, string propName, bool isReadOnly, IControlFactory factory)
            : base(tb, propName, isReadOnly, factory)
        {
            _textBox = tb;
            _textBoxMapperStrategy = factory.CreateTextBoxMapperStrategy();

            _oldText = "";
        }

        /// <summary>
        /// Returns the <see cref="ITextBoxMapperStrategy"/> being used by this mapper.
        /// </summary>
        public ITextBoxMapperStrategy TextBoxMapperStrategy
        {
            get { return _textBoxMapperStrategy; }
        }

        /// <summary>
        /// Gets and sets the business object that has a property
        /// being mapped by this mapper.  In other words, this property
        /// does not return the exact business object being shown in the
        /// control, but rather the business object shown in the
        /// form.  Where the business object has been amended or
        /// altered, the <see cref="ControlMapper.UpdateControlValueFromBusinessObject"/> method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public override IBusinessObject BusinessObject
        {
            get { return base.BusinessObject; }
            set
            {
                base.BusinessObject = value;
                TextBoxMapperStrategy.AddKeyPressEventHandler(this, base.CurrentBOProp());
                TextBoxMapperStrategy.AddUpdateBoPropOnTextChangedHandler(this, base.CurrentBOProp());
            }
        }

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            _textBox.Text = Convert.ToString(GetPropertyValue());
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            string value = _textBox.Text;

            if (!_isEditable) return;

            try
            {
                SetPropertyValue(value);
            }
            catch (FormatException)
            {
                _textBox.Text = _oldText;
                throw new BusObjectInAnInvalidStateException("The business object '" +
                                                             this._businessObject.ClassDef.ClassName + "' - '" +
                                                             this._businessObject +
                                                             "' could not be updated since the value '" + value +
                                                             "' is not valid for the property '" + _propertyName + "'");
            }
            _oldText = _textBox.Text;
        }
    }
}