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
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestListBoxGiz : TestListBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

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
