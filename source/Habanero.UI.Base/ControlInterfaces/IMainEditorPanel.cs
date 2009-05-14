namespace Habanero.UI.Base
{
    /// <summary>
    /// This is a Main Editor Panel that consists of a Header control that can be styled and takes an Icon and a Title.
    /// </summary>
    public interface IMainEditorPanel: IPanel
    {
        /// <summary>
        /// The Control that is positioned at the top of this panel that can be used to set an icon and title for the
        ///  information being displayed on the MainEditorPanelVWG
        /// </summary>
        IMainTitleIconControl MainTitleIconControl { get; }

        /// <summary>
        /// The Panel in which the control being set is placed in.
        /// </summary>
        IPanel EditorPanel { get; }
    }
}