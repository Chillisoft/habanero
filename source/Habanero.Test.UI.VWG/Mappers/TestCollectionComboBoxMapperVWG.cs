using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestCollectionComboBoxMapperVWG : TestCollectionComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}