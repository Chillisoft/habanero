using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;
using Habanero.Ui.Grid;
using log4net;
using System.Collections.Generic;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Creates panels that display business object information in a user
    /// interface
    /// </summary>
    public class PanelFactory
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Ui.Forms.PanelFactory");
        private BusinessObject[] _boArray;
        //private IUserInterfaceMapper[] _uiArray;
        private UIFormDef _uiFormDef;
        private Control _firstControl;

        private EventHandler _emailTextBoxDoubleClickedHandler;

        /// <summary>
        /// Constructor to initialise a new PanelFactory object, assuming
        /// the ui form definition is from an unnamed ui definition
        /// </summary>
        /// <param name="bo">The business object to be represented</param>
        public PanelFactory(BusinessObject bo)
        {
            //_boArray = new BusinessObject[1];
            //_boArray[0] = bo;
            BOMapper mapper = new BOMapper(bo);
            _uiFormDef = mapper.GetUIDef().GetUIFormProperties();
            //_emailTextBoxDoubleClickedHandler = new EventHandler(EmailTextBoxDoubleClickedHandler);
            InitialiseFactory(bo);
        }

        /// <summary>
        /// A constructor to initialise a new instance, with a UIFormDef object 
        /// specified
        /// </summary>
        public PanelFactory(BusinessObject bo, UIFormDef uiFormDef)
        {
            _uiFormDef = uiFormDef;
            InitialiseFactory(bo);
        }

        /// <summary>
        /// A constructor as before, but with a UIDefName specified
        /// </summary>
        public PanelFactory(BusinessObject bo, string uiDefName)
        {
            BOMapper mapper = new BOMapper(bo);
            _uiFormDef = mapper.GetUIDef(uiDefName).GetUIFormProperties();
            InitialiseFactory(bo);
        }

        /// <summary>
        /// Initialises factory components
        /// </summary>
        private void InitialiseFactory(BusinessObject bo)
        {
            _boArray = new BusinessObject[1];
            _boArray[0] = bo;
            _emailTextBoxDoubleClickedHandler = new EventHandler(EmailTextBoxDoubleClickedHandler);
        }

        /// <summary>
        /// Creates a panel to display a business object
        /// </summary>
        /// <returns>Returns the object containing the panel</returns>
        public PanelFactoryInfo CreatePanel()
        {
            //log.Debug("Creating panel for object of type " + _boArray[0].GetType().Name ) ;
            PanelFactoryInfo factoryInfo;
            _firstControl = null;
            if (_uiFormDef.Count > 1)
            {
                Panel mainPanel = new Panel();
                ControlMapperCollection controlMappers = new ControlMapperCollection();
                IDictionary<string, EditableGrid> formGrids = new Dictionary<string, EditableGrid>();
                TabControl tabControl = new TabControl();
                tabControl.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(tabControl);
                foreach (UIFormTab formTab in _uiFormDef)
                {
                    PanelFactoryInfo onePanelInfo = CreateOnePanel(formTab);
                    foreach (ControlMapper controlMapper in onePanelInfo.ControlMappers)
                    {
                        controlMappers.Add(controlMapper);
                    }
                    foreach (KeyValuePair<string, EditableGrid> keyValuePair in onePanelInfo.FormGrids)
                    {
                        formGrids.Add(keyValuePair);
                    }

                    onePanelInfo.Panel.Dock = DockStyle.Fill;
                    TabPage page = new TabPage(formTab.Name);
                    page.Controls.Add(onePanelInfo.Panel);
                    tabControl.TabPages.Add(page);
                }
                factoryInfo = new PanelFactoryInfo(mainPanel, controlMappers, _firstControl);
                factoryInfo.FormGrids = formGrids;
            }
            else
            {
                factoryInfo = CreateOnePanel(_uiFormDef[0]);
            }
            if (_uiFormDef.Height > -1)
            {
                factoryInfo.PreferredHeight = _uiFormDef.Height;
            }
            if (_uiFormDef.Width > -1)
            {
                factoryInfo.PreferredWidth = _uiFormDef.Width;
            }
            return factoryInfo;
        }

        /// <summary>
        /// Creates a panel with the controls and dimensions as prescribed
        /// </summary>
        /// <param name="uiFormTab">The UIFormTab object</param>
        /// <returns>Returns the object containing the new panel</returns>
        /// TODO ERIC - this is a very long method!
        private PanelFactoryInfo CreateOnePanel(UIFormTab uiFormTab)
        {
            if (uiFormTab.UIFormGrid != null)
            {
                return CreatePanelWithGrid(uiFormTab.UIFormGrid);
            }
            Panel p = new Panel();
            GridLayoutManager manager = new GridLayoutManager(p);
            int rowCount = 0;
            int colCount = 0;
            colCount += uiFormTab.Count;
            foreach (UIFormColumn uiFormColumn in uiFormTab)
            {
                if (uiFormColumn.Count > rowCount)
                {
                    rowCount = uiFormColumn.Count;
                }
            }
            manager.SetGridSize(rowCount, colCount*2);
            for (int col = 0; col < colCount; col++)
            {
                manager.FixColumnBasedOnContents(col*2);
            }

            ControlMapperCollection controlMappers = new ControlMapperCollection();

            TextBox temptb = ControlFactory.CreateTextBox();
            for (int row = 0; row < rowCount; row++)
            {
                manager.FixRow(row, temptb.Height);
            }
            //manager.FixAllRowsBasedOnContents();
            GridLayoutManager.ControlInfo[,] controls = new GridLayoutManager.ControlInfo[rowCount,colCount*2];
            int currentColumn = 0;
            foreach (UIFormColumn uiFormColumn in uiFormTab)
            {
                int currentRow = 0;
                foreach (UIFormProperty property in uiFormColumn)
                {
                    //log.Debug("Creating label and control for property " + property.PropertyName + " with mapper type " + property.MapperTypeName) ;
                    bool isCompulsory = false;
                    PropDef propDef = _boArray[0].ClassDef.GetPropDef(property.PropertyName);
                    if (propDef != null)
                    {
                        isCompulsory = propDef.Compulsory;
                    }
                    else
                    {
                        isCompulsory = false;
                    }
                    controls[currentRow, currentColumn + 0] =
                        new GridLayoutManager.ControlInfo(ControlFactory.CreateLabel(property.Label, isCompulsory));
                    Control ctl = ControlFactory.CreateControl(property.ControlType);
                    ctl.Enabled = property.Editable;
                    if (!ctl.Enabled)
                    {
                        ctl.ForeColor = Color.Black;
                        ctl.BackColor = Color.Beige;
                    }
                    else
                    {
                        if (_firstControl == null)
                        {
                            _firstControl = ctl;
                        }
                    }
                    if (ctl is TextBox)
                    {
                        if (property.GetParameterValue("numLines") != null)
                        {
                            int numLines = 0;
                            try
                            {
                                numLines = Convert.ToInt32(property.GetParameterValue("numLines"));
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidXmlDefinitionException("An error " +
                                    "occurred while reading the 'numLines' parameter " +
                                    "from the class definitions.  The 'value' " +
                                    "attribute must be a valid integer.");
                            }
                            if (numLines > 1)
                            {
                                TextBox tb = (TextBox) ctl;
                                tb.Multiline = true;
                                tb.Height = tb.Height*numLines;
                                tb.ScrollBars = ScrollBars.Vertical;
                            }
                        }
                        if (property.GetParameterValue("isEmail") != null)
                        {
                            string isEmailValue = (string)property.GetParameterValue("isEmail");
                            if (isEmailValue != "true" && isEmailValue != "false")
                            {
                                throw new InvalidXmlDefinitionException("An error " +
                                    "occurred while reading the 'isEmail' parameter " +
                                    "from the class definitions.  The 'value' " +
                                    "attribute must hold either 'true' or 'false'.");
                            }
                            bool isEmail = Convert.ToBoolean(isEmailValue);
                            if (isEmail)
                            {
                                TextBox tb = (TextBox) ctl;
                                tb.DoubleClick += _emailTextBoxDoubleClickedHandler;
                            }
                        }
                    }
                    if (ctl is DateTimePicker)
                    {
                        DateTimePicker editor = (DateTimePicker) ctl;
                        //if (property.GetParameterValue("displayDatePicker") != null )
                        //{
                        //    bool isDropDown = Convert.ToBoolean(property.GetParameterValue("displayDatePicker") ) ;
                        //    if (!isDropDown) 
                        //    {
                        //        editor.DropDownButtonDisplayStyle = ButtonDisplayStyle.Never ; 
                        //    }
                        //    editor.
                        //}
                        editor.Enter += new EventHandler(DateTimePickerEnterHandler);
                    }
                    if (ctl is NumericUpDown)
                    {
                        NumericUpDown upDown = (NumericUpDown) ctl;
                        upDown.Enter += new EventHandler(UpDownEnterHandler);
                    }
                    ControlMapper ctlMapper =
                        ControlMapper.Create(property.MapperTypeName, ctl, property.PropertyName,  !property.Editable);
                    ctlMapper.SetPropertyAttributes(property.Parameters);
                    controlMappers.Add(ctlMapper);
                    ctlMapper.BusinessObject = _boArray[0];

                    int colSpan = 1;
                    if (property.GetParameterValue("colSpan") != null)
                    {
                        try
                        {
                            colSpan = Convert.ToInt32(property.GetParameterValue("colSpan"));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidXmlDefinitionException("An error " +
                                "occurred while reading the 'colSpan' parameter " +
                                "from the class definitions.  The 'value' " +
                                "attribute must be a valid integer.");
                        }
                    }
                    colSpan = (colSpan - 1)*2 + 1; // must span label columns too

                    int rowSpan = 1;
                    if (property.GetParameterValue("rowSpan") != null)
                    {
                        try
                        {
                            rowSpan = Convert.ToInt32(property.GetParameterValue("rowSpan"));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidXmlDefinitionException("An error " +
                                "occurred while reading the 'rowSpan' parameter " +
                                "from the class definitions.  The 'value' " +
                                "attribute must be a valid integer.");
                        }
                    }
                    if (rowSpan == 1)
                    {
                        manager.FixRow(currentRow, ctl.Height);
                    }
                    controls[currentRow, currentColumn + 1] = new GridLayoutManager.ControlInfo(ctl, rowSpan, colSpan);
                    currentRow++;
                    //log.Debug("Done creating label and control");
                }
                currentColumn += 2;
            }
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount*2; j++)
                {
                    if (controls[i, j] == null)
                    {
                        manager.AddControl(null);
                    }
                    else
                    {
                        manager.AddControl(controls[i, j].Control, controls[i, j].ColumnSpan, controls[i, j].RowSpan);
                        controls[i, j].Control.TabIndex = rowCount*j + i;
                        //j += controls[i, j].ColSpan - 1;
                    }
                }
            }
            for (int col = 0; col < colCount; col++)
            {
                if (uiFormTab[col].Width > -1)
                {
                    manager.FixColumn(col*2 + 1, uiFormTab[col].Width - manager.GetFixedColumnWidth(col*2));
                }
            }

            p.Height = manager.GetFixedHeightIncludingGaps();
            return new PanelFactoryInfo(p, controlMappers, _firstControl);
        }

        /// <summary>
        /// A handler to deal with the case of an entered panel
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void PanelEnteredHandler(object sender, EventArgs e)
        {
            _firstControl.Focus();
        }

        /// <summary>
        /// A handler to deal with the press of an Enter key when the control
        /// is an up-down object
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void UpDownEnterHandler(object sender, EventArgs e)
        {
            NumericUpDown upDown = (NumericUpDown) sender;
            upDown.Select(0, upDown.Text.Length);
        }

        /// <summary>
        /// A handler to deal with the press of an Enter key when the control
        /// is a date-time picker
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DateTimePickerEnterHandler(object sender, EventArgs e)
        {
            DateTimePicker editor = (DateTimePicker) sender;
            //editor.se
            //editor.SelectAll() ;
        }

        /// <summary>
        /// A handler to deal with a double-click on an email textbox, which
        /// causes the default mail client on the user system to be opened
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void EmailTextBoxDoubleClickedHandler(object sender, EventArgs e)
        {
            TextBox tb = (TextBox) sender;
            if (tb.Text.IndexOf("@") != -1)
            {
                string comm = "mailto:" + tb.Text;
                System.Diagnostics.Process.Start(comm);
            }
        }

        /// <summary>
        /// Creates a panel with a grid containing the business object
        /// information
        /// </summary>
        /// <param name="formGrid">The grid to fill</param>
        /// <returns>Returns the object containing the panel</returns>
        private PanelFactoryInfo CreatePanelWithGrid(UIFormGrid formGrid)
        {
            EditableGrid myGrid = new EditableGrid();

            BusinessObject bo = _boArray[0];
            ClassDef classDef = ClassDef.ClassDefs[bo.GetType()];
            myGrid.ObjectInitialiser =
                new RelationshipObjectInitialiser(bo, classDef.GetRelationship(formGrid.RelationshipName),
                                                  formGrid.CorrespondingRelationshipName);

            BusinessObjectCollection<BusinessObject> collection =
                bo.Relationships.GetRelatedBusinessObjectCol(formGrid.RelationshipName);
            //foreach (UIGridProperty property in collection.SampleBo.GetUserInterfaceMapper().GetUIGridProperties())
            //{
            //    //log.Debug("Heading: " + property.Heading + ", controlType: " + property.GridControlType.Name);
            //}

            myGrid.SetCollection(collection);

            myGrid.Dock = DockStyle.Fill;
            Panel p = ControlFactory.CreatePanel(formGrid.RelationshipName);
            p.Controls.Add(myGrid);

            PanelFactoryInfo pinfo = new PanelFactoryInfo(p);
            pinfo.FormGrids.Add(formGrid.RelationshipName, myGrid);
            return pinfo;
        }
    }
}