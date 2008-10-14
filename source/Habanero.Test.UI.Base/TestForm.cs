using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Form class.
    /// </summary>
    public class TestBaseMethodsWin_Form : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateForm();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Form class.
    /// </summary>
    public class TestBaseMethodsVWG_Form : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateForm();
        }
    }

    /// <summary>
    /// This test class tests the Form class.
    /// </summary>
    [TestFixture]
    public class TestForm
    {
    }
}
