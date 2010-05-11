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
using Habanero.Base;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;

namespace Habanero.Test.DB
{
    internal class TransactionCommitterStubDB : TransactionCommitterDB
    {
        public TransactionCommitterStubDB(IDatabaseConnection databaseConnection) : base(databaseConnection)
        {
        }

        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            TransactionalBusinessObjectDB transactionDB = (TransactionalBusinessObjectDB)transaction;
            transactionDB.GetPersistSql();
        }

        public List<ITransactional> GetOriginalTransactions() { return base.OriginalTransactions; }
    }



    internal class StubDatabaseFailureTransaction : TransactionalBusinessObjectDB
    {
        private bool _committed;

        internal StubDatabaseFailureTransaction()
            : base(new MockBO(), null)
        {
            _committed = false;
        }

        ///<summary>
        /// Execute
        ///</summary>
        public override ISqlStatementCollection GetPersistSql()
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///</summary>
        public override void UpdateStateAsCommitted()
        {
            _committed = true;
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        protected override bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }

        public bool Committed
        {
            get { return _committed; }
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected override bool IsDeleted
        {
            get { return false; }
        }
    }

    internal class StubDatabaseTransactionMultiple : TransactionalBusinessObjectDB
    {
        public StubDatabaseTransactionMultiple()
            : base(new MockBO(), null)
        {
        }

        ///<summary>
        /// Execute
        ///</summary>
        public override ISqlStatementCollection GetPersistSql()
        {
            ISqlStatementCollection col = new SqlStatementCollection();
            col.Add(
                new SqlStatement(DatabaseConnection.CurrentConnection,
                                 "insert into stubdatabasetransaction values('1', 'test')"));
            col.Add(
                new SqlStatement(DatabaseConnection.CurrentConnection,
                                 "insert into stubdatabasetransaction values('2', 'test')"));
            return col;
        }

        ///<summary>
        ///</summary>
        public override void UpdateStateAsCommitted()
        {
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected override bool IsDeleted
        {
            get { return false; }
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        protected override bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }
    }

    internal class StubDatabaseTransaction : TransactionalBusinessObjectDB
    {
        private bool _committed;

        internal StubDatabaseTransaction()
            : base(new MockBO(), null)
        {
        }

        ///<summary>
        /// Execute
        ///</summary>
        public override ISqlStatementCollection GetPersistSql()
        {
            return new SqlStatementCollection(
                new SqlStatement(DatabaseConnection.CurrentConnection,
                                 "insert into stubdatabasetransaction values('1', 'test')"));
        }

        ///<summary>
        ///</summary>
        public override void UpdateStateAsCommitted()
        {
            _committed = true;
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected override bool IsDeleted
        {
            get { return false; }
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        protected override bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }


        public bool Committed
        {
            get { return _committed; }
        }
    }

    internal class StubFailingTransaction : TransactionalBusinessObjectDB
    {
        private bool _committed;

        internal StubFailingTransaction()
            : base(new MockBO(), null)
        {
            _committed = false;
        }

        public override ISqlStatementCollection GetPersistSql()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected override bool IsDeleted
        {
            get { return false; }
        }

        ///<summary>
        ///</summary>
        public override void UpdateStateAsCommitted()
        {
            _committed = true;
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        protected override bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }

        public bool Committed
        {
            get { return _committed; }
        }
    }

}