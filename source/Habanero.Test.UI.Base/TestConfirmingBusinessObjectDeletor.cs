using System;
using Habanero.Base;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

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
        public void Test_DeleteBusinessObject_ConfirmedIsTrue()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            Expect.Call(confirmer.Confirm(null, null))
                .Constraints(Is.Anything(),Is.NotNull())
                .Do((Delegates.Function<bool, string, ConfirmationDelegate>)delegate(string message, ConfirmationDelegate confirmationDelegate)
                {
                    confirmationDelegate(true);
                    return true;
                });

            IBusinessObject boToDelete = mockRepository.StrictMock<IBusinessObject>();
            Expect.Call(boToDelete.MarkForDelete).Repeat.Once();
            Expect.Call(boToDelete.Save).Repeat.Once();

            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }
        
        public class ConfirmingBusinessObjectDeletor : IBusinessObjectDeletor
        {
            public IConfirmer Confirmer { get; private set; }

            public ConfirmingBusinessObjectDeletor(IConfirmer confirmer)
            {
                Confirmer = confirmer;
            }

            ///<summary>
            /// Deletes the given business object
            ///</summary>
            ///<param name="businessObject">The business object to delete</param>
            public void DeleteBusinessObject(IBusinessObject businessObject)
            {
                Confirmer.Confirm(null, delegate { });
                businessObject.MarkForDelete();
                businessObject.Save();
            }
        }
    }
}