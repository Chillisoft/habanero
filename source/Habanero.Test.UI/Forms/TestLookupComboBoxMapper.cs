//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestLookupComboBoxMapper : TestUsingDatabase
    {
        private ComboBox _comboBox;
        private LookupComboBoxMapper _lookupComboBoxMapper;
        private Sample _sample;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            _comboBox = new ComboBox();
            _lookupComboBoxMapper = new LookupComboBoxMapper(_comboBox, "SampleLookupID", false);
            _sample = new Sample();
            _lookupComboBoxMapper.SetLookupList(Sample.LookupCollection);

        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_comboBox, _lookupComboBoxMapper.Control);
            Assert.AreSame("SampleLookupID", _lookupComboBoxMapper.PropertyName);
        }

        [Test]
        public void TestSetLookupList()
        {
            Assert.AreEqual(4, _comboBox.Items.Count);
            Assert.AreSame(typeof (string), _comboBox.Items[0].GetType());
            Assert.IsTrue(_comboBox.Items.Contains("Test1"));
        }

        [Test]
        public void TestComboBoxValue()
        {
            _sample.SampleLookupID = new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}");
            _lookupComboBoxMapper.BusinessObject = _sample;
            Assert.AreEqual("Test1", (string)_comboBox.SelectedItem, "Value is not set.");
            _sample.SampleLookupID = new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}");
            Assert.AreEqual("Test2", (string)_comboBox.SelectedItem, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingComboValueUpdatesBO()
        {
            _sample.SampleLookupID = new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}");
            _lookupComboBoxMapper.BusinessObject = _sample;
            _comboBox.SelectedIndex = 2;
            string selected = (string) _comboBox.SelectedItem;
            Assert.AreEqual(Sample.LookupCollection[selected], _sample.SampleLookupID,
                            "BO property value isn't changed when control value is changed.");
        }

//		[Test, ExpectedException(typeof (LookupListNotSetException), "You must set the lookup list before using a control that requires it.")]
//		public void TestNotSettingLookupList() {
//			_comboBox = new ComboBox();
//			_lookupComboBoxMapper = new LookupComboBoxMapper(_comboBox, "SampleLookupID", false);
//			_sample.SampleLookupID = Sample.LookupCollection[0].Id;
//			_lookupComboBoxMapper.BusinessObject = _sample;
//		}

        [Test]
        public void TestUsingPropWithLookupSource()
        {
            _comboBox = new ComboBox();
            _lookupComboBoxMapper = new LookupComboBoxMapper(_comboBox, "SampleLookup2ID", false);
            _sample = new Sample();
            _sample.SetPropertyValue("SampleLookup2ID", new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}"));
            _lookupComboBoxMapper.BusinessObject = _sample;
            Assert.AreEqual(4, _comboBox.Items.Count);
            Assert.AreSame(typeof (string), _comboBox.Items[0].GetType());
            Assert.IsTrue(_comboBox.Items.Contains("Test1"));
            Assert.AreEqual("Test2", (string) _comboBox.SelectedItem);
        }

        [Test]
        public void TestUsingBOLookupList()
        {
            _comboBox = new ComboBox();
            _lookupComboBoxMapper = new LookupComboBoxMapper(_comboBox, "SampleLookup2ID", false);
            _lookupComboBoxMapper.SetLookupList(Sample.BOLookupCollection);
            _sample = new Sample();
            _sample.SetPropertyValue("SampleLookup2ID", Sample.BOLookupCollection["Test2"]);
            _lookupComboBoxMapper.BusinessObject = _sample;
            Assert.AreEqual(4, _comboBox.Items.Count);
            Assert.AreSame(typeof (string), _comboBox.Items[0].GetType());
            Assert.IsTrue(_comboBox.Items.Contains("Test1"));
            Assert.AreEqual("Test2", (string) _comboBox.SelectedItem);
        }

        [Test]
        public void TestUsingBOLookupListStr()
        {
            _comboBox = new ComboBox();
            _lookupComboBoxMapper = new LookupComboBoxMapper(_comboBox, "SampleLookup3ID", false);
            _lookupComboBoxMapper.SetLookupList(Sample.BOLookupCollection);
            _sample = new Sample();
            _sample.SetPropertyValue("SampleLookup3ID", Sample.BOLookupCollection["Test2"]);
            _lookupComboBoxMapper.BusinessObject = _sample;
            Assert.AreEqual(4, _comboBox.Items.Count);
            Assert.AreSame(typeof(string), _comboBox.Items[0].GetType());
            Assert.IsTrue(_comboBox.Items.Contains("Test1"));
            Assert.AreEqual("Test2", (string)_comboBox.SelectedItem);
        }
    }
}