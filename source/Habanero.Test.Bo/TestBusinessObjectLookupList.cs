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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectLookupList : TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            SetupDBConnection();
            ContactPerson.CreateSampleData();
            ContactPerson.LoadDefaultClassDef();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ContactPerson.DeleteAllContactPeople();
        }

        [Test]
        public void TestGetLookupList() 
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof (ContactPerson));

            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(3, col.Count);
            foreach (object o in col.Values) {
                Assert.AreSame(typeof(ContactPerson), o.GetType());
            }
        }

        [Test]
        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPerson));
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Dictionary<string, object> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreSame(col2, col);
        }

        [Test]
        public void TestLookupListTimeout()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPerson), 100);
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            System.Threading.Thread.Sleep(250);
            Dictionary<string, object> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreNotSame(col2, col);
        }

        [Test]
        public void TestLookupListCriteria()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO", 
                "ContactPerson", "surname='zzz'");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
        }

    }
}
