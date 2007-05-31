namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// An interface to model a grid that can be edited
    /// </summary>
    public interface IEditableGrid : IGrid
    {
        /// <summary>
        /// Commits changes that have been made in the grid contents
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Removes changes that have been made in the grid contents and
        /// restores previous values
        /// </summary>
        void RejectChanges();
    }
}