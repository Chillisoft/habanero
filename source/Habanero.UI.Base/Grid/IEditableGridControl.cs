using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IEditableGridControl
    {
        IEditableGrid Grid { get; }
        void Initialise(ClassDef classDef);
        void Initialise(ClassDef classDef, string uiDefName);
    }
}