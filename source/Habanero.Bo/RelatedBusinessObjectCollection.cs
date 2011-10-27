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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    ///<summary>
    /// This is a collection of business objects as utilised in an object collection e.g. Person has many addreses.
    /// This person.GetAddresses will return a collection of addresses for the person where the collection is of type
    /// RelatedBusinessObjectCollection
    ///</summary>
    ///<typeparam name="TBusinessObject"></typeparam>
    public class RelatedBusinessObjectCollection<TBusinessObject> : BusinessObjectCollection<TBusinessObject>
        where TBusinessObject : class, IBusinessObject, new()
    {
        private readonly Relationship _relationship;

        ///<summary>
        /// The related business object is constructed with the parent Business object of this 
        /// relationship as well the relationship name.
        ///</summary>
        ///<param name="relationship"></param>
        public RelatedBusinessObjectCollection(IRelationship relationship)
        {
            _relationship = (Relationship) relationship;
        }


        //Relationship 
        //-- should this reference the reverse relationship if exists 
        //  (i.e. bidirectional navigatable relationship).

        //-- 

        /// <summary>
        /// Adds a business object to the collection
        /// </summary>
        /// <param name="bo">The business object to add</param>
        protected override bool AddInternal(TBusinessObject bo)
        {
            var boColInternal = ((IBusinessObjectCollectionInternal)this);
            if (!boColInternal.Loading)
            {
                if (!this._relationship.RelKey.Criteria.IsMatch(bo, false))
                {
                    MultipleRelationshipDef def = this._relationship.RelationshipDef as MultipleRelationshipDef;
                    if (def != null) def.CheckCanAddChild(bo);
                }
            }
            if (!base.AddInternal(bo)) return false;
            if (boColInternal.Loading) return true;

            if (IsForeignKeySetup(bo)) return true;

            SetUpForeignKey(bo);
            SetupRelatedObject(bo);
            return true;
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public override bool Remove(TBusinessObject bo)
        {
            MultipleRelationshipDef def = this._relationship.RelationshipDef as MultipleRelationshipDef;
            var boColInternal = ((IBusinessObjectCollectionInternal)this);
            if (!bo.Status.IsNew && def != null && !boColInternal.Loading && (def.RelationshipType == RelationshipType.Composition))
            {
                string message = "The " + def.RelatedObjectClassName +
                                 " could not be removed since the " + def.RelationshipName +
                                 " relationship is set up as a composition relationship (RemoveChildAction.Prevent)";
                throw new HabaneroDeveloperException(message, message);
            }
            return RemoveInternal(bo);
        }

        /// <summary>
        /// A method for removing the busines object without doing any checks for relationship type.
        /// This is needed for cases where a parent business object that has created composite children has
        /// <see cref="IBusinessObject.CancelEdits"/> called. The parent business objects should
        /// then cancel all its children
        /// </summary>
        /// <param name="bo">The child business object that needs to be removed from the collection.</param>
        /// <returns>true if the business object is removed, otherwise false.</returns>
        internal bool RemoveInternal(TBusinessObject bo)
        {
            var boColInternal = ((IBusinessObjectCollectionInternal)this);
            if (!base.Remove(bo)) return false;
            if (boColInternal.Loading) return true;
            DereferenceBO(bo);
            MultipleRelationshipDef def = this._relationship.RelationshipDef as MultipleRelationshipDef;
            if (!(!bo.Status.IsNew && def != null && !boColInternal.Loading && (def.RelationshipType == RelationshipType.Composition)))
            {
                RemoveRelatedObject(bo);
            }
            return true;
        }

        private void DereferenceBO(TBusinessObject bo)
        {
            foreach (RelPropDef relPropDef in _relationship.RelationshipDef.RelKeyDef)
            {
                bo.SetPropertyValue(relPropDef.RelatedClassPropName, null);
            }
        }

        private void RemoveRelatedObject(TBusinessObject bo)
        {
            ISingleRelationship reverseRelationship = GetReverseRelationship(bo) as ISingleRelationship;
            if (reverseRelationship != null)
            {
                reverseRelationship.SetRelatedObject(null);
            }
        }

        /// <summary>
        /// Commits to the database all the business objects that are either
        /// new or have been altered since the last committal
        /// </summary>
        public override void SaveAll()
        {
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();

            foreach (TBusinessObject bo in _removedBusinessObjects)
            {
                committer.AddBusinessObject(bo);
            }
            SaveAllInTransaction(committer);
            _removedBusinessObjects.Clear();
        }

        /// <summary>
        /// Creates a new TBusinessObject for this RelatedBusinessObjectCollection.
        /// The new BusinessObject has all of its foreign key properties set, but is not added in to the collection yet.
        /// </summary>
        /// <returns>A new TBusinessObject.</returns>
        protected override TBusinessObject CreateNewBusinessObject()
        {
            TBusinessObject newBusinessObject = base.CreateNewBusinessObject();
            SetUpForeignKey(newBusinessObject);
            SetupRelatedObject(newBusinessObject);
            return newBusinessObject;
        }

        private void SetupRelatedObject(TBusinessObject bo)
        {
            SingleRelationshipBase reverseRelationship = GetReverseRelationship(bo) as SingleRelationshipBase;
            if (reverseRelationship != null)
            {
                reverseRelationship.SetRelatedObjectFromMultiple(this._relationship.OwningBO);
            }
        }

        private bool IsForeignKeySetup(TBusinessObject bo)
        {
            ISingleRelationship reverseRelationship = GetReverseRelationship(bo) as ISingleRelationship;
            if (reverseRelationship != null)
            {
                IBusinessObject relatedObject = reverseRelationship.GetRelatedObject();
                return (relatedObject == this._relationship.OwningBO);
            }
            return false;
        }

        internal IRelationship GetReverseRelationship(TBusinessObject bo)
        {
            return this._relationship.GetReverseRelationship(bo);
        }

        private void SetUpForeignKey(TBusinessObject bo)
        {
            foreach (RelPropDef relPropDef in _relationship.RelationshipDef.RelKeyDef)
            {
                bo.SetPropertyValue
                    (relPropDef.RelatedClassPropName,
                     _relationship.OwningBO.GetPropertyValue(relPropDef.OwnerPropertyName));
            }
        }

        /// <summary>
        /// Handles the event of a Business object being restored.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void RestoredEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = (TBusinessObject) e.BusinessObject;
            bool removedListContains = this.RemovedBusinessObjects.Contains(bo);
            bool addedListContains = this.AddedBusinessObjects.Contains(bo);
            base.RestoredEventHandler(sender, e);
            if (removedListContains) this.Add(bo);
//            if (addedListContains)
//            {
//                DereferenceBO(bo);
//            }
        }

        /// <summary>
        /// Handles the event of the Business object becoming invalid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void SavedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = (TBusinessObject) e.BusinessObject;
            bool removedListContains = this.RemovedBusinessObjects.Contains(bo);
            base.SavedEventHandler(sender, e);
            if (removedListContains) RemoveFromPersistedCollection(bo);
            if (this.AddedBusinessObjects.Contains(bo)) this.AddedBusinessObjects.Remove(bo);
        }

        private void RemoveFromPersistedCollection(TBusinessObject bo)
        {
            this.PersistedBusinessObjects.Remove(bo);
        }
    }
}