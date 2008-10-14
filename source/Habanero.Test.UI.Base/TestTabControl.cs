using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the TabControl class.
    /// </summary>
    public class TestBaseMethodsWin_TabControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTabControl();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the TabControl class.
    /// </summary>
    public class TestBaseMethodsVWG_TabControl : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTabControl();
        }
    }

    /// <summary>
    /// This test class tests the TabControl class.
    /// </summary>
    [TestFixture]
    public class TestTabControl
    {
    }
}
