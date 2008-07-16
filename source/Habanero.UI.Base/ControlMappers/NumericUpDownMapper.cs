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

namespace Habanero.UI.Base
{
    public abstract class NumericUpDownMapper : ControlMapper
    {
        protected INumericUpDown _numericUpDown;

        /// <summary>
        /// Constructor to instantiate a new instance of the class
        /// </summary>
        /// <param name="ctl">The control object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read only.
        /// If so, it then becomes disabled.  If not,
        /// handlers are assigned to manage key presses.</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        protected NumericUpDownMapper(IControlChilli ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
            _numericUpDown = (INumericUpDown)ctl;
        }
       


        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            _numericUpDown.Value = Convert.ToDecimal(GetPropertyValue());
        }
    }
}