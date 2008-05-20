//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Diagnostics;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI;
using Habanero.UI.Grid;
using Habanero.Util;
using Habanero.Util.File;
using log4net;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Creates panels that display business object information in a user
    /// interface
    /// </summary>
    public class PanelFactory
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Forms.PanelFactory");
        private BusinessObject _currentBusinessObject;
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
            _uiForm = uiForm;
            InitialiseFactory(bo);
        }

        /// <summary>
        /// A constructor as before, but with a UIDefName specified
        /// </summary>
        public PanelFactory(BusinessObject bo, string uiDefName)
        {
            BOMapper mapper = new BOMapper(bo);
            _uiForm = mapper.GetUIDef(uiDefName).GetUIFormProperties();
            InitialiseFactory(bo);
        }

        /// <summary>
        /// Initialises factory components
        /// </summary>
        private void InitialiseFactory(BusinessObject bo)
        {
            _currentBusinessObject = bo;
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
            AttachTriggers(_uiForm, factoryInfo, _currentBusinessObject);
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
            Panel panel = new Panel();
            ToolTip toolTip = new ToolTip();
            GridLayoutManager manager = new GridLayoutManager(panel);
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
                    ClassDef classDef = _currentBusinessObject.ClassDef;
                    //PropDef propDef = classDef.GetPropDef(field.PropertyName, false);
                    PropDef propDef = field.GetPropDefIfExists(classDef);
                    if (propDef != null)
                    {
                        isCompulsory = propDef.Compulsory;
                    }
                    else
                    {
                        isCompulsory = false;
                    }
                    string labelCaption = field.GetLabel(classDef);
                    BOPropCol boPropCol = _currentBusinessObject.Props;
                    if (boPropCol.Contains(field.PropertyName))
                    {
                        BOProp boProp = boPropCol[field.PropertyName];
                        if (!boProp.HasDisplayName())
                        {
                            boProp.DisplayName = labelCaption;
                        }
                    }

                    Label labelControl = ControlFactory.CreateLabel(labelCaption, isCompulsory);
                    controls[currentRow, currentColumn + 0] = new GridLayoutManager.ControlInfo(labelControl);
                    Control ctl = CreateControl(field);

                    if (ctl is TextBox && propDef != null)
                    {
                        if (propDef.PropertyType == typeof (bool))
                        {
                            ctl = ControlFactory.CreateControl(typeof (CheckBox));
                        } else if (propDef.PropertyType == typeof(string) && propDef.KeepValuePrivate)
                        {
                            ctl = ControlFactory.CreatePasswordTextBox();
                        }
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
                    ctlMapper.BusinessObject = _currentBusinessObject;

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
                    string toolTipText = field.GetToolTipText(classDef);
                    if (!String.IsNullOrEmpty(toolTipText))
                    {
                        toolTip.SetToolTip(labelControl, toolTipText);
                        toolTip.SetToolTip(ctl, toolTipText);
                    }
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
            panel.Height = manager.GetFixedHeightIncludingGaps();
            panel.Width = manager.GetFixedWidthIncludingGaps();
            PanelFactoryInfo panelFactoryInfo = new PanelFactoryInfo(panel, controlMappers, _firstControl);
            panelFactoryInfo.ToolTip = toolTip;
            return panelFactoryInfo;
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
                Process.Start(comm);
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

            BusinessObject bo = _currentBusinessObject;
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

        /// <summary>
        /// Creates the appropriate control for the given field element.
        /// Preference is given to a specific type over the type and assembly names.
        /// </summary>
        private static Control CreateControl(UIFormField field)
        {
            if (field.ControlType != null)
            {
                return ControlFactory.CreateControl(field.ControlType);
            }
            else
            {
                return ControlFactory.CreateControl(field.ControlTypeName, field.ControlAssemblyName);
            }
        }

        /// <summary>
        /// Cycles through the fields being displayed and attaches
        /// the triggers to the respective controls
        /// </summary>
        private static void AttachTriggers(UIForm form, PanelFactoryInfo panel, BusinessObject bo)
        {
            foreach (UIFormTab tab in form)
            {
                foreach (UIFormColumn column in tab)
                {
                    foreach (UIFormField field in column)
                    {
                        foreach (Trigger trigger in field.Triggers)
                        {
                            AttachSingleTrigger(field, trigger, panel, bo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attaches the specified trigger to the relevant field control
        /// </summary>
        /// TODO ERIC: 
        /// - deal with controls that don't have TextChanged, eg a Checkbox?
        /// - use a Prop.Updated event rather than TextChanged? But what about reflected properties?
        /// - support for DatabaseLookupList
        private static void AttachSingleTrigger(UIFormField field, Trigger trigger, PanelFactoryInfo panel, BusinessObject bo)
        {
            Trigger.CheckTriggerValid(trigger);

            string sourceProperty = null;
            string targetProperty = null;
            Control sourceControl = null;
            Control targetControl = null;

            if (trigger.TriggeredBy == null)
            {
                sourceProperty = field.PropertyName;
                sourceControl = panel.ControlMappers[field.PropertyName].Control;
                if (!String.IsNullOrEmpty(trigger.Target))
                {
                    targetProperty = trigger.Target;
                    targetControl = panel.ControlMappers[trigger.Target].Control;
                    if (targetControl == null)
                    {
                        throw new ArgumentException(String.Format(
                                                        "In a 'trigger', there was no 'target' field found " +
                                                        "for the property '{0}'.", trigger.Target));
                    }
                }
            }
            else
            {
                targetProperty = field.PropertyName;
                targetControl = panel.ControlMappers[field.PropertyName].Control;
                sourceProperty = trigger.TriggeredBy;
                sourceControl = panel.ControlMappers[trigger.TriggeredBy].Control;
                if (sourceControl == null)
                {
                    throw new ArgumentException(String.Format(
                                                    "In a 'trigger', there was no 'triggeredBy' field found " +
                                                    "for the property '{0}'.", trigger.TriggeredBy));
                }
            }

            switch (trigger.Action)
            {
                case "assignLiteral":
                    AddTriggerForAssignLiteral(sourceControl, targetControl, trigger);
                    break;
                case "assignProperty":
                    AddTriggerForAssignProperty(sourceControl, targetControl, trigger, bo);
                    break;
                case "execute":
                    AddTriggerForExecute(sourceControl, trigger, bo);
                    break;
                case "filter":
                    AddTriggerForFilter(sourceControl, targetControl, trigger, panel, bo, sourceProperty, targetProperty);
                    break;
                case "filterReverse":
                    AddTriggerForFilterReverse(sourceControl, targetControl, trigger, bo, sourceProperty, targetProperty);
                    break;
                case "setEditable":
                case "setEditableOnce":
                    AddTriggerForEnable(sourceControl, targetControl, trigger);
                    break;
            }
        }

        /// <summary>
        /// Adds a trigger that assigns a literal value to the
        /// target property
        /// </summary>
        private static void AddTriggerForAssignLiteral(Control sourceControl, Control targetControl, Trigger trigger)
        {
            Control relevantControl = sourceControl;
            if (targetControl != null) relevantControl = targetControl;
            sourceControl.TextChanged +=
                delegate
                    {
                        if (trigger.ConditionValue == null ||
                            sourceControl.Text == trigger.ConditionValue)
                        {
                            relevantControl.Text = trigger.Value;
                        }
                    };
        }

        /// <summary>
        /// Adds a trigger that assigns a property value to the
        /// target property
        /// </summary>
        private static void AddTriggerForAssignProperty(Control sourceControl,
            Control targetControl, Trigger trigger, IBusinessObject bo)
        {
            Control relevantControl = sourceControl;
            if (targetControl != null) relevantControl = targetControl;
            sourceControl.TextChanged +=
                delegate
                {
                    if (trigger.ConditionValue == null ||
                        sourceControl.Text == trigger.ConditionValue)
                    {
                        relevantControl.Text = ReflectionUtilities.GetPropertyValue(bo, trigger.Value).ToString();
                    }
                };
        }

        /// <summary>
        /// Adds a new trigger that executes a named method on the BO of the
        /// form when the control's value changes
        /// </summary>
        private static void AddTriggerForExecute(Control control, Trigger trigger, IBusinessObject bo)
        {
            control.TextChanged +=
                delegate
                {
                    if (trigger.ConditionValue == null ||
                        control.Text == trigger.ConditionValue)
                    {
                        ReflectionUtilities.ExecuteMethod(bo, trigger.Value);
                    }
                };
        }

        /// <summary>
        /// Adds a trigger that filters a lookup-list.  Assumes that the "child"
        /// has a field that has the same name as the parent, and lists only those
        /// items that have a matching value on that field, while also respecting
        /// other existing lookup-list criteria.
        /// </summary>
        private static void AddTriggerForFilter(Control sourceControl, Control targetControl,
            Trigger trigger, PanelFactoryInfo panel, BusinessObject bo,
            string sourceProperty, string targetProperty)
        {
            if (!(targetControl is ComboBox))
            {
                throw new ArgumentException(String.Format(
                    "In a 'trigger' with a 'filter' action, the field for property " +
                    "'{0}' must be a type of ComboBox.", targetProperty));
            }
            ComboBox targetComboBox = (ComboBox)targetControl;

            bo.Props[sourceProperty].Updated +=
                delegate
                    {
                        if (trigger.ConditionValue == null ||
                            sourceControl.Text == trigger.ConditionValue)
                        {
                            targetComboBox.Items.Clear();
                            targetComboBox.Text = "";

                            object sourceValue = bo.Props[sourceProperty].Value;
                            if (sourceValue != null && sourceValue != DBNull.Value)
                            {
                                ILookupList ilookuplist = bo.ClassDef.PropDefcol[targetProperty].LookupList;
                                if (!(ilookuplist is BusinessObjectLookupList))
                                {
                                    throw new ArgumentException(String.Format(
                                        "In a 'trigger', the lookup-list for the property " +
                                        "'{0}' must be a BusinessObjectLookupList.", targetProperty));
                                }
                                BusinessObjectLookupList lookupList = (BusinessObjectLookupList) ilookuplist;
                                string oldCriteria = lookupList.Criteria;
                                string newCriteria = oldCriteria;
                                if (newCriteria == null) newCriteria = "";
                                if (newCriteria.Length > 0) newCriteria += " AND ";
                                if (sourceValue is Guid)
                                    newCriteria += sourceProperty + " = '" + ((Guid) sourceValue).ToString("B") + "'";
                                else newCriteria += sourceProperty + " = '" + sourceValue + "'";
                                lookupList.Criteria = newCriteria;

                                Dictionary<string, object> list = lookupList.GetLookupList(true);
                                LookupComboBoxMapper mapper = (LookupComboBoxMapper) panel.ControlMappers[targetProperty];
                                mapper.SetLookupList(list);
                                
                                lookupList.Criteria = oldCriteria;
                                if (targetComboBox.Items.Count == 1)
                                {
                                    targetComboBox.SelectedItem = targetComboBox.Items[0];
                                }
                                else if (targetComboBox.Items.Count == 2 && targetComboBox.Items[0].ToString() == "")
                                {
                                    targetComboBox.SelectedItem = targetComboBox.Items[1];
                                }
                            }
                        }
                    };
        }

        /// <summary>
        /// Adds a trigger that filters a lookup-list, but in the opposite
        /// direction to to "filter".  If an item in a child list has been
        /// chosen, the parent item can be set.  Assumes that the child has
        /// a field with the same name as the parent ID.
        /// </summary>
        private static void AddTriggerForFilterReverse(Control sourceControl, Control targetControl,
            Trigger trigger, BusinessObject bo, string sourceProperty, string targetProperty)
        {
            if (!(targetControl is ComboBox))
            {
                throw new ArgumentException(String.Format(
                    "In a 'trigger' with a 'filterReverse' action, the field for property " +
                    "'{0}' must be a type of ComboBox.", targetProperty));
            }
            ComboBox targetComboBox = (ComboBox)targetControl;

            bo.Props[sourceProperty].Updated +=
                delegate
                {
                    if (trigger.ConditionValue == null ||
                        sourceControl.Text == trigger.ConditionValue)
                    {
                        ILookupList iSourceLookup = bo.ClassDef.PropDefcol[sourceProperty].LookupList;
                        ILookupList iTargetLookup = bo.ClassDef.PropDefcol[targetProperty].LookupList;
                        if (!(iSourceLookup is BusinessObjectLookupList) ||
                            !(iTargetLookup is BusinessObjectLookupList))
                        {
                            throw new ArgumentException(String.Format(
                                "In a 'trigger', the lookup-list for the source property '{0}' and the target property " +
                                "'{1}' must be a BusinessObjectLookupList.", sourceProperty, targetProperty));
                        }
                        BusinessObjectLookupList sourceLookup = (BusinessObjectLookupList)iSourceLookup;
                        BusinessObjectLookupList targetLookup = (BusinessObjectLookupList)iTargetLookup;

                        object sourceValue = bo.Props[sourceProperty].Value;
                        if (sourceValue != null && sourceValue != DBNull.Value)
                        {
                            string sourceCriteria;
                            if (sourceValue is Guid)
                                sourceCriteria = sourceProperty + " = '" + ((Guid)sourceValue).ToString("B") + "'";
                            else sourceCriteria = sourceProperty + " = '" + sourceValue + "'";

                            Type sourceType = TypeLoader.LoadType(sourceLookup.AssemblyName, sourceLookup.ClassName);
                            IBusinessObjectCollection sourceCol =
                                BOLoader.Instance.GetBusinessObjectCol(sourceType, sourceCriteria, "");

                            if (sourceCol.Count > 0)
                            {
                                object targetValue = sourceCol[0].Props[targetProperty].Value;
                                if (targetValue != null && targetValue != DBNull.Value)
                                {
                                    string targetCriteria;
                                    if (targetValue is Guid)
                                        targetCriteria = targetProperty + " = '" + ((Guid) targetValue).ToString("B") + "'";
                                    else targetCriteria = targetProperty + " = '" + targetValue + "'";

                                    Type targetType = TypeLoader.LoadType(targetLookup.AssemblyName, targetLookup.ClassName);
                                    IBusinessObjectCollection targetCol =
                                        BOLoader.Instance.GetBusinessObjectCol(targetType, targetCriteria, "");

                                    if (targetCol.Count > 0)
                                    {
                                        targetComboBox.SelectedItem = targetCol[0].ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            targetComboBox.SelectedIndex = 0;
                        }
                    }
                };
        }

        /// <summary>
        /// Adds a trigger that enables or disables a control.  Disabling
        /// is only added for the action of "setEditable" (ie. "setEditableOnce" only
        /// carries out the action once).
        /// </summary>
        private static void AddTriggerForEnable(Control sourceControl, Control targetControl, Trigger trigger)
        {
            bool enabled;
            if (!Boolean.TryParse(trigger.Value, out enabled))
            {
                throw new ArgumentException(String.Format(
                                                "In a 'trigger', the 'value' given as '{0}' was invalid. " +
                                                "Only 'true' and 'false' are valid.", trigger.Value));
            }
            Control relevantControl = sourceControl;
            if (targetControl != null) relevantControl = targetControl;
            sourceControl.TextChanged +=
                delegate
                    {
                        if (trigger.ConditionValue == null ||
                            sourceControl.Text == trigger.ConditionValue)
                        {
                            relevantControl.Enabled = enabled;
                        }
                    };
            if (trigger.Action == "setEditable")
            {
                sourceControl.TextChanged +=
                    delegate
                        {
                            if (trigger.ConditionValue != null &&
                                sourceControl.Text != trigger.ConditionValue)
                            {
                                relevantControl.Enabled = !enabled;
                            }
                        };
            }
        }
    }
}