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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a related set of Controls. The controls managed by this control 
    ///  may be other Group Controls or may be Leaf Controls. A Typical Example of
    /// I figure the pattern out better
    /// </summary>
    public interface IGroupControl : IControlHabanero
    {
//        /// <summary>
//        /// Gets the collection of tab pages in this tab control
//        /// </summary>
//        IControlCollection ChildControls { get; }

//        /// <summary>
//        /// Gets or sets the index of the currently selected ChildControl
//        /// </summary>
//        int SelectedIndex { get; set; }
//
//        /// <summary>
//        /// Gets or sets the currently selected tab page
//        /// </summary>
//        IControlHabanero SelectedChildControl { get; set; }
//
//        /// <summary>
//        /// Occurs when the SelectedIndex property is changed
//        /// </summary>
//        event EventHandler SelectedIndexChanged;


        /// <summary>
        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
        ///    wrapped in the appropriate Child Control Type.
        /// </summary>
        /// <param name="contentControl">The control that is being placed as a child within this control. The content control could be 
        ///  a Panel of <see cref="IBusinessObject"/>.<see cref="IBOProp"/>s or any other child control</param>
        /// <param name="headingText">The heading text that will be shown as the Header for this Group e.g. For a <see cref="ITabControl"/>
        ///   this will be the Text shown in the Tab for a <see cref="ICollapsiblePanelGroupControl"/> this will be the text shown
        ///   on the Collapse Panel and for an <see cref="IGroupBox"/> this will be the title of the Group Box.</param>
        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be.
        ///   This height along with any other spacing required will be used as the minimum height for the ChildControlCreated</param>
        /// <param name="minimumControlWidth">The minimum width that the <paramref name="contentControl"/> can be</param>
        /// <returns></returns>
        IControlHabanero AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth);

        //TODO  28 Feb 2009: Replace the IControlHabaneroWith the appropriate child control interface.
    }
}