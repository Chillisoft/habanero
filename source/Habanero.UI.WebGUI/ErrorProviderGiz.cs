//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    internal class ErrorProviderGiz : ErrorProvider, IErrorProvider
    {
        public string GetError(IControlChilli objControl)
        {
            return base.GetError((Control) objControl);
        }

        public ErrorIconAlignmentChilli GetIconAlignment(IControlChilli objControl)
        {
            return (ErrorIconAlignmentChilli) base.GetIconAlignment((Control) objControl);
        }

        public int GetIconPadding(IControlChilli objControl)
        {
            return base.GetIconPadding((Control) objControl);
        }

        public void SetError(IControlChilli objControl, string strValue)
        {
            base.SetError((Control) objControl, strValue);
        }

        public void SetIconAlignment(IControlChilli objControl, ErrorIconAlignmentChilli enmValue)
        {
            base.SetIconAlignment((Control) objControl, (ErrorIconAlignment) enmValue);
        }

        public void SetIconPadding(IControlChilli objControl, int intPadding)
        {
            base.SetIconPadding((Control) objControl, intPadding);
        }

        public ErrorBlinkStyleChilli BlinkStyleChilli
        {
            get { return (ErrorBlinkStyleChilli) base.BlinkStyle; }
            set { base.BlinkStyle = (ErrorBlinkStyle) value; }
        }
    }
}