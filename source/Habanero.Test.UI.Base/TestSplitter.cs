using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Splitter class.
    /// </summary>
    public class TestBaseMethodsWin_Splitter : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitter();
        }

        [Test, Ignore("The Splitter Control does not support this docking setting by design.")]
        public override void TestConversion_DockStyle_None()
        {
            base.TestConversion_DockStyle_None();
        }

        [Test, Ignore("The Splitter Control does not support this docking setting by design.")]
        public override void TestConversion_DockStyle_Fill()
        {
            base.TestConversion_DockStyle_Fill();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Splitter class.
    /// </summary>
    public class TestBaseMethodsVWG_Splitter : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitter();
        }

        [Test, Ignore("The Splitter Control does not support this docking setting by design.")]
        public override void TestConversion_DockStyle_None()
        {
            base.TestConversion_DockStyle_None();
        }

        [Test, Ignore("The Splitter Control does not support this docking setting by design.")]
        public override void TestConversion_DockStyle_Fill()
        {
            base.TestConversion_DockStyle_Fill();
        }
    }

    /// <summary>
    /// This test class tests the Splitter class.
    /// </summary>
    [TestFixture]
    public class TestSplitter
    {
    }
}
