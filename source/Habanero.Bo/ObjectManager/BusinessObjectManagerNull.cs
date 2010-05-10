using System;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// This is a Business Object Manager that can be used for ASP and similarly stateless environments.
    /// </summary>
    public class BusinessObjectManagerNull : BusinessObjectManager
    {
        public override void Add(IBusinessObject businessObject)
        {
            //DO Nothing
        }

        public override IBusinessObject this[Guid objectID]
        {
            get { return null; }
        }

        public override IBusinessObject this[IPrimaryKey objectID]
        {
            get { return null;}
        }
    }
}