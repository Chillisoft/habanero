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
    /// This test class tests the base inherited methods of the Label class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Label : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateLabel();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Label class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_Label : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateLabel();
        }
    }

    /// <summary>
    /// This test class tests the Label class.
    /// </summary>
    public abstract class TestLabel
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestLabelWin : TestLabel
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestLabelVWG : TestLabel
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.VWG.ControlFactoryVWG();
            }

            [Test]
            public void TestPreferredSize()
            {
                //---------------Set up test pack-------------------
                ILabel myLabel = GetControlFactory().CreateLabel();
                string labelText = "sometext";
                myLabel.Text = labelText;

                //---------------Execute Test ----------------------
                int preferredWidth = myLabel.PreferredWidth;
                //---------------Test Result -----------------------

                Assert.AreEqual(labelText.Length * 6, preferredWidth);
                //---------------Tear Down -------------------------          
            }
        }

        [Test]
        public void TestCreateLabel()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            ILabel myLabel = GetControlFactory().CreateLabel();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myLabel);

            //---------------Tear Down -------------------------   
        }

   




    }
}
