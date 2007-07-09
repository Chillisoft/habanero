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
        /// Sets the grid's data provider to that provided
        /// </summary>
        /// <param name="boCol">The collection</param>
        /// <param name="uiName">The ui to use</param>
        void SetCollection(BusinessObjectCollection<BusinessObject> boCol, string uiName);

        /// <summary>
        /// Saves the changes made to the grid by committing them to the
        /// database
        /// </summary>
        void SaveChanges();
    }
}