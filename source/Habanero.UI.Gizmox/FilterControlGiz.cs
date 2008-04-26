#region Using

using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;

#endregion

namespace Habanero.UI.Gizmox
{
    //TODO: move to flowlayoutmanager???  investigate this.
    public partial class FilterControlGiz : FlowLayoutPanel, IFilterControl
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