using System;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Bo.ClassDefinition;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;
using Habanero.Ui.Grid;
using log4net;

using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
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
        private static readonly ILog log = LogManager.GetLogger("Habanero.Ui.Forms.DefaultBOEditorForm");
        private readonly string _uiDefName;
        private ButtonControl _buttons;
        protected BusinessObject _bo;
        private Panel _boPanel;
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
            Button cancelButton = _buttons.AddButton("&Cancel", new EventHandler(CancelButtonHandler));
            Button okbutton = _buttons.AddButton("&OK", new EventHandler(OKButtonHandler));
            okbutton.NotifyDefault(true);
            AcceptButton = okbutton;
            CancelButton = cancelButton;
 
            this.Text = def.Title;
            this.Height = def.Height;
            this.Width = def.Width;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;


            CreateLayout();
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
            BorderLayoutManager manager = new BorderLayoutManager(this);
            manager.AddControl(this.BoPanel, BorderLayoutManager.Position.Centre);
            manager.AddControl(this.Buttons, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// A handler to respond when the "Cancel" button has been pressed.
        /// Any unsaved edits are cancelled and the dialog is closed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonHandler(object sender, EventArgs e)
        {
            _bo.Restore();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
                if (!ValidateData())
                {
                    return;
                }
                Transaction transaction = CreateSaveTransaction();
                transaction.CommitTransaction();

                //TODO: this is TERRIBLE!
                if (_boPanel.Controls[0] is TabControl)
                {
                    //Console.Out.WriteLine("tabcontrol found.");
                    TabControl tabControl = (TabControl) _boPanel.Controls[0];
                    foreach (TabPage page in tabControl.TabPages)
                    {
                        foreach (Panel panel in page.Controls)
                        {
                            foreach (Control control in panel.Controls)
                            {
                                //Console.Out.WriteLine(control.GetType().Name);
                                if (control is EditableGrid)
                                {
                                    //Console.Out.WriteLine("EditableGrid found.");
                                    ((EditableGrid)control).SaveChanges();
                                }
                            }
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
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
        protected virtual Transaction CreateSaveTransaction()
        {
            Transaction saveTransaction = new Transaction(BOLoader.Instance.GetDatabaseConnection(_bo));
            saveTransaction.AddTransactionObject(_bo);
            return saveTransaction;
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
    }
}