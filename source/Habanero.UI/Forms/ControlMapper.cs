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
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI;
using Habanero.Util;
using Habanero.Util.File;
using log4net;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This provides a super class for objects that map user interface
    /// controls
    /// </summary>
    public abstract class ControlMapper : IControlMapper
    {
        protected static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.ControlMapper");
        protected Control _control;
        protected string _propertyName;
        protected readonly bool _isReadOnly;
    	protected bool _isEditable;
        protected BusinessObject _businessObject;
        protected Hashtable _attributes;

        /// <summary>
        /// Constructor to instantiate a new instance of the class
        /// </summary>
        /// <param name="ctl">The control object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read only.
        /// If so, it then becomes disabled.  If not,
        /// handlers are assigned to manage key presses.</param>
        protected ControlMapper(Control ctl, string propName, bool isReadOnly)
        {
            _control = ctl;
            _propertyName = propName;
            _isReadOnly = isReadOnly;
			if (!_isReadOnly)
			{
				_control.KeyUp += CtlKeyUpHandler;
				_control.KeyDown += CtlKeyDownHandler;
				_control.KeyPress += CtlKeyPressHandler;
			}
        	UpdateIsEditable();
        }

    	#region Key Press Event Handlers

		/// <summary>
        /// A handler to deal with the case where a key has been pressed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CtlKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x013)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// A handler to deal with the case where a key is down.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CtlKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// A handler to deal with the case where a key has been released.
        /// If the key is an Enter key, focus moves to the next item in the
        /// tab order.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CtlKeyUpHandler(object sender, KeyEventArgs e)
        {
//            if (e.KeyCode == Keys.Enter)
//            {
//                e.Handled = true;
//
//                if (_control is TextBox && ((TextBox)_control).Multiline)
//                {
//                    return;
//                }
//
//                Control nextControl = GetNextControlInTabOrder(_control.Parent, _control);
//                //_control.FindForm().GetNextControl(_control, true) ;
//
//                if (nextControl != null)
//                {
//                    nextControl.Focus();
//                }
//            }
		}

		#endregion

		#region Control Order Methods

		/// <summary>
        /// Provides the next item in the tab order on a control
        /// </summary>
        /// <param name="parentControl">The parent of the controls in question</param>
        /// <param name="control">The current control</param>
        /// <returns>Returns the next control in the tab order</returns>
        private static Control GetNextControlInTabOrder(Control parentControl, Control control)
        {
            Control nextControl = parentControl.GetNextControl(control, true);
            if (nextControl == null)
            {
                return GetFirstControl(parentControl, control);
            }
            if (!nextControl.TabStop)
            {
                return GetNextControlInTabOrder(parentControl, nextControl);
            }
            return nextControl;
        }

        /// <summary>
        /// Provides the first control in the tab order on a control
        /// </summary>
        /// <param name="parentControl">The parent of the controls in question</param>
        /// <param name="control">The current control</param>
        /// <returns>Returns the first control in the tab order</returns>
        private static Control GetFirstControl(Control parentControl, Control control)
        {
            Control lastTabStopControl = control;
            Control currentControl = control;
            do
            {
                Control prevControl = parentControl.GetNextControl(currentControl, false);
                if (prevControl == null)
                {
                    return lastTabStopControl;
                }
                if (prevControl.TabStop)
                {
                    lastTabStopControl = prevControl;
                }
                currentControl = prevControl;
            } while (true);
		}

		#endregion

		/// <summary>
        /// Returns the control being mapped
        /// </summary>
        public Control Control
        {
            get { return _control; }
        }

        /// <summary>
        /// Returns the name of the property being edited in the control
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Gets and sets the broader business object that has a property
        /// being mapped by this mapper.  In other words, this property
        /// does not return the exact business object being shown in the
        /// control, but rather the broader business object shown in the
        /// form.  Where the business object has been amended or
        /// altered, the ValueUpdated() method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public BusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                RemoveCurrentBOPropHandlers();
                _businessObject = value;
                OnBusinessObjectChanged();
            	UpdateIsEditable();
                ValueUpdated();
                AddCurrentBOPropHandlers();
            }
        }

        protected virtual void OnBusinessObjectChanged() { }

        private void AddCurrentBOPropHandlers()
        {
            IBOProp boProp = CurrentBOProp();
            if (boProp != null)
            {
                //Add needed handlers
                boProp.Updated += this.BOPropValueUpdatedHandler;
            }
        }

        private void RemoveCurrentBOPropHandlers()
        {
            IBOProp boProp = CurrentBOProp();
            if (boProp != null)
            {
                //Remove existing handlers
                boProp.Updated -= this.BOPropValueUpdatedHandler;
            }
        }

        protected IBOProp CurrentBOProp()
        {
			if (_businessObject != null && _businessObject.Props.Contains(_propertyName))
			{
				return _businessObject.Props[_propertyName];
			} else
			{
			    return null;
			}
        }


        /// <summary>
		/// Updates the isEditable flag and updates 
		/// the control according to the current state
		/// </summary>
		private void UpdateIsEditable()
		{
            bool virtualPropertySetExists = false;
            if (_businessObject != null && _propertyName.IndexOf(".") == -1 && _propertyName.IndexOf("-") != -1)
            {
                string virtualPropName = _propertyName.Substring(1, _propertyName.Length - 2);
                PropertyInfo propertyInfo = ReflectionUtilities.GetPropertyInfo(_businessObject.GetType(), virtualPropName);
                virtualPropertySetExists = propertyInfo != null && propertyInfo.CanWrite;
            }
            _isEditable = !_isReadOnly && _businessObject != null
                    && (_businessObject.Props.Contains(_propertyName) || virtualPropertySetExists);
            if (_isEditable && _businessObject.ClassDef.GetPrimaryKeyDef().IsObjectID &&
				_businessObject.ID.Contains(_propertyName) &&
				!_businessObject.State.IsNew)
			{
				_isEditable = false;
			}
            if (_isEditable && !virtualPropertySetExists)
            {
                if (_businessObject.ClassDef.PropDefColIncludingInheritance.Contains(_propertyName))
                {
                    IPropDef propDef = _businessObject.ClassDef.PropDefColIncludingInheritance[_propertyName];
                    IBOProp boProp = CurrentBOProp();
                    switch (propDef.ReadWriteRule)
                    {
                        case PropReadWriteRule.ReadOnly:
                            _isEditable = false;
                            break;
                        case PropReadWriteRule.WriteOnce:
                            object persistedPropertyValue = boProp.PersistedPropertyValue;
                            if (persistedPropertyValue is string)
                            {
                                _isEditable = String.IsNullOrEmpty(persistedPropertyValue as string);
                            } else
                            {
                                _isEditable = persistedPropertyValue == null;
                            }
                            break;
                        case PropReadWriteRule.WriteNew:
                            _isEditable = _businessObject.State.IsNew;
                            break;
                        case PropReadWriteRule.WriteNotNew:
                            _isEditable = !_businessObject.State.IsNew;
                            break;
                    }
                }
            }
			if (_isEditable)
			{
				_control.ResetBackColor();
				_control.ResetForeColor();
			} else
			{
    			_control.ForeColor = Color.Black;
    			_control.BackColor = Color.Beige;
			}
			_control.Enabled = _isEditable;
		}


    	/// <summary>
        /// Handler to carry out changes where the value of a business
        /// object property has changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BOPropValueUpdatedHandler(object sender, EventArgs e)
        {
            ValueUpdated();
        }

        /// <summary>
        /// Carries out appropriate changes when a value being shown has
        /// been changed.
        /// </summary>
        protected abstract void ValueUpdated();

        /// <summary>
        /// Creates a new control mapper of a specified type.  If no 'mapperType'
        /// has been specified, an appropriate mapper for standard controls will
        /// be assigned as appropriate.
        /// </summary>
        /// <param name="mapperTypeName">The class name of the mapper type
        /// (e.g. ComboBoxMapper).  The current namespace of this
        /// ControlMapper class will then be prefixed to the name.</param>
        /// <param name="mapperAssembly">The assembly where the mapper is
        /// located</param>
        /// <param name="ctl">The control to be mapped</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read only</param>
        /// <returns>Returns a new object which is a subclass of ControlMapper</returns>
        /// <exception cref="UnknownTypeNameException">An exception is
        /// thrown if the mapperTypeName does not provide a type that is
        /// a subclass of the ControlMapper class.</exception>
        public static ControlMapper Create(string mapperTypeName, string mapperAssembly, Control ctl, string propertyName, bool isReadOnly)
        {
            if (mapperTypeName == "TextBoxMapper" && !(ctl is TextBox) && !(ctl is PasswordTextBox))
            {
                if (ctl is ComboBox) mapperTypeName = "LookupComboBoxMapper";
                else if (ctl is CheckBox) mapperTypeName = "CheckBoxMapper";
                else if (ctl is DateTimePicker) mapperTypeName = "DateTimePickerMapper";
                else if (ctl is ListView) mapperTypeName = "ListViewCollectionMapper";
                else if (ctl is NumericUpDown) mapperTypeName = "NumericUpDownIntegerMapper";
                else
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "No suitable 'mapperType' has been provided in the class " +
                        "definitions for the form control '{0}'.  Either add the " +
                        "'mapperType' attribute or check that spelling and " +
                        "capitalisation are correct.", ctl.Name));
                }
            }

            Type mapperType;
            if (String.IsNullOrEmpty(mapperAssembly)) {
                string nspace = typeof (ControlMapper).Namespace;
                mapperType = Type.GetType(nspace + "." + mapperTypeName);
            } else {
                mapperType = TypeLoader.LoadType(mapperAssembly, mapperTypeName);
            }
            ControlMapper controlMapper;
            if (mapperType != null && mapperType.IsSubclassOf(typeof (ControlMapper)))
            {
                controlMapper =
                    (ControlMapper)
                    Activator.CreateInstance(mapperType, new object[] {ctl, propertyName, isReadOnly});
            }
            else
            {
                throw new UnknownTypeNameException("The control mapper " +
                    "type '" + mapperTypeName + "' was not found.  All control " +
                    "mappers must inherit from ControlMapper.");
            }
            return controlMapper;
        }

        /// <summary>
        /// Returns the property value of the business object being mapped
        /// </summary>
        /// <returns>Returns the property value in appropriate object form</returns>
        protected virtual object GetPropertyValue()
        {
			if (_businessObject != null)
			{
				BOMapper boMapper = new BOMapper(_businessObject);
				return boMapper.GetPropertyValueToDisplay(_propertyName);
			} else
			{
				return null;
			}
        }

        protected virtual void SetPropertyValue(object value)
        {
            if (_businessObject != null)
            {
                BOMapper boMapper = new BOMapper(_businessObject);
                boMapper.SetDisplayPropertyValue(_propertyName, value);
            }
        }

        /// <summary>
        /// Applies a hashtable of property attributes
        /// </summary>
        /// <param name="attributes">A hashtable of attributes</param>
        /// TODO ERIC - clarify, what regulations?
        public void SetPropertyAttributes(Hashtable attributes)
        {
            _attributes = attributes;
            InitialiseWithAttributes();
        }

        /// <summary>
        /// Initialises the control using the attributes already provided
        /// </summary>
        protected virtual void InitialiseWithAttributes()
        {
        }
    }
}