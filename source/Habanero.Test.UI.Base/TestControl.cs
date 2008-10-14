using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Control class.
    /// </summary>
    public class TestBaseMethodsWin_Control : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateControl();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Control class.
    /// </summary>
    public class TestBaseMethodsVWG_Control : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateControl();
        }
    }

    /// <summary>
    /// This test class tests the Control class.
    /// </summary>
    [TestFixture]
    public class TestControl
    {
        
    }
}
