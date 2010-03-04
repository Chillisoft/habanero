using System;
using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestDateTimePickerMapperWin : TestDateTimePickerMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();

        }
        [Test]
        public void TestSetBusinessObjectValue_ChangesDateTimePickerImmediately_InWin()
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
            Assert.AreEqual(sampleBusinessObject.SampleDate, dateTimePicker.Value);
            //---------------Tear Down -------------------------          
        }

    }
}