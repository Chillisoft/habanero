namespace Habanero.UI.Base
{
    public interface IComboBoxObjectCollection
    {
        void Add(object item);
        int Count { get; }
        void Remove(object item);
        void Clear();
    }
}