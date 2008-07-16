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
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestCheckBoxMapper.
    /// </summary>
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

            [Test]
            public void TestCheckBox_Value_UpdatedwhenBusinessobjectUpdated()
            {
                //----------Setup test pack----------------------------
                _sampleBusinessObject.SampleBoolean = false;
                _mapper.BusinessObject = _sampleBusinessObject;
                //----------verify test pack --------------------------
                Assert.IsFalse(_cb.Checked);
                //----------Execute test ------------------------------
                _sampleBusinessObject.SampleBoolean = true;
                _mapper.ApplyChanges();
                //----------verify test ------------------------------
                Assert.IsTrue(_cb.Checked);
            }

            [Test]
            public void TestSettingCheckBoxCheckedUpdatesBO()
            {
                _sampleBusinessObject.SampleBoolean = false;
                _mapper.BusinessObject = _sampleBusinessObject;
                _cb.Checked = true;
                _mapper.ApplyChangesToBusinessObject();
                Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
            }

            [Test]
            public void TestCheckBoxHasCorrectStrategy()
            {
                //---------------Test Result -----------------------
                Assert.AreSame(typeof(CheckBoxStrategyWin),_mapper.GetStrategy().GetType());
            }


            //TODO: Check the strategy is correct and how to test using NUnit Forms
            [Test]
            public void TestClickingOfCheckBoxUpdatesBO()
            {
                //---------------Set up test pack-------------------
                _cb.Name = "TestCheckBox";
                _cb.Checked = false;
                _sampleBusinessObject.SampleBoolean = false;
                _mapper.BusinessObject = _sampleBusinessObject;
                Form frm = AddControlToForm(_cb);
                //---------------Execute Test ----------------------
                frm.Show();
                CheckBoxTester box = new CheckBoxTester("TestCheckBox");
                box.Click();
                box.Check();
                //---------------Test Result -----------------------
                Assert.IsTrue(_cb.Checked);
                Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
                //---------------Tear down -------------------------
            }
            private Form AddControlToForm(IControlChilli parentControl)
            {
                Form frm = new Form();
                frm.Controls.Clear();
                frm.Controls.Add((Control)parentControl);
                return frm;
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
            _mapper = new CheckBoxMapper(_cb, "SampleBoolean", false, GetControlFactory());
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
            _mapper = new CheckBoxMapper(_cb, "MyRelationship.MyRelatedTestProp", true, GetControlFactory());
            _mapper.BusinessObject = itsMyBo;
            Assert.IsNotNull(_mapper.BusinessObject);
            Assert.AreEqual(true, _cb.Checked);
        }
    }
}