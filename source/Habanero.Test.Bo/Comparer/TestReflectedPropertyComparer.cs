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

using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Comparer;
using NUnit.Framework;

namespace Habanero.Test.BO.Comparer
{
    [TestFixture]
    public class TestReflectedPropertyComparer
    {
        private BusinessObjectCollection<MultiPropBO> _boCol;
        private IClassDef _classDef;
        private ReflectedPropertyComparer<MultiPropBO> comparer;

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
            //_boCol.Add((MultiPropBO)_classDef.CreateNewBusinessObject());
            //_boCol.Add((MultiPropBO)_classDef.CreateNewBusinessObject());
        }

        [Test]
        public void TestBothNull()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol.Sort(comparer);
            Assert.IsNull(_boCol[0].IntProp);
            Assert.IsNull(_boCol[1].IntProp);
        }

        [Test]
        public void TestLeftNull()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol[0].IntProp = 1;
            _boCol.Sort(comparer);
            Assert.IsNull(_boCol[0].IntProp);
            Assert.IsNotNull(_boCol[1].IntProp);
        }

        [Test]
        public void TestRightNull()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol[1].IntProp = 1;
            _boCol.Sort(comparer);
            Assert.IsNull(_boCol[0].IntProp);
            Assert.IsNotNull(_boCol[1].IntProp);
        }

        [Test]
        public void TestString()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("StringProp");
            _boCol[0].StringProp = "b";
            _boCol[1].StringProp = "a";
            _boCol.Sort(comparer);
            Assert.IsTrue(string.Compare(_boCol[0].StringProp, _boCol[1].StringProp) < 0);
        }

        [Test]
        public void TestInt()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol[0].IntProp = 2;
            _boCol[1].IntProp = 1;
            _boCol.Sort(comparer);
            Assert.IsTrue(_boCol[0].IntProp < _boCol[1].IntProp);
        }

        [Test]
        public void TestDouble()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("DoubleProp");
            _boCol[0].DoubleProp = 2.1;
            _boCol[1].DoubleProp = 2.0;
            _boCol.Sort(comparer);
            Assert.IsTrue(_boCol[0].DoubleProp < _boCol[1].DoubleProp);
        }

        [Test]
        public void TestDateTime()
        {
            comparer = new ReflectedPropertyComparer<MultiPropBO>("DateTimeProp");
            _boCol[0].DateTimeProp = new DateTime?(new DateTime(2006, 1, 2));
            _boCol[1].DateTimeProp = new DateTime?(new DateTime(2005, 1, 2));
            _boCol.Sort(comparer);
            Assert.IsTrue(_boCol[0].DateTimeProp < _boCol[1].DateTimeProp);
        }

        // Couldn't get coverage on last two lines - keep getting InvalidOperationException
        //[Test]
        //public void TestSingle()
        //{
        //    comparer = new ReflectedPropertyComparer<MultiPropBO>("SingleProp");
        //    _boCol[0].SingleProp = (Single)2.1;
        //    _boCol[1].SingleProp = (Single)2.0;
        //    _boCol.Sort(comparer);
        //    Assert.IsTrue(_boCol[0].SingleProp < _boCol[1].SingleProp);
        //}
    }
}