#region Using

using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;

#endregion

namespace Habanero.UI.WebGUI
{
    //TODO: move to flowlayoutmanager???  investigate this.
    public class FilterControlGiz : FlowLayoutPanel, IFilterControl
    {
        private readonly FilterControlManager _filterControlManager;
        private readonly Button _filterButton;

        public event EventHandler Filter;
        
        public FilterControlGiz(IControlFactory controlFactory)
        {
            _filterControlManager = new FilterControlManager(controlFactory);
            this.Height = 40;

            _filterButton = new Button();
            _filterButton.Text = "Filter";
            _filterButton.Click += delegate { FireFilterEvent(); };
            this.Controls.Add(_filterButton);

            Button clearButton = new Button();
            clearButton.Text = "Clear";
            clearButton.Click += delegate { ClearFilters(); };
            this.Controls.Add(clearButton);
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

            this.Controls.Add(new Label(labelText));
            this.Controls.Add((Control)textBox);
            return textBox;
        }

        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch)
        {
            IComboBox comboBox =  _filterControlManager.AddStringFilterComboBox(labelText, columnName, options, strictMatch);
            comboBox.Height = new TextBox().Height;
            this.Controls.Add(new Label(labelText));
            this.Controls.Add((Control)comboBox);
            return comboBox;
        }

        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        ICollection IFilterControl.Controls
        {
            get { return this.Controls; }
        }

        public Button FilterButton
        {
            get { return _filterButton; }
        }

        public void ClearFilters()
        {
            _filterControlManager.ClearFilters();
            FireFilterEvent();
        }
    }
}