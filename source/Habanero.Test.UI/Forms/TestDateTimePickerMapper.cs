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
using System.Windows.Forms;
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