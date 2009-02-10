//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Creates OK/Cancel dialogs which contain OK and Cancel buttons, as well
    /// as control placed above the buttons, which the developer must provide.
    /// </summary>
    public class OKCancelDialogFactoryVWG : IOKCancelDialogFactory
    {
        private readonly IControlFactory _controlFactory;

        public OKCancelDialogFactoryVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public IOKCancelPanel CreateOKCancelPanel(IControlHabanero nestedControl)
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
        public IFormHabanero CreateOKCancelForm(IControlHabanero nestedControl, string formTitle)
        {
            IFormHabanero form = _controlFactory.CreateForm();
            IPanel mainPanel = CreateOKCancelPanel(nestedControl);
            mainPanel.Dock = DockStyle.Fill;
            form.Controls.Add(mainPanel);
            form.Text = formTitle;
            return form;
        }

        /// <summary>
        /// Represents a panel that contains an OK and Cancel button
        /// </summary>
        private class OKCancelPanelGiz : PanelVWG, IOKCancelPanel
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