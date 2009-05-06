using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.BO;

namespace Habanero.UI.Base
{
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


        protected override void LoadCollectionForBusinessObject() { 
            if (this.BusinessObjectCollection == null)
            {
                
                this.BusinessObjectCollection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(
                    _relationshipDef.RelatedObjectClassDef, "");
            }       
        }
    }
}
