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
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public class MultipleRelationship : Relationship
    {
        //private BusinessObjectCollection<BusinessObject> _boCol;

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
            _boCol = CreateNewRelatedBusinessObjectCollection(_relDef.RelatedObjectClassType, this);
        }

        /// <summary>
        /// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
        /// </summary>
        /// <param name="boType">The type parameter to be used</param>
        /// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
        /// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
        public static IBusinessObjectCollection CreateRelatedBusinessObjectCollection(Type boType, IRelationship relationship)
        {
            IBusinessObjectCollection collection = CreateNewRelatedBusinessObjectCollection(boType, relationship);
            SetupCriteriaForRelationship(relationship, collection);
            return collection;
        }

        /// <summary>
        /// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
        /// </summary>
        /// <param name="boType">The type parameter to be used</param>
        /// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
        /// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
        private static IBusinessObjectCollection CreateNewRelatedBusinessObjectCollection(Type boType, IRelationship relationship)
        {
            Utilities.CheckTypeCanBeCreated(boType);
            Type relatedCollectionType = typeof(RelatedBusinessObjectCollection<>);
            relatedCollectionType = relatedCollectionType.MakeGenericType(boType);
            IBusinessObjectCollection collection = (IBusinessObjectCollection)Activator.CreateInstance(relatedCollectionType, relationship);
            return collection;
        }

        private static void SetupCriteriaForRelationship(IRelationship relationship, IBusinessObjectCollection collection)
        {
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);

            OrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            //QueryBuilder.PrepareCriteria(relationship.RelatedObjectClassDef, relationshipCriteria);
            collection.SelectQuery.Criteria = relationshipCriteria;
            collection.SelectQuery.OrderCriteria = preparedOrderCriteria;
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
                //if (!IsRelationshipLoaded) return false;
                bool dirtyCollections = HasDirtyEditingCollections;
                if (dirtyCollections) return true;
                foreach (IBusinessObject bo  in _boCol.PersistedBOCol)
                {
                    if (bo.Status.IsDirty)
                    {
                        return true;
                    }
                }
                return false; // || 
            }
        }

        protected bool HasDirtyEditingCollections
        {
            get
            {
                //if (!IsRelationshipLoaded) return false;
                return (_boCol.CreatedBOCol.Count > 0) || (_boCol.MarkForDeletionBOCol.Count > 0);
            }
        }

        protected override IBusinessObjectCollection GetRelatedBusinessObjectColInternal<TBusinessObject>()
        {
            //TODO: Need a strategy for what should be happening here when the collection is previously loaded.
            //I would suggest option 1
            //1) The collection is reloaded from the database as is currently being done.
            //2) The collection is is returned
            if (_boCol != null)
            {
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh((BusinessObjectCollection<TBusinessObject>) _boCol);
                return _boCol;
            }

            Type relatedBusinessObjectType = _relDef.RelatedObjectClassType;
            Type genericType = typeof (TBusinessObject);

            CheckTypeCanBeCreated(relatedBusinessObjectType);

            CheckTypeIsASubClassOfGenericType<TBusinessObject>(relatedBusinessObjectType, genericType);

            _boCol = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<TBusinessObject>(this);

            return _boCol;
        }

        protected override IBusinessObjectCollection GetRelatedBusinessObjectColInternal()
        {


            if (_boCol != null)
            {
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(_boCol);
                return _boCol;
            }
            Type type = _relDef.RelatedObjectClassType;
            CheckTypeCanBeCreated(type);
            _boCol = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection(type, this);
            return _boCol;
        }

        private static void CheckTypeIsASubClassOfGenericType<TBusinessObject>(Type type, Type collectionItemType)
        {
            if (!(type == collectionItemType || type.IsSubclassOf(collectionItemType)))
            {
                throw new HabaneroArgumentException
                    (String.Format
                         ("An error occurred while attempting to load a related "
                          + "business object collection of type '{0}' into a "
                          + "collection of the specified generic type('{1}').", type, typeof (TBusinessObject)));
            }
        }

        private static void CheckTypeCanBeCreated(Type type)
        {
            //Check that the type can be created and raise appropriate error 
            try
            {
                Activator.CreateInstance(type, true);
            }
            catch (Exception ex)
            {
                throw new UnknownTypeNameException
                    (String.Format
                         ("An error occurred while attempting to load a related "
                          + "business object collection, with the type given as '{0}'. "
                          + "Check that the given type exists and has been correctly "
                          + "defined in the relationship and class definitions for the classes " + "involved.", type),
                     ex);
            }
        }

        ///<summary>
        /// Returns a list of all the related objects that are dirty.
        /// In the case of a composition or aggregation this will be a list of all 
        ///   dirty related objects (child objects). 
        /// In the case of association
        ///   this will only be a list of related objects that are added, removed, marked4deletion or created
        ///   as part of the relationship.
        ///</summary>
        public override IList<IBusinessObject> GetDirtyChildren()
        {
            IList<IBusinessObject> dirtyBusinessObjects = new List<IBusinessObject>();
            if (HasDirtyEditingCollections)
            {
                foreach (IBusinessObject bo in _boCol.CreatedBOCol)
                {
                    dirtyBusinessObjects.Add(bo);
                }
                foreach (IBusinessObject bo in _boCol.MarkForDeletionBOCol)
                {
                    dirtyBusinessObjects.Add(bo);
                }
            }
            foreach (IBusinessObject bo in _boCol.PersistedBOCol)
            {
                if (bo.Status.IsDirty && !dirtyBusinessObjects.Contains(bo))
                {
                    dirtyBusinessObjects.Add(bo);
                }
            }
            return dirtyBusinessObjects;
        }

        protected override void DoInitialisation()
        {
            SetupCriteriaForRelationship(this, _boCol);
        }

     
    }
}