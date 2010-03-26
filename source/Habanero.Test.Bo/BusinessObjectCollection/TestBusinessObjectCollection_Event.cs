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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    public abstract class TestBusinessObjectCollection_Event<TBOEventArgs>
        where TBOEventArgs : BOEventArgs
    {
        private DataAccessorInMemory _dataAccessor;
        private DataStoreInMemory _dataStore;

        #region Setup

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            _dataStore = new DataStoreInMemory();
            _dataAccessor = new DataAccessorInMemory(_dataStore);
            BORegistry.DataAccessor = _dataAccessor;
            ContactPersonTestBO.LoadDefaultClassDef();
        }

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //            ClassDef.ClassDefs.Clear();
            _dataStore.ClearAllBusinessObjects();
            TestUtil.WaitForGC();
        }

        #endregion

        protected abstract void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo);

        protected abstract void RegisterForEvent(EventHandler<TBOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col);

        protected abstract void DeregisterForEvent(EventHandler<TBOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col);
        
        [Test]
        public void Test_BusinessObjectAdded_Register_ShouldRegister()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            EventHandler<TBOEventArgs> eventHandler =
                MockRepository.GenerateStub<EventHandler<TBOEventArgs>>();
            //---------------Assert Precondition----------------
            AssertBOEventHandlerNotRegistered(eventHandler, col, CauseEventToFire);
            //---------------Execute Test ----------------------
            RegisterForEvent(eventHandler, col);
            //---------------Test Result -----------------------
            AssertBOEventHandlerRegistered(eventHandler, col, CauseEventToFire);
        }

        [Test]
        public void Test_BusinessObjectAdded_Deregister_WhenRegistered_ShouldDeregister()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            EventHandler<TBOEventArgs> eventHandler =
                MockRepository.GenerateStub<EventHandler<TBOEventArgs>>();
            RegisterForEvent(eventHandler, col);
            //---------------Assert Precondition----------------
            AssertBOEventHandlerRegistered(eventHandler, col, CauseEventToFire);
            //---------------Execute Test ----------------------
            DeregisterForEvent(eventHandler, col);
            //---------------Test Result -----------------------
            AssertBOEventHandlerNotRegistered(eventHandler, col, CauseEventToFire);
        }

        [Test]
        public void Test_BusinessObjectAdded_Deregister_WhenNotRegistered_ShouldLeaveUnregistered()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            EventHandler<TBOEventArgs> eventHandler =
                MockRepository.GenerateStub<EventHandler<TBOEventArgs>>();
            //---------------Assert Precondition----------------
            AssertBOEventHandlerNotRegistered(eventHandler, col, CauseEventToFire);
            //---------------Execute Test ----------------------
            DeregisterForEvent(eventHandler, col);
            //---------------Test Result -----------------------
            AssertBOEventHandlerNotRegistered(eventHandler, col, CauseEventToFire);
        }

        private static void AssertBOEventHandlerRegistered(
            EventHandler<TBOEventArgs> eventHandler,
            BusinessObjectCollection<ContactPersonTestBO> col, 
            Action<BusinessObjectCollection<ContactPersonTestBO>, ContactPersonTestBO> eventTrigger)
        {
            ContactPersonTestBO bo = new ContactPersonTestBO();
            eventTrigger(col, bo);
            AssertBOEventWasHandled(eventHandler, col, bo);
        }

        private static void AssertBOEventWasHandled(
            EventHandler<TBOEventArgs> eventHandler, 
            BusinessObjectCollection<ContactPersonTestBO> col, 
            ContactPersonTestBO bo)
        {
            eventHandler.AssertWasCalled(handler => handler(
                                                        Arg<object>.Is.Same(col),
                                                        Arg<TBOEventArgs>.Matches(predicate => predicate.BusinessObject == bo)));
        }

        private static void AssertBOEventHandlerNotRegistered(
            EventHandler<TBOEventArgs> eventHandler,
            BusinessObjectCollection<ContactPersonTestBO> col,
            Action<BusinessObjectCollection<ContactPersonTestBO>, ContactPersonTestBO> eventTrigger)
        {
            ContactPersonTestBO bo = new ContactPersonTestBO();
            eventTrigger(col, bo);
            AssertBOEventWasNotHandled(eventHandler, col, bo);
        }

        private static void AssertBOEventWasNotHandled(
            EventHandler<TBOEventArgs> eventHandler,
            BusinessObjectCollection<ContactPersonTestBO> col,
            ContactPersonTestBO bo)
        {
            eventHandler.AssertWasNotCalled(handler => handler(
                                                           Arg<object>.Is.Same(col),
                                                           Arg<TBOEventArgs>.Matches(predicate => predicate.BusinessObject == bo)));
        }


        //    private static void AssertBOEventHandlerRegistered<TBOType, TBOEventArgs>(EventHandler<TBOEventArgs> eventHandler, BusinessObjectCollection<TBOType> col, Action<BusinessObjectCollection<TBOType>, TBOType> eventTrigger)
        //        where TBOType : class, IBusinessObject, new()
        //        where TBOEventArgs : BOEventArgs
        //    {
        //        TBOType bo = new TBOType();
        //        eventTrigger(col, bo);
        //        AssertBOEventWasHandled(eventHandler, col, bo);
        //    }

        //    private static void AssertBOEventWasHandled<TBOType, TBOEventArgs>(EventHandler<TBOEventArgs> eventHandler, BusinessObjectCollection<TBOType> col, TBOType bo)
        //        where TBOType : class, IBusinessObject, new()
        //        where TBOEventArgs : BOEventArgs
        //    {
        //        eventHandler.AssertWasCalled(handler => handler(
        //                                                    Arg<object>.Is.Same(col),
        //                                                    Arg<TBOEventArgs>.Matches(predicate => predicate.BusinessObject == bo)));
        //    }

        //    private static void AssertBOEventHandlerNotRegistered<TBOType, TBOEventArgs>(EventHandler<TBOEventArgs> eventHandler, BusinessObjectCollection<TBOType> col, Action<BusinessObjectCollection<TBOType>, TBOType> eventTrigger)
        //        where TBOType : class, IBusinessObject, new()
        //        where TBOEventArgs : BOEventArgs
        //    {
        //        TBOType bo = new TBOType();
        //        eventTrigger(col, bo);
        //        AssertBOEventWasNotHandled(eventHandler, col, bo);
        //    }

        //    private static void AssertBOEventWasNotHandled<TBOType, TBOEventArgs>(EventHandler<TBOEventArgs> eventHandler, BusinessObjectCollection<TBOType> col, TBOType bo)
        //        where TBOType : class, IBusinessObject, new()
        //        where TBOEventArgs : BOEventArgs
        //    {
        //        eventHandler.AssertWasNotCalled(handler => handler(
        //                                                       Arg<object>.Is.Same(col),
        //                                                       Arg<TBOEventArgs>.Matches(predicate => predicate.BusinessObject == bo)));
        //    }
    }

}