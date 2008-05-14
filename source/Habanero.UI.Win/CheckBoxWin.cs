using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class CheckBoxWin : CheckBox, ICheckBox
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