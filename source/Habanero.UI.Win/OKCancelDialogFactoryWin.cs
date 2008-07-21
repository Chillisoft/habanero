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
            return form;
        }

        private class OKCancelPanelWin : PanelWin, IOKCancelPanel
        {
            private readonly IControlFactory _controlFactory;
            private IButton _okButton;
            private IPanel _contentPanel;

            public OKCancelPanelWin(IControlFactory controlFactory)
            {
                _controlFactory = controlFactory;
                // create content panel
                _contentPanel = _controlFactory.CreatePanel();
                _contentPanel.Dock = DockStyle.Fill;
                this.Controls.Add((Control)_contentPanel);

                // create buttons
                IButtonGroupControl buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                buttonGroupControl.Dock = DockStyle.Bottom;
                _okButton = buttonGroupControl.AddButton("OK");
                _okButton.NotifyDefault(true);
                buttonGroupControl.AddButton("Cancel");
                this.Controls.Add((Control)buttonGroupControl);
            }

            public IButton OKButton
            {
                get { return _okButton; }
            }

            public IPanel ContentPanel
            {
                get { return _contentPanel; }
            }
        }
    }
}