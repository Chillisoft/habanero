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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Contains the collection of controls that the TabPage uses
    /// </summary>
    public interface ITabPageCollection
    {
        //int Add(ITabPage page);

        /// <summary>
        /// Adds a tab page to the collection
        /// </summary>
        void Add(ITabPage page);

        /// <summary>
        /// Indicates the tab page at the specified indexed location in the collection
        /// </summary>
        ITabPage this[int i] { get; }

        /// <summary>
        /// Indicates the number of tab pages in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Retrieves the index of the specified tab page in the collection
        /// </summary>
        /// <returns>A zero-based index value that represents the position of the specified
        /// tab page in the collection</returns>
        int IndexOf(ITabPage page);
    }
}