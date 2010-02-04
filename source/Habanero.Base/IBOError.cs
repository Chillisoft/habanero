// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
    /// <summary>
    /// The Error Level for this Error e.g. Error or Warning
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// Is this an Error e.g. the Object cannot be saved unless this is fixed.
        /// </summary>
        Error,
        /// <summary>
        /// Is this a warning e.g. the object can be saved but is not considered to be in a valid state until it is repaired.
        /// </summary>
        Warning,
        /// <summary>
        /// Is this a suggestion e.g. the object can be saved and is considered valid but there may be a better way of doing it.
        /// </summary>
        Suggestion
    }
    /// <summary>
    /// An interface representing a particular Error on a <see cref="IBusinessObject"/>.
    /// </summary>
    public interface IBOError
    {
        /// <summary>
        /// The Business Object that the error occured on.
        /// </summary>
        IBusinessObject BusinessObject { get; }
        /// <summary>
        /// The <see cref="ErrorLevel"/> of the business object.
        /// </summary>
        ErrorLevel Level { get; }
        /// <summary>
        /// The Message to be shown to the End user for a particular error message.
        /// </summary>
        string Message { get; }
    }
}