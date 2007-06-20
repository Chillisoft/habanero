using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Generic;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// An interface to model a provider of data to a grid
    /// </summary>
    public interface IGridDataProvider
    {
        /// <summary>
        /// Returns the business object collection being represented
        /// </summary>
        /// <returns>Returns the business object collection</returns>
        BusinessObjectCollection GetCollection();

        /// <summary>
        /// Returns the UIGridDef object
        /// </summary>
        /// <returns>Returns the UIGridDef object</returns>
        UIGridDef GetUIGridDef();

        /// <summary>
        /// Sets the parent object to that specified
        /// </summary>
        /// <param name="parentObject">The parent object</param>
        void SetParentObject(object parentObject);

        /// <summary>
        /// Returns the class definition being held
        /// </summary>
        ClassDef ClassDef { get; }
    }
}