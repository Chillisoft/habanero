using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class CheckBoxGiz : CheckBox, ICheckBox
    {
        //ICollection IChilliControl.Controls
        //{
        //    get { return this.Controls; }
        //}
        //List<IChilliControl> IChilliControl.Controls
        //{
        //    get
        //    {
        //        return new List<IChilliControl>();
        //    }
        //}
        IList IChilliControl.Controls
        {
            get {return base.Controls; }
        }
    }
}