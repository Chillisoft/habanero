using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.DB4O
{
    internal class TransactionalBusinessObjectDB4O : TransactionalBusinessObject 
    {
        protected internal TransactionalBusinessObjectDB4O(IBusinessObject businessObject) : base(businessObject) {}
    }
}
