// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_PictureBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePictureBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Panel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_PictureBox : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreatePictureBox();
        }
    }

    /// <summary>
    /// This test class tests the Panel class.
    /// </summary>
    public abstract class TestPictureBox
    {
        protected abstract IControlFactory GetControlFactory();

        protected abstract string GetUnderlyingSizeModeToString(IPictureBox pictureBox);

        protected IPictureBox CreatePictureBox()
        {
            IPictureBox pictureBox = GetControlFactory().CreatePictureBox();
            return pictureBox;
        }

        [TestFixture]
        public class TestPictureBoxWin : TestPictureBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
            {
                System.Windows.Forms.PictureBox control = (System.Windows.Forms.PictureBox)pictureBox;
                return control.SizeMode.ToString();
            }
        }

        [TestFixture]
        public class TestPictureBoxVWG : TestPictureBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
            {
                Gizmox.WebGUI.Forms.PictureBox control = (Gizmox.WebGUI.Forms.PictureBox)pictureBox;
                return control.SizeMode.ToString();
            }
        }

        [Test]
        public void Test_SizeMode()
        {
            //---------------Set up test pack-------------------
            IPictureBox pictureBox = CreatePictureBox();
            PictureBoxSizeMode sizeMode = PictureBoxSizeMode.AutoSize;
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            pictureBox.SizeMode = sizeMode;
            //---------------Test Result -----------------------
            Assert.AreEqual(sizeMode.ToString(), GetUnderlyingSizeModeToString(pictureBox));
        }
    }
}
