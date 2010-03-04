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


using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public abstract class TestFileChooser
    {
        private const string TEST_PATH="test/path/test";
        protected abstract IControlFactory GetControlFactory();



        [Test]
        public void TestCreateControl()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IFileChooser fileChooser = GetControlFactory().CreateFileChooser();
            //---------------Test Result -----------------------
            Assert.IsNotNull(fileChooser, "A File Chooser should have been created");
            Assert.AreEqual(2, fileChooser.Controls.Count);
            Assert.IsInstanceOf(typeof(ITextBox), fileChooser.Controls[0]);
            Assert.IsInstanceOf(typeof(IButton), fileChooser.Controls[1]);
            IButton button = (IButton)fileChooser.Controls[1];
            Assert.AreEqual(button.Text, "Select...");
            //---------------Tear Down -------------------------          

        }

        [Test]
        public void TestSelectedFilePath()
        {
            //---------------Set up test pack-------------------
            IFileChooser fileChooser = GetControlFactory().CreateFileChooser();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual("",fileChooser.SelectedFilePath);
            //---------------Execute Test ----------------------
            fileChooser.SelectedFilePath = TEST_PATH;
            //---------------Test Result -----------------------
            Assert.AreEqual(TEST_PATH,fileChooser.SelectedFilePath);
            //---------------Tear Down -------------------------          
        }
        
    }
}
