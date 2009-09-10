//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an editing facility for a business object.
    /// The default editor is used by controls like GridControl to edit business objects.
    /// Where used, this class can be replaced by a custom control that
    /// inherits form <see cref="IBusinessObjectEditor"/>.
    /// </summary>
    public class DefaultBOEditor : IBusinessObjectEditor
    {
        /// <summary>
        /// The Control Factory that is used to create the controls on this Editor.
        /// </summary>
        protected readonly IControlFactory _controlFactory;

        ///<summary>
        /// Constructor for <see cref="DefaultBOEditor"/>
        ///</summary>
        ///<param name="controlFactory"></param>
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
        public virtual bool EditObject(IBusinessObject obj, string uiDefName)
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
        /// <param name="postEditAction">Action to be performed when the editing is completed or cancelled. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid)</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        public virtual bool EditObject(IBusinessObject obj, string uiDefName, PostObjectEditDelegate postEditAction)
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
        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <param name="action"></param>
        /// <returns>Returns a DefaultBOEditorForm object</returns>
        protected virtual IDefaultBOEditorForm CreateEditorForm(BusinessObject bo, string uiDefName, PostObjectEditDelegate action)
        {
            return _controlFactory.CreateBOEditorForm(bo, uiDefName, action);
        }
    }
}