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


using System.Windows.Forms;
using Habanero.BO;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestCollectionComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestCollectionComboBoxMapper
    {
        ComboBox itsComboBox;
        CollectionComboBoxMapper mapper;
        private BusinessObjectCollection<BusinessObject> itsCollection;

        [SetUp]
        public void SetupTest()
        {
            itsComboBox = new ComboBox();
            mapper = new CollectionComboBoxMapper(itsComboBox);
            itsCollection = new BusinessObjectCollection<BusinessObject>(Sample.GetClassDef());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            mapper.SetCollection(itsCollection, false);

        }

        [Test]
        public void TestSetCollection()
        {
            Assert.AreEqual(3, itsComboBox.Items.Count);
        }

        [Test]
        public void TestGetBusinessObject()
        {
            itsComboBox.SelectedItem = itsCollection[2];
            Assert.IsNotNull(mapper.SelectedBusinessObject );
            Assert.AreSame(itsCollection[2], mapper.SelectedBusinessObject);
        }

        [Test]
        public void TestAddBoToCollection()
        {
            itsCollection.Add(new Sample());
            Assert.AreEqual(4, itsComboBox.Items.Count);
        }

        [Test]
        public void TestRemoveBoFromCollection()
        {
            itsCollection.RemoveAt(0);
            Assert.AreEqual(2, itsComboBox.Items.Count);
        }
    }
}