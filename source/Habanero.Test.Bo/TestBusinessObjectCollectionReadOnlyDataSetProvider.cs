//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.BO;
using Habanero.Base;
using NUnit.Framework;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollectionReadOnlyDataSetProvider.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollectionReadOnlyDataSetProvider : TestBusinessObjectCollectionDataProvider
    {
        protected override IDataSetProvider CreateDataSetProvider(BusinessObjectCollection<BusinessObject> col)
        {
            return new BOCollectionReadOnlyDataSetProvider(itsCollection);
        }

        [Test]
        public void TestUpdateBusinessObjectUpdatesRow()
        {
            itsBo1.SetPropertyValue("TestProp", "UpdatedValue");
            Assert.AreEqual("UpdatedValue", itsTable.Rows[0][1]);
        }

        [Test]
        public void TestAddBusinessObjectAddsRow()
        {
            BusinessObject bo3 = itsClassDef.CreateNewBusinessObject(itsConnection);
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "s1");
            itsCollection.Add(bo3);
            Assert.AreEqual(3, itsTable.Rows.Count);
            Assert.AreEqual("bo3prop1", itsTable.Rows[2][1]);
        }

        [Test]
        public void TestAddBusinessObjectAndUpdateUpdatesNewRow()
        {
            BusinessObject bo3 = itsClassDef.CreateNewBusinessObject(itsConnection);
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "s2");
            itsCollection.Add(bo3);
            bo3.SetPropertyValue("TestProp", "UpdatedValue");
            Assert.AreEqual("UpdatedValue", itsTable.Rows[2][1]);
        }

        [Test]
        public void TestRemoveBusinessObjectRemovesRow()
        {
            itsCollection.Remove(itsBo1);
            Assert.AreEqual(1, itsTable.Rows.Count);
        }
    }
}