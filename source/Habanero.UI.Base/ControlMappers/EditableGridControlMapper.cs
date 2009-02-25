namespace Habanero.UI.Base
{
    /// <summary>
    /// Maps a multiple relationship collection onto an <see cref="IEditableGridControl"/>
    /// </summary>
    public class EditableGridControlMapper : ControlMapper
    {
        private readonly IEditableGridControl _editableGrid;

        /// <summary>
        /// Constructor for the mapper.
        /// </summary>
        /// <param name="ctl">The IEditableGridControl</param>
        /// <param name="propName">This is the relationship name to use - this relationship must be a multiple relationship and exist on the BusinessObject</param>
        /// <param name="isReadOnly">Whether the editable grid should be read only or not. Ignored</param>
        /// <param name="factory">The control factory to use</param>
        public EditableGridControlMapper(IControlHabanero ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
            _editableGrid = (IEditableGridControl)ctl;
            _editableGrid.Buttons.Visible = false;
        }
        public override void ApplyChangesToBusinessObject() { }
        protected override void InternalUpdateControlValueFromBo()
        {
            _editableGrid.Initialise(_businessObject.Relationships[_propertyName].RelatedObjectClassDef);
            _editableGrid.SetBusinessObjectCollection(_businessObject.Relationships.GetRelatedCollection(_propertyName));

        }
    }
}