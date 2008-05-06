using System.Collections;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class FilterControlWin : FlowLayoutPanel, IFilterControl
    {
            private readonly FilterControlManager _filterControlManager;

        public FilterControlWin(IControlFactory controlFactory)
            {
                _filterControlManager = new FilterControlManager(controlFactory);

                this.Height = 40;
            }

            public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
            {
                ITextBox textBox = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);

                Label lbl = new Label();
                lbl.Text = labelText;
                this.Controls.Add(lbl);
                this.Controls.Add((Control)textBox);
                return textBox;
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