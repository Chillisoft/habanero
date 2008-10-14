using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the NumericUpDown class.
    /// </summary>
    public class TestBaseMethodsWin_NumericUpDown : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateNumericUpDown();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the NumericUpDown class.
    /// </summary>
    public class TestBaseMethodsVWG_NumericUpDown : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateNumericUpDown();
        }
    }

    /// <summary>
    /// This test class tests the NumericUpDown class.
    /// </summary>
    [TestFixture]
    public class TestNumericUpDown
    {
    }
}
