using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an editing facility for a business object.
    /// The default editor is used by facilities
    /// like readOnlyGridControl to edit business objects.  Inherit
    /// from this class if you need to carry out additional steps at the time
    /// of editing a new business object or if you need to specify a
    /// different editing form to use to edit the object.
    /// </summary>
    public class DefaultBOEditor : IBusinessObjectEditor
    {
        private readonly IControlFactory _controlFactory;

        public DefaultBOEditor(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Edits the given business object by providing a form in which the
        /// user can edit the data
        /// </summary>
        /// <param name="obj">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if the user chose to save the edits or
        /// false if the user cancelled the edits</returns>
        public bool EditObject(IBusinessObject obj, string uiDefName)
        {
            BusinessObject bo = (BusinessObject)obj;
            IDefaultBOEditorForm form = CreateEditorForm(bo, uiDefName);
            return form.ShowDialog();
        }

        /// <summary>
        /// Edits the given object
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        /// <param name="postEditAction">The delete to be executeActionOn After The edit is saved.
        /// will be the object that the method is called on</param>
        public bool EditObject(IBusinessObject obj, string uiDefName, PostObjectPersistingDelegate postEditAction)
        {
            BusinessObject bo = (BusinessObject)obj;
            IDefaultBOEditorForm form = CreateEditorForm(bo, uiDefName, postEditAction);
            return form.ShowDialog();
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns a DefaultBOEditorForm object</returns>
        protected virtual IDefaultBOEditorForm CreateEditorForm(BusinessObject bo, string uiDefName)
        {
            return _controlFactory.CreateBOEditorForm(bo, uiDefName);
        }

        private IDefaultBOEditorForm CreateEditorForm(BusinessObject bo, string uiDefName, PostObjectPersistingDelegate action)
        {
            return _controlFactory.CreateBOEditorForm(bo, uiDefName, action);
        }
    }
}