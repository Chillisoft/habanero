using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestBOColMultiselectorWin :TestBOColSelector
    {
        private const string _gridIdColumnName = "HABANERO_OBJECTID";
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
        
        protected override IBOColSelectorControl CreateSelector()
        {
            MultiSelectorWin<MyBO> multiSelectorWin = new MultiSelectorWin<MyBO>(GetControlFactory());
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add(multiSelectorWin);
            return multiSelectorWin;
        }
    }
}
