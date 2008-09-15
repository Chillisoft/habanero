namespace Habanero.UI.Base
{
    public interface IMenuItemCollection
    {
        int Count { get; }
        IMenuItem this[int index] { get; }
        void Add(IMenuItem menuItem);
    }
}