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

namespace Habanero.Base
{
    ///<summary>
    /// A delegate which provides a value specifying the user's confirmation response.
    ///</summary>
    ///<param name="confirmed">The user's confirmation response.</param>
    public delegate void ConfirmationDelegate(bool confirmed);

    /// <summary>
    /// An interface to model a tool to get confirmation from the user before
    /// proceeding with some action
    /// </summary>
    public interface IConfirmer
    {
        /// <summary>
        /// Gets confirmation from the user after providing them with an option
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>Returns true if the user confirms the choice and false
        /// if they decline the offer</returns>
        bool Confirm(string message);

        ///<summary>
        /// Gets confirmation from the user after providing them with an option
        /// and executes the provided delegate once the user has responded.
        ///</summary>
        ///<param name="message">The message to display</param>
        ///<param name="confirmationDelegate">The delegate to execute once the user has responded.</param>
        ///<returns>Returns true if the user confirms the choice and false
        /// if they decline the offer</returns>
        void Confirm(string message, ConfirmationDelegate confirmationDelegate);
    }
}