using Habanero.Base;

namespace Habanero.UI.Base
{
    ///<summary>
    /// Interface for a control that displays a collection of a Business Object along side an editor/creator panel.
    ///</summary>
    public interface IGridAndBOEditorControl : IControlHabanero
    {
        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        void SetBusinessObjectCollection(IBusinessObjectCollection col);

        IReadOnlyGridControl ReadOnlyGridControl { get; }
        IBusinessObjectControlWithErrorDisplay BusinessObjectControl { get; }
        IButtonGroupControl ButtonGroupControl { get; }
        IBusinessObject CurrentBusinessObject { get; }
    }


}