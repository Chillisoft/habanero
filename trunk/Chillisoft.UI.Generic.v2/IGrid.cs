using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
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