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
    /// <summary>
    /// Represents a control that represents a single business object.
    /// The primary purpose of this interface is to ensure that a master
    /// control can access the current business object held in a sub-control
    /// that inherits from this interface.  See IBOColTabControl for an
    /// example usage.
    /// </summary>
    public interface IBusinessObjectControl : IControlHabanero
    {
        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        IBusinessObject BusinessObject { set; get; }
    }
}