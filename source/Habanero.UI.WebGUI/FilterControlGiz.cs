#region Using

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
        
        public FilterControlGiz(IControlFactory controlFactory)
        {
            _filterControlManager = new FilterControlManager(controlFactory);
            this.Height = 40;
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
    }
}