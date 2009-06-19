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
//using log4net;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Creates panels for displaying business object details on a form.  Use
    /// CreatePanel to create the panel and catch the <see cref="IPanelFactoryInfo" /> generated,
    /// which contains all the information relating to the panel, including the controls, the
    /// mappers, the business object and the panel control.
    /// </summary>
    [Obsolete("Panelfactory and PanelFactoryInfo has been replaced by PanelBuild and PanelInfo.")]
    public class PanelFactory : IPanelFactory
    {
//        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.PanelFactory");
        private BusinessObject _currentBusinessObject;
        private readonly IUIForm _uiForm;
        private IControlHabanero _firstControl;
        private readonly IControlFactory _controlFactory;
        private readonly string _uiDefName;

        /// <summary>
        /// Constructor to initialise a new PanelFactory object, assuming
        /// the ui form definition is from an unnamed ui definition
        /// </summary>
        /// <param name="bo">The business object to be represented</param>
        /// <param name="controlFactory">the control factory used to create controls</param>
        public PanelFactory(BusinessObject bo, IControlFactory controlFactory)
            : this(bo, "default", controlFactory)
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

            IUIDef uiDef = mapper.GetUIDef(uiDefName);
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
        private IPanelFactoryInfo CreateOnePanel(IUIFormTab uiFormTab)
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
            const int intNoOfLayoutGridColumnsPerPanel = 3;
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
                new GridLayoutManager.ControlInfo[rowCount, colCount * intNoOfLayoutGridColumnsPerPanel];
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
                        if (propDef.PropertyType == typeof(bool))
                        {
                            ctl = _controlFactory.CreateCheckBox();
                        }
                        else if (propDef.PropertyType == typeof(string) && propDef.KeepValuePrivate)
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
                            string isEmailValue = (string)field.GetParameterValue("isEmail");
                            if (isEmailValue != "true" && isEmailValue != "false")
                            {
                                throw new InvalidXmlDefinitionException
                                    ("An error " + "occurred while reading the 'isEmail' parameter "
                                     + "from the class definitions.  The 'value' "
                                     + "attribute must hold either 'true' or 'false'.");
                            }

                            //bool isEmail = Convert.ToBoolean(isEmailValue);
                            //if (isEmail)
                            //{
                            //    ITextBox tb = (ITextBox) ctl;
                            //    tb.DoubleClick += _emailTextBoxDoubleClickedHandler;
                            //}
                        }
                    }
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
                        ControlMapper controlMapper = (ControlMapper)ctlMapper;
                        controlMapper.SetPropertyAttributes(field.Parameters);
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
                        manager.AddControl(controls[i, j]);
                        controls[i, j].Control.TabIndex = rowCount * j + i;
                    }
                }
            }
            for (int col = 0; col < colCount; col++)
            {
                if (uiFormTab[col].Width > -1)
                {
                    //Fix width of the control column e.g. textbox or combobox.
                    manager.FixColumn(col * intNoOfLayoutGridColumnsPerPanel + 1, uiFormTab[col].Width - manager.GetFixedColumnWidth(col * intNoOfLayoutGridColumnsPerPanel));
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
        ///// <summary>
        ///// A handler to deal with the press of an Enter key when the control
        ///// is a date-time picker
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private static void DateTimePickerEnterHandler(object sender, EventArgs e)
        //{
        //}

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
            return factory.CreateControl(field.ControlTypeName, field.ControlAssemblyName);
        }

        /// <summary>
        /// Creates a panel with a grid containing the business object
        /// information
        /// </summary>
        /// <param name="formGrid">The grid to fill</param>
        /// <returns>Returns the object containing the panel</returns>
        private PanelFactoryInfo CreatePanelWithGrid(IUIFormGrid formGrid)
        {
            IEditableGridControl myGrid = _controlFactory.CreateEditableGridControl();

            BusinessObject bo = _currentBusinessObject;
            ClassDef classDef = bo.ClassDef;
            DataSetProvider dataSetProvider = myGrid.Grid.DataSetProvider as DataSetProvider;
            if (dataSetProvider != null)
            {
                dataSetProvider.ObjectInitialiser =
                    new RelationshipObjectInitialiser(bo, (RelationshipDef) classDef.GetRelationship(formGrid.RelationshipName),
                                                      formGrid.CorrespondingRelationshipName);
            }
            IBusinessObjectCollection collection =
                bo.Relationships.GetRelatedCollection(formGrid.RelationshipName);

            myGrid.SetBusinessObjectCollection(collection);

            myGrid.Dock = DockStyle.Fill;
            IPanel panel = _controlFactory.CreatePanel(formGrid.RelationshipName, _controlFactory);
            panel.Controls.Add(myGrid);

            PanelFactoryInfo panelFactoryInfo = new PanelFactoryInfo(panel);
            panelFactoryInfo.FormGrids.Add(formGrid.RelationshipName, myGrid);
            return panelFactoryInfo;
        }
    }
}
