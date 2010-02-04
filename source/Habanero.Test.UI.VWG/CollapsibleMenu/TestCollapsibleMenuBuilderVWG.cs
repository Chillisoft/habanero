using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleMenuBuilderVWG : TestCollapsibleMenuBuilder
    {
        protected override IMenuBuilder CreateMenuBuilder()
        {
            return new CollapsibleMenuBuilderVWG(GetControlFactory());
        }

        protected override IFormControlStub CreateFormControlStub()
        {
            return new FormControlStubVWG();
        }

        private class FormControlStubVWG : UserControlVWG, IFormControlStub
        {
            public void SetForm(IFormHabanero form)
            {
                SetFormCalled = true;
                SetFormArgument = form;
            }

            public IFormHabanero SetFormArgument { get; private set; }

            public bool SetFormCalled { get; private set; }
        }

        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}