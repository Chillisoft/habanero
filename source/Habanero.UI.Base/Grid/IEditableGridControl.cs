using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IEditableGridControl:IGridControl
    {
        void ApplyChangesToBusinessObject();
    }
}