using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the RadioButton class.
    /// </summary>
    public class TestBaseMethodsWin_RadioButton : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateRadioButton("");
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the RadioButton class.
    /// </summary>
    public class TestBaseMethodsVWG_RadioButton : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateRadioButton("");
        }
    }

    /// <summary>
    /// This test class tests the RadioButton class.
    /// </summary>
    [TestFixture]
    public class TestRadioButton
    {
    }
}
