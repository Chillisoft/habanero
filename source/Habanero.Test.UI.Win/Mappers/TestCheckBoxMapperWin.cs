using System.Windows.Forms;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
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
            _mapper.UpdateControlValueFromBusinessObject();
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
            Assert.AreSame(typeof(CheckBoxStrategyWin), _mapper.GetStrategy().GetType());
        }


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
        private static Form AddControlToForm(IControlHabanero parentControl)
        {
            Form frm = new Form();
            frm.Controls.Clear();
            frm.Controls.Add((Control)parentControl);
            return frm;
        }
    }
}