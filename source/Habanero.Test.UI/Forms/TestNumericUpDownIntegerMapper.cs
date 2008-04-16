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


using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestDateTimeEditorMapper.
    /// </summary>
    [TestFixture]
    public class TestNumericUpDownIntegerMapper : TestMapperBase
    {
        private NumericUpDown itsNumUpDown;
        private NumericUpDownIntegerMapper mapper;
        private Sample s;

        public TestNumericUpDownIntegerMapper()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            itsNumUpDown = new NumericUpDown();
            mapper = new NumericUpDownIntegerMapper(itsNumUpDown, "SampleInt", false);
            s = new Sample();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(itsNumUpDown, mapper.Control);
            Assert.AreSame("SampleInt", mapper.PropertyName);
            Assert.AreEqual(0, itsNumUpDown.DecimalPlaces);
            Assert.AreEqual(int.MinValue, itsNumUpDown.Minimum);
            Assert.AreEqual(int.MaxValue, itsNumUpDown.Maximum);
        }

        [Test]
        public void TestControlValue()
        {
            s.SampleInt = 10;
            mapper.BusinessObject = s;
            Assert.AreEqual(10, itsNumUpDown.Value, "Value is not set.");
            s.SampleInt = 20;
            Assert.AreEqual(20, itsNumUpDown.Value, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingControlValueUpdatesBO()
        {
            s.SampleInt = 12;
            mapper.BusinessObject = s;
            itsNumUpDown.Value = 13;
            Assert.AreEqual(13, s.SampleInt, "BO property value isn't changed when numupdown value is changed.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("4");
            mapper = new NumericUpDownIntegerMapper(itsNumUpDown, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.AreEqual(4, itsNumUpDown.Value);
        }
    }
}