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
using Habanero.UI.Base;
using Habanero.UI.Base.ControlInterfaces;
using Habanero.UI.VWG.Grid;
using Habanero.UI.VWG.Grid;
using Habanero.Util.File;
using DateTimePickerFormat=Habanero.UI.Base.DateTimePickerFormat;
using ScrollBars=Gizmox.WebGUI.Forms.ScrollBars;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Creates controls for the Gizmox.WebGUI.Forms UI environment
    /// </summary>
    public class ControlFactoryVWG : IControlFactory
    {
        public const int TEXTBOX_HEIGHT = 20;

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
            return tb;
        }

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        public virtual IComboBox CreateComboBox()
        {
            ComboBoxVWG comboBox = new ComboBoxVWG();
            comboBox.Height = TEXTBOX_HEIGHT;
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
            return new MultiSelectorVWG<T>();
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
                lbl.Text = labelText + " *";
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
        /// Creates a control for the given type and assembly name
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        /// the default type, which is usually TextBox.</returns>
        public virtual IControlHabanero CreateControl(string typeName, string assemblyName)
        {
            Type controlType = null;

            if (String.IsNullOrEmpty(typeName) || String.IsNullOrEmpty(assemblyName))
            {
                controlType = typeof (TextBox);
            }
            else
            {
                TypeLoader.LoadClassType(ref controlType, assemblyName, typeName,
                                         "field", "field definition");
            }

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

        public IStaticDataEditor CreateStaticDataEditor()
        {
            throw new System.NotImplementedException();
        }

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
            TabPageVWG page = new TabPageVWG();
            page.Text = title;
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
        /// <param name="action">Action to be performed when the editing is complete. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid)</param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName,
                                                               PostObjectPersistingDelegate action)
        {
            return new DefaultBOEditorFormVWG(bo, uiDefName, this, action);
        }

        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in.
        /// </summary>
        /// <param name="bo"></param>
        ///   a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid</param>
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

        /// <summary>
        /// Creates a DataGridViewImageColumn
        /// </summary>
        public virtual IDataGridViewImageColumn CreateDataGridViewImageColumn()
        {
            return new GridBaseVWG.DataGridViewImageColumnVWG(new DataGridViewImageColumn());
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

        public IScreen CreateScreen()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new numeric up-down control
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        private INumericUpDown CreateNumericUpDown()
        {
            INumericUpDown ctl = new NumericUpDownVWG();
            ctl.Height = GetStandardHeight();
                // set the NumericUpDown to the default height of a text box on this machine.
            return ctl;
        }

        private static int GetStandardHeight()
        {
            return TEXTBOX_HEIGHT;
        }

        public IDataGridViewColumn CreateDataGridViewTextBoxColumn()
        {
            return new DataGridViewTextBoxColumn() as IDataGridViewColumn;
        }
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
        public void RemoveCurrentHandlers(LookupComboBoxMapper mapper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(LookupComboBoxMapper mapper)
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
        public void RemoveCurrentHandlers(LookupComboBoxMapper mapper)
        {
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(LookupComboBoxMapper mapper)
        {
        }

        #endregion

        public void AddItemSelectedEventHandler(LookupComboBoxMapper mapper)
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

        #endregion
    }
}