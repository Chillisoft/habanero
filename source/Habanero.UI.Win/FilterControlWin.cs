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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    // TODO: move this into FilterControl directory like Giz version
    public class FilterControlWin : PanelWin, IFilterControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly IPanel _controlPanel;
        private readonly IPanel _filterButtonPanel;
        private readonly FilterControlManager _filterControlManager;
        private readonly IGroupBox _gbox;
        private IButton _clearButton;
        private IButton _filterButton;
        private FilterModes _filterMode;

        public FilterControlWin(IControlFactory controlFactory)
        {
            Height = 50;
            _controlFactory = controlFactory;
            _gbox = _controlFactory.CreateGroupBox();
            _controlFactory.CreateBorderLayoutManager(this).AddControl(_gbox, BorderLayoutManager.Position.Centre);
            _gbox.Text = "Filter the Grid";

            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(_gbox);
            layoutManager.BorderSize = 20;
            _filterButtonPanel = controlFactory.CreatePanel();
            _filterButtonPanel.Height = 50;
            _filterButtonPanel.Width = 110;
            _filterButtonPanel.Visible = false;
            CreateFilterButtons(_filterButtonPanel);

            layoutManager.AddControl(_filterButtonPanel, BorderLayoutManager.Position.West);

            _controlPanel = controlFactory.CreatePanel();
            _controlPanel.Width = Width;

            layoutManager.AddControl(_controlPanel, BorderLayoutManager.Position.Centre);

            Height = 50;
            _filterControlManager = new FilterControlManager(controlFactory,
                                                             new FlowLayoutManager(_controlPanel, controlFactory));
        }

        #region IFilterControl Members

        public event EventHandler Filter;

        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ITextBox textBox = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);
            return textBox;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the TextBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        /// <param name="filterClauseOperator">Operator To Use For the filter clause</param>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName,
                                               FilterClauseOperator filterClauseOperator)
        {
            return _filterControlManager.AddStringFilterTextBox(labelText, propertyName, filterClauseOperator);
        }

        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public IComboBox AddStringFilterComboBox(string labelText, string propertyName, ICollection options,
                                                 bool strictMatch)
        {
            IComboBox comboBox =
                _filterControlManager.AddStringFilterComboBox(labelText, propertyName, options, strictMatch);
            return comboBox;
        }

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
            //_layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            //DateTimePicker picker =
            //    _filterInputBoxCollection.AddDateFilterDateTimePicker(columnName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            //_layoutManager.AddControl(picker);
            //return picker;
            IDateTimePicker dtPicker = _filterControlManager.AddDateFilterDateTimePicker(propertyName,
                                                                                         filterClauseOperator, nullable,
                                                                                         defaultValue);
            return dtPicker;
        }

        /// <summary>
        ///Applies the filter that has been captured.
        ///This allows an external control e.g. another button click to be used as the event that causes the filter to fire.
        ///Typically used when the filter controls are being set manually
        /// </summary>
        public void ApplyFilter()
        {
            //TODO: this is used by TestEditableGridControl, but should this behaviour be tested in the Win version?
            //   ie. it gets the tests working, but perhaps the tests should be adapted depending on whether its win or giz
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

        public int CountOfFilters
        {
            get { return _filterControlManager.CountOfFilters; }
        }

        public IButton FilterButton
        {
            get { return _filterButton; }
        }

        /// <summary>
        /// Returns the clear button (this is the button that when clicked clears the filter.
        /// </summary>
        public IButton ClearButton
        {
            get { return _clearButton; }
        }

        public FilterModes FilterMode
        {
            get { return _filterMode; }
            set
            {
                _filterMode = value;
                _filterButtonPanel.Visible = (_filterMode == FilterModes.Search);
            }
        }

        public IList FilterControls
        {
            get { return _filterControlManager.FilterControls; }
        }

        public IControlChilli GetChildControl(string propertyName)
        {
            return _filterControlManager.GetChildControl(propertyName);
        }

        public void ClearFilters()
        {
            throw new NotImplementedException("not implemented on win");
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
        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate,
                                                             bool includeEndDate)
        {
            return _filterControlManager.AddDateRangeFilterComboBox(labelText, columnName, includeStartDate,
                                                                    includeEndDate);
        }

        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName,
                                                             List<DateRangeOptions> options, bool includeStartDate,
                                                             bool includeEndDate)
        {
            return
                _filterControlManager.AddDateRangeFilterComboBox(labelText, columnName, options, includeStartDate,
                                                                 includeEndDate);
        }

        #endregion

        private void CreateFilterButtons(IPanel filterButtonPanel)
        {
            const int buttonHeight = 20;
            const int buttonWidth = 45;
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

        private void FireFilterEvent()
        {
            if (Filter != null)
            {
                Filter(this, new EventArgs());
            }
        }
    }
}