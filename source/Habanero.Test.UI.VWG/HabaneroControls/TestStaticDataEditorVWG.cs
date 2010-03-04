using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestStaticDataEditorVWG : TestStaticDataEditor
    {
        protected override IStaticDataEditor CreateEditorOnForm(out IFormHabanero frm)
        {
            frm = GetControlFactory().CreateForm();
            IStaticDataEditor editor = GetControlFactory().CreateStaticDataEditor();
            frm.Controls.Add(editor);
            //frm.Show();
            return editor;
        }

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override void TearDownForm(IFormHabanero frm)
        {

        }

        [Test, Ignore("Does not work because VWG form cannot be shown")]
        public override void TestSelectSection()
        {
            base.TestSelectSection();
        }

        [Test, Ignore("Does not work because VWG form cannot be shown")]
        public override void TestSaveChanges()
        {
            base.TestSaveChanges();
        }

        [Test, Ignore("Does not work because VWG form cannot be shown")]
        public override void TestRejectChanges()
        {
            base.TestRejectChanges();
        }
    }
}