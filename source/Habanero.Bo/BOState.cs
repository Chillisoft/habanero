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

using System;

namespace Habanero.BO
{
    ///<summary>
    /// The current state of a business object.
    ///</summary>
    public class BOState
    {
        private readonly BusinessObject _bo;

        ///<summary>
        ///</summary>
        ///<param name="bo"></param>
        public BOState(BusinessObject bo)
        {
            _bo = bo;
        }
        private States _flagState = States.isNew;
        /// <summary>
        /// An enumeration that describes the object's state
        /// </summary>
        [Flags]
        private enum States
        {
            /// <summary>The object is new</summary>
            isNew = 1,
            /// <summary>The object has changed since its last persistance to
            /// the database</summary>
            isDirty = 2,
            /// <summary>The object has been deleted</summary>
            isDeleted = 4,
            /// <summary>The object is being edited</summary>
            isEditing = 8,
        }

        /// <summary>
        /// Indicates if the business object is new
        /// </summary>
        public bool IsNew
        {
            get { return GetBOFlagValue(States.isNew); }
            internal set
            {
                SetBOFlagValue(States.isNew, value);
            }
        }

        /// <summary>
        /// Indicates if the business object has been deleted
        /// </summary>
        public bool IsDeleted
        {
            get { return GetBOFlagValue(States.isDeleted); }
            internal set { SetBOFlagValue(States.isDeleted, value); }
        }

        /// <summary>
        /// Gets and sets the flag which indicates if the business object
        /// is currently being edited
        /// </summary>
        public bool IsEditing
        {
            get { return GetBOFlagValue(States.isEditing); }
            internal set { SetBOFlagValue(States.isEditing, value); }
        }

        /// <summary>
        /// Indicates whether the business object has been amended since it
        /// was last persisted to the database
        /// </summary>
        public bool IsDirty
        {
            get { return GetBOFlagValue(States.isDirty); }
            internal set { SetBOFlagValue(States.isDirty, value); }
        }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <param name="message">If the object is not valid then this returns the reason for it being invalid/param>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid(out string message)
        {
                return _bo.IsValid(out message);
        }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid()
        {
            return _bo.IsValid();
        }

        ///<summary>
        /// Returns an invalid message if the object is valid <see IsValid()>
        ///</summary>
        public string IsValidMessage
        {
            get
            {
                string message;
                _bo.IsValid(out message);
                return message;

            }
        }

        /// <summary>
        /// Indicates if the specified flag is currently set
        /// </summary>
        /// <param name="objFlag">The flag in question. See the States
        /// enumeration for more detail.</param>
        /// <returns>Returns true if set, false if not</returns>
        private bool GetBOFlagValue(States objFlag)
        {
            return Convert.ToBoolean(_flagState & objFlag);
        }

        /// <summary>
        /// Sets the flag value as specified
        /// </summary>
        /// <param name="flag">The flag value to set. See the States
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value to set to</param>
        private void SetBOFlagValue(States flag, bool bValue)
        {
            if (bValue)
            {
                _flagState = _flagState | flag;
            }
            else
            {
                _flagState = _flagState & ~flag;
            }
        }
    }
}
