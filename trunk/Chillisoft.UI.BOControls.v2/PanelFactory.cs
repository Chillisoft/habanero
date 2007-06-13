using System;
using System.Drawing;
using System.Windows.Forms;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;
using log4net;
using System.Collections.Generic;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Creates panels that display business object information in a user
    /// interface
    /// </summary>
    /// TODO ERIC - don't understand the dif btw dif create() methods
    public class PanelFactory
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.UI.BoControls.PanelFactory");
        private BusinessObjectBase[] _boArray;
        //private IUserInterfaceMapper[] _uiArray;
        private UIFormDef _uiFormDef;
        private Control _firstControl;

        private EventHandler _emailTextBoxDoubleClickedHandler;

        /// <summary>
        /// Constructor to initialise a new PanelFactory object
        /// </summary>
        /// <param name="bo">The business object to be represented</param>
        public PanelFactory(BusinessObjectBase bo)
        {
            _boArray = new BusinessObjectBase[1];
            _boArray[0] = bo;
            BOMapper mapper = new BOMapper(bo);
            _uiFormDef = mapper.GetUserInterfaceMapper().GetUIFormProperties();
            _emailTextBoxDoubleClickedHandler = new EventHandler(EmailTextBoxDoubleClickedHandler);
        }

        /// <summary>
        /// A constructor as before, but with a UIFormDef object specified
        /// </summary>
        public PanelFactory(BusinessObjectBase bo, UIFormDef uiFormDef) : this(bo)
        {
            _uiFormDef = uiFormDef;
        }

        /// <summary>
        /// A constructor as before, but with a UIDefName specified
        /// </summary>
        public PanelFactory(BusinessObjectBase bo, string uiDefName) : this(bo)
        {
            BOMapper mapper = new BOMapper(bo);
            _uiFormDef = mapper.GetUserInterfaceMapper(uiDefName).GetUIFormProperties();
            ;
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
                IDictionary<string, IGrid> formGrids = new Dictionary<string, IGrid> ();
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
                    foreach (KeyValuePair<string, IGrid> keyValuePair in onePanelInfo.FormGrids)
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
                //factoryInfo.Panel.Height = _uiFormDef.Height;
            }
            if (_uiFormDef.Width > -1)
            {
                factoryInfo.PreferredWidth = _uiFormDef.Width;
                //factoryInfo.Panel.Width = _uiFormDef.Width;
            }
            //log.Debug("Done Creating panel for object of type " + _boArray[0].GetType().Name ) ;
            return factoryInfo;
        }

        /// <summary>
        /// Creates a panel with the controls and dimensions as prescribed
        /// </summary>
        /// <param name="uiFormTab">The UIFormTab object</param>
        /// <returns>Returns the object containing the new panel</returns>
        /// TODO ERIC - this is a very long method!
        /// - improve comments above
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
                    if (propDef != null && propDef.PropRule != null)
                    {
                        isCompulsory = propDef.PropRule.IsCompulsory;
                    }
                    else
                    {
                        isCompulsory = false;
                    }
                    controls[currentRow, currentColumn + 0] =
                        new GridLayoutManager.ControlInfo(ControlFactory.CreateLabel(property.Label, isCompulsory));
                    Control ctl = ControlFactory.CreateControl(property.ControlType);
                    ctl.Enabled = !property.IsReadOnly;
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
                        if (property.GetAttributeValue("numLines") != null)
                        {
                            int numLines = Convert.ToInt32(property.GetAttributeValue("numLines"));
                            if (numLines > 1)
                            {
                                TextBox tb = (TextBox) ctl;
                                tb.Multiline = true;
                                tb.Height = tb.Height*numLines;
                                tb.ScrollBars = ScrollBars.Vertical;
                            }
                        }
                        if (property.GetAttributeValue("isEmail") != null)
                        {
                            bool isEmail = Convert.ToBoolean(property.GetAttributeValue("isEmail"));
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
                        //if (property.GetAttributeValue("displayDatePicker") != null )
                        //{
                        //    bool isDropDown = Convert.ToBoolean(property.GetAttributeValue("displayDatePicker") ) ;
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
                        ControlMapper.Create(property.MapperTypeName, ctl, property.PropertyName, property.IsReadOnly);
                    ctlMapper.SetPropertyAttributes(property.Attributes);
                    controlMappers.Add(ctlMapper);
                    ctlMapper.BusinessObject = _boArray[0];

                    int colSpan = 1;
                    if (property.GetAttributeValue("colSpan") != null)
                    {
                        colSpan = Convert.ToInt32(property.GetAttributeValue("colSpan"));
                    }
                    colSpan = (colSpan - 1)*2 + 1; // must span label columns too

                    int rowSpan = 1;
                    if (property.GetAttributeValue("rowSpan") != null)
                    {
                        rowSpan = Convert.ToInt32(property.GetAttributeValue("rowSpan"));
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
        /// TODO ERIC - entered? is this used?
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
        /// TODO ERIC - enter key or "entered into", see below and above also
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
            //log.Debug("Creating a panel with a grid on it with relationship " + formGrid.RelationshipName);
            IGrid myGrid = (IGrid) Activator.CreateInstance(formGrid.GridType);
            //GridBase myGrid = myGridWithButtons.Grid;
            BusinessObjectBase bo = _boArray[0];
            ClassDef classDef = ClassDef.GetClassDefCol[bo.GetType()];
            //Console.Out.WriteLine(classDef.RelationshipDefCol);
            myGrid.ObjectInitialiser =
                new RelationshipObjectInitialiser(bo, classDef.GetRelationship(formGrid.RelationshipName),
                                                  formGrid.CorrespondingRelationshipName);
            //log.Debug("Listing UI Grid properties");
            BusinessObjectBaseCollection collection =
                bo.Relationships.GetRelatedBusinessObjectCol(formGrid.RelationshipName);
            foreach (UIGridProperty property in collection.SampleBo.GetUserInterfaceMapper().GetUIGridProperties())
            {
                //log.Debug("Heading: " + property.Heading + ", controlType: " + property.GridControlType.Name);
            }
            myGrid.SetGridDataProvider(
                new SimpleGridDataProvider(collection,
                                           collection.SampleBo.GetUserInterfaceMapper().GetUIGridProperties()));
            ((Control) myGrid).Dock = DockStyle.Fill;
            Panel p = ControlFactory.CreatePanel(formGrid.RelationshipName );
            p.Controls.Add((Control) myGrid);
            PanelFactoryInfo pinfo = new PanelFactoryInfo(p);
            pinfo.FormGrids.Add(formGrid.RelationshipName, myGrid);
            return pinfo;
        }
    }
}