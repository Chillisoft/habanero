using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestCheckBoxMapperVWG : TestCheckBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
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
}