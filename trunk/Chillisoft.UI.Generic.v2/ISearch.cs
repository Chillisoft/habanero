namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// An interface to model a search facility
    /// </summary>
    /// TODO ERIC - is this needed?
    public interface ISearch
    {
        /// <summary>
        /// Returns a grid data provider
        /// </summary>
        /// <returns>Returns a grid data provider</returns>
        IGridDataProvider Search();
    }
}