using Habanero.Base;
using Habanero.Bo;
using Habanero.Ui.Base;

namespace Habanero.Ui.Grid
{
    /// <summary>
    /// An interface to model a grid in a user interface
    /// </summary>
    public interface IGrid
    {
        /// <summary>
        /// Sets the object initialiser to that provided
        /// </summary>
        IObjectInitialiser ObjectInitialiser { set; }

        /// <summary>
        /// Sets the business object collection to display in the grid, along
        /// with the ui definition used to format the display.  The ui name
        /// is obtained from the 'name' attribute in the 'ui' element.
        /// </summary>
        /// <param name="boCol">The business object collection</param>
        /// <param name="uiName">The ui definition name to use</param>
        void SetCollection(BusinessObjectCollection<BusinessObject> boCol, string uiName);

        /// <summary>
        /// Saves the changes made to the grid by committing them to the
        /// database
        /// </summary>
        void SaveChanges();
    }
}