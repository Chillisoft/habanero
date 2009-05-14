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
using System.ComponentModel;
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Displays a business object collection in a tab control, with one
    /// business object per tab.  Each tab holds a business control, provided
    /// by the developer, that refreshes to display the business object for
    /// the current tab.
    /// <br/>
    /// This control is suitable for a business object collection with a limited
    /// number of objects.
    /// </summary>
    public interface IBOColTabControl: IBOColSelectorControl
    {
        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.<vbr/>
        /// The business object control that is
        /// displaying the business object information in the tab page
        /// </summary>
        IBusinessObjectControl BusinessObjectControl { set; get;}

        /////////
        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler<TabPageEventArgs> TabPageAdded;
        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event System.EventHandler<TabPageEventArgs> TabPageRemoved;

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        ITabControl TabControl { get; }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        IBusinessObject GetBo(ITabPage tabPage);

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        ITabPage GetTabPage(IBusinessObject bo);

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        IBusinessObject CurrentBusinessObject { get; set; }

        /// <summary>
        /// Gets and Sets the Business Object Control Creator. This is a delegate for creating a
        ///  Business Object Control. This can be used as an alternate to setting the control
        /// on the <see cref="IBOColTabControl"/> so that a different instance of the control
        ///  is created for each tab instead of them  using the same control with diff data.
        /// This has been created for performance reasons.
        /// </summary>
        BusinessObjectControlCreatorDelegate BusinessObjectControlCreator { get; set; }
    }
}