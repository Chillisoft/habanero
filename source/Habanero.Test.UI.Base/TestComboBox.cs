using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the ComboBox class.
    /// </summary>
    public class TestBaseMethodsWin_ComboBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the ComboBox class.
    /// </summary>
    public class TestBaseMethodsVWG_ComboBox : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateComboBox();
        }
    }

    /// <summary>
    /// This test class tests the ComboBox class.
    /// </summary>
    [TestFixture]
    public class TestComboBox
    {
    }
}
