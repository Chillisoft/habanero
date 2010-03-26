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
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    internal class DateTimePickerMapperStrategyWin : IDateTimePickerMapperStrategy
    {
        // Assumes that one strategy is created for each control.
        // These fields exist so that the IsValidCharacter method knows
        //   which prop and textbox it is dealing with

        public void AddUpdateBoPropOnValueChangedHandler(DateTimePickerMapper mapper)
        {
            if (mapper.Control is IDateTimePicker)
            {
                IDateTimePicker dtp = (IDateTimePicker)mapper.Control;
                dtp.ValueChanged +=delegate {
                                           mapper.ApplyChangesToBusinessObject();
                                       };
            }
        }
    }
}