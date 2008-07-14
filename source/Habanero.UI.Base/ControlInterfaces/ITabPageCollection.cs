namespace Habanero.UI.Base
{
    public interface ITabPageCollection
    {
        //int Add(ITabPage page);

        void Add(ITabPage page);

        ITabPage this[int i] { get; }

        int Count { get; }

        int IndexOf(ITabPage page);
    }
}