using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;

namespace Habanero.Test.DB
{
    internal class TransactionCommitterStubDB : TransactionCommitterDB
    {


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
            : base(new MockBO())
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
            : base(new MockBO())
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
            : base(new MockBO())
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
            : base(new MockBO())
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