using System;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Manages property definitions for a grid in a user interface editing
    /// form that edits properties belonging to a class that is in a
    /// relationship with the object being edited.  For example, if you have
    /// order items that make up an order, contained in the editing form
    /// for the order would be a UIFormGrid in which you can edit the
    /// individual order items.
    /// </summary>
    public class UIFormGrid
    {
        private string _relationshipName;
        private Type _gridType;
        private string _correspondingRelationshipName;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="relationshipName">The relationship name</param>
        /// <param name="gridType">The grid type</param>
        /// <param name="correspondingRelationshipName">The corresponding
        /// relationship name</param>
        public UIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName)
        {
            this._relationshipName = relationshipName;
            this._gridType = gridType;
            this._correspondingRelationshipName = correspondingRelationshipName;
        }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        public string RelationshipName
        {
            get { return _relationshipName; }
            protected set { _relationshipName = value; }
        }

        /// <summary>
        /// Returns the grid type
        /// </summary>
        public Type GridType
        {
            get { return _gridType; }
            protected set { _gridType = value; }
        }

        /// <summary>
        /// Returns the corresponding relationship name
        /// </summary>
        public string CorrespondingRelationshipName
        {
            get { return _correspondingRelationshipName; }
            protected set { _correspondingRelationshipName = value; }
        }
    }
}