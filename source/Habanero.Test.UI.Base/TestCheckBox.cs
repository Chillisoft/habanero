using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the CheckBox class.
    /// </summary>
    public class TestBaseMethodsWin_CheckBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCheckBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CheckBox class.
    /// </summary>
    public class TestBaseMethodsVWG_CheckBox : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCheckBox();
        }
    }

    /// <summary>
    /// This test class tests the CheckBox class.
    /// </summary>
    [TestFixture]
    public class TestCheckBox
    {
    }
}
