using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.Base.ControlInterfaces;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public abstract class TestFileChooser
    {
        private const string TEST_PATH="test/path/test";
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestFileChooserWin : TestFileChooser
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestFileChooserGiz : TestFileChooser
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

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
            Assert.IsInstanceOfType(typeof(ITextBox), fileChooser.Controls[0]);
            Assert.IsInstanceOfType(typeof(IButton), fileChooser.Controls[1]);
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
