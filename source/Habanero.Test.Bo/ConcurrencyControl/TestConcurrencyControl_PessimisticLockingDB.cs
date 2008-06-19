using System;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.ConcurrencyControl;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestConcurrencyControl_PessimisticLockingDB : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            //Runs every time that any testmethod is executed
            //base.SetupTest();
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
            //DeleteObjects();
        }
        [Test]
        public void Test_Locking_InCheckConcurrencyControlBeforeBeginEditing()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            //---------------Execute Test ----------------------
            //execute CheckConcurrencyControl Begin Edit.
            IConcurrencyControl concurrCntrl = cp.concurrencyControl();
            concurrCntrl.CheckConcurrencyBeforeBeginEditing();
            //---------------Test Result -----------------------
            //Test that locked.
            AssertIsLocked(cp);

            BOLoader.Instance.Refresh(cp);//load from DB
            AssertIsLocked(cp);
            Assert.AreEqual(GetUserName(), cp.UserLocked);
            Assert.AreEqual(GetOperatingSystemUser(), cp.OperatingSystemUser);
            Assert.AreEqual(GetMachineName(), cp.MachineLocked);
            Assert.GreaterOrEqual( cp.DateTimeLocked, DateTime.Now.AddMinutes(-1));
            Assert.LessOrEqual(cp.DateTimeLocked, DateTime.Now);
        }

        [Test]
        public void Test_ThrowErrorIfCheckConcurrencyBeforeEditingTwice()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            //---------------Execute Test ----------------------
            IConcurrencyControl concurrCntrl = cp.concurrencyControl();
            concurrCntrl.CheckConcurrencyBeforeBeginEditing();
            try
            {
                concurrCntrl.CheckConcurrencyBeforeBeginEditing();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
            catch(BusObjPessimisticConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot begin edits on the 'ContactPersonPessimisticLockingDB', as another user has started edits and therefore locked to this record."));
            }
        }

        [Test]
        public void Test_ThrowErrorIfObjectDeletedPriorToBeginEdits()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            //---------------Execute Test ----------------------
            ContactPerson.DeleteAllContactPeople();
            try
            {
                IConcurrencyControl concurrCntrl = cp.concurrencyControl();
                concurrCntrl.CheckConcurrencyBeforeBeginEditing();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot save the changes to 'ContactPersonPessimisticLockingDB', as another user has deleted the record"));
            }
        }

        [Test]
        public void Test_ThrowErrorIfSecondInstanceOfContactPersonBeginEdit()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            BOLoader.Instance.ClearLoadedBusinessObjects();
            ContactPersonPessimisticLockingDB cp2 = BOLoader.Instance.GetBusinessObjectByID<ContactPersonPessimisticLockingDB>(cp.ID);
            //---------------Execute Test ----------------------
            string surname = cp.Surname;
            cp.Surname = Guid.NewGuid().ToString();
            try
            {
                cp2.Surname = Guid.NewGuid().ToString();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
            catch (BusObjPessimisticConcurrencyControlException ex)
            {
                Assert.AreEqual(surname, cp2.Surname);
                Assert.IsTrue(ex.Message.Contains("You cannot begin edits on the 'ContactPersonPessimisticLockingDB', as another user has started edits and therefore locked to this record."));
            }
        }

        [Test]
        public void Test_SurnameNotUpdatedToDBWhenUpdatingLockingProps()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            string surname = cp.Surname;
            //---------------Execute Test ----------------------
           
            cp.Surname = Guid.NewGuid().ToString();
            BOLoader.Instance.Refresh(cp);
            Assert.AreEqual(surname, cp.Surname);
            
        }
        [Test]
        public void Test_UnLocking_WhenReleaseWriteLocksIsCalled()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            IConcurrencyControl concurrCntrl = cp.concurrencyControl();
            //Create Lock
            concurrCntrl.CheckConcurrencyBeforeBeginEditing();
            //---------------Execute Test ----------------------

            concurrCntrl.ReleaseWriteLocks();

            //---------------Test Result -----------------------
            //Test that locked.
            AssertIsNotLocked(cp);
        }

        [Test]
        public void Test_UnLocking_WhenCancelEditsCalled()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            IConcurrencyControl concurrCntrl = cp.concurrencyControl();
            //Create Lock
            concurrCntrl.CheckConcurrencyBeforeBeginEditing();
            //---------------Execute Test ----------------------

            cp.Restore();

            //---------------Test Result -----------------------
            //Test that locked.
            AssertIsNotLocked(cp);
        }
        [Test]
        public void Test_NotLockedIfLockDurationExceeded()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            IConcurrencyControl concurrCntrl = cp.concurrencyControl();
            //Create Lock
            concurrCntrl.CheckConcurrencyBeforeBeginEditing();
            int lockDuration = 15;
            UpdateDatabaseLockAsExpired(lockDuration);
            //---------------Execute Test ----------------------

            concurrCntrl.CheckConcurrencyBeforeBeginEditing();

            //---------------Test Result -----------------------
            //Should not raise an error since the lock duration has been exceeded.
        }

        [Test]
        public void Test_EditContactPersonTwiceDoesNotCauseProblems()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            //---------------Execute Test ----------------------

            cp.Surname = Guid.NewGuid().ToString();
            cp.Surname = Guid.NewGuid().ToString();
            //---------------Test Result -----------------------
            //Should not raise an error since the lock duration has been exceeded.
        }

        [Test]
        public void Test_WhenContactPersonsavedReleaseLocks()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            //---------------Execute Test ----------------------

            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            //---------------Test Result -----------------------
            AssertIsNotLocked(cp);
            BOLoader.Instance.Refresh(cp);//load from DB
            AssertIsNotLocked(cp);
        }
        [Test]
        public void Test_MultipleSavesNoProblem()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            //---------------Execute Test ----------------------

            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            //---------------Test Result -----------------------

        }

        [Test, Ignore("Causes issues when run through Resharper runner")]
        public void Test_WhenCleansUpObjectClearsItsLock()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            IPrimaryKey id = cp.ID;
            //---------------Execute Test ----------------------

            cp.Surname = Guid.NewGuid().ToString();
#pragma warning disable RedundantAssignment
            cp = null;//so that garbage collector can work
#pragma warning restore RedundantAssignment
            GC.Collect(); //Force the GC to collect
            WaitForDB();
            //---------------Test Result -----------------------
            BOLoader.Instance.ClearLoadedBusinessObjects();
            ContactPersonPessimisticLockingDB cp2 =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonPessimisticLockingDB>(id);
            AssertIsNotLocked(cp2);
        }

        [Test]
        public void Test_IfThisThreadLocksAndTimesOutBeforePersistingThenThrowErrorWhenPersisting()
        {
            //---------------Set up test pack-------------------
            ContactPersonPessimisticLockingDB cp = CreateSavedContactPersonPessimisticLocking();
            cp.Surname = Guid.NewGuid().ToString();
            IBOProp propDateTimeLocked = cp.Props["DateTimeLocked"];
            int lockDuration = 15;
            //---------------Execute Test ----------------------
            propDateTimeLocked.Value = DateTime.Now.AddMinutes(-1*lockDuration - 1);
            UpdateDatabaseLockAsExpired(lockDuration);
            try
            {
                cp.Save();
                Assert.Fail();
            }
            //---------------Test Result -----------------------
            catch (BusObjPessimisticConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The lock on the business object ContactPersonPessimisticLockingDB has a duration of 15 minutes and has been exceeded for the object"));
            }
        }

        private static void UpdateDatabaseLockAsExpired(int lockDuration)
        {
            SqlStatement sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection);
            sqlStatement.Statement.Append("UPDATE `contact_person` SET ");
            sqlStatement.Statement.Append(SqlFormattingHelper.FormatFieldName("DateTimeLocked", DatabaseConnection.CurrentConnection));
            sqlStatement.Statement.Append(" = ");
            sqlStatement.AddParameterToStatement(DateTime.Now.AddMinutes(-1 * lockDuration -1));
            DatabaseConnection.CurrentConnection.ExecuteSql(sqlStatement);
        }

        private static void AssertIsLocked(ContactPersonPessimisticLockingDB cp)
        {
            Assert.IsTrue(Convert.ToBoolean(cp.BoPropLocked.Value));
        }

        private static void AssertIsNotLocked(ContactPersonPessimisticLockingDB cp)
        {
            Assert.IsFalse(Convert.ToBoolean(cp.BoPropLocked.Value));
        }
        private static ContactPersonPessimisticLockingDB CreateSavedContactPersonPessimisticLocking()
        {
            ContactPersonPessimisticLockingDB.LoadDefaultClassDef();
            ContactPersonPessimisticLockingDB cp = new ContactPersonPessimisticLockingDB();
            cp.Surname = Guid.NewGuid().ToString();
            TransactionCommitter tc = new TransactionCommitterDB();
            tc.AddBusinessObject(cp);
            tc.CommitTransaction();
            return cp;
        }

        private static string GetOperatingSystemUser()
        {
            try
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
            }
            return "";
        }

        private static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch (InvalidOperationException)
            {
            }
            return "";
        }

        private static string GetUserName()
        {
            try
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
            }
            return "";
        }
    }

    internal class ContactPersonPessimisticLockingDB : BusinessObject
    {
        private readonly IBOProp _boPropLocked;

        public ContactPersonPessimisticLockingDB()
        {
            IBOProp propDateLocked = _boPropCol["DateTimeLocked"];
            IBOProp propUserLocked = _boPropCol["UserLocked"];
            IBOProp propMachineLocked = _boPropCol["MachineLocked"];
            IBOProp propOperatingSystemUserLocked = _boPropCol["OperatingSystemUserLocked"];
            _boPropLocked = _boPropCol["Locked"];

            SetConcurrencyControl(new PessimisticLockingDB(this, 15, propDateLocked,
                                                           propUserLocked, propMachineLocked,
                                                           propOperatingSystemUserLocked, _boPropLocked));

        }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonPessimisticLockingDB"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
					<property  name=""DateTimeLocked"" type=""DateTime"" />
					<property  name=""UserLocked"" />
					<property  name=""Locked"" type=""Boolean""/>
					<property  name=""MachineLocked"" />
					<property  name=""OperatingSystemUserLocked"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public Guid ContactPersonID
        {
            get { return (Guid)GetPropertyValue("ContactPersonID"); }
            set { this.SetPropertyValue("ContactPersonID", value); }
        }

        public string Surname
        {
            get { return (string)GetPropertyValue("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }

        public override string ToString()
        {
            return Surname;
        }

        public IBOProp BoPropLocked
        {
            get { return _boPropLocked; }
        }

        public string UserLocked
        {
            get { return (string)GetPropertyValue("UserLocked"); }
            set { SetPropertyValue("UserLocked", value); }
        }

        public string OperatingSystemUser
        {
            get { return (string)GetPropertyValue("OperatingSystemUserLocked"); }
            set { SetPropertyValue("OperatingSystemUserLocked", value); }
        }

        public string MachineLocked
        {
            get { return (string)GetPropertyValue("MachineLocked"); }
            set { SetPropertyValue("MachineLocked", value); }
        }
        public DateTime? DateTimeLocked
        {
            get { return (DateTime?)GetPropertyValue("DateTimeLocked"); }
            set { SetPropertyValue("DateTimeLocked", value); }
        }
        public IConcurrencyControl concurrencyControl()
        {
            return _concurrencyControl;
        }
    }
}