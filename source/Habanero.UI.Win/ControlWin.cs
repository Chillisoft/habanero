using System.Collections;
using System.Windows.Forms;
using Habanero.UI;

namespace Habanero.UI.Win
{
    internal class ControlWin : Control, IChilliControl
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