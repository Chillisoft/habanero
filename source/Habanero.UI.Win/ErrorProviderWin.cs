using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ErrorProviderWin:ErrorProvider,IErrorProvider
    {
        public string GetError(IControlChilli objControl)
        {
            return base.GetError((Control)objControl);
        }

        public ErrorIconAlignmentChilli GetIconAlignment(IControlChilli objControl)
        {
            return (ErrorIconAlignmentChilli)base.GetIconAlignment((Control)objControl);
        }

        public int GetIconPadding(IControlChilli objControl)
        {
            return base.GetIconPadding((Control)objControl);

        }

        public void SetError(IControlChilli objControl, string strValue)
        {
            base.SetError((Control)objControl, strValue);
        }

        public void SetIconAlignment(IControlChilli objControl, ErrorIconAlignmentChilli enmValue)
        {
            base.SetIconAlignment((Control)objControl, (ErrorIconAlignment)enmValue);

        }

        public void SetIconPadding(IControlChilli objControl, int intPadding)
        {
            base.SetIconPadding((Control)objControl, intPadding);

        }


        public ErrorBlinkStyleChilli BlinkStyleChilli
        {
            get { return (ErrorBlinkStyleChilli)base.BlinkStyle; }
            set { base.BlinkStyle = (ErrorBlinkStyle)value; }
        }
    }
}
