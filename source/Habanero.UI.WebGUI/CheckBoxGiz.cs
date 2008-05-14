using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class CheckBoxGiz : CheckBox, ICheckBox
    {
        //ICollection IControlChilli.Controls
        //{
        //    get { return this.Controls; }
        //}
        //List<IControlChilli> IControlChilli.Controls
        //{
        //    get
        //    {
        //        return new List<IControlChilli>();
        //    }
        //}
        IList IControlChilli.Controls
        {
            get {return base.Controls; }
        }
    }
}