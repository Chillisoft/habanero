using System;
using System.Collections;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages a property definition for a control in a user interface editing
    /// form, as specified in the class definitions xml file
    /// </summary>
    public class UIFormProperty
    {
        private string _label;
        private string _propertyName;
        private string _mapperTypeName;
        private Type _controlType;
        private bool _isReadOnly;
        private readonly Hashtable _propertyAttributes;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="controlType">The control type</param>
        /// <param name="mapperTypeName">The mapper type name</param>
        /// <param name="isReadOnly">Whether the control is read-only (cannot
        /// be edited directly)</param>
        /// <param name="propertyAttributes">The property attributes</param>
        public UIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName,
                              bool isReadOnly, Hashtable propertyAttributes)
        {
            this._label = label;
            this._propertyName = propertyName;
            this._mapperTypeName = mapperTypeName;
            this._isReadOnly = isReadOnly;
            this._propertyAttributes = propertyAttributes;
            this._controlType = controlType;
        }

        /// <summary>
        /// Returns the label
        /// </summary>
        public string Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Returns the mapper type name
        /// </summary>
        public string MapperTypeName
        {
            get { return _mapperTypeName; }
        }

        /// <summary>
        /// Returns the control type
        /// </summary>
        public Type ControlType
        {
            get { return _controlType; }
        }

        /// <summary>
        /// Indicates whether the control is read-only (cannot
        /// be edited directly)
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Returns the attribute value for the name provided
        /// </summary>
        /// <param name="attName">The attribute name</param>
        /// <returns>Returns the attribute value or null if not found</returns>
        public object GetAttributeValue(string attName)
        {
            if (this._propertyAttributes.ContainsKey(attName))
            {
                return this._propertyAttributes[attName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        public Hashtable Attributes
        {
            get { return this._propertyAttributes; }
        }
    }
}