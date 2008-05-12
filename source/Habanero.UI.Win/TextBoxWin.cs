using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class TextBoxWin : TextBox, ITextBox
    {
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