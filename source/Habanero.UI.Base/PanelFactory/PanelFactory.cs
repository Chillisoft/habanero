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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using log4net;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Creates panels for displaying business object details on a form.  Use
    /// CreatePanel to create the panel and catch the <see cref="IPanelFactoryInfo" /> generated,
    /// which contains all the information relating to the panel, including the controls, the
    /// mappers, the business object and the panel control.
    /// </summary>
    public class PanelFactory : IPanelFactory
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.PanelFactory");
        private BusinessObject _currentBusinessObject;
        private readonly UIForm _uiForm;
        private IControlHabanero _firstControl;
        private readonly IControlFactory _controlFactory;
        private readonly string _uiDefName;
        //TODO Port:        private EventHandler _emailTextBoxDoubleClickedHandler;

        /// <summary>
        /// Constructor to initialise a new PanelFactory object, assuming
        /// the ui form definition is from an unnamed ui definition
        /// </summary>
        /// <param name="bo">The business object to be represented</param>
        /// <param name="controlFactory">the control factory used to create controls</param>
        public PanelFactory(BusinessObject bo, IControlFactory controlFactory) : this(bo, "default", controlFactory)
        {
        }

        /// <summary>
        /// A constructor to initialise a new instance, with a UIForm object 
        /// specified
        /// </summary>
        /// <param name="bo">The business object to be represented</param>
        /// <param name="uiForm"></param>
        /// <param name="controlFactory">the control factory used to create controls</param>
        public PanelFactory(BusinessObject bo, UIForm uiForm, IControlFactory controlFactory)
        {
            if (uiForm == null) throw new ArgumentNullException("uiForm");
            _uiForm = uiForm;
            _controlFactory = controlFactory;
            InitialiseFactory(bo);
        }

        /// <summary>
        /// A constructor as before, but with a UIDefName specified
        /// </summary>
        public PanelFactory(BusinessObject bo, string uiDefName, IControlFactory controlFactory)
        {
            _uiDefName = uiDefName;
            _controlFactory = controlFactory;
            BOMapper mapper = new BOMapper(bo);

            UIDef uiDef = mapper.GetUIDef(uiDefName);
            if (uiDef == null)
            {
                string errMessage = "Cannot create a panel factory for '" + bo.ClassDef.ClassName
                                    + "' since the classdefs do not contain a uiDef '" + uiDefName + "'";
                throw new HabaneroDeveloperException
                    ("There is a serious application error please contact your system administrator"
                     + Environment.NewLine + errMessage, errMessage);
            }
            _uiForm = uiDef.GetUIFormProperties();
            if (_uiForm == null)
            {
                string errMsg = "Cannot create a panel factory for '" + bo.ClassDef.ClassName
                                + "' since the classdefs do not contain a form def for uidef '" + uiDefName + "'";
                throw new HabaneroDeveloperException
                    ("There is a serious application error please contact your system administrator"
                     + Environment.NewLine + errMsg, errMsg);
            }
            InitialiseFactory(bo);
        }

        /// <summary>
        /// Initialises factory components
        /// </summary>
        private void InitialiseFactory(BusinessObject bo)
        {
            _currentBusinessObject = bo;
            //TODO Port:            _emailTextBoxDoubleClickedHandler = EmailTextBoxDoubleClickedHandler;
        }

        /// <summary>
        /// Creates a panel to display a business object
        /// </summary>
        /// <returns>Returns the panel info object containing the panel</returns>
        public IPanelFactoryInfo CreatePanel()
        {
            IPanelFactoryInfo factoryInfo;
            _firstControl = null;
            if (_uiForm.Count > 1)
            {
                IPanel mainPanel = _controlFactory.CreatePanel(_controlFactory);
                ControlMapperCollection controlMappers = new ControlMapperCollection();
                IDictionary<string, IEditableGridControl> formGrids = new Dictionary<string, IEditableGridControl>();
                ITabControl tabControl = _controlFactory.CreateTabControl();
                BorderLayoutManager mainPanelManager = _controlFactory.CreateBorderLayoutManager(mainPanel);
                mainPanelManager.AddControl(tabControl, BorderLayoutManager.Position.Centre);
                foreach (UIFormTab formTab in _uiForm)
                {
                    IPanelFactoryInfo onePanelInfo = CreateOnePanel(formTab);
                    AddControlMappers(onePanelInfo, controlMappers);
                    AddFormGrids(onePanelInfo, formGrids);
                    ITabPage page = _controlFactory.CreateTabPage(formTab.Name);
                    BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(page);
                    manager.AddControl(onePanelInfo.Panel, BorderLayoutManager.Position.Centre);
                    tabControl.TabPages.Add(page);
                }
                factoryInfo = new PanelFactoryInfo(mainPanel, controlMappers, _uiDefName, _firstControl);
                // TODO : This should not set FormGrid, this should be populated some other way
                factoryInfo.FormGrids = formGrids;
            }
            else
            {
                factoryInfo = CreateOnePanel(_uiForm[0]);
            }
            SetFormPreferredHeight(factoryInfo);
            //TODO_Port AttachTriggers(_uiForm, factoryInfo, _currentBusinessObject);
            return factoryInfo;
        }

        private static void AddFormGrids(IPanelFactoryInfo onePanelInfo, IDictionary<string, IEditableGridControl> formGrids)
        {
            foreach (KeyValuePair<string, IEditableGridControl> keyValuePair in onePanelInfo.FormGrids)
            {
                formGrids.Add(keyValuePair);
            }
        }

        /// <summary>
        /// Creates one panel for each UI Form definition of a business object
        /// </summary>
        /// <returns>Returns the list of panel info objects created</returns>
        /// TODO: improve tab order (ie make all tabs use one sequence rather than each starting a new sequence)
        public List<IPanelFactoryInfo> CreateOnePanelPerUIFormTab()
        {
            List<IPanelFactoryInfo> panelInfoList = new List<IPanelFactoryInfo>();
            foreach (UIFormTab formTab in _uiForm)
            {
                IPanelFactoryInfo onePanelInfo = CreateOnePanel(formTab);

                panelInfoList.Add(onePanelInfo);
            }
            return panelInfoList;
        }


        private static void AddControlMappers(IPanelFactoryInfo onePanelInfo, ControlMapperCollection controlMappers)
        {
            foreach (ControlMapper controlMapper in onePanelInfo.ControlMappers)
            {
                controlMappers.Add(controlMapper);
            }
        }

        private void SetFormPreferredHeight(IPanelFactoryInfo factoryInfo)
        {
            if (_uiForm.Height > -1)
            {
                factoryInfo.PreferredHeight = _uiForm.Height;
            }
            if (_uiForm.Width > -1)
            {
                factoryInfo.PreferredWidth = _uiForm.Width;
            }
        }

        /// <summary>
        /// Creates a panel with the controls and dimensions as prescribed
        /// </summary>
        /// <param name="uiFormTab">The UIFormTab object</param>
        /// <returns>Returns the object containing the new panel</returns>
        private IPanelFactoryInfo CreateOnePanel(UIFormTab uiFormTab)
        {
            if (uiFormTab.UIFormGrid != null)
            {
                return CreatePanelWithGrid(uiFormTab.UIFormGrid);
            }
            IPanel panel = _controlFactory.CreatePanel(_controlFactory);
            IToolTip toolTip = _controlFactory.CreateToolTip();
            GridLayoutManager manager = new GridLayoutManager(panel, _controlFactory);
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
            int intNoOfLayoutGridColumnsPerPanel = 3;
            manager.SetGridSize(rowCount, colCount * intNoOfLayoutGridColumnsPerPanel);
            for (int col = 0; col < colCount; col++)
            {
                //Fixing the labels column widths
                manager.FixColumnBasedOnContents(col * intNoOfLayoutGridColumnsPerPanel);
            }

            ControlMapperCollection controlMappers = new ControlMapperCollection();
            controlMappers.BusinessObject = _currentBusinessObject;
            ITextBox temptb = _controlFactory.CreateTextBox();
            for (int row = 0; row < rowCount; row++)
            {
                manager.FixRow(row, temptb.Height);
            }
            manager.FixAllRowsBasedOnContents();
            GridLayoutManager.ControlInfo[,] controls =
                new GridLayoutManager.ControlInfo[rowCount,colCount * intNoOfLayoutGridColumnsPerPanel];
            int currentColumn = 0;
            foreach (UIFormColumn uiFormColumn in uiFormTab)
            {
                int currentRow = 0;
                foreach (UIFormField field in uiFormColumn)
                {
                    bool isCompulsory;
                    ClassDef classDef = _currentBusinessObject.ClassDef;
                    IPropDef propDef = field.GetPropDefIfExists(classDef);
                    if (propDef != null)
                    {
                        isCompulsory = propDef.Compulsory;
                    }
                    else
                    {
                        isCompulsory = false;
                    }
                    string labelCaption = field.GetLabel(classDef);
//                    BOPropCol boPropCol = _currentBusinessObject.Props;
//                    if (boPropCol.Contains(field.PropertyName))
//                    {
//                        IBOProp boProp = boPropCol[field.PropertyName];
//                        if (!boProp.HasDisplayName())
//                        {
//                            boProp.DisplayName = labelCaption;
//                        }
//                    }

                    ILabel labelControl = _controlFactory.CreateLabel(labelCaption, isCompulsory);
                    controls[currentRow, currentColumn + 0] = new GridLayoutManager.ControlInfo(labelControl);
                    IControlHabanero ctl = CreateControl(field, _controlFactory);

                    if (ctl is ITextBox && propDef != null)
                    {
                        if (propDef.PropertyType == typeof (bool))
                        {
                            ctl = _controlFactory.CreateCheckBox();
                        }
                        else if (propDef.PropertyType == typeof (string) && propDef.KeepValuePrivate)
                        {
                            ctl = _controlFactory.CreatePasswordTextBox();
                        }
                        else if (field.GetParameterValue("numLines") != null)
                        {
                            int numLines;
                            try
                            {
                                numLines = Convert.ToInt32(field.GetParameterValue("numLines"));
                            }
                            catch (Exception)
                            {
                                throw new InvalidXmlDefinitionException
                                    ("An error " + "occurred while reading the 'numLines' parameter "
                                     + "from the class definitions.  The 'value' "
                                     + "attribute must be a valid integer.");
                            }
                            if (numLines > 1)
                            {
                                ctl = _controlFactory.CreateTextBoxMultiLine(numLines);
                            }
                        }
                    }

                    bool editable = CheckIfEditable(field, ctl);

                    if (ctl is ITextBox)
                    {
                        if (field.GetParameterValue("isEmail") != null)
                        {
                            string isEmailValue = (string) field.GetParameterValue("isEmail");
                            if (isEmailValue != "true" && isEmailValue != "false")
                            {
                                throw new InvalidXmlDefinitionException
                                    ("An error " + "occurred while reading the 'isEmail' parameter "
                                     + "from the class definitions.  The 'value' "
                                     + "attribute must hold either 'true' or 'false'.");
                            }

                            //TODO: Port
                            //bool isEmail = Convert.ToBoolean(isEmailValue);
                            //if (isEmail)
                            //{
                            //    ITextBox tb = (ITextBox) ctl;
                            //    tb.DoubleClick += _emailTextBoxDoubleClickedHandler;
                            //}
                        }
                    }
                    //TODO: Port
                    //if (ctl is IDateTimePicker)
                    //{
                    //    IDateTimePicker editor = (IDateTimePicker) ctl;
                    //    editor.Enter += DateTimePickerEnterHandler;
                    //}
                    //if (ctl is INumericUpDown)
                    //{
                    //    INumericUpDown upDown = (INumericUpDown) ctl;
                    //    upDown.Enter += UpDownEnterHandler;
                    //}


                    CheckGeneralParameters(field, ctl);

                    IControlMapper ctlMapper =
                        ControlMapper.Create
                            (field.MapperTypeName, field.MapperAssembly, ctl, field.PropertyName, !editable,
                             _controlFactory);
                    if (ctlMapper is ControlMapper)
                    {
                        ControlMapper controlMapper = (ControlMapper) ctlMapper;
                        controlMapper.SetPropertyAttributes(field.Parameters);
                        //TODO: This was a quick fix to get his working. Investigate where the SetPropertyAttributes method should go.
                    }
                    controlMappers.Add(ctlMapper);
                    ctlMapper.BusinessObject = _currentBusinessObject;

                    int colSpan = 1;
                    if (field.GetParameterValue("colSpan") != null)
                    {
                        try
                        {
                            colSpan = Convert.ToInt32(field.GetParameterValue("colSpan"));
                        }
                        catch (Exception)
                        {
                            throw new InvalidXmlDefinitionException
                                ("An error " + "occurred while reading the 'colSpan' parameter "
                                 + "from the class definitions.  The 'value' " + "attribute must be a valid integer.");
                        }
                    }
                    colSpan = (colSpan - 1) * intNoOfLayoutGridColumnsPerPanel + 1; // must span label columns too

                    int rowSpan = 1;
                    if (field.GetParameterValue("rowSpan") != null)
                    {
                        try
                        {
                            rowSpan = Convert.ToInt32(field.GetParameterValue("rowSpan"));
                        }
                        catch (Exception)
                        {
                            throw new InvalidXmlDefinitionException
                                ("An error " + "occurred while reading the 'rowSpan' parameter "
                                 + "from the class definitions.  The 'value' " + "attribute must be a valid integer.");
                        }
                    }
                    if (rowSpan == 1)
                    {
                        manager.FixRow(currentRow, ctl.Height);
                    }
                    controls[currentRow, currentColumn + 1] = new GridLayoutManager.ControlInfo(ctl, rowSpan, colSpan);
                    currentRow++;
                    string toolTipText = field.GetToolTipText(classDef);
                    if (!String.IsNullOrEmpty(toolTipText))
                    {
                        toolTip.SetToolTip(labelControl, toolTipText);
                        toolTip.SetToolTip(ctl, toolTipText);
                    }
                    //Hack brett trying to fix prob with dynamic properties
                    ctl.Width = 100;
                }

                currentColumn += 3;
            }
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount * intNoOfLayoutGridColumnsPerPanel; j++)
                {
                    if (controls[i, j] == null)
                    {
                        manager.AddControl(null);
                    }
                    else
                    {
                        manager.AddControl(new GridLayoutManager.ControlInfo(controls[i, j].Control, controls[i, j].ColumnSpan, controls[i, j].RowSpan));
                        controls[i, j].Control.TabIndex = rowCount * j + i;
                    }
                }
            }
            for (int col = 0; col < colCount; col++)
            {
                if (uiFormTab[col].Width > -1)
                {
                    //Fix width of the control column e.g. textbox or combobox.
                    manager.FixColumn(col * intNoOfLayoutGridColumnsPerPanel + 1,uiFormTab[col].Width - manager.GetFixedColumnWidth(col * intNoOfLayoutGridColumnsPerPanel));
                }
                manager.FixColumn(col * intNoOfLayoutGridColumnsPerPanel + 2, 15);
            }

            panel.Height = manager.GetFixedHeightIncludingGaps();
            panel.Width = manager.GetFixedWidthIncludingGaps();
            IPanelFactoryInfo panelFactoryInfo = new PanelFactoryInfo(panel, controlMappers, _uiDefName, _firstControl);
            panelFactoryInfo.MinimumPanelHeight = panel.Height;
            panelFactoryInfo.MinumumPanelWidth = panel.Width;
            panelFactoryInfo.ToolTip = toolTip;
            panelFactoryInfo.PanelTabText = uiFormTab.Name;
            panelFactoryInfo.UIForm = _uiForm;
            panelFactoryInfo.UiFormTab = uiFormTab;
            return panelFactoryInfo;
        }

        /// <summary>
        /// Checks for a range of parameters that may apply to some or all fields
        /// </summary>
        private static void CheckGeneralParameters(UIFormField field, IControlHabanero ctl)
        {
            if (field.GetParameterValue("alignment") != null)
            {
                //TODO_Port needs to be tested.

                //string alignmentParam = field.GetParameterValue("alignment").ToString().ToLower().Trim();
                //Gizmox.WebGUI.Forms.HorizontalAlignment alignment;
                //if (alignmentParam == "left") alignment = Gizmox.WebGUI.Forms.HorizontalAlignment.Left;
                //else if (alignmentParam == "right") alignment = Gizmox.WebGUI.Forms.HorizontalAlignment.Right;
                //else if (alignmentParam == "center") alignment = Gizmox.WebGUI.Forms.HorizontalAlignment.Center;
                //else if (alignmentParam == "centre") alignment = Gizmox.WebGUI.Forms.HorizontalAlignment.Center;
                //else
                //{
                //    throw new InvalidXmlDefinitionException(String.Format(
                //                                                "In a 'parameter' element, the value '{0}' given for 'alignment' was " +
                //                                                "invalid.  The available options are: left, right, center and centre.",
                //                                                alignmentParam));
                //}
                // if (ctl is ITextBox) ((ITextBox) ctl).TextAlign = alignment;
                // this if (ctl is INumericUpDown) ((INumericUpDown) ctl).TextAlign = alignment;
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
        private bool CheckIfEditable(UIFormField field, IControlHabanero ctl)
        {
            bool editable = field.Editable;
            if (editable)
            {
                if (_firstControl == null)
                {
                    _firstControl = ctl;
                }
            }
            if (editable)
            {
                object parameterValue = field.GetParameterValue("editable");
                if (parameterValue != null) editable = Convert.ToBoolean(parameterValue);

                parameterValue = field.GetParameterValue("readWriteRule");
                string writeRule = "";
                if (parameterValue != null) writeRule = Convert.ToString(parameterValue);
                if (writeRule.Length > 0)
                {
                    if (writeRule == "ReadOnly") editable = false;
                    if (writeRule == "WriteNew")
                    {
                        if (_currentBusinessObject.Status.IsNew)
                        {
                            editable = true;
                        }
                        else editable = false;

                    }
                    if (writeRule == "WriteNotNew")
                    {
                        if (_currentBusinessObject.Status.IsNew)
                        {
                            editable = false;
                        }
                        else editable = true;

                    }
                }
            }
            return editable;
        }


        ///// <summary>
        ///// A handler to deal with the case of an entered panel
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private void PanelEnteredHandler(object sender, EventArgs e)
        //{
        //    _firstControl.Focus();
        //}

        //TODO Port:
        ///// <summary>
        ///// A handler to deal with the press of an Enter key when the control
        ///// is an up-down object
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private static void UpDownEnterHandler(object sender, EventArgs e)
        //{
        //    INumericUpDown upDown = (INumericUpDown) sender;
        //    upDown.Select(0, upDown.Text.Length);
        //}
//TODO Port:
        ///// <summary>
        ///// A handler to deal with the press of an Enter key when the control
        ///// is a date-time picker
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private static void DateTimePickerEnterHandler(object sender, EventArgs e)
        //{
        //}

        //TODO Port:        ///// <summary>
        ///// A handler to deal with a double-click on an email textbox, which
        ///// causes the default mail client on the user system to be opened
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private static void EmailTextBoxDoubleClickedHandler(object sender, EventArgs e)
        //{
        //    ITextBox tb = (ITextBox) sender;
        //    if (tb.Text.IndexOf("@") != -1)
        //    {
        //        string comm = "mailto:" + tb.Text;
        //        Process.Start(comm);
        //    }
        //}

        /// <summary>
        /// Creates the appropriate control for the given field element.
        /// Preference is given to a specific type over the type and assembly names.
        /// </summary>
        private static IControlHabanero CreateControl(UIFormField field, IControlFactory factory)
        {
            if (field.ControlType != null)
            {
                return factory.CreateControl(field.ControlType);
            }
            else
            {
                return factory.CreateControl(field.ControlTypeName, field.ControlAssemblyName);
            }
        }

        // TODO: Finish port
        /// <summary>
        /// Creates a panel with a grid containing the business object
        /// information
        /// </summary>
        /// <param name="formGrid">The grid to fill</param>
        /// <returns>Returns the object containing the panel</returns>
        private PanelFactoryInfo CreatePanelWithGrid(UIFormGrid formGrid)
        {
            IEditableGridControl myGrid = _controlFactory.CreateEditableGridControl();

            BusinessObject bo = _currentBusinessObject;
            ClassDef classDef = bo.ClassDef;
            DataSetProvider dataSetProvider = myGrid.Grid.DataSetProvider as DataSetProvider;
            if (dataSetProvider != null)
            {
                dataSetProvider.ObjectInitialiser =
                    new RelationshipObjectInitialiser(bo, classDef.GetRelationship(formGrid.RelationshipName),
                                                      formGrid.CorrespondingRelationshipName);
            }
            IBusinessObjectCollection collection =
                bo.Relationships.GetRelatedCollection(formGrid.RelationshipName);
            //foreach (UIGridColumn property in collection.SampleBo.GetUserInterfaceMapper().GetUIGridProperties())
            //{
            //    //log.Debug("Heading: " + property.Heading + ", controlType: " + property.GridControlType.Name);
            //}

            myGrid.SetBusinessObjectCollection(collection);

            myGrid.Dock = DockStyle.Fill;
            IPanel panel = _controlFactory.CreatePanel(formGrid.RelationshipName, _controlFactory);
            panel.Controls.Add(myGrid);

            PanelFactoryInfo panelFactoryInfo = new PanelFactoryInfo(panel);
            panelFactoryInfo.FormGrids.Add(formGrid.RelationshipName, myGrid);
            return panelFactoryInfo;
        }

        //TODO_Port : removed all trigger stuff
        /// <summary>
        /// Cycles through the fields being displayed and attaches
        /// the triggers to the respective controls
        /// </summary>
        //private static void AttachTriggers(UIForm form, PanelFactoryInfo panel, BusinessObject bo)
        //{
        //    foreach (UIFormTab tab in form)
        //    {
        //        foreach (UIFormColumn column in tab)
        //        {
        //            foreach (UIFormField field in column)
        //            {
        //                foreach (Trigger trigger in field.Triggers)
        //                {
        //                    AttachSingleTrigger(field, trigger, panel, bo);
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Attaches the specified trigger to the relevant field control
        ///// </summary>
        ///// TODO ERIC: 
        ///// - deal with controls that don't have TextChanged, eg a Checkbox?
        ///// - use a Prop.Updated event rather than TextChanged? But what about reflected properties?
        ///// - support for DatabaseLookupList
        //private static void AttachSingleTrigger(UIFormField field, Trigger trigger, PanelFactoryInfo panel, BusinessObject bo)
        //{
        //    Trigger.CheckTriggerValid(trigger);

        //    string sourceProperty;
        //    string targetProperty = null;
        //    Control sourceControl;
        //    Control targetControl = null;

        //    if (trigger.TriggeredBy == null)
        //    {
        //        sourceProperty = field.PropertyName;
        //        sourceControl = panel.ControlMappers[field.PropertyName].Control;
        //        if (!String.IsNullOrEmpty(trigger.Target))
        //        {
        //            targetProperty = trigger.Target;
        //            targetControl = panel.ControlMappers[trigger.Target].Control;
        //            if (targetControl == null)
        //            {
        //                throw new ArgumentException(String.Format(
        //                                                "In a 'trigger', there was no 'target' field found " +
        //                                                "for the property '{0}'.", trigger.Target));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        targetProperty = field.PropertyName;
        //        targetControl = panel.ControlMappers[field.PropertyName].Control;
        //        sourceProperty = trigger.TriggeredBy;
        //        sourceControl = panel.ControlMappers[trigger.TriggeredBy].Control;
        //        if (sourceControl == null)
        //        {
        //            throw new ArgumentException(String.Format(
        //                                            "In a 'trigger', there was no 'triggeredBy' field found " +
        //                                            "for the property '{0}'.", trigger.TriggeredBy));
        //        }
        //    }

        //    switch (trigger.Action)
        //    {
        //        case "assignLiteral":
        //            AddTriggerForAssignLiteral(sourceControl, targetControl, trigger);
        //            break;
        //        case "assignProperty":
        //            AddTriggerForAssignProperty(sourceControl, targetControl, trigger, bo);
        //            break;
        //        case "execute":
        //            AddTriggerForExecute(sourceControl, trigger, bo);
        //            break;
        //        case "filter":
        //            AddTriggerForFilter(sourceControl, targetControl, trigger, panel, bo, sourceProperty, targetProperty);
        //            break;
        //        case "filterReverse":
        //            AddTriggerForFilterReverse(sourceControl, targetControl, trigger, bo, sourceProperty, targetProperty);
        //            break;
        //        case "setEditable":
        //        case "setEditableOnce":
        //            AddTriggerForEnable(sourceControl, targetControl, trigger);
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Adds a trigger that assigns a literal value to the
        ///// target property
        ///// </summary>
        //private static void AddTriggerForAssignLiteral(Control sourceControl, Control targetControl, Trigger trigger)
        //{
        //    Control relevantControl = sourceControl;
        //    if (targetControl != null) relevantControl = targetControl;
        //    sourceControl.TextChanged +=
        //        delegate
        //            {
        //                if (trigger.ConditionValue == null ||
        //                    sourceControl.Text == trigger.ConditionValue)
        //                {
        //                    relevantControl.Text = trigger.Value;
        //                }
        //            };
        //}

        ///// <summary>
        ///// Adds a trigger that assigns a property value to the
        ///// target property
        ///// </summary>
        //private static void AddTriggerForAssignProperty(Control sourceControl,
        //                                                Control targetControl, Trigger trigger, BusinessObject bo)
        //{
        //    Control relevantControl = sourceControl;
        //    if (targetControl != null) relevantControl = targetControl;
        //    sourceControl.TextChanged +=
        //        delegate
        //            {
        //                if (trigger.ConditionValue == null ||
        //                    sourceControl.Text == trigger.ConditionValue)
        //                {
        //                    relevantControl.Text = ReflectionUtilities.GetPropertyValue(bo, trigger.Value).ToString();
        //                }
        //            };
        //}

        ///// <summary>
        ///// Adds a new trigger that executes a named method on the BO of the
        ///// form when the control's value changes
        ///// </summary>
        //private static void AddTriggerForExecute(Control control, Trigger trigger, BusinessObject bo)
        //{
        //    control.TextChanged +=
        //        delegate
        //            {
        //                if (trigger.ConditionValue == null ||
        //                    control.Text == trigger.ConditionValue)
        //                {
        //                    ReflectionUtilities.ExecuteMethod(bo, trigger.Value);
        //                }
        //            };
        //}

        ///// <summary>
        ///// Adds a trigger that filters a lookup-list.  Assumes that the "child"
        ///// has a field that has the same name as the parent, and lists only those
        ///// items that have a matching value on that field, while also respecting
        ///// other existing lookup-list criteria.
        ///// </summary>
        //private static void AddTriggerForFilter(Control sourceControl, Control targetControl,
        //                                        Trigger trigger, PanelFactoryInfo panel, BusinessObject bo,
        //                                        string sourceProperty, string targetProperty)
        //{
        //    if (!(targetControl is ComboBox))
        //    {
        //        throw new ArgumentException(String.Format(
        //                                        "In a 'trigger' with a 'filter' action, the field for property " +
        //                                        "'{0}' must be a type of ComboBox.", targetProperty));
        //    }
        //    ComboBox targetComboBox = (ComboBox)targetControl;

        //    bo.Props[sourceProperty].Updated +=
        //        delegate
        //            {
        //                if (trigger.ConditionValue == null ||
        //                    sourceControl.Text == trigger.ConditionValue)
        //                {
        //                    targetComboBox.Items.Clear();
        //                    targetComboBox.Text = "";

        //                    object sourceValue = bo.Props[sourceProperty].Value;
        //                    if (sourceValue != null && sourceValue != DBNull.Value)
        //                    {
        //                        ILookupList ilookuplist = bo.ClassDef.PropDefcol[targetProperty].LookupList;
        //                        if (!(ilookuplist is BusinessObjectLookupList))
        //                        {
        //                            throw new ArgumentException(String.Format(
        //                                                            "In a 'trigger', the lookup-list for the property " +
        //                                                            "'{0}' must be a BusinessObjectLookupList.", targetProperty));
        //                        }
        //                        BusinessObjectLookupList lookupList = (BusinessObjectLookupList) ilookuplist;
        //                        string oldCriteria = lookupList.Criteria;
        //                        string newCriteria = oldCriteria;
        //                        if (newCriteria == null) newCriteria = "";
        //                        if (newCriteria.Length > 0) newCriteria += " AND ";
        //                        if (sourceValue is Guid)
        //                            newCriteria += sourceProperty + " = '" + ((Guid) sourceValue).ToString("B") + "'";
        //                        else newCriteria += sourceProperty + " = '" + sourceValue + "'";
        //                        lookupList.Criteria = newCriteria;

        //                        Dictionary<string, object> list = lookupList.GetLookupList(true);
        //                        LookupComboBoxMapper mapper = (LookupComboBoxMapper) panel.ControlMappers[targetProperty];
        //                        mapper.SetLookupList(list);

        //                        lookupList.Criteria = oldCriteria;
        //                        if (targetComboBox.Items.Count == 1)
        //                        {
        //                            targetComboBox.SelectedItem = targetComboBox.Items[0];
        //                        }
        //                        else if (targetComboBox.Items.Count == 2 && targetComboBox.Items[0].ToString() == "")
        //                        {
        //                            targetComboBox.SelectedItem = targetComboBox.Items[1];
        //                        }
        //                    }
        //                }
        //            };
        //}

        ///// <summary>
        ///// Adds a trigger that filters a lookup-list, but in the opposite
        ///// direction to to "filter".  If an item in a child list has been
        ///// chosen, the parent item can be set.  Assumes that the child has
        ///// a field with the same name as the parent ID.
        ///// </summary>
        //private static void AddTriggerForFilterReverse(Control sourceControl, Control targetControl,
        //                                               Trigger trigger, BusinessObject bo, string sourceProperty, string targetProperty)
        //{
        //    if (!(targetControl is ComboBox))
        //    {
        //        throw new ArgumentException(String.Format(
        //                                        "In a 'trigger' with a 'filterReverse' action, the field for property " +
        //                                        "'{0}' must be a type of ComboBox.", targetProperty));
        //    }
        //    ComboBox targetComboBox = (ComboBox)targetControl;

        //    bo.Props[sourceProperty].Updated +=
        //        delegate
        //            {
        //                if (trigger.ConditionValue == null ||
        //                    sourceControl.Text == trigger.ConditionValue)
        //                {
        //                    ILookupList iSourceLookup = bo.ClassDef.PropDefcol[sourceProperty].LookupList;
        //                    ILookupList iTargetLookup = bo.ClassDef.PropDefcol[targetProperty].LookupList;
        //                    if (!(iSourceLookup is BusinessObjectLookupList) ||
        //                        !(iTargetLookup is BusinessObjectLookupList))
        //                    {
        //                        throw new ArgumentException(String.Format(
        //                                                        "In a 'trigger', the lookup-list for the source property '{0}' and the target property " +
        //                                                        "'{1}' must be a BusinessObjectLookupList.", sourceProperty, targetProperty));
        //                    }
        //                    BusinessObjectLookupList sourceLookup = (BusinessObjectLookupList)iSourceLookup;
        //                    BusinessObjectLookupList targetLookup = (BusinessObjectLookupList)iTargetLookup;

        //                    object sourceValue = bo.Props[sourceProperty].Value;
        //                    if (sourceValue != null && sourceValue != DBNull.Value)
        //                    {
        //                        string sourceCriteria;
        //                        if (sourceValue is Guid)
        //                            sourceCriteria = sourceProperty + " = '" + ((Guid)sourceValue).ToString("B") + "'";
        //                        else sourceCriteria = sourceProperty + " = '" + sourceValue + "'";

        //                        Type sourceType = TypeLoader.LoadType(sourceLookup.AssemblyName, sourceLookup.ClassName);
        //                        IBusinessObjectCollection sourceCol =
        //                            BOLoader.Instance.GetBusinessObjectCol(sourceType, sourceCriteria, "");

        //                        if (sourceCol.Count > 0)
        //                        {
        //                            object targetValue = sourceCol[0].Props[targetProperty].Value;
        //                            if (targetValue != null && targetValue != DBNull.Value)
        //                            {
        //                                string targetCriteria;
        //                                if (targetValue is Guid)
        //                                    targetCriteria = targetProperty + " = '" + ((Guid) targetValue).ToString("B") + "'";
        //                                else targetCriteria = targetProperty + " = '" + targetValue + "'";

        //                                Type targetType = TypeLoader.LoadType(targetLookup.AssemblyName, targetLookup.ClassName);
        //                                IBusinessObjectCollection targetCol =
        //                                    BOLoader.Instance.GetBusinessObjectCol(targetType, targetCriteria, "");

        //                                if (targetCol.Count > 0)
        //                                {
        //                                    targetComboBox.SelectedItem = targetCol[0].ToString();
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        targetComboBox.SelectedIndex = 0;
        //                    }
        //                }
        //            };
        //}

        ///// <summary>
        ///// Adds a trigger that enables or disables a control.  Disabling
        ///// is only added for the action of "setEditable" (ie. "setEditableOnce" only
        ///// carries out the action once).
        ///// </summary>
        //private static void AddTriggerForEnable(Control sourceControl, Control targetControl, Trigger trigger)
        //{
        //    bool enabled;
        //    if (!Boolean.TryParse(trigger.Value, out enabled))
        //    {
        //        throw new ArgumentException(String.Format(
        //                                        "In a 'trigger', the 'value' given as '{0}' was invalid. " +
        //                                        "Only 'true' and 'false' are valid.", trigger.Value));
        //    }
        //    Control relevantControl = sourceControl;
        //    if (targetControl != null) relevantControl = targetControl;
        //    sourceControl.TextChanged +=
        //        delegate
        //            {
        //                if (trigger.ConditionValue == null ||
        //                    sourceControl.Text == trigger.ConditionValue)
        //                {
        //                    relevantControl.Enabled = enabled;
        //                }
        //            };
        //    if (trigger.Action == "setEditable")
        //    {
        //        sourceControl.TextChanged +=
        //            delegate
        //                {
        //                    if (trigger.ConditionValue != null &&
        //                        sourceControl.Text != trigger.ConditionValue)
        //                    {
        //                        relevantControl.Enabled = !enabled;
        //                    }
        //                };
        //    }
        //}
    }
}
