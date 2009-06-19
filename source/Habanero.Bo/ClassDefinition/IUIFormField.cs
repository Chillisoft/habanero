using System;
using System.Collections;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{
    public interface IUIFormField
    {
        /// <summary>
        /// Returns the property name
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Returns the mapper type name
        /// </summary>
        string MapperTypeName { get; set; }

        ///<summary>
        /// Returns the mapper assembly
        ///</summary>
        string MapperAssembly { get; }

        /// <summary>
        /// The name of the property type assembly
        /// </summary>
        string ControlAssemblyName { get; set; }

        /// <summary>
        /// The name of the control type
        /// </summary>
        string ControlTypeName { get; set; }

        /// <summary>
        /// Returns the control type
        /// </summary>
        Type ControlType { get; set; }

        /// <summary>
        /// Indicates whether the control is editable
        /// </summary>
        bool Editable { get;set; }

        ///<summary>
        /// Returns the text that will be shown in the Tool Tip for the control.
        ///</summary>
        string ToolTipText { get; }

        /// <summary>
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        Hashtable Parameters { get; }

        /// <summary>
        /// Returns the collection of triggers managed by this
        /// field
        /// </summary>
        ITriggerCol Triggers { get; }

        ///<summary>
        /// How many rows the Field must span.
        ///</summary>
        ///<exception cref="InvalidXmlDefinitionException"></exception>
        int RowSpan { get; }

        ///<summary>
        /// How many columns the field must span
        ///</summary>
        int ColSpan { get; }

        ///<summary>
        /// Is the field compulsory (i.e. must it be shown as compulsory on the form or not)
        ///</summary>
        bool IsCompulsory { get; }

        ///<summary>
        /// The <see cref="UIFormColumn"/> that this form field is to be placed in.
        ///</summary>
        IUIFormColumn UIFormColumn { get; set; }

        ///<summary>
        /// Returns the alignment property of the form field or null if none is provided
        ///</summary>
        string Alignment { get; }

        ///<summary>
        /// Returns the decimalPlaces property from the form field or null if none is provided
        ///</summary>
        string DecimalPlaces { get; }

        ///<summary>
        /// Returns the Options property from the form field or null if none is provided
        ///</summary>
        string Options { get; }

        ///<summary>
        /// Returns the IsEmail property from the form field or null if none is provided
        ///</summary>
        string IsEmail { get; }

        ///<summary>
        /// Returns the DateFormat property from the form field or null if none is provided
        ///</summary>
        string DateFormat { get; }

        ///<summary>
        /// The <see cref="LayoutStyle"/> to be used for this form field.
        ///</summary>
        LayoutStyle Layout { get; set; }

        /// <summary>
        /// Returns the label
        /// </summary>
        string Label { get; set; }

        ///<summary>
        /// Gets the property definition for the property that this field refers to.
        /// This property could be on a related object. If the property is not found, then 
        /// nul is returned.
        ///</summary>
        ///<param name="classDef">The class definition that this field is for.</param>
        ///<returns>The property definition that is refered to, otherwise null. </returns>
        IPropDef GetPropDefIfExists(IClassDef classDef);

        ///<summary>
        /// Gets the text that will be shown in the tool tip for the control.
        ///</summary>
        /// <returns> The text that will be used for the tool tip for this control. </returns>
        string GetToolTipText();

        ///<summary>
        /// Gets the text that will be shown in the tool tip for the control given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this form field. </param>
        /// <returns> The text that will be used for the tool tip for this control. </returns>
        string GetToolTipText(IClassDef classDef);

        ///<summary>
        /// Gets the label for this form field.
        ///</summary>
        ///<returns> The label for this form field </returns>
        string GetLabel();

        ///<summary>
        /// Gets the label for this form field given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this form field. </param>
        ///<returns> The label for this form field </returns>
        string GetLabel(IClassDef classDef);

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        object GetParameterValue(string parameterName);

        ///<summary>
        /// Returns true if the UIFormField has a paramter value.
        ///</summary>
        ///<param name="parameterName"></param>
        ///<returns></returns>
        bool HasParameterValue(string parameterName);
    }
}