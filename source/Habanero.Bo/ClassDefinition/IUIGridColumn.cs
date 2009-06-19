using System;
using System.Collections;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    public interface IUIGridColumn
    {
        /// <summary>
        /// Returns the heading text that will be used for this column.
        /// </summary>
        string Heading { get; set; }

        /// <summary>
        /// Returns the property name
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Returns the grid control type
        /// </summary>
        Type GridControlType { get; set; }

        /// <summary>
        /// Indicates whether the column is editable
        /// </summary>
        bool Editable { get; set; }

        /// <summary>
        /// Returns the width
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Returns the horizontal alignment
        /// </summary>
        PropAlignment Alignment { get; set; }

        /// <summary>
        /// Returns the Hashtable containing the property parameters
        /// </summary>
        Hashtable Parameters { get; }

        /// <summary>
        /// Gets and sets the name of the grid control type
        /// </summary>
        String GridControlTypeName { get; set; }

        /// <summary>
        /// Gets and sets the assembly name of the grid control type
        /// </summary>
        String GridControlAssemblyName { get; set; }

        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        string GetHeading();

        ///<summary>
        /// Gets the heading for this grid column given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this grid column. </param>
        ///<returns> The heading for this grid column </returns>
        string GetHeading(IClassDef classDef);

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        /// TODO this should return a string
        object GetParameterValue(string parameterName);

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        IUIGridColumn Clone();
    }
}