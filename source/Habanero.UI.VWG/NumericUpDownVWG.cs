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
using System;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using HorizontalAlignment=Habanero.UI.Base.HorizontalAlignment;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a spin box (also known as an up-down control) that displays numeric values
    /// </summary>
    public class NumericUpDownVWG :NumericUpDown, INumericUpDown
    {
        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        /// <summary>
        /// Selects a range of text in the spin box (also known as an up-down control)
        /// specifying the starting position and number of characters to select
        /// </summary>
        /// <param name="i">The position of the first character to be selected</param>
        /// <param name="length">The total number of characters to be selected</param>
        public void Select(int i, object length)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// Gets or sets the alignment of text in the up-down control
        /// Gizmox does not support changing the TextAlign Property (Default value iss Left) 
        /// </summary>
        HorizontalAlignment INumericUpDown.TextAlign
        {
            get { return EnumerationConverter.HorizontalAlignmentToHabanero(TextAlign); }
            set { throw new System.NotSupportedException(); }
        }
    }
}