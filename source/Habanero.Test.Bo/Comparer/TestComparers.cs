// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Comparer;
using NUnit.Framework;

namespace Habanero.Test.BO.Comparer
{
    [TestFixture]
    public class TestComparers
    {
        private BusinessObjectCollection<MultiPropBO> _boCol;
        private IClassDef _classDef;

        [TestFixtureSetUp]
        public void DefineBO()
        {
            ClassDef.ClassDefs.Clear();
            _classDef = MultiPropBO.LoadClassDef();
        }

        [SetUp]
        public void InitialiseCollection()
        {
            _boCol = new BusinessObjectCollection<MultiPropBO>();
            _boCol.Add((MultiPropBO)_classDef.CreateNewBusinessObject());
            _boCol.Add((MultiPropBO)_classDef.CreateNewBusinessObject());
            _boCol.Add((MultiPropBO)_classDef.CreateNewBusinessObject());
            _boCol.Add((MultiPropBO)_classDef.CreateNewBusinessObject());
        }

        [Test]
        public void TestDateTimeComparer()
        {
            string propName = "DateTimeProp";
            DateTime dt1 = new DateTime(2006, 4, 1);
            DateTime dt2 = new DateTime(2005, 4, 2);
            DateTime dt3 = new DateTime(2005, 4, 1);

            _boCol[0].SetPropertyValue(propName, dt1);
            _boCol[1].SetPropertyValue(propName, dt2);
            _boCol[2].SetPropertyValue(propName, dt3);

            DateTimeComparer<MultiPropBO> comparer = new DateTimeComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual(dt3, _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual(dt2, _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual(dt1, _boCol[3].GetPropertyValue(propName));
        }

        [Test]
        public void TestDoubleComparer()
        {
            string propName = "DoubleProp";
            _boCol[0].SetPropertyValue(propName, 3.1);
            _boCol[1].SetPropertyValue(propName, 5.2);
            _boCol[2].SetPropertyValue(propName, 3.0);

            DoubleComparer<MultiPropBO> comparer = new DoubleComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual(3.0, _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual(3.1, _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual(5.2, _boCol[3].GetPropertyValue(propName));
        }

        [Test]
        public void TestGuidComparer()
        {
            string propName = "GuidProp";
            Guid guid1 = new Guid("11111111-1111-1111-1111-111111111111");
            Guid guid2 = new Guid("22222222-2222-2222-2222-222222222222");
            Guid guid3 = new Guid("33333333-3333-3333-3333-333333333333");

            _boCol[0].SetPropertyValue(propName, guid3);
            _boCol[1].SetPropertyValue(propName, guid1);
            _boCol[2].SetPropertyValue(propName, guid2);
            
            GuidComparer<MultiPropBO> comparer = new GuidComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual(guid1, _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual(guid2, _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual(guid3, _boCol[3].GetPropertyValue(propName));
        }

        [Test]
        public void TestIntComparer()
        {
            string propName = "IntProp";
            _boCol[0].SetPropertyValue(propName, 3);
            _boCol[1].SetPropertyValue(propName, 5);
            _boCol[2].SetPropertyValue(propName, 2);

            IntComparer<MultiPropBO> comparer = new IntComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual(2, _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual(3, _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual(5, _boCol[3].GetPropertyValue(propName));
        }

        [Test]
        public void TestSingleComparer()
        {
            string propName = "SingleProp";
            _boCol[0].SetPropertyValue(propName, 3.1);
            _boCol[1].SetPropertyValue(propName, 5.2);
            _boCol[2].SetPropertyValue(propName, 3.0);

            SingleComparer<MultiPropBO> comparer = new SingleComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual((Single)3.0, _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual((Single)3.1, _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual((Single)5.2, _boCol[3].GetPropertyValue(propName));
        }

        [Test]
        public void TestStringComparer()
        {
            string propName = "StringProp";
            _boCol[0].SetPropertyValue(propName, "b");
            _boCol[1].SetPropertyValue(propName, "c");
            _boCol[2].SetPropertyValue(propName, "a");

            StringComparer<MultiPropBO> comparer = new StringComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual("a", _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual("b", _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual("c", _boCol[3].GetPropertyValue(propName));
        }

        [Test]
        public void TestTimeSpanComparer()
        {
            string propName = "TimeSpanProp";
            TimeSpan ts1 = new TimeSpan(0, 0, 1);
            TimeSpan ts2 = new TimeSpan(0, 1, 1);
            TimeSpan ts3 = new TimeSpan(1, 1, 1);

            _boCol[0].SetPropertyValue(propName, ts3);
            _boCol[1].SetPropertyValue(propName, ts1);
            _boCol[2].SetPropertyValue(propName, ts2);

            TimeSpanComparer<MultiPropBO> comparer = new TimeSpanComparer<MultiPropBO>(propName);
            _boCol.Sort(comparer);

            Assert.IsNull(_boCol[0].GetPropertyValue(propName));
            Assert.AreEqual(ts1, _boCol[1].GetPropertyValue(propName));
            Assert.AreEqual(ts2, _boCol[2].GetPropertyValue(propName));
            Assert.AreEqual(ts3, _boCol[3].GetPropertyValue(propName));
        }
    }
}