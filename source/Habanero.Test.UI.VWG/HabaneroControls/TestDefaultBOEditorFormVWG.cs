using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestDefaultBOEditorFormVWG : TestDefaultBOEditorForm
    {

        protected override void ShowFormIfNecessary(IFormHabanero form)
        {

        }

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new Habanero.UI.VWG.ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        [Ignore("This cannot be tested for VWG because you cannot show a form to close it")]
        public override void Test_CloseForm_ShouldCallDelegateWithCorrectInformation()
        {
        }
    }
}