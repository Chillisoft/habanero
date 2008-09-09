//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{

    public abstract class TestDateTimePickerMapper
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestDateTimePickerMapperWin : TestDateTimePickerMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
  
            }
            [Test]
            public void TestSetBusinessObjectValue_ChangesDateTimePicker_InWin()
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
                dtpMapper.UpdateControlValueFromBusinessObject();

                //---------------Test Result -----------------------
                Assert.AreEqual(sampleBusinessObject.SampleDate, dateTimePicker.Value);
                //---------------Tear Down -------------------------          
            }
            [Test]
            public void TestUpdateValueInPicker_ChangesValueInBO_ForWin()
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
                dtpMapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                Assert.AreEqual(newDate, sampleBusinessObject.SampleDate);
                //---------------Tear Down -------------------------          
            }
        }

        [TestFixture]
        public class TestDateTimePickerMapperVWG : TestDateTimePickerMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.VWG.ControlFactoryVWG();
                //return null;
            }
            [Test]
            public void TestSetBusinessObjectValue_DoesNotChangeDateTimePicker_InVWG()
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
            public void TestUpdateValueInPicker_DoesNot_ChangesValueInBO_ForVWG()
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
                dtpMapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                Assert.AreEqual(newDate, sampleBusinessObject.SampleDate);
                //---------------Tear Down -------------------------          
            }
        }

        [Test]
        public void TestCreateDateTimePickermapper()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            string propertyName = "SampleDateProperty";
            DateTimePickerMapper dtpMapper = new DateTimePickerMapper(dateTimePicker, propertyName, false, GetControlFactory());

            //---------------Verify Result -----------------------
            Assert.AreEqual(dateTimePicker, dtpMapper.DateTimePicker);
            Assert.AreEqual(propertyName, dtpMapper.PropertyName);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestSetBusinessObjectForDateTimePickerMapper()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            //---------------Verify test pack-------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value.Date);
            //---------------Execute Test ----------------------

            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            //---------------Tear Down -------------------------          
        }

        //TODO: Do tests for null value and changes from null value
        //TODO: Do tests for null business object
        //TODO: Test custom formats etc in DateTimePickerUtils



        //TODO: Fix readonly compulsory field for control mappper base class


        private IDateTimePicker GetDateTimePicker(out DateTimePickerMapper dtpMapper)
        {
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            string propertyName = "SampleDate";
            dtpMapper = new DateTimePickerMapper(dateTimePicker, propertyName, false, GetControlFactory());
            return dateTimePicker;
        }
    }
}
