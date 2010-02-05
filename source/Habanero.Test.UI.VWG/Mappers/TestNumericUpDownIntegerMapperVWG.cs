using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestNumericUpDownIntegerMapperVWG : TestNumericUpDownIntegerMapper
    {
        public override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}