using System;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages property definitions for a grid in a user interface editing
    /// form, as specified in the class definitions xml file
    /// </summary>
    /// TODO ERIC - review - what does this do? relationships?
    public class UIFormGrid
    {
        private string itsRelationshipName;
        public Type itsGridType;
        private string itsCorrespondingRelationshipName;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="relationshipName">The relationship name</param>
        /// <param name="gridType">The grid type</param>
        /// <param name="correspondingRelationshipName">The corresponding
        /// relationship name</param>
        public UIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName)
        {
            this.itsRelationshipName = relationshipName;
            this.itsGridType = gridType;
            this.itsCorrespondingRelationshipName = correspondingRelationshipName;
        }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        public string RelationshipName
        {
            get { return itsRelationshipName; }
        }

        /// <summary>
        /// Returns the grid type
        /// </summary>
        public Type GridType
        {
            get { return itsGridType; }
        }

        /// <summary>
        /// Returns the corresponding relationship name
        /// </summary>
        public string CorrespondingRelationshipName
        {
            get { return itsCorrespondingRelationshipName; }
        }
    }
}