using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the ProgressBar class.
    /// </summary>
    public class TestBaseMethodsWin_ProgressBar : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateProgressBar();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the ProgressBar class.
    /// </summary>
    public class TestBaseMethodsVWG_ProgressBar : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateProgressBar();
        }
    }

    /// <summary>
    /// This test class tests the ProgressBar class.
    /// </summary>
    [TestFixture]
    public class TestProgressBar
    {
    }
}
