using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestRelationshipComboBoxMapperVWG : TestRelationshipComboBoxMapper
    {
        protected override void CreateControlFactory()
        {
            _controlFactory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = _controlFactory;
        }
    }
}