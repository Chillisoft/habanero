namespace Habanero.UI
{
    public interface ILabel:IChilliControl
    {
        string Text { get; set; }

        int PreferredWidth { get; }
    }
}