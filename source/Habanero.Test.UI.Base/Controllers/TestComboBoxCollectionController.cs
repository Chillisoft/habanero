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
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Controllers
{

    public abstract class TestComboBoxCollectionController
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestComboBoxCollectionControllerWin : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestComboBoxCollectionControllerGiz : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

        }

        [Test]
        public void TestCreateTestComboBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>(); 
            MyBO myBO1 = new MyBO();
            MyBO myBO2 = new MyBO();
            myBOs.Add(myBO1,myBO2);
            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionController controller = new ComboBoxCollectionController(cmb,GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            controller.SetCollection(myBOs, false);
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, controller.Collection);
            Assert.AreSame(cmb,controller.Control);
            //---------------Tear Down -------------------------   
        }

    }
}
