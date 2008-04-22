using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;

namespace Habanero.Test.BO
{
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
        protected internal override ISqlStatementCollection GetSql()
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
        public override bool IsValid(out string invalidReason)
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
        public override bool IsDeleted
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
        protected internal override ISqlStatementCollection GetSql()
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
        public override bool IsDeleted
        {
            get { return false; }
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        public override bool IsValid(out string invalidReason)
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
        protected internal override ISqlStatementCollection GetSql()
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
        public override bool IsDeleted
        {
            get { return false; }
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        public override bool IsValid(out string invalidReason)
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

        protected internal override ISqlStatementCollection GetSql()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        public override bool IsDeleted
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
        public override bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }

        public bool Committed
        {
            get { return _committed; }
        }
    }

    internal class StubSuccessfullTransaction : ITransactionalBusinessObject
    {
        private bool _committed;

        public StubSuccessfullTransaction()
        {
            _committed = false;
        }

        public ISqlStatementCollection GetSql()
        {
            return null;
        }

        ///<summary>
        /// Returns the business object that this objects decorates.
        ///</summary>
        public BusinessObject BusinessObject
        {
            get { return null; }
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        public bool IsDeleted
        {
            get { return false; }
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        public bool IsNew()
        {
            return true;
        }

        ///<summary>
        ///</summary>
        public void UpdateStateAsCommitted()
        {
            _committed = true;
        }

        ///<summary>
        ///</summary>
        ///<param name="invalidReason"></param>
        ///<returns></returns>
        public bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }

        /// <summary>
        /// Indicates whether there is a duplicate of this object in the data store
        /// eg. for a database this will select from the table to find an object
        /// that matches this object's primary key. In this case this object would be
        /// a duplicate.
        /// </summary>
        /// <param name="errMsg">The description of the duplicate</param>
        /// <returns>Whether a duplicate of this object exists in the data store (based on the ID/primary key)</returns>
        public bool HasDuplicateIdentifier(out string errMsg)
        {
            errMsg = "";
            return false;
        }

        ///<summary>
        ///</summary>
        ///<param name="transactionCommitter"></param>
        public void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter)
        {
            
        }

        public bool Committed
        {
            get { return _committed; }
        }
    }


    internal class TransactionCommitterStubDB : TransactionCommitterDB
    {
        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
        }

        protected override void ExecuteTransactionToDataSource(ITransactionalBusinessObject transaction)
        {
            TransactionalBusinessObjectDB transactionDB = (TransactionalBusinessObjectDB)transaction;
            transactionDB.GetSql();
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override void CommitToDatasource()
        {
        }

        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="TransactionCommitter.ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="TransactionCommitter.CommitToDatasource"/>
        /// </summary>
        protected override void TryRollback()
        {
        }
    }

    public class TransactionCommitterStub : TransactionCommitter
    {
        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override void CommitToDatasource()
        {
        }

        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="TransactionCommitter.ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="TransactionCommitter.CommitToDatasource"/>
        /// </summary>
        protected override void TryRollback()
        {
        }
    }

}