using System;
using System.Windows.Forms;
using Habanero.UI.Base;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.Win
{
    public class OKCancelDialogFactoryWin : IOKCancelDialogFactory
    {
        private readonly IControlFactory _controlFactory;

        public OKCancelDialogFactoryWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public IOKCancelPanel CreateOKCancelPanel(IControlChilli nestedControl)
        {
            OKCancelPanelWin mainPanel = new OKCancelPanelWin(_controlFactory);
            mainPanel.ContentPanel.Controls.Add(nestedControl);
            nestedControl.Dock = DockStyle.Fill;
            return mainPanel;
        }

        public IFormChilli CreateOKCancelForm(IControlChilli nestedControl)
        {
            FormWin form = (FormWin) _controlFactory.CreateForm();
            IOKCancelPanel mainPanel = CreateOKCancelPanel(nestedControl);
            mainPanel.Dock = DockStyle.Fill;
            form.Controls.Add((Control) mainPanel);
            form.AcceptButton = (IButtonControl) mainPanel.OKButton;
            mainPanel.OKButton.Click += delegate
                    {
                        OkButton_ClickHandler(form);
                    };
            mainPanel.CancelButton.Click += delegate
                    {
                        CancelButton_ClickHandler(form);
                    };
            return form;
        }

        public void CancelButton_ClickHandler(FormWin form)
        {
            form.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            form.Close();
        }

        public void OkButton_ClickHandler(FormWin form)
        {
            form.DialogResult = System.Windows.Forms.DialogResult.OK;
            form.Close();
        }

        private class OKCancelPanelWin : PanelWin, IOKCancelPanel
        {
            private readonly IControlFactory _controlFactory;
            private IButton _okButton;
            private IPanel _contentPanel;
            private IButton _cancelButton;

            public OKCancelPanelWin(IControlFactory controlFactory)
            {
                _controlFactory = controlFactory;
                // create content panel
                _contentPanel = _controlFactory.CreatePanel();
                _contentPanel.Dock = DockStyle.Fill;
                this.Controls.Add((Control) _contentPanel);

                // create buttons
                IButtonGroupControl buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                buttonGroupControl.Dock = DockStyle.Bottom;
                _okButton = buttonGroupControl.AddButton("OK");
                _okButton.NotifyDefault(true);
                _cancelButton = buttonGroupControl.AddButton("Cancel");
                this.Controls.Add((Control) buttonGroupControl);
            }

            public IButton OKButton
            {
                get { return _okButton; }
            }

            public IButton CancelButton
            {
                get { return _cancelButton; }
            }

            public IPanel ContentPanel
            {
                get { return _contentPanel; }
            }
        }
    }
}