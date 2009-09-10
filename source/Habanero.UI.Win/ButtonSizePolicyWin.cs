// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// An implementation of <see cref="IButtonSizePolicy"/> that will size all the buttons equally based on the widest one.  It also maintains a minimum
    /// button size equal to the screen resolution / 20.
    /// </summary>
    public class ButtonSizePolicyWin : IButtonSizePolicy
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use.</param>
        public ButtonSizePolicyWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Recalculates the button sizes of the given collection of buttons.
        /// </summary>
        /// <param name="buttonCollection"></param>
        public void RecalcButtonSizes(IControlCollection buttonCollection)
        {
            int maxButtonWidth = 0;
            foreach (IButton btn in buttonCollection)
            {
                ILabel lbl = _controlFactory.CreateLabel(btn.Text);
                if (lbl.PreferredWidth + 15 > maxButtonWidth)
                {
                    maxButtonWidth = lbl.PreferredWidth + 15;
                }
            }
            if (maxButtonWidth < Screen.PrimaryScreen.Bounds.Width / 20)
            {
                maxButtonWidth = Screen.PrimaryScreen.Bounds.Width / 20;
            }
            foreach (IButton btn in buttonCollection)
            {
                btn.Width = maxButtonWidth;
            }
        }
    }
}