// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
