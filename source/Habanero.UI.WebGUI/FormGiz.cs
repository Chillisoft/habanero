using System;
using System.Collections.Generic;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class FormGiz : Form, IFormChilli
    {
        private IControlCollection _controls;

        IControlCollection IControlChilli.Controls
        {
            get { return _controls; }
        }

        
        public void Refresh()
        {
            // do nothing
        }

        IFormChilli IFormChilli.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form)value; }
        }
    }
}
