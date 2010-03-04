using Habanero.Base;
using Habanero.Test.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestBOEditorControlWin : TestBOEditorControl
    {
        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            return new BOEditorControlWin(classDef);
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef def, string uiDefName)
        {
            return new BOEditorControlWin(controlFactory, def, uiDefName);
        }
    }

    [TestFixture]
    public class TestBOEditorControlWin_Generic : TestBOEditorControlWin
    {
        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            if (classDef != null && classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlWin<OrganisationTestBO>();
            }
            return new BOEditorControlWin<ContactPersonTestBO>();
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (classDef != null && classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlWin<OrganisationTestBO>(controlFactory, uiDefName);
            }
            return new BOEditorControlWin<ContactPersonTestBO>(controlFactory, uiDefName);
        }

        [Test]
        public override void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            //Not relevant for a Generic since the ClassDef is implied from the Generic Type
        }

    }
}