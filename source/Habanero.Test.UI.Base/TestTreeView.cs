using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the TreeView class.
    /// </summary>
    public class TestBaseMethodsWin_TreeView : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTreeView("");
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the TreeView class.
    /// </summary>
    public class TestBaseMethodsVWG_TreeView : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTreeView("");
        }
    }

    /// <summary>
    /// This test class tests the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestTreeView
    {
    }
}
