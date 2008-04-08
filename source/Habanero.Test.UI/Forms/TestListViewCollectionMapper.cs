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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    [TestFixture ]
    public class TestListViewCollectionMapper
    {

        ListView itsListView;
        ListViewCollectionMapper mapper;
        private BusinessObjectCollection<BusinessObject> itsCollection;

        [SetUp]
        public void SetupTest()
        {
            itsListView = new ListView();
            mapper = new ListViewCollectionMapper(itsListView);
            itsCollection = new BusinessObjectCollection<BusinessObject>(Sample.GetClassDef());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            mapper.SetCollection(itsCollection);
        }

        [Test]
        public void TestSetCollection()
        {
            Assert.AreEqual(3, itsListView.Items.Count);
        }

        
        //TODO: this test works in debug mode, but not in nunit.
        //[Test]
        //public void TestGetBusinessObject()
        //{
        //    itsListView.Focus();
        //    itsListView.Items[2].Selected = true;
        //    itsListView.Items[0].Focused = true;
        //    Assert.IsNotNull(mapper.SelectedBusinessObject);
        //    Assert.AreSame(_collection[2], mapper.SelectedBusinessObject);
        //}

        [Test]
        public void TestAddBoToCollection()
        {
            itsCollection.Add(new Sample());
            Assert.AreEqual(4, itsListView .Items.Count);
        }

        [Test]
        public void TestRemoveBoFromCollection()
        {
            itsCollection.RemoveAt(0);
            Assert.AreEqual(2, itsListView.Items.Count);
        }        
    }
}