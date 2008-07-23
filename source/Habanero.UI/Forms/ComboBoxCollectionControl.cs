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

using System;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI;
using log4net;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a control for collection ComboBoxes
    /// in a user interface.
    /// </summary>
    /// TODO ERIC - still don't quite understand what this is and what the
    /// add button does.  need to see in action
    public class ComboBoxCollectionControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOControls.ComboBoxCollectionControl");
        private IDatabaseConnection _databaseConnection;
        private IConfirmer _confirmer;
        private PanelFactoryInfo _panelFactoryInfo;
        private ComboBox _collectionComboBox;
        private CollectionComboBoxMapper _comboBoxMapper;
        private int _oldSelectedIndex;
        private ButtonControl _buttons;
		private IBusinessObjectCollection _collection;
        private string _uiName;
        private GroupBox _uxBOGroupBox;
        private Label _label;
        private Panel comboBoxPanel;

        /// <summary>
        /// Constructor to initialise a new control object that has a
        /// window listing a label, a ComboBox and buttons to allow business
        /// objects in the ComboBox to be added or edited
        /// </summary>
        public ComboBoxCollectionControl()
        {
            //log.Debug("Creating comboboxcollectioncontrol") ;
            //_databaseConnection = databaseConnection;
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _collectionComboBox = new ComboBox();
            _comboBoxMapper = new CollectionComboBoxMapper(_collectionComboBox);

            comboBoxPanel = new Panel();
            GridLayoutManager comboBoxPanelManager = new GridLayoutManager(comboBoxPanel);
            comboBoxPanelManager.SetGridSize(1, 2);
            comboBoxPanelManager.FixColumnBasedOnContents(0);
            comboBoxPanelManager.FixAllRowsBasedOnContents();
            _label = ControlFactory.CreateLabel("Your label here", false);
            comboBoxPanelManager.AddControl(_label);
            comboBoxPanelManager.AddControl(_collectionComboBox);
            comboBoxPanel.Height = comboBoxPanelManager.GetFixedHeightIncludingGaps();

            _uxBOGroupBox = ControlFactory.CreateGroupBox();


            _buttons = new ButtonControl();
            _buttons.AddButton("Save", new EventHandler(SaveButtonClickHander));
            _buttons.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            _buttons.AddButton("Add", new EventHandler(AddButtonClickHandler));


            manager.AddControl(_uxBOGroupBox, BorderLayoutManager.Position.Centre);
            manager.AddControl(comboBoxPanel, BorderLayoutManager.Position.North);
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);

            _collectionComboBox.SelectedIndexChanged += new EventHandler(CollectionComboBoxIndexChangedHandler);
            //_confirmer = confirmer;
            _oldSelectedIndex = -1;
            //log.Debug("Done Creating comboboxcollectioncontrol") ;
        }

		public void SetCollection(IBusinessObjectCollection collection)
        {
            SetCollection(collection, "default");
        }

		public void SetCollection(IBusinessObjectCollection collection, string uiName)
        {
            _collection = collection;
            _uiName = uiName;

            _comboBoxMapper.SetCollection(_collection, true);

		    ClassDef classDef = (ClassDef) _collection.ClassDef;
		    UIDef uiDef = classDef.GetUIDef(uiName);
		    PanelFactory panelFactory =
                new PanelFactory(classDef.CreateNewBusinessObject(), uiDef.UIForm);
            _panelFactoryInfo = panelFactory.CreatePanel();
            _panelFactoryInfo.Panel.Enabled = false;

            _panelFactoryInfo.Panel.Dock = DockStyle.Fill;
            _uxBOGroupBox.Controls.Clear();
            _uxBOGroupBox.Controls.Add(_panelFactoryInfo.Panel);

            this.Height = comboBoxPanel.Height + _panelFactoryInfo.Panel.Height + _buttons.Height + 20;
            this.Width = _panelFactoryInfo.Panel.Width + 20;

        }

		public IBusinessObjectCollection Collection
        {
            get
            {
                return _collection;
            }
        }

        public IConfirmer Confirmer
        {
            get
            {
                return _confirmer;

            }
            set
            {
                _confirmer = value;
            }
        }

        public string Label
        {
            get
            {
                return _label.Text;
            }
            set
            {
                Label l = ControlFactory.CreateLabel(value, false);
                _label.Width = l.Width;
                _label.Text = value;

            }
        }

        /// <summary>
        /// A handler called when the "Add" button has been pressed.
        /// It allows a new business object to be created.
        /// </summary>
        /// <param name="sender">The object that notified of the click</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AddButtonClickHandler(object sender, EventArgs e)
        {
            _panelFactoryInfo.ControlMappers.BusinessObject = _collection.ClassDef.CreateNewBusinessObject();
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
            this.SelectedBusinessObject.Restore();
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
            if (this.SelectedBusinessObject.State.IsNew)
            {
                this.SelectedBusinessObject.Save();
                _collection.Add(this.SelectedBusinessObject);
                this.CollectionComboBox.SelectedItem = this.SelectedBusinessObject;
            }
            else
            {
                this.SelectedBusinessObject.Save();
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
            this._panelFactoryInfo.Panel.Enabled = true;
            if (_oldSelectedIndex != -1 && _oldSelectedIndex != _collectionComboBox.SelectedIndex &&
                SelectedBusinessObject.State.IsDirty)
            {
                if (_confirmer != null) {
                    if (_confirmer.Confirm("Do you want to want to save before moving on?")) {
                        SelectedBusinessObject.Save();
                    }
                    else {
                        _collectionComboBox.SelectedIndex = _oldSelectedIndex;
                    }
                } else {
                    //TODO: removed by soriya/brett
                    //if (_databaseConnection != null) {
                    //    BOLoader.Instance.SetDatabaseConnection(SelectedBusinessObject, _databaseConnection);
                    //}
                    
                    SelectedBusinessObject.Save();
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
            get { return (BusinessObject) _panelFactoryInfo.ControlMappers.BusinessObject; }
        }

        public IDatabaseConnection DatabaseConnection
        {
            get { return _databaseConnection;  }
            set { _databaseConnection = value; }
        }
    }
}