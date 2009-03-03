//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

namespace Habanero.Base
{
    ///<summary>
    /// Delegator definition to be used by the Interface IBusinessObjectEditor for 
    /// running a post save (persist) delegate on the busines object
    /// This is needed by Webgui where the pop up edit forms are running out of thread to the
    /// rest of the application.
    ///</summary>
    ///<param name="bo"></param>
    ///<param name="cancelled"></param>
    public delegate void PostObjectEditDelegate(IBusinessObject bo, bool cancelled);

    /// <summary>
    /// Provides a facility to edit business objects
    /// </summary>
    public interface IBusinessObjectEditor
    {
        /// <summary>
        /// Edits the given object
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        bool EditObject(IBusinessObject obj, string uiDefName);

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
        bool EditObject(IBusinessObject obj, string uiDefName, PostObjectEditDelegate postEditAction);
    }
    ///<summary>
    /// Delegator definition to be used by the Interface IBusinessObjectEditor for 
    /// running a post save (persist) delegate on the busines object
    /// This is needed by Webgui where the pop up edit forms are running out of thread to the
    /// rest of the application.
    ///</summary>
    ///<param name="bo">The business object being edited</param>
    ///<param name="cancelled">Whether the Edits where cancelled or not</param>
    public delegate void PostObjectEditDelegate<T>(T bo, bool cancelled);

    /// <summary>
    /// Provides a facility to edit business objects of a specific type
    /// </summary>
    public interface IBusinessObjectEditor<T> : IBusinessObjectEditor
    {

        /// <summary>
        /// Edits the given object
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        bool EditObject(T obj, string uiDefName);
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
        bool EditObject(T obj, string uiDefName, PostObjectEditDelegate<T> postEditAction);
    }
}