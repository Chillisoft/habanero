using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    public class TransactionalBusinessObjectInMemory : TransactionalBusinessObject
    {
        protected internal TransactionalBusinessObjectInMemory(IBusinessObject businessObject) : base(businessObject)
        {
        }
    }
}
