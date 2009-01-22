using System;
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A filter that comprises a TextBox and filters on a string type.
    /// </summary>
    public class StringTextBoxFilter : ICustomFilter
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private ITextBox _textBox;

        public StringTextBoxFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _textBox = _controlFactory.CreateTextBox();
            _textBox.TextChanged += (sender, e) => FireValueChanged();
        }

        private void FireValueChanged()
        {
            if (ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        public IControlHabanero Control { get { return _textBox; } }
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            return _textBox.Text.Length > 0
                       ? filterClauseFactory.CreateStringFilterClause(_propertyName, _filterClauseOperator, _textBox.Text)
                       : filterClauseFactory.CreateNullFilterClause();
        }
        public void Clear() { _textBox.Text = ""; }
        public event EventHandler ValueChanged;
        public string PropertyName { get { return _propertyName; } }
        public FilterClauseOperator FilterClauseOperator { get { return _filterClauseOperator; } }
    }
}