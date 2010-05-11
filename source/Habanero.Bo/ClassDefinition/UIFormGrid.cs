// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages property definitions for a grid in a user interface editing
    /// form that edits properties belonging to a class that is in a
    /// relationship with the object being edited.  For example, if you have
    /// order items that make up an order, contained in the editing form
    /// for the order would be a UIFormGrid in which you can edit the
    /// individual order items.
    /// </summary>
    public class UIFormGrid : IUIFormGrid
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
            set { _relationshipName = value; }
        }

        /// <summary>
        /// Returns the grid type
        /// </summary>
        public Type GridType
        {
            get { return _gridType; }
            set { _gridType = value; }
        }

        /// <summary>
        /// Returns the corresponding relationship name
        /// </summary>
        public string CorrespondingRelationshipName
        {
            get { return _correspondingRelationshipName; }
            set { _correspondingRelationshipName = value; }
        }
    }
}