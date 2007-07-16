using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Base;
using log4net;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// This provides a super class for objects that map user interface
    /// controls
    /// </summary>
    public abstract class ControlMapper
    {
        protected static readonly ILog log = LogManager.GetLogger("Chillisoft.UI.BoControls.ControlMapper");
        protected Control _control;
        protected string _propertyName;
        protected readonly bool _isReadOnceOnly;
        protected BusinessObject _businessObject;
        protected Hashtable _attributes;

        /// <summary>
        /// Constructor to instantiate a new instance of the class
        /// </summary>
        /// <param name="ctl">The control object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the control can only be
        /// read once.  If so, it then becomes disabled.  If not,
        /// handlers are assigned to manage key presses.</param>
        protected ControlMapper(Control ctl, string propName, bool isReadOnceOnly)
        {
            _control = ctl;
            _propertyName = propName;
            _isReadOnceOnly = isReadOnceOnly;
            if (isReadOnceOnly)
            {
                _control.Enabled = false;
                _control.ForeColor = Color.Black;
                _control.BackColor = Color.Beige;
            }
            else
            {
                ctl.KeyUp += new KeyEventHandler(CtlKeyUpHandler);
                ctl.KeyDown += new KeyEventHandler(CtlKeyDownHandler);
                ctl.KeyPress += new KeyPressEventHandler(CtlKeyPressHandler);
            }
        }

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
        /// Controls access to the business object being represented
        /// by the control.  Where the business object has been amended or
        /// altered, the ValueUpdated() method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public BusinessObject BusinessObject
        {
            set
            {
                _businessObject = value;
                ValueUpdated();
                if (!_isReadOnceOnly)
                {
                    _businessObject.Props[_propertyName].Updated +=
                        new EventHandler<BOPropEventArgs>(this.BOPropValueUpdatedHandler);
                }
            }
            get { return _businessObject; }
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
        /// <param name="ctl">The control to be mapped</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the control can be read once
        /// only</param>
        /// <returns>Returns a new object which is a subclass of ControlMapper</returns>
        /// <exception cref="UnknownTypeNameException">An exception is
        /// thrown if the mapperTypeName does not provide a type that is
        /// a subclass of the ControlMapper class.</exception>
        public static ControlMapper Create(string mapperTypeName, Control ctl, string propertyName, bool isReadOnceOnly)
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

            string nspace = typeof (ControlMapper).Namespace;
            Type mapperType = Type.GetType(nspace + "." + mapperTypeName);
            ControlMapper mapper;
            if (mapperType != null && mapperType.IsSubclassOf(typeof (ControlMapper)))
            {
                mapper =
                    (ControlMapper)
                    Activator.CreateInstance(mapperType, new object[] {ctl, propertyName, isReadOnceOnly});
            }
            else
            {
                throw new UnknownTypeNameException("The control mapper " +
                    "type '" + mapperTypeName + "' was not found.  All control " +
                    "mappers must inherit from ControlMapper.");
            }
            return mapper;
        }

        /// <summary>
        /// Returns the property value of the business object being mapped
        /// </summary>
        /// <returns>Returns the property value in appropriate object form</returns>
        protected virtual object GetPropertyValue()
        {
            BOMapper mapper = new BOMapper(_businessObject);
            return mapper.GetPropertyValueToDisplay(_propertyName);
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