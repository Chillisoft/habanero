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
using System.Drawing;
using System.Reflection;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.Util;
using DateTimePickerFormat=Habanero.UI.Base.DateTimePickerFormat;
using DialogResult=Habanero.UI.Base.DialogResult;
using HorizontalAlignment=Gizmox.WebGUI.Forms.HorizontalAlignment;
using MessageBoxButtons=Habanero.UI.Base.MessageBoxButtons;
using MessageBoxIcon=Habanero.UI.Base.MessageBoxIcon;
using ScrollBars=Gizmox.WebGUI.Forms.ScrollBars;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Creates controls for the Gizmox.WebGUI.Forms UI environment
    /// </summary>
    public class ControlFactoryVWG : IControlFactory
    {
        /// <summary>
        /// The standard height to be used when constructing a textbox
        /// </summary>
        public const int TEXTBOX_HEIGHT = 20;
        //I Guess that the manager was created with the idea of 
        // moving the common code in the Win and VWG control factory to a manager.
//        private readonly ControlFactoryManager _manager;

//        ///<summary>
//        /// base Constructor
//        ///</summary>
//        public ControlFactoryVWG()
//        {
////            _manager = new ControlFactoryManager(this);
//        }

        /// <summary>
        /// Creates a filter control with the default layout manager
        /// </summary>
        public virtual IFilterControl CreateFilterControl()
        {
            return new FilterControlVWG(this);
        }

        /// <summary>
        /// Creates a TextBox control
        /// </summary>
        public virtual ITextBox CreateTextBox()
        {
            TextBoxVWG tb = new TextBoxVWG();
            tb.Height = TEXTBOX_HEIGHT;
            tb.TextAlign = HorizontalAlignment.Left;
            return tb;
        }

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        public virtual IComboBox CreateComboBox()
        {
            ComboBoxVWG comboBox = new ComboBoxVWG();
            comboBox.Height = TEXTBOX_HEIGHT;
            comboBox.AutoCompleteSource = Gizmox.WebGUI.Forms.AutoCompleteSource.ListItems;
            comboBox.AutoCompleteMode = Gizmox.WebGUI.Forms.AutoCompleteMode.SuggestAppend;
            return comboBox;
        }

        /// <summary>
        /// Creates a ListBox control
        /// </summary>
        /// <returns></returns>
        public virtual IListBox CreateListBox()
        {
            return new ListBoxVWG();
        }

        /// <summary>
        /// Creates a multi-selector control
        /// </summary>
        /// <typeparam name="T">The business object type being managed in the control</typeparam>
        public virtual IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorVWG<T>(this);
        }

        /// <summary>
        /// Creates a button control
        /// </summary>
        public virtual IButton CreateButton()
        {
            return new ButtonVWG();
        }

        /// <summary>
        /// Creates a button control
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        public virtual IButton CreateButton(string text)
        {
            IButton btn = CreateButton();
            btn.Text = text;
            btn.Name = text;
            ((Button) btn).FlatStyle = FlatStyle.Standard;
            btn.Width = CreateLabel(text, false).PreferredWidth + 20;
            return btn;
        }

        /// <summary>
        /// Creates a button control with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        public virtual IButton CreateButton(string text, EventHandler clickHandler)
        {
            IButton btn = CreateButton(text);
            btn.Click += clickHandler;
            return btn;
        }

        /// <summary>
        /// Creates a label without text
        /// </summary>
        public virtual ILabel CreateLabel()
        {
            return CreateLabel("");
        }

        /// <summary>
        /// Creates a label with specified text
        /// </summary>
        public virtual ILabel CreateLabel(string labelText)
        {
            LabelVWG label = new LabelVWG(labelText);
            label.Width = label.PreferredWidth;
            label.Height = 15;
            label.TabStop = false;
            return label;
        }

        /// <summary>
        /// Creates a DateTimePicker
        /// </summary>
        public virtual IDateTimePicker CreateDateTimePicker()
        {
            DateTimePickerVWG dtp = new DateTimePickerVWG(this);
            dtp.Height = TEXTBOX_HEIGHT;
            dtp.Format = (Gizmox.WebGUI.Forms.DateTimePickerFormat) DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd MMM yyyy";
            return dtp;
        }

        /// <summary>
        /// Creates a BorderLayoutManager to place controls on the given parent control
        /// </summary>
        /// <param name="control">The parent control on which to managed the layout</param>
        public virtual BorderLayoutManager CreateBorderLayoutManager(IControlHabanero control)
        {
            return new BorderLayoutManagerVWG(control, this);
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        public virtual IPanel CreatePanel()
        {
            return new PanelVWG();
        }

        /// <summary>
        /// Creates a read-only Grid
        /// </summary>
        public virtual IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridVWG();
        }


        /// <summary>
        /// Creates a ReadOnlyGridControl
        /// </summary>
        public virtual IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return new ReadOnlyGridControlVWG(this);
        }

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete(" Replaced by IBOSelectorAndEditor: Brett 03 Mar 2009")]
        public IGridWithPanelControl<T> CreateGridWithPanelControl<T>() where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlVWG<T>(this, "default");
        }

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete(" Replaced by IBOSelectorAndEditor: Brett 03 Mar 2009")]
        public IGridWithPanelControl<T> CreateGridWithPanelControl<T>(string uiDefName) where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlVWG<T>(this, uiDefName);
        }

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete(" Replaced by IBOSelectorAndEditor: Brett 03 Mar 2009")]
        public IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl) where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlVWG<T>(this, businessObjectControl, "default");
        }

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete(" Replaced by IBOSelectorAndEditor: Brett 03 Mar 2009")]
        public IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl, string uiDefName) where T : class, IBusinessObject, new()
        {
            return new GridWithPanelControlVWG<T>(this, businessObjectControl, uiDefName);
        }

        /// <summary>
        /// Creates a control to manage a group of buttons that display next to each other
        /// </summary>
        public virtual IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlVWG(this);
        }

        /// <summary>
        /// Creates a buttons control for a <see cref="IReadOnlyGridControl"/>
        /// </summary>
        public virtual IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            return new ReadOnlyGridButtonsControlVWG(this);
        }


        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual IPanel CreatePanel(IControlFactory controlFactory)
        {
            IPanel pnl = new PanelVWG();
            return pnl;
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual IPanel CreatePanel(string name, IControlFactory controlFactory)
        {
            IPanel pnl = CreatePanel(controlFactory);
            pnl.Name = name;
            return pnl;
        }

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="labelText">The text to appear in the label</param>
        /// <param name="isBold">Whether the text appears in bold font</param>
        public virtual ILabel CreateLabel(string labelText, bool isBold)
        {
            ILabel lbl = CreateLabel(labelText);
            lbl.AutoSize = true;
            lbl.Text = labelText;
            ((Label) lbl).FlatStyle = FlatStyle.Standard;
            if (isBold)
            {
                lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                //lbl.Text = labelText + " *";
            }
            lbl.Width = lbl.PreferredWidth;
            if (isBold)
            {
                //lbl.Width += 10;
                lbl.Width += 14;
            }
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.TabStop = false;
            return lbl;
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
            return new ToolTipVWG();
        }

        /// <summary>
        /// Creates a generic control
        /// </summary>
        public virtual IControlHabanero CreateControl()
        {
            ControlVWG cntrl = new ControlVWG();
            cntrl.Height = 10;
            return cntrl;
        }

        /// <summary>
        /// Creates a user control
        /// </summary>
        public IUserControlHabanero CreateUserControl()
        {
            return new UserControlVWG();
        }

        /// <summary>
        /// Creates a user control with the specified name.
        /// </summary>
        public IUserControlHabanero CreateUserControl(string name)
        {
            IUserControlHabanero userControlHabanero = CreateUserControl();
            userControlHabanero.Name = name;
            return userControlHabanero;
        }

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        /// <param name="name">The name of the TreeView</param>
        public virtual ITreeView CreateTreeView(string name)
        {
            ITreeView tv = new TreeViewVWG();
            tv.Name = name;
            return tv;
        }

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        public ITreeView CreateTreeView()
        {
            return CreateTreeView("TreeView");
        }

        ///<summary>
        /// Creates a new TreeNode for a TreeView control.
        ///</summary>
        ///<param name="nodeName">The name for the node</param>
        ///<returns>The newly created TreeNode object.</returns>
        public ITreeNode CreateTreeNode(string nodeName)
        {
            return new TreeViewVWG.TreeNodeVWG(nodeName);
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

            if (String.IsNullOrEmpty(typeName)) return CreateControl(typeof(TextBox));

            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = "Gizmox.WebGUI.Forms";
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
                if (controlType == typeof (NumericUpDown)) return CreateNumericUpDown();
                try
                {
                    ctl = (IControlHabanero) Activator.CreateInstance(controlType);
                }
                catch (MissingMethodException)
                {
                    ctl = (IControlHabanero) Activator.CreateInstance(controlType, new object[] {this});
                }
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
                    string.Format(
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
            IDateTimePicker editor = CreateDateTimePicker();
            editor.Value = defaultDate;
            return editor;
        }

        /// <summary>
        /// Creates a new DateRangeComboBox control
        /// </summary>
        public virtual IDateRangeComboBox CreateDateRangeComboBox()
        {
            DateRangeComboBoxVWG dateRangeCombo = new DateRangeComboBoxVWG();
            dateRangeCombo.Height = CreateTextBox().Height;
            return dateRangeCombo;
        }

        /// <summary>
        /// Creates DateRangeComboBox control with a specific set of date range
        /// options to display
        /// </summary>
        /// <param name="optionsToDisplay">A list of date range options to display</param>
        public virtual IDateRangeComboBox CreateDateRangeComboBox(List<DateRangeOptions> optionsToDisplay)
        {
            return new DateRangeComboBoxVWG(optionsToDisplay);
        }

        /// <summary>
        /// Creates an ErrorProvider
        /// </summary>
        public IErrorProvider CreateErrorProvider()
        {
            return new ErrorProviderVWG();
        }

        /// <summary>
        /// Creates a Form control
        /// </summary>
        public IFormHabanero CreateForm()
        {
            return new FormVWG();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a CheckBox for the environment
        /// </summary>
        public ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy()
        {
            return new CheckBoxMapperStrategyVWG();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a ComboBox for the environment
        /// </summary>
        public IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy()
        {
            return new ListComboBoxMapperStrategyVWG();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a lookup ComboBox for the environment
        /// </summary>
        public ILookupComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy()
        {
            return new LookupComboBoxMapperStrategyVWG();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        public ILookupComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy()
        {
            return new LookupComboBoxKeyPressMapperStrategyVWG();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a NumericUpDown for the environment
        /// </summary>
        public INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy()
        {
            return new NumericUpDownMapperStrategyVWG();
        }

        /// <summary>
        /// Creates an buttons control for an <see cref="IEditableGridControl"/>
        /// </summary>
        public IEditableGridButtonsControl CreateEditableGridButtonsControl()
        {
            return new EditableGridButtonsControlVWG(this);
        }

        /// <summary>
        /// Creates an OKCancelDialog
        /// </summary>
        public IOKCancelDialogFactory CreateOKCancelDialogFactory()
        {
            return new OKCancelDialogFactoryVWG(this);
        }

        /// <summary>
        /// Creates a static data editor
        /// </summary>
        /// <returns></returns>
        public IStaticDataEditor CreateStaticDataEditor()
        {
            return new StaticDataEditorVWG(this);
        }

        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<returns>The message box result.</returns>
        public Base.DialogResult ShowMessageBox(string message, string title, Base.MessageBoxButtons buttons, Base.MessageBoxIcon icon)
        {
            return (Base.DialogResult)MessageBox.Show(message, title,
                (Gizmox.WebGUI.Forms.MessageBoxButtons)buttons, (Gizmox.WebGUI.Forms.MessageBoxIcon)icon);
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
        public DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, DialogCompletionDelegate dialogCompletionDelegate)
        {
            Gizmox.WebGUI.Forms.MessageBoxButtons messageBoxButtons = (Gizmox.WebGUI.Forms.MessageBoxButtons)buttons;
            Gizmox.WebGUI.Forms.MessageBoxIcon messageBoxIcon = (Gizmox.WebGUI.Forms.MessageBoxIcon)icon;
            return (Base.DialogResult)MessageBox.Show(message, title, messageBoxButtons, messageBoxIcon,
                (sender, e) => dialogCompletionDelegate(sender, (DialogResult)((Form) sender).DialogResult));
        }

        ///<summary>
        /// Displays a message box with specified text.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<returns>The message box result.</returns>
        public Base.DialogResult ShowMessageBox(string message)
        {
            return (Base.DialogResult)MessageBox.Show(message);
        }

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        public IPictureBox CreatePictureBox()
        {
            return new PictureBoxVWG();
        }

        ///<summary>
        /// Creates a <see cref="IDateTimePickerMapperStrategy"/>
        ///</summary>
        public IDateTimePickerMapperStrategy CreateDateTimePickerMapperStrategy()
        {
            return new DateTimePickerMapperStrategyVWG();
        }

        ///<summary>
        /// Creates a <see cref="IBOSelectorAndEditor"/>
        ///</summary>
        public IBOGridAndEditorControl CreateGridAndBOEditorControl<TBusinessObject>() where TBusinessObject : class, IBusinessObject
        {
            throw new System.NotImplementedException();
        }

        ///<summary>
        /// Creates a <see cref="IBOSelectorAndEditor"/>
        ///</summary>
        public IBOGridAndEditorControl CreateGridAndBOEditorControl(ClassDef classDef)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// Creates a <see cref="IBOSelectorAndEditor"/>
        ///</summary>
        public IBOGridAndEditorControl CreateGridAndBOEditorControl<TBusinessObject>(IBOEditorControl editorControlPanel) where TBusinessObject : class, IBusinessObject
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// Creates a <see cref="ICollapsiblePanel"/>
        ///</summary>
        public ICollapsiblePanel CreateCollapsiblePanel()
        {
            return new CollapsiblePanelVWG(this); 
        }

        ///<summary>
        /// Creates a <see cref="ICollapsiblePanel"/>
        ///</summary>
        public ICollapsiblePanel CreateCollapsiblePanel(string name)
        {
            CollapsiblePanelVWG collapsiblePanel = new CollapsiblePanelVWG(this) {Text = name, Name = name};
            collapsiblePanel.CollapseButton.Text = name;
            collapsiblePanel.CollapseButton.Name = name;
            return collapsiblePanel;
        }

//
//        public IBOGridAndEditorControl CreateGridAndBOEditorControl(IBOEditorControl boEditorPanel)
//        {
//            throw new NotImplementedException();
//        }


        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        public virtual IDateTimePicker CreateMonthPicker()
        {
            DateTimePickerVWG editor = (DateTimePickerVWG) CreateDateTimePicker();
            editor.Format = (Gizmox.WebGUI.Forms.DateTimePickerFormat) DateTimePickerFormat.Custom;
            editor.CustomFormat = "MMM yyyy";
            return editor;
        }

        ///<summary>
        /// Creates a new numeric up-down control
        ///</summary>
        ///<returns>The created NumericUpDown control</returns>
        public INumericUpDown CreateNumericUpDown()
        {
            INumericUpDown ctl = new NumericUpDownVWG();
            // set the NumericUpDown to the default height of a text box on this machine.
            ctl.Height = GetStandardHeight();
            return ctl;
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
            ctl.Maximum = decimal.MaxValue;
            ctl.Minimum = decimal.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        public virtual ICheckBox CreateCheckBox()
        {
            return CreateCheckBox(false);
        }

        /// <summary>
        /// Creates a CheckBox control with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is checked</param>
        public virtual ICheckBox CreateCheckBox(bool defaultValue)
        {
            CheckBoxVWG cbx = new CheckBoxVWG();
            cbx.Checked = defaultValue;
            cbx.FlatStyle = FlatStyle.Standard;
            cbx.Height = CreateTextBox().Height;
                // set the CheckBoxVWG to the default height of a text box on this machine.
            cbx.CheckAlign = ContentAlignment.MiddleLeft;
            cbx.Width = cbx.Height;
            cbx.BackColor = SystemColors.Control;


            return cbx;
        }

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        public virtual IProgressBar CreateProgressBar()
        {
            ProgressBarVWG bar = new ProgressBarVWG();
            return bar;
        }

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        public virtual ISplitter CreateSplitter()
        {
            SplitterVWG splitter = new SplitterVWG();
            Color newBackColor =
                Color.FromArgb(Math.Min(splitter.BackColor.R - 30, 255), Math.Min(splitter.BackColor.G - 30, 255),
                               Math.Min(splitter.BackColor.B - 30, 255));
            splitter.BackColor = newBackColor;
            return splitter;
        }

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        public virtual ITabPage CreateTabPage(string title)
        {
            TabPageVWG page = new TabPageVWG {Text = title, Name = title};
            return page;
        }

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        public virtual IRadioButton CreateRadioButton(string text)
        {
            RadioButtonVWG rButton = new RadioButtonVWG();
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
            return new GroupBoxVWG();
        }

        /// <summary>
        /// Creates a new GroupBox with the specified text as the title.
        /// </summary>
        public IGroupBox CreateGroupBox(string text)
        {
            IGroupBox groupBox = CreateGroupBox();
            groupBox.Text = text;
            groupBox.Name = text;
            return groupBox;
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
            return new DefaultBOEditorFormVWG(bo, uiDefName, this);
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
            return new DefaultBOEditorFormVWG(bo, uiDefName, this, action);
        }
        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="groupControlCreator">The Creator that will be used to Create the <see cref="IGroupControl"/></param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName,
                                                               GroupControlCreator groupControlCreator)
        {
            return new DefaultBOEditorFormVWG(bo, uiDefName, this, groupControlCreator);
        }

        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in.
        /// </summary>
        /// <param name="bo">   a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid</param>
        /// <returns></returns>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo)
        {
            return new DefaultBOEditorFormVWG(bo, "default", this);
        }

//        public virtual IListView CreateListView()
//        {
        //            return new ListViewVWG();
//        }


        /// <summary>
        /// Creates an editable grid
        /// </summary>
        public virtual IEditableGrid CreateEditableGrid()
        {
            return new EditableGridVWG();
        }

        /// <summary>
        /// Creates an EditableGridControl
        /// </summary>
        public virtual IEditableGridControl CreateEditableGridControl()
        {
            return new EditableGridControlVWG(this);
        }

        /// <summary>
        /// Creates a FileChooser control
        /// </summary>
        public virtual IFileChooser CreateFileChooser()
        {
            return new FileChooserVWG(this);
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
            return new BOColTabControlVWG(this);
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
            return new ControlMapperStrategyVWG();
        }

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        public virtual ITextBoxMapperStrategy CreateTextBoxMapperStrategy()
        {
            return new TextBoxMapperStrategyVWG();
        }

        ///<summary>
        /// Creates a DataGridView
        ///</summary>
        public IDataGridView CreateDataGridView()
        {
            return new DataGridViewVWG();
        }

        /// <summary>
        /// Creates a DataGridViewImageColumn
        /// </summary>
        public virtual IDataGridViewImageColumn CreateDataGridViewImageColumn()
        {
            return new DataGridViewImageColumnVWG(new DataGridViewImageColumn());
        }

        /// <summary>
        /// Creates a DataGridViewCheckBoxColumn
        /// </summary>
        public virtual IDataGridViewCheckBoxColumn CreateDataGridViewCheckBoxColumn()
        {
            return new DataGridViewCheckBoxColumnVWG(new DataGridViewCheckBoxColumn());
        }

        /// <summary>
        /// Creates a DataGridViewComboBoxColumn
        /// </summary>
        public virtual IDataGridViewComboBoxColumn CreateDataGridViewComboBoxColumn()
        {
            return new DataGridViewComboBoxColumnVWG(new DataGridViewComboBoxColumn());
        }

        /// <summary>
        /// Creates a DataGridViewDateTimeColumn
        /// </summary>
        public IDataGridViewDateTimeColumn CreateDataGridViewDateTimeColumn()
        {
            throw new NotImplementedException("No VWG equivalent implemented");
        }

        ///<summary>
        /// Creates a DataGridViewNumericUpDownColumn
        ///</summary>
        ///<returns>A new DataGridViewNumericUpDownColumn</returns>
        public IDataGridViewNumericUpDownColumn CreateDataGridViewNumericUpDownColumn()
        {
            throw new NotImplementedException("No VWG equivalent implemented");
        }

        /// <summary>
        /// Creates a column for a DataGridView for the given type
        /// </summary>
        /// <param name="typeName">The name of the type</param>
        /// <param name="assemblyName">The name of the assembly</param>
        public IDataGridViewColumn CreateDataGridViewColumn(string typeName, string assemblyName)
        {
            Type controlType = null;

            if (String.IsNullOrEmpty(typeName))
            {
                typeName = "DataGridViewTextBoxColumn";
            }

            // VWG VERSION NOT YET IMPLEMENTED
            //if (typeName == "DataGridViewDateTimeColumn" && String.IsNullOrEmpty(assemblyName))
            //{
            //    assemblyName = "Habanero.UI.VWG";
            //}
            //else if (String.IsNullOrEmpty(assemblyName))
            
            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = "Gizmox.WebGUI.Forms";
            }

            TypeLoader.LoadClassType(ref controlType, assemblyName, typeName,
                                         "column", "column definition");

            return CreateDataGridViewColumn(controlType);
        }

        /// <summary>
        /// Creates a column for a DataGridView for the given type
        /// </summary>
        /// <param name="columnType">The type of the column</param>
        public IDataGridViewColumn CreateDataGridViewColumn(Type columnType)
        {
            if (!columnType.IsSubclassOf(typeof(DataGridViewColumn)))
            {
                throw new UnknownTypeNameException(
                    string.Format(
                        "The column type name {0} does not inherit from {1}.", columnType.FullName,
                        typeof(DataGridViewColumn)));
            }

            if (columnType == typeof(DataGridViewCheckBoxColumn)) return CreateDataGridViewCheckBoxColumn();
            if (columnType == typeof(DataGridViewComboBoxColumn)) return CreateDataGridViewComboBoxColumn();
            if (columnType == typeof(DataGridViewImageColumn)) return CreateDataGridViewImageColumn();

            return new DataGridViewColumnVWG((DataGridViewColumn)Activator.CreateInstance(columnType));
        }

        /// <summary>
        /// Creates a TabControl
        /// </summary>
        public virtual ITabControl CreateTabControl()
        {
            return new TabControlVWG();
        }

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines">The number of lines to show in the TextBox</param>
        public virtual ITextBox CreateTextBoxMultiLine(int numLines)
        {
            TextBoxVWG tb = (TextBoxVWG) CreateTextBox();
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
            return new WizardControlVWG(wizardController, this);
        }

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <returns></returns>
        public IWizardForm CreateWizardForm(IWizardController wizardController)
        {
            return new WizardFormVWG(wizardController, this);
        }

//        public IScreen CreateScreen()
//        {
//            throw new NotImplementedException();
//        }

        

        private static int GetStandardHeight()
        {
            return TEXTBOX_HEIGHT;
        }

//        public IDataGridViewColumn CreateDataGridViewTextBoxColumn()
//        {
//            return new DataGridViewTextBoxColumn() as IDataGridViewColumn;
//        }

        #region Collapsible Panel Button Creators

        ///<summary>
        /// Creates a <see cref="IButton"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="IButton"/> </returns>
        public IButton CreateButtonCollapsibleStyle()
        {
            ButtonVWG button = (ButtonVWG)CreateButton();
            ConfigureCollapsibleStyleButton(button);
            return button;
        }

        private static void ConfigureCollapsibleStyleButton(IButton button)
        {
            ButtonVWG buttonVWG = ((ButtonVWG) button);
            buttonVWG.BackgroundImage = CollapsiblePanelResource.headergradient;
            buttonVWG.FlatStyle = FlatStyle.Flat;

        }

        ///<summary>
        /// Creates a <see cref="ILabel"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="ILabel"/> </returns>
        public ILabel CreateLabelPinOffStyle()
        {
            LabelVWG label = (LabelVWG)CreateLabel();
            ConfigurePinOffStyleLabel(label);
            return label;
        }

        ///<summary>
        /// Configures the <see cref="ILabel"/> with the pinoff style
        ///</summary>
        public void ConfigurePinOffStyleLabel(ILabel label)
        {
            LabelVWG labelVWG = ((LabelVWG) label);
            labelVWG.BackgroundImage = CollapsiblePanelResource.pinoff_withcolour;
            labelVWG.FlatStyle = FlatStyle.Flat;
            labelVWG.BackgroundImageLayout = ImageLayout.Center;
            labelVWG.Width = 24;
        }

        ///<summary>
        /// Configures the <see cref="ILabel"/> with the pinon style
        ///</summary>
        public void ConfigurePinOnStyleLabel(ILabel label)
        {
            LabelVWG labelVWG = ((LabelVWG)label);
            labelVWG.BackgroundImage = CollapsiblePanelResource.pinon_withcolour;
            labelVWG.FlatStyle = FlatStyle.Flat;
            labelVWG.BackgroundImageLayout = ImageLayout.Center;
            labelVWG.Width = 24;
        }

        ///<summary>
        /// Craetes an <see cref="ICollapsiblePanelGroupControl"/>
        ///</summary>
        ///<returns></returns>
        public ICollapsiblePanelGroupControl CreateCollapsiblePanelGroupControl()
        {
            return new CollapsiblePanelGroupControlVWG();
        }

        ///<summary>
        /// Creates a <see cref="IGroupBoxGroupControl"/>
        ///</summary>
        ///<returns></returns>
        public IGroupBoxGroupControl CreateGroupBoxGroupControl()
        {
            return new GroupBoxGroupControlVWG(this);
        }

        ///<summary>
        /// Creates an <see cref="IBOComboBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        public IBOComboBoxSelector CreateComboBoxSelector()
        {
            ComboBoxSelectorVWG comboBox = new ComboBoxSelectorVWG();
            comboBox.Height = TEXTBOX_HEIGHT;
            comboBox.AutoCompleteSource = Gizmox.WebGUI.Forms.AutoCompleteSource.ListItems;
            comboBox.AutoCompleteMode = Gizmox.WebGUI.Forms.AutoCompleteMode.SuggestAppend;
            return comboBox;
        }

        ///<summary>
        /// Creates an <see cref="IBOListBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        public IBOListBoxSelector CreateListBoxSelector()
        {
            return new ListBoxSelectorVWG();
        }
        
        ///<summary>
        /// Creates an <see cref="IBOCollapsiblePanelSelector"/>
        ///</summary>
        ///<returns></returns>
        public IBOCollapsiblePanelSelector CreateCollapsiblePanelSelector()
        {
            return new CollapsiblePanelSelectorVWG(this);
        }

        /// <summary>
        /// Creates an <see cref="IMainMenuHabanero"/>
        /// </summary>
        /// <returns></returns>
        public IMainMenuHabanero CreateMainMenu()
        {
            return new MainMenuVWG();
        }

        /// <summary>
        /// Creates an <see cref="IMenuItem"/> with the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>returns the Created MenuItem</returns>
        public IMenuItem CreateMenuItem(string name)
        {
            return new MenuItemVWG(name);
        }

        /// <summary>
        /// Creates an <see cref="IMenuItem"/> with the name.
        /// </summary>
        /// <param name="item">the HabaneroMenu.Item that the IMenuItem is being created for</param>
        /// <returns>returns the Created MenuItem</returns>
        public IMenuItem CreateMenuItem(HabaneroMenu.Item item)
        {
            return new MenuItemVWG(item);
        }

        /// <summary>
        /// Creates an <see cref="IMainMenuHabanero"/> with associated habaneroMenu.
        /// </summary>
        /// <param name="habaneroMenu">the HabaneroMenu that the IMainMenuHabanero is being created for</param>
        /// <returns>returns the Created IMainMenuHabanero</returns>
        public IMainMenuHabanero CreateMainMenu(HabaneroMenu habaneroMenu)
        {
            return new MainMenuVWG(habaneroMenu);
        }

        /// <summary>
        /// Creates an <see cref="ISplitContainer"/>
        /// </summary>
        /// <returns>returns the created split container</returns>
        public ISplitContainer CreateSplitContainer()
        {
            return new SplitContainerVWG();
        }

        #endregion
    }
 

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a NumericUpDown
    /// depending on the environment
    /// </summary>
    internal class NumericUpDownMapperStrategyVWG : INumericUpDownMapperStrategy
    {
        #region INumericUpDownMapperStrategy Members

        /// <summary>
        /// Handles the value changed event suitably for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the NumericUpDown</param>
        public void ValueChanged(NumericUpDownMapper mapper)
        {
        }

        #endregion
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    internal class LookupComboBoxKeyPressMapperStrategyVWG : ILookupComboBoxMapperStrategy
    {
        #region ILookupComboBoxMapperStrategy Members

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void RemoveCurrentHandlers(ILookupComboBoxMapper mapper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(ILookupComboBoxMapper mapper)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    internal class LookupComboBoxMapperStrategyVWG : ILookupComboBoxMapperStrategy
    {
        #region ILookupComboBoxMapperStrategy Members

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void RemoveCurrentHandlers(ILookupComboBoxMapper mapper)
        {
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(ILookupComboBoxMapper mapper)
        {
        }

        #endregion

        public void AddItemSelectedEventHandler(ILookupComboBoxMapper mapper)
        {
        }
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    internal class ListComboBoxMapperStrategyVWG : IListComboBoxMapperStrategy
    {
        #region IListComboBoxMapperStrategy Members

        /// <summary>
        /// Adds an ItemSelected event handler.
        /// For Windows Forms you may want the business object to be updated immediately, however
        /// for a web environment with low bandwidth you may choose to only update when the user saves.
        ///</summary>
        public void AddItemSelectedEventHandler(ListComboBoxMapper mapper)
        {
        }

        #endregion
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a CheckBox
    /// depending on the environment
    /// </summary>
    internal class CheckBoxMapperStrategyVWG : ICheckBoxMapperStrategy
    {
        #region ICheckBoxMapperStrategy Members

        /// <summary>
        /// Adds click event handler
        /// </summary>
        /// <param name="mapper">The checkbox mapper</param>
        public void AddClickEventHandler(CheckBoxMapper mapper)
        {
        }

        #endregion
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a control
    /// depending on the environment
    /// </summary>
    internal class ControlMapperStrategyVWG : IControlMapperStrategy
    {
        #region IControlMapperStrategy Members

        /// <summary>
        /// Adds handlers to events of a current business object property.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        public virtual void AddCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp)
        {
            //Does nothing for gizmox due to overheads of server based events
        }

        /// <summary>
        /// Removes handlers to events of a current business object property.
        /// It is essential that if the AddCurrentBoPropHandlers is implemented then this 
        /// is implemented such that editing a business object that is no longer being shown on the control does not
        /// does not update the value in the control.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        public virtual void RemoveCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp)
        {
            //Does nothing for gizmox due to overheads of server based events
        }

        /// <summary>
        /// Handles the default key press behaviours on a control.
        /// This is typically used to change the handling of the enter key (such as having
        /// the enter key cause focus to move to the next control).
        /// </summary>
        /// <param name="control">The control whose events will be handled</param>
        public virtual void AddKeyPressEventHandler(IControlHabanero control)
        {
            //Does nothing for gizmox due to overheads of server based events
        }

        #endregion
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    internal class TextBoxMapperStrategyVWG : ITextBoxMapperStrategy
    {
        #region ITextBoxMapperStrategy Members

        /// <summary>
        /// Adds key press event handlers that carry out actions like
        /// limiting the input of certain characters, depending on the type of the
        /// property
        /// </summary>
        /// <param name="mapper">The TextBox mapper</param>
        /// <param name="boProp">The property being mapped</param>
        public virtual void AddKeyPressEventHandler(TextBoxMapper mapper, IBOProp boProp)
        {
            //Would require heavy event handling, so unsuitable for WebGUI at the moment
        }

        public void AddUpdateBoPropOnTextChangedHandler(TextBoxMapper mapper, IBOProp boProp)
        {
            //This is not suitable for web.
        }

        #endregion
    }
}
