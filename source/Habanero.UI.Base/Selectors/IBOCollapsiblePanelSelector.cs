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
using Habanero.Base;

namespace Habanero.UI.Base
{
    ///<summary>
    /// Provides an interface for a control that specialises in showing a list of 
    /// Business Objects <see cref="IBusinessObjectCollection"/>.
    /// This control shows each business object in its own collapsible Panel.
    /// This is a very powerfull control for easily adding or viewing a fiew items E.g. for 
    /// a list of addresses for a person.
    ///</summary>
    public interface IBOCollapsiblePanelSelector : IBOColSelectorControl
    {
    }
}