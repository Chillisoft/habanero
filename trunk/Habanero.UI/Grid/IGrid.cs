using Habanero.Base;
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
        /// <param name="dataProvider">The grid data provider</param>
        void SetGridDataProvider(IGridDataProvider dataProvider);

        /// <summary>
        /// Saves the changes made to the grid by committing them to the
        /// database
        /// </summary>
        void SaveChanges();
    }
}