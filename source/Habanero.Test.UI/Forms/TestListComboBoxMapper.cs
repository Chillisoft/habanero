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

using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestListComboBoxMapper : TestUsingDatabase
    {
        private ComboBox cbx;
        private ListComboBoxMapper mapper;
        private Sample s;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            cbx = new ComboBox();
            mapper = new ListComboBoxMapper(cbx, "SampleText", false);
            s = new Sample();
            mapper.SetList("One|Two|Three|Four", false);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(cbx, mapper.Control);
            Assert.AreSame("SampleText", mapper.PropertyName);
        }

        [Test]
        public void TestSetList()
        {
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof(string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Two"));
        }

        [Test]
        public void TestComboBoxValue()
        {
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            Assert.AreEqual("Three", cbx.SelectedItem, "Value is not set.");
            s.SampleText = "Four";
            Assert.AreEqual("Four", cbx.SelectedItem, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingComboValueUpdatesBO()
        {
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            cbx.SelectedIndex = 0;
            //string selected = (string)cbx.SelectedItem;
            Assert.AreEqual(cbx.SelectedItem, s.SampleText, "BO property value isn't changed when control value is changed.");
        }
    }
}