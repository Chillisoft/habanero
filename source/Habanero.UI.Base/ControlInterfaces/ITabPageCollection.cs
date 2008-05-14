namespace Habanero.UI.Base
{
    public interface ITabPageCollection
    {
        int Add(ITabPage page);

        IControlChilli this[int i] { get; }

        int Count { get; }
    }
}