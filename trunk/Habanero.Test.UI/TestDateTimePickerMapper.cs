using System;
using System.Windows.Forms;
using Habanero.Test.General;
using Habanero.Ui.Forms;
using NUnit.Framework;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestDateTimePickerMapper : TestMapperBase
    {
        private DateTimePicker dte;
        private DateTimePickerMapper mapper;
        private Sample s;

        public TestDateTimePickerMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            dte = new DateTimePicker();
            mapper = new DateTimePickerMapper(dte, "SampleDate", false);
            s = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(dte, mapper.Control);
            Assert.AreSame("SampleDate", mapper.PropertyName);
        }

        [Test]
        public void TestDateTimePickerValue()
        {
            s.SampleDate = new DateTime(2000, 1, 1);
            mapper.BusinessObject = s;
            Assert.AreEqual(new DateTime(2000, 1, 1), dte.Value, "Value is not set.");
            s.SampleDate = new DateTime(2001, 2, 2);
            Assert.AreEqual(new DateTime(2001, 2, 2), dte.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingDateTimePickerValueUpdatesBO()
        {
            s.SampleDate = new DateTime(2000, 1, 1);
            mapper.BusinessObject = s;
            dte.Value = new DateTime(2002, 3, 3);
            Assert.AreEqual(new DateTime(2002, 3, 3), s.SampleDate,
                            "BO property value isn't changed when datetimepicker value is changed.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(new DateTime(2004, 1, 1));
            mapper = new DateTimePickerMapper(dte, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual(new DateTime(2004, 1, 1), dte.Value);
        }
    }
}