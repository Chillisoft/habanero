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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Maps a multiple relationship collection onto an <see cref="IEditableGridControl"/>
    /// This is used for editing a Many relationship e.g. If an Invoice has many items
    /// then this control mapper can be used to provide an editable grid of Invoice Items.
    /// </summary>
    public class EditableGridControlMapper : ControlMapper
    {
        private readonly IEditableGridControl _editableGrid;

        /// <summary>
        /// Constructor for the mapper.
        /// </summary>
        /// <param name="ctl">The IEditableGridControl</param>
        /// <param name="relationshipName">This is the relationship name to use - this relationship must be a multiple relationship and exist on the BusinessObject</param>
        /// <param name="isReadOnly">Whether the editable grid should be read only or not. Ignored</param>
        /// <param name="factory">The control factory to use</param>
        public EditableGridControlMapper(IControlHabanero ctl, string relationshipName, bool isReadOnly, IControlFactory factory)
            : base(ctl, relationshipName, isReadOnly, factory)
        {
            _editableGrid = (IEditableGridControl)ctl;
            _editableGrid.Buttons.Visible = false;
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject() { }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            if (_businessObject != null)
            {
                IRelationship relationship = _businessObject.Relationships[PropertyName];
                _editableGrid.Initialise(relationship.RelatedObjectClassDef);
                _editableGrid.BusinessObjectCollection =_businessObject.Relationships.GetRelatedCollection(PropertyName);
            } else
            {
                _editableGrid.BusinessObjectCollection = null;
            }
        }
    }
}