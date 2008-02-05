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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NMock;
using NUnit.Framework;
using System.Collections;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectLookupList : TestUsingDatabase
    {

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            ContactPerson.LoadDefaultClassDef();
        }

        [SetUp]
        public void SetupTest()
        {

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
        public void TestTimeout()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPerson), 100);
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            System.Threading.Thread.Sleep(250);
            Dictionary<string, object> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreNotSame(col2, col);
        }

        [Test]
        public void TestCriteria()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO", 
                "ContactPerson", "surname='zzz'", "");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            Assert.AreEqual(1, col.Count);
        }

        [Test]
        public void TestSortAttribute()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList(typeof(ContactPerson));
            Assert.IsNull(source.Sort);

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "surname");
            Assert.AreEqual("surname", source.Sort);

            source.Sort = "surname asc";
            Assert.AreEqual("surname asc", source.Sort);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortInvalidProperty()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "invalidprop");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortInvalidDirection()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "surname invalidorder");
        }

        //  This test is excluded because the sort attribute is checked in the middle of
        //  class def loading when not all the defs have been loaded.
        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestSortInvalidClassAndAssembly()
        //{
        //    BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
        //        "InvalidClass", "", "surname");
        //    source.Sort = "surname desc";
        //}

        [Test]
        public void TestSortingByDefault()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            ArrayList items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());
        }

        [Test]
        public void TestSortingCollection()
        {
            BusinessObjectLookupList source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "surname");
            Dictionary<string, object> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            ArrayList items = new ArrayList(col.Values);
            
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "surname asc");
            col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("abc", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("zzz", items[2].ToString());

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "surname desc");
            col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("zzz", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("abc", items[2].ToString());

            source = new BusinessObjectLookupList("Habanero.Test.BO",
                "ContactPerson", "", "surname des");
            col = source.GetLookupList(DatabaseConnection.CurrentConnection);
            items = new ArrayList(col.Values);

            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("zzz", items[0].ToString());
            Assert.AreEqual("abcd", items[1].ToString());
            Assert.AreEqual("abc", items[2].ToString());
        }
    }
}
