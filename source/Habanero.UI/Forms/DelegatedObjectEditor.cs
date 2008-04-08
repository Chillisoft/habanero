using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.UI.Forms
{
    ///<summary>
    /// An implememtation of the IObjectEditor interface where the EditObject method
    /// executes the supplied delegate.
    ///</summary>
    public class DelegatedObjectEditor: IObjectEditor
    {
        /// <summary>
        /// A delegate that contains the same parameters as the EditObject method.
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        public delegate bool EditObjectDelegate(object obj, string uiDefName);

        private EditObjectDelegate _editObjectDelegate;
        
        /// <summary>
        /// The constructor for this class allows for the implementation of the EditObject
        /// method to be supplied by a delegate.
        /// </summary>
        /// <param name="editObjectDelegate">This delegate will be executed as the implementation of the EditObject method.</param>
        public DelegatedObjectEditor(EditObjectDelegate editObjectDelegate)
        {
            _editObjectDelegate = editObjectDelegate;
        }

        /// <summary>
        /// Edits the given object.
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        public bool EditObject(object obj, string uiDefName)
        {
            if (_editObjectDelegate != null)
            {
                return _editObjectDelegate(obj, uiDefName);
            }
            return false;
        }
    }
}
