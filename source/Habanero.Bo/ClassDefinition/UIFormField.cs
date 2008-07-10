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
using Habanero.Base;
using Habanero.Util;
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
        private TriggerCol _triggers;
        private string _toolTipText;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="controlType">The control type</param>
        /// <param name="mapperTypeName">The mapper type name</param>
        /// <param name="mapperAssembly">The mapper assembly</param>
        /// <param name="editable">Whether the control is editable or not</param>
        /// <param name="toolTipText">The tool tip text to be used.</param>
        /// <param name="parameters">The property attributes</param>
        /// <param name="triggers">The collection of triggers managed by the field</param>
        public UIFormField(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly,
                           bool editable, string toolTipText, Hashtable parameters, TriggerCol triggers)
            : this(label, propertyName, controlType, null, null, mapperTypeName, mapperAssembly, editable, toolTipText, parameters, triggers)
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
        /// <param name="editable">Whether the control is editable or not</param>
        /// <param name="toolTipText">The tool tip text to be used.</param>
        /// <param name="parameters">The property attributes</param>
        /// <param name="triggers">The collection of triggers managed by the field</param>
        public UIFormField(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly,
                           bool editable, string toolTipText, Hashtable parameters, TriggerCol triggers)
			: this(label, propertyName, null, controlTypeName, controlAssembly,
                    mapperTypeName, mapperAssembly, editable, toolTipText, parameters, triggers)
		{}

        /// <summary>
        /// The master constructor for all of the possible arguments
        /// </summary>
        private UIFormField(string label, string propertyName, Type controlType, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly,
                           bool editable, string toolTipText, Hashtable parameters, TriggerCol triggers)
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
            if (parameters == null) parameters = new Hashtable(0);
            _label = label;
            _propertyName = propertyName;
            _mapperTypeName = mapperTypeName;
            _mapperAssembly = mapperAssembly;
            _editable = editable;
            _toolTipText = toolTipText;
            _parameters = parameters;
            //_controlType = controlType;
            _triggers = triggers;
            if (_triggers == null)
            {
                _triggers = new TriggerCol();
            }
		}

		#region Properties

		/// <summary>
        /// Returns the label
        /// </summary>
        internal string Label
        {
            get { return _label; }
            set { _label = value; }
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

        ///<summary>
        /// Returns the text that will be shown in the Tool Tip for the control.
        ///</summary>
        public string ToolTipText
        {
            get { return _toolTipText; }
        }

        /// <summary>
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        public Hashtable Parameters
        {
            get { return this._parameters; }
        }

        /// <summary>
        /// Returns the collection of triggers managed by this
        /// field
        /// </summary>
        public TriggerCol Triggers
        {
            get { return _triggers; }
        }

		#endregion

        #region Helper Methods

        ///<summary>
        /// Gets the property definition for the property that this field refers to.
        /// This property could be on a related object. If the property is not found, then 
        /// nul is returned.
        ///</summary>
        ///<param name="classDef">The class definition that this field is for.</param>
        ///<returns>The property definition that is refered to, otherwise null. </returns>
        public IPropDef GetPropDefIfExists(ClassDef classDef)
        {
            return ClassDefHelper.GetPropDefByPropName(classDef, PropertyName);
        }

        ///<summary>
        /// Gets the text that will be shown in the tool tip for the control.
        ///</summary>
        /// <returns> The text that will be used for the tool tip for this control. </returns>
        public string GetToolTipText()
        {
            return GetToolTipText(null);
        }

        ///<summary>
        /// Gets the text that will be shown in the tool tip for the control given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this form field. </param>
        /// <returns> The text that will be used for the tool tip for this control. </returns>
        public string GetToolTipText(ClassDef classDef)
        {
            if (!String.IsNullOrEmpty(_toolTipText))
            {
                return _toolTipText;
            }
            string toolTipText = null;
            IPropDef propDef = GetPropDefIfExists(classDef);
            if (propDef != null)
            {
                toolTipText = propDef.Description;
            }
            if (String.IsNullOrEmpty(toolTipText))
            {
                toolTipText = null;
            }
            return toolTipText;
        }

        ///<summary>
        /// Gets the label for this form field.
        ///</summary>
        ///<returns> The label for this form field </returns>
        public string GetLabel()
        {
            return GetLabel(null);
        }

        ///<summary>
        /// Gets the label for this form field given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this form field. </param>
        ///<returns> The label for this form field </returns>
        public string GetLabel(ClassDef classDef)
        {
            if (!String.IsNullOrEmpty(_label))
            {
                return _label;
            }
            string label = null;
            IPropDef propDef = GetPropDefIfExists(classDef);
            if (propDef != null)
            {
                label = propDef.DisplayNameFull;
            }
            if (String.IsNullOrEmpty(label))
            {
                label = StringUtilities.DelimitPascalCase(_propertyName, " ");
            }
            return label + LabelSuffix;
        }

        private string LabelSuffix
        {
            get
            {
                if (ControlTypeName != null && ControlTypeName.EndsWith("CheckBox"))
                {
                    return "?";
                }
                return ":";
            }
        }

        #endregion //Helper Methods

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            UIFormField otherFormField = obj as UIFormField;
            if ((object)otherFormField == null)
            {
                return false;
            }
            return otherFormField.PropertyName == this.PropertyName &&
                otherFormField.ControlTypeName == this.ControlTypeName &&
                otherFormField.Label == this.Label;
        }
        public static bool operator ==(UIFormField a, UIFormField b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(UIFormField a, UIFormField b)
        {
            return !(a == b);
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
			    _controlTypeName = _controlType.Name;
			    _controlAssembly = _controlType.Namespace;
			}
		}
               

        #endregion Type Initialisation
    }
}