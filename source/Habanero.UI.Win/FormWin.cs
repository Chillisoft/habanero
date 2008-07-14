using System;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class FormWin : Form, IFormChilli
    {
        private IControlCollection _controls;

        IControlCollection IControlChilli.Controls
        {
            get { return _controls; }
        }
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }

        IFormChilli IFormChilli.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form) value; }
        }
    }
}