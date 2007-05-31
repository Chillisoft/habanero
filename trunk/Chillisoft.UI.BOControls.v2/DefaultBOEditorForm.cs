using System;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;
using log4net;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.UI.BOControls.v2
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
        private readonly string itsUiDefName;
        private ButtonControl itsButtons;
        protected BusinessObjectBase itsBo;
        private Panel itsBoPanel;
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.UI.BOControls.v2.DefaultBOEditorForm");
        protected PanelFactoryInfo itsPanelFactoryInfo;

        /// <summary>
        /// Constructor to initialise a new form with a panel containing the
        /// business object to edit and "OK" and "Cancel" buttons at the
        /// bottom, with attached event handlers.
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The uiDefName</param>
        public DefaultBOEditorForm(BusinessObjectBase bo, string uiDefName)
        {
            itsBo = bo;
            itsUiDefName = uiDefName;

            BOMapper mapper = new BOMapper(bo);

            UIFormDef def;
            if (itsUiDefName.Length > 0)
            {
                def = mapper.GetUserInterfaceMapper(itsUiDefName).GetUIFormProperties();
            }
            else
            {
                def = mapper.GetUserInterfaceMapper().GetUIFormProperties();
            }

            PanelFactory factory = new PanelFactory(itsBo, def);
            itsPanelFactoryInfo = factory.CreatePanel();
            itsBoPanel = itsPanelFactoryInfo.Panel;
            itsButtons = new ButtonControl();
            itsButtons.AddButton("&Cancel", new EventHandler(CancelButtonHandler));
            itsButtons.AddButton("&OK", new EventHandler(OKButtonHandler)).NotifyDefault(true);
 
            this.Text = def.Heading;
            this.Height = def.Height;
            this.Width = def.Width;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;


            CreateLayout();
        }

        /// <summary>
        /// Constructor as before, but sets the uiDefName to an empty string
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        public DefaultBOEditorForm(BusinessObjectBase bo)
            : this(bo, "")
        {
        }

        /// <summary>
        /// Returns the panel object being managed
        /// </summary>
        protected Panel BoPanel
        {
            get { return itsBoPanel; }
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
            itsBo.CancelEdit();
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
                if (itsBoPanel.Controls[0] is TabControl)
                {
                    //Console.Out.WriteLine("tabcontrol found.");
                    TabControl tabControl = (TabControl) itsBoPanel.Controls[0];
                    foreach (TabPage page in tabControl.TabPages)
                    {
                        foreach (Panel panel in page.Controls)
                        {
                            foreach (Control control in panel.Controls)
                            {
                                //Console.Out.WriteLine(control.GetType().Name);
                                if (control is IGrid)
                                {
                                    //Console.Out.WriteLine("IGrid found.");
                                    ((IGrid) control).SaveChanges();
                                }
                            }
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BaseApplicationException ex)
            {
                log.Error(ExceptionUtil.GetExceptionString(ex, 0));
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
            Transaction saveTransaction = new Transaction(itsBo.GetDatabaseConnection());
            saveTransaction.AddTransactionObject(itsBo);
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
            get { return itsButtons; }
        }
    }
}