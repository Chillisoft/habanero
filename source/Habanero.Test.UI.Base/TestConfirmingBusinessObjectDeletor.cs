using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is=Rhino.Mocks.Constraints.Is;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestConfirmingBusinessObjectDeletor
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IBusinessObjectDeletor), confirmingBusinessObjectDeletor);
            Assert.AreSame(confirmer, confirmingBusinessObjectDeletor.Confirmer);
        }

        [Test]
        public void Test_Construct_WithCustomConfirmationMessageDelegate()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            Habanero.UI.Base.Function<IBusinessObject, string> customConfirmationMessageDelegate = t => "aaa";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer, customConfirmationMessageDelegate);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IBusinessObjectDeletor), confirmingBusinessObjectDeletor);
            Assert.AreSame(confirmer, confirmingBusinessObjectDeletor.Confirmer);
            Assert.AreSame(customConfirmationMessageDelegate, confirmingBusinessObjectDeletor.CustomConfirmationMessageDelegate);
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmationMessage()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            string boToString = TestUtil.GetRandomString();
            string expectedMessage = string.Format("Are you certain you want to delete the object '{0}'", boToString);
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, 
                Is.Equal(expectedMessage), false);
            IBusinessObject boToDelete = new MockBOWithToString(boToString);
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_DeleteBusinessObject_CustomConfirmationMessage()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            string expectedMessage = TestUtil.GetRandomString();
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, 
                Is.Equal(expectedMessage), false);
            IBusinessObject boToDelete = mockRepository.StrictMock<IBusinessObject>();
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = 
                new ConfirmingBusinessObjectDeletor(confirmer, t => expectedMessage);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmedIsTrue()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, true);
            IBusinessObject boToDelete = CreateMockBOWithDeleteExpectation(mockRepository);
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmedIsFalse()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, false);
            IBusinessObject boToDelete = mockRepository.StrictMock<IBusinessObject>();
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmationDelegateUsed_ConfirmedIsTrue()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, true);
            IBusinessObject boToDelete = CreateMockBOWithDeleteExpectation(mockRepository);
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmationDelegateUsed_ConfirmedIsFalse()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, false);
            IBusinessObject boToDelete = mockRepository.StrictMock<IBusinessObject>();
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        private static IBusinessObject CreateMockBOWithDeleteExpectation(MockRepository mockRepository)
        {
            IBusinessObject boToDelete = mockRepository.StrictMock<IBusinessObject>();
            Expect.Call(boToDelete.MarkForDelete).Repeat.Once();
            Expect.Call(boToDelete.Save).Repeat.Once();
            return boToDelete;
        }

        private static IConfirmer CreateMockConfirmerWithExpectation(MockRepository mockRepository, bool confirmReturnValue)
        {
            return CreateMockConfirmerWithExpectation(mockRepository, Is.Anything(), confirmReturnValue);
        }

        private static IConfirmer CreateMockConfirmerWithExpectation(MockRepository mockRepository, AbstractConstraint messageConstraint, bool confirmReturnValue)
        {
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            Expect.Call(()=>confirmer.Confirm(null, null))
                .Constraints(messageConstraint,Is.NotNull())
                .Do((Delegates.Action<string, ConfirmationDelegate>)(
                (message, confirmationDelegate) => confirmationDelegate(confirmReturnValue)));
            return confirmer;
        }

        public class MockBOWithToString : BusinessObject
        {
            private readonly string _boToString;

            public MockBOWithToString(string boToString)
                : base(Circle.CreateClassDef())
            {
                _boToString = boToString;
            }

            public override string ToString()
            {
                return _boToString;
            }
        }
    }
}