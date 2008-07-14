#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using DockStyle=Gizmox.WebGUI.Forms.DockStyle;

#endregion

namespace Habanero.UI.WebGUI
{
    public class FilterControlGiz : PanelGiz, IFilterControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly FilterControlManager _filterControlManager;
        private IButton _filterButton;
        private IButton _clearButton;
        public event EventHandler Filter;
        private readonly GroupBoxGiz _gbox;
        private FilterModes _filterMode; //Note all this should move up to windows need to decide buttons etc on win
        private IPanel _controlPanel;
        public FilterControlGiz(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _gbox = new GroupBoxGiz();
            this.Controls.Add(_gbox);
            _gbox.Text = "Filter the Grid";
            _gbox.Dock = DockStyle.Fill;
            _gbox.Height = 50;
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(_gbox);
            layoutManager.BorderSize = 20;
            IPanel filterButtonPanel = controlFactory.CreatePanel();
            filterButtonPanel.Height = 50;
            filterButtonPanel.Width = 110;
            CreateFilterButtons(filterButtonPanel);

            layoutManager.AddControl(filterButtonPanel, BorderLayoutManager.Position.West);

            _controlPanel = controlFactory.CreatePanel();
            _controlPanel.Width = this.Width;

            layoutManager.AddControl(_controlPanel, BorderLayoutManager.Position.Centre);

            FlowLayoutManager controlPanelLayoutManager = new FlowLayoutManager(_controlPanel, controlFactory);

            _filterControlManager = new FilterControlManager(controlFactory, controlPanelLayoutManager);
            this.Height = 50;
        }

        //public int CountOfFilterControls()
        //{
        //    return 
        //}
        private void CreateFilterButtons(IPanel filterButtonPanel)
        {
            int buttonHeight = 20;
            int buttonWidth = 45;
            _filterButton = CreateFilterButton(buttonWidth, buttonHeight);
            _clearButton = CreateClearButton(buttonWidth, buttonHeight);

            FlowLayoutManager layoutManager = new FlowLayoutManager(filterButtonPanel, _controlFactory);
            layoutManager.AddControl(_filterButton);
            layoutManager.AddControl(_clearButton);
        }

        private IButton CreateClearButton(int buttonWidth, int buttonHeight)
        {
            IButton clearButton = _controlFactory.CreateButton();
            clearButton.Width = buttonWidth;
            clearButton.Height = buttonHeight;
            clearButton.Top = _filterButton.Height + 2;
            clearButton.Text = "Clear";
            clearButton.Click += delegate { ClearFilters(); };
            return clearButton;
        }

        private IButton CreateFilterButton(int buttonWidth, int buttonHeight)
        {
            IButton filterButton = _controlFactory.CreateButton();
            filterButton.Width = buttonWidth;
            filterButton.Height = buttonHeight;
            filterButton.Top = 0;
            filterButton.Text = "Filter";
            filterButton.Click += delegate { FireFilterEvent(); };
            return filterButton;
        }

        /// <summary>
        ///Applies the filter that has been captured.
        ///This allows an external control e.g. another button click to be used as the event that causes the filter to fire.
        ///Typically used when the filter controls are being set manually
        /// </summary>
        public void ApplyFilter()
        {
            FireFilterEvent();
        }

        /// <summary>
        /// The header text that will be set above the filter defaults to 'Filter'
        /// </summary>
        public string HeaderText
        {
            get { return _gbox.Text; }
            set { _gbox.Text = value; }
        }

        /// <summary>
        /// The number of controls used for filtering that are on the filter control. <see cref="IFilterControl.FilterControls"/>
        /// </summary>
        public int CountOfFilters
        {
            get { return _filterControlManager.CountOfFilters; }
        }

        private void FireFilterEvent()
        {
            if (Filter != null)
            {
                Filter(this, new EventArgs());
            }
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="labelText">The label to appear before the TextBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ITextBox textBox = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);
            return textBox;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="labelText">The label to appear before the TextBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        /// <param name="filterClauseOperator">Operator To Use For the filter clause</param>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName,
                                               FilterClauseOperator filterClauseOperator)
        {
            ;
            return _filterControlManager.AddStringFilterTextBox(labelText, propertyName, filterClauseOperator);
        }

        /// <summary>
        /// Adds a combo box filter control.
        /// </summary>
        /// <param name="labelText"></param>
        /// <param name="propertyName">The property of the business object being filtered</param>
        /// <param name="options">The collection of items used to fill the combo box.</param>
        /// <param name="strictMatch"></param>
        /// <returns></returns>
        public IComboBox AddStringFilterComboBox(string labelText, string propertyName, ICollection options,
                                                 bool strictMatch)
        {
            IComboBox comboBox =
                _filterControlManager.AddStringFilterComboBox(labelText, propertyName, options, strictMatch);
            comboBox.Height = new TextBox().Height;
            return comboBox;
        }

        /// <summary>
        /// Adds a CheckBox filter that displays only rows whose boolean value
        /// matches the on-off state of the CheckBox. The column of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="labelText">The text label to appear next to the CheckBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        public ICheckBox AddBooleanFilterCheckBox(string labelText, string propertyName, bool defaultValue)
        {
            ICheckBox checkBox = _filterControlManager.AddBooleanFilterCheckBox(labelText, propertyName, defaultValue);
            return checkBox;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.  The standard
        /// DateTimePicker does not support time picking, so any date supplied
        /// or chosen will have its time values set to zero.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date or null.  The filter clause will
        /// set all times to zero.</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue,
                                                           FilterClauseOperator filterClauseOperator, bool nullable)
        {
            IDateTimePicker dtPicker =
                _filterControlManager.AddDateFilterDateTimePicker(propertyName, filterClauseOperator, nullable,
                                                                  defaultValue);
            return dtPicker;
        }

        /// <summary>
        /// Returns the filter clause as a composite of all the specific
        /// clauses in each filter control in the set
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        public IFilterClause GetFilterClause()
        {
            //if (FilterMode == FilterModes.Search)
            //{
            //    return _filterControlManager.GetFilterClause();
            //}
            return _filterControlManager.GetFilterClause();
        }

        /// <summary>
        /// Returns the filter button (this is the button that when clicked applies the filter.
        /// </summary>
        public IButton FilterButton
        {
            get { return _filterButton; }
        }

        /// <summary>
        /// gets and sets the FilterMode <see cref="FilterModes"/>
        /// </summary>
        public FilterModes FilterMode
        {
            get { return _filterMode; }
            set
            {
                _filterMode = value;
                if (_filterMode == FilterModes.Filter)
                {
                    _filterButton.Text = "Filter";
                    _gbox.Text = "Filter the Grid";
                }
                else
                {
                    _filterButton.Text = "Search";
                    _gbox.Text = "Search the Grid";
                }
            }
        }

        /// <summary>
        /// returns a collection of the controls used for filtering i.e. the textbox, combobox. This list excludes the labels etc.
        /// </summary>
        public IList FilterControls
        {
            get { return _filterControlManager.FilterControls; }
        }

        public IControlChilli GetChildControl(string propertyName)
        {
            return this._filterControlManager.GetChildControl(propertyName);
        }

        /// <summary>
        /// Returns the clear button (this is the button that when clicked clears the filter.
        /// </summary>
        public IButton ClearButton
        {
            get { return _clearButton; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        /// <summary>
        /// Clears all the values from the filter and calls <see cref="IFilterControl.ApplyFilter"/>
        /// </summary>
        public void ClearFilters()
        {
            _filterControlManager.ClearFilters();
            FireFilterEvent();
        }

        /// <summary>
        /// returns the layout manager used to lay the controls out on the filter control panel.
        /// The default layout manager is the FlowLayoutManager.
        /// </summary>
        public LayoutManager LayoutManager
        {
            get { return _filterControlManager.LayoutManager; }
            set { _filterControlManager.LayoutManager = value; }
        }

        public IPanel FilterPanel
        {
            get { return _controlPanel; }
        }

        /// <summary>
        /// Adds a ComboBox filter from which the user can choose an option, so that
        /// only rows with that option in the specified column will be shown
        /// </summary>
        /// <param name="labelText">The label to appear before the ComboBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="includeStartDate">Includes all dates that match the start
        /// date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end
        /// date exactly</param>
        /// <returns>Returns the new ComboBox added</returns>
        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate, bool includeEndDate)
        {
            return _filterControlManager.AddDateRangeFilterComboBox(labelText, columnName, includeStartDate, includeEndDate);

        }

        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, List<DateRangeOptions> options, bool includeStartDate, bool includeEndDate)
        {
            return
                _filterControlManager.AddDateRangeFilterComboBox(labelText, columnName, options, includeStartDate,
                                                                 includeEndDate);
        }
    }
}