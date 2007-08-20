using System;
using System.Collections;
using Habanero.Util.File;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a property definition for a control in a user interface editing
    /// form, as specified in the class definitions xml file
    /// </summary>
    public class UIFormField
    {
        private string _label;
        private string _propertyName;
        private string _mapperAssembly;
        private string _mapperTypeName;
		private string _controlAssembly;
		private string _controlTypeName;
		private Type _controlType;
		private bool _editable;
        private readonly Hashtable _parameters;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="controlType">The control type</param>
        /// <param name="mapperTypeName">The mapper type name</param>
        /// <param name="mapperAssembly">The mapper assembly</param>
        /// <param name="editable">Whether the control is read-only (cannot
        /// be edited directly)</param>
        /// <param name="parameters">The property attributes</param>
        public UIFormField(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly,
                           bool editable, Hashtable parameters)
			:this(label, propertyName, controlType, null, null, mapperTypeName, mapperAssembly, editable, parameters )
		{}

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="propertyName">The property name</param>
		/// <param name="controlTypeName">The control type name</param>
		/// <param name="controlAssembly">The control assembly</param>
        /// <param name="mapperTypeName">The mapper type name</param>
        /// <param name="mapperAssembly">The mapper assembly</param>
        /// <param name="editable">Whether the control is read-only (cannot
        /// be edited directly)</param>
        /// <param name="parameters">The property attributes</param>
        public UIFormField(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly,
                           bool editable, Hashtable parameters)
			: this(label, propertyName, null, controlTypeName, controlAssembly, mapperTypeName, mapperAssembly, editable, parameters)
		{}

        private UIFormField(string label, string propertyName, Type controlType, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly,
                           bool editable, Hashtable parameters)
        {
			if (controlType != null)
        	{
        		MyControlType = controlType;
        	}
        	else
        	{
        		_controlTypeName = controlTypeName;
        		_controlAssembly = controlAssembly;
        	}
            this._label = label;
            this._propertyName = propertyName;
            this._mapperTypeName = mapperTypeName;
            this._mapperAssembly = mapperAssembly;
            this._editable = editable;
            this._parameters = parameters;
            //this._controlType = controlType;
		}

		#region Properties

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

		///<summary>
		/// Returns the mapper assembly
		///</summary>
		public string MapperAssembly
		{
			get { return _mapperAssembly; }
		}

		/// <summary>
		/// The name of the property type assembly
		/// </summary>
		public string ControlAssemblyName
		{
			get { return _controlAssembly; }
			protected set
			{
				if (_controlAssembly != value)
				{
					_controlTypeName = null;
					_controlType = null;
				}
				_controlAssembly = value;
			}
		}

		/// <summary>
		/// The name of the control type
		/// </summary>
		public string ControlTypeName
		{
			get { return _controlTypeName; }
			protected set
			{
				if (_controlTypeName != value)
				{
					_controlType = null;
				}
				_controlTypeName = value;
			}
		}

        /// <summary>
        /// Returns the control type
        /// </summary>
        public Type ControlType
        {
            get { return MyControlType; }
			protected set { MyControlType = value; }
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
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        public Hashtable Parameters
        {
            get { return this._parameters; }
        }

		#endregion

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
			} else
			{
				return null;
			}
		}

		#region Type Initialisation

		private Type MyControlType
		{
			get
			{
				TypeLoader.LoadClassType(ref _controlType, _controlAssembly, _controlTypeName,
					"field", "field definition");
				return _controlType;
			}
			set
			{
				_controlType = value;
				TypeLoader.ClassTypeInfo(_controlType, out _controlAssembly, out _controlTypeName);
			}
		}

		#endregion Type Initialisation
    }
}