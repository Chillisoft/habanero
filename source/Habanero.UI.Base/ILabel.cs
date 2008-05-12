namespace Habanero.UI.Base
{
    public interface ILabel:IChilliControl
    {
        string Text { get; set; }

        int PreferredWidth { get; }
    }
}