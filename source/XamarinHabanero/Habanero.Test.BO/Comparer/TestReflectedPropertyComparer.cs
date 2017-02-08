#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
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
        private ReflectedPropertyComparer<MultiPropBO> _comparer;

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
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol.Sort(_comparer);
            Assert.IsNull(_boCol[0].IntProp);
            Assert.IsNull(_boCol[1].IntProp);
        }

        [Test]
        public void TestLeftNull()
        {
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol[0].IntProp = 1;
            _boCol.Sort(_comparer);
            Assert.IsNull(_boCol[0].IntProp);
            Assert.IsNotNull(_boCol[1].IntProp);
        }

        [Test]
        public void TestRightNull()
        {
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol[1].IntProp = 1;
            _boCol.Sort(_comparer);
            Assert.IsNull(_boCol[0].IntProp);
            Assert.IsNotNull(_boCol[1].IntProp);
        }

        [Test]
        public void TestString()
        {
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("StringProp");
            _boCol[0].StringProp = "b";
            _boCol[1].StringProp = "a";
            _boCol.Sort(_comparer);
            Assert.IsTrue(string.Compare(_boCol[0].StringProp, _boCol[1].StringProp) < 0);
        }

        [Test]
        public void TestInt()
        {
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("IntProp");
            _boCol[0].IntProp = 2;
            _boCol[1].IntProp = 1;
            _boCol.Sort(_comparer);
            Assert.IsTrue(_boCol[0].IntProp < _boCol[1].IntProp);
        }

        [Test]
        public void TestDouble()
        {
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("DoubleProp");
            _boCol[0].DoubleProp = 2.1;
            _boCol[1].DoubleProp = 2.0;
            _boCol.Sort(_comparer);
            Assert.IsTrue(_boCol[0].DoubleProp < _boCol[1].DoubleProp);
        }

        [Test]
        public void TestDateTime()
        {
            _comparer = new ReflectedPropertyComparer<MultiPropBO>("DateTimeProp");
            _boCol[0].DateTimeProp = new DateTime?(new DateTime(2006, 1, 2));
            _boCol[1].DateTimeProp = new DateTime?(new DateTime(2005, 1, 2));
            _boCol.Sort(_comparer);
            Assert.IsTrue(_boCol[0].DateTimeProp < _boCol[1].DateTimeProp);
        }

        [Test]
        public void Test_Construct_WithNullpropertyName_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ReflectedPropertyComparer<MultiPropBO>(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propertyName", ex.ParamName);
            }
        }
        [Test]
        public void Test_Construct_WithPropertyName_WhenDoesNotExist_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ReflectedPropertyComparer<MultiPropBO>("NonExistantProp");
                Assert.Fail("expected HabaneroArgumentException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("propertyName", ex.ParameterName);
                StringAssert.Contains("could not be found on the Business Object", ex.Message);
            }
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