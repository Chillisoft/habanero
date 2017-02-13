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

using Habanero.Base;
using Habanero.Base.DataMappers;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.Util
{
    public class SimpleValueObjectStub: SimpleValueObject{
        private readonly object _value;

        public SimpleValueObjectStub(object value, bool isLoading) : base(value, isLoading)
        {
            Successful = true;
            _value = value;
        }
        public SimpleValueObjectStub(object value): this(value, false)
        {
            
        }

        public bool Successful { get; set; }
        public string FailMessage { get; set; }

        public override object GetPersistValue()
        {
            return _value;
        }

        public override Result IsValid()
        {
            return new Result(Successful, FailMessage);
        }

        public override string ToString()
        {
            return this._value == null? "": this._value.ToString();
        }
    }
    /// <summary>
    /// This Test Class tests the functionality of the valueObject custom property class.
    /// </summary>
    [TestFixture]
    public class TestValueObject
    {
        private readonly IClassDef _itsClassDef;

        public TestValueObject()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            _itsClassDef = loader.LoadClass
                (@"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" type=""Habanero.Test.Util.SimpleValueObjectStub"" assembly=""Habanero.Test"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(_itsClassDef);
        }

        [Test]
        public void TestLoadingConstructor()
        {
            var valueObject = new SimpleValueObjectStub("test");
            Assert.IsTrue(valueObject.ToString().Equals("test"));
        }

        [Test]
        public void TestToString()
        {
            var valueObject = new SimpleValueObjectStub("test");
            Assert.AreEqual("test", valueObject.ToString());
        }

        [Test]
        public void TestPropertyType()
        {
            var propDef = (PropDef) _itsClassDef.PropDefcol["TestProp"];
            Assert.AreEqual(propDef.PropertyType, typeof(SimpleValueObjectStub));
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty()
        {
            //---------------Set up test pack-------------------
            var valueObject = new SimpleValueObjectStub("test");
            GeneralDataMapper generalDataMapper = new GeneralDataMapper(typeof(SimpleValueObjectStub));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(valueObject, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.AreSame(valueObject, returnValue);
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty_InheritedCustomProperty()
        {
            //---------------Set up test pack-------------------
            var valueObject = new SimpleValueObjectStub("test");
            var generalDataMapper = new GeneralDataMapper(typeof(CustomProperty));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(valueObject, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.AreSame(valueObject, returnValue);
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty_StringValue()
        {
            //---------------Set up test pack-------------------
            const string test = "test";
            var generalDataMapper = new GeneralDataMapper(typeof(SimpleValueObjectStub));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(test, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.IsInstanceOf(typeof(SimpleValueObjectStub), returnValue);
            var valueObject = (SimpleValueObjectStub) returnValue;
            Assert.AreSame(test, valueObject.ToString());
        }

        [Test]
        public void TestPropertyValue()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            var valueObject = new SimpleValueObjectStub("test");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bo.SetPropertyValue("TestProp", valueObject);
            object actualValue = bo.GetPropertyValue("TestProp");

            //---------------Test Result -----------------------
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOf(typeof(SimpleValueObjectStub), actualValue);
            Assert.AreSame(valueObject, actualValue);
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof(SimpleValueObjectStub), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValue("TestProp").ToString());
        }

        [Test]
        public void TestSqlFormatter_PrepareValue_ShouldReturnValue()
        {
            var valueObject = new SimpleValueObjectStub("test");
            var sqlFormatter = new SqlFormatter("'", "'", "f", "yh");
            var preparedValue = sqlFormatter.PrepareValue(valueObject);
            Assert.AreEqual("test", preparedValue.ToString());
        }

        [Test]
        public void Test_PropDefIsValid_WhenIsValidTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var propDef = (PropDef)_itsClassDef.PropDefcol["TestProp"];
            var valueObject = new SimpleValueObjectStub("test");
            //---------------Assert Precondition----------------
            Assert.IsTrue(valueObject.IsValid().Successful);
            //---------------Execute Test ----------------------
            string message = null;
            var isValueValid = propDef.IsValueValid(valueObject, ref message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValueValid);
        }
        [Test]
        public void Test_PropDefIsValid_WhenIsValidFalse_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var propDef = (PropDef)_itsClassDef.PropDefcol["TestProp"];
            var valueObject = new SimpleValueObjectStub("test") {Successful = false, FailMessage = TestUtil.GetRandomString()};
            //---------------Assert Precondition----------------
            Assert.IsFalse(valueObject.IsValid().Successful);
            //---------------Execute Test ----------------------
            string message = null;
            var isValueValid = propDef.IsValueValid(valueObject, ref message);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValueValid);
            Assert.AreEqual(valueObject.FailMessage, message);
        }
    }
}
// ReSharper restore InconsistentNaming