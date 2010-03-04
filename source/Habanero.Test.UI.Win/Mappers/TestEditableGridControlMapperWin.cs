using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestEditableGridControlMapperWin : TestEditableGridControlMapper
    {

        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}