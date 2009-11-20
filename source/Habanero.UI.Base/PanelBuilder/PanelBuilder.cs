// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A Creator (<see cref="Delegate"/> that is used to create the Grouping control being used by the 
    /// <see cref="PanelBuilder.BuildPanelForForm(IUIForm,GroupControlCreator)"/> to create the Group control
    /// that is being used to Group the Controls within the an <see cref="ITabControl"/>, an <see cref="ICollapsiblePanelGroupControl"/>
    /// or a <see cref="IGroupBox"/>
    /// </summary>
    /// <returns></returns>
    public delegate IGroupControl GroupControlCreator();

//    /// <summary>
//    /// A Creator (<see cref="Delegate"/> that is used to create the individual control being used by the 
//    /// <see cref="PanelBuilder.BuildPanelForForm(UIForm,GroupControlCreator)"/> to create the control that 
//    /// is fitted within the Control Created by the <see cref="GroupControlCreator"/>.
//    /// <example>
//    ///<li> If the Group control used is a <see cref="ITabControl"/> then this will be an <see cref="ITabPage"/><br/></li>
//    ///<li> If the Group Control is a <see cref="ICollapsiblePanelGroupControl"/> the this will be a <see cref="ICollapsiblePanel"/><br/></li>
//    ///<li> If the Group Control is a <see cref="IGroupBox"/> then this delegate can return any control that can be placed in an <see cref="IGroupBox"/>
//    ///      typically an <see cref="IPanel"/></li>
//    /// </example>
//    /// </summary>
//    /// <returns></returns>
//    public delegate IControlHabanero IndividualControlCreator(string name);

    ///<summary>
    /// Builds an <see cref="IPanelInfo"/> that has the information required to link the <see cref="IPanel"/>
    ///  built by the Panel Builder to the <see cref="IBusinessObject"/> with a certain layout (as defined by the
    ///  <see cref="LayoutManager"/> (Currently Only the <see cref="GridLayoutManager"/> can be used).<br/>
    /// The <see cref="IPanel"/> Built by the PanelBuilder has all the controls required for a <see cref="IBusinessObject"/> 
    ///     to be viewed and edited in. The Controls to be used are defind in the <see cref="UIFormTab"/> or the
    ///     <see cref="UIForm"/> depending on whether the <see cref="BuildPanelForForm(IUIForm)"/> or <see cref="BuildPanelForTab"/>
    ///     method is used.
    /// <br/><br/>
    /// The PanelBuilder can create a panel for any environment based on
    /// the implementation of <see cref="IControlFactory"/> that is passed through in
    /// the constructor.
    /// <br/><br/>
    /// Once the panel has been constructed, you will need to assign the
    /// instance of the business object.  See <see cref="IPanelInfo"/> for
    /// these options.  Once editing is completed, you will need to persist
    /// the changes. First call ApplyChangesToBusinessObject on the IPanelInfo
    /// object, and then carry out persistence on the BusinessObject.
    /// </summary>
    public class PanelBuilder
    {
        /// <summary>
        /// The width that the error provider will Take up on the screen.
        /// </summary>
        public const int ERROR_PROVIDER_WIDTH = 20;

        /// <summary>
        /// The number of controls per <see cref="IClassDef"/>.<see cref="UIDef"/> defined column on the Panel Builder. (Typically the label, the Actual control (e.g. TextBox)
        ///  and the 
        /// </summary>
        public const int CONTROLS_PER_COLUMN = 3;

        /// <summary>
        /// The column Number that the Label Control will be placed in.
        /// </summary>
        public const int LABEL_CONTROL_COLUMN_NO = 0;

        /// <summary>
        /// The column Number that the Actual user control (e.g. TextBox) will be placed in.
        /// </summary>
        public const int INPUT_CONTROL_COLUMN_NO = 1;

        /// <summary>
        /// The column that the error provider will be placed in. 
        ///   This is the last column position within the <see cref="IClassDef"/>.<see cref="UIDef"/> defined column.
        /// </summary>
        public const int ERROR_PROVIDER_COLUMN_NO = CONTROLS_PER_COLUMN - 1;


        private GroupControlCreator _groupControlCreator;

//        ///<summary>
//        /// Sets the <see cref="groupControlCreator"/> and the <see cref="individualControlCreator"/>
//        /// to use a new set of control Creators.
//        ///</summary>
//        ///<param name="groupControlCreator"></param>
//        ///<param name="individualControlCreator"></param>
//        public void SetControlCreators(GroupControlCreator groupControlCreator, IndividualControlCreator individualControlCreator)
//        {
//            this.GroupControlCreator = groupControlCreator;
//            this.IndividualControlCreator = individualControlCreator;
//        }
        ///<summary>
        /// Get or set the <see cref="GroupControlCreator"/> for the <see cref="PanelBuilder"/>.
        ///</summary>
        public GroupControlCreator GroupControlCreator
        {
            get { return _groupControlCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _groupControlCreator = value;
            }
        }

        ///<summary>
        /// Gets and Sets the control Factory that is used to create the individual controls on the <see cref="IPanel"/>
        ///</summary>
        public IControlFactory ControlFactory { get; set; }

        ///<summary>
        /// Creates the panel Builder with the <see cref="IControlFactory"/>
        ///   to be used by the panel builder for buiding the controls.
        ///</summary>
        ///<param name="controlFactory"></param>
        public PanelBuilder(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            ControlFactory = controlFactory;
            this.GroupControlCreator = controlFactory.CreateTabControl;
        }

        /// <summary>
        /// Constructs a panel for editing the data of a single instance
        /// of a BusinessObject, using the control layout as specified in
        /// a <see cref="UIFormTab"/> object. 
        /// </summary>
        /// <param name="formTab">The tab object that indicates which controls
        /// to create and how the controls are laid out</param>
        /// <returns>Returns an IPanelInfo object that contains access to the
        /// BusinessObject instance, the created panel, and all the controls,
        /// mappers, labels and error providers that were created.
        /// </returns>
        public IPanelInfo BuildPanelForTab(UIFormTab formTab)
        {
            if (formTab == null) throw new ArgumentNullException("formTab");
            IPanel panel = ControlFactory.CreatePanel();
            PanelInfo panelInfo = new PanelInfo();
            GridLayoutManager layoutManager =  SetupLayoutManager(formTab, panel);
            panelInfo.LayoutManager = layoutManager;
            AddFieldsToLayoutManager(formTab, panelInfo);
            SetupInputControlColumnWidth(panelInfo, formTab);

            panel.Width = layoutManager.GetFixedWidthIncludingGaps();
            panel.Height = layoutManager.GetFixedHeightIncludingGaps();
            Size minSize = new Size(panel.Width, panel.Height);
            panel.MinimumSize = minSize;
            panelInfo.Panel = panel;

            panelInfo.UIFormTab = formTab;
            panelInfo.MinimumPanelHeight = panel.Height;
            panelInfo.UIForm = formTab.UIForm;
            return panelInfo;
        }

        /// <summary>
        /// Constructs a panel for editing the data of a single instance
        /// of a BusinessObject, using the control layout as specified in
        /// a <see cref="UIForm"/> object. 
        /// </summary>
        /// <param name="uiForm">The UIForm object that indicates which controls
        /// to create and how the controls are laid out</param>
        /// <returns>Returns an IPanelInfo object that contains access to the
        /// BusinessObject instance, the created panel, and all the controls,
        /// mappers, labels and error providers that were created.
        /// </returns>
        public IPanelInfo BuildPanelForForm(IUIForm uiForm)
        {
            return BuildPanelForForm(uiForm, this.GroupControlCreator);
        }

        ///<summary>
        /// Builds a Panel for a single Tab as defined in the <see cref="UIForm"/>.
        ///   There will be one <see cref="IPanelInfo"/> for the defined uiForm.<br/>
        /// If there are multiple tabs defined for the <paramref name="uiForm"/> then
        ///  an actual <see cref="ITabPage"/> is created for each <see cref="UIFormTab"/> 
        ///  and the <see cref="IPanel"/> created by <see cref="BuildPanelForTab"/> will 
        ///  be placed on this TabPage.<br></br>
        /// Else the <see cref="IPanel"/> is placed on the form directly.
        ///</summary>
        /// <param name="uiForm">The UIForm object that indicates which controls
        /// to create and how the controls are laid out</param>
        ///<param name="groupControlCreator">The <see cref="GroupControlCreator"/></param>
        /// <returns>Returns an IPanelInfo object that contains access to the
        /// BusinessObject instance, the created panel, and all the controls,
        /// mappers, labels and error providers that were created.
        /// </returns>
        public IPanelInfo BuildPanelForForm(IUIForm uiForm, GroupControlCreator groupControlCreator)
        {
            if (uiForm == null) throw new ArgumentNullException("uiForm");
            if (groupControlCreator == null) throw new ArgumentNullException("groupControlCreator");
            PanelInfo panelInfo = new PanelInfo();
            // generic interface
            foreach (UIFormTab formTab in uiForm)
            {
                IPanelInfo tabPagePanelInfo = BuildPanelForTab(formTab);
                panelInfo.PanelInfos.Add(tabPagePanelInfo);
                foreach (PanelInfo.FieldInfo fieldInfo in tabPagePanelInfo.FieldInfos)
                {
                    panelInfo.FieldInfos.Add(fieldInfo);
                }
                if (tabPagePanelInfo.MinimumPanelHeight > panelInfo.MinimumPanelHeight)
                {
                    panelInfo.MinimumPanelHeight = tabPagePanelInfo.MinimumPanelHeight;
                }
            }

            IPanel mainPanel;
            if (panelInfo.PanelInfos.Count == 0)
            {
                mainPanel = ControlFactory.CreatePanel();
            }
            else if (panelInfo.PanelInfos.Count == 1)
            {
                mainPanel = panelInfo.PanelInfos[0].Panel;
                int lesserHeight = panelInfo.MinimumPanelHeight;
                if (uiForm.Height < lesserHeight) lesserHeight = uiForm.Height;
                mainPanel.MinimumSize = new Size(uiForm.Width, lesserHeight);
            }
            else
            {
                IGroupControl groupControl = groupControlCreator();
                foreach (IPanelInfo childPanelInfo in panelInfo.PanelInfos)
                {
                    IUIFormTab uiFormTab = childPanelInfo.UIFormTab;
                    IPanel childPanel = childPanelInfo.Panel;
                    groupControl.AddControl(childPanel, uiFormTab.Name, childPanel.Height, childPanel.Width);
                }

                IPanel panel = ControlFactory.CreatePanel();
                groupControl.Dock = DockStyle.Fill;
                panel.Controls.Add(groupControl);
                panel.MinimumSize = groupControl.Size;
                mainPanel = panel;
            }
            panelInfo.Panel = mainPanel;
            mainPanel.Size = new Size(uiForm.Width, uiForm.Height);
            panelInfo.UIForm = uiForm;
            return panelInfo;
        }

        private GridLayoutManager SetupLayoutManager(UIFormTab formTab, IPanel panel)
        {
            GridLayoutManager layoutManager = new GridLayoutManager(panel, ControlFactory);
            int maxRowsInColumns = formTab.GetMaxRowsInColumns();
            
            int colCount = formTab.Count * CONTROLS_PER_COLUMN;
            layoutManager.SetGridSize(maxRowsInColumns, colCount);
            layoutManager.FixColumnBasedOnContents(0);
            for (int i = 0; i < colCount; i += CONTROLS_PER_COLUMN)
            {
                layoutManager.FixColumnBasedOnContents(i + LABEL_CONTROL_COLUMN_NO);
                layoutManager.FixColumn(i + ERROR_PROVIDER_COLUMN_NO, ERROR_PROVIDER_WIDTH);
            }
            ITextBox sampleTextBoxForHeight = ControlFactory.CreateTextBox();
            for (int row = 0; row < maxRowsInColumns; row++)
            {
                layoutManager.FixRow(row, sampleTextBoxForHeight.Height);
            }
            return layoutManager;
        }

        private void AddFieldsToLayoutManager(UIFormTab formTab, IPanelInfo panelInfo)
        {
            int numberOfColumns = formTab.Count;
            int[] currentFieldPositionInColumns = new int[numberOfColumns];
            int[] rowSpanTrackerForColumn = new int[numberOfColumns];
            int maxRowsInColumns = formTab.GetMaxRowsInColumns();
            int[] columnSpanTrackerForRow = new int[maxRowsInColumns];
            for (int currentRowNo = 0; currentRowNo < maxRowsInColumns; currentRowNo++)
            {
                for (int currentColumnNo = 0; currentColumnNo < numberOfColumns; currentColumnNo++)
                {
                    IUIFormColumn currentFormColumn = formTab[currentColumnNo];

                    if (--rowSpanTrackerForColumn[currentColumnNo] > 0) continue;
                    // keep skipping this grid position until a previous row span in this column has been decremented 
                    if (--columnSpanTrackerForRow[currentRowNo] > 0) continue;
                    // keep skipping this grid position until a previous column span in this row has been decremented

                    int currentFieldNoInColumn = currentFieldPositionInColumns[currentColumnNo];
                    int totalFieldsInColumn = currentFormColumn.Count;
                    if (currentFieldNoInColumn < totalFieldsInColumn) // there exists a field in this row in this column
                    {
                        UIFormField formField = (UIFormField) currentFormColumn[currentFieldNoInColumn];
                        rowSpanTrackerForColumn[currentColumnNo] = formField.RowSpan;
                        for (int i = currentRowNo; i < currentRowNo + formField.RowSpan; i++)
                            // update colspan of all rows that this field spans into.
                            columnSpanTrackerForRow[i] = formField.ColSpan;

                        PanelInfo.FieldInfo fieldInfo = AddControlsForField(formField, panelInfo);
                        fieldInfo.InputControl.TabIndex = currentColumnNo*maxRowsInColumns + currentRowNo;
                    }
                    else
                    {
                        AddNullControlsForEmptyField(panelInfo);
                    }
                    currentFieldPositionInColumns[currentColumnNo]++;
                }
            }
        }

        private PanelInfo.FieldInfo AddControlsForField(UIFormField formField, IPanelInfo panelInfo)
        {
            IControlHabanero labelControl;
            IControlMapper controlMapper;
            if (formField.Layout == LayoutStyle.Label)
            {
                labelControl = CreateAndAddLabel(panelInfo, formField);
                controlMapper = CreateAndAddInputControl(panelInfo, formField);
            }
            else
            {
                labelControl = CreateAndAddGroupBox(panelInfo, formField);
                controlMapper = CreateAndAddInputControlToContainerControl(formField, labelControl);
            }

            CreateAndAddErrorProviderPanel(panelInfo, formField);

            PanelInfo.FieldInfo fieldInfo = new PanelInfo.FieldInfo(formField.PropertyName, labelControl, controlMapper);
            panelInfo.FieldInfos.Add(fieldInfo);
            return fieldInfo;
        }


        private IControlHabanero CreateAndAddGroupBox(IPanelInfo panelInfo, UIFormField formField)
        {
            IControlHabanero labelControl = ControlFactory.CreateGroupBox(formField.GetLabel());
            labelControl.Width = 0; // don't affect the label column's fixed width
            labelControl.Name = formField.PropertyName;
            SetToolTip(formField, labelControl);
            panelInfo.LayoutManager.AddControl(labelControl, formField.RowSpan, 2);
            return labelControl;
        }

        private static void AddNullControlsForEmptyField(IPanelInfo panelInfo)
        {
            for (int i = 0; i < CONTROLS_PER_COLUMN; i++)
                panelInfo.LayoutManager.AddControl(null);
        }

        private void CreateAndAddErrorProviderPanel(IPanelInfo panelInfo, UIFormField formField)
        {
            IPanel panel = ControlFactory.CreatePanel();
            panelInfo.LayoutManager.AddControl(panel, formField.RowSpan, 1);
        }

        private IControlMapper CreateAndAddInputControlToContainerControl
            (UIFormField formField, IControlHabanero containerControl)
        {
            IControlMapper controlMapper;
            IControlHabanero inputControl = ConfigureInputControl(formField, out controlMapper);
            inputControl.Dock = DockStyle.Fill;
            containerControl.Controls.Add(inputControl);
            return controlMapper;
        }

        private IControlMapper CreateAndAddInputControl(IPanelInfo panelInfo, UIFormField formField)
        {
            IControlMapper controlMapper;
            IControlHabanero inputControl = ConfigureInputControl(formField, out controlMapper);

            int numberOfGridColumnsToSpan = 1 + (CONTROLS_PER_COLUMN * (formField.ColSpan - 1));
            GridLayoutManager.ControlInfo inputControlInfo = new GridLayoutManager.ControlInfo
                (inputControl, numberOfGridColumnsToSpan, formField.RowSpan);

            panelInfo.LayoutManager.AddControl(inputControlInfo);
            return controlMapper;
        }

        private IControlHabanero ConfigureInputControl(UIFormField formField, out IControlMapper controlMapper)
        {
            IControlHabanero inputControl = ControlFactory.CreateControl
                (formField.ControlTypeName, formField.ControlAssemblyName);
            controlMapper = ControlMapper.Create
                (formField.MapperTypeName, formField.MapperAssembly, inputControl, formField.PropertyName,
                 !formField.Editable, ControlFactory);
            controlMapper.ClassDef = GetClassDef(formField);
            if (!String.IsNullOrEmpty(formField.Alignment)) SetInputControlAlignment(formField, inputControl);
            SetInputControlNumLines(formField, inputControl);

            controlMapper.SetPropertyAttributes(formField.Parameters);

            AddDecimalPlacesToNumericUpDown(formField, inputControl);

            AddComboBoxItems(formField, inputControl);

            AddEmailFunctionalityToTextBox(formField, inputControl);

            AddMultiLineTextbox(formField, inputControl);

            SetToolTip(formField, inputControl);
            return inputControl;
        }

        private static IClassDef GetClassDef(IUIFormField formField)
        {
            IUIFormColumn column = formField.UIFormColumn;
            if (column == null) return null;
            IUIFormTab tab = column.UIFormTab;
            if (tab == null) return null;

            IUIForm form = tab.UIForm;
            if (form == null) return null;

            IUIDef def = form.UIDef;
            if (def == null) return null;
            return def.ClassDef;
        }

        private static void AddMultiLineTextbox(UIFormField formField, IControlHabanero inputControl)
        {
            if (formField.RowSpan <= 1) return;
            if (inputControl is ITextBox) ((ITextBox) inputControl).Multiline = true;
        }

        private static void AddDecimalPlacesToNumericUpDown(UIFormField formField, IControlHabanero inputControl)
        {
            if (String.IsNullOrEmpty(formField.DecimalPlaces)) return;
            if (inputControl is INumericUpDown && !String.IsNullOrEmpty(formField.MapperTypeName) 
                && formField.MapperTypeName.ToLower() == "numericupdowncurrencymapper")
            {
                int decimalPlaces = Convert.ToInt32(formField.DecimalPlaces);
                if (decimalPlaces >= 0)
                {
                    ((INumericUpDown) inputControl).DecimalPlaces = decimalPlaces;
                }
            }
        }

        private static void AddComboBoxItems(UIFormField formField, IControlHabanero inputControl)
        {
            if (String.IsNullOrEmpty(formField.Options)) return;
            if (inputControl is IComboBox && formField.MapperTypeName.ToLower() == "listcomboboxmapper")
            {
                string[] items = formField.Options.Split('|');
                IComboBox comboBox = ((IComboBox) inputControl);
                comboBox.Items.Add(""); // This creates the blank item for the ComboBox 
                foreach (string item in items)
                {
                    comboBox.Items.Add(item);
                }
            }
        }

        private static void AddEmailFunctionalityToTextBox(UIFormField formField, IControlHabanero inputControl)
        {
            if (String.IsNullOrEmpty(formField.IsEmail)) return;
            if (inputControl is ITextBox && Convert.ToBoolean(formField.IsEmail))
            {
                ITextBox textBox = (ITextBox) inputControl;
                textBox.DoubleClick += EmailTextBoxDoubleClickedHandler;
            }
        }

        ///<summary>
        /// A handler to deal with a double-click on an email textbox, which
        /// causes the default mail client on the user system to be opened
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private static void EmailTextBoxDoubleClickedHandler(object sender, EventArgs e)
        {
            ITextBox tb = (ITextBox) sender;
            if (!IsEmailAddress(tb.Text)) return;
            string comm = "mailto:" + tb.Text;
            Process.Start(comm);
        }

        private static bool IsEmailAddress(string text)
        {
            return text.IndexOf("@") != -1;
        }

        private static void SetInputControlNumLines(UIFormField formField, IControlHabanero inputControl)
        {
            if (!(inputControl is ITextBox)) return;
            if (formField.RowSpan <= 1) return;

            ITextBox textBox = ((ITextBox) inputControl);
            textBox.Multiline = true;
            textBox.AcceptsReturn = true;
            textBox.ScrollBars = ScrollBars.Vertical;
        }

        private static void SetInputControlAlignment(UIFormField formField, IControlHabanero inputControl)
        {
            // Some controls have TextAlign and others don't. This code uses reflection to apply it if appropriate.
            // This did not work because the propertyInfo.SetValue method was not calling the TestBoxVWG TextAlign Set property method.
            // PropertyInfo propertyInfo = inputControl.GetType().GetProperty("TextAlign");
            //if (propertyInfo != null &&
            //    propertyInfo.PropertyType.Name == "HorizontalAlignment") //caters for the possibility of a custom control that implements textalign but doesn't have HorizontalAlignment as its type
            //{

            //    propertyInfo.SetValue(inputControl, GetAlignmentValue(formField.Alignment), new object[0]);
            //}
            if (inputControl is ITextBox)
            {
                ((ITextBox) inputControl).TextAlign = GetAlignmentValue(formField.Alignment);
            }
            if (inputControl is INumericUpDown)
            {
                ((INumericUpDown) inputControl).TextAlign = GetAlignmentValue(formField.Alignment);
            }
        }

        private ILabel CreateAndAddLabel(IPanelInfo panelInfo, UIFormField formField)
        {
//            IClassDef classDef = panelInfo.UIForm.UIDef.ClassDef;
            ILabel labelControl = ControlFactory.CreateLabel(formField.GetLabel(), formField.IsCompulsory);
            labelControl.Name = formField.PropertyName;
            labelControl.Enabled = formField.Editable;
            SetToolTip(formField, labelControl);
            panelInfo.LayoutManager.AddControl(labelControl, formField.RowSpan, 1);
            return labelControl;
        }

        private static void SetupInputControlColumnWidth(IPanelInfo panelInfo, IUIFormTab formTab)
        {
            GridLayoutManager layoutManager = panelInfo.LayoutManager;
            int formColCount = 0;
            foreach (UIFormColumn formColumn in formTab)
            {
                if (formColumn.Width < 0) continue;
                int gridCol = formColCount * CONTROLS_PER_COLUMN;
                int labelColumnWidth = layoutManager.GetFixedColumnWidth(gridCol + LABEL_CONTROL_COLUMN_NO);
                int errorProviderColumnWidth = layoutManager.GetFixedColumnWidth(gridCol + ERROR_PROVIDER_COLUMN_NO);
                int totalGap = (CONTROLS_PER_COLUMN - 1) * layoutManager.GapSize;
                if (formTab.Count == 1)
                    totalGap += 2 * layoutManager.BorderSize; // add extra border for single column
                else if (formColCount == formTab.Count - 1)
                    totalGap += layoutManager.BorderSize + layoutManager.GapSize; // last column in multi-column
                else if (formColCount > 0 && formTab.Count > 0)
                    totalGap += layoutManager.GapSize; //2 More gaps for internal column in multi-column
                else if (formColCount == 0 && formTab.Count > 0) totalGap += layoutManager.BorderSize;

                layoutManager.FixColumn
                    (gridCol + INPUT_CONTROL_COLUMN_NO,
                     formColumn.Width - labelColumnWidth - errorProviderColumnWidth - totalGap);
                formColCount++;
            }
        }

        private void SetToolTip(UIFormField formField, IControlHabanero control)
        {
            string toolTipText = formField.GetToolTipText();
            IToolTip toolTip = ControlFactory.CreateToolTip();
            if (!String.IsNullOrEmpty(toolTipText))
            {
                toolTip.SetToolTip(control, toolTipText);
            }
        }

        ///<summary>
        /// Checks if the alignment value is valid
        ///</summary>
        ///<param name="alignmentValue">The alignment value from the FieldInfo</param>
        ///<returns>The valid Habanero.UI.Base Horizontal Alignment</returns>
        ///<exception cref="HabaneroDeveloperException">Throws a HabaneroDeveloperException if the alignment value is invalid</exception>
        public static HorizontalAlignment GetAlignmentValue(string alignmentValue)
        {
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
            if (
                !(alignmentValue.ToLower() == "left" || alignmentValue.ToLower() == "right"
                  || alignmentValue.ToLower() == "center" || alignmentValue.ToLower() == "centre"))
            {
                string errMessage = "Invalid alignment property value '" + alignmentValue
                                    + "' in the class definitions.";
                throw new HabaneroDeveloperException(errMessage, errMessage);
            }


            if (alignmentValue.ToLower() == "left") horizontalAlignment = HorizontalAlignment.Left;
            if (alignmentValue.ToLower() == "right") horizontalAlignment = HorizontalAlignment.Right;
            if (alignmentValue.ToLower() == "center" || alignmentValue.ToLower() == "centre")
                horizontalAlignment = HorizontalAlignment.Center;

            return horizontalAlignment;
        }

        /// <summary>
        /// Creates one panel for each UI Form definition of a business object
        /// </summary>
        /// <returns>Returns the list of panel info objects created</returns>
        /// TODO: improve tab order (ie make all tabs use one sequence rather than each starting a new sequence)
        public List<IPanelInfo> CreateOnePanelPerUIFormTab(IUIForm uiForm)
        {
            List<IPanelInfo> panelInfoList = new List<IPanelInfo>();
            foreach (UIFormTab formTab in uiForm)
            {
                IPanelInfo onePanelInfo = BuildPanelForTab(formTab);

                panelInfoList.Add(onePanelInfo);
            }
            return panelInfoList;
        }
    }
}