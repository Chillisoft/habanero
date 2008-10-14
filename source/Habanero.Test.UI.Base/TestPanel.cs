using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    public class TestBaseMethodsWin_Panel : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePanel();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    public class TestBaseMethodsVWG_Panel : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePanel();
        }
    }

    /// <summary>
    /// This test class tests the Panel class.
    /// </summary>
    [TestFixture]
    public class TestPanel
    {
        
    }
}
