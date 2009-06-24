//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General public virtual License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General public virtual License for more details.
// 
//     You should have received a copy of the GNU Lesser General public virtual License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.Util;
using AutoCompleteMode=System.Windows.Forms.AutoCompleteMode;
using AutoCompleteSource=System.Windows.Forms.AutoCompleteSource;
using DateTimePickerFormat=Habanero.UI.Base.DateTimePickerFormat;
using DialogResult=Habanero.UI.Base.DialogResult;
using MessageBoxButtons=Habanero.UI.Base.MessageBoxButtons;
using MessageBoxIcon=Habanero.UI.Base.MessageBoxIcon;
using ScrollBars=System.Windows.Forms.ScrollBars;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Creates controls for the System.Windows.Forms UI environment
    /// </summary>
    public class ControlFactoryWin : IControlFactory
    {
        //This looks like it was planned to move common functionality between Win and Giz to a 
        // manger but this has obviosly not been implemented
        //I would suggest it is remvoed Brett 24/02/2009
        //private readonly ControlFactoryManager _manager;
        //        /<summary>
        //        / Construct <see cref="ControlFactoryWin"/>
        //        /</summary>
        //        public virtual ControlFactoryWin()
        //        {
        //            //_manager = new ControlFactoryManager(this);
        //        }
        


        #region IControlFactory Members

        /// <summary>
        /// Creates a filter control with the default layout manager
        /// </summary>
        public virtual IFilterControl CreateFilterControl()
        {
            return new FilterControlWin(this);
        }

        /// <summary>
        /// Creates a TextBox control
        /// </summary>
        public virtual ITextBox CreateTextBox()
        {
            return new TextBoxWin();
        }

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        /// <param name="name">The name of the TreeView</param>
        public virtual ITreeView CreateTreeView(string name)
        {
            ITreeView treeView = new TreeViewWin(this);
            treeView.Name = name;
            return treeView;
        }

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        public virtual ITreeView CreateTreeView()
        {
            return CreateTreeView("TreeView");
        }

        ///<summary>
        /// Creates a new TreeNode for a TreeView control.
        ///</summary>
        ///<param name="nodeName">The name for the node</param>
        ///<returns>The newly created TreeNode object.</returns>
        public virtual ITreeNode CreateTreeNode(string nodeName)
        {
            return new TreeViewWin.TreeNodeWin(this, nodeName);
        }

        /// <summary>
        /// Creates a control for the given type and assembly name
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        /// the default type, which is usually TextBox.</returns>
        public virtual IControlHabanero CreateControl(string typeName, string assemblyName)
        {
            Type controlType = null;

            if (String.IsNullOrEmpty(typeName)) return CreateControl(typeof (TextBox));

            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = "System.Windows.Forms";
            }
            TypeLoader.LoadClassType(ref controlType, assemblyName, typeName,
                                         "field", "field definition");
            

            return CreateControl(controlType);
        }

        /// <summary>
        /// Creates a new control of the type specified
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of <see cref="IControlHabanero"/></param>
        public virtual IControlHabanero CreateControl(Type controlType)
        {
            IControlHabanero ctl;
            if (controlType.IsSubclassOf(typeof (Control)))
            {
                if (controlType == typeof (ComboBox)) return CreateComboBox();
                if (controlType == typeof (CheckBox)) return CreateCheckBox();
                if (controlType == typeof (TextBox)) return CreateTextBox();
                if (controlType == typeof (ListBox)) return CreateListBox();
                if (controlType == typeof (DateTimePicker)) return CreateDateTimePicker();
                if (controlType == typeof (NumericUpDown)) return CreateNumericUpDownInteger();
                if (controlType == typeof (EditableGridControlWin)) return CreateEditableGridControl();

                ctl = (IControlHabanero) Activator.CreateInstance(controlType);
                PropertyInfo infoFlatStyle =
                    ctl.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);
                if (infoFlatStyle != null)
                {
                    infoFlatStyle.SetValue(ctl, FlatStyle.Standard, new object[] {});
                }
            }
            else
            {
                throw new UnknownTypeNameException(
                    String.Format(
                        "The control type name {0} does not inherit from {1}.", controlType.FullName, typeof (Control)));
            }
            return ctl;
        }

        /// <summary>
        /// Creates a new DateTimePicker with a specified date
        /// </summary>
        /// <param name="defaultDate">The initial date value</param>
        public virtual IDateTimePicker CreateDateTimePicker(DateTime defaultDate)
        {
            DateTimePickerWin dateTimePickerWin = new DateTimePickerWin(this);
            dateTimePickerWin.Value = defaultDate;
            return dateTimePickerWin;
        }

        /// <summary>
        /// Creates a new DateRangeComboBox control
        /// </summary>
        public virtual IDateRangeComboBox CreateDateRangeComboBox()
        {
            return new DateRangeComboBoxWin();
        }

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        public virtual IDateTimePicker CreateMonthPicker()
        {
            DateTimePickerWin editor = (DateTimePickerWin)CreateDateTimePicker();
            editor.Format = (System.Windows.Forms.DateTimePickerFormat) DateTimePickerFormat.Custom;
            editor.CustomFormat = "MMM yyyy";
            return editor;
        }

        ///<summary>
        /// Creates a new numeric up-down control
        ///</summary>
        ///<returns>The created NumericUpDown control</returns>
        public virtual INumericUpDown CreateNumericUpDown()
        {
            return new NumericUpDownWin();
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        public virtual INumericUpDown CreateNumericUpDownInteger()
        {
            INumericUpDown ctl = CreateNumericUpDown();
            ctl.DecimalPlaces = 0;
            ctl.Maximum = Int32.MaxValue;
            ctl.Minimum = Int32.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for currency use
        /// </summary>
        public virtual INumericUpDown CreateNumericUpDownCurrency()
        {
            INumericUpDown ctl = CreateNumericUpDown();
            ctl.DecimalPlaces = 2;
            ctl.Maximum = Decimal.MaxValue;
            ctl.Minimum = Decimal.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        public virtual IProgressBar CreateProgressBar()
        {
            return new ProgressBarWin();
        }

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        public virtual ISplitter CreateSplitter()
        {
            return new SplitterWin();
        }

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        public virtual ITabPage CreateTabPage(string title)
        {
            return new TabPageWin {Text = title, Name = title};
        }

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        public virtual IRadioButton CreateRadioButton(string text)
        {
            RadioButtonWin rButton = new RadioButtonWin();
            rButton.Text = text;
            //TODO_REmoved when porting rButton.AutoCheck = true;
            //TODO_REmoved when portingrButton.FlatStyle = FlatStyle.Standard;
            rButton.Width = CreateLabel(text, false).PreferredWidth + 25;
            return rButton;
        }

        /// <summary>
        /// Creates a new GroupBox
        /// </summary>
        public virtual IGroupBox CreateGroupBox()
        {
            return new GroupBoxWin();
        }

        /// <summary>
        /// Creates a new GroupBox with the specified text as the title.
        /// </summary>
        public virtual IGroupBox CreateGroupBox(string text)
        {
            IGroupBox groupBox = CreateGroupBox();
            groupBox.Text = text;
            groupBox.Name = text;
            return groupBox;
        }

        /// <summary>
        /// Creates a TabControl
        /// </summary>
        public virtual ITabControl CreateTabControl()
        {
            return new TabControlWin();
        }

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines">The number of lines to show in the TextBox</param>
        public virtual ITextBox CreateTextBoxMultiLine(int numLines)
        {
            TextBoxWin tb = (TextBoxWin) CreateTextBox();
            tb.Multiline = true;
            tb.AcceptsReturn = true;
            tb.Height = tb.Height*numLines;
            tb.ScrollBars = ScrollBars.Vertical;
            return tb;
        }

        /// <summary>
        /// Creates a control that can be placed on a form or a panel to implement a wizard user interface.
        /// The wizard control will have a next and previous button and a panel to place the wizard step on.
        /// </summary>
        /// <param name="wizardController">The controller that manages the wizard process</param>
        public virtual IWizardControl CreateWizardControl(IWizardController wizardController)
        {
            return new WizardControlWin(wizardController, this);
        }

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <returns></returns>
        public virtual IWizardForm CreateWizardForm(IWizardController wizardController)
        {
            return new WizardFormWin(wizardController, this);
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="action">Action to be performed when the editing is completed or cancelled. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid)</param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName,
                                                       PostObjectEditDelegate action)
        {
            return new DefaultBOEditorFormWin(bo, uiDefName, this, action);
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="groupControlCreator">The Creator that will be used to Create the <see cref="IGroupControl"/></param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName, GroupControlCreator groupControlCreator)
        {
            return new DefaultBOEditorFormWin(bo, uiDefName, this, groupControlCreator);
        }

        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in  a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid
        /// </summary>
        /// <param name="bo"> </param>
        /// <returns></returns>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo)
        {
            return new DefaultBOEditorFormWin(bo, "default", this);
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName)
        {
            return new DefaultBOEditorFormWin(bo, uiDefName, this);
        }

        //TODO: Port
//        public virtual IListView CreateListView()
//        {
//            throw new NotImplementedException();
//        }

        /// <summary>
        /// Creates an editable grid
        /// </summary>
        public virtual IEditableGrid CreateEditableGrid()
        {
            return new EditableGridWin();
        }

        /// <summary>
        /// Creates an EditableGridControl
        /// </summary>
        public virtual IEditableGridControl CreateEditableGridControl()
        {
            return new EditableGridControlWin(this);
        }

        /// <summary>
        /// Creates a FileChooser control
        /// </summary>
        public virtual IFileChooser CreateFileChooser()
        {
            return new FileChooserWin(this);
        }

        /// <summary>
        /// Displays a business object collection in a tab control, with one
        /// business object per tab.  Each tab holds a business control, provided
        /// by the developer, that refreshes to display the business object for
        /// the current tab.
        /// <br/>
        /// This control is suitable for a business object collection with a limited
        /// number of objects.
        /// </summary>
        public virtual IBOColTabControl CreateBOColTabControl()
        {
            return new BOColTabControlWin(this);
        }

        /// <summary>
        /// Creates a control mapper strategy for the management of how
        /// business object properties and their related controls update each other.
        /// For example, a windows strategy might be to update the control value whenever the property 
        /// is updated, whereas an internet strategy might be to update the control value only
        /// when the business object is loaded.
        /// </summary>
        public virtual IControlMapperStrategy CreateControlMapperStrategy()
        {
            return new ControlMapperStrategyWin();
        }

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        public virtual ITextBoxMapperStrategy CreateTextBoxMapperStrategy()
        {
            return new TextBoxMapperStrategyWin();
        }

        ///<summary>
        /// Creates a DataGridView
        ///</summary>
        public virtual IDataGridView CreateDataGridView()
        {
            return new DataGridViewWin();
        }

        /// <summary>
        /// Creates a DataGridViewImageColumn
        /// </summary>
        public virtual IDataGridViewImageColumn CreateDataGridViewImageColumn()
        {
            return new DataGridViewImageColumnWin(new DataGridViewImageColumn());
        }

        /// <summary>
        /// Creates a DataGridViewTextBoxColumn
        /// </summary>
        public virtual IDataGridViewColumn CreateDataGridViewTextBoxColumn()
        {
            return new DataGridViewColumnWin(new DataGridViewTextBoxColumn());
        }

        /// <summary>
        /// Creates a DataGridViewCheckBoxColumn
        /// </summary>
        public virtual IDataGridViewCheckBoxColumn CreateDataGridViewCheckBoxColumn()
        {
            return new DataGridViewCheckBoxColumnWin(new DataGridViewCheckBoxColumn());
        }

        /// <summary>
        /// Creates a DataGridViewComboBoxColumn
        /// </summary>
        public virtual IDataGridViewComboBoxColumn CreateDataGridViewComboBoxColumn()
        {
            return new DataGridViewComboBoxColumnWin(new DataGridViewComboBoxColumn());
        }

        /// <summary>
        /// Creates a DataGridViewDateTimeColumn
        /// </summary>
        public virtual IDataGridViewDateTimeColumn CreateDataGridViewDateTimeColumn()
        {
            //return new DataGridViewDateTimeColumnWin(new DataGridViewColumn());
            return new DataGridViewDateTimeColumnWin(new DataGridViewDateTimeColumn());
        }

        ///<summary>
        /// Creates a DataGridViewNumericUpDownColumn
        ///</summary>
        ///<returns>A new DataGridViewNumericUpDownColumn</returns>
        public virtual IDataGridViewNumericUpDownColumn CreateDataGridViewNumericUpDownColumn()
        {
            return new DataGridViewNumericUpDownColumnWin(new DataGridViewNumericUpDownColumn());
        }

        /// <summary>
        /// Creates a column for a DataGridView for the given type
        /// </summary>
        /// <param name="typeName">The name of the type</param>
        /// <param name="assemblyName">The name of the assembly</param>
        public virtual IDataGridViewColumn CreateDataGridViewColumn(string typeName, string assemblyName)
        {
            Type controlType = null;

            if (String.IsNullOrEmpty(typeName))
            {
                typeName = "DataGridViewTextBoxColumn";
            }

            if (String.IsNullOrEmpty(assemblyName))
            {
                switch (typeName)
                {
                    case "DataGridViewNumericUpDownColumn":
                    case "DataGridViewDateTimeColumn":
                        assemblyName = "Habanero.UI.Win";
                        break;
                    default:
                        assemblyName = "System.Windows.Forms";
                        break;
                }
            }

            TypeLoader.LoadClassType(ref controlType, assemblyName, typeName,
                                         "column", "column definition");

            return CreateDataGridViewColumn(controlType);
        }

        /// <summary>
        /// Creates a column for a DataGridView for the given type
        /// </summary>
        /// <param name="columnType">The type of the column</param>
        public virtual IDataGridViewColumn CreateDataGridViewColumn(Type columnType)
        {
            if (!columnType.IsSubclassOf(typeof(DataGridViewColumn)))
            {
                throw new UnknownTypeNameException(
                    String.Format(
                        "The column type name {0} does not inherit from {1}.", columnType.FullName,
                        typeof(DataGridViewColumn)));
            }

            if (columnType == typeof(DataGridViewCheckBoxColumn)) return CreateDataGridViewCheckBoxColumn();
            if (columnType == typeof(DataGridViewComboBoxColumn)) return CreateDataGridViewComboBoxColumn();
            if (columnType == typeof(DataGridViewDateTimeColumn)) return CreateDataGridViewDateTimeColumn();
            if (columnType == typeof(DataGridViewNumericUpDownColumn)) return CreateDataGridViewNumericUpDownColumn();
            if (columnType == typeof(DataGridViewImageColumn)) return CreateDataGridViewImageColumn();

            return new DataGridViewColumnWin((DataGridViewColumn) Activator.CreateInstance(columnType));
        }

        /// <summary>
        /// Creates DateRangeComboBox control with a specific set of date range
        /// options to display
        /// </summary>
        /// <param name="optionsToDisplay">A list of date range options to display</param>
        public virtual IDateRangeComboBox CreateDateRangeComboBox(List<DateRangeOptions> optionsToDisplay)
        {
            return new DateRangeComboBoxWin(optionsToDisplay);
        }

       

        /// <summary>
        /// Creates an ErrorProvider
        /// </summary>
        public virtual IErrorProvider CreateErrorProvider()
        {
            return new ErrorProviderWin();
        }

        /// <summary>
        /// Creates a Form control
        /// </summary>
        public virtual IFormHabanero CreateForm()
        {
            return new FormWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a CheckBox for the environment
        /// </summary>
        public virtual ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy()
        {
            return new CheckBoxStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a ComboBox for the environment
        /// </summary>
        public virtual IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy()
        {
            return new ListComboBoxMapperStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a lookup ComboBox for the environment
        /// </summary>
        public virtual ILookupComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy()
        {
            return new LookupComboBoxDefaultMapperStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        public virtual ILookupComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy()
        {
            return new LookupComboBoxKeyPressMapperStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a NumericUpDown for the environment
        /// </summary>
        public virtual INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy()
        {
            return new NumericUpDownMapperStrategyWin();
        }

        /// <summary>
        /// Creates an buttons control for an <see cref="IEditableGridControl"/>
        /// </summary>
        public virtual IEditableGridButtonsControl CreateEditableGridButtonsControl()
        {
            return new EditableGridButtonsControlWin(this);
        }

        /// <summary>
        /// Creates an OKCancelDialog
        /// </summary>
        public virtual IOKCancelDialogFactory CreateOKCancelDialogFactory()
        {
            return new OKCancelDialogFactoryWin(this);
        }

        /// <summary>
        /// Creates a static data editor
        /// </summary>
        public virtual IStaticDataEditor CreateStaticDataEditor()
        {
            return new StaticDataEditorWin(this);
        }

        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<returns>The message box result.</returns>
        public virtual Base.DialogResult ShowMessageBox(string message, string title, Base.MessageBoxButtons buttons, Base.MessageBoxIcon icon)
        {
            return (Base.DialogResult)MessageBox.Show(message, title, 
                (System.Windows.Forms.MessageBoxButtons)buttons, (System.Windows.Forms.MessageBoxIcon)icon);
        }
        
        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// Once the user is has responded, the provided delegate is called with an indication of the <see cref="DialogResult"/>.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<param name="dialogCompletionDelegate">A delegate to be called when the MessageBox has been completed.</param>
        ///<returns>The message box result.</returns>
        public virtual DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, DialogCompletionDelegate dialogCompletionDelegate)
        {
            System.Windows.Forms.MessageBoxButtons messageBoxButtons = (System.Windows.Forms.MessageBoxButtons)buttons;
            System.Windows.Forms.MessageBoxIcon messageBoxIcon = (System.Windows.Forms.MessageBoxIcon)icon;
            DialogResult dialogResult = (Base.DialogResult)MessageBox.Show(message, title, messageBoxButtons, messageBoxIcon);
            dialogCompletionDelegate(null, dialogResult);
            return dialogResult;
        }

        ///<summary>
        /// Displays a message box with specified text.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<returns>The message box result.</returns>
        public virtual Base.DialogResult ShowMessageBox(string message)
        {
            Cursor.Current = Cursors.Default;
            return (Base.DialogResult)MessageBox.Show(message);
        }

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        public virtual IComboBox CreateComboBox()
        {
            ComboBoxWin comboBoxWin = new ComboBoxWin();
            //Note_: This is a workaround in windows to avoid this default from breaking all the tests because if the Thread's ApartmentState is not STA then setting the AutoCompleteSource default gives an error
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                comboBoxWin.AutoCompleteSource =  AutoCompleteSource.ListItems;
                comboBoxWin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            return comboBoxWin;
        }

        /// <summary>
        /// Creates a ListBox control
        /// </summary>
        /// <returns></returns>
        public virtual IListBox CreateListBox()
        {
            return new ListBoxWin();
        }

        /// <summary>
        /// Creates a multi-selector control
        /// </summary>
        /// <typeparam name="T">The business object type being managed in the control</typeparam>
        public virtual IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>(this);
        }

        /// <summary>
        /// Creates a button control
        /// </summary>
        public virtual IButton CreateButton()
        {
            return new ButtonWin();
        }

        /// <summary>
        /// Creates a button control
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        public virtual IButton CreateButton(string text)
        {
            IButton button = CreateButton();
            button.Text = text;
            button.Name = text;
            ((Button)button).FlatStyle = FlatStyle.Standard;
            button.Width = CreateLabel(text, false).PreferredWidth + 20;
            return button;
        }

        /// <summary>
        /// Creates a button control with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        public virtual IButton CreateButton(string text, EventHandler clickHandler)
        {
            IButton button = CreateButton(text);
            button.Click += clickHandler;
            return button;
        }

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        public virtual ICheckBox CreateCheckBox()
        {
            return new CheckBoxWin();
        }

        /// <summary>
        /// Creates a CheckBox control with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is checked</param>
        public virtual ICheckBox CreateCheckBox(bool defaultValue)
        {
            ICheckBox checkBox = CreateCheckBox();
            checkBox.Checked = defaultValue;
            return checkBox;
        }

        /// <summary>
        /// Creates a label without text
        /// </summary>
        public virtual ILabel CreateLabel()
        {
            ILabel label = new LabelWin();
            label.TabStop = false;
            return label;
        }

        /// <summary>
        /// Creates a label with specified text
        /// </summary>
        public virtual ILabel CreateLabel(string labelText)
        {
            ILabel label = CreateLabel(labelText, false);
            label.Text = labelText;
            return label;
        }

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="labelText">The text to appear in the label</param>
        /// <param name="isBold">Whether the text appears in bold font</param>
        public virtual ILabel CreateLabel(string labelText, bool isBold)
        {
            LabelWin label = (LabelWin) CreateLabel();
            label.Text = labelText;
            label.FlatStyle = FlatStyle.System;
            if (isBold)
            {
                label.Font = new Font(label.Font, FontStyle.Bold);
            }
            label.Width = label.PreferredWidth;
            if (isBold)
            {
                label.Width += 10;
            }
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.TabStop = false;
            return label;
        }

        /// <summary>
        /// Creates a DateTimePicker
        /// </summary>
        public virtual IDateTimePicker CreateDateTimePicker()
        {
            return new DateTimePickerWin(this);
        }

        /// <summary>
        /// Creates a BorderLayoutManager to place controls on the given parent control
        /// </summary>
        /// <param name="control">The parent control on which to managed the layout</param>
        public virtual BorderLayoutManager CreateBorderLayoutManager(IControlHabanero control)
        {
            return new BorderLayoutManagerWin(control, this);
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        public virtual IPanel CreatePanel()
        {
            return new PanelWin();
        }

        /// <summary>
        /// Creates a read-only Grid
        /// </summary>
        public virtual IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridWin();
        }

        /// <summary>
        /// Creates a ReadOnlyGridControl
        /// </summary>
        public virtual IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return new ReadOnlyGridControlWin(this);
        }

        /// <summary>
        /// Creates a control to manage a group of buttons that display next to each other
        /// </summary>
        public virtual IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlWin(this);
        }

        /// <summary>
        /// Creates a buttons control for a <see cref="IReadOnlyGridControl"/>
        /// </summary>
        public virtual IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            return new ReadOnlyGridButtonsControlWin(this);
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Replaced by IBOGridAndEditorControl: Brett 03 Mar 2009")]
        public virtual IGridWithPanelControl<T> CreateGridWithPanelControl<T>() where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlWin<T>(this, "default");
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Replaced by IBOGridAndEditorControl:: Brett 03 Mar 2009")]
        public virtual IGridWithPanelControl<T> CreateGridWithPanelControl<T>(string uiDefName) where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlWin<T>(this, uiDefName);
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Replaced by IBOGridAndEditorControl:: Brett 03 Mar 2009")]
        public virtual IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl) where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlWin<T>(this, businessObjectControl);
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Replaced by IBOGridAndEditorControl:: Brett 03 Mar 2009")]
        public virtual IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl, string uiDefName) where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlWin<T>(this, businessObjectControl, uiDefName);
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual IPanel CreatePanel(IControlFactory controlFactory)
        {
            return new PanelWin();
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual IPanel CreatePanel(string name, IControlFactory controlFactory)
        {
            IPanel panel = CreatePanel();
            panel.Name = name;
            return panel;
        }

        /// <summary>
        /// Creates a new PasswordTextBox that masks the letters as the user
        /// types them
        /// </summary>
        /// <returns>Returns the new PasswordTextBox object</returns>
        public virtual ITextBox CreatePasswordTextBox()
        {
            ITextBox tb = CreateTextBox();
            tb.PasswordChar = '*';
            return tb;
        }

        /// <summary>
        /// Creates a ToolTip
        /// </summary>
        public virtual IToolTip CreateToolTip()
        {
            return new ToolTipWin();
        }

        /// <summary>
        /// Creates a generic control
        /// </summary>
        public virtual IControlHabanero CreateControl()
        {
            return new ControlWin();
        }

        /// <summary>
        /// Creates a user control
        /// </summary>
        public virtual IUserControlHabanero CreateUserControl()
        {
            return new UserControlWin();
        }

        /// <summary>
        /// Creates a user control with the specified name.
        /// </summary>
        public virtual IUserControlHabanero CreateUserControl(string name)
        {
            IUserControlHabanero userControlHabanero = CreateUserControl();
            userControlHabanero.Name = name;
            return userControlHabanero;
        }

        #endregion

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        /// <param name="propertyType">Type property being edited.</param>
        public virtual ITextBox CreateTextBox(Type propertyType)
        {
            return new TextBoxWin();
        }

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
         public virtual IPictureBox CreatePictureBox()
        {
            return new PictureBoxWin();
        }

        ///<summary>
        /// Creates a <see cref="IDateTimePickerMapperStrategy"/>
        ///</summary>
        public virtual IDateTimePickerMapperStrategy CreateDateTimePickerMapperStrategy()
        {
            return new DateTimePickerMapperStrategyWin();
        }
        ///<summary>
        /// Creates a <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        public virtual IBOGridAndEditorControl CreateGridAndBOEditorControl<TBusinessObject>() where TBusinessObject : class, IBusinessObject
        {
            return new BOGridAndEditorControlWin<TBusinessObject>(this, "default");

        }

        ///<summary>
        /// Creates a <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        public virtual IBOGridAndEditorControl CreateGridAndBOEditorControl(IClassDef classDef)
        {
            return new BOGridAndEditorControlWin(this, classDef, "default");
        }

        ///<summary>
        /// Creates a <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        public virtual IBOGridAndEditorControl CreateGridAndBOEditorControl<TBusinessObject>(IBOEditorControl editorControlPanel) where TBusinessObject : class, IBusinessObject
        {

            return new BOGridAndEditorControlWin<TBusinessObject>(this, editorControlPanel);
        }

//        public virtual IBOGridAndEditorControl CreateGridAndBOEditorControl(IBOEditorControl boEditorPanel,IBusinessObject businessObject)
//        {
//            return new BOGridAndEditorControlWin<IBusinessObject>(this, boEditorPanel,businessObject);
//        }

        ///<summary>
        /// Creates a <see cref="ICollapsiblePanel"/>
        ///</summary>
        public virtual ICollapsiblePanel CreateCollapsiblePanel()
        {
            return new CollapsiblePanelWin(this);
        }

        ///<summary>
        /// Creates a <see cref="ICollapsiblePanel"/>
        ///</summary>
        public virtual ICollapsiblePanel CreateCollapsiblePanel(string name)
        {
            CollapsiblePanelWin collapsiblePanel = new CollapsiblePanelWin(this) { Text = name, Name = name };
            collapsiblePanel.CollapseButton.Text = name;
            collapsiblePanel.CollapseButton.Name = name;
            return collapsiblePanel;
        }

        /// <summary>
        /// Creates a new TabPage
        /// </summary>
        public virtual ITabPage createTabPage(string name)
        {
            throw new NotImplementedException();
        }

        #region Collapsible Panel Button Creators

        ///<summary>
        /// Creates a <see cref="IButton"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="IButton"/> </returns>
        public virtual IButton CreateButtonCollapsibleStyle()
        {
            ButtonWin button = (ButtonWin)CreateButton();
            ConfigureCollapsibleStyleButton(button);
            return button;
        }

        private static void ConfigureCollapsibleStyleButton(IButton button)
        {
            ButtonWin buttonWin = ((ButtonWin)button);
            buttonWin.BackgroundImage = CollapsiblePanelResource.headergradient;
            buttonWin.FlatStyle = FlatStyle.Flat;

        }

        ///<summary>
        /// Creates a <see cref="ILabel"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="ILabel"/> </returns>
        public virtual ILabel CreateLabelPinOffStyle()
        {
            LabelWin label = (LabelWin)CreateLabel();
            ConfigurePinOffStyleLabel(label);
            return label;
        }

        ///<summary>
        /// Configures the <see cref="ILabel"/> with the pinoff style
        ///</summary>
        public virtual void ConfigurePinOffStyleLabel(ILabel label)
        {
            LabelWin labelWin = (LabelWin)label;
            labelWin.BackgroundImage = CollapsiblePanelResource.pinoff_withcolour;
            labelWin.FlatStyle = FlatStyle.Flat;
            //labelWin.ForeColor = Color.White;
            labelWin.BackgroundImageLayout = ImageLayout.Center;
            labelWin.Width = 24;
        }

        ///<summary>
        ///</summary>
        ///<param name="label"></param>
        public virtual void ConfigurePinOnStyleLabel(ILabel label)
        {
            LabelWin labelWin = (LabelWin)label;
            labelWin.BackgroundImage = CollapsiblePanelResource.pinon_withcolour;
            labelWin.FlatStyle = FlatStyle.Flat;
            //labelWin.ForeColor = Color.White;
            labelWin.BackgroundImageLayout = ImageLayout.Center;
            labelWin.Width = 24;
        }

        ///<summary>
        /// Craetes an <see cref="ICollapsiblePanelGroupControl"/>
        ///</summary>
        ///<returns></returns>
        public virtual ICollapsiblePanelGroupControl CreateCollapsiblePanelGroupControl()
        {
            return new CollapsiblePanelGroupControlWin();
        }

        ///<summary>
        /// Creates a <see cref="IGroupBoxGroupControl"/>
        ///</summary>
        ///<returns></returns>
        public virtual IGroupBoxGroupControl CreateGroupBoxGroupControl()
        {
            return new GroupBoxGroupControlWin(this);
        }

        #endregion
        ///<summary>
        /// Creates an <see cref="IBOComboBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        public virtual IBOComboBoxSelector CreateComboBoxSelector()
        {
            ComboBoxSelectorWin comboBoxWin = new ComboBoxSelectorWin(this);
            //Note_: This is a workaround in windows to avoid this default from breaking all the tests 
            //  because if the Thread's ApartmentState is not STA then setting the AutoCompleteSource default 
            //  gives an error
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                comboBoxWin.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBoxWin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            return comboBoxWin;
        }

        ///<summary>
        /// Creates an <see cref="IBOListBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        public virtual IBOListBoxSelector CreateListBoxSelector()
        {
            return new ListBoxSelectorWin(this);
        }

        ///<summary>
        /// Creates an <see cref="IBOCollapsiblePanelSelector"/>
        ///</summary>
        ///<returns></returns>
        public virtual IBOCollapsiblePanelSelector CreateCollapsiblePanelSelector()
        {
            return new CollapsiblePanelSelectorWin(this);
        }

        /// <summary>
        /// Creates an <see cref="IMainMenuHabanero"/>
        /// </summary>
        /// <returns></returns>
        public virtual IMainMenuHabanero CreateMainMenu()
        {
            return new MainMenuWin();
        }

        /// <summary>
        /// Creates an <see cref="IMenuItem"/> with the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>returns the Created MenuItem</returns>
        public virtual IMenuItem CreateMenuItem(string name)
        {
            return new MenuItemWin(name);
        }

        /// <summary>
        /// Creates an <see cref="IMenuItem"/> with the name.
        /// </summary>
        /// <param name="item">the HabaneroMenu.Item that the IMenuItem is being created for</param>
        /// <returns>returns the Created MenuItem</returns>
        public virtual IMenuItem CreateMenuItem(HabaneroMenu.Item item)
        {
            return new MenuItemWin(item);
        }

        /// <summary>
        /// Creates an <see cref="IMainMenuHabanero"/> with associated habaneroMenu.
        /// </summary>
        /// <param name="habaneroMenu">the HabaneroMenu that the IMainMenuHabanero is being created for</param>
        /// <returns>returns the Created IMainMenuHabanero</returns>
        public virtual IMainMenuHabanero CreateMainMenu(HabaneroMenu habaneroMenu)
        {
            return new MainMenuWin(habaneroMenu);
        }

        /// <summary>
        /// Creates an <see cref="ISplitContainer"/>
        /// </summary>
        /// <returns>returns the created split container</returns>
        public virtual ISplitContainer CreateSplitContainer()
        {
            return new SplitContainerWin();
        }



        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the Generic Type T.<br/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object (of type T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiDefName">The User Interface definition to use when creating the Control</param>
        /// <returns></returns>
        public virtual IBOPanelEditorControl CreateBOEditorControl<T>(string uiDefName) where T : class, IBusinessObject
        {
            return new BOEditorControlWin<T>(this, uiDefName);
        }

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the Generic Type T.<br/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object (of type T).<br/>
        /// The default uiDef is used as the user interface definition for defining which controls are used to view and edit this business object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IBOPanelEditorControl CreateBOEditorControl<T>() where T : class, IBusinessObject
        {
            return new BOEditorControlWin<T>(this, "default");
        }

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the type defined by <paramref name="classDef"/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object.<br/>
        /// </summary>
        /// <param name="classDef"></param>
        /// <param name="uiDefName">The uiDef defined in the classDef that is to be used.</param>
        /// <returns></returns>
        public virtual IBOPanelEditorControl CreateBOEditorControl(IClassDef classDef, string uiDefName)
        {
            return new BOEditorControlWin(this, classDef, uiDefName);
        }

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the type defined by <paramref name="classDef"/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object.<br/>
        /// The default uiDef is used as the user interface definition for defining which controls are used to view and edit this business object
        /// </summary>
        /// <param name="classDef"></param>
        /// <returns></returns>
        public virtual IBOPanelEditorControl CreateBOEditorControl(IClassDef classDef)
        {
            return new BOEditorControlWin(this, classDef, "default");
        }
        /// <summary>
        /// Creates a <see cref="MainTitleIconControlWin"/>
        /// </summary>
        /// <returns></returns>
        public virtual IMainTitleIconControl CreateMainTitleIconControl()
        {
            return new MainTitleIconControlWin(this);
        }
    }


}