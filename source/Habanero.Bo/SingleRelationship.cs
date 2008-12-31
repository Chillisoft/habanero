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
using Habanero.BO.CriteriaManager;
using Habanero.Util;

//using log4net;

namespace Habanero.BO
{

    public interface ISingleRelationship : IRelationship {
        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        void SetRelatedObject(IBusinessObject relatedObject);

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        IBusinessObject GetRelatedObject();

        /// <summary>
        /// Indicates whether the related object has been specified
        /// </summary>
        /// <returns>Returns true if related object exists</returns>
        bool HasRelatedObject();
    }

    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public class SingleRelationship<TBusinessObject> : Relationship<TBusinessObject>, ISingleRelationship
        where TBusinessObject : class, IBusinessObject, new()
    {
        //TODO: Implement logging private static readonly ILog log = LogManager.GetLogger("Habanero.BO.SingleRelationship");
        private TBusinessObject _relatedBo;
        private string _storedRelationshipExpression;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public SingleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
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
                return _relatedBo != null && _relatedBo.Status.IsDirty;
            }
        }

        /// <summary>
        /// Indicates whether the related object has been specified
        /// </summary>
        /// <returns>Returns true if related object exists</returns>
        public virtual bool HasRelatedObject()
        {
            return _relKey.HasRelatedObject();
        }

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        IBusinessObject ISingleRelationship.GetRelatedObject()
        {
            return GetRelatedObject();
        }

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        public virtual TBusinessObject GetRelatedObject()
        {
            IExpression newRelationshipExpression = _relKey.RelationshipExpression();

            if (RelatedBoForeignKeyHasChanged()) _relatedBo = null;

            if (_relatedBo == null || (_storedRelationshipExpression != newRelationshipExpression.ExpressionString()))
            {
                if (HasRelatedObject())
                {
                    IBusinessObject busObj = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject(this);
                    _relatedBo = (TBusinessObject) busObj;
                    _storedRelationshipExpression = newRelationshipExpression.ExpressionString();
                }
                else
                {
                    _relatedBo = null;
                    _storedRelationshipExpression = newRelationshipExpression.ExpressionString();
                }
            }
            if (RelatedBoForeignKeyHasChanged()) _relatedBo = null;
            return _relatedBo;
        }

        private bool RelatedBoForeignKeyHasChanged()
        {
            if (_relatedBo != null)
            {
                foreach (IRelProp prop in this.RelKey)
                {
                    object relatedPropValue = _relatedBo.GetPropertyValue(prop.RelatedClassPropName);
                    if (prop.BOProp.Value == null)
                    {
                        if (relatedPropValue == null) continue;
                        return true;
                    }
                    if (prop.BOProp.Value.Equals(relatedPropValue)) continue;
                    return true;
                }
            }
            return false;
        }

        void ISingleRelationship.SetRelatedObject(IBusinessObject relatedObject)
        {
            SetRelatedObject((TBusinessObject) relatedObject);
        }

        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        public virtual void SetRelatedObject(TBusinessObject relatedObject)
        {

            if (_relatedBo == null) GetRelatedObject();
            if (_relatedBo == relatedObject) return;
            IBusinessObject previousRelatedBO = _relatedBo;

           

            if (relatedObject != null)
            {
                RelationshipDef def = (RelationshipDef) RelationshipDef;
                def.CheckCanAddChild(relatedObject);
                //Add to reverse relationship
                IRelationship reverseRelationship = GetReverseRelationship(relatedObject);
                if (reverseRelationship != null)
                {
                    RelationshipDef relationshipDef =
                        reverseRelationship.RelationshipDef as RelationshipDef;
                    if (relationshipDef != null && _relatedBo != null && relationshipDef.RelationshipType == RelationshipType.Composition)
                    {
                        string message = "The " + relationshipDef.RelatedObjectClassName
                                         + " could not be added since the " + relationshipDef.RelationshipName
                                         +
                                         " relationship is set up as a composition relationship (AddChildAction.Prevent)";
                        throw new HabaneroDeveloperException(message, message);
                    }

                    if (reverseRelationship is IMultipleRelationship)
                    {
                        IMultipleRelationship multipleRelationship = (IMultipleRelationship) reverseRelationship;

                        _relatedBo = relatedObject;
                        multipleRelationship.BusinessObjectCollection.Add(this.OwningBO);
                    }

                    if (reverseRelationship is ISingleRelationship)
                    {
                        relationshipDef.CheckCanAddChild(this.OwningBO);
                        _relatedBo = relatedObject;
                        ((ISingleRelationship)reverseRelationship).SetRelatedObject(this.OwningBO);
                    }
                }
            }

            //Remove the this object from the previuosly related object

            if (previousRelatedBO != null)
            {
                this.RelationshipDef.CheckCanRemoveChild(previousRelatedBO);

                //Remove from previous relationship
                IMultipleRelationship reverseRelationship = GetReverseRelationship(previousRelatedBO) as IMultipleRelationship;
                if (reverseRelationship != null) // && reverseRelationship.IsRelationshipLoaded)
                {
                    IBusinessObjectCollection colInternal = reverseRelationship.BusinessObjectCollection;
                    if (colInternal.Contains(this.OwningBO)) colInternal.Remove(this.OwningBO);
                }
                ISingleRelationship singleReverseRelationship = GetReverseRelationship(previousRelatedBO) as ISingleRelationship;
                if (singleReverseRelationship != null)
                {
                    singleReverseRelationship.RelationshipDef.CheckCanRemoveChild(this.OwningBO);
                    _relatedBo = null;
                    UpdatedForeignKeyAndStoredRelationshipExpression();
                    singleReverseRelationship.SetRelatedObject(null);
                }
            }

            _relatedBo = relatedObject;
            
            UpdatedForeignKeyAndStoredRelationshipExpression();
        }

        private void UpdatedForeignKeyAndStoredRelationshipExpression()
        {
            if (this.RelationshipDef.RelationshipType != RelationshipType.Aggregation)
            {
                foreach (RelProp relProp in _relKey)
                {
                    object relatedObjectValue = _relatedBo == null
                                                    ? null
                                                    : _relatedBo.GetPropertyValue(relProp.RelatedClassPropName);
                    _owningBo.SetPropertyValue(relProp.OwnerPropertyName, relatedObjectValue);
                }
            }
            _storedRelationshipExpression = _relKey.RelationshipExpression().ExpressionString();
        }

        /// <summary>
        /// Sets the related business object to null, ensuring that
        /// it must be reloaded
        /// </summary>
        public void ClearCache()
        {
            _relatedBo = null;
        }

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
            if (IsRelatedBODirty()) dirtyBusinessObjects.Add(_relatedBo); 
            return dirtyBusinessObjects;
        }

        protected override IList<TBusinessObject> DoGetDirtyChildren_Typed()
        {
            IList<TBusinessObject> dirtyBusinessObjects = new List<TBusinessObject>();
            if (IsRelatedBODirty()) dirtyBusinessObjects.Add(_relatedBo); 
            return dirtyBusinessObjects;
        }

        private bool IsRelatedBODirty() { return _relatedBo != null && _relatedBo.Status.IsDirty; }

        protected override void DoInitialisation()
        {
            // do nothing
        }
    }

}