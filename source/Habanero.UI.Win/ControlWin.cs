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
            get { return (Base.DockStyle)Enum.Parse(typeof(Base.DockStyle), base.Dock.ToString()); }
            set { base.Dock = (System.Windows.Forms.DockStyle)Enum.Parse(typeof(System.Windows.Forms.DockStyle), value.ToString()); }
        }
    }
}