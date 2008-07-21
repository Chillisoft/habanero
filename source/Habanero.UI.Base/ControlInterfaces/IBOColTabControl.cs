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

using Habanero.Base;

namespace Habanero.UI.Base
{
    public interface IBOColTabControl: IControlChilli
    {
        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="value">The business object control that is
        /// displaying the business object information in the tab page</param>
        IBusinessObjectControl BusinessObjectControl { set; get;}

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="value">The business object collection to create tab pages
        /// for</param>
        IBusinessObjectCollection BusinessObjectCollection { set; get; }

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

    }
}