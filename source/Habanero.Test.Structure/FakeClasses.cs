using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{

    /// <summary>
    /// Fake so that can use simple constructor.
    /// </summary>
    public class FakePropDef : PropDef
    {
        public FakePropDef()
            : base("prop", typeof(int), PropReadWriteRule.ReadWrite, null)
        {
        }
    }
    /// <summary>
    /// Fake so that can use simple constructor.
    /// </summary>
    public class FakeBOProp : BOProp
    {
        public FakeBOProp()
            : base(new FakePropDef())
        {
        }
    }
}