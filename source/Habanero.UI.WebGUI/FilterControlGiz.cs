#region Using

using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;
#endregion

namespace Habanero.UI.WebGUI
{
    public class FilterControlGiz : Panel, IFilterControl
    {
        private readonly FilterControlManager _filterControlManager;
        private readonly Button _filterButton;

        public event EventHandler Filter;

        public FilterControlGiz(IControlFactory controlFactory)
        {
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(this);
            _filterControlManager = new FilterControlManager(controlFactory, flowLayoutManager);
            this.Height = 40;

            int buttonHeight = 18;
            int buttonWidth = 60;
            _filterButton = CreateFilterButton(buttonWidth, buttonHeight);

            Button clearButton = CreateClearButton(buttonWidth, buttonHeight);

            PanelGiz panel = CreateButtonPanel(clearButton);

            AddButtonsToPanel(clearButton, _filterButton, panel);

            flowLayoutManager.AddControl(panel);
            this.Controls.Add(panel);
        }

        private static void AddButtonsToPanel(Button clearButton, Button filterButton, PanelGiz panel)
        {
            panel.Controls.Add(filterButton);
            panel.Controls.Add(clearButton);
        }

        private PanelGiz CreateButtonPanel(Button clearButton)
        {
            PanelGiz panel = new PanelGiz();
            panel.BorderWidth = 0;
            panel.Padding = Padding.Empty;
            panel.Width = _filterButton.Width + 2;
            panel.Height = _filterButton.Height + clearButton.Height + 4;
            return panel;
        }

        private Button CreateClearButton(int buttonWidth, int buttonHeight)
        {
            Button clearButton = new Button();
            clearButton.Width = buttonWidth;
            clearButton.Height = buttonHeight;
            clearButton.Top = _filterButton.Height + 2;
            clearButton.Text = "Clear";
            clearButton.Click += delegate { ClearFilters(); };
            return clearButton;
        }

        private Button CreateFilterButton(int buttonWidth, int buttonHeight)
        {
            Button filterButton = new Button();
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
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="ignoreTime">Sets all times produced by the DateTimePicker
        /// to 12am before comparing dates</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue, FilterClauseOperator filterClauseOperator, bool ignoreTime, bool nullable)
        {
            IDateTimePicker dtPicker = _filterControlManager.AddDateFilterDateTimePicker(propertyName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            return dtPicker;
            //_layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            //DateTimePicker picker =
            //    _filterInputBoxCollection.AddDateFilterDateTimePicker(columnName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            //_layoutManager.AddControl(picker);
            //return picker;
        }

        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        public Button FilterButton
        {
            get { return _filterButton; }
        }

        IList IControlChilli.Controls
        {
            get { return this.Controls; }
        }

        public void ClearFilters()
        {
            _filterControlManager.ClearFilters();
            FireFilterEvent();
        }
    }
}