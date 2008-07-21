namespace Habanero.UI.Base
{
    public interface IOKCancelDialogFactory
    {
        IOKCancelPanel CreateOKCancelPanel(IControlChilli nestedControl);
        IFormChilli CreateOKCancelForm(IControlChilli nestedControl);
    }

    public interface IOKCancelPanel: IPanel
    {
        IButton OKButton { get; }
    }
}