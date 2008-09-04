using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Creates OK/Cancel dialogs which contain OK and Cancel buttons, as well
    /// as control placed above the buttons, which the developer must provide.
    /// </summary>
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

        /// <summary>
        /// Creates a form containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <param name="formTitle">The title shown on the form</param>
        /// <returns>Returns the created form</returns>
        public IFormChilli CreateOKCancelForm(IControlChilli nestedControl, string formTitle)
        {
            IFormChilli form = _controlFactory.CreateForm();
            IPanel mainPanel = CreateOKCancelPanel(nestedControl);
            mainPanel.Dock = DockStyle.Fill;
            form.Controls.Add(mainPanel);
            form.Text = formTitle;
            return form;
        }

        /// <summary>
        /// Represents a panel that contains an OK and Cancel button
        /// </summary>
        private class OKCancelPanelGiz : PanelGiz, IOKCancelPanel
        {
            private readonly IControlFactory _controlFactory;
            private IButton _okButton;
            private IPanel _contentPanel;
            private IButton _cancelButton;

            public OKCancelPanelGiz(IControlFactory controlFactory)
            {
                //_controlFactory = controlFactory;
                //// create content panel
                //_contentPanel = _controlFactory.CreatePanel();
                //_contentPanel.Dock = DockStyle.Fill;
                //this.Controls.Add((Control)_contentPanel);

                //// create buttons
                //IButtonGroupControl buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                //buttonGroupControl.Dock = DockStyle.Bottom;
                //_okButton = buttonGroupControl.AddButton("OK");
                //_okButton.NotifyDefault(true);
                //_cancelButton = buttonGroupControl.AddButton("Cancel");
                //this.Controls.Add((Control)buttonGroupControl);

                _controlFactory = controlFactory;
                // create content panel
                _contentPanel = _controlFactory.CreatePanel();
                // create buttons
                IButtonGroupControl buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                _okButton = buttonGroupControl.AddButton("OK");
                _okButton.NotifyDefault(true);
                _cancelButton = buttonGroupControl.AddButton("Cancel");

                BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
                layoutManager.AddControl(_contentPanel, BorderLayoutManager.Position.Centre);
                layoutManager.AddControl(buttonGroupControl, BorderLayoutManager.Position.South);
            }

            /// <summary>
            /// Gets the OK button
            /// </summary>
            public IButton OKButton
            {
                get { return _okButton; }
            }

            /// <summary>
            /// Gets the Cancel button
            /// </summary>
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