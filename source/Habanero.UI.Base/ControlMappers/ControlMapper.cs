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
using System.Collections;
using System.Drawing;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using log4net;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A superclass to model a mapper that wraps a control in
    /// order to display and capture information related to a business object 
    /// </summary>
    public abstract class ControlMapper : IControlMapper
    {
        /// <summary>
        /// the <see cref="ILog"/> used to log any messages for this class or its children
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.ControlMapper");
        private readonly IErrorProvider _errorProvider;
        private readonly IControlFactory _factory;
        /// <summary>
        /// Is the property Read Only.
        /// </summary>
        protected readonly bool _isReadOnly;
        /// <summary>
        /// A Hash table of additional Attributes available for this Control Mapper e.g. for DateTimePickerMapper may have date format
        /// </summary>
        protected Hashtable _attributes;
        /// <summary>
        /// The Business Object being mapped
        /// </summary>
        protected IBusinessObject _businessObject;
        /// <summary>
        /// The Control being mapped to 
        /// </summary>
        protected IControlHabanero _control;
        /// <summary>
        /// Whether the control must allow editing or not.
        /// </summary>
        protected bool _isEditable;
        /// <summary>
        /// The Property Name being mapped
        /// </summary>
        protected string _propertyName;

        private IBOProp _boProp;

        ///<summary>
        /// Gets and Sets the Class Def of the Business object whose property
        /// this control maps.
        ///</summary>
        public IClassDef ClassDef { get; set; }

        /// <summary>
        /// Constructor to instantiate a new instance of the class
        /// </summary>
        /// <param name="ctl">The control object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read only.
        /// If so, it then becomes disabled.  If not,
        /// handlers are assigned to manage key presses.</param>
        /// <param name="factory"></param>
        protected ControlMapper(IControlHabanero ctl, string propName, bool isReadOnly, IControlFactory factory)
        {
            if (ctl == null) throw new ArgumentNullException("ctl");
            if (factory == null) throw new ArgumentNullException("factory");
            _control = ctl;
            _propertyName = propName;
            _isReadOnly = isReadOnly;
            _factory = factory;
            _errorProvider = _factory.CreateErrorProvider();
            if (!_isReadOnly)
            {
                AddKeyPressHandlers();
            }
            UpdateIsEditable();
        }

        /// <summary>
        /// Gets the error provider for this control <see cref="IErrorProvider"/>
        /// </summary>
        public IErrorProvider ErrorProvider
        {
            get { return _errorProvider; }
        }

        ///<summary>
        /// Returns the Control Factory that this Control Mapper is set up to use
        ///</summary>
        public IControlFactory ControlFactory
        {
            get { return _factory; }
        }

        ///<summary>
        /// Returns the value of the IsReadonly field as set up in the Control Mappers's construtor.
        ///</summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        #region IControlMapper Members

        /// <summary>
        /// Returns the control being mapped
        /// </summary>
        public IControlHabanero Control
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
        /// Gets and sets the business object that has a property
        /// being mapped by this mapper.  In other words, this property
        /// does not return the exact business object being shown in the
        /// control, but rather the business object shown in the
        /// form.  Where the business object has been amended or
        /// altered, the <see cref="UpdateControlValueFromBusinessObject"/> method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public virtual IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                RemoveCurrentBOPropHandlers();
                _businessObject = value;
                if (value != null) value.IsValid();//This forces the object to do validation so that the error provider is filled correctly.

                if (_businessObject != null && _businessObject.Props.Contains(_propertyName))
                {
                    _boProp = _businessObject.Props[_propertyName];
                }
                if (_businessObject == null)
                {
                    _boProp = null;
                }
                OnBusinessObjectChanged();
                UpdateIsEditable();
                UpdateControlValueFromBusinessObject();
                AddCurrentBOPropHandlers();
                this.UpdateErrorProviderErrorMessage();
            }
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public abstract void ApplyChangesToBusinessObject();

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        public virtual void UpdateControlValueFromBusinessObject()
        {
            InternalUpdateControlValueFromBo();
            this.UpdateErrorProviderErrorMessage();
        }

        #endregion

        private void AddKeyPressHandlers()
        {
            IControlMapperStrategy mapperStrategy = _factory.CreateControlMapperStrategy();
            mapperStrategy.AddKeyPressEventHandler(_control);
        }

        private void RemoveCurrentBOPropHandlers()
        {
            IControlMapperStrategy mapperStrategy = _factory.CreateControlMapperStrategy();
            mapperStrategy.RemoveCurrentBOPropHandlers(this, CurrentBOProp());
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected abstract void InternalUpdateControlValueFromBo();

        /// <summary>
        /// An overridable method to provide custom logic to carry out
        /// when the business object is changed
        /// </summary>
        protected virtual void OnBusinessObjectChanged()
        {
        }

        private void AddCurrentBOPropHandlers()
        {
            IControlMapperStrategy mapperStrategy = _factory.CreateControlMapperStrategy();
            mapperStrategy.AddCurrentBOPropHandlers(this, CurrentBOProp());
        }

        /// <summary>
        /// Returns the <see cref="IBOProp"/> object being mapped to this control
        /// </summary>
        public IBOProp CurrentBOProp()
        {
//            if (_businessObject != null && _businessObject.Props.Contains(_propertyName))
//            {
//                return _businessObject.Props[_propertyName];
//            }
            return _boProp;
        }
        protected virtual bool IsPropertyVirtual()
        {
            return IsPropertyViaRelationship() || IsPropertyReflective();
        }

        private bool IsPropertyViaRelationship()
        {
            return _propertyName.IndexOf(".") != -1;
        }


        private bool IsPropertyReflective()
        {
            return _propertyName.IndexOf("-") != -1;
        }

        /// <summary>
        /// Updates the isEditable flag and updates 
        /// the control according to the current state
        /// </summary>
        private void UpdateIsEditable()
        {
            bool virtualPropertySetExists = false;
            if (_businessObject != null && IsPropertyReflective()  &&  !IsPropertyViaRelationship())
            {
                virtualPropertySetExists = DoesVirtualPropertyHaveSetter();
            }
            _isEditable = !_isReadOnly && _businessObject != null
                          && (CurrentBOProp() != null || virtualPropertySetExists);

            //TODO: make this support single-table-inheritance.
            //if (_isEditable && _businessObject.ClassDef.PrimaryKeyDef.IsGuidObjectID &&
            //    _businessObject.ID.Contains(_propertyName) &&
            //    !_businessObject.Status.IsNew)
            //{
            //    _isEditable = false;
            //}
            if (_isEditable && !virtualPropertySetExists)
            {
                if (_businessObject != null)
                    if (CurrentBOProp() != null)
                    {
                        CheckReadWriteRules();
                    }
            }
            if (_isEditable)
            {
                _control.ForeColor = Color.Black;
                if (_control is ICheckBox) _control.BackColor = SystemColors.Control;
                else _control.BackColor = Color.White;
            }
            else
            {
                _control.ForeColor = Color.Black;
                _control.BackColor = Color.Beige;
            }
            _control.Enabled = _isEditable;
        }

        private void CheckReadWriteRules()
        {
            IBOProp boProp = CurrentBOProp();
            IPropDef propDef = boProp.PropDef;
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
                    }
                    else
                    {
                        _isEditable = persistedPropertyValue == null;
                    }
                    break;
                case PropReadWriteRule.WriteNew:
                    _isEditable = _businessObject.Status.IsNew;
                    break;
                case PropReadWriteRule.WriteNotNew:
                    _isEditable = !_businessObject.Status.IsNew;
                    break;
            }
        }

        private bool DoesVirtualPropertyHaveSetter()
        {
            string virtualPropName = _propertyName.Substring(1, _propertyName.Length - 2);
            PropertyInfo propertyInfo =
                ReflectionUtilities.GetPropertyInfo(_businessObject.GetType(), virtualPropName);
            bool virtualPropertySetExists = propertyInfo != null && propertyInfo.CanWrite;
            return virtualPropertySetExists;
        }


        /// <summary>
        /// Handler to carry out changes where the value of a business
        /// object property has changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        public void BOPropValueUpdatedHandler(object sender, EventArgs e)
        {
            UpdateControlValueFromBusinessObject();
        }

//                public RelationshipComboBoxMapper
//            (IComboBox comboBox, IClassDef classDef, string relationshipName, bool isReadOnly,
//             IControlFactory controlFactory)

        /// <summary>
        /// Creates a new control mapper of a specified type.  If no 'mapperTypeName'
        /// has been specified, an appropriate mapper for standard controls will
        /// be assigned, depending on the type of control.
        /// </summary>
        /// <param name="mapperTypeName">The class name of the mapper type
        /// (e.g. ComboBoxMapper).  The current namespace of this
        /// ControlMapper class will then be prefixed to the name.</param>
        /// <param name="mapperAssembly">The assembly where the mapper is
        /// located</param>
        /// <param name="ctl">The control to be mapped</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read-only</param>
        /// <returns>Returns a new object which is a subclass of ControlMapper</returns>
        /// <exception cref="UnknownTypeNameException">An exception is
        /// thrown if the mapperTypeName does not provide a type that is
        /// a subclass of the ControlMapper class.</exception>
        /// <param name="controlFactory">The control factory</param>
        public static IControlMapper Create
            (string mapperTypeName, string mapperAssembly, IControlHabanero ctl, 
             string propertyName, bool isReadOnly,
             IControlFactory controlFactory)
        {
            if (string.IsNullOrEmpty(mapperTypeName)) mapperTypeName = "TextBoxMapper";

            if (mapperTypeName == "TextBoxMapper" && !(ctl is ITextBox))
                // TODO: Port && !(ctl is PasswordTextBox))
            {
                if (ctl is IComboBox) mapperTypeName = "LookupComboBoxMapper";
                else if (ctl is ICheckBox) mapperTypeName = "CheckBoxMapper";
                else if (ctl is IDateTimePicker) mapperTypeName = "DateTimePickerMapper";
                //                else if (ctl is IListView) mapperTypeName = "ListViewCollectionManager"; TODO: Port
                else if (ctl is INumericUpDown) mapperTypeName = "NumericUpDownIntegerMapper";
                else if (ctl is IExtendedComboBox) mapperTypeName = "ExtendedComboBoxMapper";
                else
                {
                    throw new InvalidXmlDefinitionException
                        (String.Format
                             ("No suitable 'mapperType' has been provided in the class "
                              + "definitions for the form control '{0}'.  Either add the "
                              + "'mapperType' attribute or check that spelling and " 
                              + "capitalisation are correct.",
                              ctl.Name));
                }
            }

            Type mapperType;
            if (String.IsNullOrEmpty(mapperAssembly))
            {
                string nspace = typeof (ControlMapper).Namespace;
                mapperType = Type.GetType(nspace + "." + mapperTypeName);
            }
            else
            {
                mapperType = TypeLoader.LoadType(mapperAssembly, mapperTypeName);
            }
            
            IControlMapper controlMapper;
            if (mapperType != null 
               && mapperType.FindInterfaces ((type, filterCriteria) => type == typeof (IControlMapper), "").Length > 0)
            {
                try
                {
                    controlMapper =
                            (IControlMapper)
                            Activator.CreateInstance(mapperType, new object[] {ctl, propertyName, isReadOnly});
                } 

                catch (MissingMethodException)
                {
                    //This is to deal with the fact that lookupcomboboxmapper has a slightly different constructor- perhaps all control mappers
                    // should have a constructor that takes an IcontrolFactory ?
                    controlMapper =
                            (IControlMapper) 
                            Activator.CreateInstance (mapperType, new object[] {ctl, propertyName, isReadOnly, controlFactory});
                }
            }
            else
            {
                throw new UnknownTypeNameException
                    ("The control mapper " + "type '" + mapperTypeName + "' was not found.  All control "
                     + "mappers must inherit from ControlMapper.");
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
            }
            return null;
        }

        /// <summary>
        /// Sets the property value to that provided.  If the property value
        /// is invalid, the error provider will be given the reason why the
        /// value is invalid.
        /// </summary>
        protected virtual void SetPropertyValue(object value)
        {
            if (_businessObject == null) return;
            BOMapper boMapper = new BOMapper(_businessObject);

            try
            {
                boMapper.SetDisplayPropertyValue(_propertyName, value);
            }
            catch (HabaneroIncorrectTypeException ex)
            {
                this.ErrorProvider.SetError(_control, ex.Message);
                return;
            }
            UpdateErrorProviderErrorMessage();
        }
        /// <summary>
        /// Sets the Error Provider Error with the appropriate value for the property e.g. if it is invalid then
        ///  sets the error provider with the invalid reason else sets the error provider with a zero length string.
        /// </summary>
        public virtual void UpdateErrorProviderErrorMessage()
        {

            if (CurrentBOProp() == null)
            {
                ErrorProvider.SetError(_control, "");
                return;
            }

           if(!_businessObject.Status.IsNew) ErrorProvider.SetError(_control, CurrentBOProp().InvalidReason);
        }
        /// <summary>
        /// Returns the Error Provider's Error message.
        /// </summary>
        /// <returns></returns>
        public virtual string GetErrorMessage()
        {
            return ErrorProvider.GetError(_control);
        }
        /// <summary>
        /// A form field can have attributes defined in the class definition.
        /// These attributes are passed to the control mapper via a hashtable
        /// so that the control mapper can adjust its behaviour accordingly.
        /// </summary>
        /// <param name="attributes">A hashtable of attributes, which consists
        /// of name-value pairs, where name is the attribute name.  This is usually
        /// set in the XML definitions for the class's user interface.</param>
        public void SetPropertyAttributes(Hashtable attributes)
        {
            _attributes = attributes;
            InitialiseWithAttributes();
        }

        /// <summary>
        /// Initialises the control using the attributes already provided, using
        /// <see cref="SetPropertyAttributes"/>.
        /// </summary>
        protected virtual void InitialiseWithAttributes()
        {
        }
    }
}