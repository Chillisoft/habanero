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
        private string itsLabel;
        private string itsPropertyName;
        private string itsMapperTypeName;
        private Type itsControlType;
        private bool itsIsReadOnly;
        private readonly Hashtable itsPropertyAttributes;

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
            this.itsLabel = label;
            this.itsPropertyName = propertyName;
            this.itsMapperTypeName = mapperTypeName;
            this.itsIsReadOnly = isReadOnly;
            this.itsPropertyAttributes = propertyAttributes;
            this.itsControlType = controlType;
        }

        /// <summary>
        /// Returns the label
        /// </summary>
        public string Label
        {
            get { return itsLabel; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return itsPropertyName; }
        }

        /// <summary>
        /// Returns the mapper type name
        /// </summary>
        public string MapperTypeName
        {
            get { return itsMapperTypeName; }
        }

        /// <summary>
        /// Returns the control type
        /// </summary>
        public Type ControlType
        {
            get { return itsControlType; }
        }

        /// <summary>
        /// Indicates whether the control is read-only (cannot
        /// be edited directly)
        /// </summary>
        public bool IsReadOnly
        {
            get { return itsIsReadOnly; }
        }

        /// <summary>
        /// Returns the attribute value for the name provided
        /// </summary>
        /// <param name="attName">The attribute name</param>
        /// <returns>Returns the attribute value or null if not found</returns>
        public object GetAttributeValue(string attName)
        {
            if (this.itsPropertyAttributes.ContainsKey(attName))
            {
                return this.itsPropertyAttributes[attName];
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
            get { return this.itsPropertyAttributes; }
        }
    }
}