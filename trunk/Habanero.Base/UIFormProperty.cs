using System;
using System.Collections;

namespace Habanero.Base
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
        private bool _editable;
        private readonly Hashtable _parameters;
        private string _mapperAssembly;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="controlType">The control type</param>
        /// <param name="mapperTypeName">The mapper type name</param>
        /// <param name="editable">Whether the control is read-only (cannot
        /// be edited directly)</param>
        /// <param name="parameters">The property attributes</param>
        public UIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly,
                              bool editable, Hashtable parameters)
        {
            this._label = label;
            this._propertyName = propertyName;
            this._mapperTypeName = mapperTypeName;
            this._mapperAssembly = mapperAssembly;
            this._editable = editable;
            this._parameters = parameters;
            this._controlType = controlType;
        }

        /// <summary>
        /// Returns the label
        /// </summary>
        public string Label
        {
            get { return _label; }
			protected set { _label = value; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
			protected set { _propertyName = value; }
        }

        /// <summary>
        /// Returns the mapper type name
        /// </summary>
        public string MapperTypeName
        {
            get { return _mapperTypeName; }
			protected set { _mapperTypeName = value; }
        }

        /// <summary>
        /// Returns the control type
        /// </summary>
        public Type ControlType
        {
            get { return _controlType; }
			protected set { _controlType = value; }
        }

        /// <summary>
        /// Indicates whether the control is editable
        /// </summary>
        public bool Editable
        {
            get { return _editable; }
			protected set { _editable = value; }
        }

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        public object GetParameterValue(string parameterName)
        {
            if (this._parameters.ContainsKey(parameterName))
            {
                return this._parameters[parameterName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        public Hashtable Parameters
        {
            get { return this._parameters; }
        }

        public string MapperAssembly
        {
            get { return _mapperAssembly; }
        }
    }
}