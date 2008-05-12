using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class ButtonWin : Button, IButton
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