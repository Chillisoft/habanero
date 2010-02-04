//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections;
using Habanero.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{

    public abstract class TestDateTimePickerMapper : TestMapperBase
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

        //BugFix: This test investigates the scenario where a bug_ was occuring
        [Test]
        public void TestSetBusinessObjectForDateTimePickerMapper_DoesntFirePropertyUpdate()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = DateTime.Now;
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            bool updatedEventFired = false;
            sampleBusinessObject.Props["SampleDate"].Updated += delegate {
                updatedEventFired = true;
            };
            //---------------Verify test pack-------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value.Date);
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            Assert.IsFalse(updatedEventFired);
        }

        //BugFix: This tests the scenario where a bug_ was occuring
        [Test]
        public void TestSetBusinessObjectForDateTimePickerMapper_Changed_DoesntFirePropertyUpdate()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = DateTime.Now;
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            bool updatedEventFired = false;
            sampleBusinessObject.Props["SampleDate"].Updated += delegate {
                updatedEventFired = true;
            };
            dtpMapper.BusinessObject = new Sample();
            //---------------Verify test pack-------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value.Date);
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            Assert.IsFalse(updatedEventFired);
        }

        //TODO: Do tests for null business object
        //TODO: Fix readonly compulsory field for control mappper base class
        //TODO: Have a look at if DateTimePickerUtils is needed anymore. I don't think it is.

        [Test]
        public void TestSetBusinessObjectValue_ChangesDateTimePickerValue()
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
        public void TestUpdateValueInPicker_ChangesValueInBO()
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


        [Test]
        public void TestSetBusinessObjectValue_ToNull_ChangesDateTimePickerValue()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            sampleBusinessObject.SampleDateNullable = DateTime.Today;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper, "SampleDateNullable");
            dtpMapper.BusinessObject = sampleBusinessObject;

            //---------------Verify test pack-------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value.Date);
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue, "DateTimePicker Should have a value");
            //---------------Execute Test ----------------------
            sampleBusinessObject.SampleDateNullable = null;
            dtpMapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue, "DateTimePicker Should not have a value");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestUpdateValueInPicker_ToNull_ChangesValueInBO()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDateNullable = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper, "SampleDateNullable");
            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Verify Preconditions -------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value.Date);
            Assert.IsTrue(sampleBusinessObject.SampleDateNullable.HasValue, "BusinessObject SampleDateNullable Should have a value");
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;
            dtpMapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsFalse(sampleBusinessObject.SampleDateNullable.HasValue, "BusinessObject SampleDateNullable Should not have a value");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetBusinessObjectValue_FromNull_ChangesDateTimePickerValue()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            sampleBusinessObject.SampleDateNullable = null;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper, "SampleDateNullable");
            dtpMapper.BusinessObject = sampleBusinessObject;

            //---------------Verify test pack-------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue, "DateTimePicker Should not have a value");
            //---------------Execute Test ----------------------
            DateTime newDate = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDateNullable = newDate;
            dtpMapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue, "DateTimePicker Should have a value");
            Assert.AreEqual(newDate, dateTimePicker.ValueOrNull.GetValueOrDefault(newDate.AddDays(1)));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestUpdateValueInPicker_FromNull_ChangesValueInBO()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            sampleBusinessObject.SampleDateNullable = null;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper, "SampleDateNullable");
            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Verify Preconditions -------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(sampleBusinessObject.SampleDateNullable.HasValue, "BusinessObject SampleDateNullable Should not have a value");
            //---------------Execute Test ----------------------
            DateTime newDate = new DateTime(2000, 1, 1);
            dateTimePicker.ValueOrNull = newDate;
            dtpMapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsTrue(sampleBusinessObject.SampleDateNullable.HasValue, "BusinessObject SampleDateNullable Should have a value");
            Assert.AreEqual(newDate, sampleBusinessObject.SampleDateNullable.GetValueOrDefault(newDate.AddDays(1)));
            //---------------Tear Down -------------------------          
        }
        
        [Test]
        public void TestAttribute_DateFormat()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = new DateTime(2000, 1, 2, 3, 4, 0);
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            dtpMapper.BusinessObject = sampleBusinessObject;
            Hashtable attributes = new Hashtable();
            string dateFormat = "dd MMM yyyy HH:mm";
            attributes.Add("dateFormat", dateFormat);
            //---------------Assert Precondition----------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            //---------------Execute Test ----------------------
            dtpMapper.SetPropertyAttributes(attributes);
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            Assert.AreEqual(DateTimePickerFormat.Custom, dateTimePicker.Format);
            Assert.AreEqual(dateFormat, dateTimePicker.CustomFormat);
        }

        [Test]
        public virtual void TestAttribute_ShowUpDown()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = new DateTime(2000, 1, 2, 3, 4, 0);
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            dtpMapper.BusinessObject = sampleBusinessObject;
            Hashtable attributes = new Hashtable();
            attributes.Add("showUpDown", "true");
            //---------------Assert Precondition----------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            Assert.IsFalse(dateTimePicker.ShowUpDown);
            //---------------Execute Test ----------------------
            dtpMapper.SetPropertyAttributes(attributes);
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value);
            Assert.IsTrue(dateTimePicker.ShowUpDown);
        }


        private IDateTimePicker GetDateTimePicker(out DateTimePickerMapper dtpMapper)
        {
            return GetDateTimePicker(out dtpMapper, "SampleDate");
        }

        private IDateTimePicker GetDateTimePicker(out DateTimePickerMapper dtpMapper, string propertyName)
        {
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            dtpMapper = new DateTimePickerMapper(dateTimePicker, propertyName, false, GetControlFactory());
            return dateTimePicker;
        }
    }
}
