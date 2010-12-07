using System;
using System.Collections;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.Util
{
    public class ValueObjectStub: ValueObject{
        private readonly object _value;

        public ValueObjectStub(object value, bool isLoading) : base(value, isLoading)
        {
            Successful = true;
            _value = value;
        }
        public ValueObjectStub(object value): this(value, false)
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
					<property  name=""TestProp"" type=""Habanero.Test.Util.ValueObjectStub"" assembly=""Habanero.Test"" />
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
            var valueObject = new ValueObjectStub("test");
            Assert.IsTrue(valueObject.ToString().Equals("test"));
        }


        [Test]
        public void TestToString()
        {
            var valueObject = new ValueObjectStub("test");
            Assert.AreEqual("test", valueObject.ToString());
        }

        [Test]
        public void TestPropertyType()
        {
            var propDef = (PropDef) _itsClassDef.PropDefcol["TestProp"];
            Assert.AreEqual(propDef.PropertyType, typeof(ValueObjectStub));
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty()
        {
            //---------------Set up test pack-------------------
            var propDef = new PropDef("Name", typeof(ValueObjectStub), PropReadWriteRule.ReadWrite, null);
            var valueObject = new ValueObjectStub("test");
            BOPropGeneralDataMapper generalDataMapper = new BOPropGeneralDataMapper(propDef);
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
            var propDef = new PropDef("Name", typeof(CustomProperty), PropReadWriteRule.ReadWrite, null);
            var valueObject = new ValueObjectStub("test");
            var generalDataMapper = new BOPropGeneralDataMapper(propDef);
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
            var propDef = new PropDef("Name", typeof(ValueObjectStub), PropReadWriteRule.ReadWrite, null);
            const string test = "test";
            var generalDataMapper = new BOPropGeneralDataMapper(propDef);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(test, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.IsInstanceOf(typeof(ValueObjectStub), returnValue);
            var valueObject = (ValueObjectStub) returnValue;
            Assert.AreSame(test, valueObject.ToString());
        }

        [Test]
        public void TestPropertyValue()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            var valueObject = new ValueObjectStub("test");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bo.SetPropertyValue("TestProp", valueObject);
            object actualValue = bo.GetPropertyValue("TestProp");

            //---------------Test Result -----------------------
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOf(typeof(ValueObjectStub), actualValue);
            Assert.AreSame(valueObject, actualValue);
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof(ValueObjectStub), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValue("TestProp").ToString());
        }

        [Test]
        public void TestSqlFormatter_PrepareValue_ShouldReturnValue()
        {
            var valueObject = new ValueObjectStub("test");
            var sqlFormatter = new SqlFormatter("'", "'", "f", "yh");
            var preparedValue = sqlFormatter.PrepareValue(valueObject);
            Assert.AreEqual("test", preparedValue.ToString());
        }

        [Test]
        public void Test_PropDefIsValid_WhenIsValidTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var propDef = (PropDef)_itsClassDef.PropDefcol["TestProp"];
            var valueObject = new ValueObjectStub("test");
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
            var valueObject = new ValueObjectStub("test") {Successful = false, FailMessage = TestUtil.GetRandomString()};
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