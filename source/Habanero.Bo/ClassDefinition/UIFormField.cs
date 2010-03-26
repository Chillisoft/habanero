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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a property definition for a control in a user interface editing
    /// form, as specified in the class definitions xml file
    /// </summary>
    public class UIFormField : IUIFormField
    {
        private string _controlAssembly;
		private string _controlTypeName;
		private Type _controlType;
        // private readonly ITriggerCol _triggers;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="propertyName">The property name</param>
        public UIFormField(string label, string propertyName)
            : this(label, propertyName, null, null, null, null, true, null, null, new Hashtable(), LayoutStyle.Label)
        { } 
        
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
        /// <param name="layout">The <see cref="LayoutStyle"/> to use</param>
        public UIFormField(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly,
                           bool editable, string toolTipText, Hashtable parameters, LayoutStyle layout)
            : this(label, propertyName, controlType, null, null, mapperTypeName, mapperAssembly, editable
            , null, toolTipText, parameters, layout)
        { }

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
        /// <param name="showAsComulsory"></param>
        /// <param name="toolTipText">The tool tip text to be used.</param>
        /// <param name="parameters">The property attributes</param>
        /// <param name="layout">The <see cref="LayoutStyle"/> to use</param>
        public UIFormField(string label, string propertyName, string controlTypeName
            , string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable
            , bool? showAsComulsory, string toolTipText, Hashtable parameters, LayoutStyle layout)
			: this(label, propertyName, null, controlTypeName, controlAssembly,
                    mapperTypeName, mapperAssembly, editable, showAsComulsory, toolTipText, parameters, layout)
		{}

        /// <summary>
        /// The master constructor for all of the possible arguments
        /// </summary>
        private UIFormField(string label, string propertyName, Type controlType, string controlTypeName
            , string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable
            , bool? showAsCompulsory, string toolTipText, Hashtable parameters, LayoutStyle layout)
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
            Label = label;
            PropertyName = propertyName;
            MapperTypeName = mapperTypeName;
            MapperAssembly = mapperAssembly;
            Editable = editable;
            ToolTipText = toolTipText;
            Parameters = parameters;
            ShowAsCompulsory = showAsCompulsory;
            //_controlType = controlType;
            //_triggers = triggers ?? new TriggerCol();
            Layout = layout;
        }

		#region Properties

        /// <summary>
        /// Returns the label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Returns the mapper type name
        /// </summary>
        public string MapperTypeName { get; set; }

        ///<summary>
        /// Returns the mapper assembly
        ///</summary>
        public string MapperAssembly { get; private set; }

        /// <summary>
		/// The name of the property type assembly
		/// </summary>
		public string ControlAssemblyName
		{
			get { return _controlAssembly; }
			set
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
			set
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
			set { MyControlType = value; }
        }

        /// <summary>
        /// Indicates whether the control is editable
        /// </summary>
        public bool Editable { get; set; }

        ///<summary>
        /// Returns the text that will be shown in the Tool Tip for the control.
        ///</summary>
        public string ToolTipText { get; private set; }

        /// <summary>
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        public Hashtable Parameters { get; private set; }

        ///// <summary>
        ///// Returns the collection of triggers managed by this
        ///// field
        ///// </summary>
        //public ITriggerCol Triggers
        //{
        //    get { return _triggers; }
        //}

		#endregion

        #region Helper Methods

        ///<summary>
        /// Gets the property definition for the property that this field refers to.
        /// This property could be on a related object. If the property is not found, then 
        /// nul is returned.
        ///</summary>
        ///<param name="classDef">The class definition that this field is for.</param>
        ///<returns>The property definition that is refered to, otherwise null. </returns>
        public IPropDef GetPropDefIfExists(IClassDef classDef)
        {
            return ClassDefHelper.GetPropDefByPropName(classDef, PropertyName);
        }

        ///<summary>
        /// Gets the text that will be shown in the tool tip for the control.
        ///</summary>
        /// <returns> The text that will be used for the tool tip for this control. </returns>
        public string GetToolTipText()
        {
            return GetToolTipText(GetClassDef());
        }

        ///<summary>
        /// Gets the text that will be shown in the tool tip for the control given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this form field. </param>
        /// <returns> The text that will be used for the tool tip for this control. </returns>
        public string GetToolTipText(IClassDef classDef)
        {
            if (!String.IsNullOrEmpty(ToolTipText))
            {
                return ToolTipText;
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
        public string GetLabel(IClassDef classDef)
        {
            if (!String.IsNullOrEmpty(Label))
            {
                return Label + GetIsCompulsoryIndicator();
            }
            string label = null;
            IPropDef propDef = GetPropDefIfExists(classDef);
            if (propDef != null)
            {
                label = propDef.DisplayNameFull;
            }
            if (String.IsNullOrEmpty(label))
            {
                label = StringUtilities.DelimitPascalCase(PropertyName, " ");
            }
            return label + LabelSuffix + GetIsCompulsoryIndicator();
        }

        private string GetIsCompulsoryIndicator()
        {
            return IsCompulsory ? " *" : "";
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

        /// <summary>
        /// Hashcode for the UIFormField
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.PropertyName + this.ControlTypeName + this.Label).GetHashCode();
        }

        /// <summary>
        /// Indicates whether two fields are equal
        /// </summary>
        public static bool operator ==(UIFormField a, UIFormField b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
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

        /// <summary>
        /// Indicates whether two fields are unequal
        /// </summary>
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
            return HasParameterValue(parameterName) ? this.Parameters[parameterName] : null;
        }


     
        ///<summary>
        /// Returns true if the UIFormField has a paramter value.
        ///</summary>
        ///<param name="parameterName"></param>
        ///<returns></returns>
        public bool HasParameterValue(string parameterName)
        {
            return (this.Parameters.ContainsKey(parameterName));
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

        ///<summary>
        /// How many rows the Field must span.
        ///</summary>
        ///<exception cref="InvalidXmlDefinitionException"></exception>
        public int RowSpan
        {
            get
            {
                if (HasParameterValue("rowSpan"))
                {
                     try
                     {
                         return Convert.ToInt32(GetParameterValue("rowSpan"));
                     }catch (FormatException)
                    {
                        throw new InvalidXmlDefinitionException(
                            "An error occurred while reading the 'rowSpan' parameter from the class definitions.  The 'value' attribute must be a valid integer.");
                    }
                }
                if (HasParameterValue("numLines"))
                {
                    try
                    {
                        return Convert.ToInt32(GetParameterValue("numLines"));

                    } catch (FormatException)
                    {
                        throw new InvalidXmlDefinitionException(
                            "An error occurred while reading the 'numLines' parameter from the class definitions.  The 'value' attribute must be a valid integer.");
                    }
                }
                return 1;
            }
        }

        ///<summary>
        /// How many columns the field must span
        ///</summary>
        public int ColSpan
        {
            get { return HasParameterValue("colSpan") ? Convert.ToInt32(GetParameterValue("colSpan")) : 1; }
        }

        ///<summary>
        /// Is the field compulsory (i.e. must it be shown as compulsory on the form or not)
        ///</summary>
        public bool IsCompulsory
        {
            get {
                if (this.ShowAsCompulsory.GetValueOrDefault(false))
                {
                    return true;
                }
                IClassDef def = GetClassDef();
                if (def == null) return false;
                IPropDef propDef = this.GetPropDefIfExists(def);
                return propDef != null && propDef.Compulsory;
            }
        }

        ///<summary>
        /// The <see cref="UIFormColumn"/> that this form field is to be placed in.
        ///</summary>
        public IUIFormColumn UIFormColumn { get; set; }

        ///<summary>
        /// Returns the alignment property of the form field or null if none is provided
        ///</summary>
        public string Alignment
        {
            get { return HasParameterValue("alignment") ? Convert.ToString(GetParameterValue("alignment")) : null; }
        }

        ///<summary>
        /// Returns the decimalPlaces property from the form field or null if none is provided
        ///</summary>
        public string DecimalPlaces
        {
            get { return HasParameterValue("decimalPlaces") ? Convert.ToString(GetParameterValue("decimalPlaces")) : null; }
        }

        ///<summary>
        /// Returns the Options property from the form field or null if none is provided
        ///</summary>
        public string Options
        {
            get { return HasParameterValue("options") ? Convert.ToString(GetParameterValue("options")) : null; }
        }

        ///<summary>
        /// Returns the IsEmail property from the form field or null if none is provided
        ///</summary>
        public string IsEmail
        {
            get { return HasParameterValue("isEmail") ? Convert.ToString(GetParameterValue("isEmail")) : null; }
        }

        ///<summary>
        /// Returns the DateFormat property from the form field or null if none is provided
        ///</summary>
        public string DateFormat
        {
            get { return HasParameterValue("dateFormat") ? Convert.ToString(GetParameterValue("dateFormat")) : null; }
        }

        ///<summary>
        /// The <see cref="LayoutStyle"/> to be used for this form field.
        ///</summary>
        public LayoutStyle Layout { get; set; }

        /// <summary>
        /// Must the Field Show Itself as Compulsory or not
        /// </summary>
        private bool? ShowAsCompulsory { get; set; }

        private IClassDef GetClassDef()
        {
            IUIFormColumn column = this.UIFormColumn;
            if (column == null) return null;
            IUIDef uiDef = column.UIFormTab.UIForm.UIDef;
            if (uiDef == null) return null;
            if (uiDef.ClassDef != null) return uiDef.ClassDef;
            return uiDef.UIDefCol == null ? null : uiDef.UIDefCol.ClassDef;
        }

        #endregion Type Initialisation

       
    }
}