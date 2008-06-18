//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;

namespace Habanero.BO
{
    /// <summary>
    /// Represents the property on which two objects match up in a relationship
    /// </summary>
    public class RelProp : IRelProp
    {
        private IBOProp _boProp;
        private RelPropDef _relPropDef;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="mRelPropDef">The relationship property definition</param>
        /// <param name="lBoProp">The property</param>
        internal RelProp(RelPropDef mRelPropDef, IBOProp lBoProp)
        {
            this._relPropDef = mRelPropDef;
            _boProp = lBoProp;
        }

        /// <summary>
        /// Returns the property name of the relationship owner
        /// </summary>
        public string OwnerPropertyName
        {
            get { return _relPropDef.OwnerPropertyName; }
        }

        /// <summary>
        /// Returns the property name of the related object
        /// </summary>
        public string RelatedClassPropName
        {
            get { return _relPropDef.RelatedClassPropName; }
        }

        /// <summary>
        /// Indicates if the property is null
        /// </summary>
        internal bool IsNull
        {
            get { return _boProp == null || _boProp.Value == null; }
        }

        /// <summary>
        /// Returns the related property's expression
        /// </summary>
        /// <returns>Returns an IExpression object</returns>
        internal IExpression RelatedPropExpression()
        {
            if (_boProp.Value == null)
            {
                return new Parameter(_relPropDef.RelatedClassPropName, "IS", "NULL");
            }
            return new Parameter(_relPropDef.RelatedClassPropName, "=", _boProp.PropertyValueString);
        }

        /// <summary>
        /// The BoProp this RelProp requires to generate its search expression
        /// </summary>
        public IBOProp BOProp
        {
            get { return _boProp; }
        }
    }

}