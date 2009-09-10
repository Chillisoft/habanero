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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Wraps a NumericUpDown control in order to display and capture a numeric property of the business object 
    /// </summary>
    public abstract class NumericUpDownMapper : ControlMapper
    {
        /// <summary>
        /// Teh actual <see cref="INumericUpDown"/> control being mapped to the <see cref="IBusinessObject"/>.<see cref="IBOProp"/>
        /// </summary>
        protected INumericUpDown _numericUpDown;
        private readonly INumericUpDownMapperStrategy _mapperStrategy;

        /// <summary>
        /// Constructor to instantiate a new instance of the class
        /// </summary>
        /// <param name="ctl">The control object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read only.
        /// If so, it then becomes disabled.  If not,
        /// handlers are assigned to manage key presses, depending on the strategy assigned to this mapper.</param>
        /// <param name="factory">The control factory to be used when creating the controlMapperStrategy</param>
        protected NumericUpDownMapper(IControlHabanero ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
            _numericUpDown = (INumericUpDown)ctl;
            _mapperStrategy = factory.CreateNumericUpDownMapperStrategy();
            _mapperStrategy.ValueChanged(this);
        }

        /// <summary>
        /// Gets the <see cref="INumericUpDownMapperStrategy"/> that has been assigned to this mapper
        /// </summary>
        public INumericUpDownMapperStrategy MapperStrategy
        {
            get { return _mapperStrategy; }
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            _numericUpDown.Value = Convert.ToDecimal(GetPropertyValue());
        }
    }
}