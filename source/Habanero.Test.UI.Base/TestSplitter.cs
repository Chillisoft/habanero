//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the Splitter class.
    /// </summary>
    [TestFixture]
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
    [TestFixture]
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
