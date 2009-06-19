using System;

namespace Habanero.BO.ClassDefinition
{
    public interface IUIFormGrid
    {
        /// <summary>
        /// Returns the relationship name
        /// </summary>
        string RelationshipName { get; set; }

        /// <summary>
        /// Returns the grid type
        /// </summary>
        Type GridType { get; set; }

        /// <summary>
        /// Returns the corresponding relationship name
        /// </summary>
        string CorrespondingRelationshipName { get; set; }
    }
}