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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for relationships between business objects
    /// </summary>
    public abstract class Relationship : IRelationship
    {
        protected RelationshipDef _relDef;
        protected readonly IBusinessObject _owningBo;
        protected internal RelKey _relKey;
        protected IBusinessObjectCollection _boCol;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object from where the 
        /// relationship originates</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public Relationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
        {
            _relDef = lRelDef;
            _owningBo = owningBo;
            _relKey = _relDef.RelKeyDef.CreateRelKey(lBOPropCol);
        }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        public string RelationshipName
        {
            get { return _relDef.RelationshipName; }
        }

        /// <summary>
        /// Returns the relationship definition
        /// </summary>
        internal RelationshipDef RelationshipDef
        {
            get { return _relDef; }
        }

        ///<summary>
        /// Returns the appropriate delete action when the parent is deleted.
        /// i.e. delete related objects, dereference related objects, prevent deletion.
        ///</summary>
        public DeleteParentAction DeleteParentAction
        {
            get { return _relDef.DeleteParentAction; }
        }

        ///<summary>
        /// 
        ///</summary>
        public OrderCriteria OrderCriteria
        {
            get { return _relDef.OrderCriteria; }
        }

        ///<summary>
        /// Returns the business object that owns this relationship e.g. Invoice has many lines
        /// the owning BO would be invoice.
        ///</summary>
        public IBusinessObject OwningBO
        {
            get { return _owningBo; }
        }

        /// <summary>
        /// Returns the set of business objects that relate to this one
        /// through the specific relationship
        /// </summary>
        /// <returns>Returns a collection of business objects</returns>
        public virtual IBusinessObjectCollection GetRelatedBusinessObjectCol()
        {
            return GetRelatedBusinessObjectColInternal();
            
        }

        /// <summary>
        /// Returns the set of business objects that relate to this one
        /// through the specific relationship
        /// </summary>
        /// <returns>Returns a collection of business objects</returns>
        public virtual BusinessObjectCollection<TBusinessObject> GetRelatedBusinessObjectCol<TBusinessObject>()
            where TBusinessObject : BusinessObject, new() 
        {
            IBusinessObjectCollection boCol = GetRelatedBusinessObjectColInternal<TBusinessObject>();
            return (BusinessObjectCollection<TBusinessObject>)boCol;
        }

        protected abstract IBusinessObjectCollection GetRelatedBusinessObjectColInternal<TBusinessObject>()
            where TBusinessObject : BusinessObject, new() ;

        protected virtual IBusinessObjectCollection GetRelatedBusinessObjectColInternal()
        {
            if (_boCol != null)
            {
                BOLoader.Instance.LoadBusinessObjectCollection(this._relKey.RelationshipExpression(), _boCol, this.OrderCriteria, "");
                return _boCol;
            }

            Type type = _relDef.RelatedObjectClassType;
            //Check that the type can be created and raise appropriate error 
            try
            {
                Activator.CreateInstance(type, true);
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
            IBusinessObjectCollection boCol = BOLoader.Instance.GetRelatedBusinessObjectCollection(type, this);

            if (_relDef.KeepReferenceToRelatedObject)
            {
                _boCol = boCol;
            }
            return boCol;
        }


        public IRelKey RelKey
        {
            get { return _relKey; }
        }
    }
}