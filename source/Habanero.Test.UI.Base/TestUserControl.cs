using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the UserControl class.
    /// </summary>
    public class TestBaseMethodsWin_UserControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return new UserControlWin();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the UserControl class.
    /// </summary>
    public class TestBaseMethodsVWG_UserControl : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return new UserControlVWG();
        }
    }

    /// <summary>
    /// This test class tests the UserControl class.
    /// </summary>
    [TestFixture]
    public class TestUserControl
    {
        
    }
}
