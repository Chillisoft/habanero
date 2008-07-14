using System;
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
        /// <summary>
        /// Gets or sets the docking style of this control - this can be none, top, bottom, left, right or fill, 
        /// depending on how you want your  control to dock inside its container control
        /// See <see cref="Habanero.UI.Base.DockStyle"/>
        /// </summary>
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }
    }
}