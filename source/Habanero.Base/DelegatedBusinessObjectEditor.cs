// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.Base
{
    ///<summary>
    /// A BusinessObjectEditor that passes off its logic to a delegate passed in the constructor.
    ///</summary>
    public class DelegatedBusinessObjectEditor : DelegatedBusinessObjectEditor<IBusinessObject>
    {
        ///<summary>
        /// Initialises the <see cref="DelegatedBusinessObjectEditor"/> with the specified delegate.
        ///</summary>
        ///<param name="editObjectDelegate">The delegate to be executed when EditObject is called.</param>
        public DelegatedBusinessObjectEditor(EditObjectDelegate editObjectDelegate) 
            : base(editObjectDelegate)
        {
        }
    }

    ///<summary>
    /// A BusinessObjectEditor that passes off its logic to a delegate passed in the constructor.
    ///</summary>
    public class DelegatedBusinessObjectEditor<TBusinessObject> : IBusinessObjectEditor
        where TBusinessObject : class, IBusinessObject
    {
        private readonly EditObjectDelegate _editObjectDelegate;

        ///<summary>
        /// The delegate for the EditObject methods.
        ///</summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <param name="postEditAction">The delete to be executeActionOn After The edit is saved.
        /// will be the object that the method is called on</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        public delegate bool EditObjectDelegate(TBusinessObject obj, string uiDefName, PostObjectEditDelegate postEditAction);

        ///<summary>
        /// Initialises the <see cref="DelegatedBusinessObjectEditor{TBusinessObject}"/> with the specified delegate.
        ///</summary>
        ///<param name="editObjectDelegate">The delegate to be executed when EditObject is called.</param>
        public DelegatedBusinessObjectEditor(EditObjectDelegate editObjectDelegate)
        {
            _editObjectDelegate = editObjectDelegate;
        }

        #region Implementation of IBusinessObjectEditor

        /// <summary>
        /// Edits the given object
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        public bool EditObject(IBusinessObject obj, string uiDefName)
        {
            return EditObject(obj, uiDefName, null);
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
        public bool EditObject(IBusinessObject obj, string uiDefName, PostObjectEditDelegate postEditAction)
        {
            if (_editObjectDelegate == null) return true;
            if (!(obj is TBusinessObject)) return true;
            return _editObjectDelegate((TBusinessObject) obj, uiDefName, postEditAction);
        }

        #endregion
    }
}