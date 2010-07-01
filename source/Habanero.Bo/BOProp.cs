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
using System.Security;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
    ///<summary>
    /// Stores the values (current Value, DatabaseValue etc) and 
    ///  state (dirty, valid) of a property of a <see cref="IBusinessObject"/>.
    /// Has a reference to the Property Definition <see cref="PropDef"/> that was used to create it.
    /// The Property definition includes property rules and validation functionality.
    /// The property of a business object may represent a property such as FirstName, Surname.
    /// Typically a <see cref="IBusinessObject"/> will have a collection of Properties.
    ///</summary>
    public class BOProp : IBOProp
    {
        /// <summary> The Logger </summary>
        protected static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOProp");
        /// <summary> The current value of the BOProp </summary>
        protected object _currentValue;
        /// <summary> Whether the prop has been edited since being created or loaded from the database </summary>
        protected internal bool _isDirty;
        /// <summary> Is the boProp valid </summary>
        protected bool _isValid = true;
        /// <summary> The propDef for the BOProp </summary>
        protected IPropDef _propDef;
        /// <summary> The reason the prop is invalid </summary>
        protected string _invalidReason = "";
        /// <summary> The persisted value of the Property. </summary>
        protected object _persistedValue;
        /// <summary> Whether the origional value loaded from the DB or the default value was valid. </summary>
        protected bool _origValueIsValid = true;
        /// <summary> The reason that the origional value was not valid. </summary>
        protected string _origInvalidReason = "";
        /// <summary> Is the business object new. </summary>
        protected bool _isObjectNew;
        /// <summary> The value prior to the last edit. </summary>
        protected object _valueBeforeLastEdit;
        private IBOPropAuthorisation _boPropAuthorisation;
        protected bool _convertEmptyStringToNull = true;
        private bool _loadedPropHasBeenValidated;
        /// <summary>
        /// Indicates that the value held by the property has been
        /// changed. This is fired any time that the current value of the property is set to a new value.
        /// </summary>
        public event EventHandler<BOPropEventArgs> Updated;

        private BusinessObject _businessObject;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        public BOProp(IPropDef propDef)
        {
            if (propDef == null) throw new ArgumentNullException("propDef");
            _propDef =  propDef;
            UpdatesBusinessObjectStatus = true;
        }

        /// <summary>
        /// Constructor to initialise a new property with a specific value
        /// </summary>
        /// <param name="propDef">The property definition</param>
        /// <param name="propValue">The initial value</param>
        public BOProp(IPropDef propDef, object propValue)
            : this(propDef)
        {
            if (propDef == null) throw new ArgumentNullException("propDef");
            InitialiseProp(propValue, true);
        }

        ///<summary>
        /// Indicates whether changes to this <see cref="BOProp"/> updates the BusinessObject's status.
        ///</summary>
        public bool UpdatesBusinessObjectStatus { get; set; }

        ///<summary>
        /// This is the <see cref="IBusinessObject"/> to which this <see cref="IBOProp"/> belongs.
        ///</summary>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            internal set
            {
                if (_businessObject != null && _businessObject != value)
                {
                    throw new HabaneroDeveloperException("A critical error has occurred. Please contact your System Administrator.",
                        "Once a BOProp has been assigned to a BusinessObject it cannot be assigned to another BusinessObject.");
                }
                _businessObject = (BusinessObject) value;
            }
        }

        ///<summary>
        /// The property definition of the property that this BOProp represents.
        ///</summary>
        public IPropDef PropDef
        {
            get { return _propDef; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return _propDef.PropertyName; }
        }

        /// <summary>
        /// Returns the database field name
        /// </summary>
        public string DatabaseFieldName
        {
            get { return _propDef.DatabaseFieldName; }
        }

        /// <summary>
        /// Initialises the property with the specified value
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        public virtual bool InitialiseProp(object propValue)
        {
            return InitialiseProp(propValue, false);
        }

        /// <summary>
        /// Validates whether the property values set for the BOProp are valid
        /// as compared to the BOProp rules. This is used by the Business Object 
        /// Validate Method.
        /// </summary>
        public void Validate()
        {
            //---Added for performance based on profiling LGMIS.
            if (!_loadedPropHasBeenValidated && !this.IsDirty)
            {
                _isValid = _propDef.IsValueValid(this.Value, ref _invalidReason);
                _loadedPropHasBeenValidated = true;
            } 
            else if(this.IsDirty)
            {
                _isValid = _propDef.IsValueValid(this.Value, ref _invalidReason);
            }
        }

        /// <summary>
        /// Initialises the property with the specified value, and indicates
        /// whether the object is new or not
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        /// <param name="isObjectNew">Whether the object is new or not</param>
        protected virtual bool InitialiseProp(object propValue, bool isObjectNew)
        {
            object newValue;
            bool propValueChanged = false;
            ParsePropValue(propValue, out newValue);
            _invalidReason = "";
            //Brett 12 Jan 2009: Removed due to performance improvement during loading.
            // No bo loaded from the database will ever be placed in an invalid state
            //_isValid = _propDef.IsValueValid(newValue, ref _invalidReason);

            if ((_currentValue == null && newValue != null) || (_currentValue != null && newValue == null) ||
                (_currentValue != null && !_currentValue.Equals(newValue)))
            {
                propValueChanged = true;
                _currentValue = newValue;
            }
            //Set up origional properties s.t. property can be backed up and restored.
            BackupPropValue();
            this.IsObjectNew = isObjectNew;
            return propValueChanged;
        }
        /// <summary>
        /// This method provides a the functionality to convert any object to the appropriate
        ///   type for the particular BOProp Type. e.g it will convert a valid guid string to 
        ///   a valid Guid Object.
        /// </summary>
        /// <param name="valueToParse">The value to be converted</param>
        /// <param name="returnValue">The value that has been parsed</param>
        public virtual void ParsePropValue(object valueToParse, out object returnValue)
        {
            bool isParsed = this.PropDef.TryParsePropValue(valueToParse, out returnValue);

            if (isParsed)
            {
                if (returnValue is IResolvableToValue)
                {
                    returnValue = ((IResolvableToValue) returnValue).ResolveToValue();
                }
            } else 
            {
                RaiseIncorrectTypeException(valueToParse);
            }
        }

        /// <summary>
        /// Restores the property's original value as defined in PersistedValue
        /// </summary>
        public void RestorePropValue()
        {
            if (_currentValue == _persistedValue)
            {
                return;
            }

            _currentValue = _persistedValue;
            _valueBeforeLastEdit = _persistedValue;
            Validate();
            _isDirty = false;
            
            FireBOPropValueUpdated();
        }

        /// <summary>
        /// Copies the current property value to PersistedValue.
        /// This is usually called when the object is persisted
        /// to the database.
        /// </summary>
        public void BackupPropValue()
        {
            _persistedValue = _currentValue;
            _valueBeforeLastEdit = _currentValue;
            _origInvalidReason = _invalidReason;
            _origValueIsValid = _isValid;
            _isObjectNew = false;
            _isDirty = false;
        }

        /// <summary>
        /// Gets and sets the value for this property
        /// </summary>
        public virtual object Value
        {
            get
            {
                if (!this.IsReadable())
                {
                    //We have created two IsReadable Methods one that 
                    // builds the error message and one that does not.
                    // these two are used to avoid the performance
                    // overhead of creating the unneccessary string message.
                    string message;
                    this.IsReadable(out message);
                    throw new BOPropReadException(message);
                }
                return _currentValue;
            }
            set
            {
                if ((_currentValue != null) && (_currentValue.Equals(value))) return;

                object newValue;
                ParsePropValue(value, out newValue);

                if (!Equals(_persistedValue, newValue))
                {
                    string message;
                    if (!IsEditable(out message))
                    {
                        throw new BOPropWriteException(_propDef, message);
                    }
                }
                if (UpdatesBusinessObjectStatus && _businessObject != null && !_businessObject.Status.IsEditing)
                {
                    _businessObject.BeginEdit();
                }
                _invalidReason = "";
                _isValid = _propDef.IsValueValid(newValue, ref _invalidReason);
                _valueBeforeLastEdit = _currentValue;
                _currentValue = newValue;
                _isDirty = !Equals(_persistedValue, newValue);
                if (_isDirty && UpdatesBusinessObjectStatus && _businessObject != null)
                {
                    _businessObject.SetDirty(true);
                }
                FireBOPropValueUpdated();
            }
        }

        /// <summary>
        /// Raises an Erorr if the Incorrect type of property is being set to this BOProp.
        /// </summary>
        /// <param name="value"></param>
        protected void RaiseIncorrectTypeException(object value)
        {
            string message = string.Format("{0} cannot be set to {1}. It is not a type of {2}" 
                                          , this.PropertyName, value, this.PropDef.PropertyTypeName);
            throw new HabaneroIncorrectTypeException(
                message, message);
        }

        /// <summary>
        /// Gets the value held before the value was last updated.
        /// If the object has just been created, this v
        /// </summary>
        public object ValueBeforeLastEdit
        {
            get { return _valueBeforeLastEdit; }
        }

        /// <summary>
        /// Fires an Updated event
        /// </summary>
        protected void FireBOPropValueUpdated()
        {
            if (this.Updated != null)
            {
                Updated(this, new BOPropEventArgs(this));
            }
        }

        /// <summary>
        /// Returns the persisted property value as a string (the value 
        /// assigned at the last backup or database committal)
        /// </summary>
        public virtual string PersistedPropertyValueString
        {
            get { return this.PropDef.ConvertValueToString(this.PersistedPropertyValue); }
        }

        /// <summary>
        /// Returns the property value as a string
        /// </summary>
        public virtual string PropertyValueString
        {
            get
            {
                try
                {
                    return this.PropDef.ConvertValueToString(this.Value);
                }
                catch (Exception exc)
                {
                    throw new HabaneroApplicationException
                        (exc.Message + "/nError occured for Property " + _propDef.PropertyName, exc);
                }
            }
        }

        /// <summary>
        /// Returns the persisted property value in its object form
        /// </summary>
        public object PersistedPropertyValue
        {
            get { return _persistedValue; }
        }

        /// <summary>
        /// Indicates whether the property value is valid
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }

        /// <summary>
        /// Returns a string which indicates why the property value may
        /// be invalid
        /// </summary>
        public string IsValidMessage
        {
            get { return _invalidReason; }
        }

        /// <summary>
        /// Returns a string which indicates why the property value may
        /// be invalid
        /// </summary>
        public string InvalidReason
        {
            get { return _invalidReason; }
        }

        /// <summary>
        /// Indicates whether the property's value has been changed since
        /// it was last backed up or committed to the database
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
        }

        /// <summary>
        /// Indicates whether the object is new
        /// </summary>
        public bool IsObjectNew
        {
            get { return _isObjectNew; }
            internal set { _isObjectNew = value; }
        }


        /// <summary>
        /// Returns the property type
        /// </summary>
        public Type PropertyType
        {
            get { return _propDef.PropertyType; }
        }

        /// <summary>
        /// Returns an XML string to describe changes between the property
        /// value and the persisted value.  It consists of an element with the 
        /// property name, containing "PreviousValue" and "NewValue" elements
        /// </summary>
        public string DirtyXml
        {
            get
            {
                return "<" + PropertyName + "><PreviousValue>" + FormatForXML(PersistedPropertyValueString)
                       + "</PreviousValue><NewValue>" + FormatForXML(PropertyValueString) + "</NewValue></"
                       + PropertyName + ">";
            }
        }

        private static string FormatForXML(string text)
        {
            return SecurityElement.Escape(text);
        }

        /// <summary>
        /// The field name as given to the user in the user interface
        /// (eg. "Computer Part" rather than "ComputerPartID").  This
        /// property is used to improve error messaging, so that the
        /// user recognises the property name as displayed to them,
        /// rather than as it is represented in the code.
        /// </summary>
        public string DisplayName
        {
            get { return _propDef.DisplayName; }
        }

        /// <summary>
        /// Returns the named property value that should be displayed
        ///   on a user interface e.g. a textbox or on a report.
        /// This is used primarily for Lookup lists where
        ///    the value stored for the object may be a guid but the value
        ///    to display may be a string.
        /// </summary>
        /// <returns>Returns the property value</returns>
        public virtual object PropertyValueToDisplay
        {
            get { return Value; }
        }

        /// <summary>
        /// Set the authorisation rule strategy to be used 
        /// </summary>
        /// <param name="boPropAuthorisation"></param>
        public void SetAuthorisationRules(IBOPropAuthorisation boPropAuthorisation)
        {
            _boPropAuthorisation = boPropAuthorisation;
        }

        ///<summary>
        /// Returns whether the BOProperty is Editable or not. The BOProp may not be editable
        ///  based on a number of factors. 
        ///  1) If its ReadWrite Rules are set to ReadOnly etc.
        ///  2) The user may not have permissions to edit this property Value.
        ///</summary>
        ///<param name="message"></param>
        ///<returns></returns>
        public virtual bool IsEditable(out string message)
        {
            if (!AreReadWriteRulesEditable(out message)) return false;
            if (_boPropAuthorisation == null) return true;
            if (!_boPropAuthorisation.IsAuthorised(this, BOPropActions.CanUpdate))
            {
                message = string.Format
                    ("The logged on user {0} is not authorised to update the {1} ",
                     Thread.CurrentPrincipal.Identity.Name, this.PropertyName);
                return false;
            }

            return true;
        }
        /// <summary>
        /// Are there any authorisation rules preventing this property from being read.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsReadable()
        {
            return _boPropAuthorisation == null || _boPropAuthorisation.IsAuthorised(this, BOPropActions.CanRead);
        }

        ///<summary>
        /// Returns whether the BOProperty is Readable or not. The BOProp may not be Readable
        ///  if the user may not have permissions to read the property Value.
        ///</summary>
        ///<param name="message">the reason why the user cannot read the property.</param>
        ///<returns></returns>
        public virtual bool IsReadable(out string message)
        {
            message = "";
            if (!IsReadable())
            {
                message = string.Format
                    ("The logged on user {0} is not authorised to read the {1} ", Thread.CurrentPrincipal.Identity.Name,
                     this.PropertyName);
                return false;
            }
            return true;
        }

        private bool AreReadWriteRulesEditable(out string message)
        {
            //Brett Jan 2009 I think this should be turned into a strategy
            string className = this.PropDef.ClassDef == null ? "" : this.PropDef.ClassDef.ClassNameFull;
            string propNameFull = className + "." + this.DisplayName;
            switch (_propDef.ReadWriteRule)
            {
                case PropReadWriteRule.ReadWrite:
                    break;
                case PropReadWriteRule.ReadOnly:
                    message = "The property '" + propNameFull + "' is not editable since it is set up as ReadOnly";
                    return false;
                case PropReadWriteRule.WriteOnce:
                    if (_isObjectNew || _persistedValue == null) break;
                    message = "The property '" + propNameFull
                              + "' is not editable since it is set up as WriteOnce and the value has already been set";
                    return false;
                case PropReadWriteRule.WriteNotNew:
                    if (_isObjectNew)
                    {
                        message = "The property '" + propNameFull
                                  + "' is not editable since it is set up as WriteNotNew and the object is new";
                        return false;
                    }
                    break;
                case PropReadWriteRule.WriteNew:
                    if (!_isObjectNew)
                    {
                        message = "The property '" + propNameFull
                                  + "' is not editable since it is set up as WriteNew and the object is not new";
                        return false;
                    }
                    break;
                default:
                    break;
            }
            message = "";
            return true;
        }
        /// <summary>
        /// is the <paramref name="compareToValue"/> equal to the 
        /// current Value of the BOProp. 
        /// </summary>
        /// <param name="compareToValue"></param>
        /// <returns></returns>
        public bool CurrentValueEquals(object compareToValue)
        {
            if (_currentValue == compareToValue) return true;
            if (_currentValue != null) return _currentValue.Equals(compareToValue);
            if(compareToValue == null) return true;
            return _convertEmptyStringToNull && (string.IsNullOrEmpty(Convert.ToString(compareToValue)));
        }
    }
}