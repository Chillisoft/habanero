using System;
using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestDateTimePickerMapperVWG : TestDateTimePickerMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
            //return null;
        }

        [Test, Ignore("ShowUpDown property does not exist for VWG : June 2008")]
        public override void TestAttribute_ShowUpDown()
        {
            base.TestAttribute_ShowUpDown();
        }

        [Test]
        public void TestSetBusinessObjectValue_DoesNotChangeDateTimePickerImmediately_InVWG()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            sampleBusinessObject.SampleDate = DateTime.Today;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            dtpMapper.BusinessObject = sampleBusinessObject;

            //---------------Verify test pack-------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value.Date);
            //---------------Execute Test ----------------------
            DateTime testDateChangedValue = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDate = testDateChangedValue;

            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestUpdateValueInPicker_DoesNotChangeValueInBO_ForVWG()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Verify Preconditions -------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value.Date);
            //---------------Execute Test ----------------------
            DateTime newDate = DateTime.Today.AddDays(+3);
            dateTimePicker.Value = newDate;
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, sampleBusinessObject.SampleDate);
            //---------------Tear Down -------------------------          
        }

    }
}