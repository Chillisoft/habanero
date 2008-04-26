using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Gizmox
{
    public class TextBoxGiz : TextBox, ITextBox
    {
        private readonly IControlFactory _controlFactory;

        public TextBoxGiz(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
        }

        //private readonly TextBoxManager _manager = new TextBoxManager();

        //IList<IChilliControl> IControl.ChilliControls
        //{
        //    get { return _tb.ChilliControls; }
        //}
    }
}
