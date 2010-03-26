// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    ///<summary>
    /// This delegate provides a signature for a method to be called when a Dialog completes.
    /// It provides a parameter for a reference to the actal dialog that completed, and 
    /// the <see cref="DialogResult"/> of the dialog.
    ///</summary>
    ///<param name="sender">A reference to the actual dialog that was completed, resulting in this delegate being called.
    /// This may be null if the particular Dialog implementation does not allow references to the Dialog type. eg. MessageBox in windows</param>
    ///<param name="dialogResult">The <see cref="DialogResult"/> of the dialog when it was completed.</param>
    public delegate void DialogCompletionDelegate(object sender, DialogResult dialogResult);

    /// <summary>
    /// Creates controls for a specific UI environment.
    /// The control Factory provides a specific piece of functionality fundamental to the 
    /// ability of Habanero to swap between Windows, Web and WPF. If the control factory is 
    /// used for creating all controls in the application, then moving the application from windows to web
    /// or vice versa is trivial. The control factory also provides a simple and easy way to 
    /// style an application: swap out the control factory and create 
    /// controls with any image, etc. you want.
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
        [Obsolete("Replaced by IBOGridAndEditorControl: Brett 03 Mar 2009")]
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>() where T : class, IBusinessObject, new();

        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete("Replaced by IBOGridAndEditorControl: Brett 03 Mar 2009")]
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>(string uiDefName) where T : class, IBusinessObject, new();


        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete("Replaced by IBOGridAndEditorControl: Brett 03 Mar 2009")]
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl)
            where T : class, IBusinessObject, new();


        /// <summary>
        /// Creates a GridWithPanelControl
        /// </summary>
        [Obsolete("Replaced by IBOGridAndEditorControl: Brett 03 Mar 2009")]
        IGridWithPanelControl<T> CreateGridWithPanelControl<T>(IBusinessObjectControl businessObjectControl,
                                                               string uiDefName) where T : class, IBusinessObject, new();

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
        ITreeView CreateTreeView();

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
        /// Creates a new GroupBox with the specified text as the title.
        /// </summary>
        IGroupBox CreateGroupBox(string text);

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
        /// <param name="action">Action to be performed when the editing is completed or cancelled. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid)</param>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName, PostObjectEditDelegate action);

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="groupControlCreator">The Creator that will be used to Create the <see cref="IGroupControl"/></param>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName,
                                                GroupControlCreator groupControlCreator);

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

        ///<summary>
        /// Creates a DataGridView
        ///</summary>
        IDataGridView CreateDataGridView();

        /// <summary>
        /// Creates a DataGridViewImageColumn
        /// </summary>
        IDataGridViewImageColumn CreateDataGridViewImageColumn();

        /// <summary>
        /// Creates a DataGridViewCheckBoxColumn
        /// </summary>
        IDataGridViewCheckBoxColumn CreateDataGridViewCheckBoxColumn();

        /// <summary>
        /// Creates a DataGridViewTextBoxColumn
        /// </summary>
        IDataGridViewColumn CreateDataGridViewTextBoxColumn();

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
        IComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        IComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy();

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
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// Once the user is has responded, the provided delegate is called with an indication of the <see cref="DialogResult"/>.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<param name="dialogCompletionDelegate">A delegate to be called when the MessageBox has been completed.</param>
        ///<returns>The message box result.</returns>
        DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon,
                                    DialogCompletionDelegate dialogCompletionDelegate);

        ///<summary>
        /// Displays a message box with specified text.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<returns>The message box result.</returns>
        DialogResult ShowMessageBox(string message);

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        IPictureBox CreatePictureBox();

        ///<summary>
        /// Creates a <see cref="IDateTimePickerMapperStrategy"/>
        ///</summary>
        IDateTimePickerMapperStrategy CreateDateTimePickerMapperStrategy();

        ///<summary>
        /// Creates a <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        IBOGridAndEditorControl CreateGridAndBOEditorControl<TBusinessObject>()
            where TBusinessObject : class, IBusinessObject;

        ///<summary>
        /// Creates a <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        IBOGridAndEditorControl CreateGridAndBOEditorControl(IClassDef classDef);

        ///<summary>
        /// Creates a <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        IBOGridAndEditorControl CreateGridAndBOEditorControl<TBusinessObject>(IBOEditorControl editorControlPanel)
            where TBusinessObject : class, IBusinessObject;

        ///<summary>
        /// Creates a <see cref="ICollapsiblePanel"/>
        ///</summary>
        ICollapsiblePanel CreateCollapsiblePanel();

        ///<summary>
        /// Creates a <see cref="ICollapsiblePanel"/>
        ///</summary>
        ICollapsiblePanel CreateCollapsiblePanel(string name);


        ///<summary>
        /// Creates a <see cref="IButton"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="IButton"/> </returns>
        IButton CreateButtonCollapsibleStyle();

        ///<summary>
        /// Creates a <see cref="ILabel"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="ILabel"/> </returns>
        ILabel CreateLabelPinOffStyle();

        ///<summary>
        /// Configures the <see cref="ILabel"/> with the pinoff style
        ///</summary>
        void ConfigurePinOffStyleLabel(ILabel label);

        ///<summary>
        /// Configures the <see cref="ILabel"/> with the pinon style
        ///</summary>
        void ConfigurePinOnStyleLabel(ILabel label);

        ///<summary>
        /// Craetes an <see cref="ICollapsiblePanelGroupControl"/>
        ///</summary>
        ///<returns></returns>
        ICollapsiblePanelGroupControl CreateCollapsiblePanelGroupControl();

        ///<summary>
        /// Creates a <see cref="IGroupBoxGroupControl"/>
        ///</summary>
        ///<returns></returns>
        IGroupBoxGroupControl CreateGroupBoxGroupControl();

        ///<summary>
        /// Creates an <see cref="IBOComboBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        IBOComboBoxSelector CreateComboBoxSelector();

        ///<summary>
        /// Creates an <see cref="IBOListBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        IBOListBoxSelector CreateListBoxSelector();

        ///<summary>
        /// Creates an <see cref="IBOCollapsiblePanelSelector"/>
        ///</summary>
        ///<returns></returns>
        IBOCollapsiblePanelSelector CreateCollapsiblePanelSelector();

        /// <summary>
        /// Creates an <see cref="IMainMenuHabanero"/>
        /// </summary>
        /// <returns>returns the Created Main Menu</returns>
        IMainMenuHabanero CreateMainMenu();

        /// <summary>
        /// Creates an <see cref="IMenuItem"/> with the name.
        /// </summary>
        /// <param name="name">The Name of the MenuItem</param>
        /// <returns>returns the Created MenuItem</returns>
        IMenuItem CreateMenuItem(string name);

        /// <summary>
        /// Creates an <see cref="IMenuItem"/> with the name.
        /// </summary>
        /// <param name="item">the HabaneroMenu.Item that the IMenuItem is being created for</param>
        /// <returns>returns the Created MenuItem</returns>
        IMenuItem CreateMenuItem(HabaneroMenu.Item item);

        /// <summary>
        /// Creates an <see cref="IMainMenuHabanero"/> with associated habaneroMenu.
        /// </summary>
        /// <param name="habaneroMenu">the HabaneroMenu that the IMainMenuHabanero is being created for</param>
        /// <returns>returns the Created IMainMenuHabanero</returns>
        IMainMenuHabanero CreateMainMenu(HabaneroMenu habaneroMenu);

        /// <summary>
        /// Creates an <see cref="ISplitContainer"/>
        /// </summary>
        /// <returns>returns the created split container</returns>
        ISplitContainer CreateSplitContainer();

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the Generic Type T.<br/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object (of type T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiDefName">The User Interface definition to use when creating the Control</param>
        /// <returns></returns>
        IBOPanelEditorControl CreateBOEditorControl<T>(string uiDefName) where T : class, IBusinessObject;

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the Generic Type T.<br/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object (of type T).<br/>
        /// The default uiDef is used as the user interface definition for defining which controls are used to view and edit this business object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IBOPanelEditorControl CreateBOEditorControl<T>() where T : class, IBusinessObject;

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the type defined by <paramref name="classDef"/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object.<br/>
        /// </summary>
        /// <param name="classDef"></param>
        /// <param name="uiDefName">The uiDef defined in the classDef that is to be used.</param>
        /// <returns></returns>
        IBOPanelEditorControl CreateBOEditorControl(IClassDef classDef, string uiDefName);

        /// <summary>
        /// Creates a <see cref="IBOPanelEditorControl"/> for the type defined by <paramref name="classDef"/>
        /// This is a simple control that is built to allow the user to view and edit a business object.<br/>
        /// The controls placed on the Panel are defined by the user interface definition defined in the classDef
        /// for the Business Object.<br/>
        /// The default uiDef is used as the user interface definition for defining which controls are used to view and edit this business object
        /// </summary>
        /// <param name="classDef"></param>
        /// <returns></returns>
        IBOPanelEditorControl CreateBOEditorControl(IClassDef classDef);

        /// <summary>
        /// Creates a <see cref="IMainTitleIconControl"/>
        /// </summary>
        /// <returns></returns>
        IMainTitleIconControl CreateMainTitleIconControl();
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

        ///<summary>
        /// Add a handler to the <see cref="ITextBox"/> TextChanged Event that
        /// automatically updates the Business Object with this change.
        /// This is only applicable in Windows not for VWG (Web).
        ///</summary>
        ///<param name="mapper"></param>
        ///<param name="boProp"></param>
        void AddUpdateBoPropOnTextChangedHandler(TextBoxMapper mapper, IBOProp boProp);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    public interface IDateTimePickerMapperStrategy
    {
        /// <summary>
        /// Adds value changed event handlers.
        /// </summary>
        /// <param name="mapper">The DateTime mapper</param>
        void AddUpdateBoPropOnValueChangedHandler(DateTimePickerMapper mapper);
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
    public interface IComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        void AddHandlers(IComboBoxMapper mapper);

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        void RemoveCurrentHandlers(IComboBoxMapper mapper);
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