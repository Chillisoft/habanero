// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// An implementation of <see cref="IButtonSizePolicy"/> that will size all the buttons equally based on the widest one. 
    /// </summary>
    public class ButtonSizePolicyVWG : IButtonSizePolicy
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use.</param>
        public ButtonSizePolicyVWG(IControlFactory controlFactory)
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
                if (lbl.PreferredWidth + 10 > maxButtonWidth)
                {
                    maxButtonWidth = lbl.PreferredWidth + 10;
                }
            }
            foreach (IButton btn in buttonCollection)
            {
                btn.Width = maxButtonWidth;
            }
        }
    }
}