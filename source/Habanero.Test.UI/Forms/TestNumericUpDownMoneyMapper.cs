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


using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestNumericUpDownMoneyMapper : TestMapperBase
    {
        private NumericUpDown itsNumUpDown;
        private NumericUpDownMoneyMapper mapper;
        private Sample s;

        public TestNumericUpDownMoneyMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            itsNumUpDown = new NumericUpDown();
            mapper = new NumericUpDownMoneyMapper(itsNumUpDown, "SampleMoney", false);
            s = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(itsNumUpDown, mapper.Control);
            Assert.AreSame("SampleMoney", mapper.PropertyName);
            Assert.AreEqual(2, itsNumUpDown.DecimalPlaces);
            Assert.AreEqual(int.MinValue, itsNumUpDown.Minimum);
            Assert.AreEqual(int.MaxValue, itsNumUpDown.Maximum);
        }

        [Test]
        public void TestControlValue()
        {
            s.SampleMoney = (decimal) 10.32;
            mapper.BusinessObject = s;
            Assert.AreEqual(10.32, itsNumUpDown.Value, "Value is not set.");
            s.SampleMoney = (decimal) 20.56;
            Assert.AreEqual(20.56, itsNumUpDown.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingControlValueUpdatesBO()
        {
            s.SampleMoney = (decimal) 13.45;
            mapper.BusinessObject = s;
            itsNumUpDown.Value = (decimal) 15.67;
            Assert.AreEqual(15.67, s.SampleMoney, "BO property value isn't changed when numupdown value is changed.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("4.32");
            mapper = new NumericUpDownMoneyMapper(itsNumUpDown, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual(4.32, itsNumUpDown.Value);
        }

        [Test]
        public void TestSettingAttributes()
        {
            Hashtable attributes = new Hashtable();
            attributes.Add("decimalPlaces", "4");
            mapper.SetPropertyAttributes(attributes);
            Assert.AreEqual(4, itsNumUpDown.DecimalPlaces);
        }
    }
}