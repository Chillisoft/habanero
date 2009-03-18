using System;
using Habanero.Base;
using Habanero.UI.Base;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestBusinessObjectDeletor
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
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DefaultBODeletor businessObjectDeletor = new DefaultBODeletor();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IBusinessObjectDeletor), businessObjectDeletor);
        }

        [Test]
        public void Test_DeleteBusinessObject_Success()
        {
            //---------------Set up test pack-------------------
            IBusinessObject boToDelete = MockRepository.GenerateMock<IBusinessObject>();
            DefaultBODeletor businessObjectDeletor = new DefaultBODeletor();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            businessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            boToDelete.AssertWasCalled(o => o.MarkForDelete());
            boToDelete.AssertWasCalled(o => o.Save());
            boToDelete.AssertWasNotCalled(o => o.CancelEdits());
        }

        [Test]
        public void Test_DeleteBusinessObject_Failure()
        {
            //---------------Set up test pack-------------------
            DefaultBODeletor businessObjectDeletor = new DefaultBODeletor();
            IBusinessObject boToDelete = MockRepository.GenerateMock<IBusinessObject>();
            Exception expectedException = new Exception();
            boToDelete.Stub(t => t.Save()).Throw(expectedException);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Exception exception = null;
            try 
            {
                businessObjectDeletor.DeleteBusinessObject(boToDelete);
            } 
            catch (Exception ex) { exception = ex; }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Expected to throw an Exception");
            Assert.AreSame(expectedException, exception);
            boToDelete.AssertWasCalled(o => o.MarkForDelete());
            boToDelete.AssertWasCalled(o => o.Save());
            boToDelete.AssertWasCalled(o => o.CancelEdits());
        }
    }
}
