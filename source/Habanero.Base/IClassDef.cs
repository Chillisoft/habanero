using System;
using Habanero.Base.Exceptions;
using Habanero.BO.Base;

namespace Habanero.Base
{
    public interface IClassDef
    {
        /// <summary>
        /// Searches the property definition collection and returns the 
        /// property definition for the property with the name provided.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the property definition if found, or
        /// throws an error if not</returns>
        /// <exception cref="InvalidPropertyNameException">
        /// This exception is thrown if the property is not found</exception>
        IPropDef GetPropDef(string propertyName);

        /// <summary>
        /// Searches the property definition collection and returns 
        /// the lookup-list found under the property with the
        /// name specified.  Also checks the super-class.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the lookup-list if the property is
        /// found, or a NullLookupList object if not</returns>
        ILookupList GetLookupList(string propertyName);


        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns the new object</returns>
        IBusinessObject CreateNewBusinessObject();

        /// <summary>
        /// The table this classdef maps to, if applicable.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// The type of the class definition
        /// </summary>
        Type ClassType
        {
            get;
            set;
        }

        /// <summary>
        /// The collection of property definitions
        /// </summary>
        IPropDefCol PropDefcol
        {
            get;
            set;
        }

        /// <summary>
        /// The collection of property definitions for this
        /// class and any properties inherited from parent classes
        /// </summary>
        IPropDefCol PropDefColIncludingInheritance
        {
            get;
        }

        /// <summary>
        /// Returns the name of the table that applies to the propdef given, taking into allowance
        /// any inheritance structure.
        /// </summary>
        /// <param name="propDef">The propdef to map to a table name. This propdef must be part of this classdef heirarchy</param>
        /// <returns></returns>
        string GetTableName(IPropDef propDef);

        ///<summary>
        /// Gets the type of the specified property for this classDef.
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        Type GetPropertyType(string propertyName);

        ///<summary>
        /// Creates a property comparer for the given property
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        IPropertyComparer<T> CreatePropertyComparer<T>(string propertyName) where T:IBusinessObject;

        /// <summary>
        /// Returns the table name for this class
        /// </summary>
        /// <returns>Returns the table name of first real table for this class.</returns>
        string GetTableName();
    }
}