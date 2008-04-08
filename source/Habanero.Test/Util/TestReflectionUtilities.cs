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
using System.Reflection;
using Habanero.Base.Exceptions;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestReflectionUtilities
    {
        [Test]
        public void TestExecuteMethod()
        {
            SimpleClass simpleClass = new SimpleClass();
            Assert.IsFalse(simpleClass.MethodCalled);

            ReflectionUtilities.ExecuteMethod(simpleClass, "MyMethod");
            Assert.IsTrue(simpleClass.MethodCalled);
        }

        [Test, ExpectedException(typeof(Exception))]
        public void TestExecuteMethodDoesntExist()
        {
            SimpleClass simpleClass = new SimpleClass();
            ReflectionUtilities.ExecuteMethod(simpleClass, "InvalidMethod");
        }

        private class SimpleClass
        {
            private bool _methodCalled = false;

            public SimpleClass()
            {}

            public void MyMethod()
            {
                _methodCalled = true;
            }

            public bool MethodCalled
            {
                get { return _methodCalled; }
            }
        }
    }
}
