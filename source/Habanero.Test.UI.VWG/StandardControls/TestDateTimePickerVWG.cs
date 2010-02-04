using System;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.StandardControls
{
    [TestFixture]
    public class TestDateTimePickerVWG : TestDateTimePicker
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override EventArgs GetKeyDownEventArgsForDeleteKey()
        {
            return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.Delete);
        }

        protected override EventArgs GetKeyDownEventArgsForBackspaceKey()
        {
            return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.Back);
        }

        protected override EventArgs GetKeyDownEventArgsForOtherKey()
        {
            return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.A);
        }

        protected override void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value)
        {
            Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
            picker.Value = value;
        }

        protected override void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value)
        {
            Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
            picker.Checked = value;
        }

    }
}