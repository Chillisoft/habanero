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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for relationships between business objects
    /// </summary>
    public abstract class Relationship : IRelationship // <TBusinessObject> : IRelationship
        //where TBusinessObject : class, IBusinessObject, new() 

    {
        protected RelationshipDef _relDef;
        protected readonly IBusinessObject _owningBo;
        protected internal RelKey _relKey;
        protected IBusinessObjectCollection _boCol;
        private bool _initialised = false;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object from where the 
        /// relationship originates</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        protected Relationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
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
        public RelationshipDef RelationshipDef
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
            get
            {
                if (_relDef.OrderCriteria == null) return new OrderCriteria();
                return _relDef.OrderCriteria;
            }
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
            return (BusinessObjectCollection<TBusinessObject>) boCol;
        }

        protected abstract IBusinessObjectCollection GetRelatedBusinessObjectColInternal<TBusinessObject>()
            where TBusinessObject : BusinessObject, new();


        protected abstract IBusinessObjectCollection GetRelatedBusinessObjectColInternal();





        ///<summary>
        /// The key that identifies this relationship i.e. the properties in the 
        /// source object and how they are related to properties in the related object.
        ///</summary>
        public IRelKey RelKey
        {
            get { return _relKey; }
        }

        /// <summary>
        /// The class Definition for the related object.
        /// </summary>
        public IClassDef RelatedObjectClassDef
        {
            get { return _relDef.RelatedObjectClassDef; }
        }

        ///<summary>
        /// Returns whether the relationship is dirty or not.
        /// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
        /// If the relationship is of type composition or aggregation then it is dirty if it has any 
        ///  related (children) business objects that are dirty.
        ///</summary>
        public abstract bool IsDirty { get; }


        ///<summary>
        /// Returns a list of all the related objects that are dirty.
        /// In the case of a composition or aggregation this will be a list of all 
        ///   dirty related objects (child objects). 
        /// In the case of association
        ///   this will only be a list of related objects that are added, removed, marked4deletion or created
        ///   as part of the relationship.
        ///</summary>
        public abstract IList<IBusinessObject> GetDirtyChildren();

        //TODO: This should be temporary code and will b removed when define reverse relationships in Firestarter and classdefs.
        /// <summary>
        /// Returns the reverse relationship for this relationship i.e. If invoice has invoice lines and you 
        /// can navigate from invoice lines to invoices then the invoicelines to invoice relationship is the
        /// reverse relationship of the invoice to invoicelines relationship and vica versa.
        /// </summary>
        /// <param name="bo">The related Business object (in the example the invoice lines)</param>
        /// <returns>The reverse relationship or null if no reverse relationship is set up.</returns>
        internal IRelationship GetReverseRelationship(IBusinessObject bo)
        {
            //This is a horrrible Hack but I do not want to do the reverse relationship 
            IRelationship reverseRelationship = null;
            foreach (Relationship relationship in bo.Relationships)
            {
                if (relationship.RelationshipDef.RelatedObjectClassType != this.OwningBO.GetType()) continue;
                bool reverseRelatedPropFound = false;
                foreach (IRelProp prop in this._relKey)
                {
                    foreach (IRelProp relProp in relationship.RelKey)
                    {
                        if (prop.RelatedClassPropName != relProp.OwnerPropertyName) continue;
                        reverseRelatedPropFound = true;
                        break;
                    }
                }
                if (!reverseRelatedPropFound) continue;
                reverseRelationship = relationship;
                break;
            }
            return reverseRelationship;
        }

        internal IBusinessObjectCollection GetLoadedBOColInternal()
        {
            return _boCol;
        }

        public void Initialise()
        {
            if (_initialised) return;
            DoInitialisation();
            _initialised = true;
        }

        protected abstract void DoInitialisation();
    }
}