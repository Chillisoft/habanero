using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestStaticDataEditorWin : TestStaticDataEditor
    {
        protected override IStaticDataEditor CreateEditorOnForm(out IFormHabanero frm)
        {
            frm = GetControlFactory().CreateForm();
            IStaticDataEditor editor = GetControlFactory().CreateStaticDataEditor();
            frm.Controls.Add(editor);
            frm.Show();
            return editor;
        }

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override void TearDownForm(IFormHabanero frm)
        {
            frm.Close();
            frm.Dispose();
        }
    }
}