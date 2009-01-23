using System;
using System.Diagnostics;
using System.Reflection;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{

    public class PanelBuilder
    {
        private IControlFactory _factory;
        public const int ERROR_PROVIDER_WIDTH = 20;
        public const int CONTROLS_PER_COLUMN = 3;
        public const int LABEL_CONTROL_COLUMN_NO = 0;
        public const int INPUT_CONTROL_COLUMN_NO = 1;
        public const int ERROR_PROVIDER_COLUMN_NO = CONTROLS_PER_COLUMN - 1;


        public PanelBuilder(IControlFactory factory)
        {
            _factory = factory;
        }

        public IControlFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public IPanelInfo BuildPanelForTab(UIFormTab formTab)
        {
            IPanel panel = Factory.CreatePanel();
            IPanelInfo panelInfo = new PanelInfo();
            GridLayoutManager layoutManager = panelInfo.LayoutManager = SetupLayoutManager(formTab, panel);
            AddFieldsToLayoutManager(formTab, panelInfo);
            SetupInputControlColumnWidth(panelInfo, formTab);

            panel.Width = layoutManager.GetFixedWidthIncludingGaps();
            panel.Height = layoutManager.GetFixedHeightIncludingGaps();

            panelInfo.Panel = panel;
            return panelInfo;
        }


        public IPanelInfo BuildPanelForForm(UIForm uiForm)
        {
            IPanelInfo panelInfo = new PanelInfo();
            IPanel panel = Factory.CreatePanel();
            panelInfo.Panel = panel;
            ITabControl tabControl = Factory.CreateTabControl();
            IPanel tabPagePanel = Factory.CreatePanel();
            foreach (UIFormTab formTab in uiForm)
            {
                ITabPage tabPage = Factory.CreateTabPage(formTab.Name);
                IPanelInfo tabPagePanelInfo = BuildPanelForTab(formTab);
                tabPagePanel = tabPagePanelInfo.Panel;
                tabPage.Width = tabPagePanel.Width;
                tabPage.Height = tabPagePanel.Height;
                tabPagePanel.Dock = DockStyle.Fill;
                tabPage.Controls.Add(tabPagePanel);
                tabControl.TabPages.Add(tabPage);
                panelInfo.PanelInfos.Add(tabPagePanelInfo);
                foreach (PanelInfo.FieldInfo fieldInfo in tabPagePanelInfo.FieldInfos)
                {
                    panelInfo.FieldInfos.Add(fieldInfo);
                }
            }
            tabControl.Dock = DockStyle.Fill;
            if (uiForm.Count == 1)
            {
                panelInfo.Panel = tabPagePanel;
            }
            else
            {
                panel.Controls.Add(tabControl);
            }
            return panelInfo;
        }

        private GridLayoutManager SetupLayoutManager(UIFormTab formTab, IPanel panel)
        {
            GridLayoutManager layoutManager = new GridLayoutManager(panel, Factory);
            int maxRowsInColumns = formTab.GetMaxRowsInColumns();
            int colCount = formTab.Count * CONTROLS_PER_COLUMN;
            layoutManager.SetGridSize(maxRowsInColumns, colCount);
            layoutManager.FixColumnBasedOnContents(0);
            for (int i = 0; i < colCount; i += CONTROLS_PER_COLUMN)
            {
                layoutManager.FixColumnBasedOnContents(i + LABEL_CONTROL_COLUMN_NO);
                layoutManager.FixColumn(i + ERROR_PROVIDER_COLUMN_NO, ERROR_PROVIDER_WIDTH);
            }
            ITextBox sampleTextBoxForHeight = Factory.CreateTextBox();
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
            for (int currentRowNo = 0; currentRowNo < formTab.GetMaxRowsInColumns(); currentRowNo++)
            {
                int columnSpanTracker = 0;
                for (int currentColumnNo = 0; currentColumnNo < numberOfColumns; currentColumnNo++)
                {
                    UIFormColumn currentFormColumn = formTab[currentColumnNo];

                    if (--rowSpanTrackerForColumn[currentColumnNo] > 0) continue;  // keep skipping this grid position until a previous row span in this column has been decremented 
                    if (--columnSpanTracker > 0) continue;  // keep skipping this grid position until a previous column span in this row has been decremented

                    int currentFieldNoInColumn = currentFieldPositionInColumns[currentColumnNo];
                    int totalFieldsInColumn = currentFormColumn.Count;
                    if (currentFieldNoInColumn < totalFieldsInColumn) // there exists a field in this row in this column
                    {
                        UIFormField formField = currentFormColumn[currentFieldNoInColumn];
                        rowSpanTrackerForColumn[currentColumnNo] = formField.RowSpan;
                        columnSpanTracker = formField.ColSpan;

                        AddControlsForField(formField, panelInfo);
                    }
                    else
                    {
                        AddNullControlsForEmptyField(panelInfo);
                    }
                    currentFieldPositionInColumns[currentColumnNo]++;
                }
            }
        }

        private void AddControlsForField(UIFormField formField, IPanelInfo panelInfo)
        {
            ILabel label = CreateAndAddLabel(panelInfo, formField);
            IControlMapper controlMapper = CreateAndAddInputControl(panelInfo, formField);
            CreateAndAddErrorProviderPanel(panelInfo, formField);

            panelInfo.FieldInfos.Add(new PanelInfo.FieldInfo(formField.PropertyName, label, controlMapper));
        }

        private void AddNullControlsForEmptyField(IPanelInfo panelInfo)
        {
            for (int i = 0; i < CONTROLS_PER_COLUMN; i++)
                panelInfo.LayoutManager.AddControl(null);
        }

        private void CreateAndAddErrorProviderPanel(IPanelInfo panelInfo, UIFormField formField)
        {
            IPanel panel = Factory.CreatePanel();
            panelInfo.LayoutManager.AddControl(panel, formField.RowSpan, 1);
        }

        private IControlMapper CreateAndAddInputControl(IPanelInfo panelInfo, UIFormField formField)
        {
            IControlHabanero inputControl = Factory.CreateControl(formField.ControlTypeName,
                                                                  formField.ControlAssemblyName);
            IControlMapper controlMapper = ControlMapper.Create(formField.MapperTypeName,
                                                                formField.MapperAssembly, inputControl,
                                                                formField.PropertyName, !formField.Editable, _factory);


            if (!String.IsNullOrEmpty(formField.Alignment)) SetInputControlAlignment(formField, inputControl);
            if (!String.IsNullOrEmpty(formField.NumLines))  SetInputControlNumLines(formField, inputControl);
            
            if (!String.IsNullOrEmpty(formField.DecimalPlaces))
            {
                if (inputControl is INumericUpDown && formField.MapperTypeName.ToLower() == "numericupdowncurrencymapper")
                {
                    int decimalPlaces = Convert.ToInt32(formField.DecimalPlaces);
                    if (decimalPlaces>=0)
                    {
                        ((INumericUpDown)inputControl).DecimalPlaces = decimalPlaces;
                    }
                }
            }

            if (!String.IsNullOrEmpty(formField.Options))
            {
                if (inputControl is IComboBox && formField.MapperTypeName.ToLower() == "listcomboboxmapper")
                {
                    string[] items = formField.Options.Split('|');
                    IComboBox comboBox = ((IComboBox)inputControl);
                    comboBox.Items.Add(""); // This creates the blank item for the ComboBox 
                    foreach (string item in items)
                    {
                        
                        comboBox.Items.Add(item);
                    }
                }
            }

            if (!String.IsNullOrEmpty(formField.IsEmail))
            {
                if (inputControl is ITextBox && Convert.ToBoolean(formField.IsEmail))
                {
                    ITextBox textBox = (ITextBox)inputControl;
                    textBox.DoubleClick += EmailTextBoxDoubleClickedHandler;
                }
            }

            if (formField.MapperTypeName=="DateTimePickerMapper")
            {
                DateTimePickerMapper dateTimePickerMapper = new DateTimePickerMapper((IDateTimePicker) inputControl, formField.PropertyName,formField.Editable,_factory);
                dateTimePickerMapper.SetPropertyAttributes(formField.Parameters);
            }

            if (formField.RowSpan > 1)
            {
                if (inputControl is ITextBox) ((ITextBox)inputControl).Multiline = true;
            }
            int numberOfGridColumnsToSpan = 1 + (CONTROLS_PER_COLUMN * (formField.ColSpan - 1));
            GridLayoutManager.ControlInfo inputControlInfo =
                new GridLayoutManager.ControlInfo(inputControl, numberOfGridColumnsToSpan,
                                                  formField.RowSpan);
            SetToolTip(formField, inputControl);
            panelInfo.LayoutManager.AddControl(inputControlInfo);
            return controlMapper;
        }

        /// A handler to deal with a double-click on an email textbox, which
        /// causes the default mail client on the user system to be opened
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private static void EmailTextBoxDoubleClickedHandler(object sender, EventArgs e)
        {
            ITextBox tb = (ITextBox)sender;
            if (tb.Text.IndexOf("@") != -1)
            {
                string comm = "mailto:" + tb.Text;
                Process.Start(comm);
            }
        }

        private void SetInputControlNumLines(UIFormField formField, IControlHabanero inputControl)
        {
           if (inputControl is ITextBox)
           {
               int numLines;
               try
               {
                   numLines = Convert.ToInt32(formField.NumLines);
               }
               catch (Exception)
               {
                   throw new InvalidXmlDefinitionException
                       ("An error " + "occurred while reading the 'numLines' parameter "
                        + "from the class definitions.  The 'value' "
                        + "attribute must be a valid integer.");
               }

               ITextBox textBox = ((ITextBox)inputControl);
               textBox.Multiline = true;
               textBox.AcceptsReturn = true;
               textBox.ScrollBars = ScrollBars.Vertical;
               textBox.Height = inputControl.Height * numLines;
           }
            
        }

        private void SetInputControlAlignment(UIFormField formField, IControlHabanero inputControl)
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
                ((INumericUpDown)inputControl).TextAlign = GetAlignmentValue(formField.Alignment);
            }

            
        }

        private ILabel CreateAndAddLabel(IPanelInfo panelInfo, UIFormField formField)
        {
            ILabel labelControl = Factory.CreateLabel(formField.GetLabel(), formField.IsCompulsory);
            labelControl.Name = formField.PropertyName;
            SetToolTip(formField, labelControl);
            panelInfo.LayoutManager.AddControl(labelControl, formField.RowSpan, 1);
            return labelControl;
        }

        private void SetupInputControlColumnWidth(IPanelInfo panelInfo, UIFormTab formTab)
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

                layoutManager.FixColumn(gridCol + INPUT_CONTROL_COLUMN_NO,
                                        formColumn.Width - labelColumnWidth - errorProviderColumnWidth - totalGap);
                formColCount++;
            }
        }

        private void SetToolTip(UIFormField formField, IControlHabanero inputControl)
        {
            string toolTipText = formField.GetToolTipText();
            IToolTip toolTip = _factory.CreateToolTip();
            if (!String.IsNullOrEmpty(toolTipText))
            {
                toolTip.SetToolTip(inputControl, toolTipText);
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
            if (!(alignmentValue.ToLower() == "left" || alignmentValue.ToLower() == "right" || alignmentValue.ToLower() == "center" || alignmentValue.ToLower() == "centre"))
            {
                string errMessage = "Invalid alignment property value '" + alignmentValue + "' in the class definitions.";
                throw new HabaneroDeveloperException(errMessage, errMessage);
            }


            if (alignmentValue.ToLower() == "left") horizontalAlignment = HorizontalAlignment.Left;
            if (alignmentValue.ToLower()=="right") horizontalAlignment=HorizontalAlignment.Right;
            if (alignmentValue.ToLower() == "center" || alignmentValue.ToLower() == "centre") horizontalAlignment = HorizontalAlignment.Center;

            return horizontalAlignment;
        }

       
    }
}