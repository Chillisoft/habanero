using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestListView
    {
        protected abstract IControlFactory GetControlFactory();

//        [TestFixture]
//        public class TestListBoxWin : TestListView
//        {
//            protected override IControlFactory GetControlFactory()
//            {
//                return new ControlFactoryWin();
//            }
//        }

        [TestFixture]
        public class TestLisViewGiz : TestListView
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [Test]
        public void TestLisView()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlChilli controlChilli = GetControlFactory().CreateListView();

            //---------------Test Result -----------------------
            Assert.IsNotNull(controlChilli);
            Assert.AreEqual(typeof(Habanero.UI.WebGUI.ListViewGiz), controlChilli.GetType());

            //---------------Tear Down -------------------------   
        }

        //[Test]
        //public void TestListView_SetCollection()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    IListView listView = GetControlFactory().CreateListView();
        //    MyBO.LoadDefaultClassDefGizmox();
        //    BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
        //    col.Add(new MyBO());
        //    col.Add(new MyBO());
        //    //---------------Execute Test ----------------------
        //    listView.SetCollection(col);

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(2, listView.Items.Count);
        //    //---------------Tear Down -------------------------   
        //}

        [Test]
        public void TestLisView_SelectedItems()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IListView listView = GetControlFactory().CreateListView();
            MyBO.LoadDefaultClassDefGizmox();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(new MyBO());
            col.Add(new MyBO());
            col.Add(new MyBO());
            listView.SetCollection(col);
            //---------------Execute Test ----------------------

            listView.Items[0].Selected = true;
            listView.Items[1].Selected = true;


            //---------------Test Result -----------------------
            Assert.AreEqual(2, listView.SelectedItems.Count);
            //---------------Tear Down -------------------------   
        }


    }
}

