using System;
using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestExtendedLookupComboBoxMapperWin : TestExtendedLookupComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IExtendedComboBox CreateExtendedComboBox()
        {
            return new ExtendedComboBoxWin(GetControlFactory());
        }
    }
}