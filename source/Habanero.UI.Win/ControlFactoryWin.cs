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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.ControlInterfaces;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.Grid;
using Habanero.Util.File;
using KeyPressEventArgs=Habanero.UI.Base.ControlInterfaces.KeyPressEventArgs;

namespace Habanero.UI.Win
{
    public class ControlFactoryWin : IControlFactory
    {
        /// <summary>
        /// Creates a filter control with the default layout manager
        /// </summary>
        /// <returns></returns>
        public IFilterControl CreateFilterControl()
        {
            return new FilterControlWin(this);
        }


        /// <summary>
        /// Creates a TextBox with no filtering of characters. 
        /// If the type of the property is known, rather use the overloaded version of this method.
        /// </summary>
        /// <returns>Returns a new ITextBox object.</returns>
        public ITextBox CreateTextBox()
        {
            return new TextBoxWin();
        }


        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        /// <param name="propertyType">Type property being edited.</param>
        /// <returns>Returns a new ITextBox object.</returns>
        public ITextBox CreateTextBox(Type propertyType)
        {
            return new TextBoxWin();
        }

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        /// <param name="name">The name of the view</param>
        /// <returns>Returns a new ITreeView object</returns>
        public ITreeView CreateTreeView(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a control for the given type and assembly name.
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        ///          the default type, which is usually TextBox.
        /// </returns>
        public IControlChilli CreateControl(string typeName, string assemblyName)
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
        /// Creates a new control of the type specified.
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of Control</param>
        /// <returns>Returns a new object of the type requested</returns>
        public IControlChilli CreateControl(Type controlType)
        {
            IControlChilli ctl;
            if (controlType.IsSubclassOf(typeof (Control)))
            {
                if (controlType == typeof (ComboBox)) return CreateComboBox();
                if (controlType == typeof (CheckBox)) return CreateCheckBox();
                if (controlType == typeof (TextBox)) return CreateTextBox();
                if (controlType == typeof (ListBox)) return CreateListBox();
                if (controlType == typeof (DateTimePicker)) return CreateDateTimePicker();

                ctl = (IControlChilli) Activator.CreateInstance(controlType);
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
        /// <returns>Returns a new DateTimePicker object</returns>
        public IDateTimePicker CreateDateTimePicker(DateTime defaultDate)
        {
            DateTimePickerWin dateTimePickerWin = new DateTimePickerWin(this);
            dateTimePickerWin.Value = defaultDate; 
            return dateTimePickerWin;
        }

        /// <summary>
        /// Creates a new DateRangeComboBox
        /// </summary>
        /// <returns>Returns a new DateRangeComboBox object</returns>
        public IDateRangeComboBox CreateDateRangeComboBox()
        {
            return new DateRangeComboBoxWin();
        }

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        public IDateTimePicker CreateMonthPicker()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        public INumericUpDown CreateNumericUpDownInteger()
        {
            NumericUpDownWin ctl = new NumericUpDownWin();
            ctl.DecimalPlaces = 0;
            ctl.Maximum = Int32.MaxValue;
            ctl.Minimum = Int32.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for Currency use
        /// </summary>
        /// <returns></returns>
        public INumericUpDown CreateNumericUpDownCurrency()
        {
            NumericUpDownWin ctl = new NumericUpDownWin();
            ctl.DecimalPlaces = 2;
            ctl.Maximum = Int32.MaxValue;
            ctl.Minimum = Int32.MinValue;
            return ctl;
        }


        /// <summary>
        /// Creates a new CheckBox with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is ticked</param>
        /// <returns>Returns a new CheckBox object</returns>
        public ICheckBox CreateCheckBox(bool defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        /// <returns>Returns a new ProgressBar object</returns>
        public IProgressBar CreateProgressBar()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        /// <returns>Returns a new Splitter object</returns>
        public ISplitter CreateSplitter()
        {
            return new SplitterWin();
        }

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        /// <returns>Returns a new TabPage object</returns>
        public ITabPage CreateTabPage(string title)
        {
            TabPageWin page = new TabPageWin();
            page.Text = title;
            return page;
        }

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        /// <returns>Returns a new RadioButton object</returns>
        public IRadioButton CreateRadioButton(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new GroupBox
        /// </summary>
        /// <returns>Returns the new GroupBox</returns>
        public IGroupBox CreateGroupBox()
        {
            return new GroupBoxWin();
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns a DefaultBOEditorForm object</returns>
        public IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName)
        {
            throw new NotImplementedException();
        }

        public ITabControl CreateTabControl()
        {
            return new TabControlWin();
        }

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines"></param>
        public ITextBox CreateTextBoxMultiLine(int numLines)
        {
            TextBoxWin tb = (TextBoxWin) CreateTextBox();
            tb.Multiline = true;
            tb.AcceptsReturn = true;
            tb.Height = tb.Height*numLines;
            tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            return tb;
        }


        public IWizardControl CreateWizardControl(IWizardController wizardController)
        {
            throw new NotImplementedException();
        }

        public IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string name,
                                                       PostObjectPersistingDelegate action)
        {
            return new DefaultBOEditorFormWin(bo, name, this, action);
        }

        public IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo)
        {
            return new DefaultBOEditorFormWin(bo, "default", this);
        }

        public IListView CreateListView()
        {
            throw new NotImplementedException();
        }

        public IEditableGrid CreateEditableGrid()
        {
            return new EditableGridWin();
        }


        public IEditableGridControl CreateEditableGridControl()
        {
            return new EditableGridControlWin(this);
        }


        public IFileChooser CreateFileChooser()
        {
            return new FileChooserWin(this);
        }

        public IBOColTabControl CreateBOColTabControl()
        {
            return new BOColTabControlWin(this);
        }

        /// <summary>
        /// returns a control mapper strategy for the management of how
        /// business objects properties and their related controls update each other.
        /// E.g. A windows strategy might be to update the control value whenever the property 
        /// is updated.
        /// An internet strategy might be to update the control value only when the business object
        /// is loaded.
        /// </summary>
        /// <returns></returns>
        public IControlMapperStrategy CreateControlMapperStrategy()
        {
            return new ControlMapperStrategyWin();
        }

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        /// <returns>Returns a strategy</returns>
        public ITextBoxMapperStrategy CreateTextBoxMapperStrategy()
        {
            return new TextBoxMapperStrategyWin();
        }

        public IDataGridViewImageColumn CreateDataGridViewImageColumn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a DataGridViewCheckBoxColumn for the appropriate userinterface framework
        /// </summary>
        /// <returns></returns>
        public IDataGridViewCheckBoxColumn CreateDataGridViewCheckBoxColumn()
        {
            return new DataGridViewCheckBoxColumnWin(new DataGridViewCheckBoxColumn());
        }

        public IDataGridViewComboBoxColumn CreateDataGridViewComboBoxColumn()
        {
            return new DataGridViewComboBoxColumnWin(new DataGridViewComboBoxColumn());
        }

        /// <summary>
        /// Constructor that provides a specific list of optionsToDisplay to display
        /// </summary>
        /// <param name="optionsToDisplay">A list of date range optionsToDisplay</param>
        public IDateRangeComboBox CreateDateRangeComboBox(List<DateRangeOptions> optionsToDisplay)
        {
            return new DateRangeComboBoxWin(optionsToDisplay);
        }

        public IErrorProvider CreateErrorProvider()
        {
            return new ErrorProviderWin();
        }

        public IFormChilli CreateForm()
        {
            return new FormWin();
        }

        public ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy()
        {
            return new CheckBoxStrategyWin();
        }

        public IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy()
        {
            return new ListComboBoxMapperStrategyWin();
        }

        public ILookupComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy()
        {
            return new LookupComboBoxDefaultMapperStrategyWin();
        }

        public ILookupComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy()
        {
            return new LookupComboBoxKeyPressMapperStrategyWin();
        }

        public INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy()
        {
            return new NumericUpDownMapperStrategyWin();
        }

        public IEditableGridButtonsControl CreateEditableGridButtonsControl()
        {
            return new EditableGridButtonsControlWin(this);
        }

        public IOKCancelDialogFactory CreateOKCancelDialogFactory()
        {
            return new OKCancelDialogFactoryWin(this);
        }

        public IComboBox CreateComboBox()
        {
            return new ComboBoxWin();
        }

        public IListBox CreateListBox()
        {
            return new ListBoxWin();
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>();
        }

        /// <summary>
        /// Creates a new Button
        /// </summary>
        /// <returns>Returns the new Button object</returns>
        public IButton CreateButton()
        {
            return new ButtonWin();
        }

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <returns>Returns the new Button object</returns>
        public IButton CreateButton(string text)
        {
            IButton button = CreateButton();
            button.Text = text;
            return button;
        }

        /// <summary>
        /// Creates a new button with an attached event handler to carry out
        /// further instructions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the event</param>
        /// <returns>Returns the new Button object</returns>
        public IButton CreateButton(string text, EventHandler clickHandler)
        {
            IButton button = CreateButton(text);
            button.Click += clickHandler;
            return button;
        }

        public ICheckBox CreateCheckBox()
        {
            return new CheckBoxWin();
        }

        public ILabel CreateLabel()
        {
            ILabel label = new LabelWin();
            label.TabStop = false;
            return label;
        }

        public ILabel CreateLabel(string labelText)
        {
            ILabel label = CreateLabel();
            label.Text = labelText;
            return label;
        }

        public ILabel CreateLabel(string labelText, bool isBold)
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

        public IDateTimePicker CreateDateTimePicker()
        {
            return new DateTimePickerWin(this);
        }

        public BorderLayoutManager CreateBorderLayoutManager(IControlChilli control)
        {
            return new BorderLayoutManagerWin(control, this);
        }

        public IPanel CreatePanel()
        {
            return new PanelWin();
        }

        public IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridWin();
        }

        public IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return new ReadOnlyGridControlWin(this);
        }

        public IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlWin(this);
        }

        public IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            return new ReadOnlyGridButtonsControlWin(this);
        }

        public IPanel CreatePanel(IControlFactory controlFactory)
        {
            return new PanelWin();
        }

        /// <summary>
        /// Creates a new panel
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <returns>Returns a new Panel object</returns>
        public IPanel CreatePanel(string name, IControlFactory controlFactory)
        {
            throw new NotImplementedException();
        }

        public ITabPage createTabPage(string name)
        {
            throw new System.NotImplementedException();
        }


        public ITextBox CreatePasswordTextBox()
        {
            ITextBox tb = CreateTextBox();
            tb.PasswordChar = '*';
            return tb;
        }

        public IToolTip CreateToolTip()
        {
            return new ToolTipWin();
        }

        public IControlChilli CreateControl()
        {
            return new ControlWin();
        }
    }
}

