using System;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.BOControls.v2;
using BusinessObject=Chillisoft.Bo.v2.BusinessObject;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Provides an editing facility for a business object.
    /// The default editor is used by facilities
    /// like ReadOnlyGridWithButtons to edit business objects.  Inherit
    /// from this class if you need to carry out additional steps at the time
    /// of editing a new business object or if you need to specify a
    /// different editing form to use to edit the object.
    /// </summary>
    public class DefaultBOEditor : IObjectEditor
    {
        /// <summary>
        /// Edits the given business object by providing a form in which the
        /// user can edit the data
        /// </summary>
        /// <param name="obj">The business object to edit</param>
        /// <returns>Returs true if the user chose to save the edits or
        /// false if the user cancelled the edits</returns>
        public bool EditObject(Object obj)
        {
            BusinessObject bo = (BusinessObject) obj;
            DefaultBOEditorForm form = CreateEditorForm(bo);
            if (form.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <returns>Returns a DefaultBOEditorForm object</returns>
        protected virtual DefaultBOEditorForm CreateEditorForm(BusinessObject bo)
        {
            return new DefaultBOEditorForm(bo);
        }
    }
}