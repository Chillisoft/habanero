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

        IFormChilli IFormChilli.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form) value; }
        }
    }
}