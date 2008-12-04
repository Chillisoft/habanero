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
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    ///// <summary>
    ///// This test class tests the base inherited methods of the ListView class.
    ///// </summary>
    //public class TestBaseMethodsWin_ListView : TestBaseMethods.TestBaseMethodsWin
    //{
    //    protected override IControlHabanero CreateControl()
    //    {
    //        return new GetControlFactory().CreateListView();
    //    }
    //}

    ///// <summary>
    ///// This test class tests the base inherited methods of the ListView class.
    ///// </summary>
    //public class TestBaseMethodsVWG_ListView : TestBaseMethods.TestBaseMethodsVWG
    //{
    //    protected override IControlHabanero CreateControl()
    //    {
    //        return GetControlFactory().CreateListView();
    //    }
    //}

    /// <summary>
    /// This test class tests the ListView class.
    /// </summary>
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
        public class TestLisViewVWG : TestListView
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
        }
        //TODO: Port
//        [Test]
//        public void TestListViewConstructor()
//        {
//            //---------------Set up test pack-------------------
//            //---------------Execute Test ----------------------
//            IControlHabanero controlChilli = GetControlFactory().CreateListView();
//
//            //---------------Test Result -----------------------
//            Assert.IsNotNull(controlChilli);
//            Assert.AreEqual(typeof(Habanero.UI.WebGUI.ListViewVWG), controlChilli.GetType());
//
//            //---------------Tear Down -------------------------   
//        }

        //[Test]
        //public void TestListView_SetCollection()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    IListView listView = GetControlFactory().CreateListView();
        //    MyBO.LoadDefaultClassDefVWG();
        //    BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
        //    col.Add(new MyBO());
        //    col.Add(new MyBO());
        //    //---------------Execute Test ----------------------
        //    listView.SetCollection(col);

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(2, listView.Items.Count);
        //    //---------------Tear Down -------------------------   
        //}

        //[Test]
        //public void TestLisView_SelectedItems()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    IListView listView = GetControlFactory().CreateListView();
        //    MyBO.LoadDefaultClassDefVWG();
        //    BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
        //    col.Add(new MyBO());
        //    col.Add(new MyBO());
        //    col.Add(new MyBO());
        //    listView.SetCollection(col);
        //    //---------------Execute Test ----------------------

        //    listView.Items[0].Selected = true;
        //    listView.Items[1].Selected = true;


        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(2, listView.SelectedItems.Count);
        //    //---------------Tear Down -------------------------   
        //}


    }
}

