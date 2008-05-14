namespace Habanero.UI.Base
{
    public interface ITabControl : IControlChilli
    {
        IDockStyle Dock { get; set; }

        ITabPageCollection TabPages { get; }
    }
}