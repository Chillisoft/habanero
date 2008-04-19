using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTransactionCommitter : TestUsingDatabase
    {
        private static readonly string _customRuleErrorMessage = "Broken Rule";

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            CleanStubDatabaseTransactionTable();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        
        }
        [Test]
        public void TestAddTransactionsToATransactionCommiter()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();

            //---------------Execute Test ----------------------
            committerDB.AddTransaction(new StubSuccessfullTransaction());
            committerDB.AddTransaction(new StubSuccessfullTransaction());

            //---------------Test Result -----------------------
            Assert.AreEqual(2, committerDB.OriginalTransactions.Count);
        }
        [Test]
        public void TestCommitAddedTransactions()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStub committerDB = new TransactionCommitterStub();
            StubSuccessfullTransaction transactional1 = new StubSuccessfullTransaction();
            committerDB.AddTransaction(transactional1);
            StubSuccessfullTransaction transactional2 = new StubSuccessfullTransaction();
            committerDB.AddTransaction(transactional2);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsTrue(transactional1.Committed);
            Assert.IsTrue(transactional2.Committed);
        }



        [Test,ExpectedException(typeof (NotImplementedException))]
        public void TestRaisesException_onError()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStubDB committerDB = new TransactionCommitterStubDB();
            StubFailingTransaction trn = new StubFailingTransaction();
            committerDB.AddTransaction(trn);
            committerDB.AddTransaction(new StubSuccessfullTransaction());
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestRaisesException_onError_DoesNotCommit()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStub committerDB = new TransactionCommitterStub();
            StubSuccessfullTransaction transactional1 = new StubSuccessfullTransaction();
            committerDB.AddTransaction(transactional1);
            StubFailingTransaction transactional2 = new StubFailingTransaction();
            committerDB.AddTransaction(transactional2);
            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            }
            //---------------Test Result -----------------------
            catch(NotImplementedException )
            {
                Assert.IsFalse(transactional1.Committed);
                Assert.IsFalse(transactional2.Committed);               
            }
        }

        [Test]
        public void TestDatabaseTransaction_Success()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            committerDB.AddTransaction(transactional1);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.IsTrue(transactional1.Committed);
            AssertTransactionsInTableAre(1);
        }

        private static void AssertTransactionsInTableAre(int expected)
        {
            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection, "select * from stubdatabasetransaction");
            Assert.AreEqual(expected, DatabaseConnection.CurrentConnection.LoadDataTable(statement, "", "").Rows.Count);
        }

        private static void CleanStubDatabaseTransactionTable()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from stubdatabasetransaction");
        }

        public void TestDatabaseTransaction_SuccessTwoTransactions()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            StubDatabaseTransaction transactional2 = new StubDatabaseTransaction();
            committerDB.AddTransaction(transactional1);
            committerDB.AddTransaction(transactional2);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertTransactionsInTableAre(2);
        }

        [Test]
        public void TestDatabaseTransaction_Failure()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            StubDatabaseFailureTransaction transactional2 = new StubDatabaseFailureTransaction();
            committerDB.AddTransaction(transactional1);
            committerDB.AddTransaction(transactional2);

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            } catch (NotImplementedException)
            {
                
            }
            //---------------Test Result -----------------------
            Assert.IsFalse(transactional1.Committed);
            Assert.IsFalse(transactional2.Committed);
            AssertTransactionsInTableAre(0);
        }

        [Test]
        public void TestMultipleStatementsInOneITransactional()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransactionMultiple transactional1 = new StubDatabaseTransactionMultiple();
            committerDB.AddTransaction(transactional1);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertTransactionsInTableAre(2);
        }

        [Test]
        public void TestPersistSimpleBO_Insert()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            
            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            MockBO savedMockBO =
                BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBo.MockBOID.ToString("B") + "'");
            Assert.AreSame(mockBo, savedMockBO);
        }

        [Test]
        public void TestPersistSimpleBO_Update()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test]
        public void TestPersistSimpleBO_UpdateNonDirty()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            Guid? mockBOProp1 = mockBo.MockBOProp1;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }
        [Test]
        public void TestPersistSimpleBO_Delete()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            mockBo.Delete();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(mockBo);
            AssertMockBONotInDatabase(mockBo.MockBOID);
        }

        [Test]
        public void TestPersistSimpleBO_NewDelete()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            mockBo.Delete();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(mockBo);
            AssertMockBONotInDatabase(mockBo.MockBOID);
        }

        [Test,ExpectedException(typeof(BusObjectInAnInvalidStateException))]
        public void TestPersistSimpleBO_Insert_InvalidData()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            //AssertBOStateIsValidAfterInsert_Updated(contactPersonTestBO);
            //BOLoader.Instance.Refresh(contactPersonTestBO);
            //MockBO savedMockBO =
            //    BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + contactPersonTestBO.MockBOID.ToString("B") + "'");
            //Assert.AreSame(contactPersonTestBO, savedMockBO);
        }
        [Test]
        public void TestPersistSimpleBO_Delete_InvalidData()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            contactPersonTestBO.Surname = null;
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO));

            //---------------Execute Test ----------------------
            contactPersonTestBO.Delete();
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
        }

        [Test]
        public void TestPersistSimpleBO_FailingCustomRules()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MockBOWithCustomRule mockBO = new MockBOWithCustomRule();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBO));

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            }
            catch (BusObjectInAnInvalidStateException ex)
            //---------------Test Result -----------------------
            {
                Assert.IsTrue(ex.Message.Contains(_customRuleErrorMessage));
            }
        }
        [Test]
        public void TestMessageTwoPersistSimpleBO_Failing()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MockBOWithCustomRule mockBO = new MockBOWithCustomRule();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBO));

            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            contactPersonTestBO.Surname = null;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO));

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            }
            catch (BusObjectInAnInvalidStateException ex)
            //---------------Test Result -----------------------
            {
                Assert.IsTrue(ex.Message.Contains(_customRuleErrorMessage));
                Assert.IsTrue(ex.Message.Contains("Surname"));

            }
        }

        [Test]
        public void TestPersistSimpleBO_Update_NotUsingTransactionalBusinessObject()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(mockBo);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }


        [Test, ExpectedException(typeof(BusinessObjectReferentialIntegrityException))]
        public void TestPreventDelete()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------

        }
        [Test]
        public void TestNotPreventDelete_ForNonDeleted()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            contactPersonTestBO.FirstName = Guid.NewGuid().ToString();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.State.IsDirty);
        }

        [Test]
        public void TestDeleteRelated()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDatabase(contactPersonTestBO);
            AssertBusinessObjectNotInDatabase(address);
        }

        [Test]
        public void TestDeleteRelatedWithFailureAfter()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            committerDB.AddTransaction(new StubDatabaseFailureTransaction());
            
            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
                Assert.Fail();
            }
            catch (NotImplementedException)
            {
            }
            //---------------Test Result -----------------------
            AssertBusinessObjectInDatabase(contactPersonTestBO);
            AssertBusinessObjectInDatabase(address);

            Assert.IsTrue(address.State.IsDeleted);
            Assert.IsTrue(contactPersonTestBO.State.IsDeleted);
            Assert.IsFalse(address.State.IsNew);
            Assert.IsFalse(contactPersonTestBO.State.IsNew);
        }



        private void AssertBusinessObjectNotInDatabase(BusinessObject bo)
        {
            BusinessObject missingBO =
                BOLoader.Instance.GetBusinessObject(bo.GetType(),bo.ID.ToString());
            Assert.IsNull(missingBO);
        }

        private void AssertBusinessObjectInDatabase(BusinessObject bo)
        {
            BusinessObject loadedBO =
                BOLoader.Instance.GetBusinessObject(bo.GetType(), bo.ID.ToString());
            Assert.IsNotNull(loadedBO);
        }

        #region HelperMethods

        private static MockBO CreateSavedMockBO()
        {
            MockBO mockBo = new MockBO();
            mockBo.Save();
            return mockBo;
        }

        private static void AssertMockBONotInDatabase(Guid mockBOID)
        {
            MockBO savedMockBO =
                BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBOID.ToString("B") + "'");
            Assert.IsNull(savedMockBO);
        }

        private static void AssertBOStateIsValidAfterDelete(BusinessObject mockBo)
        {
            Assert.IsTrue(mockBo.State.IsDeleted);
            Assert.IsTrue(mockBo.State.IsNew);
        }

        private static void AssertBOStateIsValidAfterInsert_Updated(BusinessObject mockBo)
        {
            Assert.IsFalse(mockBo.State.IsNew);
            Assert.IsFalse(mockBo.State.IsDirty);
            Assert.IsFalse(mockBo.State.IsDeleted);
            Assert.IsFalse(mockBo.State.IsEditing);
            Assert.IsTrue(mockBo.State.IsValid);
        }


        internal class MockBOWithCustomRule : MockBO
        {
            /// <summary>
            /// Override this method in subclasses of BusinessObject to check custom rules for that
            /// class.  The default implementation returns true and sets customRuleErrors to the empty string.
            /// </summary>
            /// <param name="customRuleErrors">The error string to display</param>
            /// <returns>true if no custom rule errors are encountered.</returns>
            protected override bool CheckCustomRules(out string customRuleErrors)
            {
                customRuleErrors = _customRuleErrorMessage;
                return false;
            }
        }
        #endregion
    }


    #region StubObjectsForTesting

    internal class StubDatabaseTransactionMultiple : TransactionalBusinessObjectDB
    {
        public StubDatabaseTransactionMultiple() : base(null)
        {
        }

        ///<summary>
        /// Execute
        ///</summary>
        protected internal override ISqlStatementCollection GetSql()
        {
            ISqlStatementCollection col = new SqlStatementCollection();
            col.Add(new SqlStatement(DatabaseConnection.CurrentConnection, "insert into stubdatabasetransaction values('1', 'test')"));
            col.Add(new SqlStatement(DatabaseConnection.CurrentConnection, "insert into stubdatabasetransaction values('2', 'test')"));
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

    internal class StubDatabaseFailureTransaction: TransactionalBusinessObjectDB
    {
        private bool _committed;

        internal StubDatabaseFailureTransaction() : base(null)
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

    internal class StubDatabaseTransaction : TransactionalBusinessObjectDB
    {
        private bool _committed;

        internal StubDatabaseTransaction()
            : base(null)
        {
        }
        ///<summary>
        /// Execute
        ///</summary>
        protected internal override ISqlStatementCollection GetSql()
        {
            return new SqlStatementCollection(
                new SqlStatement(DatabaseConnection.CurrentConnection, "insert into stubdatabasetransaction values('1', 'test')"));
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

        internal StubFailingTransaction() : base(null)
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
        public  void UpdateStateAsCommitted()
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
            TransactionalBusinessObjectDB transactionDB = (TransactionalBusinessObjectDB) transaction;
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
    public class TransactionCommitterStub:TransactionCommitter
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

    #endregion

}
