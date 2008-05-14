namespace Habanero.UI.Base
{
    public interface ICheckBox:IControlChilli
    {
        bool Checked { get; set; }

        string Text { get; set; }
    }
}
