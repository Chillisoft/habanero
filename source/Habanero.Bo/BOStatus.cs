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
using System;
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO
{
    ///<summary>
    /// The current state of a business object.
    ///</summary>
    [Serializable]
    public class BOStatus : IBOStatus
    {
        private BusinessObject _bo;
        private Statuses _flagState = Statuses.isNew;

        ///<summary>
        ///</summary>
        ///<param name="bo"></param>
        public BOStatus(BusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            _bo = bo;
        }

        /// <summary>
        /// An enumeration that describes the object's state
        /// </summary>
        [Flags]
        internal enum Statuses
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
            get { return GetBOFlagValue(Statuses.isNew); }

            internal set { SetBOFlagValue(Statuses.isNew, value); }
        }

        /// <summary>
        /// Indicates if the business object has been deleted
        /// </summary>
        public bool IsDeleted
        {
            get { return GetBOFlagValue(Statuses.isDeleted); }
            internal set { SetBOFlagValue(Statuses.isDeleted, value); }
        }

        /// <summary>
        /// Gets and sets the flag which indicates if the business object
        /// is currently being edited
        /// </summary>
        public bool IsEditing
        {
            get { return GetBOFlagValue(Statuses.isEditing); }
            internal set { SetBOFlagValue(Statuses.isEditing, value); }
        }

        /// <summary>
        /// Indicates whether the business object has been amended since it
        /// was last persisted to the database
        /// </summary>
        public bool IsDirty
        {
            get
            {
                if (this._bo == null)
                {
                    const string message = "The IsDirty Failed because the Business Object is null";
                    throw new HabaneroDeveloperException(message, message);
                }
                return (GetBOFlagValue(Statuses.isDirty)) || this._bo.Relationships.IsDirty;
            }
            internal set { SetBOFlagValue(Statuses.isDirty, value); }
        }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <param name="message">If the object is not valid then this returns the reason for it being invalid</param>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid(out string message)
        {
            IList<IBOError> errors;
            bool isValid = IsValid(out errors);
            message = errors
                .Aggregate("", (current, error) => StringUtilities.AppendMessage(current, error.Message));
            return isValid;
        }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <param name="errors">If the object is not valid then this list is populated with the errors</param>
        /// <returns>Returns true if all are valid </returns>
        public bool IsValid(out IList<IBOError> errors)
        {
            errors = new List<IBOError>();
            if (IsDeleted) return true;

            bool valid = _bo.Props.IsValid(out errors);
            foreach (BOError error in errors)
            {
                error.BusinessObject = this.BusinessObject;
            }

            IList<IBOError> customRuleErrors;
            valid &= _bo.AreCustomRulesValidInternal(out customRuleErrors);
            if (customRuleErrors != null)
            {
                foreach (BOError error in customRuleErrors)
                {
                    error.BusinessObject = this.BusinessObject;
                    errors.Add(error);
                }
            }

            string customRuleErrorsString;
            valid &= _bo.AreCustomRulesValidInternal(out customRuleErrorsString);
            if (!String.IsNullOrEmpty(customRuleErrorsString))
            {
                BOError customError = new BOError(customRuleErrorsString, ErrorLevel.Error)
                                          {BusinessObject = this.BusinessObject};
                errors.Add(customError);
            }

            return valid;
        }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid()
        {
            string invalidReason;
            return IsValid(out invalidReason);
        }

        ///<summary>
        /// Returns an invalid message if the object is valid <see cref="IsValid()"/>
        ///</summary>
        public string IsValidMessage
        {
            get
            {
                string message;
                IsValid(out message);
                return message;
            }
        }

        /// <summary>
        /// Returns the Business Object that this Status is for.
        /// </summary>
        public IBusinessObject BusinessObject
        {
            get { return _bo; }
            internal set
            {
                if (value == null) throw new ArgumentNullException("value");
                _bo = (BusinessObject) value;
            }
        }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid and that the business object is a valid state to persist.
        /// Also returns true if the Business Object has any warnings or Suggestions <see cref="ErrorLevel"/>.
        /// Warnings and Suggestions do not prevent the Business Object from being persisted but indicate that the
        /// Business object is not in a valid state e.g. A Customer order can be saved but cannot be Approved if it has warnings.
        /// </summary>
        /// <param name="errors">If the object is not valid then this list is populated with the errors</param>
        /// <returns>Returns true if all are valid </returns>
        public bool HasWarnings(out IList<IBOError> errors)
        {
            IsValid(out errors);

            IList<IBOError> warnings;
            _bo.HasWarnings(out warnings);
            foreach (IBOError error in warnings)
            {
                errors.Add(error);
            }
            return errors.Count != 0;
        }

        /// <summary>
        /// Indicates if the specified flag is currently set
        /// </summary>
        /// <param name="objFlag">The flag in question. See the Statuses
        /// enumeration for more detail.</param>
        /// <returns>Returns true if set, false if not</returns>
        private bool GetBOFlagValue(Statuses objFlag)
        {
            return Convert.ToBoolean(_flagState & objFlag);
        }

        /// <summary>
        /// Sets the flag value as specified
        /// </summary>
        /// <param name="flag">The flag value to set. See the Statuses
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value to set to</param>
        internal void SetBOFlagValue(Statuses flag, bool bValue)
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

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        ///<exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            BOStatus otherStatus = obj as BOStatus;
            if (otherStatus == null) return false;
            return (this._flagState == otherStatus._flagState);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _flagState.GetHashCode();
        }
    }
}