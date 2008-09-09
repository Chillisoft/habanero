using System.Windows.Forms;
using Habanero.UI.Base;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Creates OK/Cancel dialogs which contain OK and Cancel buttons, as well
    /// as control placed above the buttons, which the developer must provide.
    /// </summary>
    public class OKCancelDialogFactoryWin : IOKCancelDialogFactory
    {
        private readonly IControlFactory _controlFactory;

        public OKCancelDialogFactoryWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public IOKCancelPanel CreateOKCancelPanel(IControlHabanero nestedControl)
        {
            OKCancelPanelWin mainPanel = new OKCancelPanelWin(_controlFactory);
            mainPanel.Width = nestedControl.Width  + 5;
            mainPanel.Height = nestedControl.Height + mainPanel.ButtonGroupControl.Height + 5;
            mainPanel.ContentPanel.Controls.Add(nestedControl);
            nestedControl.Dock = DockStyle.Fill;
            return mainPanel;
        }

        public IFormHabanero CreateOKCancelForm(IControlHabanero nestedControl, string formTitle)
        {
            FormWin form = (FormWin) _controlFactory.CreateForm();
            form.Text = formTitle;
            IOKCancelPanel mainPanel = CreateOKCancelPanel(nestedControl);
            mainPanel.Dock = DockStyle.Fill;
            form.Controls.Add((Control) mainPanel);
            form.Height = mainPanel.Height + 5;
            form.Width = mainPanel.Width + 5;
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

        /// <summary>
        /// Represents a panel that contains an OK and Cancel button
        /// </summary>
        private class OKCancelPanelWin : PanelWin, IOKCancelPanel
        {
            private readonly IControlFactory _controlFactory;
            private readonly IButton _okButton;
            private readonly IPanel _contentPanel;
            private readonly IButton _cancelButton;
            private readonly IButtonGroupControl _buttonGroupControl;

            public OKCancelPanelWin(IControlFactory controlFactory)
            {
                //_controlFactory = controlFactory;
                //// create content panel
                //_contentPanel = _controlFactory.CreatePanel();
                //_contentPanel.Dock = DockStyle.Fill;
                //this.Controls.Add((Control) _contentPanel);

                //// create buttons
                //IButtonGroupControl buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                //buttonGroupControl.Dock = DockStyle.Bottom;
                //_okButton = buttonGroupControl.AddButton("OK");
                //_okButton.NotifyDefault(true);
                //_cancelButton = buttonGroupControl.AddButton("Cancel");
                //this.Controls.Add((Control) buttonGroupControl);

                _controlFactory = controlFactory;
                // create content panel
                _contentPanel = _controlFactory.CreatePanel();
                // create buttons
                _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                _okButton = ButtonGroupControl.AddButton("OK");
                _okButton.NotifyDefault(true);
                _cancelButton = ButtonGroupControl.AddButton("Cancel");

                BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
                layoutManager.AddControl(_contentPanel, BorderLayoutManager.Position.Centre);
                layoutManager.AddControl(ButtonGroupControl, BorderLayoutManager.Position.South);
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

            /// <summary>
            /// Gets the button group control containing the buttons
            /// </summary>
            public IButtonGroupControl ButtonGroupControl
            {
                get { return _buttonGroupControl; }
            }
        }
    }
}