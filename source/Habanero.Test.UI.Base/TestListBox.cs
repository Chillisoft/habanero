using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestListBox
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestListBoxWin : TestListBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new WinControlFactory();
            }
        }

        [TestFixture]
        public class TestListBoxGiz : TestListBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new GizmoxControlFactory();
            }
        }

        //[Test]
        //public void TestCreateListBoxGizmox()
        //{
        //    TestCreateListBox(new GizmoxControlFactory());
        //}

        //[Test]
        //public void TestCreateListBoxWindows()
        //{
        //    TestCreateListBox(new WinControlFactory());
        //}

        [Test]
        public void TestCreateListBox()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IListBox myListBox = GetControlFactory().CreateListBox();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myListBox);

            //---------------Tear Down -------------------------   
        }

        //[Test]
        //public void TestListBoxItemsGiz()
        //{
        //    TestListBoxItems(new GizmoxControlFactory());
        //}

        //[Test]
        //public void TestListBoxItemsWin()
        //{
        //    TestListBoxItems(new WinControlFactory());
        //}

        public void TestListBoxItems()
        {
            //---------------Set up test pack-------------------
            IListBox myListBox = GetControlFactory().CreateListBox();

            //---------------Execute Test ----------------------
            myListBox.Items.Add("testitem");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, myListBox.Items.Count);
            //---------------Tear Down -------------------------   
        }


      
        
    }
}
