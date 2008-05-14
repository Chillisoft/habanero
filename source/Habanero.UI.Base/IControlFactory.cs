using System;
using Habanero.BO;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;

namespace Habanero.UI.Base
{
    public interface IControlFactory
    {
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
        IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons(IControlFactory controlfactory);
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
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        IDateTimePicker CreateMonthPicker();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for monetary use
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        INumericUpDown CreateNumericUpDownMoney();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        INumericUpDown CreateNumericUpDownInteger();

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
    }
}
