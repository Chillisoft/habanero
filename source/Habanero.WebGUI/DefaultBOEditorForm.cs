//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

#pragma warning disable DoNotCallOverridableMethodsInConstructor

using System;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using log4net;

namespace Habanero.WebGUI
{
    /// <summary>
    /// Provides a form to edit a business object.  This form is initiated
    /// by UI.Application.DefaultBOEditor and UI.Application.DefaultBOCreator
    /// and is used by facilities like ReadOnlyGridWithButtons.
    /// If you need to implement a different version of this form, you will 
    /// need to also implement a new version of the editor (see
    /// DefaultBOEditor for more information).
    /// </summary>
    public class DefaultBOEditorForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.DefaultBOEditorForm");
        private readonly string _uiDefName;
        private readonly ButtonControl _buttons;
        protected BusinessObject _bo;
        private readonly Panel _boPanel;
        protected PanelFactoryInfo _panelFactoryInfo;

        /// <summary>
        /// Constructor to initialise a new form with a panel containing the
        /// business object to edit and "OK" and "Cancel" buttons at the
        /// bottom, with attached event handlers.
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The uiDefName</param>
        public DefaultBOEditorForm(BusinessObject bo, string uiDefName)
        {
            _bo = bo;
            _uiDefName = uiDefName;

            BOMapper mapper = new BOMapper(bo);

            UIForm def;
            if (_uiDefName.Length > 0)
            {
                UIDef uiMapper = mapper.GetUIDef(_uiDefName);
                if (uiMapper == null)
                {
                    throw new NullReferenceException("An error occurred while " +
                                                     "attempting to load an object editing form.  A possible " +
                                                     "cause is that the class definitions do not have a " +
                                                     "'form' section for the class, under the 'ui' " +
                                                     "with the name '" + _uiDefName + "'.");
                }
                def = uiMapper.GetUIFormProperties();
            }
            else
            {
                UIDef uiMapper = mapper.GetUIDef();
                if (uiMapper == null)
                {
                    throw new NullReferenceException("An error occurred while " +
                                                     "attempting to load an object editing form.  A possible " +
                                                     "cause is that the class definitions do not have a " +
                                                     "'form' section for the class.");
                }
                def = uiMapper.GetUIFormProperties();
            }
            if (def == null)
            {
                throw new NullReferenceException("An error occurred while " +
                                                 "attempting to load an object editing form.  A possible " +
                                                 "cause is that the class definitions do not have a " +
                                                 "'form' section for the class.");
            }

            PanelFactory factory = new PanelFactory(_bo, def);
            _panelFactoryInfo = factory.CreatePanel();
            _boPanel = _panelFactoryInfo.Panel;
            _buttons = new ButtonControl();
            Button cancelButton = _buttons.AddButton("&Cancel", CancelButtonHandler);
            Button okbutton = _buttons.AddButton("&OK", OKButtonHandler);
            okbutton.NotifyDefault(true);
            AcceptButton = okbutton;
            CancelButton = cancelButton;

            Text = def.Title;
            SetupFormSize(def);
            MinimizeBox = false;
            MaximizeBox = false;
            //this.ControlBox = false;

            CreateLayout();
            OnResize(new EventArgs());
        }

        //private void DefaultBOEditorForm_Load(object sender, EventArgs e)
        //{
        //    if (_panelFactoryInfo.ControlMappers.BusinessObject == null && _bo != null)
        //    {
        //        _panelFactoryInfo.ControlMappers.BusinessObject = _bo;
        //    }
        //}

        protected virtual void SetupFormSize(UIForm def)
        {
            int width = def.Width;
            int minWidth = _boPanel.Width +
                Margin.Left + Margin.Right;
            if (width < minWidth)
            {
                width = minWidth;
            }
            int height = def.Height;
            int minHeight = _boPanel.Height + _buttons.Height + 
                Margin.Top + Margin.Bottom;
            if (height < minHeight)
            {
                height = minHeight;
            }
            Height = height;
            Width = width;
        }

        /// <summary>
        /// Constructor as before, but sets the uiDefName to an empty string,
        /// which uses the ui definition without a specified name attribute
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        public DefaultBOEditorForm(BusinessObject bo)
            : this(bo, "")
        {
        }

        /// <summary>
        /// Returns the panel object being managed
        /// </summary>
        protected Panel BoPanel
        {
            get { return _boPanel; }
        }

        /// <summary>
        /// Sets up the layout of the panel and buttons
        /// </summary>
        protected virtual void CreateLayout()
        {
            BorderLayoutManager borderLayoutManager;
            borderLayoutManager = new BorderLayoutManager(this);
            borderLayoutManager.AddControl(BoPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Buttons, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// A handler to respond when the "Cancel" button has been pressed.
        /// Any unsaved edits are cancelled and the dialog is closed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonHandler(object sender, EventArgs e)
        {
            _panelFactoryInfo.ControlMappers.BusinessObject = null;
            _bo.Restore();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// A handler to respond when the "OK" button has been pressed.
        /// All changes are committed to the database and the dialog is closed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonHandler(object sender, EventArgs e)
        {
            try
            {
                _panelFactoryInfo.ControlMappers.ApplyChangesToBusinessObject();
                if (!ValidateData())
                {
                    return;
                }
                TransactionCommitter committer = CreateSaveTransaction();
                committer.CommitTransaction();

                DialogResult = DialogResult.OK;
                Close();
                _panelFactoryInfo.ControlMappers.BusinessObject = null;
            }
            catch (Exception ex)
            {
                log.Error(ExceptionUtilities.GetExceptionString(ex, 0, true));
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem saving for the following reason(s):",
                                                          "Saving Problem");
            }
        }

        /// <summary>
        /// Returns a transaction object, preparing the database connection and
        /// specifying which object to update
        /// </summary>
        /// <returns>Returns the transaction object</returns>
        protected virtual TransactionCommitter CreateSaveTransaction()
        {
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(_bo);
            return committer;
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <returns></returns>
        /// TODO ERIC - implement
        protected virtual bool ValidateData()
        {
            return true;
        }

        /// <summary>
        /// Returns the button control for the buttons in the form
        /// </summary>
        public ButtonControl Buttons
        {
            get { return _buttons; }
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // DefaultBOEditorForm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(292, 266);
        //    this.Name = "DefaultBOEditorForm";
        //    this.Load += new System.EventHandler(this.DefaultBOEditorForm_Load);
        //    this.ResumeLayout(false);

        //}

        
    }
}