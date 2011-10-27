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
    /// <summary>
    /// An interface to model a generator of unique numbers. It keeps
    /// record of the last number retrieved and can provide a new
    /// unique number on request.  An example usage might be a till
    /// receipt number for a retail business.  Incrementing the numbers
    /// as they are dispensed is one means of achieving uniqueness.
    /// </summary>
    public interface INumberGenerator
    {
        /// <summary>
        /// Returns the next available unique number. One possible means
        /// of providing unique numbers is simply to increment the last one
        /// dispensed.
        /// </summary>
        /// <returns>Returns an integer</returns>
        long NextNumber();
        /// <summary>
        /// Allows the developer to set the new Sequence number this can be used when initialy creating the numbers e.g. when 
        /// you want to ensure that the numbers are generated starting at 10000.
        /// </summary>
        /// <param name="newSequenceNumber"></param>
        void SetSequenceNumber(long newSequenceNumber);

        /// <summary>
        /// Interface to add the number generator to a transaction via the transaction committer.
        /// </summary>
        /// <param name="transactionCommitter"></param>
        void AddToTransaction(ITransactionCommitter transactionCommitter);
    }
}