using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class TextBoxGiz : TextBox, ITextBox
    {
        //private readonly IControlFactory _controlFactory;

        //public TextBoxGiz()
        //{
        //    //this._controlFactory = controlFactory;
        //}

        //private readonly TextBoxManager _manager = new TextBoxManager();

        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
        //List<IChilliControl> IChilliControl.Controls
        //{
        //    get
        //    {
        //        return new List<IChilliControl>();
        //    }
        //}
    }
}
