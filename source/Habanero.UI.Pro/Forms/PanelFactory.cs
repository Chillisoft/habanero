using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Forms;
using Habanero.UI.Grid;
using log4net;
using System.Collections.Generic;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Creates panels that display business object information in a user
    /// interface
    /// </summary>
    public class PanelFactory
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.PanelFactory");
        private BusinessObject[] _boArray;
        //private IUserInterfaceMapper[] _uiArray;
        private UIForm _uiForm;
        private Control _firstControl;

        private EventHandler _emailTextBoxDoubleClickedHandler;

        /// <summary>
        /// Constructor to initialise a new PanelFactory object, assuming
        /// the ui form definition is from an unnamed ui definition
        /// </summary>
        /// <param name="bo">The business object to be represented</param>
        public PanelFactory(BusinessObject bo)
        {
            Permission.Check(this);
            //_boArray = new BusinessObject[1];
            //_boArray[0] = bo;
            BOMapper mapper = new BOMapper(bo);
            _uiForm = mapper.GetUIDef().GetUIFormProperties();
            //_emailTextBoxDoubleClickedHandler = new EventHandler(EmailTextBoxDoubleClickedHandler);
            InitialiseFactory(bo);
        }

        /// <summary>
        /// A constructor to initialise a new instance, with a UIForm object 
        /// specified
        /// </summary>
        public PanelFactory(BusinessObject bo, UIForm uiForm)
        {
            Permission.Check(this);
            _uiForm = uiForm;
            InitialiseFactory(bo);
        }

        /// <summary>
        /// A constructor as before, but with a UIDefName specified
        /// </summary>
        public PanelFactory(BusinessObject bo, string uiDefName)
        {
            Permission.Check(this);
            BOMapper mapper = new BOMapper(bo);
            _uiForm = mapper.GetUIDef(uiDefName).GetUIFormProperties();
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
            if (_uiForm.Count > 1)
            {
                Panel mainPanel = new Panel();
                ControlMapperCollection controlMappers = new ControlMapperCollection();
                IDictionary<string, EditableGrid> formGrids = new Dictionary<string, EditableGrid>();
                TabControl tabControl = new TabControl();
                tabControl.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(tabControl);
                foreach (UIFormTab formTab in _uiForm)
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
                factoryInfo = CreateOnePanel(_uiForm[0]);
            }
            if (_uiForm.Height > -1)
            {
                factoryInfo.PreferredHeight = _uiForm.Height;
            }
            if (_uiForm.Width > -1)
            {
                factoryInfo.PreferredWidth = _uiForm.Width;
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
                foreach (UIFormField field in uiFormColumn)
                {
                    //log.Debug("Creating label and control for property " + property.PropertyName + " with mapper type " + property.MapperTypeName) ;
                    bool isCompulsory = false;
                    PropDef propDef = _boArray[0].ClassDef.GetPropDef(field.PropertyName, false);
                    if (propDef != null)
                    {
                        isCompulsory = propDef.Compulsory;
                    }
                    else
                    {
                        isCompulsory = false;
                    }
                    if (_boArray[0].Props.Contains(field.PropertyName)) 
                        _boArray[0].Props[field.PropertyName].DisplayName = field.Label;

                    controls[currentRow, currentColumn + 0] =
                        new GridLayoutManager.ControlInfo(ControlFactory.CreateLabel(field.Label, isCompulsory));
                    Control ctl = ControlFactory.CreateControl(field.ControlType);

                    if (ctl is TextBox && _boArray[0].Props.Contains(field.PropertyName) && _boArray[0].Props[field.PropertyName].PropertyType == typeof(bool))
                    {
                        ctl = ControlFactory.CreateControl(typeof (CheckBox));
                    }

                    bool editable = CheckIfEditable(field, ctl);

                    if (ctl is TextBox)
                    {
                        if (field.GetParameterValue("numLines") != null)
                        {
                            int numLines = 0;
                            try
                            {
                                numLines = Convert.ToInt32(field.GetParameterValue("numLines"));
                            } catch (Exception)
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
                                tb.AcceptsReturn = true;
                                tb.Height = tb.Height*numLines;
                                tb.ScrollBars = ScrollBars.Vertical;
                            }
                        }
                        if (field.GetParameterValue("isEmail") != null)
                        {
                            string isEmailValue = (string)field.GetParameterValue("isEmail");
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

                    CheckGeneralParameters(field, ctl);

                    ControlMapper ctlMapper =
                        ControlMapper.Create(field.MapperTypeName, field.MapperAssembly, ctl, field.PropertyName, !editable);
                    ctlMapper.SetPropertyAttributes(field.Parameters);
                    controlMappers.Add(ctlMapper);
                    ctlMapper.BusinessObject = _boArray[0];

                    int colSpan = 1;
                    if (field.GetParameterValue("colSpan") != null)
                    {
                        try
                        {
                            colSpan = Convert.ToInt32(field.GetParameterValue("colSpan"));
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
                    if (field.GetParameterValue("rowSpan") != null)
                    {
                        try
                        {
                            rowSpan = Convert.ToInt32(field.GetParameterValue("rowSpan"));
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
            // TODO: Should this not be the PanelFactoryInfo Preferred Height and Width?
            p.Height = manager.GetFixedHeightIncludingGaps();
            p.Width = manager.GetFixedWidthIncludingGaps();
            return new PanelFactoryInfo(p, controlMappers, _firstControl);
        }

        /// <summary>
        /// Checks for a range of parameters that may apply to some or all fields
        /// </summary>
        private void CheckGeneralParameters(UIFormField field, Control ctl)
        {
            if (field.GetParameterValue("alignment") != null)
            {
                string alignmentParam = field.GetParameterValue("alignment").ToString().ToLower().Trim();
                HorizontalAlignment alignment;
                if (alignmentParam == "left") alignment = HorizontalAlignment.Left;
                else if (alignmentParam == "right") alignment = HorizontalAlignment.Right;
                else if (alignmentParam == "center") alignment = HorizontalAlignment.Center;
                else if (alignmentParam == "centre") alignment = HorizontalAlignment.Center;
                else
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In a 'parameter' element, the value '{0}' given for 'alignment' was " +
                        "invalid.  The available options are: left, right, center and centre.",
                        alignmentParam));
                }
                if (ctl is TextBox) ((TextBox) ctl).TextAlign = alignment;
                if (ctl is NumericUpDown) ((NumericUpDown) ctl).TextAlign = alignment;
            }
        }

        /// <summary>
        /// Checks whether a given field should be editable and makes appropriate
        /// changes.  If the property is an ObjectID and the BO
        /// is not new, then editing should not be done.
        /// </summary>
        /// <param name="field">The field being added</param>
        /// <param name="ctl">The control being prepared</param>
        /// <returns>Returns true if editable</returns>
        private bool CheckIfEditable(UIFormField field, Control ctl)
        {
            bool editable = field.Editable;
			//if (_boArray[0].ClassDef.PrimaryKeyDef.IsObjectID &&
			//    _boArray[0].ID.Contains(field.PropertyName) &&
			//    !_boArray[0].State.IsNew)
			//{
			//    editable = false;
			//}
			//ctl.Enabled = editable;
			//if (!ctl.Enabled)
			//{
			//    ctl.ForeColor = Color.Black;
			//    ctl.BackColor = Color.Beige;
			//}
			//else
			if (editable)
            {
                if (_firstControl == null)
                {
                    _firstControl = ctl;
                }
            }
            return editable;
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

			IBusinessObjectCollection collection =
                bo.Relationships.GetRelatedCollection(formGrid.RelationshipName);
            //foreach (UIGridColumn property in collection.SampleBo.GetUserInterfaceMapper().GetUIGridProperties())
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