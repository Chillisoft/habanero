using System;
using System.Windows.Forms;
namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Represents a column in data grid view
    /// </summary>
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor to initialise a new column
        /// </summary>
        public DataGridViewNumericUpDownColumn()
            : base(new NumericUpDownCell())
        {
        }

        /// <summary>
        /// Gets and sets the cell template
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(NumericUpDownCell)))
                {
                    throw new InvalidCastException("Must be a NumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
