//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the CheckBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_CheckBox : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCheckBox();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CheckBox class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_CheckBox : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCheckBox();
        }
    }

    /// <summary>
    /// This test class tests the CheckBox class.
    /// </summary>
    [TestFixture]
    public class TestCheckBox
    {
//
//        [Test]
//        public void TestMethod()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            BusinessObjectGridForm businessObjectGridForm = new BusinessObjectGridForm(ContactPersonTestBO.LoadDefaultClassDefWithUIDef());
//            //---------------Assert Precondition----------------
//
//            //---------------Execute Test ----------------------
//            businessObjectGridForm.LoadCollection("", "", 100);
//            businessObjectGridForm.GridControl.Buttons["Add"].PerformClick();
//            //---------------Test Result -----------------------
//
//        }
//        class BusinessObjectGridForm : UserControlWin, IFormControl
//        {
//            BorderLayoutManager _layoutManager;
//            IBusinessObjectCollection collection;
//            private IReadOnlyGridControl grid;
//            protected static IBusinessObjectCollection CreateCollectionOfType(Type BOType)
//            {
//                Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(BOType);
//                return (IBusinessObjectCollection)Activator.CreateInstance(boColType);
//            }
//            public BusinessObjectGridForm(ClassDef classDef)
//                : base()
//            {
//                ControlFactoryWin controlFactoryWin = new ControlFactoryWin();
//                _layoutManager = controlFactoryWin.CreateBorderLayoutManager(this);
//
////                collection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, "");
//                collection = CreateCollectionOfType(classDef.ClassType);
//                collection.ClassDef = classDef;
//                grid = controlFactoryWin.CreateReadOnlyGridControl();
//                GridControl.FilterControl.Visible = false;
//
//                _layoutManager.AddControl(GridControl, BorderLayoutManager.Position.Centre);
//            }
//
//            public void SetForm(IFormHabanero form) { }
//
//            public BorderLayoutManager FormBorderLayoutManager
//            {
//                get { return _layoutManager; }
//                set { _layoutManager = value; }
//            }
//
//            public void LoadCollection(string searchCriteria, string orderByClause, int limit)
//            {
//                collection.LoadWithLimit(searchCriteria, orderByClause, limit);
//                GridControl.SetBusinessObjectCollection(collection);
//
//            }
//
//            public IBusinessObjectCollection GridCollection
//            {
//                get { return collection; }
//            }
//
//            public IReadOnlyGridControl GridControl
//            {
//                get { return grid; }
//            }
//        }
    }
}
