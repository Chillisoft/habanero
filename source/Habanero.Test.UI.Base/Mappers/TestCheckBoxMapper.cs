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


using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestCheckBoxMapper.
    /// </summary>
    [TestFixture]
    public abstract class TestCheckBoxMapper : TestMapperBase
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestCheckBoxMapperWin : TestCheckBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            [Test, Ignore("To be implemented for windows")]
            public void TestCheckBox_Value_UpdatedwhenBusinessobjectUpdated()
            {
                //----------Setup test pack----------------------------
                _sampleBusinessObject.SampleBoolean = false;
                _mapper.BusinessObject = _sampleBusinessObject;
                //----------verify test pack --------------------------
                Assert.IsFalse(_cb.Checked);
                //----------Execute test ------------------------------
                _sampleBusinessObject.SampleBoolean = true;
                //----------verify test ------------------------------
                Assert.IsTrue(_cb.Checked);
            }

            [Test, Ignore("To be implemented for windows")]
            public void TestSettingCheckBoxCheckedUpdatesBO()
            {
                _sampleBusinessObject.SampleBoolean = false;
                _mapper.BusinessObject = _sampleBusinessObject;
                _cb.Checked = true;
                Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
            }
        }

        [TestFixture]
        public class TestCheckBoxMapperGiz : TestCheckBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            [Test]
            public void TestSettingCheckBoxCheckedUpdatesBO()
            {
                //----------Setup test pack----------------------------
                _sampleBusinessObject.SampleBoolean = false;
                _mapper.BusinessObject = _sampleBusinessObject;
                //----------verify test pack --------------------------
                //----------Execute test ------------------------------
                _cb.Checked = true;
                _mapper.ApplyChangesToBusinessObject();
                //----------verify test ------------------------------
                Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
            }
        }

        private ICheckBox _cb;
        private CheckBoxMapper _mapper;
        private Sample _sampleBusinessObject;

        [SetUp]
        public void SetupTest()
        {
            _cb = GetControlFactory().CreateCheckBox();
            _mapper = new CheckBoxMapper(_cb, "SampleBoolean", false);
            _sampleBusinessObject = new Sample();
        }


        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_cb, _mapper.Control);
            Assert.AreSame("SampleBoolean", _mapper.PropertyName);
        }


        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(true);
            _cb = GetControlFactory().CreateCheckBox();
            _mapper = new CheckBoxMapper(_cb, "MyRelationship.MyRelatedTestProp", true);
            _mapper.BusinessObject = itsMyBo;
            Assert.IsNotNull(_mapper.BusinessObject);
            Assert.AreEqual(true, _cb.Checked);
        }
    }
}