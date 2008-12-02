using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Checks whether the user wants to save
    /// </summary>
    public delegate DialogResult ConfirmSave();

    /// <summary>
    /// Represents a control to edit a collection of business objects.  A grid
    /// lists the objects as specified by SetBusinessObjectCollection and a control
    /// below the grid allows the selected business object to be edited.  Default
    /// buttons are provided: Save, New, Delete and Cancel.
    /// <br/>
    /// The editing control is
    /// specified here as a IBusinessObjectControl, allowing the developer to pass
    /// in a custom control, but the default instantiation uses a IBusinessObjectPanel,
    /// which is more suited to displaying errors.  If the developer provides a custom
    /// control, they are responsible for updating the business object status
    /// and displaying useful feedback to the user (by
    /// catching appropriate events on the business object or the controls).
    /// <br/>
    /// Some customisation is provided through the GridWithPanelControlStrategy,
    /// including how controls should be enabled for the appropriate environment.
    /// </summary>
    public interface IGridWithPanelControl<TBusinessObject> : IControlHabanero
    {
        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        void SetBusinessObjectCollection(IBusinessObjectCollection col);

        IReadOnlyGridControl ReadOnlyGridControl { get; }
        IBusinessObjectControl BusinessObjectControl { get; }
        IButtonGroupControl ButtonGroupControl { get; }
        TBusinessObject CurrentBusinessObject { get; }
        IGridWithPanelControlStrategy<TBusinessObject> GridWithPanelControlStrategy { get; set; }

        /// <summary>
        /// Called when the user attempts to move away from a dirty business object
        /// and needs to indicate Yes/No/Cancel to the option of saving.  This delegate
        /// facility is provided primarily to facilitate testing.
        /// </summary>
        ConfirmSave ConfirmSaveDelegate { get; set;}

    }

    public interface IGridWithPanelControlStrategy<TBusinessObject>
    {
        void UpdateControlEnabledState(IBusinessObject lastSelectedBusinessObject);
        bool ShowConfirmSaveDialog { get; }

        /// <summary>
        /// Indicates whether PanelInfo.ApplyChangesToBusinessObject needs to be
        /// called to copy control values to the business object.  This will be
        /// the case if the application uses minimal events and does not update
        /// the BO every time a control value changes.
        /// </summary>
        bool CallApplyChangesToEditBusinessObject { get; }
    }
}