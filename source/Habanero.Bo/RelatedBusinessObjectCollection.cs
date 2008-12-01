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

        public override void Add(TBusinessObject bo)
        {
            base.Add(bo);
            if (bo.Status.IsNew)
            {
                SetUpForeignKey(bo);
            }
            //TODO: what must we do if you add a business object to a relationship but the foreign key does not match?
            // Possibly this is a strategy must look at this so that extendable
            // (see similar issue for remove below)
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public override bool Remove(TBusinessObject bo)
        {
            //TODO: This should be configured in the relationship the relationship
            // should allow you to either delete the object when removing or to dereference the object.
            //for now will dereference as well as delete.
            if (!bo.Status.IsNew) bo.Delete();

            DereferenceBO(bo);
            return base.Remove(bo);
        }

        private void DereferenceBO(TBusinessObject bo)
        {
            foreach (RelPropDef relPropDef in _relationship.RelationshipDef.RelKeyDef)
            {
                bo.SetPropertyValue(relPropDef.RelatedClassPropName, null);
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
            //bo.Relationships this._relationship.OwningBO
            return bo;
        }

        private void SetUpForeignKey(TBusinessObject bo)
        {
            foreach (RelPropDef relPropDef in _relationship.RelationshipDef.RelKeyDef)
            {
                bo.SetPropertyValue(relPropDef.RelatedClassPropName,
                                    _relationship.OwningBO.GetPropertyValue(relPropDef.OwnerPropertyName));
                
            }
        }
    }
}
