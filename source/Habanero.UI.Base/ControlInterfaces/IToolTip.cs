namespace Habanero.UI.Base
{
    public interface IToolTip
    {
        void SetToolTip(IControlChilli label, string toolTipText);
        string GetToolTip(IControlChilli controlChilli);
    }
}