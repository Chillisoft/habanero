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
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This mapper loads all the Business Objects of type relationshipDef.RelatedObjectClassDef.
    /// </summary>
    public class AutoLoadingRelationshipComboBoxMapper : RelationshipComboBoxMapper
    {
        /// <summary>
        /// Constructs an <see cref="AutoLoadingRelationshipComboBoxMapper"/> with the <paramref name="comboBox"/>
        ///  <paramref name="relationshipName"/>
        /// </summary>
        /// <param name="comboBox">The combo box that is being mapped to</param>
        /// <param name="relationshipName">The name of the relation that is being mapped to</param>
        /// <param name="isReadOnly">Whether the Combo box can be used to edit from or whether it is only viewable</param>
        /// <param name="controlFactory">A control factory that is used to create control mappers etc</param>
        public AutoLoadingRelationshipComboBoxMapper(IComboBox comboBox, string relationshipName, bool isReadOnly, IControlFactory controlFactory) : base(comboBox, relationshipName, isReadOnly, controlFactory) {}


        /// <summary>
        /// Provides an overrideable method for Loading the collection of business objects
        /// </summary>
        protected override void LoadCollectionForBusinessObject() { 
           // if (this.BusinessObjectCollection == null)
            //{
                
                this.BusinessObjectCollection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(
                    RelatedObjectClassDef, "");
            //}       
        }
    }
}
