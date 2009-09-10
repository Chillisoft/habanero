// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
namespace Habanero.Base
{
    ///<summary>
    /// The interface used to implement relationships between two classes.
    ///</summary>
    public interface IRelationship
    {
        ///<summary>
        /// The key that identifies this relationship i.e. the properties in the 
        /// source object and how they are related to properties in the related object.
        ///</summary>
        IRelKey RelKey
        {
            get;
        }

        /// <summary>
        /// The class Definition for the related object.
        /// </summary>
        IClassDef RelatedObjectClassDef { get; }

        ///<summary>
        /// Returns whether the relationship is dirty or not.
        /// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
        /// If the relationship is of type composition or aggregation then it is dirty if it has any 
        ///  related (children) business objects that are dirty.
        ///</summary>
        bool IsDirty { get; }

        /// <summary>
        /// Returns the relationship definition
        /// </summary>
        IRelationshipDef RelationshipDef { get; }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        string RelationshipName { get; }

        ///<summary>
        /// Returns true if the Relationship is initialised or not.
        ///</summary>
        bool Initialised { get; }

        ///<summary>
        /// Returns the appropriate delete action when the parent is deleted.
        /// i.e. delete related objects, dereference related objects, prevent deletion.
        ///</summary>
        DeleteParentAction DeleteParentAction { get; }

        ///<summary>
        /// Returns the business object that owns this relationship e.g. Invoice has many lines
        /// the owning BO would be invoice.
        ///</summary>
        IBusinessObject OwningBO { get; }

        /////<summary>
        ///// Returns a list of all the related objects that are dirty.
        ///// In the case of a composition or aggregation this will be a list of all 
        /////   dirty related objects (child objects). 
        ///// In the case of association
        /////   this will only be a list of related objects that are added, removed, marked4deletion or created
        /////   as part of the relationship.
        /////</summary>
        //IList<IBusinessObject> GetDirtyChildren();

        /// <summary>
        /// Is there anything in this relationship to prevent the business object from being deleted.
        /// e.g. if there are related business objects that are not marked as mark for delete.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool IsDeletable(out string message);


        /// <summary>
        /// If the relationship is <see cref="DeleteParentAction"/>.DeleteRelated then
        /// all the related objects and their relevant children will be marked for Delete.
        /// See <see cref="IBusinessObject.MarkForDelete"/>
        /// </summary>
        void MarkForDelete();
    }
}
