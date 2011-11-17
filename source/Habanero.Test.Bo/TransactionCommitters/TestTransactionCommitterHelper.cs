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
using Habanero.Base;
using Habanero.BO;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.TransactionCommitters
{
    public class TransactionCommitterTestHelper
    {
        public static void AssertBOStateIsValidAfterInsert_Updated(BusinessObject businessObject)
        {
           
            Assert.IsFalse(businessObject.Status.IsNew);
            Assert.IsFalse(businessObject.Status.IsDirty);
            Assert.IsFalse(businessObject.Status.IsDeleted);
            Assert.IsFalse(businessObject.Status.IsEditing);
            Assert.IsTrue(businessObject.Status.IsValid());
            string message;
            Assert.IsTrue(businessObject.Status.IsValid(out message));
            Assert.AreEqual("", message);
        }
    }


    public class StubSuccessfullTransaction : TransactionalBusinessObject
    {
        private bool _committed;

        public StubSuccessfullTransaction()
            : base(new MockBO())
        {
            _committed = false;
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected internal override bool IsDeleted
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
        protected internal override bool IsValid(out string invalidReason)
        {
            invalidReason = "";
            return true;
        }

        public bool Committed
        {
            get { return _committed; }
        }

    }
}