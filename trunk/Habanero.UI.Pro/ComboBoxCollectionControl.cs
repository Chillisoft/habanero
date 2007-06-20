using System;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Generic;
using Habanero.Ui.Generic;
using log4net;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.BoControls
{
    /// <summary>
    /// Provides a control for collection ComboBoxes
    /// in a user interface.
    /// </summary>
    /// TODO ERIC - still don't quite understand what this is and what the
    /// add button does.  need to see in action
    public class ComboBoxCollectionControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Bo.BOControls.ComboBoxCollectionControl");
        private readonly IDatabaseConnection _databaseConnection;
        private readonly IConfirmer _confirmer;
        private PanelFactoryInfo _panelFactoryInfo;
        private ComboBox _collectionComboBox;
        private CollectionComboBoxMapper _comboBoxMapper;
        private int _oldSelectedIndex;
        private ButtonControl _buttons;
        private BusinessObjectCollection _boCollection;

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
            _databaseConnection = databaseConnection;
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _collectionComboBox = new ComboBox();
            _comboBoxMapper = new CollectionComboBoxMapper(_collectionComboBox);
            _boCollection = provider.GetCollection();
            _comboBoxMapper.SetCollection(_boCollection, true);

            Panel comboBoxPanel = new Panel();
            GridLayoutManager comboBoxPanelManager = new GridLayoutManager(comboBoxPanel);
            comboBoxPanelManager.SetGridSize(1, 2);
            comboBoxPanelManager.FixColumnBasedOnContents(0);
            comboBoxPanelManager.FixAllRowsBasedOnContents();
            comboBoxPanelManager.AddControl(ControlFactory.CreateLabel(label, false));
            comboBoxPanelManager.AddControl(_collectionComboBox);
            comboBoxPanel.Height = comboBoxPanelManager.GetFixedHeightIncludingGaps();

            PanelFactory panelFactory =
                new PanelFactory(_boCollection.ClassDef.CreateNewBusinessObject(), provider.GetUIFormDef());
            _panelFactoryInfo = panelFactory.CreatePanel();
            _panelFactoryInfo.Panel.Enabled = false;

            _buttons = new ButtonControl();
            _buttons.AddButton("Save", new EventHandler(SaveButtonClickHander));
            _buttons.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            _buttons.AddButton("Add", new EventHandler(AddButtonClickHandler));

            this.Height = comboBoxPanel.Height + _panelFactoryInfo.Panel.Height + _buttons.Height;
            this.Width = _panelFactoryInfo.Panel.Width;
            manager.AddControl(_panelFactoryInfo.Panel, BorderLayoutManager.Position.Centre);
            manager.AddControl(comboBoxPanel, BorderLayoutManager.Position.North);
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);

            _collectionComboBox.SelectedIndexChanged += new EventHandler(CollectionComboBoxIndexChangedHandler);
            _confirmer = confirmer;
            _oldSelectedIndex = -1;
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
            _panelFactoryInfo.ControlMappers.BusinessObject =
                _boCollection.ClassDef.CreateNewBusinessObject(_databaseConnection);
            _collectionComboBox.SelectedIndex = -1;
            _collectionComboBox.Enabled = false;
            _buttons["Add"].Enabled = false;
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
            _collectionComboBox.Enabled = true;
            _buttons["Add"].Enabled = true;
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
                _boCollection.Add(this.SelectedBusinessObject);
                this.CollectionComboBox.SelectedItem = this.SelectedBusinessObject;
            }
            else
            {
                this.SelectedBusinessObject.ApplyEdit();
            }
            _collectionComboBox.Enabled = true;
            _buttons["Add"].Enabled = true;
            int index = _collectionComboBox.Items.IndexOf(this.SelectedBusinessObject);
            _collectionComboBox.Items.RemoveAt(index);
            _collectionComboBox.Items.Insert(index, this.SelectedBusinessObject);
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
            if (_collectionComboBox.SelectedIndex == -1)
            {
                this._panelFactoryInfo.Panel.Enabled = false;
                _oldSelectedIndex = -1;
                return;
            }
            else
            {
                this._panelFactoryInfo.Panel.Enabled = true;
            }
            if (_oldSelectedIndex != -1 && _oldSelectedIndex != _collectionComboBox.SelectedIndex &&
                SelectedBusinessObject.IsDirty)
            {
                if (_confirmer.Confirm("Do you want to want to save before moving on?"))
                {
                    SelectedBusinessObject.ApplyEdit();
                }
                else
                {
                    _collectionComboBox.SelectedIndex = _oldSelectedIndex;
                }
            }
            _panelFactoryInfo.ControlMappers.BusinessObject = _comboBoxMapper.SelectedBusinessObject;
            _panelFactoryInfo.Panel.Enabled = true;
            _oldSelectedIndex = _collectionComboBox.SelectedIndex;
        }

        /// <summary>
        /// Returns the ComboBox object
        /// </summary>
        public ComboBox CollectionComboBox
        {
            get { return _collectionComboBox; }
        }

        /// <summary>
        /// Returns the panel that contains the objects
        /// </summary>
        public Panel BusinessObjectPanel
        {
            get { return _panelFactoryInfo.Panel; }
        }

        /// <summary>
        /// Returns the PanelFactoryInfo object
        /// </summary>
        public PanelFactoryInfo PanelFactoryInfo
        {
            get { return _panelFactoryInfo; }
        }

        /// <summary>
        /// Returns the button control object
        /// </summary>
        public ButtonControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Returns the business object that is currently selected
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _panelFactoryInfo.ControlMappers.BusinessObject; }
        }
    }
}