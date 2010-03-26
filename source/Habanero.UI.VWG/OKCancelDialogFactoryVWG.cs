// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Creates OK/Cancel dialogs which contain OK and Cancel buttons, as well
    /// as control placed above the buttons, which the developer must provide.
    /// </summary>
    public class OKCancelDialogFactoryVWG : IOKCancelDialogFactory
    {
        private readonly IControlFactory _controlFactory;

        ///<summary>
        /// Constructs teh <see cref="OKCancelDialogFactoryVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public OKCancelDialogFactoryVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Creates a panel containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <returns>Returns the created panel</returns>
        public IOKCancelPanel CreateOKCancelPanel(IControlHabanero nestedControl)
        {
            OKCancelPanelVWG mainPanel = new OKCancelPanelVWG(_controlFactory);
            mainPanel.Width = nestedControl.Width;
            mainPanel.Height = nestedControl.Height + mainPanel.ButtonGroupControl.Height;
            mainPanel.ContentPanel.Controls.Add(nestedControl);
            nestedControl.Dock = DockStyle.Fill;
            return mainPanel;
        }

        /// <summary>
        /// Creates a form containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <param name="formTitle">The title shown on the form</param>
        /// <returns>Returns the created form</returns>
        public IFormHabanero CreateOKCancelForm(IControlHabanero nestedControl, string formTitle)
        {
            IOKCancelPanel mainPanel = CreateOKCancelPanel(nestedControl);
            IFormHabanero form = _controlFactory.CreateForm();
            form.Text = formTitle;
            form.ClientSize = mainPanel.Size;
            mainPanel.Dock = DockStyle.Fill;
            form.Controls.Add(mainPanel);
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

        ///<summary>
        /// Handles the event of the Cancel Button being clicked.
        ///</summary>
        ///<param name="form"></param>
        public void CancelButton_ClickHandler(IFormHabanero form)
        {
            form.DialogResult = Habanero.UI.Base.DialogResult.Cancel;
            form.Close();
        }

        ///<summary>
        /// Handles the event of the OKButton Being Clicked.
        ///</summary>
        ///<param name="form"></param>
        public void OkButton_ClickHandler(IFormHabanero form)
        {
            form.DialogResult = Habanero.UI.Base.DialogResult.OK;
            form.Close();
        }

        /// <summary>
        /// Represents a panel that contains an OK and Cancel button
        /// </summary>
        private class OKCancelPanelVWG : PanelVWG, IOKCancelPanel
        {
            private readonly IControlFactory _controlFactory;
            private IButton _okButton;
            private IPanel _contentPanel;
            private IButton _cancelButton;
            private IButtonGroupControl _buttonGroupControl;

            public OKCancelPanelVWG(IControlFactory controlFactory)
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
                _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
                _cancelButton = _buttonGroupControl.AddButton("Cancel");
                _okButton = _buttonGroupControl.AddButton("OK");
                _okButton.NotifyDefault(true);

                BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
                layoutManager.AddControl(_contentPanel, BorderLayoutManager.Position.Centre);
                layoutManager.AddControl(_buttonGroupControl, BorderLayoutManager.Position.South);
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