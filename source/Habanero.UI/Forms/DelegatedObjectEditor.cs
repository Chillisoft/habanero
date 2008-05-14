//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

namespace Habanero.UI.Forms
{
    ///<summary>
    /// An implememtation of the IBusinessObjectEditor interface where the EditObject method
    /// executes the supplied delegate.
    ///</summary>
    public class DelegatedObjectEditor: IBusinessObjectEditor
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
        public delegate bool EditObjectDelegate(IBusinessObject obj, string uiDefName);

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
        public bool EditObject(IBusinessObject obj, string uiDefName)
        {
            if (_editObjectDelegate != null)
            {
                return _editObjectDelegate(obj, uiDefName);
            }
            return false;
        }
    }
}
