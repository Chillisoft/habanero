using System;
using Habanero.Test.General;
using Habanero.Ui.Forms;
using MyTimePicker;
using NUnit.Framework;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestTimePickerMapper : TestMapperBase
    {
        private TimePicker itsTimePicker;
        private TimePickerMapper itsPickerMapper;
        private Sample s;

        public TestTimePickerMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            itsTimePicker = new TimePicker();
            itsPickerMapper = new TimePickerMapper(itsTimePicker, "SampleDate", false);
            s = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(itsTimePicker, itsPickerMapper.Control);
            Assert.AreSame("SampleDate", itsPickerMapper.PropertyName);
        }

        [Test]
        public void TestDateTimePickerValue()
        {
            s.SampleDate = new DateTime(2000, 1, 1, 6, 0, 0);
            itsPickerMapper.BusinessObject = s;
            Assert.AreEqual(new TimeSpan(6, 0, 0), itsTimePicker.Value, "Value is not set.");
            s.SampleDate = new DateTime(2001, 2, 2, 8, 0, 0);
            Assert.AreEqual(new TimeSpan(8, 0, 0), itsTimePicker.Value, "Value is not set after changing bo prop");
        }

        
        //Peter - the TimePicker we're using doesn't seem to fire the ValueChanged or Textchanged event
        // when you change the value from code, so this test cannot be performed.
        //[Test]
        //public void TestSettingDateTimePickerValueUpdatesBO()
        //{
        //    s.SampleDate = new DateTime(2000, 1, 1, 6, 0, 0);
        //    itsPickerMapper.BusinessObject = s;
        //    itsTimePicker.Value = new TimeSpan(10, 0, 0);
        //    Assert.AreEqual(new DateTime(2000, 1, 1, 10, 0, 0), s.SampleDate,
        //                    "BO property value isn't changed when datetimepicker value is changed.");
        //}

//		[Test]
//		public void TestDisplayingRelatedProperty() 
//		{
//			SetupClassDefs(new DateTime(2004, 1,1));
//			mapper = new DateTimePickerMapper(dte, "MyRelationship.MyRelatedTestProp", true) ;
//			mapper.BusinessObject = itsMyBo;
//			Assert.AreEqual(new DateTime(2004, 1,1), dte.Value ) ;
//		}
    }
}