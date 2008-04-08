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

using System;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public class MultipleRelationship : Relationship
    {
		internal IBusinessObjectCollection _boCol;
		//private BusinessObjectCollection<BusinessObject> _boCol;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        // Note: Mark - I Changed this to public from internal TODO - investigate if there was a good reason to have it internal
        public MultipleRelationship(BusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

		/// <summary>
		/// Returns the set of business objects that relate to this one
		/// through the specific relationship
		/// </summary>
		/// <returns>Returns a collection of business objects</returns>
		public virtual IBusinessObjectCollection GetRelatedBusinessObjectCol()
		{
            return GetRelatedBusinessObjectColInternal<BusinessObject>(false);
		}

    	/// <summary>
    	/// Returns the set of business objects that relate to this one
    	/// through the specific relationship
    	/// </summary>
    	/// <returns>Returns a collection of business objects</returns>
		public virtual BusinessObjectCollection<T> GetRelatedBusinessObjectCol<T>()
    		where T : BusinessObject
        {
    	    IBusinessObjectCollection boCol = GetRelatedBusinessObjectColInternal<T>(true);
    	    return (BusinessObjectCollection<T>)boCol;
        }

        private IBusinessObjectCollection GetRelatedBusinessObjectColInternal<T>(bool useGenericParameter)
    		where T : BusinessObject
        {
            BusinessObject busObj;
            Type type = _relDef.RelatedObjectClassType;
            Type collectionItemType;
            if(useGenericParameter)
            {
                collectionItemType = typeof (T);
            } else
            {
                collectionItemType = type;
            }
            try
            {
                busObj = (BusinessObject)Activator.CreateInstance(type, true);
            }
            catch (Exception ex)
            {
                throw new UnknownTypeNameException(String.Format(
                                                       "An error occurred while attempting to load a related " +
                                                       "business object collection, with the type given as '{0}'. " +
                                                       "Check that the given type exists and has been correctly " +
                                                       "defined in the relationship and class definitions for the classes " +
                                                       "involved.", type), ex);
            }
            if (!(type == collectionItemType || type.IsSubclassOf(collectionItemType)))
            {
                throw new HabaneroArgumentException(String.Format(
                                                        "An error occurred while attempting to load a related " +
                                                        "business object collection of type '{0}' into a " +
                                                        "collection of the specified generic type('{1}').",
                                                        type, typeof(T)));
            }
            IBusinessObjectCollection boCol;
            if (useGenericParameter)
            {
                boCol = BOLoader.Instance.GetBusinessObjectCol<T>(
                    _relKey.RelationshipExpression(), ((MultipleRelationshipDef) _relDef).OrderBy);
            } else
            {
                boCol = BOLoader.Instance.GetBusinessObjectCol(type, 
                    _relKey.RelationshipExpression(), ((MultipleRelationshipDef) _relDef).OrderBy);
            }

            if (this._relDef.KeepReferenceToRelatedObject)
            {
                //// TODO - Add a check to see if the count of objects has changed.  Removed this keep reference because if an object
                //// gets added with the foreign key nothing will pick that up other than a reload.
                ////if (_boCol == null) {
                //_boCol = BOLoader.Instance.GetBusinessObjectCol(busObj.GetType(), _relKey.RelationshipExpression(),
                ////                                     ((MultipleRelationshipDef) _relDef).OrderBy);
                ////}
                _boCol = boCol;
            }
            return boCol;
        }
    }
}