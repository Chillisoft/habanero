using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.WebGUI
{
    public class OKCancelDialogFactoryGiz : IOKCancelDialogFactory
    {
        private readonly IControlFactory _controlFactory;

        public OKCancelDialogFactoryGiz(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public IOKCancelPanel CreateOKCancelPanel(IControlChilli nestedControl)
        {
            OKCancelPanelGiz mainPanel = new OKCancelPanelGiz(_controlFactory);
            mainPanel.ContentPanel.Controls.Add(nestedControl);
            nestedControl.Dock = DockStyle.Fill;
            return mainPanel;

            //IPanel mainPanel = _controlFactory.CreatePanel();

            //// create content panel
            //IPanel contentPanel = _controlFactory.CreatePanel();
            //contentPanel.Dock = DockStyle.Fill;
            //contentPanel.Controls.Add(nestedControl);
            //nestedControl.Dock = DockStyle.Fill;
            //mainPanel.Controls.Add(contentPanel);

            //// create buttons
            //IButtonGroupControl buttonGroupControl = _controlFactory.CreateButtonGroupControl();
            //buttonGroupControl.Dock = DockStyle.Bottom;
            //buttonGroupControl.AddButton("OK").NotifyDefault(true);
            //buttonGroupControl.AddButton("Cancel");
            //mainPanel.Controls.Add(buttonGroupControl);
            //return mainPanel;
        }

        public IFormChilli CreateOKCancelForm(IControlChilli nestedControl)
        {
            IFormChilli form = _controlFactory.CreateForm();
            IPanel mainPanel = CreateOKCancelPanel(nestedControl);
            mainPanel.Dock = DockStyle.Fill;
            form.Controls.Add(mainPanel);
            return form;
        }

        private class OKCancelPanelGiz : PanelGiz, IOKCancelPanel
        {
            private readonly IControlFactory _controlFactory;
            private IButton _okButton;
            private IPanel _contentPanel;

            public OKCancelPanelGiz(IControlFactory controlFactory)
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