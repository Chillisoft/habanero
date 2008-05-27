using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface ICollectionTabControlMapper
    {
        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        void SetBusinessObjectControl(IBusinessObjectControl boControl);

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="businessObjectCollection">The business object collection to create tab pages
        /// for</param>
        void SetCollection(IBusinessObjectCollection businessObjectCollection);

        /// <summary>
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        void TabChanged();

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        ITabControl TabControl { get; }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        IBusinessObject GetBo(ITabPage tabPage);

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        ITabPage GetTabPage(IBusinessObject bo);

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        IBusinessObject CurrentBusinessObject { get; set; }
    }
}