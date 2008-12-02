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
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Creates controls for a specific UI environment
    /// </summary>
    public interface IControlFactory
    {
        /// <summary>
        /// Creates a filter control with the default layout manager
        /// </summary>
        IFilterControl CreateFilterControl();

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        IComboBox CreateComboBox();

        /// <summary>
        /// Creates a ListBox control
        /// </summary>
        /// <returns></returns>
        IListBox CreateListBox();

        /// <summary>
        /// Creates a multi-selector control
        /// </summary>
        /// <typeparam name="T">The business object type being managed in the control</typeparam>
        IMultiSelector<T> CreateMultiSelector<T>();

        /// <summary>
        /// Creates a button control
        /// </summary>
        IButton CreateButton();

        /// <summary>
        /// Creates a button control
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        IButton CreateButton(string text);

        /// <summary>
        /// Creates a button control with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        IButton CreateButton(string text, EventHandler clickHandler);

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        ICheckBox CreateCheckBox();

        /// <summary>
        /// Creates a CheckBox control with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is checked</param>
        ICheckBox CreateCheckBox(bool defaultValue);

        /// <summary>
        /// Creates a label without text
        /// </summary>
        ILabel CreateLabel();

        /// <summary>
        /// Creates a label with specified text
        /// </summary>
        ILabel CreateLabel(string labelText);

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="labelText">The text to appear in the label</param>
        /// <param name="isBold">Whether the text appears in bold font</param>
        ILabel CreateLabel(string labelText, bool isBold);

        /// <summary>
        /// Creates a BorderLayoutManager to place controls on the given parent control
        /// </summary>
        /// <param name="control">The parent control on which to managed the layout</param>
        BorderLayoutManager CreateBorderLayoutManager(IControlHabanero control);

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        IPanel CreatePanel();

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        IPanel CreatePanel(IControlFactory controlFactory);

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        IPanel CreatePanel(string name, IControlFactory controlFactory);

        /// <summary>
        /// Creates a read-only Grid
        /// </summary>
        IReadOnlyGrid CreateReadOnlyGrid();

        /// <summary>
        /// Creates a ReadOnlyGridControl
        /// </summary>
        IReadOnlyGridControl CreateReadOnlyGridControl();

        /// <summary>
        /// Creates a buttons control for a <see cref="IReadOnlyGridControl"/>
        /// </summary>
        IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl();

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>() where T : class, IBusinessObject, new();

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>(string uiDefName) where T : class, IBusinessObject, new();

 
        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl) where T : class, IBusinessObject, new();


                /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl, string uiDefName) where T : class, IBusinessObject, new();

        /// <summary>
        /// Creates a control to manage a group of buttons that display next to each other
        /// </summary>
        IButtonGroupControl CreateButtonGroupControl();

        /// <summary>
        /// Creates a ToolTip
        /// </summary>
        IToolTip CreateToolTip();

        /// <summary>
        /// Creates a TextBox control
        /// </summary>
        ITextBox CreateTextBox();

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines">The number of lines to show in the TextBox</param>
        ITextBox CreateTextBoxMultiLine(int numLines);

        /// <summary>
        /// Creates a new PasswordTextBox that masks the letters as the user
        /// types them
        /// </summary>
        /// <returns>Returns the new PasswordTextBox object</returns>
        ITextBox CreatePasswordTextBox();

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        /// <param name="name">The name of the TreeView</param>
        ITreeView CreateTreeView(string name);

        ///<summary>
        /// Creates a new TreeNode for a TreeView control.
        ///</summary>
        ///<param name="nodeName">The name for the node</param>
        ///<returns>The newly created TreeNode object.</returns>
        ITreeNode CreateTreeNode(string nodeName);

        /// <summary>
        /// Creates a generic control
        /// </summary>
        IControlHabanero CreateControl();

        /// <summary>
        /// Creates a user control
        /// </summary>
        IUserControlHabanero CreateUserControl();

        /// <summary>
        /// Creates a user control with the specified name.
        /// </summary>
        IUserControlHabanero CreateUserControl(string name);

        /// <summary>
        /// Creates a control for the given type and assembly name
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        /// the default type, which is usually TextBox.</returns>
        IControlHabanero CreateControl(String typeName, String assemblyName);

        /// <summary>
        /// Creates a new control of the type specified
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of <see cref="IControlHabanero"/></param>
        IControlHabanero CreateControl(Type controlType);

        /// <summary>
        /// Creates a DateTimePicker
        /// </summary>
        IDateTimePicker CreateDateTimePicker();

        /// <summary>
        /// Creates a new DateTimePicker with a specified date
        /// </summary>
        /// <param name="defaultDate">The initial date value</param>
        IDateTimePicker CreateDateTimePicker(DateTime defaultDate);

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        IDateTimePicker CreateMonthPicker();

        /// <summary>
        /// Creates a new DateRangeComboBox control
        /// </summary>
        IDateRangeComboBox CreateDateRangeComboBox();

        /// <summary>
        /// Creates DateRangeComboBox control with a specific set of date range
        /// options to display
        /// </summary>
        /// <param name="optionsToDisplay">A list of date range options to display</param>
        IDateRangeComboBox CreateDateRangeComboBox(List<DateRangeOptions> optionsToDisplay);

        ///<summary>
        /// Creates a new numeric up-down control
        ///</summary>
        ///<returns>The created NumericUpDown control</returns>
        INumericUpDown CreateNumericUpDown();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        INumericUpDown CreateNumericUpDownInteger();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for currency use
        /// </summary>
        INumericUpDown CreateNumericUpDownCurrency();

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        IProgressBar CreateProgressBar();

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        ISplitter CreateSplitter();

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        IRadioButton CreateRadioButton(string text);

        /// <summary>
        /// Creates a new GroupBox
        /// </summary>
        IGroupBox CreateGroupBox();

        /// <summary>
        /// Creates a TabControl
        /// </summary>
        ITabControl CreateTabControl();

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        ITabPage CreateTabPage(string title);

        /// <summary>
        /// Creates a control that can be placed on a form or a panel to implement a wizard user interface.
        /// The wizard control will have a next and previous button and a panel to place the wizard step on.
        /// </summary>
        /// <param name="wizardController">The controller that manages the wizard process</param>
        IWizardControl CreateWizardControl(IWizardController wizardController);

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <returns></returns>
        IWizardForm CreateWizardForm(IWizardController wizardController);

        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in.
        /// </summary>
        /// <param name="bo"></param>
        ///   a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid</param>
        /// <returns></returns>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo);

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName);

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="action">Action to be performed when the editing is complete. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid)</param>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName, PostObjectPersistingDelegate action);

        ///// <summary>
        ///// Creates a ListView control
        ///// </summary>
        //IListView CreateListView(); //TODO: Port

        /// <summary>
        /// Creates an editable grid
        /// </summary>
        IEditableGrid CreateEditableGrid();

        /// <summary>
        /// Creates an EditableGridControl
        /// </summary>
        IEditableGridControl CreateEditableGridControl();

        /// <summary>
        /// Creates an buttons control for an <see cref="IEditableGridControl"/>
        /// </summary>
        IEditableGridButtonsControl CreateEditableGridButtonsControl();

        /// <summary>
        /// Creates a FileChooser control
        /// </summary>
        IFileChooser CreateFileChooser();

        /// <summary>
        /// Displays a business object collection in a tab control, with one
        /// business object per tab.  Each tab holds a business control, provided
        /// by the developer, that refreshes to display the business object for
        /// the current tab.
        /// <br/>
        /// This control is suitable for a business object collection with a limited
        /// number of objects.
        /// </summary>
        IBOColTabControl CreateBOColTabControl();

        /// <summary>
        /// Creates a DataGridViewImageColumn
        /// </summary>
        IDataGridViewImageColumn CreateDataGridViewImageColumn();

        /// <summary>
        /// Creates a DataGridViewCheckBoxColumn
        /// </summary>
        IDataGridViewCheckBoxColumn CreateDataGridViewCheckBoxColumn();

        /// <summary>
        /// Creates a DataGridViewComboBoxColumn
        /// </summary>
        IDataGridViewComboBoxColumn CreateDataGridViewComboBoxColumn();

        /// <summary>
        /// Creates a DataGridViewDateTimeColumn
        /// </summary>
        IDataGridViewDateTimeColumn CreateDataGridViewDateTimeColumn();

        ///<summary>
        /// Creates a DataGridViewNumericUpDownColumn
        ///</summary>
        ///<returns>A new DataGridViewNumericUpDownColumn</returns>
        IDataGridViewNumericUpDownColumn CreateDataGridViewNumericUpDownColumn();

        /// <summary>
        /// Creates a column for a DataGridView for the given type
        /// </summary>
        /// <param name="typeName">The name of the type</param>
        /// <param name="assemblyName">The name of the assembly</param>
        IDataGridViewColumn CreateDataGridViewColumn(string typeName, string assemblyName);

        /// <summary>
        /// Creates a column for a DataGridView for the given type
        /// </summary>
        /// <param name="columnType">The type of the column</param>
        IDataGridViewColumn CreateDataGridViewColumn(Type columnType);

        /// <summary>
        /// Creates an ErrorProvider
        /// </summary>
        IErrorProvider CreateErrorProvider();

        /// <summary>
        /// Creates a Form control
        /// </summary>
        IFormHabanero CreateForm();

        /// <summary>
        /// Creates an OKCancelDialog
        /// </summary>
        IOKCancelDialogFactory CreateOKCancelDialogFactory();

        /// <summary>
        /// Creates a control mapper strategy for the management of how
        /// business object properties and their related controls update each other.
        /// For example, a windows strategy might be to update the control value whenever the property 
        /// is updated, whereas an internet strategy might be to update the control value only
        /// when the business object is loaded.
        /// </summary>
        IControlMapperStrategy CreateControlMapperStrategy();

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        ITextBoxMapperStrategy CreateTextBoxMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a CheckBox for the environment
        /// </summary>
        ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a ComboBox for the environment
        /// </summary>
        IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a lookup ComboBox for the environment
        /// </summary>
        ILookupComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        ILookupComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a NumericUpDown for the environment
        /// </summary>
        INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy();

        /// <summary>
        /// Creates a static data editor
        /// </summary>
        IStaticDataEditor CreateStaticDataEditor();
        
        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<returns>The message box result.</returns>
        DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);


        ///<summary>
        /// Displays a message box with specified text.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<returns>The message box result.</returns>
        Base.DialogResult ShowMessageBox(string message);

        
    }

    /// <summary>
    /// Provides a screen
    /// </summary>
    public interface IScreen
    {
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a control
    /// depending on the environment
    /// </summary>
    public interface IControlMapperStrategy
    {
        /// <summary>
        /// Adds handlers to events of a current business object property.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        void AddCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp);

        /// <summary>
        /// Removes handlers to events of a current business object property.
        /// It is essential that if the AddCurrentBoPropHandlers is implemented then this 
        /// is implemented such that editing a business object that is no longer being shown on the control does not
        /// does not update the value in the control.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        void RemoveCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp);

        /// <summary>
        /// Handles the default key press behaviours on a control.
        /// This is typically used to change the handling of the enter key (such as having
        /// the enter key cause focus to move to the next control).
        /// </summary>
        /// <param name="control">The control whose events will be handled</param>
        void AddKeyPressEventHandler(IControlHabanero control);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    public interface ITextBoxMapperStrategy
    {
        /// <summary>
        /// Adds key press event handlers that carry out actions like
        /// limiting the input of certain characters, depending on the type of the
        /// property
        /// </summary>
        /// <param name="mapper">The TextBox mapper</param>
        /// <param name="boProp">The property being mapped</param>
        void AddKeyPressEventHandler(TextBoxMapper mapper, IBOProp boProp);

        void AddUpdateBoPropOnTextChangedHandler(TextBoxMapper mapper, IBOProp boProp);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a CheckBox
    /// depending on the environment
    /// </summary>
    public interface ICheckBoxMapperStrategy
    {
        /// <summary>
        /// Adds click event handler
        /// </summary>
        /// <param name="mapper">The checkbox mapper</param>
        void AddClickEventHandler(CheckBoxMapper mapper);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a list ComboBox
    /// depending on the environment
    /// </summary>
    public interface IListComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds an ItemSelected event handler.
        /// For Windows Forms you may want the business object to be updated immediately, however
        /// for a web environment with low bandwidth you may choose to only update when the user saves.
        ///</summary>
        void AddItemSelectedEventHandler(ListComboBoxMapper mapper);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    public interface ILookupComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        void AddHandlers(LookupComboBoxMapper mapper);

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        void RemoveCurrentHandlers(LookupComboBoxMapper mapper);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a NumericUpDown
    /// depending on the environment
    /// </summary>
    public interface INumericUpDownMapperStrategy
    {
        /// <summary>
        /// Handles the value changed event suitably for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the NumericUpDown</param>
        void ValueChanged(NumericUpDownMapper mapper);
    }
}