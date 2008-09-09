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

using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    public abstract class TestComboBoxCollectionController
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestComboBoxCollectionControllerVWG : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
        }

        [TestFixture]
        public class TestComboBoxCollectionControllerWin : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            ComboBoxCollectionSelector mapper = new ComboBoxCollectionSelector(cmbox, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapper);
            Assert.AreSame(cmbox, mapper.Control);
            Assert.AreSame(controlFactory, mapper.ControlFactory);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetComboBoxCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector mapper = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------
            mapper.SetCollection(myBoCol,false);
            //---------------Test Result -----------------------
            Assert.AreEqual(3,mapper.Collection.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector mapper = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO selectedBO = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(selectedBO);
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------
            mapper.SetCollection(myBoCol, false);
            mapper.Control.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, mapper.SelectedBusinessObject);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctAddedToCollection()
        {
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector mapper = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO addedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            mapper.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            mapper.Collection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4,mapper.Control.Items.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctRemovedFromCollection()
        {
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector mapper = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO removedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(removedBo);
            myBoCol.Add(new MyBO());
            mapper.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            mapper.Collection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Tear down -------------------------
        }
    }
}
