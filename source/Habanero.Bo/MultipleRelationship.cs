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

using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public interface IMultipleRelationship : IRelationship
    {
        ///<summary>
        /// The criteria by which this relationship is ordered. I.e. by default all the
        /// related objects are loaded in this order.
        ///</summary>
        OrderCriteria OrderCriteria { get; }

        ///<summary>
        /// The collection of business objects that is managed by this relationship.
        ///</summary>
        IBusinessObjectCollection BusinessObjectCollection { get; }
    }

    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public class MultipleRelationship<TBusinessObject> : Relationship<TBusinessObject>, IMultipleRelationship
        where TBusinessObject : class, IBusinessObject, new()
    {
        protected BusinessObjectCollection<TBusinessObject> _boCol;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public MultipleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
            _boCol =
                (RelatedBusinessObjectCollection<TBusinessObject>)
                RelationshipUtils.CreateNewRelatedBusinessObjectCollection(_relDef.RelatedObjectClassType, this);
        }

        ///<summary>
        /// Returns whether the relationship is dirty or not.
        /// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
        /// If the relationship is of type composition or aggregation then it is dirty if it has any 
        ///  related (children) business objects that are dirty.
        ///</summary>
        public override bool IsDirty
        {
            get
            {
                bool dirtyCollections = HasDirtyEditingCollections;
                if (dirtyCollections) return true;
                if (this.RelationshipDef.RelationshipType == RelationshipType.Aggregation ||
                    RelationshipDef.RelationshipType == RelationshipType.Composition)
                {
                    foreach (IBusinessObject bo in _boCol.PersistedBusinessObjects)
                    {
                        if (bo.Status.IsDirty)
                        {
                            return true;
                        }
                    }
                }
                return false; // || 
            }
        }

        protected bool HasDirtyEditingCollections
        {
            get
            {
                return (_boCol.CreatedBusinessObjects.Count > 0) || (_boCol.MarkedForDeleteBusinessObjects.Count > 0) || (_boCol.RemovedBusinessObjects.Count > 0) || (_boCol.AddedBusinessObjects.Count > 0);
            }
        }

        IBusinessObjectCollection IMultipleRelationship.BusinessObjectCollection
        {
            get
            {
                return this.BusinessObjectCollection;
            }
        }

        /// <summary>
        /// Returns the collection for this relationship.  The collection is refreshed before
        /// it is returned.
        /// </summary>
        public BusinessObjectCollection<TBusinessObject> BusinessObjectCollection
        {
            get
            {
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(_boCol);
                return _boCol;
            }
        }
        
        private delegate void Add(IBusinessObject bo);

        private delegate bool Contains(IBusinessObject bo);
        
        ///<summary>
        /// Returns a list of all the related objects that are dirty.
        /// In the case of a composition or aggregation this will be a list of all 
        ///   dirty related objects (child objects). 
        /// In the case of association
        ///   this will only be a list of related objects that are added, removed, marked4deletion or created
        ///   as part of the relationship.
        ///</summary>
        protected override IList<IBusinessObject> DoGetDirtyChildren()
        {
            IList<IBusinessObject> dirtyBusinessObjects = new List<IBusinessObject>();
            PopulateDirtyBusinessObjects(dirtyBusinessObjects.Add, dirtyBusinessObjects.Contains);
            return dirtyBusinessObjects;
        }

        protected override IList<TBusinessObject> DoGetDirtyChildren_Typed()
        {
            IList<TBusinessObject> dirtyBusinessObjects = new List<TBusinessObject>();
            PopulateDirtyBusinessObjects
                (bo => dirtyBusinessObjects.Add((TBusinessObject) bo),
                 bo => dirtyBusinessObjects.Contains((TBusinessObject) bo));
            return dirtyBusinessObjects;
        }

        private void PopulateDirtyBusinessObjects(Add add, Contains contains)
        {
            if (HasDirtyEditingCollections)
            {
                foreach (IBusinessObject bo in _boCol.CreatedBusinessObjects)
                {
                    add(bo);
                }
                foreach (IBusinessObject bo in _boCol.MarkedForDeleteBusinessObjects)
                {
                    add(bo);
                }
                foreach (IBusinessObject bo in _boCol.RemovedBusinessObjects)
                {
                    add(bo);
                }
                foreach (IBusinessObject bo in _boCol.AddedBusinessObjects)
                {
                    add(bo);
                }
            }
            if (this.RelationshipDef.RelationshipType == RelationshipType.Composition
                || this.RelationshipDef.RelationshipType == RelationshipType.Aggregation)
            {
                foreach (IBusinessObject bo in _boCol.PersistedBusinessObjects)
                {
                    if (bo.Status.IsDirty && !contains(bo))
                    {
                        add(bo);
                    }
                }
            }
        }

        protected override void DoInitialisation()
        {
            RelationshipUtils.SetupCriteriaForRelationship(this, _boCol);
        }

        /// <summary>
        /// Returns the underlying collection without refreshing it.
        /// </summary>
        /// <returns></returns>
        internal IBusinessObjectCollection GetLoadedBOColInternal()
        {
            return _boCol;
        }

        ///<summary>
        /// Returns the <see cref="OrderCriteria"/> for this relationship (which determines how the 
        /// objects in the collection are ordered.
        ///</summary>
        public OrderCriteria OrderCriteria
        {
            get
            {
                if (_relDef.OrderCriteria == null) return new OrderCriteria();
                return _relDef.OrderCriteria;
            }
        }
    }
}
