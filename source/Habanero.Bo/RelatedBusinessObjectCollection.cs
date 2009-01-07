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

        public override void Add(TBusinessObject bo)
        {
            if (!Loading)
            {
                MultipleRelationshipDef def = this._relationship.RelationshipDef as MultipleRelationshipDef;
                if (def != null) def.CheckCanAddChild(bo);
            }
            base.Add(bo);
            if (this.Loading) return;

            if (IsForeignKeySetup(bo)) return;

            SetUpForeignKey(bo);
            SetupRelatedObject(bo);
        }




        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public override bool Remove(TBusinessObject bo)
        {
            MultipleRelationshipDef def = this._relationship.RelationshipDef as MultipleRelationshipDef;
            if (def != null && !Loading && (def.RelationshipType == RelationshipType.Composition))
            {
                string message = "The " + def.RelatedObjectClassName +
                                 " could not be removed since the " + def.RelationshipName +
                                 " relationship is set up as a composition relationship (RemoveChildAction.Prevent)";
                throw new HabaneroDeveloperException(message, message);
            }
            if (!base.Remove(bo)) return false;
            if (this.Loading) return true;
            DereferenceBO(bo);
            RemoveRelatedObject(bo);
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
        /// Creates a business object of type TBusinessObject
        /// Adds this BO to the CreatedBusinessObjects list. When the object is saved it will
        /// be added to the actual bo collection.
        /// </summary>
        /// <returns></returns>
        public override TBusinessObject CreateBusinessObject()
        {
            //TODO: Think about this we are trying to solve the problem that you can set
            // the properties of an object but the related object is only loaded based on its persisted values.
            TBusinessObject bo = base.CreateBusinessObject();
            SetUpForeignKey(bo);
            SetupRelatedObject(bo);
            return bo;
        }

        private void SetupRelatedObject(TBusinessObject bo)
        {
            ISingleRelationship reverseRelationship = GetReverseRelationship(bo) as ISingleRelationship;
            if (reverseRelationship != null)
            {
                reverseRelationship.SetRelatedObject(this._relationship.OwningBO);
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

        protected override void RestoredEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = (TBusinessObject)e.BusinessObject;
            bool removedListContains = this.RemovedBusinessObjects.Contains(bo);
            bool addedListContains = this.AddedBusinessObjects.Contains(bo);
            base.RestoredEventHandler(sender, e);
            if (removedListContains) this.Add(bo);
            if (addedListContains) DereferenceBO(bo);
        }

        protected override void SavedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = (TBusinessObject)e.BusinessObject;
            bool removedListContains = this.RemovedBusinessObjects.Contains(bo);
            base.SavedEventHandler(sender,e);
            if (removedListContains) RemoveFromPersistedCollection(bo);
            if (this.AddedBusinessObjects.Contains(bo)) this.AddedBusinessObjects.Remove(bo);
        }

        private void RemoveFromPersistedCollection(TBusinessObject bo)
        {
            this.PersistedBusinessObjects.Remove(bo);
        }
    }
}