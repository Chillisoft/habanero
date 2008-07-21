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
using System.ComponentModel;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base.ControlInterfaces;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base;
using Habanero.UI.Base.Grid;

namespace Habanero.UI.Base
{
    public interface IControlFactory
    {

        /// <summary>
        /// Creates a filter control with the default layout manager
        /// </summary>
        /// <returns></returns>
        IFilterControl CreateFilterControl();

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        /// <returns>Returns the new ComboBox object</returns>
        IComboBox CreateComboBox();

        IListBox CreateListBox();
        IMultiSelector<T> CreateMultiSelector<T>();

        IButton CreateButton();

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <returns>Returns the new Button object</returns>
        IButton CreateButton(string text);

        /// <summary>
        /// Creates a new button with an attached event handler to carry out
        /// further instructions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the event</param>
        /// <returns>Returns the new Button object</returns>
        IButton CreateButton(string text, EventHandler clickHandler);


        ICheckBox CreateCheckBox();
        ILabel CreateLabel();
        IControlChilli CreateControl();
        ILabel CreateLabel(string labelText);
        IDateTimePicker CreateDateTimePicker();
        BorderLayoutManager CreateBorderLayoutManager(IControlChilli control);
        IPanel CreatePanel();
        IReadOnlyGrid CreateReadOnlyGrid();
        IReadOnlyGridControl CreateReadOnlyGridControl();
        IButtonGroupControl CreateButtonGroupControl();
        IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl();

        /// <summary>
        /// Creates a new panel
        /// </summary>
        /// <returns>Returns a new Panel object</returns>
        IPanel CreatePanel(IControlFactory controlFactory);

        /// <summary>
        /// Creates a new panel
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <returns>Returns a new Panel object</returns>
        /// <param name="controlFactory">the factory that this panel will use to create any controls on it</param>
        IPanel CreatePanel(string name,IControlFactory controlFactory);

        ///// <summary>
        ///// Creates a label
        ///// </summary>
        ///// <param name="text">The text to appear in the label</param>
        ///// <param name="isBold">Whether the text appears in bold lettering</param>
        ///// <returns>Returns the new Label object</returns>
        ILabel CreateLabel(string labelText, bool isBold);

        /// <summary>
        /// Creates a new PasswordTextBox that masks the letters as the user
        /// types them
        /// </summary>
        /// <returns>Returns the new PasswordTextBox object</returns>
        ITextBox CreatePasswordTextBox();

        IToolTip CreateToolTip();

        /// <summary>
        /// Creates a new empty TextBox
        /// </summary>
        /// <returns>Returns the new TextBox object</returns>
        ITextBox CreateTextBox();

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        /// <param name="name">The name of the view</param>
        /// <returns>Returns a new TreeView object</returns>
        ITreeView CreateTreeView(string name);

        /// <summary>
        /// Creates a control for the given type and assembly name.
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        ///          the default type, which is usually TextBox.
        /// </returns>
        IControlChilli CreateControl(String typeName, String assemblyName);

        /// <summary>
        /// Creates a new control of the type specified.
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of Control</param>
        /// <returns>Returns a new object of the type requested</returns>
        IControlChilli CreateControl(Type controlType);

        /// <summary>
        /// Creates a new DateTimePicker with a specified date
        /// </summary>
        /// <param name="defaultDate">The initial date value</param>
        /// <returns>Returns a new DateTimePicker object</returns>
        IDateTimePicker CreateDateTimePicker(DateTime defaultDate);

        /// <summary>
        /// Creates a new DateRangeComboBox
        /// </summary>
        /// <returns>Returns a new DateRangeComboBox object</returns>
        IDateRangeComboBox CreateDateRangeComboBox();

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        IDateTimePicker CreateMonthPicker();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        INumericUpDown CreateNumericUpDownInteger();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for Currency use
        /// </summary>
        /// <returns></returns>
        INumericUpDown CreateNumericUpDownCurrency();

        /// <summary>
        /// Creates a new CheckBox with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is ticked</param>
        /// <returns>Returns a new CheckBox object</returns>
        ICheckBox CreateCheckBox(bool defaultValue);

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        /// <returns>Returns a new ProgressBar object</returns>
        IProgressBar CreateProgressBar();

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        /// <returns>Returns a new Splitter object</returns>
        ISplitter CreateSplitter();

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        /// <returns>Returns a new TabPage object</returns>
        ITabPage CreateTabPage(string title);

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        /// <returns>Returns a new RadioButton object</returns>
        IRadioButton CreateRadioButton(string text);

        /// <summary>
        /// Creates a new GroupBox
        /// </summary>
        /// <returns>Returns the new GroupBox</returns>
        IGroupBox CreateGroupBox();

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns a DefaultBOEditorForm object</returns>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName);

        ITabControl CreateTabControl();

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines"></param>
        ITextBox CreateTextBoxMultiLine(int numLines);

        /// <summary>
        /// Creates a control that can be places on a form or a panel to to implement a wizard user interface.
        /// The wizard control will have a next and previous button and a panel to place the wizard step on.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <returns></returns>
        IWizardControl CreateWizardControl(IWizardController wizardController);
        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in.
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="uiDefName"></param>
        /// <param name="action">Action to be performed when the editing is complete. Typically used if you want to update
        ///   a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid</param>
        /// <returns></returns>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName, PostObjectPersistingDelegate action);

        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in.
        /// </summary>
        /// <param name="bo"></param>
        ///   a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid</param>
        /// <returns></returns>
        IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo);

        IListView CreateListView();


        IEditableGrid CreateEditableGrid();
        IEditableGridControl CreateEditableGridControl();
        IFileChooser CreateFileChooser();
        IBOColTabControl CreateBOColTabControl();
        /// <summary>
        /// returns a control mapper strategy for the management of how
        /// business objects properties and their related controls update each other.
        /// E.g. A windows strategy might be to update the control value whenever the property 
        /// is updated.
        /// An internet strategy might be to update the control value only when the business object
        /// is loaded.
        /// </summary>
        /// <returns></returns>
        IControlMapperStrategy CreateControlMapperStrategy();

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        /// <returns>Returns a strategy</returns>
        ITextBoxMapperStrategy CreateTextBoxMapperStrategy();

        /// <summary>
        /// Creates a DataGridViewImageColumn for the appropriate userinterface framework
        /// </summary>
        /// <returns></returns>
        IDataGridViewImageColumn CreateDataGridViewImageColumn();
        /// <summary>
        /// Creates a DataGridViewCheckBoxColumn for the appropriate userinterface framework
        /// </summary>
        /// <returns></returns>
        IDataGridViewCheckBoxColumn CreateDataGridViewCheckBoxColumn();
        /// <summary>
        /// Creates a DataGridViewComboBoxColumn for the appropriate userinterface framework
        /// </summary>
        /// <returns></returns>
        IDataGridViewComboBoxColumn CreateDataGridViewComboBoxColumn();
        /// <summary>
        /// Constructor that provides a specific list of optionsToDisplay to display
        /// </summary>
        /// <param name="optionsToDisplay">A list of date range optionsToDisplay</param>
        IDateRangeComboBox CreateDateRangeComboBox(List<DateRangeOptions> optionsToDisplay);

        /// <summary>
        /// Constructor that provides a specific ErrorProvider. 
        /// </summary>
        /// <returns></returns>
        IErrorProvider CreateErrorProvider();

        IFormChilli CreateForm();
        ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy();
        IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy();
        ILookupComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy();
        ILookupComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy();
        INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy();
        IEditableGridButtonsControl CreateEditableGridButtonsControl();
    }

    public interface IScreen
    {
    }

    /// <summary>
    /// Provides a set of strategies that can be applied to a control
    /// depending on the environment
    /// </summary>
    public interface IControlMapperStrategy
    {
        /// <summary>
        /// Provides an interface for Adding handlers to updated events of current business object
        /// property. This provides the ability to implement various strategies for updating the 
        /// control value based on changes in the business object.
        /// </summary>
        /// <param name="mapper">The business object mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        void AddCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp);
       
        /// <summary>
        /// Provides an interface for Removing handlers to updated events of current business object
        /// properties. It is essential that if the AddCurrentBoPropHandlers is implemented then this 
        /// is implemented such that a editing a business object that is no longer being shown on the control does not
        /// does not update the value in the control.
        /// </summary>
        /// <param name="mapper">The business object mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        void RemoveCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp);
        
        /// <summary>
        /// Provides an interface for handling the default key press behaviours on a control.
        /// This is typically used to change the handling of the enter key. I.e. A common behavioural
        /// requirement is to have the enter key move to the next control.
        /// </summary>
        /// <param name="control">The control whose events will be handled</param>
        void AddKeyPressEventHandler(IControlChilli control);
    }

    /// <summary>
    /// Provides a set of strategies that can be applied to a textbox
    /// depending on the environment
    /// </summary>
    public interface ITextBoxMapperStrategy
    {
        /// <summary>
        /// Adds key press event handlers that carry out actions like
        /// limiting the characters input, depending on the type of the
        /// property
        /// </summary>
        /// <param name="mapper">The textbox mapper</param>
        /// <param name="boProp">The property being mapped</param>
         void AddKeyPressEventHandler(TextBoxMapper mapper, IBOProp boProp);
    }

    /// <summary>
    /// Provides a set of strategies that can be applied to a checkbox
    /// depending on the environment
    /// </summary>
    public interface ICheckBoxMapperStrategy
    {
        /// <summary>
        /// Adds click event handler.
        /// <param name="mapper">The checkbox mapper</param>
        /// </summary>
        void AddClickEventHandler(CheckBoxMapper mapper);
    }

    public interface IListComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds Item selected event handler. For Windows we want the Business to be updated immediately, however
        /// for Web environment with low bandwidth we may choose to only update when the user saves.
        ///</summary>
        void AddItemSelectedEventHandler(ListComboBoxMapper mapper);
    }

    public interface ILookupComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds Item selected event handler. For Windows we want the Business to be updated immediately, however
        /// for Web environment with low bandwidth we may choose to only update when the user saves.
        ///</summary>
        void RemoveCurrentHandlers(LookupComboBoxMapper mapper);
        void AddHandlers(LookupComboBoxMapper mapper);
    }


    public interface INumericUpDownMapperStrategy
    {
        void ValueChanged(NumericUpDownMapper mapper);
    }
}
