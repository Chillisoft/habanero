using System;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    public class DateTimePickerMapper : ControlMapper
    {
        private readonly IDateTimePicker _picker;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="picker">The datepicker object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public DateTimePickerMapper(IDateTimePicker picker, string propName, bool isReadOnly, IControlFactory factory)
            : base(picker, propName, isReadOnly, factory)
        {
            _picker = picker;
            _propertyName = propName;
        }

        public IDateTimePicker DateTimePicker
        {
            get { return _picker; }
        }

        public override void ApplyChangesToBusinessObject()
        {
            object newValue = GetValueOfDateTimePicker();
            SetPropertyValue(newValue);
        }
        /// <summary>
        /// Returns the value currently held by the picker
        /// </summary>
        /// <returns>Returns the value held</returns>
        private object GetValueOfDateTimePicker()
        {
            return DateTimePickerUtil.GetValue(_picker);
        }
        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected  override void InternalUpdateControlValueFromBo()
        {
            //object propValue = GetPropertyValue();
            //if (propValue == null || propValue == DBNull.Value)
            //{
            //    _dateTimePicker.Text = "";
            //}
            //else
            //{
            //    SetValueOfDateTimePicker(Convert.ToDateTime(propValue));
            //}
            _picker.Value = Convert.ToDateTime(_businessObject.GetPropertyValue(_propertyName));
        }

        ///// <summary>
        ///// Returns the property value of the business object being mapped
        ///// </summary>
        ///// <returns>Returns the property value in appropriate object form</returns>
        //protected virtual object GetPropertyValue()
        //{
        //    if (_businessObject != null)
        //    {
        //        BOMapper boMapper = new BOMapper(_businessObject);
        //        return boMapper.GetPropertyValueToDisplay(_propertyName);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}