using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Bo
{
    public class BOState
    {
        private States _flagState = States.isNew;
        /// <summary>
        /// An enumeration that describes the object's state
        /// </summary>
        [Flags()]
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
        /// Checks that the specified flag value matches the value specified,
        /// and throws an exception if it does not
        /// </summary>
        /// <param name="objFlag">The flag to check. See the States
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value the flag should hold</param>
        private void CheckBOFlagValue(States objFlag, bool bValue)
        {
            if (GetBOFlagValue(objFlag) != bValue)
            {
                CheckBOFlagValue(objFlag, bValue, "The " + this.GetType().Name +
                                              " object is " + (bValue ? "not " : "") + objFlag.ToString());
            }
        }

        /// <summary>
        /// Checks that the specified flag value matches the value specified,
        /// and throws an exception if it does not, using the exception message
        /// provided
        /// </summary>
        /// <param name="objFlag">The flag to check. See the States
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value the flag should hold</param>
        /// <param name="errorMessage">The error message to display if the flag
        /// value is not as expected</param>
        private void CheckBOFlagValue(States objFlag,
                                        bool bValue,
                                        string errorMessage)
        {
            if (GetBOFlagValue(objFlag) != bValue)
            {
                throw (new Exception(errorMessage));
            }
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
