using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.Base
{
    /// <summary>
    /// Provides an interface for a UI definition.  This consists of a <see cref="IUIGrid"/> and <see cref="IUIForm"/> definition.
    /// </summary>
    public interface IUIDef
    {
        /// <summary>
        /// Returns the form definition
        /// </summary>
        IUIForm UIForm { get; set; }

        /// <summary>
        /// Returns the name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Returns the grid definition
        /// </summary>
        IUIGrid UIGrid { get; set; }

        ///<summary>
        /// Gets a Collection of UIDefs
        ///</summary>
        UIDefCol UIDefCol { get; set; }

        /// <summary>
        /// The Class Definition that this UIDef belongs to.
        /// </summary>
        IClassDef ClassDef { get; set; }

        /// <summary>
        /// Returns the form property definitions
        /// </summary>
        /// <returns>Returns a UIForm object</returns>
        [Obsolete("Please use the UIForm property instead as it returns the same UIForm. This method will be removed in later versions of Habanero")]
        IUIForm GetUIFormProperties();

        /// <summary>
        /// Returns the grid property definitions
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        [Obsolete("Please use the UIGrid property instead as it returns the same UIGrid. This method will be removed in later versions of Habanero")]
        IUIGrid GetUIGridProperties();

        ///<summary>
        /// Returns the form field for this UIDefinition for the property specified.
        /// If the form field for the property is not defined in the uidef then null is returned.
        ///</summary>
        ///<param name="propertyName">The property name that you want the form field for</param>
        ///<returns>the form field or null</returns>
        IUIFormField GetFormField(string propertyName);

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        IUIDef Clone();
    }
}