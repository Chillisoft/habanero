// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Collections;
using System.Drawing;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
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

        /// <summary>
        /// A Hash table of additional Attributes available for this Control Mapper e.g. for DateTimePickerMapper may have date format
        /// </summary>
        protected Hashtable _attributes;
        /// <summary>
        /// The Business Object being mapped
        /// </summary>
        protected IBusinessObject _businessObject;

        /// <summary>
        /// Whether the control must allow editing or not.
        /// </summary>
        protected bool IsEditable { get; private set; }

        private IBOProp BOProp { get; set; }


        /// <summary>
        /// Gets the error provider for this control <see cref="IErrorProvider"/>
        /// </summary>
        public IErrorProvider ErrorProvider { get; private set; }

        ///<summary>
        /// Returns the Control Factory that this Control Mapper is set up to use
        ///</summary>
        public IControlFactory ControlFactory { get; private set; }

        ///<summary>
        /// Returns the value of the IsReadonly field as set up in the Control Mappers's construtor.
        ///</summary>
        public bool IsReadOnly { get; set; }

        #region IControlMapper Members

        /// <summary>
        /// Returns the control being mapped
        /// </summary>
        public IControlHabanero Control { get; protected set; }

        /// <summary>
        /// Returns the name of the property being edited in the control
        /// </summary>
        public string PropertyName { get; protected set; }
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
            Control = ctl;
            PropertyName = propName;
            IsReadOnly = isReadOnly;
            ControlFactory = factory;
            ErrorProvider = ControlFactory.CreateErrorProvider();
            if (!IsReadOnly)
            {
                AddKeyPressHandlers();
            }
            UpdateIsEditable();
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

                if (_businessObject != null && _businessObject.Props.Contains(PropertyName))
                {
                    BOProp = _businessObject.Props[PropertyName];
                }
                if (_businessObject == null)
                {
                    BOProp = null;
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
            IControlMapperStrategy mapperStrategy = ControlFactory.CreateControlMapperStrategy();
            mapperStrategy.AddKeyPressEventHandler(Control);
        }

        private void RemoveCurrentBOPropHandlers()
        {
            IControlMapperStrategy mapperStrategy = ControlFactory.CreateControlMapperStrategy();
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
        // ReSharper disable VirtualMemberNeverOverriden.Global
        protected virtual void OnBusinessObjectChanged()
        {
        }
        // ReSharper restore VirtualMemberNeverOverriden.Global

        private void AddCurrentBOPropHandlers()
        {
            IControlMapperStrategy mapperStrategy = ControlFactory.CreateControlMapperStrategy();
            mapperStrategy.AddCurrentBOPropHandlers(this, CurrentBOProp());
        }

        /// <summary>
        /// Returns the <see cref="IBOProp"/> object being mapped to this control
        /// </summary>
        public IBOProp CurrentBOProp()
        {
            return BOProp;
        }
        /// <summary>
        /// is the property a virtual property i.e. is it loaded via reflection or via a relationship.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsPropertyVirtual()
        {
            return IsPropertyViaRelationship() || IsPropertyReflective();
        }

        private bool IsPropertyViaRelationship()
        {
            return PropertyName.IndexOf(".") != -1;
        }


        private bool IsPropertyReflective()
        {
            return PropertyName.IndexOf("-") != -1;
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
            IsEditable = !IsReadOnly && _businessObject != null
                          && (CurrentBOProp() != null || virtualPropertySetExists);

            if (IsEditable && !virtualPropertySetExists)
            {
                if (_businessObject != null)
                    if (CurrentBOProp() != null)
                    {
                        IsEditable = CheckReadWriteRules();
                    }
            }
            UpdateControlVisualState(IsEditable);
            UpdateControlEnabledState(IsEditable);
        }

        protected virtual void UpdateControlEnabledState(bool editable)
        {
            Control.Enabled = editable;
        }

        protected virtual void UpdateControlVisualState(bool editable)
        {
            if (editable)
            {
                Control.ForeColor = Color.Black;
                if (Control is ICheckBox) Control.BackColor = SystemColors.Control;
                else Control.BackColor = Color.White;
            }
            else
            {
                Control.ForeColor = Color.Black;
                Control.BackColor = Color.Beige;
            }
        }

        private bool CheckReadWriteRules()
        {
            IBOProp boProp = CurrentBOProp();
            string message;
            //Should Add the message to tool tip text
            return boProp.IsEditable(out message);
        }

        private bool DoesVirtualPropertyHaveSetter()
        {
            string virtualPropName = PropertyName.Substring(1, PropertyName.Length - 2);
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
                            Activator.CreateInstance (mapperType, new object[] {ctl, propertyName, isReadOnly, controlFactory});
                } 

                catch (MissingMethodException)
                {
                    //This is to cater for older versions of custom implemented control mappers that did not have the control factory parameter
                    controlMapper =
                            (IControlMapper)
                            Activator.CreateInstance(mapperType, new object[] {ctl, propertyName, isReadOnly});
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
                return boMapper.GetPropertyValueToDisplay(PropertyName);
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
                boMapper.SetDisplayPropertyValue(PropertyName, value);
            }
            catch (HabaneroIncorrectTypeException ex)
            {
                this.ErrorProvider.SetError(Control, ex.Message);
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
                ErrorProvider.SetError(Control, "");
                return;
            }

            ErrorProvider.SetError(Control, CurrentBOProp().InvalidReason);
        }
        /// <summary>
        /// Returns the Error Provider's Error message.
        /// </summary>
        /// <returns></returns>
        public virtual string GetErrorMessage()
        {
            return ErrorProvider.GetError(Control);
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