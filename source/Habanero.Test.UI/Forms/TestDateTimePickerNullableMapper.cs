using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestDateTimePickerNullableMapper: TestMapperBase
    {
        private DateTimePicker _dateTimePicker;
        private DateTimePickerNullableMapper _dateTimePickerNullableMapper;
        private Sample _sample;

        public TestDateTimePickerNullableMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            _dateTimePicker = new DateTimePicker();
            _dateTimePickerNullableMapper = new DateTimePickerNullableMapper(_dateTimePicker, "SampleDateNullable", false);
            _sample = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_dateTimePicker, _dateTimePickerNullableMapper.Control);
            Assert.AreSame("SampleDateNullable", _dateTimePickerNullableMapper.PropertyName);
        }

        [Test]
        public void TestDateTimePickerValue()
        {
            _sample.SampleDateNullable = new DateTime(2000, 1, 1);
            _dateTimePickerNullableMapper.BusinessObject = _sample;
            Assert.AreEqual(new DateTime(2000, 1, 1), _dateTimePicker.Value, "Value is not set.");
            _sample.SampleDateNullable = new DateTime(2001, 2, 2);
            Assert.AreEqual(new DateTime(2001, 2, 2), _dateTimePicker.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestDateTimePickerValueNullable()
        {
            _sample.SampleDateNullable = new DateTime(2000, 1, 1);
            _dateTimePickerNullableMapper.BusinessObject = _sample;
            Assert.AreEqual(new DateTime(2000, 1, 1), _dateTimePicker.Value, "Value is not set.");
            _sample.SampleDateNullable = null;
            Assert.AreEqual(null, _dateTimePickerNullableMapper.DateTimePickerController.Value, "Value is not set after changing bo prop to null");
        }

        [Test]
        public void TestSettingDateTimePickerValueUpdatesBO()
        {
            _sample.SampleDateNullable = new DateTime(2000, 1, 1);
            _dateTimePickerNullableMapper.BusinessObject = _sample;
            _dateTimePicker.Value = new DateTime(2002, 3, 3);
            Assert.AreEqual(new DateTime(2002, 3, 3), _sample.SampleDateNullable,
                            "BO property value isn't changed when datetimepicker value is changed.");
        }

        [Test]
        public void TestSettingDateTimePickerValueUpdatesBONullable()
        {
            _sample.SampleDateNullable = new DateTime(2000, 1, 1);
            _dateTimePickerNullableMapper.BusinessObject = _sample;
            _dateTimePickerNullableMapper.DateTimePickerController.Value = null;
            Assert.AreEqual(null, _sample.SampleDateNullable,
                            "BO property value isn't changed when datetimepicker value is changed to null.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(new DateTime(2004, 1, 1));
            _dateTimePickerNullableMapper = new DateTimePickerNullableMapper(_dateTimePicker, "MyRelationship.MyRelatedTestProp", true);
            _dateTimePickerNullableMapper.BusinessObject = itsMyBo;
            Assert.AreEqual(2004, _dateTimePicker.Value.Year);
            Assert.AreEqual(1, _dateTimePicker.Value.Month);
            Assert.AreEqual(1, _dateTimePicker.Value.Day);
        }
    }
}