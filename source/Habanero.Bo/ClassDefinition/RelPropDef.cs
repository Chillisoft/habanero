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
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Lists a property on which two classes in a relationship are
    /// being matched
    /// </summary>
    public class RelPropDef
    {
        private IPropDef _ownerPropDef;
		private string _relatedClassPropName;

		/// <summary>
        /// Constructor to create new RelPropDef object
        /// </summary>
        /// <param name="ownerClassPropDef">The property definition of the 
        /// owner object</param>
        /// <param name="relatedObjectPropName">The property name of the 
        /// related object</param>
        public RelPropDef(IPropDef ownerClassPropDef,
                          string relatedObjectPropName)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(ownerClassPropDef, "ownerClassPropDef");
            _ownerPropDef = ownerClassPropDef;
            _relatedClassPropName = relatedObjectPropName;
		}

    	///<summary>
    	/// Gets or sets the property definition for the relationship owner
    	///</summary>
    	protected IPropDef OwnerProperty
    	{
			get { return _ownerPropDef; }
			set
			{
				ArgumentValidationHelper.CheckArgumentNotNull(value, "value");
				_ownerPropDef = value;
			}
    	}

		/// <summary>
        /// Returns the property name for the relationship owner
        /// </summary>
        public string OwnerPropertyName
        {
            get { return _ownerPropDef.PropertyName; }
        }

        /// <summary>
        /// The property name to be matched to in the related class
        /// </summary>
        public string RelatedClassPropName
        {
			get { return _relatedClassPropName; }
			protected set { _relatedClassPropName = value; }
		}

		/// <summary>
        /// Creates a new RelProp object based on this property definition
        /// </summary>
        /// <param name="boPropCol">The collection of properties</param>
        /// <returns>The newly created RelProp object</returns>
        protected internal RelProp CreateRelProp(BOPropCol boPropCol)
		{
		    IBOProp boProp = boPropCol[OwnerPropertyName];
		    return new RelProp(this, boProp);
		}
    }
}