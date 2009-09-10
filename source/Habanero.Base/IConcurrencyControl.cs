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
    /// <summary>
    /// An interface to model optimistic or pessimistic concurrency
    /// control, as used by business objects.
    /// This interface fulfills the roll of the Strategy Object 
    /// in the GOF Strategy pattern.
    /// <br/><br/>
    /// Since this architecture/framework supports the storing of 
    /// objects in an object manager, it is possible to retrieve a 
    /// stale object that has since been edited by another user or process.
    /// This interface allows you to implement a concurrency control 
    /// strategy to deal with this by
    /// raising an error, automatically refreshing the object, putting 
    /// a read lock on the object or any other 
    /// strategy that you wish to implement.
    /// </summary>
    public interface IConcurrencyControl
    {
        //This is no longer used every time a non dirty object is retrieved from the 
        // object manager it is refreshed.
        ///// <summary>
        ///// Checks concurrency when retrieving an object from the object
        ///// manager, in order to ensure that up-to-date information is
        ///// displayed to the user
        ///// </summary>
        //void CheckConcurrencyOnGettingObjectFromObjectManager();

        /// <summary>
        /// Checks concurrency before the user begins editing an object, in
        /// order to avoid the user making changes to an object and then losing
        /// those changes when the committal process shows a concurrency
        /// failure.
        /// Strategy1: SemiOptimisitic Concurrency Control
        /// If another user has edited and persisted the object between when the 
        /// current user loaded the object and when she started editing the object
        /// a BusObjBeginEditConcurrencyControlException Exception will be thrown.
        /// Strategy2: Pessimistic ConcurrencyControl
        /// If another user has begun edits on an object then the object is marked
        /// as locked. Any other user may view the object but any user attempting
        /// to begin edits on the object will result in 
        /// a BusObjPessimisticConcurrencyControlException Exception
        /// </summary>
        /// <exception >BusObjBeginEditConcurrencyControlException</exception>
        /// <exception >BusObjPessimisticConcurrencyControlException</exception>
        void CheckConcurrencyBeforeBeginEditing();

        /// <summary>
        /// Checks concurrency before persisting an object to the datasource
        /// in order to prevent one of two conflicting copies from being lost.
        /// If another user has edited and persisted the object between when the 
        /// current user started editing the object and tried to persist then a
        /// a BusObjOptimisticConcurrencyControlException Exception will be thrown.
        /// This would happen when an object is implementing a fully optimistic
        /// concurrency strategy.
        /// </summary>
        /// <exception >BusObjOptimisticConcurrencyControlException</exception>
        void CheckConcurrencyBeforePersisting();

        /// <summary>
        /// Many optimistic concurrency control strategies rely on updating 
        /// certain properties in the datasource, such as the version number,
        /// time last updated, etc. This method must be implemented in the 
        /// "concrete concurrency control strategy" to update the
        /// appropriate properties before the object is persisted to the 
        /// datasource.
        /// </summary>
        void UpdatePropertiesWithLatestConcurrencyInfoBeforePersisting();

        ///// <summary>
        ///// If your concurrency control strategy involves read locks, then 
        ///// this method must be implemented to release the read locks.
        ///// Read locks would typically be used where you prevent 
        ///// </summary>
        //void ReleaseReadLocks();

        /// <summary>
        /// If your concurrency control strategy involves write locks, then 
        /// this method must be implemented to release the write locks.
        /// </summary>
        void ReleaseWriteLocks();

        ///<summary>
        /// Makes any changes required to the concurrency control mechanism 
        /// to assert that the transaction has failed and thus been rolled back.
        ///</summary>
        void UpdateAsTransactionRolledBack();
    }
}