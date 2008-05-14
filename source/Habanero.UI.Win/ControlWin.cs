using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ControlWin : Control, IControlChilli
    {
        IList IControlChilli.Controls
        {
            get { return this.Controls; }
        }
        //List<IControlChilli> IControlChilli.Controls
        //{
        //    get
        //    {
        //        return new List<IControlChilli>();
        //    }
        //}
    }
}