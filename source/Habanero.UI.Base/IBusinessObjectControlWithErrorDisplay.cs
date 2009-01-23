namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an interface for controls used in GridAndBOEditorControlWin, which calls
    /// through to DisplayErrors when the BO is invalid and cannot be persisted.
    /// </summary>
    public interface IBusinessObjectControlWithErrorDisplay : IBusinessObjectControl
    {
        /// <summary>
        /// Displays any errors or invalid fields associated with the BusinessObjectInfo
        /// being edited.  A typical use would involve activating the ErrorProviders
        /// on a BO panel.
        /// </summary>
        void DisplayErrors();

        /// <summary>
        /// Hides all the error providers.  Typically used where a new object has just
        /// been added and the interface is being cleaned up.
        /// </summary>
        void ClearErrors();
    }
}