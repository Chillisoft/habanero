using Habanero.UI.Grid;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// An interface that models a grid that has buttons
    /// </summary>
    public interface IGridWithButtons
    {
        /// <summary>
        /// Returns the grid object
        /// </summary>
        GridBase Grid { get; }

        /// <summary>
        /// Saves the changes made to the grid, committing them to the
        /// database
        /// </summary>
        void SaveChanges();
    }
}