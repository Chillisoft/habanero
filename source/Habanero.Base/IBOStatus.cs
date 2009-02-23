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
    /// The Current Status of a Business Object.
    ///</summary>
    public interface IBOStatus
    {
        /// <summary>
        /// Indicates if the business object is new
        /// </summary>
        bool IsNew { get;  }

        /// <summary>
        /// Indicates if the business object has been marked for deletion
        /// </summary>
        bool IsDeleted { get;  }

        /// <summary>
        /// Gets and sets the flag which indicates if the business object
        /// is currently being edited
        /// </summary>
        bool IsEditing { get;  }

        /// <summary>
        /// Indicates whether the business object has been amended since it
        /// was last persisted to the database
        /// </summary>
        bool IsDirty { get;  }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <param name="message">If the object is not valid then this returns the reason for it being invalid/param>
        /// <returns>Returns true if all are valid </returns>
        /// </summary>
        bool IsValid(out string message);

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        bool IsValid();

        ///<summary>
        /// Returns an invalid message if the object is valid <see IsValid()>
        ///</summary>
        string IsValidMessage { get; }
    }
}