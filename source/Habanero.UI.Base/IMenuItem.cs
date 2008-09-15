namespace Habanero.UI.Base
{
    public interface IMenuItem
    {
        string Text { get; }
        IMenuItemCollection MenuItems { get; }
        void PerformClick();
    }
}