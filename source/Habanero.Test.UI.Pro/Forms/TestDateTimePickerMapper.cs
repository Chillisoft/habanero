using System;
using System.Windows.Forms;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestDateTimePickerMapper : TestMapperBase
    {
        private DateTimePicker _dateTimePicker;
        private DateTimePickerMapper _dateTimePickerMapper;
        private Sample _sample;

        public TestDateTimePickerMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            _dateTimePicker = new DateTimePicker();
            _dateTimePickerMapper = new DateTimePickerMapper(_dateTimePicker, "SampleDate", false);
            _sample = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_dateTimePicker, _dateTimePickerMapper.Control);
            Assert.AreSame("SampleDate", _dateTimePickerMapper.PropertyName);
        }

        [Test]
        public void TestDateTimePickerValue()
        {
            _sample.SampleDate = new DateTime(2000, 1, 1);
            _dateTimePickerMapper.BusinessObject = _sample;
            Assert.AreEqual(new DateTime(2000, 1, 1), _dateTimePicker.Value, "Value is not set.");
            _sample.SampleDate = new DateTime(2001, 2, 2);
            Assert.AreEqual(new DateTime(2001, 2, 2), _dateTimePicker.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingDateTimePickerValueUpdatesBO()
        {
            _sample.SampleDate = new DateTime(2000, 1, 1);
            _dateTimePickerMapper.BusinessObject = _sample;
            _dateTimePicker.Value = new DateTime(2002, 3, 3);
            Assert.AreEqual(new DateTime(2002, 3, 3), _sample.SampleDate,
                            "BO property value isn't changed when datetimepicker value is changed.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(new DateTime(2004, 1, 1));
            _dateTimePickerMapper = new DateTimePickerMapper(_dateTimePicker, "MyRelationship.MyRelatedTestProp", true);
            _dateTimePickerMapper.BusinessObject = itsMyBo;
            Assert.AreEqual(2004, _dateTimePicker.Value.Year);
            Assert.AreEqual(1, _dateTimePicker.Value.Month);
            Assert.AreEqual(1, _dateTimePicker.Value.Day);
        }
    }
}