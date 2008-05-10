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

            int buttonHeight = 18;
            int buttonWidth = 60;
            _filterButton = new Button();
            _filterButton.Width = buttonWidth;
            _filterButton.Height = buttonHeight;
            _filterButton.Top = 0;
            _filterButton.Text = "Filter";
            _filterButton.Click += delegate { FireFilterEvent(); };

            Button clearButton = new Button();
            clearButton.Width = buttonWidth;
            clearButton.Height = buttonHeight;
            clearButton.Top = _filterButton.Height + 2;
            clearButton.Text = "Clear";
            clearButton.Click += delegate { ClearFilters(); };

            Panel panel = new Panel();
            panel.BorderWidth = 0;
            panel.Padding = Padding.Empty;
            panel.Width = _filterButton.Width + 2;
            panel.Height = _filterButton.Height + clearButton.Height + 4;


            panel.Controls.Add(_filterButton);
            panel.Controls.Add(clearButton);

            this.Controls.Add(panel);
        
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

            this.Controls.Add(CreateLabel(labelText));
            this.Controls.Add((Control)textBox);
            return textBox;
        }

        private static Label CreateLabel(string labelText)
        {
            Label label = new Label(labelText);
            label.Width = labelText.Length*8;
            label.Height = 15;
            return label;
        }

        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch)
        {
            IComboBox comboBox =  _filterControlManager.AddStringFilterComboBox(labelText, columnName, options, strictMatch);
            comboBox.Height = new TextBox().Height;
            this.Controls.Add(CreateLabel(labelText));
            this.Controls.Add((Control)comboBox);
            return comboBox;
        }

        public ICheckBox AddStringFilterCheckBox(string labelText, string propertyName, bool defaultValue)
        {
            ICheckBox checkBox = _filterControlManager.AddStringFilterCheckBox(labelText, propertyName, defaultValue);
            //checkBox.Height = new TextBox().Height;
            this.Controls.Add(CreateLabel(labelText));
            this.Controls.Add((Control)checkBox);
            return checkBox;

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