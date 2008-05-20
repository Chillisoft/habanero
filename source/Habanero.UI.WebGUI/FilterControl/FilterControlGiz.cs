#region Using

using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
#endregion

namespace Habanero.UI.WebGUI
{
    public class FilterControlGiz : Panel, IFilterControl
    {
        private readonly FilterControlManager _filterControlManager;
        private IButton _filterButton;
        private IButton _clearButton;
        public event EventHandler Filter;
        private GroupBoxGiz _gbox;

        public FilterControlGiz(IControlFactory controlFactory)
        {
            _gbox = new GroupBoxGiz();
            this.Controls.Add(_gbox);
            _gbox.Text = "Filter the Grid";
            _gbox.Dock = DockStyle.Fill;
            _gbox.Height = 50;
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(_gbox, controlFactory);
            CreateFilterButtons(flowLayoutManager);
            flowLayoutManager.BorderSize = 20;
            _filterControlManager = new FilterControlManager(controlFactory, flowLayoutManager);
            this.Height = 50;
        }

        private void CreateFilterButtons(LayoutManager flowLayoutManager)
        {
            int buttonHeight = 20;
            int buttonWidth = 45;
            _filterButton = CreateFilterButton(buttonWidth, buttonHeight);

            _clearButton = CreateClearButton(buttonWidth, buttonHeight);

            flowLayoutManager.AddControl(_filterButton);
            flowLayoutManager.AddControl(_clearButton);
        }

        private ButtonGiz CreateClearButton(int buttonWidth, int buttonHeight)
        {
            ButtonGiz clearButton = new ButtonGiz();
            clearButton.Width = buttonWidth;
            clearButton.Height = buttonHeight;
            clearButton.Top = _filterButton.Height + 2;
            clearButton.Text = "Clear";
            clearButton.Click += delegate { ClearFilters(); };
            return clearButton;
        }

        private ButtonGiz CreateFilterButton(int buttonWidth, int buttonHeight)
        {
            ButtonGiz filterButton = new ButtonGiz();
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

        public string HeaderText
        {
            get { return _gbox.Text; }
            set { _gbox.Text = value; }
        }

        private void FireFilterEvent()
        {
            if (Filter != null)
            {
                Filter(this, new EventArgs());
            }
        }

        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ITextBox textBox = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);
            return textBox;
        }

        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options,
                                                 bool strictMatch)
        {
            IComboBox comboBox =
                _filterControlManager.AddStringFilterComboBox(labelText, columnName, options, strictMatch);
            comboBox.Height = new TextBox().Height;
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
        public IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue, FilterClauseOperator filterClauseOperator, bool nullable)
        {
            IDateTimePicker dtPicker = _filterControlManager.AddDateFilterDateTimePicker(propertyName, defaultValue, filterClauseOperator, nullable);
            return dtPicker;
        }

        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        public IButton FilterButton
        {
            get { return _filterButton; }
        }

        public IButton ClearButton
        {
            get { return _clearButton; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        public void ClearFilters()
        {
            _filterControlManager.ClearFilters();
            FireFilterEvent();
        }
    }
}