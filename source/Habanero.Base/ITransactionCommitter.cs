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
using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// This base class manages and commits a collection of ITransactions to a datasource. 
    /// The sub classes of this class implement a specific strategy e.g. Committing to a 
    /// database, file, message queue, Webservice etc.
    /// This provides an Implementation of the 'Unit of Work' Pattern 
    /// (Fowler - Patterns of Enterprise Application Architecture 184 
    ///   ‘Maintains a list of objects affected by a business transaction and co-ordinates 
    ///   the writing out of changes and the detection and resolution of concurrency problems’).
    /// <br/>
    /// 
    /// The TransactionCommitter also implements the GOF Strategy Pattern and as such 
    ///   the transaction committer can be implemented with a concrete class or the ITransactionCommitter
    ///   can be implemented by the Application developer to provide any functionality required for the 
    ///   updating of business objects to a datastore.
    /// <br/>
    /// The TransactionCommitter works with the TransactionalBusinessObject. 
    /// <br/>
    /// The TransactionalBusinessObject implements the GOF adaptor pattern. 
    ///   As well as the Fowler - DataMapper 165 pattern. As such it
    ///   wraps the business object and uses the Class definitions (MetaData Mapping Fowler 306) to map the
    ///   business object to the Datastore. It also provides methods to call through to underlying Business
    ///   object methods.
    /// <br/>
    /// The TransactionCommitter and TransactionalBusinessObject also work together to ensure that 
    ///   all concurrency control <see cref="IConcurrencyControl"/> strategies for the business object have been 
    ///   implemented.
    /// <br/>
    /// The Application developer can also add Transactions to the TransactionCommitter that are not Business objects
    ///   these objects must implement the <see cref="ITransactional"/> interface. This is typically used 
    ///   when the application developer needs to insert or updated a datasource that is not wrapped by a business object.
    ///   E.g. The application developer may implement a NumberGenerator to generate a code e.g. Product code.
    ///   The Habanero Framework uses this capability to write out TransactionLogTable.
    /// <br/>
    /// When <see cref="CommitTransaction"/> is called all the objects in the TransactionCommitter are executed to the
    ///   datasource in the case of the TransactionCommitterDB these are executed within an individual 
    ///   transaction if the transaction fails then all updates to the database are rolled back.
    /// <br/>
    /// In cases where a single object is edited and persisted the Transaction committer does not have to be 
    ///   used by the Application developer. The architecture uses a convenience method
    ///   <see cref="IBusinessObject"/>  <see cref="IBusinessObject.Save"/> this 
    ///   creates the appropriate transactionCommitter and commits it.
    /// <br/>
    /// The TransactionCommitter is very simple to use the Application developer can add the required objects to
    ///   Transaction Committer. When the business transaction is complete the <see cref="CommitTransaction"/> is called.
    /// <br/>
    /// <example>
    ///        ContactPerson contactP = New ContactPerson();
    ///        //set relevant data for contact person.
    ///        committerDB.AddBusinessObject(contactP);
    ///        committerDB.CommitTransaction();
    /// </example>
    /// </summary>
    public interface ITransactionCommitter
    {
        ///<summary>
        /// Add an object of type business object to the transaction.
        /// The DBTransactionCommiter wraps this Business Object in the
        /// appropriate Transactional Business Object
        ///</summary>
        ///<param name="businessObject"></param>
        void AddBusinessObject(IBusinessObject businessObject);

        ///<summary>
        /// This method adds an <see cref="ITransactional"/> to the list of transactions.
        ///</summary>
        ///<param name="transaction"></param>
        void AddTransaction(ITransactional transaction);

        ///<summary>
        /// Commit the transactions to the datasource e.g. the database, file, memory DB
        ///</summary>
        ///<returns></returns>
        List<Guid> CommitTransaction();
    }
}