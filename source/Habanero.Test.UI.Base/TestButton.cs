using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Button class.
    /// </summary>
    public class TestBaseMethodsWin_Button : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateButton();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Button class.
    /// </summary>
    public class TestBaseMethodsVWG_Button : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateButton();
        }
    }

    /// <summary>
    /// This test class tests the Button class.
    /// </summary>
    [TestFixture]
    public class TestButton
    {
    }
}
