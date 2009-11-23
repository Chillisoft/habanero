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
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    ///<summary>
    /// Implements an auto Incrementing field pattern for a business object. The appropriate property(s) 
    /// must be set as autoincrementing in the property definition.
    ///</summary>
    public class SupportsAutoIncrementingFieldBO : ISupportsAutoIncrementingField {
        private readonly IBusinessObject _bo;

        ///<summary>
        /// Constructs the autoincremeing fieldBO object with the appropriate business object.
        ///</summary>
        ///<param name="bo"></param>
        public SupportsAutoIncrementingFieldBO(IBusinessObject bo)
        {
            _bo = bo;
        }

        ///<summary>
        ///</summary>
        ///<param name="value">sets the objects autoincremented number from the database</param>
        public void SetAutoIncrementingFieldValue(long value)
        {
            foreach (PropDef def in _bo.ClassDef.PropDefcol) {
                if (def.AutoIncrementing) {
                    _bo.SetPropertyValue(def.PropertyName, value);
                }
            }
        }
    }
}
