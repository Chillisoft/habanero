using System;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class TreeViewWin : TreeView, ITreeView
    {       
        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>  
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)Enum.Parse(typeof(Base.DockStyle), base.Dock.ToString()); }
            set { base.Dock = (System.Windows.Forms.DockStyle)Enum.Parse(typeof(System.Windows.Forms.DockStyle), value.ToString()); }
        }
    }
}