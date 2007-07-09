using Habanero.Ui.Base;

namespace Habanero.Ui.Base
{
    /// <summary>
    /// An interface to model a search facility
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// Returns a grid data provider
        /// </summary>
        /// <returns>Returns a grid data provider</returns>
        IGridDataProvider Search();
    }
}