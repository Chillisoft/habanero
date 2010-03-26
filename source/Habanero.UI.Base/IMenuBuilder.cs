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
namespace Habanero.UI.Base
{
    ///<summary>
    /// An interface for the Menu Builder. This provides common functionality to build a a menu.
    ///</summary>
    public interface IMenuBuilder
    {
        ///<summary>
        /// Builds the Main Menu based on a <paramref name="habaneroMenu"/>
        ///</summary>
        ///<param name="habaneroMenu"></param>
        ///<returns></returns>
        IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu);

        /// <summary>
        /// Returns the control factory being used to create the Menu and the MenuItems
        /// </summary>
        IControlFactory ControlFactory { get; }
    }
}