using System.Collections.Generic;
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
        where TBusinessObject : BusinessObject
    {
        //private BusinessObject _parentBusinessObject;
        //private readonly string _relationshipName;
        private readonly Relationship _relationship;
        private readonly List<TBusinessObject> _removedBusinessObjects = new List<TBusinessObject>();

        ///<summary>
        /// The related business object is constructed with the parent Business object of this 
        /// relationship as well the relationship name.
        ///</summary>
        ///<param name="relationship"></param>
        public RelatedBusinessObjectCollection(Relationship relationship)
        {
            _relationship = relationship;
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public override bool Remove(TBusinessObject bo)
        {
            //TODO: This should be configured in the relationship the relationship
            // should allow you to either delete the object when removing or to dereference the object.
            bo.Delete();
            if (!_removedBusinessObjects.Contains(bo))
            {
                _removedBusinessObjects.Add(bo);
            }
            return base.Remove(bo);
        }

        /// <summary>
        /// Commits to the database all the business objects that are either
        /// new or have been altered since the last committal
        /// </summary>
        public override void SaveAll()
        {
            TransactionCommitterDB committer = new TransactionCommitterDB();

            
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
            foreach (RelPropDef relPropDef in _relationship.RelationshipDef.RelKeyDef)
            {
                bo.SetPropertyValue(relPropDef.RelatedClassPropName,
                                    _relationship.OwningBO.GetPropertyValue(relPropDef.OwnerPropertyName));
                
            }
            return bo;
        }
        ///<summary>
        /// Returns a collection of business objects that have been removed from the relationship
        /// but have not yet been persisted.
        ///</summary>
        public List<TBusinessObject> RemovedBusinessObjects
        {
            get { return _removedBusinessObjects; }
        }
    }
}
