using Habanero.Base;
using Habanero.Test.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestBOEditorControlVWG : TestBOEditorControl
    {
        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            return new BOEditorControlVWG(classDef);
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef def, string uiDefName)
        {
            return new BOEditorControlVWG(controlFactory, def, uiDefName);
        }

        [Test]
        public override void Test_IfValidState_WhenSetControlValueToInvalidValue_ShouldUpdatesErrorProviders()
        {
            //The error provider is not refreshed immediately in VWG
            //Modify test to do an Update
        }

        [Test]
        public override void Test_HasErrors_WhenBOValid_ButCompulsorytFieldSetToNull_ShouldBeTrue()
        {
            //The error provider is not refreshed immediately in VWG
            //Modify test to do an Update
        }
    }

    [TestFixture]
    public class TestBOEditorControlVWG_Generic : TestBOEditorControlVWG
    {
        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            if (classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlVWG<OrganisationTestBO>();
            }
            return new BOEditorControlVWG<ContactPersonTestBO>();
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlVWG<OrganisationTestBO>(controlFactory, uiDefName);
            }
            return new BOEditorControlVWG<ContactPersonTestBO>(controlFactory, uiDefName);
        }
        [Test]
        public override void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            //Not relevant for a Generic since the ClassDef is implied from the Generic Type
        }
    }
}