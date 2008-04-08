//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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


using System;
using System.Windows.Forms;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Test;
using Habanero.UI.Base;
using Habanero.UI.Forms;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestBoTabColControl.
    /// </summary>
    [TestFixture]
    public class TestBoTabColControl : TestUsingDatabase
    {
        private BoTabColControl itsTabColControl;
        private ClassDef itsClassDef;
        private BusinessObjectCollection<BusinessObject> itsCol;
        private MyBO itsBo1;
        private MyBO itsBo2;

        [TestFixtureSetUp]
        public void SetupTextFixture()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadDefaultClassDef();
            itsCol = new BusinessObjectCollection<BusinessObject>(itsClassDef);

            itsBo1 = new MyBO();
            itsBo2 = new MyBO();
            itsCol.Add(itsBo1);
            itsCol.Add(itsBo2);

            itsTabColControl = new BoTabColControl();
            itsTabColControl.SetBusinessObjectControl(new NullBusinessObjectControl());
            itsTabColControl.SetCollection(itsCol);
        }

        [
            Test,
            ExpectedException(typeof (ArgumentException),
                "boControl must be of type Control or one of its subtypes.")]
        public void TestCheckForControlSubClass()
        {
            Mock mock = new DynamicMock(typeof (IBusinessObjectControl));
            IBusinessObjectControl mockBoControl = (IBusinessObjectControl) mock.MockInstance;
            BoTabColControl testControl = new BoTabColControl();
            testControl.SetBusinessObjectControl(mockBoControl);
        }

        [Test]
        public void TestNumberOfTabs()
        {
            Assert.AreEqual(2, itsTabColControl.TabControl.TabPages.Count);
        }

        [Test]
        public void TestCorrespondingBO()
        {
            Assert.AreSame(itsBo1, itsTabColControl.GetBo(itsTabColControl.TabControl.TabPages[0]));
            Assert.AreSame(itsBo2, itsTabColControl.GetBo(itsTabColControl.TabControl.TabPages[1]));
        }

        [Test]
        public void TestCorrespondingBONull()
        {
            Assert.IsNull(itsTabColControl.GetBo(new TabPage()));
        }


        [Test]
        public void TestGetBoWithNullTab()
        {
            Assert.IsNull(itsTabColControl.GetBo(null));
        }

        [Test]
        public void TestCorrespondingTabPage()
        {
            Assert.AreSame(itsTabColControl.TabControl.TabPages[0], itsTabColControl.GetTabPage(itsBo1));
            Assert.AreSame(itsTabColControl.TabControl.TabPages[1], itsTabColControl.GetTabPage(itsBo2));
        }

        [Test]
        public void TestSettingCollectionTwice()
        {
            BoTabColControl tabColControl = new BoTabColControl();
            tabColControl.SetBusinessObjectControl(new NullBusinessObjectControl());
            tabColControl.SetCollection(itsCol);
            tabColControl.SetCollection(itsCol);
            Assert.AreEqual(2, itsTabColControl.TabControl.TabPages.Count);
        }

        [Test]
        public void TestCurrentBusinessObject()
        {
            itsTabColControl.TabControl.SelectedTab = itsTabColControl.TabControl.TabPages[1];
            Assert.AreSame(itsBo2, itsTabColControl.CurrentBusinessObject);
            itsTabColControl.CurrentBusinessObject = itsBo1;
            Assert.AreSame(itsTabColControl.TabControl.TabPages[0], itsTabColControl.TabControl.SelectedTab);
        }


        private class NullBusinessObjectControl : Control, IBusinessObjectControl
        {
            public void SetBusinessObject(BusinessObject bo)
            {
            }
        }
    }
}