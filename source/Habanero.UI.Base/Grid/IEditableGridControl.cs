using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IEditableGridControl:IGridControl
    {
        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default ui definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        void SetBusinessObjectCollection(IBusinessObjectCollection boCollection);
    }
}