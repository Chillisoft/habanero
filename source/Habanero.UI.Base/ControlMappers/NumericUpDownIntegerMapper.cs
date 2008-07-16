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
    public class NumericUpDownIntegerMapper : NumericUpDownMapper
    {
        private INumericUpDownMapperStrategy _mapperStrategy;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="numericUpDownControl">The numericUpDownControl object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public NumericUpDownIntegerMapper(IControlChilli numericUpDownControl, string propName, bool isReadOnly, IControlFactory factory)
            : base(numericUpDownControl, propName, isReadOnly, factory)
        {
            _numericUpDown.DecimalPlaces = 0;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
            _mapperStrategy = factory.CreateNumericUpDownMapperStrategy();
            _mapperStrategy.ValueChanged(this);
        }

        public INumericUpDownMapperStrategy MapperStrategy
        {
            get { return _mapperStrategy; }
        }

        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(Convert.ToInt32(_numericUpDown.Value));
        }

    }
}