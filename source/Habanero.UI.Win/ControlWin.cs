using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ControlWin : Control, IControlChilli
    {
        IControlCollection IControlChilli.Controls
        {
            get {
                return new ControlCollectionWin(base.Controls); 
            }
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