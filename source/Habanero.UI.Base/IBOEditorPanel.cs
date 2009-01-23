using Habanero.Base;

namespace Habanero.UI.Base
{
    public interface IBOEditorPanel
    {
        IBusinessObject BusinessObject { get; set; }
        void DisplayErrors();
        void ClearErrors();
    }
}