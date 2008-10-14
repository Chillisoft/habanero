using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the TabPage class.
    /// </summary>
    public class TestBaseMethodsWin_TabPage : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTabPage("");
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the TabPage class.
    /// </summary>
    public class TestBaseMethodsVWG_TabPage : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTabPage("");
        }
    }

    /// <summary>
    /// This test class tests the TabPage class.
    /// </summary>
    [TestFixture]
    public class TestTabPage
    {
    }
}
