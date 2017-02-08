using Habanero.BO;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    public class BusinessObjectCollectionTestHelper
    {
        private bool _addedEventFired;
        private bool _removedEventFired;

        public static void AssertOneObjectInCurrentAndAddedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public static void AssertOneObjectInCurrentAndCreatedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        public static void AssertOneObjectInMarkForDeleteAndAddedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public static void AssertAllCollectionsHaveNoItems(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public static void AssertOneObjectInCurrentPersistedAndAddedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public static void AssertOneObjectInCurrentPersistedCollectionOnly
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public void AssertAddedAndRemovedEventsNotFired()
        {
            AssertAddedEventNotFired();
            AssertRemovedEventNotFired();
        }

        public void AssertRemovedEventNotFired()
        {
            Assert.IsFalse(_removedEventFired, "Removed event should not be fired");
        }

        public void AssertAddedEventNotFired()
        {
            Assert.IsFalse(_addedEventFired, "Added event should not be fired");
        }

        public void AssertRemovedEventFired()
        {
            Assert.IsTrue(_removedEventFired, "Removed event should be fired");
        }

        public void AssertAddedEventFired()
        {
            Assert.IsTrue(_addedEventFired, "Added event should be fired");
        }

        public void RegisterForAddedAndRemovedEvents(IBusinessObjectCollection cpCol)
        {
            RegisterForAddedEvent(cpCol);
            RegisterForRemovedEvent(cpCol);
        }

        public void RegisterForRemovedEvent(IBusinessObjectCollection cpCol)
        {
            _removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { _removedEventFired = true; };
        }

        public void RegisterForAddedEvent(IBusinessObjectCollection cpCol)
        {
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };
        }
    }
}