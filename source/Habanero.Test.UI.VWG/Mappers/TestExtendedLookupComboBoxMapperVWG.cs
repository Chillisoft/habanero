using System;
using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [Ignore("The ExtendedComboBox is not implemented for VWG")]
    [TestFixture]
    public class TestExtendedLookupComboBoxMapperVWG : TestExtendedLookupComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IExtendedComboBox CreateExtendedComboBox()
        {
            throw new NotImplementedException();
        }
    }
}