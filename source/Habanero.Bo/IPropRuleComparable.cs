//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// This is an interface for Comparable Prop Rule typically Comparable prop rule are Prop Rules that are implemented
    /// for certain types in this case we have implemented them for Decimal, Double, DateTime and Integer.
    /// The IPropRuleComparable has only a MinValue and a MaxValue to Type T.
    /// </summary>
    public interface IPropRuleComparable<T>: IPropRule where T:struct
    {
        /// <summary>
        /// Gets and sets the minimum value that the Double can be assigned
        /// </summary>
        T MinValue { get; }

        /// <summary>
        /// Gets and sets the maximum value that the Double can be assigned
        /// </summary>
        T MaxValue { get; }
    }
}