namespace Habanero.UI.Base
{
    public interface IReadOnlyGridButtonsControl:IChilliControl
    {
        IButton this[string buttonName] { get; }
    }
}