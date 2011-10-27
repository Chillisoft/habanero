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
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.RelatedBusinessObjectCollection
{
    public class TestUtilsRelated
    {
        private bool _addedEventFired;
        private bool _removedEventFired;

        public bool AddedEventFired
        {
            get { return _addedEventFired; }
        }

        public bool RemovedEventFired
        {
            get { return _removedEventFired; }
        }

        public void AssertOneObjectInCurrentAndAddedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count, "There should b no persisted items");
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public void AssertOneObjectInCurrentPersistedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count, "No objects should be in the added collection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count, "One object should be in the persisted items");
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count, "zero objects should be in the created items");
        }

        public void AssertOneObjectInCurrentAndCreatedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        public void AssertOneObjectInMarkForDeleteAndAddedCollection
            (IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public void AssertOneObjectInRemovedAndPersisted(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public void AssertAllCollectionsHaveNoItems(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        public void AssertOneObjectInCurrentPersistedAndAddedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
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
            Assert.IsFalse(RemovedEventFired, "Removed event should not be fired");
        }

        public void AssertAddedEventNotFired()
        {
            Assert.IsFalse(AddedEventFired, "Added event should not be fired");
        }

        public void AssertRemovedEventFired()
        {
            Assert.IsTrue(RemovedEventFired, "Removed event should be fired");
        }

        public void AssertAddedEventFired()
        {
            Assert.IsTrue(AddedEventFired, "Added event should be fired");
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