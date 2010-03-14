// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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

        [Test]
        public void TestExecuteMethod_WithReturnValue()
        {
            SimpleClass simpleClass = new SimpleClass();
            Assert.IsFalse(simpleClass.MethodCalled);

            var returnValue = ReflectionUtilities.ExecuteMethod(simpleClass, "MyMethodWithReturn");
            Assert.IsTrue(simpleClass.MethodCalled);
            Assert.IsTrue(Convert.ToBoolean(returnValue));
        }

        [Test, ExpectedException(typeof (Exception))]
        public void TestExecuteMethodDoesntExist()
        {
            SimpleClass simpleClass = new SimpleClass();
            ReflectionUtilities.ExecuteMethod(simpleClass, "InvalidMethod");
        }

        [Test]
        public void Test_ExecutePrivateMethod_WithNoArguments()
        {
            //--------------- Set up test pack ------------------
            ClassWithPrivateMethod classWithPrivateMethod = new ClassWithPrivateMethod();
            //--------------- Test Preconditions ----------------
            Assert.IsFalse(classWithPrivateMethod.PrivateMethodCalled);
            //--------------- Execute Test ----------------------
            ReflectionUtilities.ExecutePrivateMethod(classWithPrivateMethod, "MyPrivateMethod");
            //--------------- Test Result -----------------------
            Assert.IsTrue(classWithPrivateMethod.PrivateMethodCalled);
        }

        [Test]
        public void Test_ExecutePrivateMethod_WithArguments()
        {
            //--------------- Set up test pack ------------------
            ClassWithPrivateMethod classWithPrivateMethod = new ClassWithPrivateMethod();
            string testStringParam = TestUtil.GetRandomString();
            int testIntParam = TestUtil.GetRandomInt();
            //--------------- Test Preconditions ----------------
            Assert.IsFalse(classWithPrivateMethod.PrivateMethodCalled);
            Assert.IsNull(classWithPrivateMethod.StringParamValue);
            Assert.IsFalse(classWithPrivateMethod.IntParamValue.HasValue);
            //--------------- Execute Test ----------------------
            ReflectionUtilities.ExecutePrivateMethod(classWithPrivateMethod, "MyPrivateMethodWithParams",
                                                     testStringParam, testIntParam);
            //--------------- Test Result -----------------------
            Assert.IsTrue(classWithPrivateMethod.PrivateMethodCalled);
            Assert.AreEqual(testStringParam, classWithPrivateMethod.StringParamValue);
            Assert.IsTrue(classWithPrivateMethod.IntParamValue.HasValue);
            Assert.AreEqual(testIntParam, classWithPrivateMethod.IntParamValue.GetValueOrDefault());
        }


        [Test]
        public void Test_ExecutePrivateMethod_WithNoArgumentsWithReturn_ShouldReturnValue()
        {
            //--------------- Set up test pack ------------------
            ClassWithPrivateMethod classWithPrivateMethod = new ClassWithPrivateMethod();
            //--------------- Test Preconditions ----------------
            Assert.IsFalse(classWithPrivateMethod.PrivateMethodCalled);
            //--------------- Execute Test ----------------------
            var returnValue = ReflectionUtilities.ExecutePrivateMethod(classWithPrivateMethod,
                                                                       "MyPrivateMethodWithReturn");
            //--------------- Test Result -----------------------
            Assert.IsTrue(classWithPrivateMethod.PrivateMethodCalled);
            Assert.AreEqual("SomeString", returnValue);
        }

        [Test]
        public void Test_ExecutePrivateMethod_WithArgumentsWithReturn_ShouldReturnValue()
        {
            //--------------- Set up test pack ------------------
            ClassWithPrivateMethod classWithPrivateMethod = new ClassWithPrivateMethod();
            string testStringParam = TestUtil.GetRandomString();
            int testIntParam = TestUtil.GetRandomInt();
            //--------------- Test Preconditions ----------------
            Assert.IsFalse(classWithPrivateMethod.PrivateMethodCalled);
            Assert.IsNull(classWithPrivateMethod.StringParamValue);
            Assert.IsFalse(classWithPrivateMethod.IntParamValue.HasValue);
            //--------------- Execute Test ----------------------
            var returnValue = ReflectionUtilities.ExecutePrivateMethod(classWithPrivateMethod,
                                                                       "MyPrivateMethodWithParamsWithReturn",
                                                                       testStringParam, testIntParam);
            //--------------- Test Result -----------------------
            Assert.IsTrue(classWithPrivateMethod.PrivateMethodCalled);
            Assert.AreEqual(testStringParam, classWithPrivateMethod.StringParamValue);
            Assert.IsTrue(classWithPrivateMethod.IntParamValue.HasValue);
            Assert.AreEqual(testIntParam, classWithPrivateMethod.IntParamValue.Value);
            Assert.AreEqual(testStringParam, returnValue);
        }

        [Test]
        public void Test_SetPropertyValue_WithObject()
        {
            //---------------Set up test pack-------------------
            ClassWithProperties classWithProperties = new ClassWithProperties();
            object newValue = new object();
            //---------------Assert Precondition----------------
            Assert.IsNull(classWithProperties.ObjectProperty);
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPropertyValue(classWithProperties, "ObjectProperty", newValue);
            //---------------Test Result -----------------------
            Assert.AreSame(newValue, classWithProperties.ObjectProperty);
        }

        [Test]
        public void Test_SetPropertyValue_WithString()
        {
            //---------------Set up test pack-------------------
            ClassWithProperties classWithProperties = new ClassWithProperties();
            const string newValue = "MyString";
            //---------------Assert Precondition----------------
            Assert.IsNull(classWithProperties.StringProperty);
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPropertyValue(classWithProperties, "StringProperty", newValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, classWithProperties.StringProperty);
        }

        [Test]
        public void Test_SetPropertyValue_WithInt()
        {
            //---------------Set up test pack-------------------
            ClassWithProperties classWithProperties = new ClassWithProperties();
            const int newValue = 345;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, classWithProperties.IntProperty);
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPropertyValue(classWithProperties, "IntProperty", newValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, classWithProperties.IntProperty);
        }

        [Test]
        public void Test_SetPropertyValue_WithInterface()
        {
            //---------------Set up test pack-------------------
            ClassWithProperties classWithProperties = new ClassWithProperties();
            IMyInterface newValue = new MyInterfaceClass();
            //---------------Assert Precondition----------------
            Assert.IsNull(classWithProperties.InterfaceProperty);
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPropertyValue(classWithProperties, "InterfaceProperty", newValue);
            //---------------Test Result -----------------------
            Assert.AreSame(newValue, classWithProperties.InterfaceProperty);
        }

        [Test]
        public void Test_SetPropertyValue_ToNull_WithInterface()
        {
            //---------------Set up test pack-------------------
            ClassWithProperties classWithProperties = new ClassWithProperties();
            IMyInterface oldValue = new MyInterfaceClass();
            classWithProperties.InterfaceProperty = oldValue;
            //---------------Assert Precondition----------------
            Assert.AreSame(oldValue, classWithProperties.InterfaceProperty);
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPropertyValue(classWithProperties, "InterfaceProperty", null);
            //---------------Test Result -----------------------
            Assert.IsNull(classWithProperties.InterfaceProperty);
        }

        [Test]
        public void Test_SetPrivateProp_WherePropNullableBool()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "NullableBoolProp";
            var classWithProperties = new ClassWithProperties();
            //---------------Assert Precondition----------------
            Assert.IsNull(ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName));
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPrivatePropertyValue(classWithProperties, propertyName, true);
            //---------------Test Result -----------------------
            bool propValue = (bool) ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName);
            Assert.IsTrue(propValue);
        }

        [Test]
        public void Test_SetPrivateProp_toNull_WherePropNullableBool()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "NullableBoolProp";
            var classWithProperties = new ClassWithProperties();
            //---------------Assert Precondition----------------
            Assert.IsNull(ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName));
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPrivatePropertyValue(classWithProperties, propertyName, null);
            //---------------Test Result -----------------------
            bool? propValue = (bool?) ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName);
            Assert.IsNull(propValue);
        }

        [Test]
        public void Test_SetPrivateProp_WherePropBool()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "BoolProp";
            var classWithProperties = new ClassWithProperties();
            //---------------Assert Precondition----------------
            Assert.IsFalse((bool) ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName));
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPrivatePropertyValue(classWithProperties, propertyName, true);
            //---------------Test Result -----------------------
            bool propValue = (bool) ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName);
            Assert.IsTrue(propValue);
        }

        [Test]
        public void Test_SetPrivatePropToNull_WherePropBool()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "BoolProp";
            var classWithProperties = new ClassWithProperties();
            //---------------Assert Precondition----------------
            Assert.IsFalse((bool) ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName));
            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPrivatePropertyValue(classWithProperties, propertyName, null);
            //---------------Test Result -----------------------
            bool propValue = (bool) ReflectionUtilities.GetPrivatePropertyValue(classWithProperties, propertyName);
            Assert.IsFalse(propValue);
        }

        [Test]
        public void Test_GetUnderlyingPropertyType_WhenPublicNullableBool_ShouldReturnBool()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Type propType = ReflectionUtilities.GetUndelyingPropertType(typeof (ClassWithProperties),
                                                                        "PublicNullableBoolProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof (bool), propType);
        }

        [Test]
        public void Test_GetUnderlyingPropertyType_WhenPrivateNullableBool_ShouldReturnBool()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Type propType = ReflectionUtilities.GetUndelyingPropertType(typeof (ClassWithProperties), "NullableBoolProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof (bool), propType);
        }

        [Test]
        public void Test_GetUnderlyingPropertyType_WhenPrivateBool_ShouldReturnBool()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Type propType = ReflectionUtilities.GetUndelyingPropertType(typeof (ClassWithProperties), "BoolProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof (bool), propType);
        }

        [Test]
        public void Test_GetPropertyInfo_WithLambda_ShouldRetInfo()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            var propInfo = ReflectionUtilities.GetPropertyInfo<ClassWithProperties>(bo => bo.StringProperty);
            //---------------Test Result -----------------------
            Assert.AreEqual("StringProperty", propInfo.Name);
            Assert.AreSame(typeof (string), propInfo.PropertyType);
        }
        [Test]
        public void Test_GetPropertyName_WithLambda_ShouldRetName()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            var propertyName = ReflectionUtilities.GetPropertyName<ClassWithProperties>(bo => bo.StringProperty);
            //---------------Test Result -----------------------
            Assert.AreEqual("StringProperty", propertyName);
        }

        [Test]
        public void Test_GetPropertyInfo_WithInvalidLambda_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                ReflectionUtilities.GetPropertyInfo<ClassWithProperties>(bo => bo.GetType());
                Assert.Fail("Expected to throw an ArgumentException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("Not a member access", ex.Message);
            }
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local

        // ReSharper disable UnusedMember.Local
        private class ClassWithProperties
        {
            public object ObjectProperty { get; set; }
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
            public IMyInterface InterfaceProperty { get; set; }

            private bool? NullableBoolProp { get; set; }
            public bool? PublicNullableBoolProp { get; set; }
            private bool BoolProp { get; set; }
        }

        // ReSharper restore UnusedMember.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        internal interface IMyInterface
        {
        }

        internal class MyInterfaceClass : IMyInterface
        {
        }


        private class ClassWithPrivateMethod
        {
            private void MyPrivateMethod()
            {
                PrivateMethodCalled = true;
            }

            private string MyPrivateMethodWithReturn()
            {
                PrivateMethodCalled = true;
                return "SomeString";
            }

            private string MyPrivateMethodWithParamsWithReturn(string stringParam, int intParam)
            {
                PrivateMethodCalled = true;
                StringParamValue = stringParam;
                IntParamValue = intParam;
                return stringParam;
            }

            private void MyPrivateMethodWithParams(string stringParam, int intParam)
            {
                PrivateMethodCalled = true;
                StringParamValue = stringParam;
                IntParamValue = intParam;
            }

            public int? IntParamValue { get; private set; }

            public string StringParamValue { get; private set; }

            public bool PrivateMethodCalled { get; private set; }
        }

        private class SimpleClass
        {
            private bool _methodCalled = false;

            public SimpleClass()
            {
            }

            public void MyMethod()
            {
                _methodCalled = true;
            }

            public bool MyMethodWithReturn()
            {
                _methodCalled = true;
                return true;
            }

            public bool MethodCalled
            {
                get { return _methodCalled; }
            }
        }
    }
}