using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.StandardControls
{
    [TestFixture]
    public class TestComboBoxVWG : TestComboBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override string GetUnderlyingAutoCompleteSourceToString(IComboBox controlHabanero)
        {
            Gizmox.WebGUI.Forms.ComboBox control = (Gizmox.WebGUI.Forms.ComboBox)controlHabanero;
            return control.AutoCompleteSource.ToString();
        }
    }
}