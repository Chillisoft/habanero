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
using System.Collections.Generic;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Creates panels for displaying business object details on a form.  Use
    /// CreatePanel to create the panel and catch the <see cref="IPanelFactoryInfo" /> generated,
    /// which contains all the information relating to the panel, including the controls, the
    /// mappers, the business object and the panel control.
    /// </summary>
    [Obsolete("Panelfactory and PanelFactoryInfo is soon to be replaced by PanelBuilder and PanelInfo.")]
    public interface IPanelFactory
    {
        /// <summary>
        /// Creates a panel to display a business object
        /// </summary>
        /// <returns>Returns the panel info object containing the panel</returns>
        IPanelFactoryInfo CreatePanel();

        /// <summary>
        /// Creates one panel for each UI Form definition of a business object
        /// </summary>
        /// <returns>Returns the list of panel info objects created</returns>
        List<IPanelFactoryInfo> CreateOnePanelPerUIFormTab();
    }
}