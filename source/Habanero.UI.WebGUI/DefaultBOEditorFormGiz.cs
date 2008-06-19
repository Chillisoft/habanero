using System;
using System.Reflection;
using Gizmox.WebGUI.Common.Interfaces;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using log4net;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Provides a form to edit a business object.  This form is initiated
    /// by UI.Application.DefaultBOEditor and UI.Application.DefaultBOCreator
    /// and is used by facilities like readOnlyGridControl.
    /// If you need to implement a different version of this form, you will 
    /// need to also implement a new version of the editor (see
    /// DefaultBOEditor for more information).
    /// </summary>
    public class DefaultBOEditorFormGiz : Form, IDefaultBOEditorForm
    {
        private readonly PostObjectPersistingDelegate _action;
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.DefaultBOEditorFormGiz");
        private readonly string _uiDefName;
        private readonly IButtonGroupControl _buttons;
        protected BusinessObject _bo;
        private readonly IControlFactory _controlFactory;
        private readonly IPanel _boPanel;
        protected IPanelFactoryInfo _panelFactoryInfo;

        public DefaultBOEditorFormGiz(BusinessObject bo, string name, IControlFactory controlFactory, PostObjectPersistingDelegate action):this(bo, name, controlFactory)
        {
            _action = action;
            
        }

        /// <summary>
        /// Constructor to initialise a new form with a panel containing the
        /// business object to edit and "OK" and "Cancel" buttons at the
        /// bottom, with attached event handlers.
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The uiDefName</param>
        /// <param name="controlFactory"></param>
        public DefaultBOEditorFormGiz(BusinessObject bo, string uiDefName, IControlFactory controlFactory)
        {
            _bo = bo;
            _controlFactory = controlFactory;
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

            IPanelFactory factory = new PanelFactory(_bo, def, _controlFactory);
            _panelFactoryInfo = factory.CreatePanel();
            _boPanel = _panelFactoryInfo.Panel;
            _buttons = _controlFactory.CreateButtonGroupControl();
            _buttons.AddButton("&Cancel", CancelButtonHandler);
            IButton okbutton = _buttons.AddButton("&OK", OKButtonHandler);
            okbutton.NotifyDefault(true);
            this.AcceptButton = (ButtonGiz)okbutton;
            this.Load += delegate { FocusOnFirstControl(); };

            Text = def.Title;
            SetupFormSize(def);
            MinimizeBox = false;
            MaximizeBox = false;
            //this.ControlBox = false;

            CreateLayout();
            OnResize(new EventArgs());
        }

        private void FocusOnFirstControl()
        {
            IControlChilli controlToFocus = _panelFactoryInfo.FirstControlToFocus;
            MethodInfo focusMethod = controlToFocus.GetType().
                GetMethod("Focus", BindingFlags.Instance | BindingFlags.Public);
            if (focusMethod != null)
            {
                focusMethod.Invoke(controlToFocus, new object[] { });
            }
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

        ///// <summary>
        ///// Constructor as before, but sets the uiDefName to an empty string,
        ///// which uses the ui definition without a specified name attribute
        ///// </summary>
        ///// <param name="bo">The business object to represent</param>
        //public DefaultBOEditorFormGiz(BusinessObject bo)
        //    : this(bo, "", null)
        //{
        //}

        /// <summary>
        /// Returns the panel object being managed
        /// </summary>
        protected IPanel BoPanel
        {
            get { return _boPanel; }
        }

        /// <summary>
        /// Sets up the layout of the panel and buttons
        /// </summary>
        protected virtual void CreateLayout()
        {
            BorderLayoutManager borderLayoutManager = new BorderLayoutManagerGiz(this, _controlFactory);
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
                TransactionCommitter committer = CreateSaveTransaction();
                committer.CommitTransaction();


                //TODO_Port: Removed by peter
                //if (_boPanel.Controls[0] is TabControl)
                //{
                //    //Console.Out.WriteLine("tabcontrol found.");
                //    TabControl tabControl = (TabControl)_boPanel.Controls[0];
                //    foreach (TabPage page in tabControl.TabPages)
                //    {
                //        foreach (Panel panel in page.Controls)
                //        {
                //            foreach (Control control in panel.Controls)
                //            {
                //                //Console.Out.WriteLine(control.GetType().Name);
                //                if (control is EditableGrid)
                //                {
                //                    //Console.Out.WriteLine("EditableGrid found.");
                //                    ((EditableGrid)control).SaveChanges();
                //                }
                //            }
                //        }
                //    }
                //}

                DialogResult = DialogResult.OK;
                Close();
                if (_action != null)
                {
                    _action(this._bo);
                }
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
        /// Returns the button control for the buttons in the form
        /// </summary>
        public IButtonGroupControl Buttons
        {
            get { return _buttons; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(this.Controls); }
        }

        /// <summary>
        /// Pops the form up in a modal dialog.  If the BO is successfully edited and saved, returns true
        /// else returns false.
        /// </summary>
        /// <returns>True if the edit was a success, false if not</returns>
         bool IDefaultBOEditorForm.ShowDialog() {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}