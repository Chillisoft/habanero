#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{

    #region Test BusinessObjectAdded Event

    [TestFixture]
    public class TestBusinessObjectCollection_Event_BusinessObjectAdded : TestBusinessObjectCollection_Event<BOEventArgs<ContactPersonTestBO>>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectAdded += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectAdded -= eventHandler;
        }
    }

    [TestFixture]
    public class TestBusinessObjectCollection_Event_IBusinessObjectCollection_BusinessObjectAdded : TestBusinessObjectCollection_Event<BOEventArgs>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectAdded += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectAdded -= eventHandler;
        }
    }

    #endregion //Test BusinessObjectAdded Event

    #region Test BusinessObjectIDUpdated Event

    [TestFixture]
    public class TestBusinessObjectCollection_Event_BusinessObjectIDUpdated : TestBusinessObjectCollection_Event<BOEventArgs<ContactPersonTestBO>>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
            bo.ContactPersonID = Guid.NewGuid();
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectIDUpdated += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectIDUpdated -= eventHandler;
        }
    }

    [TestFixture]
    public class TestBusinessObjectCollection_Event_IBusinessObjectCollection_BusinessObjectIDUpdated : TestBusinessObjectCollection_Event<BOEventArgs>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
            bo.ContactPersonID = Guid.NewGuid();
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectIDUpdated += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectIDUpdated -= eventHandler;
        }
    }

    #endregion //Test BusinessObjectIDUpdated Event

    #region Test BusinessObjectRemoved Event

    [TestFixture]
    public class TestBusinessObjectCollection_Event_BusinessObjectRemoved : TestBusinessObjectCollection_Event<BOEventArgs<ContactPersonTestBO>>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
            col.Remove(bo);
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectRemoved += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectRemoved -= eventHandler;
        }
    }

    [TestFixture]
    public class TestBusinessObjectCollection_Event_IBusinessObjectCollection_BusinessObjectRemoved : TestBusinessObjectCollection_Event<BOEventArgs>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
            col.Remove(bo);
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectRemoved += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectRemoved -= eventHandler;
        }
    }

    #endregion //Test BusinessObjectRemoved Event

    #region Test BusinessObjectUpdated Event

    [TestFixture]
    public class TestBusinessObjectCollection_Event_BusinessObjectUpdated : TestBusinessObjectCollection_Event<BOEventArgs<ContactPersonTestBO>>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
            bo.Surname = "NewSurname";
            bo.Save();
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectUpdated += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs<ContactPersonTestBO>> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            col.BusinessObjectUpdated -= eventHandler;
        }
    }

    [TestFixture]
    public class TestBusinessObjectCollection_Event_IBusinessObjectCollection_BusinessObjectUpdated : TestBusinessObjectCollection_Event<BOEventArgs>
    {
        protected override void CauseEventToFire(BusinessObjectCollection<ContactPersonTestBO> col, ContactPersonTestBO bo)
        {
            col.Add(bo);
            bo.Surname = "NewSurname";
            bo.Save();
        }

        protected override void RegisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectUpdated += eventHandler;
        }

        protected override void DeregisterForEvent(EventHandler<BOEventArgs> eventHandler, BusinessObjectCollection<ContactPersonTestBO> col)
        {
            ((IBusinessObjectCollection)col).BusinessObjectUpdated -= eventHandler;
        }
    }

    #endregion //Test BusinessObjectUpdated Event
}