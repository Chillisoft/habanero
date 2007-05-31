using System;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;
using log4net;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Provides a control for collection ComboBoxes
    /// in a user interface.
    /// </summary>
    /// TODO ERIC - still don't quite understand what this is and what the
    /// add button does.  need to see in action
    public class ComboBoxCollectionControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.v2.BOControls.ComboBoxCollectionControl");
        private readonly IDatabaseConnection itsDatabaseConnection;
        private readonly IConfirmer itsConfirmer;
        private PanelFactoryInfo itsPanelFactoryInfo;
        private ComboBox itsCollectionComboBox;
        private CollectionComboBoxMapper itsComboBoxMapper;
        private int itsOldSelectedIndex;
        private ButtonControl itsButtons;
        private BusinessObjectBaseCollection itsBoCollection;

        /// <summary>
        /// Constructor to initialise a new control object that has a
        /// window listing a label, a ComboBox and buttons to allow business
        /// objects in the ComboBox to be added or edited
        /// </summary>
        /// <param name="label">The label to appear above the ComboBox</param>
        /// <param name="provider">The provider of the relevant data</param>
        /// <param name="confirmer">The confirmer object</param>
        /// <param name="databaseConnection">The database connection</param>
        public ComboBoxCollectionControl(string label, IFormDataProvider provider, IConfirmer confirmer,
                                         IDatabaseConnection databaseConnection)
        {
            //log.Debug("Creating comboboxcollectioncontrol") ;
            itsDatabaseConnection = databaseConnection;
            BorderLayoutManager manager = new BorderLayoutManager(this);
            itsCollectionComboBox = new ComboBox();
            itsComboBoxMapper = new CollectionComboBoxMapper(itsCollectionComboBox);
            itsBoCollection = provider.GetCollection();
            itsComboBoxMapper.SetCollection(itsBoCollection, true);

            Panel comboBoxPanel = new Panel();
            GridLayoutManager comboBoxPanelManager = new GridLayoutManager(comboBoxPanel);
            comboBoxPanelManager.SetGridSize(1, 2);
            comboBoxPanelManager.FixColumnBasedOnContents(0);
            comboBoxPanelManager.FixAllRowsBasedOnContents();
            comboBoxPanelManager.AddControl(ControlFactory.CreateLabel(label, false));
            comboBoxPanelManager.AddControl(itsCollectionComboBox);
            comboBoxPanel.Height = comboBoxPanelManager.GetFixedHeightIncludingGaps();

            PanelFactory panelFactory =
                new PanelFactory(itsBoCollection.ClassDef.CreateNewBusinessObject(), provider.GetUIFormDef());
            itsPanelFactoryInfo = panelFactory.CreatePanel();
            itsPanelFactoryInfo.Panel.Enabled = false;

            itsButtons = new ButtonControl();
            itsButtons.AddButton("Save", new EventHandler(SaveButtonClickHander));
            itsButtons.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            itsButtons.AddButton("Add", new EventHandler(AddButtonClickHandler));

            this.Height = comboBoxPanel.Height + itsPanelFactoryInfo.Panel.Height + itsButtons.Height;
            this.Width = itsPanelFactoryInfo.Panel.Width;
            manager.AddControl(itsPanelFactoryInfo.Panel, BorderLayoutManager.Position.Centre);
            manager.AddControl(comboBoxPanel, BorderLayoutManager.Position.North);
            manager.AddControl(itsButtons, BorderLayoutManager.Position.South);

            itsCollectionComboBox.SelectedIndexChanged += new EventHandler(CollectionComboBoxIndexChangedHandler);
            itsConfirmer = confirmer;
            itsOldSelectedIndex = -1;
            //log.Debug("Done Creating comboboxcollectioncontrol") ;
        }

        /// <summary>
        /// A handler called when the "Add" button has been pressed.
        /// It allows a new business object to be created.
        /// </summary>
        /// <param name="sender">The object that notified of the click</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AddButtonClickHandler(object sender, EventArgs e)
        {
            itsPanelFactoryInfo.ControlMappers.BusinessObject =
                itsBoCollection.ClassDef.CreateNewBusinessObject(itsDatabaseConnection);
            itsCollectionComboBox.SelectedIndex = -1;
            itsCollectionComboBox.Enabled = false;
            itsButtons["Add"].Enabled = false;
            this.BusinessObjectPanel.Enabled = true;
        }

        /// <summary>
        /// A handler called when the "Cancel" button has been pressed. 
        /// This cancels the edits that have been made on the business object.
        /// </summary>
        /// <param name="sender">The object that notified of the click</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonClickHandler(object sender, EventArgs e)
        {
            this.SelectedBusinessObject.CancelEdit();
            itsCollectionComboBox.Enabled = true;
            itsButtons["Add"].Enabled = true;
        }

        /// <summary>
        /// A handler called when the "Save" button has been pressed.
        /// It saves the changes that have been made to the business object
        /// and ensures that it is selected in the ComboBox.
        /// </summary>
        /// <param name="sender">The object that notified of the click</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SaveButtonClickHander(object sender, EventArgs e)
        {
            if (this.SelectedBusinessObject.IsNew)
            {
                this.SelectedBusinessObject.ApplyEdit();
                itsBoCollection.Add(this.SelectedBusinessObject);
                this.CollectionComboBox.SelectedItem = this.SelectedBusinessObject;
            }
            else
            {
                this.SelectedBusinessObject.ApplyEdit();
            }
            itsCollectionComboBox.Enabled = true;
            itsButtons["Add"].Enabled = true;
            int index = itsCollectionComboBox.Items.IndexOf(this.SelectedBusinessObject);
            itsCollectionComboBox.Items.RemoveAt(index);
            itsCollectionComboBox.Items.Insert(index, this.SelectedBusinessObject);
        }

        /// <summary>
        /// A handler called when the user has chosen a different item
        /// in the ComboBox.  If the user has made changes to an item and not
        /// saved those changes, they will be prompted.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CollectionComboBoxIndexChangedHandler(object sender, EventArgs e)
        {
            //log.Debug("index changed ");
            if (itsCollectionComboBox.SelectedIndex == -1)
            {
                this.itsPanelFactoryInfo.Panel.Enabled = false;
                itsOldSelectedIndex = -1;
                return;
            }
            else
            {
                this.itsPanelFactoryInfo.Panel.Enabled = true;
            }
            if (itsOldSelectedIndex != -1 && itsOldSelectedIndex != itsCollectionComboBox.SelectedIndex &&
                SelectedBusinessObject.IsDirty)
            {
                if (itsConfirmer.Confirm("Do you want to want to save before moving on?"))
                {
                    SelectedBusinessObject.ApplyEdit();
                }
                else
                {
                    itsCollectionComboBox.SelectedIndex = itsOldSelectedIndex;
                }
            }
            itsPanelFactoryInfo.ControlMappers.BusinessObject = itsComboBoxMapper.SelectedBusinessObject;
            itsPanelFactoryInfo.Panel.Enabled = true;
            itsOldSelectedIndex = itsCollectionComboBox.SelectedIndex;
        }

        /// <summary>
        /// Returns the ComboBox object
        /// </summary>
        public ComboBox CollectionComboBox
        {
            get { return itsCollectionComboBox; }
        }

        /// <summary>
        /// Returns the panel that contains the objects
        /// </summary>
        public Panel BusinessObjectPanel
        {
            get { return itsPanelFactoryInfo.Panel; }
        }

        /// <summary>
        /// Returns the PanelFactoryInfo object
        /// </summary>
        public PanelFactoryInfo PanelFactoryInfo
        {
            get { return itsPanelFactoryInfo; }
        }

        /// <summary>
        /// Returns the button control object
        /// </summary>
        public ButtonControl Buttons
        {
            get { return itsButtons; }
        }

        /// <summary>
        /// Returns the business object that is currently selected
        /// </summary>
        public BusinessObjectBase SelectedBusinessObject
        {
            get { return itsPanelFactoryInfo.ControlMappers.BusinessObject; }
        }
    }
}